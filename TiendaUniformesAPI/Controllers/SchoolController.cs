using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaUniformesAPI.Models;

namespace TiendaUniformesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly TiendaUniformesContext _dbContext;
        public SchoolController(TiendaUniformesContext dbContext) { _dbContext = dbContext; }

        [HttpPost("CreateSchool")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateSchool(School request)
        {
            BaseResponse response = new BaseResponse
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                if (string.IsNullOrEmpty(request.Name))
                    response.Errors.Add("Ninguno de los campos puede quedar vacio.");
                else
                {
                    School newSchool = new School
                    {
                        IsActive = true,
                        Name = request.Name,
                        CreateUser = request.CreateUser,
                        CreateDate = DateOnly.FromDateTime(DateTime.Now)
                    };
                    _dbContext.Schools.Add(newSchool);
                    await _dbContext.SaveChangesAsync();

                    response.Status = StatusCodes.Status200OK;
                    response.Title = "Creación exitosa";
                }
            }
            catch (DbUpdateException)
            {
                response.Status = StatusCodes.Status500InternalServerError;
                response.Errors.Add("Error al guardar los datos en la base de datos.");
            }
            catch (Exception ex)
            {
                response.Status = StatusCodes.Status500InternalServerError;
                response.Errors.Add("Ocurrió un error, inténtalo de nuevo más tarde");
                response.Errors.Add(ex.Message);
            }
            return StatusCode(response.Status, response);
        }

        [HttpPost("UpdateSchool")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateSchool(School request)
        {
            BaseResponse response = new BaseResponse()
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                var entity = _dbContext.Schools.FirstOrDefault(x => x.IdSc == request.IdSc);
                if (entity != null)
                {
                    entity.Name = request.Name;
                    entity.ModifyUser = request.ModifyUser;
                    entity.ModifyDate = DateOnly.FromDateTime(DateTime.Now);

                    _dbContext.Schools.Update(entity);
                    await _dbContext.SaveChangesAsync();

                    response.Status = StatusCodes.Status200OK;
                    response.Title = "Actualización éxitosa";
                }
                else
                    response.Errors.Add("No se encontró la entidad para actualizar");
            }
            catch (DbUpdateException)
            {
                response.Status = StatusCodes.Status500InternalServerError;
                response.Errors.Add("Error al guardar los datos en la base de datos.");
            }
            catch (Exception ex)
            {
                response.Status = StatusCodes.Status500InternalServerError;
                response.Errors.Add("Ocurrió un error, inténtalo de nuevo más tarde");
                response.Errors.Add(ex.Message);
            }
            return StatusCode(response.Status, response);
        }

        [HttpPost("DeleteSchool")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteSchool(int idSc)
        {
            BaseResponse response = new BaseResponse()
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                var row = await _dbContext.Schools.FindAsync(idSc);
                if (row == null || !row.IsActive)
                    response.Errors.Add("No se encontró la entidad con el ID proporcionado.");
                else
                {
                    row.IsActive = false;
                    _dbContext.Schools.Update(row);
                    await _dbContext.SaveChangesAsync();

                    response.Status = StatusCodes.Status200OK;
                    response.Title = "Eliminación  éxitosa";
                }
            }
            catch (DbUpdateException)
            {
                response.Status = StatusCodes.Status500InternalServerError;
                response.Errors.Add("Error al guardar los datos en la base de datos.");
            }
            catch (Exception ex)
            {
                response.Status = StatusCodes.Status500InternalServerError;
                response.Errors.Add("Ocurrió un error, inténtalo de nuevo más tarde");
                response.Errors.Add(ex.Message);
            }
            return StatusCode(response.Status, response);
        }

        [HttpGet("GetSchool")]
        [ProducesResponseType(typeof(ApiResponse<List<School>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSchool(int IdU)
        {
            ApiResponse<List<School>> response = new ApiResponse<List<School>>()
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                List<School> schools = new List<School>();
                schools = await _dbContext.Schools
                        .AsNoTracking()
                        .Where(x => x.CreateUser == IdU && x.IsActive)
                        .Select(x => new School
                        {
                            IdSc = x.IdSc,
                            Name = x.Name
                        })
                        .ToListAsync();
                response.Data = schools;
                if (response.Data is not null && response.Data.Any())
                {
                    response.Status = StatusCodes.Status200OK;
                    response.Title = response.Data.Any() ? "Consulta Exitosa" : "Sin datos que mostrar";
                }
                else
                    response.Errors.Add("Campo fuera de los valores existentes");
            }
            catch (Exception ex)
            {
                response.Status = StatusCodes.Status500InternalServerError;
                response.Errors.Add("Ocurrió un error, inténtalo de nuevo más tarde");
                response.Errors.Add(ex.Message);
            }
            return StatusCode(response.Status, response);
        }
    }
}
