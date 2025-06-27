using System.Runtime.InteropServices;
using Tienda.Microservicios.Autor.Api.Modelo;
using AutoMapper;
namespace Tienda.Microservicios.Autor.Api.Aplicacion
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<AutorLibro, AutorDto>();

        }
    }
}
