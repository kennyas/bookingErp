using System.ComponentModel;

namespace UserManagement.Core.Enums
{
    public enum CaptainStatus
    {
        Idle,
        [Description("On-a-Journey")]
        OnAJourney,
        Suspended,
        OnLeave,
        [Description("Dismissed")]
        Dismissed,
        Decease,
        Retired
    }
}
