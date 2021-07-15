using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.EF;
using Microsoft.EntityFrameworkCore;
using Tornado.Shared.EF.Services;
using System.Linq;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.Timing;
using Tornado.Shared.ViewModels;
using Tornado.Shared.Enums;

namespace Booking.Core.Services
{
    public class VehicleModelRouteFeeService : Service<VehicleModelRouteFee>, IVehicleModelRouteFeeService
    {
        private readonly IHttpUserService _currentUserService;

        public VehicleModelRouteFeeService(IHttpUserService currentUserService,IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
        }

        public async Task<List<ValidationResult>> CreateVehicleModelRouteFee(CreateVehicleModelRouteFeeViewModel model)
        {
            var existingFare = FirstOrDefault(routeFee => Equals(routeFee.RouteId, model.RouteId)
                                    && Equals(routeFee.VehicleModelId, model.VehicleModelId));

            if (existingFare != null)
            {
                results.Add(new ValidationResult("Fare already exists"));
                goto finish;
            }

            var route = await UnitOfWork.Repository<Route>().GetByIdAsync(model.RouteId);

            if (route == null)
            {
                results.Add(new ValidationResult("route does not exists"));
                goto finish;
            }
            var vehicleModel = await UnitOfWork.Repository<VehicleModel>().GetByIdAsync(model.VehicleModelId);

            if (vehicleModel == null)
            {
                results.Add(new ValidationResult("Vehicle model point does not exists"));
                goto finish;
            }


           
            var subrouteFare = new VehicleModelRouteFee
            {
                CreatedBy = _currentUserService.GetCurrentUser().UserId,
                CreatedOn = Clock.Now,
                RouteId = model.RouteId,
                BaseFee = model.BaseFare,
                VehicleModelId = model.VehicleModelId
            };

            var addedEntity
                = await AddAsync(subrouteFare);

            if (addedEntity <= 0)
                results.Add(new ValidationResult("Could not add sub route"));


            finish:
            return results;

        }

        public async Task<List<ValidationResult>> DeleteVehicleModelRouteFee(string id)
        {
            if (Guid.TryParse(id, out Guid subrouteFeeId))
            {
                results.Add(new ValidationResult("Bad request"));
                goto finish;
            }

            var existingVModelFee = await GetByIdAsync(subrouteFeeId);

            if (existingVModelFee == null)
            {
                results.Add(new ValidationResult("Route does not exist"));
                goto finish;
            }

            existingVModelFee.DeletedBy = _currentUserService.GetCurrentUser().UserId;
            existingVModelFee.DeletedOn = Clock.Now;
            existingVModelFee.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            existingVModelFee.ModifiedOn = Clock.Now;

            Delete(existingVModelFee);

        finish:
            return results;
        }

        public async Task<List<ValidationResult>> EditVehicleModelRouteFee(EditVehicleModelRouteFeeViewModel model)
        {
            var existingFare = FirstOrDefault(
                        subRoute => Equals(subRoute.Id, model.Id)
                        && Equals(subRoute.VehicleModelId, model.VehicleModelId)
                        && !Equals(subRoute.Id, model.Id));

            if (existingFare != null)
            {
                results.Add(new ValidationResult("Fare to be edited to already exists"));
                goto finish;
            }

            var fare = await GetByIdAsync(model.Id);

            //just to verify if it belongs to the same route

            var route = await UnitOfWork.Repository<Route>().GetByIdAsync(model.RouteId);

            if (route == null)
            {
                results.Add(new ValidationResult("route does not exists"));
                goto finish;
            }
            var vehicleModel = await UnitOfWork.Repository<VehicleModel>().GetByIdAsync(model.VehicleModelId);

            if (vehicleModel == null)
            {
                results.Add(new ValidationResult("Vehicle model point does not exists"));
                goto finish;
            }


            fare.ModifiedOn = Clock.Now;
            fare.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            fare.BaseFee = model.BaseFare;
            fare.VehicleModelId = model.VehicleModelId;
            fare.RouteId = model.RouteId;


            await UpdateAsync(fare);

        finish:
            return results;
        }

        public  Task<List<VehicleModelRouteFeeViewModel>> GetAllVehicleModelRouteFees(VehicleModelRouteFeeRequestViewModel model, out int totalCount)
        {
            var routes = UnitOfWork.Repository<Route>();
            var routeFees = UnitOfWork.Repository<VehicleModelRouteFee>();
            var vehicleModels = UnitOfWork.Repository<VehicleModel>();


            var points = UnitOfWork.Repository<Point>().GetAll();



            var query =          from routeFee in routeFees.GetAll()

                                 join route in routes.GetAll()
                                 on routeFee.RouteId equals route.Id

                                 join vehicleModel in vehicleModels.GetAll()
                                 on routeFee.VehicleModelId equals vehicleModel.Id

                                 where ((model.RouteId == null ) || route.Id == model.RouteId)
                                    && ((model.VehicleModelId == null) || routeFee.VehicleModelId == model.VehicleModelId)
                                 //include others

                                 select new VehicleModelRouteFeeViewModel
                                 {
                                     RouteId = model.RouteId,
                                     VehicleModelId = vehicleModel.Id,
                                     VehicleModelTitle = vehicleModel.Title,
                                     Id = routeFee.Id,
                                     BaseFare = routeFee.BaseFee
                                 };
            totalCount = query.Count();

            return  query.ToListAsync();

        }

        public async Task<ApiResponse<VehicleModelRouteFeeViewModel>> GetVehicleModelRouteFee(string id)
        {
            var response = new ApiResponse<VehicleModelRouteFeeViewModel> { };

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
            var model = new VehicleModelRouteFeeViewModel
            {
                BaseFare = existingFee.BaseFee,
                Id = existingFee.Id,
                RouteId = existingFee.RouteId,
                VehicleModelTitle = existingFee?.VehicleModel.Title
            };

            response.Payload = model;
            response.Code = ApiResponseCodes.OK;

        finish:
            return response;
        }
    }
}
