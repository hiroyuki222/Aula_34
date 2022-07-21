using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StatusCode.Models;

namespace StatusCode.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private SistemaContext DbSistema = new SistemaContext();

        [HttpGet]
        public ActionResult<Usuario[]> RequererTodos()
        {
            return Ok(DbSistema.Usuario.ToArray());
        }

        [HttpGet("{Id}")]
        public ActionResult<Usuario> RequererUmPelaId(int Id)
        {
            Usuario? usuario = DbSistema.Usuario.Find(Id);
            if(usuario != null)
            {
                return Ok(usuario);
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult<Usuario> PublicarUm(Usuario Usuario)
        {   
            if(DbSistema.Usuario.Any(x => x.Cpf == Usuario.Cpf))
            {
                return Conflict();
            }
            DbSistema.Usuario.Add(Usuario);
            DbSistema.SaveChanges();
            return StatusCode(StatusCodes.Status201Created,Usuario);
        }

        [HttpDelete("{Id}")]
        public ActionResult<Usuario> DeletarUmPelaId(int Id, Usuario Usuario)
        {
            Usuario usuario = DbSistema.Usuario.Find(Id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Unauthorized();


        }

        [HttpPut("{Id}")]
        public ActionResult<Usuario> SubstituirUmPelaId(int Id, Usuario Usuario)
        {
            Usuario usuario = DbSistema.Usuario.Find(Id);
            if(usuario == null)
            {
                return NotFound();
            }
            if (DbSistema.Usuario.Any(x => x.Cpf == Usuario.Cpf))
            {
                return Conflict();
            }
            usuario.Nome = Usuario.Nome;
            usuario.Sobrenome = Usuario.Sobrenome;
            usuario.Cpf = Usuario.Cpf;  

            DbSistema.SaveChanges();
            return Ok(usuario);
        }
    }
}
