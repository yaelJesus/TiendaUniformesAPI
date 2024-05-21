using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaUniformesAPI.Models;

namespace TiendaUniformesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GarmentController : ControllerBase
    {
        private readonly TiendaUniformesContext _dbContext;
        public GarmentController(TiendaUniformesContext dbContext) { _dbContext = dbContext; }

        [HttpPost("CreateGarment")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateGarment(Garment request)
        {
            BaseResponse response = new BaseResponse
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                if (string.IsNullOrEmpty(request.Type) && request.IdS >= 0 && request.IdSc >= 0)
                    response.Errors.Add("Ninguno de los campos puede quedar vacio.");
                else
                {
                    Garment newGarment = new Garment
                    {
                        IsActive = true,
                        Type = request.Type,
                        Desctiption = request.Desctiption,
                        IdS = request.IdS,
                        IdSc = request.IdSc,
                        CreateUser = request.CreateUser,
                        CreateDate = DateOnly.FromDateTime(DateTime.Now)
                    };
                    _dbContext.Garments.Add(newGarment);
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


        [HttpPost("UpdateGarment")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateGarment(Garment request)
        {
            BaseResponse response = new BaseResponse()
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                var entity = _dbContext.Garments.FirstOrDefault(x => x.IdG == request.IdG);
                if (entity != null)
                {
                    entity.IdG = request.IdG;
                    entity.IsActive = request.IsActive;
                    entity.Type = request.Type;
                    entity.Desctiption = request.Desctiption;
                    entity.IdS = request.IdS;
                    entity.IdSc = request.IdSc;
                    entity.ModifyUser = request.ModifyUser;
                    entity.ModifyDate = DateOnly.FromDateTime(DateTime.Now);

                    _dbContext.Garments.Update(entity);
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

        [HttpPost("DeleteGarment")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteGarment(int idG)
        {
            BaseResponse response = new BaseResponse()
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                var row = await _dbContext.Garments.FindAsync(idG);
                if (row == null)
                    response.Errors.Add("No se encontró la entidad con el ID proporcionado.");
                else
                {
                    row.IsActive = false;
                    _dbContext.Garments.Update(row);
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

        [HttpGet("GetGarment")]
        [ProducesResponseType(typeof(ApiResponse<List<Size>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGarment(int IdU)
        {
            ApiResponse<List<Garment>> response = new ApiResponse<List<Garment>>()
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                List<Garment> Garments = new List<Garment>();
                Garments = await _dbContext.Garments
                        .AsNoTracking()
                        .Where(x => x.CreateUser == IdU && x.IsActive)
                        .Select(x => new Garment
                        {
                            Type = x.Type,
                            Desctiption = x.Desctiption,
                            IdS = x.IdS,
                            IdSc = x.IdSc,
                            CreateUser = x.CreateUser,
                            CreateDate = x.CreateDate,
                            IsActive = x.IsActive
                        })
                        .ToListAsync();
                response.Data = Garments;
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