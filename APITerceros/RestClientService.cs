using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Text.Json;
using CommonServices;
using Models.APITerceros;

namespace APITerceros
{
    public interface IRestClientService
    {
        public void SetBaseUri(string baseUri);

        public void SetBearerToken(string token);
        public Task<RestClientResponse<T>> Get<T>(string partUri = "");
        public Task<RestClientResponse<T>> Post<T, V>(V body, EnumBodyType enumBodyType, string partUri = "");
        public Task<RestClientResponse<T>> Patch<T, V>(V body, EnumBodyType enumBodyType, string partUri);
    }

    public class RestClientService(IUtilsService utilsService) : IRestClientService
    {
        readonly HttpClient _httpClient = new();

        public void SetBaseUri(string baseUri)
        {
            _httpClient.BaseAddress = new Uri(baseUri);
        }

        public void SetBearerToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        }

        private HttpContent GetContent<T>(EnumBodyType enumBodyType, T body)
        {
            HttpContent? content = new StringContent("");
            switch (enumBodyType)
            {
                case EnumBodyType.ApplicationJson:
                    content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
                    break;
                case EnumBodyType.XWWWFormUrlEncoded:
                    if (body == null || body is not IEnumerable<KeyValuePair<string, string>>)
                    {
                        content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>());
                    }
                    else
                    {
                        content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)body);
                    }
                    break;
            }

            return content;
        }
        public async Task<RestClientResponse<T>> Get<T>(string partUri = "")
        {
            try
            {
                var response = new RestClientResponse<T>();
                var apiResponse = await _httpClient.GetAsync(partUri);

                if (apiResponse != null)
                {
                    response.Status = apiResponse.StatusCode;
                    if (response.Status == HttpStatusCode.OK)
                    {
                        response.Data = await apiResponse.Content.ReadFromJsonAsync<T>();
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<RestClientResponse<T>> Post<T, V>(V body, EnumBodyType enumBodyType, string partUri = "")
        {
            try
            {
                var response = new RestClientResponse<T>();
                HttpContent content = GetContent(enumBodyType, body);

                var apiResponse = await _httpClient.PostAsync(partUri, content);
                if (apiResponse != null)
                {
                    response.Status = apiResponse.StatusCode;
                    if (response.Status == HttpStatusCode.OK || response.Status == HttpStatusCode.Created)
                    {
                        string jsonResponse = await apiResponse.Content.ReadAsStringAsync();
                        if (jsonResponse != null && utilsService.IsStringValidJson(jsonResponse))
                        {
                            response.Data = JsonSerializer.Deserialize<T>(jsonResponse);
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<RestClientResponse<T>> Patch<T, V>(V body, EnumBodyType enumBodyType, string partUri)
        {
            try
            {
                var response = new RestClientResponse<T>();
                HttpContent content = GetContent(enumBodyType, body);

                var apiResponse = await _httpClient.PatchAsync(partUri, content);
                if (apiResponse != null)
                {
                    response.Status = apiResponse.StatusCode;
                    if (response.Status == HttpStatusCode.OK)
                    {
                        string jsonResponse = await apiResponse.Content.ReadAsStringAsync();
                        if (jsonResponse != null && utilsService.IsStringValidJson(jsonResponse))
                        {
                            response.Data = JsonSerializer.Deserialize<T>(jsonResponse);
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
