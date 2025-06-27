using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tienda.Microservicios.Autor.Api.Aplicacion;

namespace Tienda.Microservicios.Autor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AutorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/autor
        [HttpPost]
        public async Task<ActionResult> Crear([FromBody] Nuevo.Ejecuta data)
        {
            await _mediator.Send(data);
            return Ok();
        }

        // GET: api/autor
        [HttpGet]
        public async Task<ActionResult<List<AutorDto>>> GetAutores()
        {
            var autores = await _mediator.Send(new Consulta.ListaAutor());
            return Ok(autores);
        }

        // GET: api/autor/{guid}
        [HttpGet("{guid}")]
        public async Task<ActionResult<AutorDto>> GetAutorPorGuid(string guid)
        {
            try
            {
                var autor = await _mediator.Send(new ConsultarFiltro.AutorUnico { AutorGuid = guid });

                if (autor == null)
                    return NotFound();

                return Ok(autor);
            }
            catch (Exception ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }

        // GET: api/autor/nombre/{nombre}
        [HttpGet("nombre/{nombre}")]
        public async Task<ActionResult<AutorDto>> GetAutorPorNombre(string nombre)
        {
            try
            {
                var autor = await _mediator.Send(new ConsultarNombre.AutorPorNombre { Nombre = nombre });

                if (autor == null)
                    return NotFound();

                return Ok(autor);
            }
            catch (Exception ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }

        [HttpPut("{guid}")]
        public async Task<ActionResult<Unit>> ActualizarAutor(string guid, [FromBody] Actualizar.AutorActualizar data)
        {
            data.AutorLibroGuid = guid;
            return await _mediator.Send(data);
        }




        // DELETE: api/autor/{guid}
        [HttpDelete("{guid}")]
        public async Task<ActionResult> Eliminar(string guid)
        {
            try
            {
                await _mediator.Send(new Eliminar.Ejecuta { AutorLibroGuid = guid });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
