using System.ComponentModel;

namespace Report.Core.Enums
{
    public enum PaymentStatus
    {
        PENDING,
        PAID,
        FAILED,
        REVERSED,
        [Description("Partial-Reversal")]
        PARTIAL_REVERSED,
        DECLINED
    }
}