using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LibKo.ServiceConnection
{
    public class ServiceDataqw
    {
        private static HttpClient _clientProperties = Settings1.ClientProperties();

        public static HttpClient ClientProperties
        {
            get { return ServiceDataqw._clientProperties; }
            set { ServiceDataqw._clientProperties = value; }
        }

        private static List<HttpClient> _ClientList = new List<HttpClient>();

        public static List<HttpClient> ClientList
        {
            get { return ServiceDataqw._ClientList; }
            set { ServiceDataqw._ClientList = value; }
        }

        private static HttpClient _client = new HttpClient();

        public static HttpClient Client
        {
            get { return ServiceDataqw._client; }
            set { ServiceDataqw._client = value; }
        }
    }
}
