using FluentValidation;

namespace Report.Core.Validators
{
    public class GetTotalSalesByVehicleValidator : AbstractValidator<ViewModels.GetTotalSalesByVehicleViewModel>
    {
        public GetTotalSalesByVehicleValidator()
        {
            When(model => model != null, () =>
            {
                //RuleFor(model => model.VehicleId).NotEmpty();
            });
        }
    }

    public class GetTotalSalesByTripValidator : AbstractValidator<ViewModels.GetTotalSalesByTripViewModel>
    {
        public GetTotalSalesByTripValidator()
        {
            When(model => model != null, () =>
            {
                //RuleFor(model => model.TripId).NotEmpty();
            });
        }
    }

    public class GetTotalSalesByTerminalValidator : AbstractValidator<ViewModels.GetTotalSalesByTerminalViewModel>
    {
        public GetTotalSalesByTerminalValidator()
        {
            When(model => model != null, () =>
            {
                //RuleFor(model => model.DepartureTerminalId).NotEmpty();
            });
        }
    }

    public class GetTotalSalesByRouteValidator : AbstractValidator<ViewModels.GetTotalSalesByRouteViewModel>
    {
        public GetTotalSalesByRouteValidator()
        {
            When(model => model != null, () =>
            {
                //RuleFor(model => model.RouteId).NotEmpty();
            });
        }
    }
}
