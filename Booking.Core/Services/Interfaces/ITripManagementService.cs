using Booking.Core.Models;
using Booking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tornado.Shared.EF.Services;
using Tornado.Shared.ViewModels;

namespace Booking.Core.Services.Interfaces
{
    public interface ITripManagementService : IService<TripManagement>
    {
        List<ValidationResult> CreateTripManagement(CreateTripManagementViewModel model);
        TripManagementViewModel GetTripInformation(Guid tripInfoId);
        List<TripManagementViewModel> GetAllTripInformation(BaseSearchViewModel searchModel);
    }
}