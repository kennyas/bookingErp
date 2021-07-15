using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Core.Validators
{
    class GetAllTripValidator : AbstractValidator<ViewModels.GetAllTripViewModel>
    {
        public GetAllTripValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.PageIndex).NotEmpty().WithMessage("Kindly specify page index");
                RuleFor(model => model.PageTotal).NotEmpty().WithMessage("Kindly specify page total");
            });
        }
    }

    class GetTripsByCreatedAtDateRangeValidator : AbstractValidator<ViewModels.GetTripsByDateRangeViewModel>
    {
        public GetTripsByCreatedAtDateRangeValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.StartDate).NotEmpty().WithMessage("Start date is required");
                RuleFor(model => model.EndDate).NotEmpty().WithMessage("End date is required");
                RuleFor(model => model.PageIndex).NotEmpty().WithMessage("Kindly specify page index");
                RuleFor(model => model.PageTotal).NotEmpty().WithMessage("Kindly specify page total");
            });
        }
    }

    class GetTripsWithActiveDiscountValidator : AbstractValidator<ViewModels.GetTripsWithActiveDiscountViewModel>
    {
        public GetTripsWithActiveDiscountValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.PageIndex).NotEmpty().WithMessage("Kindly specify page index");
                RuleFor(model => model.PageTotal).NotEmpty().WithMessage("Kindly specify page size");
            });
        }
    }

    class GetTripsByIdValidator : AbstractValidator<ViewModels.GetTripByIdViewModel>
    {
        public GetTripsByIdValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Id).NotEmpty();
            });
        }
    }  
    
    class GetTripsByVehicleIdValidator : AbstractValidator<ViewModels.GetTripsByVehicleIdViewModel>
    {
        public GetTripsByVehicleIdValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.VehicleId).NotEmpty();
                RuleFor(model => model.PageIndex).NotEmpty().WithMessage("Kindly specify page index");
                RuleFor(model => model.PageTotal).NotEmpty().WithMessage("Kindly specify page size");
            });
        }
    }

    class GetTripsByVehicleRegNoValidator : AbstractValidator<ViewModels.GetTripsByVehicleRegNoViewModel>
    {
        public GetTripsByVehicleRegNoValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.VehicleRegistrationNumber).NotEmpty();
                RuleFor(model => model.PageIndex).NotEmpty().WithMessage("Kindly specify page index");
                RuleFor(model => model.PageTotal).NotEmpty().WithMessage("Kindly specify page size");
            });
        }
    }

    class CreateTripValidator : AbstractValidator<ViewModels.TripViewModel>
    {
        public CreateTripValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.RouteId).NotEmpty();
                RuleFor(model => model.VehicleModelTitle).NotEmpty();
                RuleFor(model => model.DepartureTime).NotEmpty();
                RuleFor(model => model.VehicleId).NotEmpty();
            });
        }
    }

    class DeleteTripValidator : AbstractValidator<ViewModels.DeleteTripViewModel>
    {
        public DeleteTripValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Id).NotEmpty();
            });
        }
    }

    class DeleteTripChildrenFeeValidator : AbstractValidator<ViewModels.DeleteTripChildrenFeeViewModel>
    {
        public DeleteTripChildrenFeeValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Id).NotEmpty();
            });
        }
    }

    class DeleteTripDiscountValidator : AbstractValidator<ViewModels.DeleteTripDiscountViewModel>
    {
        public DeleteTripDiscountValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Id).NotEmpty();
            });
        }
    }

    class EditTripValidator : AbstractValidator<ViewModels.EditTripViewModel>
    {
        public EditTripValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Id).NotEmpty();
                RuleFor(model => model.DepartureTime).NotEmpty();
                RuleFor(model => model.CanBeScheduled).NotEmpty();
                RuleFor(model => model.VehicleId).NotEmpty();
            });
        }
    } 
    
    class EditTripChildrenFeeValidator : AbstractValidator<ViewModels.EditTripChildrenFeeViewModel>
    {
        public EditTripChildrenFeeValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Id).NotEmpty();
                RuleFor(model => model.ChildrenFee).NotEmpty();
            });
        }
    }  
    
    class EditTripDiscountValidator : AbstractValidator<ViewModels.TripDiscountViewModel>
    {
        public EditTripDiscountValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Id).NotEmpty();
                RuleFor(model => model.Discount).NotEmpty();
                RuleFor(model => model.DiscountStartDate).NotEmpty();
                RuleFor(model => model.DiscountEndDate).NotEmpty();
            });
        }
    }


}
