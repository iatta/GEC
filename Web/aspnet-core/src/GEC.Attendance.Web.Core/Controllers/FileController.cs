using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Abp.Auditing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using GEC.Attendance.Dto;
using GEC.Attendance.Storage;

namespace GEC.Attendance.Web.Controllers
{
    public class FileController : AttendanceControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IHostingEnvironment _environment;

        public FileController(
            ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager,
            IHostingEnvironment environment
        )
        {
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
            _environment = environment;
        }

        [DisableAuditing]
        public ActionResult DownloadTempFile(FileDto file)
        {
            var fileBytes = _tempFileCacheManager.GetFile(file.FileToken);
            if (fileBytes == null)
            {
                return NotFound(L("RequestedFileDoesNotExists"));
            }

            return File(fileBytes, file.FileType, file.FileName);
        }

        [HttpGet]
        public ActionResult DownloadTimeProfileSample()
        {
            var path = Path.Combine(_environment.ContentRootPath, "SampleFiles/TimeSheetFormat.xlsx");
            var fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TimeSheetFormat.xlsx");

        }
        [HttpGet]
        public ActionResult DownloadEmployeeVacationSample()
        {
            var path = Path.Combine(_environment.ContentRootPath, "SampleFiles/EmpVacation.xlsx");
            var fileBytes = System.IO.File.ReadAllBytes(path);

            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "TimeSheetFormat.xlsx");

        }

        [DisableAuditing]
        public async Task<ActionResult> DownloadBinaryFile(Guid id, string contentType, string fileName)
        {
            var fileObject = await _binaryObjectManager.GetOrNullAsync(id);
            if (fileObject == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            return File(fileObject.Bytes, contentType, fileName);
        }
    }
}