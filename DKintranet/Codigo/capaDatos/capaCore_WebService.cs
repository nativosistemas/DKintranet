using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using DKintranet.Codigo.clases;


namespace DKintranet.Codigo.capaDatos
{
    public class capaCore_WebService
    {
        private static readonly HttpClient client = new HttpClient();
        private static string url = System.Configuration.ConfigurationManager.AppSettings["url_DKcore"];
        private static JsonSerializerOptions oJsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
       // private static JsonSerializerOptions oJsonSerializerOptions_2 = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy. };
        private static string _pass { get; set; }
        private static string _login { get; set; }
        public static void setDatosLogin(string login, string pass)
        {
            _login = login;
            _pass = pass;
            var t = Task.Run(() => SetAuthorization());
        }
        public static async Task SetAuthorization()
        {
            if (_login != null && _pass != null)
            {
                try
                {
                    string resultResponse = await authenticate(_login, _pass);
                    DKbase.Models.AuthenticateResponse result = JsonSerializer.Deserialize<DKbase.Models.AuthenticateResponse>(resultResponse, oJsonSerializerOptions);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);
                }
                catch (Exception ex)
                {
                    FuncionesPersonalizadas.grabarLog(MethodBase.GetCurrentMethod(), ex, DateTime.Now);
                }
            }
        }
        private static async Task<HttpResponseMessage> PostAsync(string name, object pParameter, bool isRepeatBecauseNotAuthorized = true)
        {
            try
            {
                string url_api = url + name;
                var myContent = JsonSerializer.Serialize(pParameter);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync(url_api, byteContent);
                if (response.IsSuccessStatusCode)
                    return response;
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    DKbase.generales.Log.LogError(MethodBase.GetCurrentMethod(), "StatusCode == HttpStatusCode.Unauthorized", DateTime.Now, name, pParameter);
                    if (isRepeatBecauseNotAuthorized)
                    {
                        await SetAuthorization();
                        return await PostAsync(name, pParameter, false);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                FuncionesPersonalizadas.grabarLog(MethodBase.GetCurrentMethod(), ex, DateTime.Now, name, pParameter);
                return null;
            }
        }
        public static async Task<string> authenticate(string pLogin, string pPass)
        {
            string result = null;
            string name = @"Authenticate";
            DKbase.Models.AuthenticateRequest parameter = new DKbase.Models.AuthenticateRequest() { login = pLogin, pass = pPass };
            HttpResponseMessage response = await PostAsync(name, parameter, false);
            if (response != null)
            {
                var resultResponse = response.Content.ReadAsStringAsync().Result;
                return resultResponse;
            }
            return result;
        }
        //public static async Task<DKbase.dll.cDllPedido> TomarPedidoConIdCarritoAsync_Get(int pIdCarrito, string pLoginCliente, string pIdSucursal, string pMensajeEnFactura, string pMensajeEnRemito, string pTipoEnvio, List<DKbase.dll.cDllProductosAndCantidad> pListaProducto, bool pIsUrgente)
        //{
        //    try
        //    {
        //        string name = "TomarPedidoConIdCarrito";
        //        string url_api = url + name;
        //        capaCAR.InicioCarritoEnProceso(pIdCarrito, Constantes.cAccionCarrito_TOMAR);
        //        DKbase.dll.cDllPedido product = null;
        //        HttpResponseMessage response = await client.GetAsync(url_api);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            string data = await response.Content.ReadAsStringAsync();
        //            product = JsonSerializer.Deserialize<DKbase.dll.cDllPedido>(data);
        //        }
        //        return product;
        //    }
        //    catch (Exception ex)
        //    {
        //        FuncionesPersonalizadas.grabarLog(MethodBase.GetCurrentMethod(), ex, DateTime.Now, pIdCarrito, pLoginCliente, pIdSucursal, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, pListaProducto, pIsUrgente);
        //        return null;
        //    }
        //    finally
        //    {
        //        capaCAR.EndCarritoEnProceso(pIdCarrito);
        //    }
        //}
        //  
        public static async Task<DKbase.dll.cDllPedido> TomarPedidoConIdCarritoAsync(int pIdCarrito, string pLoginCliente, string pIdSucursal, string pMensajeEnFactura, string pMensajeEnRemito, string pTipoEnvio, List<DKbase.dll.cDllProductosAndCantidad> pListaProducto, bool pIsUrgente)
        {
            DKbase.dll.cDllPedido result = null;
            string name = "TomarPedidoConIdCarrito";
            DKbase.Models.TomarPedidoConIdCarritoRequest parameter = new DKbase.Models.TomarPedidoConIdCarritoRequest() { pIdCarrito = pIdCarrito, pLoginCliente = pLoginCliente, pIdSucursal = pIdSucursal, pMensajeEnFactura = pMensajeEnFactura, pMensajeEnRemito = pMensajeEnRemito, pTipoEnvio = pTipoEnvio, pListaProducto = pListaProducto, pIsUrgente = pIsUrgente };
            HttpResponseMessage response = await PostAsync(name, parameter);
            if (response != null)
            {
                var resultResponse = response.Content.ReadAsStringAsync().Result;
                result = JsonSerializer.Deserialize<DKbase.dll.cDllPedido>(resultResponse, oJsonSerializerOptions);
            }
            return result;
        }
        public static async Task<DKbase.dll.cDllPedido> TomarPedidoConIdCarritoIntranetAsync(int pIdCarrito, string pLoginCliente, string pIdSucursal, string pMensajeEnFactura, string pMensajeEnRemito, string pTipoEnvio, List<DKbase.dll.cDllProductosAndCantidad> pListaProducto, bool pIsUrgente)
        {
            DKbase.dll.cDllPedido result = null;
            string name = "TomarPedidoConIdCarritoIntranet";
            DKbase.Models.TomarPedidoConIdCarritoRequest parameter = new DKbase.Models.TomarPedidoConIdCarritoRequest() { pIdCarrito = pIdCarrito, pLoginCliente = pLoginCliente, pIdSucursal = pIdSucursal, pMensajeEnFactura = pMensajeEnFactura, pMensajeEnRemito = pMensajeEnRemito, pTipoEnvio = pTipoEnvio, pListaProducto = pListaProducto, pIsUrgente = pIsUrgente };
            HttpResponseMessage response = await PostAsync(name, parameter);
            if (response != null)
            {
                var resultResponse = response.Content.ReadAsStringAsync().Result;
                result = JsonSerializer.Deserialize<DKbase.dll.cDllPedido>(resultResponse);
                //result = JsonSerializer.Deserialize<DKbase.dll.cDllPedido>(resultResponse, oJsonSerializerOptions);
            }
            return result;
        }
    }
}