﻿using System.Threading.Tasks;

namespace Pixel.GEC.Attendance.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}