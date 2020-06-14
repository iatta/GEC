﻿using System.Threading.Tasks;
using Abp.Webhooks;

namespace GEC.Attendance.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
