using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SoccerWorldCupSweepstake.GUI.Models;

namespace SoccerWorldCupSweepstake.GUI.Controllers {
   [Route("api/[controller]")]
   public class SmartContractController : Controller {
      private SmartContractConfiguration _configuration;
      private static HttpClient _httpClient;

      static SmartContractController() {
         _httpClient = new HttpClient(new HttpClientHandler { UseCookies = false });
      }


      public SmartContractController(IOptions<SmartContractConfiguration> configuration) {
         this._configuration = configuration.Value;
      }

      [HttpGet("[action]")]
      public IEnumerable<string> Teams() {
         return GetStorage<string>("TeamsCsv", SmartContractStorageDataType.String).Result?.Split(",");
      }


      private Task<TResult> GetStorage<TResult>(string StorageKey, SmartContractStorageDataType DataType) {
         var url = $"{_configuration.ApiUrl}/storage?ContractAddress={_configuration.SmartContractAddress}&StorageKey={StorageKey}&DataType={DataType}";

         var response = _httpClient.GetAsync(url)
            .ContinueWith<TResult>(currentTask => {
               string jsonString = currentTask.Result.Content.ReadAsStringAsync().Result;
               return JsonConvert.DeserializeObject<TResult>(jsonString);
            });
         return response;
      }
   }
}
