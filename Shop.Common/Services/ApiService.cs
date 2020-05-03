
namespace Shop.Common.Services
{
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class ApiService
    {//Es un método asincrono Task, porque eso es de comunicaciones,
     //servicePrefix es /api
     //controller es /product

        //Este metodo se usa sin seguridad
        //trae una lista de productos de forma no segura    
        public async Task<Response> GetListAsync<T>(
            string urlBase, 
            string servicePrefix, 
            string controller)
        {
            //está en un Try porque en cualquier momento revienta
            //puede ser por datos o por falla en la conexión a la Int
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };
                //concatenación de los parámetros para la url
                var url = $"{servicePrefix}{controller}";
                //trame lo que hay allá
                var response = await client.GetAsync(url);
                //leeme la respuesta
                var result = await response.Content.ReadAsStringAsync();

                //si no funciona, es decir si el status no es 200 ok
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

        //para tomar en cuenta el token, sobrecargaremos el método
        //ambos métodos se llaman igual
        /*lo que cambia es el número de parámetros*/


        //Este otro metodo usa seguridad
        public async Task<Response> GetListAsync<T>(
        string urlBase,
        string servicePrefix,
        string controller,
        string tokenType,
        string accessToken)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase),
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);

                var url = $"{servicePrefix}{controller}";
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

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


        //MÉTODO PARA CONSEGUIR Y CONSUMIR EL TOKEN
    public async Task<Response> GetTokenAsync(
    string urlBase,
    string servicePrefix,
    string controller,
    TokenRequest request)
        {
            try
            {
                //al get con las sig dos lineas mandamos el body
                //serializar es coger un objeto y volverlo string, serializamos el request
                //utf-8 de nuestro lenguaje
                var requestString = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestString, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}";
                //acá hacemos el post
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var token = JsonConvert.DeserializeObject<TokenResponse>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = token
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

        //POST GENERICO
    public async Task<Response> PostAsync<T>(
    string urlBase,
    string servicePrefix,
    string controller,
    T model,
    string tokenType,
    string accessToken)
        {
            try
            {
                //lineas request y content  para serializar body
                var request = JsonConvert.SerializeObject(model, new JsonSerializerSettings {NullValueHandling =  NullValueHandling.Ignore });
               
                var content = new StringContent(request, Encoding.UTF8, "application/json");



                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                //header de autorización para consumo seguro
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                //armamo el metodo
                var url = $"{servicePrefix}{controller}";
                //dcimos que es un post
                var response = await client.PostAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }
                //si funciona deserializa el objeto T(producto)
                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }




        //HACER PUT

        public async Task<Response> PutAsync<T>(
    string urlBase,
    string servicePrefix,
    string controller,
    int id,
    T model,
    string tokenType,
    string accessToken)
        {
            
            try
            {
                
                

                var request = JsonConvert.SerializeObject(model);
             
               
                
               
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                //hacemos el llamado
                var url = $"{servicePrefix}{controller}/{id}";
                var response = await client.PutAsync(url, content);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                var obj = JsonConvert.DeserializeObject<T>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = obj,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }


        //DELETE
        public async Task<Response> DeleteAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            int id,
            string tokenType,
            string accessToken)
        {
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, accessToken);
                var url = $"{servicePrefix}{controller}/{id}";
                var response = await client.DeleteAsync(url);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                return new Response
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }


        string reemplazar ( Models.User numero)
        {
            var PhoneNumber = numero.PhoneNumber.Replace(" ", "");
            return PhoneNumber ;
        }

    }

   
}
