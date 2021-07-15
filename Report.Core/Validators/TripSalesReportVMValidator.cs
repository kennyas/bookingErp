using FluentValidation;

namespace Report.Core.Validators
{
    class GetTripSalesReportValidator : AbstractValidator<ViewModels.GetTripSalesReportViewModel>
    {
        public GetTripSalesReportValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.PageIndex).NotEmpty().WithMessage("Kindly specify page index");
                RuleFor(model => model.PageTotal).NotEmpty().WithMessage("Kindly specify page size");
            });
        }
    }
}
