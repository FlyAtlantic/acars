using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acars.FlightData
{
    public class VatsimProxyApi
    {
        const string BaseUrl = "https://vatsim-status-proxy.herokuapp.com";

        public VatsimProxyApi() { }

        public T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = new System.Uri(BaseUrl);
            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                var crap = new ApplicationException(message, response.ErrorException);
                throw crap;
            }
            return response.Data;
        }

        public VatsimProxyClients GetClientByCid(string CID)
        {
            var request = new RestRequest();
            request.Resource = "clients";
            request.RootElement = "VatsimProxyClients";

            request.AddParameter("where", "{\"cid\":" + CID + "}");

            VatsimProxyClients res = Execute<VatsimProxyClients>(request);

            if (res.Clients.Count > 0)
            {
                Console.WriteLine("id: {0}", res.Clients[0].id);
                Console.WriteLine("etag {0}", res.Clients[0].etag);
                Console.WriteLine("location {0} {1}", res.Clients[0].location[0], res.Clients[0].location[1]);
            }

            return res;
        }

        public class VatsimProxyClients
        {
            [DeserializeAs(Name = "_items")]
            public List<VatsimProxyClient> Clients { get; set; }
        }

        public class VatsimProxyClient
        {
            public string callsign { get; set; }
            public string clienttype { get; set; }
            [DeserializeAs(Name = "_id")]
            public string id { get; set; }
            [DeserializeAs(Name = "_etag")]
            public string etag { get; set; }
            public double[] location { get; set; }
        }
    }
}
