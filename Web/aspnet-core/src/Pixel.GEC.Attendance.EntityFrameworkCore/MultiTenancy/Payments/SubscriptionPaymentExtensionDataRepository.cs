﻿using System.Threading.Tasks;
using Abp.EntityFrameworkCore;
using Pixel.GEC.Attendance.EntityFrameworkCore;
using Pixel.GEC.Attendance.EntityFrameworkCore.Repositories;

namespace Pixel.GEC.Attendance.MultiTenancy.Payments
{
    public class SubscriptionPaymentExtensionDataRepository : AttendanceRepositoryBase<SubscriptionPaymentExtensionData, long>,
        ISubscriptionPaymentExtensionDataRepository
    {
        public SubscriptionPaymentExtensionDataRepository(IDbContextProvider<AttendanceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<string> GetExtensionDataAsync(long subscriptionPaymentId, string key)
        {
            var data = await FirstOrDefaultAsync(p => p.SubscriptionPaymentId == subscriptionPaymentId && p.Key == key);

            return data?.Value;
        }

        public async Task<long?> GetPaymentIdOrNullAsync(string key, string value)
        {
            var data = await FirstOrDefaultAsync(p => p.Key == key && p.Value == value);
            return data?.SubscriptionPaymentId;
        }

        public async Task SetExtensionDataAsync(long subscriptionPaymentId, string key, string value)
        {
            var data = await FirstOrDefaultAsync(p => p.SubscriptionPaymentId == subscriptionPaymentId && p.Key == key);

            if (data != null)
            {
                await DeleteAsync(data);
            }

            await InsertAsync(new SubscriptionPaymentExtensionData()
            {
                SubscriptionPaymentId = subscriptionPaymentId,
                Key = key,
                Value = value
            });
        }

    }
}
