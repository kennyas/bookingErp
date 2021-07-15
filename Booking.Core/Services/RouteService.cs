using Booking.Core.Dtos;
using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using System;
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
    public class RouteService : Service<Route>, IRouteService
    {
        private readonly IHttpUserService _currentUserService;
        public RouteService(IUnitOfWork unitOfWork, IHttpUserService currentUserService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<CreateRouteViewModel>> AddRoute(CreateRouteViewModel model)
        {
            if (model.DeparturePickupPointId == model.DestinationPickupPointId)
            {
                return new ApiResponse<CreateRouteViewModel>
                    (errors: "Departure terminal and Destination terminal cannot be the same.", 
                     codes: ApiResponseCodes.INVALID_REQUEST, message: "Unsuccessful!");
            }

            (var departure, var destination) = (UnitOfWork.Repository<Point>().GetFirstOrDefault(p => p.Id == model.DeparturePickupPointId),
                UnitOfWork.Repository<Point>().GetFirstOrDefault(p => p.Id == model.DestinationPickupPointId));

            if (departure == null || destination == null)
            {
                return new ApiResponse<CreateRouteViewModel>(errors: "Could not find destination or departure terminal.", codes: ApiResponseCodes.INVALID_REQUEST, message: "Unsuccessful!");
            }
            var conflictingRoute = FirstOrDefault(p => string.Equals(p.Name, model.Name));
            if (conflictingRoute != null)
            {
                return new ApiResponse<CreateRouteViewModel>(errors: "Route already exists", codes: ApiResponseCodes.FAILED, message: "Unsuccessful!");
            }
            var route = new Route
            {
                CreatedOn = Clock.Now,
                CreatedBy = _currentUserService.GetCurrentUser().UserId,
                Description = model.Description,
                ShortDescription = model.ShortDescription,
                Name = RouteName(departure.Title, destination.Title),
            };
            var addedRoute = await AddAsync(route);


            model.Id = route.Id.ToString();
            model.Name = route.Name;
            model.CreatedId = _currentUserService.GetCurrentUser().UserId;


            return
             addedRoute <= 0 ? 
                 new ApiResponse<CreateRouteViewModel>(errors: "Could not add route", codes: ApiResponseCodes.FAILED, message: "Unsuccessful!") :
                 new ApiResponse<CreateRouteViewModel>(message: "Successful", codes: ApiResponseCodes.OK, data: model);
 

        }

        public async Task<ApiResponse<RouteViewModel>> EditRoute(EditRouteViewModel model)
        {

            if (model == null)
            {
#if DEBUG
                return new ApiResponse<RouteViewModel>(errors: "model cannot be null or empty", codes: ApiResponseCodes.INVALID_REQUEST, message: "Unsuccessful!");
#else
                return new ApiResponse<RouteViewModel>(errors: "invalid request", codes: ApiResponseCodes.INVALID_REQUEST, message: "Unsuccessful!");
#endif
            }
            var guidId = Guid.Parse(model.Id);

            var existingRoute = await GetByIdAsync(guidId);
            if (existingRoute == null)
            {
                return new ApiResponse<RouteViewModel>(errors: "Departure terminal and Destination terminal cannot be the same.", codes: ApiResponseCodes.INVALID_REQUEST, message: "Unsuccessful!");
            }

            if (model.DeparturePickupPointId == model.DestinationPickupPointId)
            {
                return new ApiResponse<RouteViewModel>(errors: "Departure terminal and Destination terminal cannot be the same.", codes: ApiResponseCodes.INVALID_REQUEST, message: "Unsuccessful!");
            }

            (string departure, string destination) = (UnitOfWork.Repository<Point>().GetFirstOrDefault(p => p.Id == model.DeparturePickupPointId)?.Title,
               UnitOfWork.Repository<Point>().GetFirstOrDefault(p => p.Id == model.DestinationPickupPointId)?.Title);


            if (string.IsNullOrEmpty(destination) || string.IsNullOrEmpty(departure))
            {
                return new ApiResponse<RouteViewModel>(errors: "Could not find destination or departure terminal.", codes: ApiResponseCodes.NOT_FOUND, message: "Unsuccessful!");
            }
            model.Name = RouteName(departure, destination);

            var conflictingRoute = FirstOrDefault(p => string.Equals(p.Name, model.Name) && !Equals(p.Id, guidId));


            if (conflictingRoute != null)
            {
                return new ApiResponse<RouteViewModel>(errors: "Route already exists", codes: ApiResponseCodes.FAILED, message: "Unsuccessful!");
            }

            existingRoute.ModifiedOn = Clock.Now;
            existingRoute.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            existingRoute.Description = model.Description;
            existingRoute.ShortDescription = model.ShortDescription;
            existingRoute.Name = model.Name;
       
            var updatedEntity = await UpdateAsync(existingRoute);

            model.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            model.CreatedId = existingRoute.CreatedBy.Value;

            return new ApiResponse<RouteViewModel>(codes: ApiResponseCodes.OK, message: "Successful!", 
                data: (RouteViewModel)model);
        }

        public async Task<ApiResponse<RouteViewModel>> GetRoute(string id)
        {
            if (!Guid.TryParse(id, out Guid guidId) || string.IsNullOrEmpty(id))
            {
#if DEBUG
                return new ApiResponse<RouteViewModel>(errors: "model cannot be null or empty", codes: ApiResponseCodes.INVALID_REQUEST, message: "Unsuccessful!");
#else
                return new ApiResponse<RouteViewModel>(errors: "invalid request", codes: ApiResponseCodes.INVALID_REQUEST,  message: "Unsuccessful!");
#endif
            }

            var existingRoute =  await GetByIdAsync(guidId);

            if (existingRoute == null)
            {
                return new ApiResponse<RouteViewModel>(errors: "Could not find Route with Id" , codes: ApiResponseCodes.NOT_FOUND, message: "Unsuccessful!");
            }
            

            return new ApiResponse<RouteViewModel>(codes: ApiResponseCodes.OK, data: (RouteViewModel)existingRoute);
        }

        public async Task<ApiResponse<PaginatedList<RouteListViewModel>>> GetAllRoutesAsync(BaseSearchViewModel searchModel)
        {
            //var response = new ApiResponse<PaginatedList<RouteViewModel>> { Code = ApiResponseCodes.OK };

            var keyword = string.IsNullOrEmpty(searchModel.Keyword) ? null : searchModel.Keyword.ToLower();
            int pageStart = searchModel.PageIndex ?? 1;
            int pageEnd = searchModel.PageTotal ?? 50;

            //join pickup points// departure and destination
            var modelEntities = SqlQuery<RouteDto>("[dbo].[Sp_GetRoutes]  @p0, @p1, @p2",
                                                     keyword, pageStart, pageEnd)
                                                    .Select(p => (RouteListViewModel)p)
                                                    .ToList();


            var count = modelEntities.Count();
            return await Task.FromResult(new ApiResponse<PaginatedList<RouteListViewModel>>(message: modelEntities != null && modelEntities.Any() ? "Successful" : "No route record found",
                                                                                codes: ApiResponseCodes.OK, 
                                                                                totalCount: modelEntities != null && modelEntities.Any() ?  modelEntities.FirstOrDefault().TotalCount: 0,
                                                                                data: modelEntities.ToPaginatedList(pageStart, pageEnd, count)));
        }

       

        private string RouteName(string departurePickupPt, string destinationPickupPt) => $"{departurePickupPt} ==> {destinationPickupPt}";
    }
}
