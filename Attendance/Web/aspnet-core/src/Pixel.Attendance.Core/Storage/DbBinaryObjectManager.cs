﻿using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;

namespace Pixel.Attendance.Storage
{
    public class DbBinaryObjectManager : IBinaryObjectManager, ITransientDependency
    {
        private readonly IRepository<BinaryObject, Guid> _binaryObjectRepository;

        public DbBinaryObjectManager(IRepository<BinaryObject, Guid> binaryObjectRepository)
        {
            _binaryObjectRepository = binaryObjectRepository;
        }

        public Task<BinaryObject> GetOrNullAsync(Guid id)
        {
            return _binaryObjectRepository.FirstOrDefaultAsync(id);
        }

        public void SaveAsync(BinaryObject file)
        {
             _binaryObjectRepository.Insert(file);
        }

        public Task DeleteAsync(Guid id)
        {
            return _binaryObjectRepository.DeleteAsync(id);
        }
    }
}