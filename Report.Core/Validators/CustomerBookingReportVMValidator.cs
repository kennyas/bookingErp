using FluentValidation;

namespace Report.Core.Validators
{
    class GetCustomerBookingReportValidator : AbstractValidator<ViewModels.GetCustomerBookingsReportViewModel>
    {
        public GetCustomerBookingReportValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.PageIndex).NotEmpty().WithMessage("Kindly specify page index");
                RuleFor(model => model.PageTotal).NotEmpty().WithMessage("Kindly specify page size");
                RuleFor(model => model.StartDate).NotEmpty().WithMessage("Kindly specify booking start date");
                RuleFor(model => model.EndDate).NotEmpty().WithMessage("Kindly specify booking end date");
            });
        }
    }

    class AddCustomerBookingReportValidator : AbstractValidator<ViewModels.AddCustomerBookingsReportViewModel>
    {
        public AddCustomerBookingReportValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.RouteId);
            });
        }
    }

    class UpdateCustomerBookingReportValidator : AbstractValidator<ViewModels.UpdateCustomerBookingsReportViewModel>
    {
        public UpdateCustomerBookingReportValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.RouteId);
            });
        }
    }
}
