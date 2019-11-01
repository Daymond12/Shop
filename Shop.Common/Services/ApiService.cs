
namespace Shop.Common.Services
{
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class ApiService
    {//Es un método asincrono Task, porque eso es de comunicaciones,
        //servicePrefix es /api
        //controller es /product
        public async Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller)
        {
            //está en un Try porque en cualquier momento revienta
            //puede ser por datos o por falla en la conexión a la INt
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };
                //concatenación de los parámetros para la url
                var url = $"{servicePrefix}{controller}";
                //trame lo que hay en allá
                var response = await client.GetAsync(url);
                //leeme la respuesta
                var result = await response.Content.ReadAsStringAsync();

                //si no funciona, es decir status 200 ok
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        //no pude
                        IsSuccess = false,
                        //dice porque no pudo
                        Message = result,
                    };
                }

                //deserializa el enorme string del <T> y lo vuelve un Json
                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }

}
