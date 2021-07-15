using FluentValidation;

namespace Report.Core.Validators
{
    public class GetBookedBusReportValidator : AbstractValidator<ViewModels.GetBookedBusReportViewModel>
    {
        public GetBookedBusReportValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.PageIndex).NotEmpty().WithMessage("Kindly specify page index");
                RuleFor(model => model.PageTotal).NotEmpty().WithMessage("Kindly specify page size");
            });
        }
    }
}
