﻿using Microsoft.EntityFrameworkCore;
using Tienda.Microservicios.Autor.Api.Modelo;

namespace Tienda.Microservicios.Autor.Api.Persistencia
{
    public class ContextoAutor : DbContext
    {
        public ContextoAutor(DbContextOptions<ContextoAutor> options) : base(options)
        {

        }

        public DbSet<AutorLibro> AutorLibro { get; set; }

        public DbSet<GradoAcademico> GradoAcademico { get; set; }
    }

}
