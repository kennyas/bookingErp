using FluentValidation;

namespace Report.Core.Validators
{
    class GetCustomerBookingDashboardByDateRangeValidator : AbstractValidator<ViewModels.GetCustomerBookingsDashboardByDateRangeViewModel>
    {
        public GetCustomerBookingDashboardByDateRangeValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.PageIndex).NotEmpty().WithMessage("Kindly specify page index");
                RuleFor(model => model.PageTotal).NotEmpty().WithMessage("Kindly specify page size");
                RuleFor(model => model.StartDate).NotEmpty();
                RuleFor(model => model.EndDate).NotEmpty();

            });
        }
    }


    class GetCustomerBookingDashboardByDateValidator : AbstractValidator<ViewModels.GetCustomerBookingsDashboardByDateViewModel>
    {
        public GetCustomerBookingDashboardByDateValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.PageIndex).NotEmpty().WithMessage("Kindly specify page index");
                RuleFor(model => model.PageTotal).NotEmpty().WithMessage("Kindly specify page size");
                RuleFor(model => model.Date).NotEmpty();
            });
        }
    }
}
