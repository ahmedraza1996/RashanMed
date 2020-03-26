using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RMDS.Controllers.apiControllers
{
    public class HomeController : ApiController
    {

        public HttpResponseMessage GetRiderDetail(Object User)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(Donation));

        }

    }
}
