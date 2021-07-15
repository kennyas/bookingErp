using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Core.Validators
{
    class GetRoutePickupPointsVMValidator : AbstractValidator<ViewModels.GetRoutePickupPoints>
    {
        public GetRoutePickupPointsVMValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.RouteId).NotEmpty();
            });
        }
    }

    class CreateRoutePickupPointVMValidator : AbstractValidator<ViewModels.CreateRoutePickupPointViewModel>
        {
            public CreateRoutePickupPointVMValidator()
            {
                When(model => model != null, () =>
                {
                    RuleFor(model => model.RouteId).NotEmpty();
                    RuleFor(model => model.OrderNo).NotEmpty();
                    RuleFor(model => model.PickupPointId).NotEmpty();
                    RuleFor(model => model.PickupPointType).NotEmpty();
                });
            }
        }  
    
    class DeleteRoutePickupPointVMValidator : AbstractValidator<ViewModels.DeleteRoutePickupPointsById>
        {
            public DeleteRoutePickupPointVMValidator()
            {
                When(model => model != null, () =>
                {
                    RuleFor(model => model.RouteId).NotEmpty();
                    RuleFor(model => model.PickupPointId).NotEmpty();
                });
            }
        }

       class DeleteAllRoutePickupPointVMValidator : AbstractValidator<ViewModels.DeleteAllRoutePickupPointsById>
        {
            public DeleteAllRoutePickupPointVMValidator()
            {
                When(model => model != null, () =>
                {
                    RuleFor(model => model.RouteId).NotEmpty();
                });
            }
        }


    class GetRoutePickUpPointByDepartureVMValidator : AbstractValidator<ViewModels.GetRoutePickUpPointByDepartureId>
    {
        public GetRoutePickUpPointByDepartureVMValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.DeparturePickupPointId).NotEmpty();
            });
        }
    }

    class GetRoutePickUpPointByDestinationVMValidator : AbstractValidator<ViewModels.GetDeparturePickUpPointByDestinationId>
    {
        public GetRoutePickUpPointByDestinationVMValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.DestinationPickupPointId).NotEmpty();
            });
        }
    }

    class GetRoutePickUpPointsVMValidator : AbstractValidator<ViewModels.GetDeparturePickUpPoints>
    {
        public GetRoutePickUpPointsVMValidator()
        {
            When(model => model != null, () =>
            {
                //RuleFor(model => model.DestinationPickupPointId).NotEmpty();
            });
        }
    }
    class GetOrderedDeparturePickUpPointsVMValidator : AbstractValidator<ViewModels.GetOrderedDeparturePickUpPoints>
    {
        public GetOrderedDeparturePickUpPointsVMValidator()
        {
            When(model => model != null, () =>
            {
                //RuleFor(model => model.DestinationPickupPointId).NotEmpty();
                RuleFor(model => model.Longitude).NotEmpty();
                RuleFor(model => model.Latitude).NotEmpty();

            });
        }
    }
}
