using CustomerSupport.Desktop.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CustomerSupport.Desktop.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private string? _authToken;

        //public ApiService(string baseUrl)
        //{
        //    _baseUrl = baseUrl;
        //    _httpClient = new HttpClient();
        //    _httpClient.BaseAddress = new Uri(baseUrl);
        //    _httpClient.DefaultRequestHeaders.Accept.Clear();
        //    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //}
        public ApiService(string baseUrl)
        {
            _baseUrl = baseUrl;

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        public void SetAuthToken(string token)
        {
            _authToken = token;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/auth/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return loginResponse ?? new LoginResponse { Success = false, Message = "Invalid response" };
            }
            catch (Exception ex)
            {
                return new LoginResponse { Success = false, Message = $"Connection error: {ex.Message}" };
            }
        }

        public async Task<List<Ticket>> GetTicketsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/tickets");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var tickets = JsonSerializer.Deserialize<List<Ticket>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return tickets ?? new List<Ticket>();
                }
                
                return new List<Ticket>();
            }
            catch
            {
                return new List<Ticket>();
            }
        }

        public async Task<Ticket?> GetTicketAsync(int ticketId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/tickets/{ticketId}");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var ticket = JsonSerializer.Deserialize<Ticket>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return ticket;
                }
                
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Ticket?> CreateTicketAsync(CreateTicketRequest request)
        {
            try
            {
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/tickets", content);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var ticket = JsonSerializer.Deserialize<Ticket>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return ticket;
                }
                
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateTicketAsync(int ticketId, object updateRequest)
        {
            try
            {
                var json = JsonSerializer.Serialize(updateRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"/api/tickets/{ticketId}", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<User>> GetAdminUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/users/admins");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var users = JsonSerializer.Deserialize<List<User>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return users ?? new List<User>();
                }
                
                return new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }
    }
}