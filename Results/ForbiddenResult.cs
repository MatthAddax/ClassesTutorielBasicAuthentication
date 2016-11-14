using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebAPI.Results
{
    public class ForbiddenResult : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;
        private readonly string _reason;

        public ForbiddenResult(string reason, HttpRequestMessage request)
        {
            _request = request;
            _reason = reason;
        }

        public ForbiddenResult(HttpRequestMessage request)
        {
            _request = request;
            _reason = "Forbidden";
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = _request.CreateResponse(HttpStatusCode.Forbidden, "Forbidden");
            response.ReasonPhrase = _reason;
            return Task.FromResult(response);
        }
    }
}