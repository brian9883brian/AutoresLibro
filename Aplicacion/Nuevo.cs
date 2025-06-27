using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Tienda.Microservicios.Autor.Api.Modelo;
using Tienda.Microservicios.Autor.Api.Persistencia;

namespace Tienda.Microservicios.Autor.Api.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public DateTime? FechaNacimiento { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
                RuleFor(x => x.FechaNacimiento).NotNull();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly ContextoAutor _contexto;

            public Manejador(ContextoAutor contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                try
                {
                    var autorLibro = new AutorLibro
                    {
                        Nombre = request.Nombre,
                        Apellido = request.Apellido,
                        FechaNacimiento = request.FechaNacimiento!.Value,
                        AutorLibroGuid = Guid.NewGuid().ToString()
                    };

                    _contexto.AutorLibro.Add(autorLibro);

                    var respuesta = await _contexto.SaveChangesAsync(cancellationToken);

                    if (respuesta > 0)
                    {
                        return Unit.Value;
                    }

                    throw new Exception("No se pudo insertar el autor libro en la base de datos");
                }
                catch (Exception ex)
                {
                    // Aquí puedes loguear el error o manejarlo según tu política
                    throw new Exception($"Error al insertar autor: {ex.Message}", ex);
                }
            }
        }
    }
}
