using Booking.Core.Dtos;
using Booking.Core.Enums;
using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.Validators;
using Booking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.Timing;
using Tornado.Shared.ViewModels;

namespace Booking.Core.Services
{
    public class RoutePickupPointService : Service<RoutePoint>, IRoutePickupPointService
    {
        private readonly IHttpUserService _currentUserService;


        public RoutePickupPointService(IUnitOfWork unitOfWork, IHttpUserService currentUserService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<RoutePickupPointViewModel>> AddPickupPointToRoute(CreateRoutePickupPointViewModel model)
        {
            var response = new ApiResponse<RoutePickupPointViewModel> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new CreateRoutePickupPointVMValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            bool isRouteIdValid = UnitOfWork.Repository<Route>().GetById(model.RouteId) != null;

            if (!isRouteIdValid)
            {
                response.Errors.Add("Invalid route id was specified");
                response.Code = ApiResponseCodes.FAILED;

                goto ReturnToCaller;
            }

            var selectedPickupPoint = UnitOfWork.Repository<Point>().GetById(model.PickupPointId);
            bool isPickupPointIdValid = selectedPickupPoint != null;

            if (!isPickupPointIdValid)
            {
                response.Errors.Add("Invalid pickup point id was specified");
                response.Code = ApiResponseCodes.FAILED;

                goto ReturnToCaller;
            }

            var routePickpt = FirstOrDefault(pt => Equals(pt.PointId, model.PickupPointId) && Equals(pt.RouteId, model.RouteId));
            if (routePickpt != null)
            {
                return new ApiResponse<RoutePickupPointViewModel>(errors: "Pickup point already exists for that route", codes: ApiResponseCodes.INVALID_REQUEST);
            }

            switch (model.PickupPointType)
            {
                case PointType.Departure:
                    var depPick = FirstOrDefault(p => Equals(p.RouteId, model.RouteId) && p.OrderNo <= model.OrderNo);

                    if (depPick == null)
                        break;

                    response.Errors.Add("Departure pick up point should have the lowest order");
                    response.Code = ApiResponseCodes.FAILED;
                    goto ReturnToCaller;

                case PointType.Destination:
                    var destPick = FirstOrDefault(p => Equals(p.RouteId, model.RouteId) && p.OrderNo >= model.OrderNo);

                    if (destPick == null) break;

                    response.Errors.Add("Destination pick up point should have the highest order");
                    response.Code = ApiResponseCodes.FAILED;
                    goto ReturnToCaller;
                default:
                    break;
            }
            var routePickupPoint = new RoutePoint
            {
                PointId = model.PickupPointId,
                RouteId = model.RouteId,
                PointType = model.PickupPointType,
                OrderNo = model.OrderNo,
                CreatedBy = _currentUserService.GetCurrentUser().UserId,
                CreatedOn = Clock.Now,
            };
            var addedEntity = await AddAsync(routePickupPoint);

            //to do... update all other base pick up points to false if they

            response = addedEntity > 0 ? new ApiResponse<RoutePickupPointViewModel>(data: new RoutePickupPointViewModel
            {
                PickupPointId = model.PickupPointId,
                RouteId = model.RouteId,
                PickupPointType = model.PickupPointType,
                OrderNo = model.OrderNo,
                CreatedId = routePickupPoint.CreatedBy.Value,
                Title = selectedPickupPoint.Title,
                Description = selectedPickupPoint.Description,
                Latitude = selectedPickupPoint.Latitude,
                Longitude = selectedPickupPoint.Longitude,
                ShortDescription = selectedPickupPoint.ShortDescription,
                AreaId = selectedPickupPoint.AreaId,
                Id = routePickupPoint.Id.ToString()
            }
            , codes: ApiResponseCodes.OK, message: "Successful")
                                    : new ApiResponse<RoutePickupPointViewModel>(codes: ApiResponseCodes.INVALID_REQUEST, errors: "Could not add entry");
        ReturnToCaller:
            return response;
        }

        public async Task<ApiResponse<RoutePickupPointViewModel>> RemovePickupPointToRoute(DeleteRoutePickupPointsById model)
        {
            var response = new ApiResponse<RoutePickupPointViewModel> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new DeleteRoutePickupPointVMValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.PickupPointId, out Guid pickupPointId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid pickup point was specified");
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.RouteId, out Guid routeId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid route id was specified");
                goto ReturnToCaller;
            }

            var existingRoute = await Task.Run(() =>
          
                UnitOfWork.Repository<RoutePoint>().GetFirstOrDefault(routePoint => routePoint.PointId == pickupPointId && routePoint.RouteId == routeId && !routePoint.IsDeleted)
            );

            if (existingRoute == null)
            {
                response.Code = ApiResponseCodes.NOT_FOUND;
                response.Errors.Add("Could not find the specified pick up point/ route information");
                goto ReturnToCaller;
            }

            existingRoute.IsDeleted = true;
            existingRoute.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            existingRoute.ModifiedOn = Clock.Now;

            Delete(existingRoute);

            response.Code = ApiResponseCodes.OK;
            response.Description = "Success";
            response.Code = ApiResponseCodes.OK;

        ReturnToCaller:
            return response;
        }

