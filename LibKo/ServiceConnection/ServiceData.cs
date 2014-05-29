using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LibKo.ServiceConnection
{
    public class ServiceData
    {
        private static HttpClient _clientProperties = Settings.ClientProperties();

        public static HttpClient ClientProperties
        {
            get { return ServiceData._clientProperties; }
            set { ServiceData._clientProperties = value; }
        }

        private static List<HttpClient> _ClientList = new List<HttpClient>();

        public static List<HttpClient> ClientList
        {
            get { return ServiceData._ClientList; }
            set { ServiceData._ClientList = value; }
        }

        private static HttpClient _client = new HttpClient();

        public static HttpClient Client
        {
            get { return ServiceData._client; }
            set { ServiceData._client = value; }
        }
    }
}
