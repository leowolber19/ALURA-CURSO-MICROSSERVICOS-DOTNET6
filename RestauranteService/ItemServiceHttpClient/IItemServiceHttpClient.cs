using RestauranteService.Dtos;

namespace RestauranteService.IItemServiceHttpClient
{
    public interface IItemServiceHttpClient
    {
        public void EnviaRestauranteParaItemService(RestauranteReadDto readDto);
    }
}
