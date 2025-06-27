using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tienda.Microservicios.Autor.Api.Modelo;
using Tienda.Microservicios.Autor.Api.Persistencia;

namespace Tienda.Microservicios.Autor.Api.Aplicacion
{
    public class ConsultarNombre
    {
        // Petición para buscar un autor por nombre
        public class AutorPorNombre : IRequest<AutorDto>
        {
            public string Nombre { get; set; }
        }

        // Manejador de la petición
        public class Manejador : IRequestHandler<AutorPorNombre, AutorDto>
        {
            private readonly ContextoAutor _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoAutor context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<AutorDto> Handle(AutorPorNombre request, CancellationToken cancellationToken)
            {
                var autor = await _context.AutorLibro
                    .FirstOrDefaultAsync(p => p.Nombre.ToLower() == request.Nombre.ToLower(), cancellationToken);

                if (autor == null)
                {
                    throw new Exception("No se encontró el autor");
                }

                var autorDto = _mapper.Map<AutorDto>(autor);

                return autorDto;
            }
        }
    }
}
