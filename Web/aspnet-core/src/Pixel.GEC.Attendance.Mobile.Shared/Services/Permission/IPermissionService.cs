﻿namespace Pixel.GEC.Attendance.Services.Permission
{
    public interface IPermissionService
    {
        bool HasPermission(string key);
    }
}