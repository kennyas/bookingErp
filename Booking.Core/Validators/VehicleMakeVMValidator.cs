using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Core.Validators
{
    class CreateVehicleMakeValidator : AbstractValidator<ViewModels.VehicleMakeViewModel>
    {
        public CreateVehicleMakeValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Description).NotEmpty().WithMessage("Kindly specify the vehicle make description ");
                RuleFor(model => model.ShortDescription).NotEmpty().WithMessage("Kindly specify the vehicle make short description");
                RuleFor(model => model.Title).NotEmpty().WithMessage("Kindly specify the vehicle make title");
            });
        }
    }

    class EditVehicleMakeValidator : AbstractValidator<ViewModels.EditVehicleMakeViewModel>
    {
        public EditVehicleMakeValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Description).NotEmpty().WithMessage("Kindly specify the vehicle make description ");
                RuleFor(model => model.ShortDescription).NotEmpty().WithMessage("Kindly specify the vehicle make short description");
                RuleFor(model => model.Title).NotEmpty().WithMessage("Kindly specify the vehicle make title");
                RuleFor(model => model.Id).NotEmpty().WithMessage("Please specify the Vehicle make ID");
            });
        }
    }

    class VehicleMakeSearchValidator : AbstractValidator<ViewModels.VehicleMakeSearchViewModel>
    {
        public VehicleMakeSearchValidator()
        {
            When(model => model != null, () =>
            {
                //RuleFor(model => model.Keyword).NotEmpty().WithMessage("Kindly specify a search keyword");
                RuleFor(model => model.PageIndex).NotEmpty().WithMessage("Kindly specify the page index");
                RuleFor(model => model.PageTotal).NotEmpty().WithMessage("Kindly specify the page size");
            });
        }
    } 
    
    class GetAllVehicleMakeValidator : AbstractValidator<ViewModels.VehicleMakeSearchViewModel>
    {
        public GetAllVehicleMakeValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.PageIndex).NotEmpty().WithMessage("Kindly specify the page index");
                RuleFor(model => model.PageTotal).NotEmpty().WithMessage("Kindly specify the page size");
            });
        }
    } 
    
    class GetVehicleMakeValidator : AbstractValidator<ViewModels.GetVehicleMakeByIdViewModel>
    {
        public GetVehicleMakeValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.MakeId).NotEmpty().WithMessage("Kindly specify the vehicle make id");
            });
        }
    }

    class DeleteVehicleMakeValidator : AbstractValidator<ViewModels.DeleteVehicleMakeViewModel>
    {
        public DeleteVehicleMakeValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.MakeId).NotEmpty().WithMessage("Please specify the vehicle make id");
            });
        }
    }
}
