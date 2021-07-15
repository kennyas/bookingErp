using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using System.Linq;
using Tornado.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.Timing;
using System.ComponentModel;
using NPOI.SS.Formula.Functions;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.Formatters;
using Tornado.Shared.Enums;

namespace Booking.Core.Services
{
    public class SubRouteFeeService : Service<SubRouteFee>, ISubRouteFeeService
    {
        private readonly IHttpUserService _currentUserService;

        public SubRouteFeeService(IUnitOfWork unitOfWork, IHttpUserService currentUserService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
        }
        public Task<List<SubRouteFeeViewModel>> GetAllSubRouteFees(SubRouteFeeRequestViewModel model, out int totalCount)
        {
            var routes = UnitOfWork.Repository<Route>();
            var routeFees = UnitOfWork.Repository<SubRouteFee>();
            var departurePoints = UnitOfWork.Repository<RoutePoint>();
            var destinationPoints = UnitOfWork.Repository<RoutePoint>();
            var vehicleModels = UnitOfWork.Repository<VehicleModel>();


            var points = UnitOfWork.Repository<Point>().GetAll();



            var query =
                                 from routeFee in routeFees.GetAll()

                                 join departureRoutePoint in departurePoints.GetAll()
                                 on routeFee.DeparturePointId equals departureRoutePoint.Id

                                 //join destinationRoutePoint in destinationPoints.GetAll()
                                 //on routeFee.DestinationPointId equals destinationRoutePoint.Id

                                 join vehicleModel in vehicleModels.GetAll()
                                 on routeFee.VehicleModelId equals vehicleModel.Id

                                 let departurePoint = points.FirstOrDefault(p => p.Id
                                    == departureRoutePoint.PointId)

                                 //let destinationPoint = points.FirstOrDefault(p => p.Id
                                 //        == destinationRoutePoint.PointId)

                                 where //(destinationRoutePoint.RouteId == model.RouteId) && 
                                    (departureRoutePoint.RouteId == model.RouteId)
                                    && ((model.VehicleModelId == null) 
                                    || routeFee.VehicleModelId == model.VehicleModelId)
                                 //include others

                                 select new SubRouteFeeViewModel
                                 {
                                     DeparturePointId = departurePoint.Id,
                                     DeparturePointName = departurePoint.Title,
                                     DepartureRoutePointId = departureRoutePoint.Id,
                                     RouteId = model.RouteId,
                                     VehicleModelId = vehicleModel.Id,
                                     VehicleModelTitle = vehicleModel.Title,
                                     Id = routeFee.Id,
                                     Fare = routeFee.Fare
                                 };
            totalCount = query.Count();

            return query.ToListAsync();

        }


        public async Task<List<ValidationResult>> CreateSubRouteFee(CreateSubRouteFeeRequestViewModel model)
        {
            var existingFare = FirstOrDefault(subRoute => subRoute.DeparturePointId == model.DepartureRoutePointId &&
                                   subRoute.VehicleModelId == model.VehicleModelId);

            if (existingFare != null)
            {
                results.Add(new ValidationResult("Fare already exists"));
                goto finish;
            }

            var departurePoint = UnitOfWork.Repository<RoutePoint>().GetFirstOrDefault(p => p.RouteId == model.RouteId
                                            && p.Id == model.DepartureRoutePointId);

            if (departurePoint == null)
            {
                results.Add(new ValidationResult("Departure point does not exists"));
                goto finish;
            }

           
            var subrouteFare = new SubRouteFee
            {
                CreatedBy = _currentUserService.GetCurrentUser().UserId,
                CreatedOn = Clock.Now,
                DeparturePointId = model.DepartureRoutePointId,
                Fare = model.Fare,
                VehicleModelId = model.VehicleModelId
            };

            var addedEntity
                = await AddAsync(subrouteFare);

            if (addedEntity <= 0)
                results.Add(new ValidationResult("Could not add sub route"));


            finish:
            return results;
        }

        public async Task<List<ValidationResult>> DeleteSubRouteFee(string id)
        {
            if (Guid.TryParse(id, out Guid subrouteFeeId))
            {
                results.Add(new ValidationResult("Bad request"));
                goto finish;
            }

            var existingSubRouteFee = await GetByIdAsync(subrouteFeeId);

            if (existingSubRouteFee == null)
            {
                results.Add(new ValidationResult("Route does not exist"));
                goto finish;
            }

            existingSubRouteFee.DeletedBy = _currentUserService.GetCurrentUser().UserId;
            existingSubRouteFee.DeletedOn = Clock.Now;
            existingSubRouteFee.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            existingSubRouteFee.ModifiedOn = Clock.Now;

            await DeleteAsync(existingSubRouteFee);

        finish:
            return results;
        }

        public async Task<List<ValidationResult>> EditSubRouteFee(EditSubRouteFeeRequestViewModel model)
        {
            var existingFare = FirstOrDefault(
                                    subRoute =>
                                    Equals(subRoute.DeparturePointId, model.DepartureRoutePointId)
                                    && Equals(subRoute.VehicleModelId, model.VehicleModelId)
                                    && !Equals(subRoute.Id, model.Id));

            if (existingFare != null)
            {
                results.Add(new ValidationResult("Fare to be edited to already exists"));
                goto finish;
            }

            var fare = await GetByIdAsync(model.Id);

            //just to verify if it belongs to the same route
            var departurePoint = UnitOfWork.Repository<RoutePoint>()
                                            .GetFirstOrDefault(p => p.RouteId == model.RouteId
                                            && p.Id == model.DepartureRoutePointId);

            if (departurePoint == null)
            {
                results.Add(new ValidationResult("Departure point does not exists"));
                goto finish;
            }
            //just to verify if it belongs to the same route
           

            
            fare.ModifiedOn = Clock.Now;
            fare.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            fare.Fare = model.Fare;
            fare.DeparturePointId = model.DepartureRoutePointId;

            await UpdateAsync(fare);

        finish:
            return results;
        }

        public async Task<ApiResponse<SubRouteFeeViewModel>> GetSubRouteFee(string id)
        {
            var response = new ApiResponse<SubRouteFeeViewModel> { };

            if (!Guid.TryParse(id, out Guid guidId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid Request";

                goto finish;
            }

            var existingFee = await GetByIdAsync(guidId);

            if (existingFee == null)
            {
                response.Code = ApiResponseCodes.NOT_FOUND;
                response.Description = "Request not found";
                goto finish;
            }
            //replace with linq
            var model = new SubRouteFeeViewModel
            {
                DepartureRoutePointId = existingFee?.DeparturePointId,
                Id = existingFee.Id,
                Fare = existingFee.Fare,
                VehicleModelTitle = existingFee?.VehicleModel?.Title
            };

            response.Payload = model;
            response.Code = ApiResponseCodes.OK;

        finish:
            return response;
        }
    }
}
