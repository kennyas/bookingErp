using FluentValidation;

namespace Report.Core.Validators
{
    class GetBusboySalesReportValidator : AbstractValidator<ViewModels.GetBusboySalesReportViewModel>
    {
        public GetBusboySalesReportValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.BusboyId).NotEmpty();
            });
        }
    }
}
