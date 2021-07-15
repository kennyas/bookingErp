using FluentValidation;

namespace Report.Core.Validators
{
    class GetBookedTicketReportValidator : AbstractValidator<ViewModels.GetBookedTicketReportViewModel>
    {
        public GetBookedTicketReportValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.PageIndex).NotEmpty().WithMessage("Kindly specify page index");
                RuleFor(model => model.PageTotal).NotEmpty().WithMessage("Kindly specify page size");
            });
        }
    }

    class AddBookedTicketReportValidator : AbstractValidator<ViewModels.AddBookedTicketReportViewModel>
    {
        public AddBookedTicketReportValidator()
        {
            When(model => model != null, () =>
            {
                RuleFor(model => model.ReferenceCode).NotEmpty().WithMessage("Kindly specify page index");
            });
        }
    }
}
