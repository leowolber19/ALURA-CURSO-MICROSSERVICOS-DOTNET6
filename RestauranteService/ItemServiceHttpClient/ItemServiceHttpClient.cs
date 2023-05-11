using RestauranteService.Dtos;
using System.Text;
using System.Text.Json;

namespace RestauranteService.IItemServiceHttpClient
{
    public class ItemServiceHttpClient : IItemServiceHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ItemServiceHttpClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            this._configuration = configuration;
        }

        public async void EnviaRestauranteParaItemService(RestauranteReadDto readDto)
        {
            var conteudo = new StringContent
                (
                    JsonSerializer.Serialize(readDto),
                    Encoding.UTF8,
                    "application/json"
                );

            await _httpClient.PostAsync(_configuration["ItemService"], conteudo);
        }
    }
}
