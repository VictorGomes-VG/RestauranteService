using AutoMapper;
using ItemService.Data;
using ItemService.Dtos;
using ItemService.Models;

namespace ItemService.EventProcessor
{
    public class ProcessaEvento : IProcessaEvento
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public ProcessaEvento(IMapper mapper, IServiceScopeFactory scopeFactory)
        {
            _mapper = mapper;
            _scopeFactory = scopeFactory;
        }

        public void Processa(string mensagem)
        {
            using var scope = _scopeFactory.CreateScope();

            var itemRepository = scope.ServiceProvider.GetRequiredService<IItemRepository>();

            var restauranteDto = _mapper.Map<RestauranteReadDto>(mensagem);

            var restaurante = _mapper.Map<Restaurante>(restauranteDto);

            if(!itemRepository.ExisteRestauranteExterno(restaurante.Id))
            {
                itemRepository.CreateRestaurante(restaurante);
                itemRepository.SaveChanges();
            }
        }
    }
}
