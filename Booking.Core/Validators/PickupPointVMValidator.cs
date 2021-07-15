using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Core.Validators
{
    class CreatePickupPointVMValidator : AbstractValidator<ViewModels.PickupPointViewModel>
    {
        public CreatePickupPointVMValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Description).NotEmpty();
                RuleFor(model => model.ShortDescription).NotEmpty();
                RuleFor(model => model.Title).NotEmpty();
                RuleFor(model => model.Latitude).NotEmpty();
                RuleFor(model => model.Longitude).NotEmpty();
                RuleFor(model => model.AreaId).NotEmpty();
            });
        }
    }

    class EditPickupPointVMValidator : AbstractValidator<ViewModels.PickupPointViewModel>
    {
        public EditPickupPointVMValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Id).NotEmpty();
                RuleFor(model => model).NotEmpty().WithMessage("Model can not be empty");
                RuleFor(model => model.Description).NotEmpty().WithMessage("Kindly specify pickup point description");
                RuleFor(model => model.ShortDescription).NotEmpty().WithMessage("Kindly specify pickup point short description");
                RuleFor(model => model.Title).NotEmpty().WithMessage("Kindly specify pickup point title");
                RuleFor(model => model.Latitude).NotEmpty().WithMessage("Kindly specify pickup point latitude");
                RuleFor(model => model.Longitude).NotEmpty().WithMessage("Kindly specify pickup point longitude");
                RuleFor(model => model.AreaId).NotEmpty().WithMessage("Kindly specify pickup point state id");
            });
        }
    }

    class SearchPickupPointValidator : AbstractValidator<ViewModels.SearchPickupPointViewModel>
    {
        public SearchPickupPointValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Keyword).NotEmpty().WithMessage("Kindly specify a search keyword");
                RuleFor(model => model.PageIndex).NotEmpty().WithMessage("Kindly specify page index");
                RuleFor(model => model.PageTotal).NotEmpty().WithMessage("Kindly specify page size");
            });
        }
    } 
    
    class GetPickupPointByIdValidator : AbstractValidator<ViewModels.GetPickupPointByIdViewModel>
    {
        public GetPickupPointByIdValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Id).NotEmpty();
                RuleFor(model => model.PageIndex).NotEmpty().WithMessage("Kindly specify page index");
                RuleFor(model => model.PageTotal).NotEmpty().WithMessage("Kindly specify page size");
            });
        }
    }
    
    class GetPickupPointByStateValidator : AbstractValidator<ViewModels.SearchPickupPointViewModel>
    {
        public GetPickupPointByStateValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Keyword).NotEmpty().WithMessage("Kindly specify state ID");
            });
        }
    }
    
    class GetAllPickupPointValidator : AbstractValidator<ViewModels.PickupPointPaginatedViewModel>
    {
        public GetAllPickupPointValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.PageIndex).NotEmpty().WithMessage("Kindly specify page index");
                RuleFor(model => model.PageTotal).NotEmpty().WithMessage("Kindly specify page total");
            });
        }
    }

    class DeletePickupPointValidator : AbstractValidator<ViewModels.DeletePickupPointViewModel>
    {
        public DeletePickupPointValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Id).NotEmpty().WithMessage("Kindly specify the pickup point id");
            });
        }
    }

    class CreateAreaValidator : AbstractValidator<ViewModels.CreateAreaViewModel>
    {
        public CreateAreaValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Title).NotEmpty().WithMessage("Kindly specify the title");
                RuleFor(model => model.Description).NotEmpty().WithMessage("Kindly specify the description");
                RuleFor(model => model.StateId).NotEmpty().WithMessage("Kindly specify the state");
            });
        }
    }

    class EditAreaValidator : AbstractValidator<ViewModels.EditAreaViewModel>
    {
        public EditAreaValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.Id).NotEmpty().WithMessage("Kindly specify the title");
                RuleFor(model => model.Title).NotEmpty().WithMessage("Kindly specify the title");
                RuleFor(model => model.Description).NotEmpty().WithMessage("Kindly specify the description");
                RuleFor(model => model.StateId).NotEmpty().WithMessage("Kindly specify the state");
            });
        }
    }
}
