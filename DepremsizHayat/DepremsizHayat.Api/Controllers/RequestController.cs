using DepremsizHayat.Business.IService;
using DepremsizHayat.DTO.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace DepremsizHayat.Api.Controllers
{
    public class RequestController : ApiController
    {
        private IAnalyseRequestService _analyseRequestService;
        public RequestController(IAnalyseRequestService analyseRequestService)
        {
            this._analyseRequestService = analyseRequestService;
        }
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public MyAnalyseRequest Get(string code)
        {
            return _analyseRequestService.GetRequestByUniqueCode(code);
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