        public async Task<ApiResponse<RoutePickupPointViewModel>> RemoveAllRoutePickupPoints(DeleteAllRoutePickupPointsById model)
        {
            var response = new ApiResponse<RoutePickupPointViewModel>(codes: ApiResponseCodes.OK);

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new DeleteAllRoutePickupPointVMValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.RouteId, out Guid currentRouteId))
            {
                response.Errors.Add($"You have specified an invalid Route Id");
                goto ReturnToCaller;
            }

            var routePickupPoints = GetAll().Where(p => p.RouteId == currentRouteId).ToList();

            await Task.Run(() =>
            {
                routePickupPoints.ForEach(async point =>
                {
                    var response = await RemovePickupPointToRoute( new DeleteRoutePickupPointsById{
                      RouteId =  point.RouteId.ToString(),
                      PickupPointId = model.RouteId
                    });

                    if (response.Code != ApiResponseCodes.OK)
                    {
                        response.Errors.Add($"Could not delete pick up entity {point.Point?.Title} from {point.Route?.Name}");
                    }
                });
            });

        ReturnToCaller:
            return response;
        }

        public async Task<ApiResponse<PaginatedList<RoutePickupPointViewModel>>> GetRoutePickupPoints(GetRoutePickupPoints model)
        {
            var response = new ApiResponse<PaginatedList<RoutePickupPointViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Id can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new GetRoutePickupPointsVMValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.RouteId, out Guid routeId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid pickup point id");

                goto ReturnToCaller;
            }

            var data = await Task.Run(() =>
            {
                return (from dataModel in UnitOfWork.Repository<RoutePoint>().GetAll()
                        join pickupPoint in UnitOfWork.Repository<Point>().GetAll() on dataModel.PointId equals pickupPoint.Id
                        where dataModel.RouteId == routeId && dataModel.IsDeleted == false
                        select new RoutePickupPointViewModel
                        {
                            CreatedId = dataModel.CreatedBy ?? Guid.Empty,
                            Id = dataModel.Id.ToString(),
                            RouteId = dataModel.RouteId,
                            OrderNo = dataModel.OrderNo,
                            PickupPointType = dataModel.PointType,
                            Latitude = pickupPoint.Latitude,
                            Longitude = pickupPoint.Longitude,
                            Title = pickupPoint.Title,
                            ShortDescription = pickupPoint.ShortDescription,
                            PickupPointId = pickupPoint.Id,
                            Description = pickupPoint.Description,
                            AreaId = pickupPoint.AreaId
                        });
            });

            response.Payload = data.ToPaginatedList(model.PageIndex.Value, model.PageTotal.Value);
            response.TotalCount = response.Payload.TotalCount;
            response.PayloadMetaData = new PayloadMetaData(
                   pageIndex: response.Payload.PageIndex,
                   pageSize: response.Payload.PageSize,
                   totalPageCount: response.Payload.TotalPageCount,
                   totalCount: response.Payload.TotalCount);
            response.Code = response.TotalCount == 0 ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK;
            response.Description = response.TotalCount > 0 ? "Successful" : "No pickup point was found";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

    }
}
