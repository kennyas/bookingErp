﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace UserManagement.Core.Enums
{
    public enum BusBoyStatus
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
