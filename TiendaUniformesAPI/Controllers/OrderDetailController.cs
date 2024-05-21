using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaUniformesAPI.Models;

namespace TiendaUniformesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly TiendaUniformesContext _dbContext;
        public OrderDetailController(TiendaUniformesContext dbContext) { _dbContext = dbContext; }

        [HttpPost("CreateOrderDetail")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateOrderDetail(OrderDetail request)
        {
            BaseResponse response = new BaseResponse
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                if (request.IdO >= 0 && request.IdG >= 0 && request.Quantitaty >= 0)
                    response.Errors.Add("Ninguno de los campos puede quedar vacio.");
                else
                {
                    OrderDetail newOrderDetail = new OrderDetail
                    {
                        IsActive = true,
                        IdO = request.IdO,
                        IdG = request.IdG,
                        Quantitaty = request.Quantitaty,
                        CreateUser = request.CreateUser,
                        CreateDate = DateOnly.FromDateTime(DateTime.Now)
                    };
                    _dbContext.OrderDetails.Add(newOrderDetail);
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


        [HttpPost("UpdateOrderDetail")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateOrderDetail(OrderDetail request)
        {
            BaseResponse response = new BaseResponse()
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                var entity = _dbContext.OrderDetails.FirstOrDefault(x => x.IdOd == request.IdOd);
                if (entity != null)
                {
                    entity.IdOd = request.IdOd;
                    entity.IsActive = request.IsActive;
                    entity.IdO = request.IdO;
                    entity.IdG = request.IdG;
                    entity.Quantitaty = request.Quantitaty;
                    entity.ModifyUser = request.ModifyUser;
                    entity.ModifyDate = DateOnly.FromDateTime(DateTime.Now);

                    _dbContext.OrderDetails.Update(entity);
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

        [HttpPost("DeleteOrderDetail")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteOrderDetail(int idOd)
        {
            BaseResponse response = new BaseResponse()
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                var row = await _dbContext.OrderDetails.FindAsync(idOd);
                if (row == null)
                    response.Errors.Add("No se encontró la entidad con el ID proporcionado.");
                else
                {
                    row.IsActive = false;
                    _dbContext.OrderDetails.Update(row);
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

        [HttpGet("GetOrderDetail")]
        [ProducesResponseType(typeof(ApiResponse<List<Size>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderDetail(int IdU)
        {
            ApiResponse<List<OrderDetail>> response = new ApiResponse<List<OrderDetail>>()
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                List<OrderDetail> OrderDetails = new List<OrderDetail>();
                OrderDetails = await _dbContext.OrderDetails
                        .AsNoTracking()
                        .Where(x => x.CreateUser == IdU && x.IsActive)
                        .Select(x => new OrderDetail
                        {
                            IdO = x.IdO,
                            IdG = x.IdG,
                            Quantitaty = x.Quantitaty,
                            CreateUser = x.CreateUser,
                            CreateDate = x.CreateDate,
                            IsActive = x.IsActive
                        })
                        .ToListAsync();
                response.Data = OrderDetails;
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