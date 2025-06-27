using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tienda.Microservicios.Autor.Api.Modelo;
using Tienda.Microservicios.Autor.Api.Persistencia;

namespace Tienda.Microservicios.Autor.Api.Aplicacion
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public string AutorLibroGuid { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly ContextoAutor _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoAutor context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var autor = await _context.AutorLibro.FirstOrDefaultAsync(a => a.AutorLibroGuid == request.AutorLibroGuid, cancellationToken);

                if (autor == null)
                    throw new Exception("No se encontró el autor");

                _context.AutorLibro.Remove(autor);

                var result = await _context.SaveChangesAsync(cancellationToken);

                if (result > 0)
                    return Unit.Value;

                throw new Exception("No se pudo eliminar el autor");
            }
        }
    }
}
