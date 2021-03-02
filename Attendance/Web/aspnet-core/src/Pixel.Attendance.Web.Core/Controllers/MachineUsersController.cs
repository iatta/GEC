using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pixel.Attendance.Configuration;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Web.Controllers
{
    [AbpAllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class MachineUsersController : AttendanceControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfigurationRoot _appConfiguration;
        
        public MachineUsersController(IHttpClientFactory clientFactory, IAppConfigurationAccessor configurationAccessor)
        {
            _clientFactory = clientFactory;
            _appConfiguration = configurationAccessor.Configuration;

        }

        [HttpPost]
        public async Task<List<ReadAllUsersOutput>> ReadAllUsers([FromBody]  List<UploadMachineUserInput> input)
        {
            var output = new List<ReadAllUsersOutput>();
            foreach (var item in input)
            {
                var inputJson = new StringContent(
                JsonSerializer.Serialize(item, new JsonSerializerOptions()), Encoding.UTF8, "application/json");
                var client = _clientFactory.CreateClient();
                var response = await client.PostAsync(_appConfiguration["Machine:readAllUsersAPI"], inputJson);
                if (response.IsSuccessStatusCode)
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        output.Add(await JsonSerializer.DeserializeAsync<ReadAllUsersOutput>(responseStream));
                    }
                }
            }
           

            return output;
        }

        [HttpPost]
        public async Task<bool> UploadUsers([FromBody] List<UploadMachineUserInput> input)
        {

            try
            {
                foreach (var item in input)
                {
                    //item.Person.UserCode = 22;
                    //item.Person.PName = "test move22";
                    var inputJson = new StringContent(
                    JsonSerializer.Serialize(item, new JsonSerializerOptions()), Encoding.UTF8, "application/json");
                    var client = _clientFactory.CreateClient();
                    var response = await client.PostAsync(_appConfiguration["Machine:uploadUserAPI"], inputJson);
                    if (response.IsSuccessStatusCode)
                    {
                        using (var responseStream = await response.Content.ReadAsStreamAsync())
                        {
                            await JsonSerializer.DeserializeAsync<string>(responseStream);
                        }

                        //download image 
                        var downloadImageInput = new DownloadImageInput();
                        downloadImageInput.UserCode = item.Person.UserCode;
                        downloadImageInput.MachineData = item.MachineData;
                        var uploadImageResponse = await DownloadImage(downloadImageInput);

                        downloadImageInput.Image = Convert.ToBase64String(uploadImageResponse.Datas);
                        uploadImageResponse.MachineData = item.MachineData;
                        uploadImageResponse.UserCode = item.Person.UserCode;
                        await UploadImage(uploadImageResponse);

                    }
                }

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }


        [HttpPost]
        public async Task<bool> DeleteUser([FromBody] List<DownloadImageInput>  inputs)
        {
            var output = false;
            foreach (var input in inputs)
            {
                var inputJson = new StringContent(JsonSerializer.Serialize(input, new JsonSerializerOptions()), Encoding.UTF8, "application/json");
                var client = _clientFactory.CreateClient();
                var response = await client.PostAsync(_appConfiguration["Machine:deleteUserApi"], inputJson);
                if (response.IsSuccessStatusCode)
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        output = await JsonSerializer.DeserializeAsync<bool>(responseStream);
                    }
                }

            }

            return output;
        }

        [HttpPost]
        public async Task<DownloadImageInput> DownloadImage(DownloadImageInput input)
        {
            var output = new DownloadImageInput();
            var inputJson = new StringContent(JsonSerializer.Serialize(input, new JsonSerializerOptions()), Encoding.UTF8, "application/json");
            var client = _clientFactory.CreateClient();
            var response = await client.PostAsync(_appConfiguration["Machine:downloadImageAPI"], inputJson);
            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    output = await JsonSerializer.DeserializeAsync<DownloadImageInput>(responseStream);
                }
            }

            return output;
        }

        [HttpPost]
        public async Task<DownloadImageInput> UploadImage(DownloadImageInput input)
        {
            var output = new DownloadImageInput();
            input.Image = Convert.ToBase64String(input.Datas);
            var inputJson = new StringContent(JsonSerializer.Serialize(input, new JsonSerializerOptions()), Encoding.UTF8, "application/json");
            var client = _clientFactory.CreateClient();
            var response = await client.PostAsync(_appConfiguration["Machine:uploadImageAPI"], inputJson);
            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    output = await JsonSerializer.DeserializeAsync<DownloadImageInput>(responseStream);
                }
            }

            return output;
        }



    }
}
