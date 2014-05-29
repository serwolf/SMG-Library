using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using LibKo.ServiceConnection;
namespace LibKo.WAPI
{
    public class WP<T> where T : new()
    {
        #region Constructor
        public WP()
        {
            T comodin = new T();
        }
        #endregion

        #region Get Methods
        public static List<T> GetList(Type tipo)
        {
            List<T> Lista = new List<T>();

            var url = tipo.Name;
            HttpResponseMessage response = ServiceData.ClientProperties.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var lista = response.Content.ReadAsAsync<IEnumerable<T>>().Result;
                Lista = lista.ToList();
            }

            return Lista;
        }

        public static List<T> GetList(String URI)
        {
            List<T> Lista = new List<T>();

            var url = URI;
            HttpResponseMessage response = ServiceData.ClientProperties.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var lista = response.Content.ReadAsAsync<IEnumerable<T>>().Result;
                Lista = lista.ToList();
            }

            return Lista;
        }

        public static List<T> GetList(Type tipo, String URI)
        {
            List<T> Lista = new List<T>();

            var url = tipo.Name + URI;
            HttpResponseMessage response = ServiceData.ClientProperties.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var lista = response.Content.ReadAsAsync<IEnumerable<T>>().Result;
                Lista = lista.ToList();
            }

            return Lista;
        }

        public static T Get(Type tipo, String URI)
        {
            T Lista = new T();

            var url = tipo.Name + URI;
            HttpResponseMessage response = ServiceData.ClientProperties.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var lista = response.Content.ReadAsAsync<T>().Result;
                Lista = lista;
            }

            return Lista;
        }

        public static T Get(Type tipo)
        {
            T Lista = new T();

            var url = tipo.Name;
            HttpResponseMessage response = ServiceData.ClientProperties.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var lista = response.Content.ReadAsAsync<T>().Result;
                Lista = lista;
            }

            return Lista;
        }

        public static T Get(String URI)
        {
            T Lista = new T();

            var url = URI;
            HttpResponseMessage response = ServiceData.ClientProperties.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var lista = response.Content.ReadAsAsync<T>().Result;
                Lista = lista;
            }

            return Lista;
        }
        #endregion

        #region Post Methods

        public static T Post<S>(S s) where S : new()
        {
            T ID = new T();
            try
            {
                var url = s.GetType().Name;
                var response = ServiceData.ClientProperties.PostAsJsonAsync(url, s).Result;

                if (response.IsSuccessStatusCode)
                {
                    var _ID = response.Content.ReadAsAsync<T>().Result;
                    ID = _ID;
                }

            }
            catch (HttpRequestException e)
            {
                ID = new T();
            }
            return ID;
        }

        public static T Post<S>(S s, String URI) where S : new()
        {
            T ID = new T();
            try
            {
                var url = s.GetType().Name + URI;
                var response = ServiceData.ClientProperties.PostAsJsonAsync(url, s).Result;

                if (response.IsSuccessStatusCode)
                {
                    var _ID = response.Content.ReadAsAsync<T>().Result;
                    ID = _ID;
                }

            }
            catch (HttpRequestException e)
            {
                ID = new T();
            }
            return ID;
        }

        public static T Post<S>(String URI, S s) where S : new()
        {
            T ID = new T();
            try
            {
                var url = URI;
                var response = ServiceData.ClientProperties.PostAsJsonAsync(url, s).Result;

                if (response.IsSuccessStatusCode)
                {
                    var _ID = response.Content.ReadAsAsync<T>().Result;
                    ID = _ID;
                }

            }
            catch (HttpRequestException e)
            {
                ID = new T();
            }
            return ID;
        }

        #endregion

        #region Put Methods
        public static T Put<S>(S s) where S : new()
        {
            T ID = new T();
            try
            {
                var url = s.GetType().Name;
                var response = ServiceData.ClientProperties.PutAsJsonAsync(url, s).Result;

                if (response.IsSuccessStatusCode)
                {
                    var _ID = response.Content.ReadAsAsync<T>().Result;
                    ID = _ID;
                }
            }
            catch (HttpRequestException e)
            {
                ID = new T();
            }
            return ID;
        }

        public static T Put<S>(S s, String URI) where S : new()
        {
            T ID = new T();
            try
            {
                var url = s.GetType().Name + URI;
                var response = ServiceData.ClientProperties.PutAsJsonAsync(url, s).Result;

                if (response.IsSuccessStatusCode)
                {
                    var _ID = response.Content.ReadAsAsync<T>().Result;
                    ID = _ID;
                }
            }
            catch (HttpRequestException e)
            {
                ID = new T();
            }
            return ID;
        }

        public static T Put<S>(String URI, S s) where S : new()
        {
            T ID = new T();
            try
            {
                var url = URI;
                var response = ServiceData.ClientProperties.PutAsJsonAsync(url, s).Result;

                if (response.IsSuccessStatusCode)
                {
                    var _ID = response.Content.ReadAsAsync<T>().Result;
                    ID = _ID;
                }
            }
            catch (HttpRequestException e)
            {
                ID = new T();
            }
            return ID;
        }


        #endregion

        #region Delete Methods
        public static S Delete<S>(String URI) where S : new()
        {
            S s = new S();
            try
            {

                var url = URI;
                var response = ServiceData.ClientProperties.DeleteAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    s = response.Content.ReadAsAsync<S>().Result;
                }
            }
            catch (HttpRequestException e)
            {
                s = new S();
            }
            return s;
        }
        public static Boolean Delete(String URI)
        {
            Boolean s = false;
            try
            {

                var url = URI;
                var response = ServiceData.ClientProperties.DeleteAsync(url).Result;

                s = response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                s = false;
            }
            return s;
        }
        #endregion

        #region Properties

        private int count = 0;

        public int Count
        {
            get { return count; }
            private set { count = value; }
        }

        private List<Object> _History = new List<Object>();

        public List<Object> History
        {
            get { return _History; }
            set { _History = value; }
        }

        #endregion
    }
}
