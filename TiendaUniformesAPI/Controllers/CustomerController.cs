using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaUniformesAPI.Models;

namespace TiendaUniformesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly TiendaUniformesContext _dbContext;
        public CustomerController(TiendaUniformesContext dbContext) { _dbContext = dbContext; }

        [HttpPost("CreateCustomer")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCustomer(Customer request)
        {
            BaseResponse response = new BaseResponse
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                if (string.IsNullOrEmpty(request.Name) && string.IsNullOrEmpty(request.Phone))
                {
                    response.Errors.Add("Ninguno de los campos pueden quedar vacios.");
                    return StatusCode(response.Status, response);
                }

                Customer newCustomer = new Customer
                {
                    IsActive = true,
                    Name = request.Name,
                    Phone = request.Phone,
                    CreateUser = request.CreateUser,
                    CreateDate = DateOnly.FromDateTime(DateTime.Now)
                };
                _dbContext.Customers.Add(newCustomer);
                await _dbContext.SaveChangesAsync();

                response.Status = StatusCodes.Status200OK;
                response.Title = "Creación exitosa";
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


        [HttpPost("UpdateCustomer")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCustomer(Customer request)
        {
            BaseResponse response = new BaseResponse()
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                var entity = _dbContext.Customers.FirstOrDefault(x => x.IdC == request.IdC);
                if (entity != null)
                {
                    entity.IdC = request.IdC;
                    entity.IsActive = request.IsActive;
                    entity.Name = request.Name;
                    entity.Phone = request.Phone;
                    entity.ModifyUser = request.ModifyUser;
                    entity.ModifyDate = DateOnly.FromDateTime(DateTime.Now);

                    _dbContext.Customers.Update(entity);
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

        [HttpPost("DeleteCustomer")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCustomer(int idC)
        {
            BaseResponse response = new BaseResponse()
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                var row = await _dbContext.Customers.FindAsync(idC);
                if (row == null)
                {
                    response.Errors.Add("No se encontró la entidad con el ID proporcionado.");
                    return StatusCode(response.Status, response);
                }

                row.IsActive = false;
                _dbContext.Customers.Update(row);
                await _dbContext.SaveChangesAsync();

                response.Status = StatusCodes.Status200OK;
                response.Title = "Eliminación  éxitosa";
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

        [HttpGet("GetCustomer")]
        [ProducesResponseType(typeof(ApiResponse<List<Size>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomer(int IdU)
        {
            ApiResponse<List<Customer>> response = new ApiResponse<List<Customer>>()
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                List<Customer> customers = new List<Customer>();
                customers = await _dbContext.Customers
                        .AsNoTracking()
                        .Where(x => x.CreateUser == IdU && x.IsActive)
                        .Select(x => new Customer
                        {
                            IdC = x.IdC,
                            Name = x.Name,
                            Phone = x.Phone,
                            CreateUser = x.CreateUser,
                            CreateDate = x.CreateDate,
                            IsActive = x.IsActive
                        })
                        .ToListAsync();
                response.Data = customers;
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