using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Models.APITerceros
{
    public class RestClientResponse<T>
    {
        public HttpStatusCode Status { get; set; }
        public T? Data { get; set; }
    }
}
