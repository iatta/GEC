﻿using System.Collections.Generic;

namespace Pixel.GEC.Attendance.EntityHistory
{
    public class EntityHistoryUiSetting
    {
        public bool IsEnabled { get; set; }

        public List<string> EnabledEntities { get; set; }
    }
}