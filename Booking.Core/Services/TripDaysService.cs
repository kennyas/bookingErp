using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.ViewModels;
using System.Linq;

namespace Booking.Core.Services
{
    public class TripDaysService : Service<TripDays>, ITripDaysService
    {
        private readonly IHttpUserService _currentUserService;
        public TripDaysService(IUnitOfWork unitOfWork, IHttpUserService currentUserService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
        }

        public Task<ApiResponse<List<TripDaysDetailViewModel>>> AllTripDays(TripDaysSearchViewModel model)
        {
            if (model is null)
            {
                return Task.FromResult(new ApiResponse<List<TripDaysDetailViewModel>>(errors: "", codes: ApiResponseCodes.OK, message: "Unsuccessful!"));
            }

            var allTripDays = from days in UnitOfWork.Repository<TripDays>().GetAll()
                              select new TripDaysDetailViewModel { 
                                    CreatedBy = days.CreatedBy,
                                    Friday = days.Friday,
                                    Sunday = days.Sunday,
                                    ModifiedBy = days.ModifiedBy,
                                    CreatedOn = days.CreatedOn,
                                    ModifiedOn = days.ModifiedOn.HasValue ? days.ModifiedOn.Value : default,
                                    Monday = days.Monday,
                                    Saturday = days.Saturday,
                                    Title = days.Title,
                                    Thursday = days.Thursday,
                                    Tuesday = days.Tuesday,
                                    Wednesday = days.Wednesday,
                                    Id = days.Id
                              };

            var tripDaysCount = allTripDays.Count();

            return Task.FromResult(new ApiResponse<List<TripDaysDetailViewModel>>(message: allTripDays.Any() ? "Successful!" : "Unsuccessful", data: allTripDays.ToList(), 
                                                                codes : allTripDays.Any()? ApiResponseCodes.OK : ApiResponseCodes.NOT_FOUND));

        }
        public async Task<ApiResponse<TripDaysViewModel>> CreateTripDays(TripDaysRequestModel model)
        {
            if (model is null)
            {
                return new ApiResponse<TripDaysViewModel>(errors: "", codes: ApiResponseCodes.OK, message: "Unsuccessful!");
            }

            var newTripDay = new TripDays { 
                CreatedBy = _currentUserService.GetCurrentUser().UserId,
                Friday = model.Friday,
                Monday = model.Monday,
                Saturday = model.Saturday,
                Sunday = model.Sunday,
                Thursday = model.Thursday,
                Tuesday = model.Tuesday,
                Wednesday = model.Wednesday,
                Title = model.Title,
            };

            var responseDays = await AddAsync(newTripDay);

            //responseDays > 0 = 
            return new ApiResponse<TripDaysViewModel>(codes: responseDays > 0 ? ApiResponseCodes.OK : ApiResponseCodes.FAILED,
                                                      message: responseDays > 0 ? "Successful" : "Unsuccessful!");
        }
    }
}
