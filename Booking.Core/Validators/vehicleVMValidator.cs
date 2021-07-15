using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Core.Validators
{
    class CreateVehicleValidator : AbstractValidator<ViewModels.VehicleViewModel>
    {
        public CreateVehicleValidator()
        {
            When(model => model != null, () =>
            {
                //RuleFor(model => model.PartnerId).NotNull().NotEmpty();
                RuleFor(model => model.RegistrationNumber).NotEmpty();
                RuleFor(model => model.NoOfSeats).NotEmpty();
                RuleFor(model => model.VehicleModelId).NotEmpty();
                RuleFor(model => model.ChassisNumber).NotEmpty();
            });
        }
    }

    class EditVehicleValidator : AbstractValidator<ViewModels.EditVehicleViewModel>
    {
        public EditVehicleValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.PartnerId).NotEmpty();
                RuleFor(model => model.RegistrationNumber).NotEmpty();
                RuleFor(model => model.NoOfSeats).NotEmpty();
                RuleFor(model => model.VehicleModelId).NotEmpty();
                RuleFor(model => model.ChassisNumber).NotEmpty();
                RuleFor(model => model.Id).NotEmpty();
            });
        }
    }

    class GetAllVehiclePaginatedValidator : AbstractValidator<ViewModels.VehiclePaginatedViewModel>
    {
        public GetAllVehiclePaginatedValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.PageTotal).NotEmpty();
                RuleFor(model => model.PageIndex).NotEmpty();
            });
        }
    } 
    
    class SearchVehicleValidator : AbstractValidator<ViewModels.VehiclePaginatedViewModel>
    {
        public SearchVehicleValidator()
        {
            When(model => model != null, () =>
            {
                //RuleFor(model => model.Keyword).NotEmpty();
                RuleFor(model => model.PageTotal).NotEmpty();
                RuleFor(model => model.PageIndex).NotEmpty();
            });
        }
    }

    class DeleteVehicleValidator : AbstractValidator<ViewModels.DeleteVehicleViewModel>
    {
        public DeleteVehicleValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Id).NotEmpty();
            });
        }
    }

    class SearchVehicleByChassisNumberValidator : AbstractValidator<ViewModels.SearchVehicleByChassisNumberViewModel>
    {
        public SearchVehicleByChassisNumberValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.ChassisNumber).NotEmpty();
            });
        }
    }

    class SearchVehicleByPartnerIdValidator : AbstractValidator<ViewModels.GetVehicleByPartnerIdViewModel>
    {
        public SearchVehicleByPartnerIdValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.PartnerId).NotEmpty();
            });
        }
    }

    class SearchVehicleByRegistrationNumberValidator : AbstractValidator<ViewModels.SearchVehicleByRegistrationNumberViewModel>
    {
        public SearchVehicleByRegistrationNumberValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.RegistrationNumber).NotEmpty();
            });
        }
    }

}
