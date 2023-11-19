using Newtonsoft.Json;
using RataEmpanada.Interfaces;
using System.Text;
using Shared.DTOs;

namespace RataEmpanada.Repositories
{
    public class AccountRepository : IAccountRepository
    {        
        private static string _baseUrl = "";
        private static string _token = "";

        public AccountRepository()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            
            _baseUrl = builder.GetSection("ApiSetting:baseUrl").Value;
        }


        public async Task<string> LoginAsync(UserDto user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/Api/Auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    // Puedes realizar más acciones según sea necesario
                    return token;
                }
                else
                {
                    // Manejar el caso en el que la solicitud no fue exitosa
                    return null;
                }
            }
        }
    }
}
