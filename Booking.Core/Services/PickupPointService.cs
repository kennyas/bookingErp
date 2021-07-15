using Booking.Core.Dtos;
using Booking.Core.Enums;
using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.Validators;
using Booking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public class PickupPointService : Service<Point>, IPickupPointService
    {
        private readonly IHttpUserService _currentUserService;
        public PickupPointService(IUnitOfWork unitOfWork, IHttpUserService currentUserService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
        }

        public RoutePointDto GetPoint(Guid pointId, PointType pointType, Guid routeId)
        {
            var point = SqlQuery<RoutePointDto>($"Select * from GetPointInRoute('{pointId.ToString()}',{(int)pointType},'{routeId.ToString()}')").FirstOrDefault();

            return point;
        }

        public IEnumerable<PickUpDestinationPointDto> GetPoint(Guid ppPointId, Guid dpPointId)
        {
            return SqlQuery<PickUpDestinationPointDto>($"Exec [dbo].[Sp_GetRouteWithPoints] @p0, @p1", ppPointId, dpPointId);
        }

        public async Task<ApiResponse<PickupPointViewModel>> CreatePickupPointAsync(PickupPointViewModel model)
        {
            var response = new ApiResponse<PickupPointViewModel> { Code = ApiResponseCodes.OK, Description = "" };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new CreatePickupPointVMValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.AreaId, out Guid areaId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid state ID specified");
                goto ReturnToCaller;
            }

            var existingPickupPoint = FirstOrDefault(p => string.Equals(model.Title, p?.Title, StringComparison.OrdinalIgnoreCase) && p.AreaId == areaId);

            if (existingPickupPoint != null)
            {
                response.Code = ApiResponseCodes.EXCEPTION;
                response.Errors.Add("Pick up point already exists.");
                return response;
            }

            //This should call State API and not query database for values so as to prevent bottle neck in the data access layer. A rework is possible if the State service is fully functional
            var doesAreaExist = (from area in UnitOfWork.Repository<Area>().GetAll()
                                  where area.Id == areaId
                                  select area
                                  ).Count() > 0;

            if (!doesAreaExist)
            {
                response.Code = ApiResponseCodes.EXCEPTION;
                response.Errors.Add("The specified State ID does not exist");
                return response;
            }

            FirstOrDefault(p => string.Equals(model.Title, p?.Title, StringComparison.OrdinalIgnoreCase) && p.AreaId == areaId);

            if (existingPickupPoint != null)
            {
                response.Code = ApiResponseCodes.EXCEPTION;
                response.Errors.Add("Pick up point already exists.");
                return response;
            }


            var pickupPoint = new Point
            {
                CreatedBy = _currentUserService.GetCurrentUser().UserId,
                CreatedOn = Clock.Now,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                ShortDescription = model.ShortDescription,
                AreaId = areaId,
                Title = model.Title,
                Description = model.Description
            };

            var isPickupPointCreated = await AddAsync(pickupPoint) > 0;

            response.Code = isPickupPointCreated ? ApiResponseCodes.OK : ApiResponseCodes.FAILED;
            response.Description = isPickupPointCreated ? "Successful!" : "Could not create pickup point at this time, please try again later";
            response.Payload = isPickupPointCreated ? (PickupPointViewModel)pickupPoint : null;
            response.TotalCount = isPickupPointCreated ? 1 : 0;

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<PickupPointViewModel>> DeletePickupPointAsync(DeletePickupPointViewModel model)
        {
            var response = new ApiResponse<PickupPointViewModel> { Code = ApiResponseCodes.OK, Description = "" };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new DeletePickupPointValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.Id, out Guid pickupPointId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid ID specified");
                goto ReturnToCaller;
            }

            var existingVehicleModel = await GetByIdAsync(pickupPointId);

            if (existingVehicleModel == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Pickup point does not exist or has been deleted");
                goto ReturnToCaller;
            }

            existingVehicleModel.IsDeleted = true;
            existingVehicleModel.ModifiedOn = Clock.Now;
            existingVehicleModel.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            Delete(existingVehicleModel);

            response.Code = ApiResponseCodes.OK;
            response.Description = "Successful";
            response.Payload = null;

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<PickupPointViewModel>> EditPickupPointAsync(PickupPointViewModel model)
        {
            var response = new ApiResponse<PickupPointViewModel> { Code = ApiResponseCodes.OK, Description = "" };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new EditPickupPointVMValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add(validationResult.ToString(", "));
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.AreaId, out Guid areaId) || !Guid.TryParse(model.Id, out Guid _))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid state or point Id");
                goto ReturnToCaller;
            }

            var existingData = await GetByIdAsync(Guid.Parse(model.Id));

            if (existingData == null)
            {
                response.Code = ApiResponseCodes.NOT_FOUND;
                response.Errors.Add("Pickup point does not exist.");
                goto ReturnToCaller;
            }

            existingData.ModifiedOn = Clock.Now;
            existingData.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            existingData.Latitude = model.Latitude;
            existingData.Longitude = model.Longitude;
            existingData.ShortDescription = model.ShortDescription;
            existingData.Description = model.Description;
            existingData.Title = model.Title;
            existingData.AreaId = areaId;

            var isVehicleUpdated = await UpdateAsync(existingData) > 0;

            response.Code = isVehicleUpdated ? ApiResponseCodes.OK : ApiResponseCodes.FAILED;
            response.Description = isVehicleUpdated ? "Successful" : $"Could not update pickup point at this time";
            response.Payload = isVehicleUpdated ? (PickupPointViewModel)existingData : null;

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<PaginatedList<PickupPointViewModel>>> GetAllPickupPointsAsync(PickupPointPaginatedViewModel model)
        {
            var response = new ApiResponse<PaginatedList<PickupPointViewModel>>
            {
                Code = ApiResponseCodes.OK,
                Description = ""
            };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            model.PageIndex ??= 0;
            model.PageTotal ??= 50;

            var validationResult = new GetAllPickupPointValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }


            var data = await Task.Run(() =>
            {
                return (from dataModel in UnitOfWork.Repository<Point>().GetAll()
                        where dataModel.IsDeleted == false
                        select new PickupPointViewModel
                        {
                            CreatedId = dataModel.CreatedBy ?? Guid.Empty,
                            Description = dataModel.Description,
                            Id = dataModel.Id.ToString(),
                            ShortDescription = dataModel.ShortDescription,
                            Title = dataModel.Title,
                            AreaId = dataModel.AreaId.ToString(),
                            Latitude = dataModel.Latitude,
                            Longitude = dataModel.Longitude
                        });
            });

            response.Payload = data.ToPaginatedList(model.PageIndex.Value, model.PageTotal ?? 50);
            response.TotalCount = response.Payload.TotalCount;
            response.PayloadMetaData = new PayloadMetaData(
                   pageIndex: response.Payload.PageIndex,
                   pageSize: response.Payload.PageSize,
                   totalPageCount: response.Payload.TotalPageCount,
                   totalCount: response.Payload.TotalCount
                   );
            response.Code = response.TotalCount == 0 ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK;
            response.Description = response.TotalCount > 0 ? "Successful" : "No pickup point was found";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<PaginatedList<PickupPointViewModel>>> GetPickupPointAsync(GetPickupPointByIdViewModel model)
        {
            var response = new ApiResponse<PaginatedList<PickupPointViewModel>> { Code = ApiResponseCodes.OK, };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new GetPickupPointByIdValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;

                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.Id, out Guid pickupPointId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid pickup point id");

                goto ReturnToCaller;
            }

            var data = await Task.Run(() =>
            {
                return (from dataModel in UnitOfWork.Repository<Point>().GetAll()
                        where dataModel.Id == pickupPointId && dataModel.IsDeleted == false
                        select new PickupPointViewModel
                        {
                            CreatedId = dataModel.CreatedBy ?? Guid.Empty,
                            Description = dataModel.Description,
                            Id = dataModel.Id.ToString(),
                            ShortDescription = dataModel.ShortDescription,
                            Title = dataModel.Title,
                            AreaId = dataModel.AreaId.ToString(),
                            Latitude = dataModel.Latitude,
                            Longitude = dataModel.Longitude
                        });
            });

            response.Payload = data.ToPaginatedList(model.PageIndex.Value, model.PageTotal ?? 50);
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

        public async Task<ApiResponse<PaginatedList<PickupPointViewModel>>> GetPickupPointsByTitleAsync(SearchPickupPointViewModel model)
        {
            var response = new ApiResponse<PaginatedList<PickupPointViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }


            if (string.IsNullOrEmpty(model.Keyword))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Search Keyword can not be empty");

                goto ReturnToCaller;
            }


            var validationResult = new SearchPickupPointValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            var data = await Task.Run(() =>
            {
                return (from dataModel in UnitOfWork.Repository<Point>().GetAll()
                        where dataModel.Title.Trim().ToLower().Contains(model.Keyword.Trim().ToLower()) && dataModel.IsDeleted == false
                        select new PickupPointViewModel
                        {
                            CreatedId = dataModel.CreatedBy ?? Guid.Empty,
                            Description = dataModel.Description,
                            Id = dataModel.Id.ToString(),
                            ShortDescription = dataModel.ShortDescription,
                            Title = dataModel.Title,
                            AreaId = dataModel.AreaId.ToString(),
                            Latitude = dataModel.Latitude,
                            Longitude = dataModel.Longitude
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

        public async Task<ApiResponse<PaginatedList<PickupPointViewModel>>> GetPickupPointsByStateAsync(SearchPickupPointViewModel model)
        {
            var response = new ApiResponse<PaginatedList<PickupPointViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new GetPickupPointByStateValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();

                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.Keyword, out Guid stateId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid pickup point id");

                goto ReturnToCaller;
            }

            var data = await Task.Run(() =>
            {
                return (from dataModel in UnitOfWork.Repository<Point>().GetAll()
                        where dataModel.AreaId == stateId && dataModel.IsDeleted == false
                        select new PickupPointViewModel
                        {
                            CreatedId = dataModel.CreatedBy ?? Guid.Empty,
                            Description = dataModel.Description,
                            Id = dataModel.Id.ToString(),
                            ShortDescription = dataModel.ShortDescription,
                            Title = dataModel.Title,
                            AreaId = dataModel.AreaId.ToString(),
                            Latitude = dataModel.Latitude,
                            Longitude = dataModel.Longitude
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

        public async Task<ApiResponse<PaginatedList<ListPickupPointViewModel>>> GetDesitinationPickupPointsByDepartureAsync(GetRoutePickUpPointByDepartureId model)
        {

            var response = new ApiResponse<PaginatedList<ListPickupPointViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new GetRoutePickUpPointByDepartureVMValidator().Validate(model);

            int pageStart = model.PageIndex ?? 0;
            int pageEnd = model.PageSize ?? 50;

            if (!Guid.TryParse(model.DeparturePickupPointId, out Guid departurePickupPointId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid pickup point id");

                goto ReturnToCaller;
            }
            var allPickupPoints = await Task.FromResult( SqlQuery<ListPickupPointViewModel>("[dbo].[Sp_GetPointsByDeparture] @p0, @p1, @p2",
                                                     departurePickupPointId, pageStart, pageEnd).ToList());



            response = new ApiResponse<PaginatedList<ListPickupPointViewModel>>(message: (allPickupPoints != null && allPickupPoints.Any()) ? "Successful" : "No country record found",
                                                                                           codes: ApiResponseCodes.OK,
                                                                                           totalCount: allPickupPoints != null && allPickupPoints.Any() ? allPickupPoints.FirstOrDefault().TotalCount : 0,
                                                                                           data: allPickupPoints?.ToPaginatedList( pageStart, pageEnd,  response.TotalCount));
            ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<PaginatedList<ListPickupPointViewModel>>> GetDeparturePickupPointsByDestinationAsync(GetDeparturePickUpPointByDestinationId model)
        {
            var response = new ApiResponse<PaginatedList<ListPickupPointViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new GetRoutePickUpPointByDestinationVMValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();

                goto ReturnToCaller;
            }

            int pageStart = model.PageIndex ?? 0;
            int pageEnd = model.PageSize ?? 50;
            if (!Guid.TryParse(model.DestinationPickupPointId, out Guid destinationPickupPoint))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid pickup point id");
                goto ReturnToCaller;
            }
            var allPickupPoints = await Task.FromResult(SqlQuery<ListPickupPointViewModel>(SprocRunSetup.GetFileContentWithName("GetPointsByDestination.sql"),
                                           destinationPickupPoint, pageStart, pageEnd).ToList());

            response = new ApiResponse<PaginatedList<ListPickupPointViewModel>>(message: (allPickupPoints != null && allPickupPoints.Any()) ? "Successful" : "No country record found",
                                                                                        codes: ApiResponseCodes.OK,
                                                                                        totalCount: allPickupPoints != null && allPickupPoints.Any() ? allPickupPoints.FirstOrDefault().TotalCount : 0,
                                                                                        data: allPickupPoints?.ToPaginatedList(pageStart, pageEnd, response.TotalCount));


            ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<PaginatedList<ListPickupPointViewModel>>> GetDeparturePickupPointsAsync(GetDeparturePickUpPoints model)
        {
            var response = new ApiResponse<PaginatedList<ListPickupPointViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new GetRoutePickUpPointsVMValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();

                goto ReturnToCaller;
            }
            int pageStart = model.PageIndex ?? 0;
            int pageEnd = model.PageSize ?? 50;
            //if (!Guid.TryParse(model.StateId.Value, out Guid? stateId))
            //{
            //    response.Code = ApiResponseCodes.INVALID_REQUEST;
            //    response.Errors.Add("Invalid pickup point id");
            //    goto ReturnToCaller;
            //}
            var allPickupPoints = await Task.FromResult(SqlQuery<ListPickupPointViewModel>(SprocRunSetup.GetFileContentWithName("GetDeparturePickupPoints.sql"),
                                           model.StateId, pageStart, pageEnd).ToList());

            response = new ApiResponse<PaginatedList<ListPickupPointViewModel>>(message: (allPickupPoints != null && allPickupPoints.Any()) ? "Successful" : "No country record found",
                                                                                        codes: ApiResponseCodes.OK,
                                                                                        totalCount: allPickupPoints != null && allPickupPoints.Any() ? allPickupPoints.FirstOrDefault().TotalCount : 0,
                                                                                        data: allPickupPoints?.ToPaginatedList(pageStart, pageEnd, response.TotalCount));


        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<PaginatedList<ListPickupPointViewModel>>> GetOrderedDeparturePickupPointsAsync(GetOrderedDeparturePickUpPoints model)
        {
            var response = new ApiResponse<PaginatedList<ListPickupPointViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new GetOrderedDeparturePickUpPointsVMValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();

                goto ReturnToCaller;
            }    

            int pageStart = model.PageIndex ?? 0;
            int pageEnd = model.PageSize ?? 50;
            //if (!Guid.TryParse(model.StateId.Value, out Guid? stateId))
            //{
            //    response.Code = ApiResponseCodes.INVALID_REQUEST;
            //    response.Errors.Add("Invalid pickup point id");
            //    goto ReturnToCaller;
            //}

            var query = @"

                                        DECLARE @g geography = 'POINT(' + cast({4} as nvarchar) + ' ' + cast({3} as nvarchar) + ')';

                                        SELECT distinct p.*, 
                                               cast('POINT(' + cast(Longitude as nvarchar) + ' ' + cast(Latitude as nvarchar) + ')' as geography).STDistance(@g) As Cordinate
                                        FROM RoutePoint rp join Point p on rp.PointId = p.Id join Area area on p.AreaId = area.Id
                                        where (rp.PointType = 1 or rp.PointType = 2)
                                        and ({0} is null or (({0} like '%' + p.Title + '%') or ({0} like '%' + area.title + '%')))
                                         and p.IsDeleted = 0
                                        ORDER BY Cordinate ASC
                                         OFFSET ({1} * {2}) ROWS FETCH NEXT {2} ROWS ONLY";

            var allPickupPoints = await Task.FromResult(SqlQuery<ListPickupPointViewModel>(query,
                                           model.Keyword, pageStart, pageEnd, model.Latitude, model.Longitude).ToList());


            response = new ApiResponse<PaginatedList<ListPickupPointViewModel>>(message: (allPickupPoints != null && allPickupPoints.Any()) ? "Successful" : "No country record found",
                                                                                        codes: ApiResponseCodes.OK,
                                                                                        totalCount: allPickupPoints != null && allPickupPoints.Any() ? allPickupPoints.FirstOrDefault().TotalCount : 0,
                                                                                        data: allPickupPoints?.ToPaginatedList(pageStart, pageEnd, response.TotalCount));


        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }
    }
}
