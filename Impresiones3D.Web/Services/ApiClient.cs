using System.Text;
using System.Text.Json;

namespace Impresiones3D.Web.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ApiClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
            _httpClient.BaseAddress = new Uri(_config["ApiSettings:BaseUrl"]);
        }

        // GET: devuelve la data directamente (tu API devuelve ApiResponse<T>)
        public async Task<T?> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                var statusCode = response.StatusCode;
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error {statusCode} al llamar a {endpoint}. Detalle: {errorContent}");
            }
            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(json, options);
            if (apiResponse == null || !apiResponse.Success)
                throw new Exception($"La API respondió con error: {apiResponse?.Message ?? "Mensaje no disponible"}");
            return apiResponse.Data;
        }

        public async Task<byte[]?> GetByteArrayAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadAsByteArrayAsync();
        }

        // POST
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // Intentar deserializar como ApiResponse<object>
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    var mensaje = errorResponse?.Message ?? "Error desconocido";
                    throw new Exception(mensaje);
                }
                catch (JsonException)
                {
                    // Si no es JSON, usar el texto plano (puede ser HTML)
                    throw new Exception($"Error {response.StatusCode}: {json}");
                }
            }

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<TResponse>>(json, options);
            return apiResponse!.Success ? apiResponse.Data : default;
        }

        // PUT
        public async Task<bool> PutAsync<T>(string endpoint, T data)
        {
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(endpoint, content);
            var json = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    var mensaje = errorResponse?.Message ?? "Error desconocido al actualizar";
                    throw new Exception(mensaje);
                }
                catch (JsonException)
                {
                    throw new Exception($"Error {response.StatusCode}: {json}");
                }
            }
            return true;
        }

        //PATCH
        public async Task<bool> PatchAsync(string endpoint, object? queryParams = null)
        {
            var url = endpoint;
            if (queryParams != null)
            {
                var queryString = string.Join("&", queryParams.GetType().GetProperties()
                    .Select(p => $"{p.Name}={Uri.EscapeDataString(p.GetValue(queryParams)?.ToString() ?? "")}"));
                url += "?" + queryString;
            }
            var response = await _httpClient.PatchAsync(url, null);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    var mensaje = errorResponse?.Message ?? "Error desconocido";
                    throw new Exception(mensaje);
                }
                catch (JsonException)
                {
                    throw new Exception($"Error {response.StatusCode}: {content}");
                }
            }
            return true;
        }



        // DELETE
        public async Task<bool> DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    var mensaje = errorResponse?.Message ?? "Error desconocido al eliminar";
                    throw new Exception(mensaje);
                }
                catch (JsonException)
                {
                    throw new Exception($"Error {response.StatusCode}: {content}");
                }
            }
            return true;
        }




    }

    // Clase que coincide con la respuesta de tu API (la misma que en API/Responses/ApiResponse.cs)
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public T? Data { get; set; }
    }


}