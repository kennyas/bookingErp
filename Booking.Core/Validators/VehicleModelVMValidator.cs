using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Booking.Core.Validators
{
    class CreateVehicleModelValidator : AbstractValidator<ViewModels.VehicleModelViewModel>
    {
        public CreateVehicleModelValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Title).NotEmpty().WithMessage("Please specify the vehicle name");
                RuleFor(model => model.Description).NotEmpty().WithMessage("Please specify the vehicle description");
                RuleFor(model => model.ShortDescription).NotEmpty().WithMessage("Please specify the vehicle short description");
                RuleFor(model => model.VehicleMakeId).NotEmpty().WithMessage("Please provide the MakeId of the vehicle");
                RuleFor(model => model.NoOfSeats).NotEmpty().WithMessage("Please provide the number of seat of the vehicle");
            });
        }
    }

    class EditVehicleModelValidator : AbstractValidator<ViewModels.EditVehicleModelViewModel>
    {
        public EditVehicleModelValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Title).NotEmpty().WithMessage("Please specify the vehicle name");
                RuleFor(model => model.Description).NotEmpty().WithMessage("Please specify the vehicle description");
                RuleFor(model => model.ShortDescription).NotEmpty().WithMessage("Please specify the vehicle short description");
                RuleFor(model => model.VehicleMakeId).NotEmpty().WithMessage("Please specify the MakeId of the vehicle");
                RuleFor(model => model.Id).NotEmpty().WithMessage("Please specify the Vehicle model ID");
            });
        }
    }

    class DeleteVehicleModelValidator : AbstractValidator<ViewModels.DeleteVehicleModel>
    {
        public DeleteVehicleModelValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.ModelId).NotEmpty().WithMessage("Please specify the vehicle model id");
            });
        }
    }

    class GetAllVehicleModelValidator : AbstractValidator<ViewModels.VehicleModelSearch>
    {
        public GetAllVehicleModelValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.PageIndex).NotEmpty().WithMessage("Kindly specify the page index");
                RuleFor(model => model.PageTotal).NotEmpty().WithMessage("Kindly specify the page size");
            });
        }
    }

    class SearchVehicleModelValidator : AbstractValidator<ViewModels.VehicleModelSearch>
    {
        public SearchVehicleModelValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Keyword).NotEmpty().WithMessage("Please specify a search keyword");
            });
        }
    }

    class GetVehicleModelsByMakeValidator : AbstractValidator<ViewModels.VehicleModelByMake>
    {
        public GetVehicleModelsByMakeValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.MakeId).NotEmpty().WithMessage("Kindly specify the vehicle make");
            });
        }
    }

    class GetVehicleModelByIdValidator : AbstractValidator<ViewModels.VehicleModelById>
    {
        public GetVehicleModelByIdValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Id).NotEmpty().WithMessage("Please specify the vehicle model id");
            });
        }
    }
}
