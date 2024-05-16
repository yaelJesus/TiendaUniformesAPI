﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaUniformesAPI.Models;

namespace TiendaUniformesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly TiendaUniformesContext _dbContext;
        public SizeController(TiendaUniformesContext dbContext) { _dbContext = dbContext; }

        [HttpPost("CreateUserSize")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateUserSize(Size request)
        {
            BaseResponse response = new BaseResponse
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                if (request.Size1 <= 0 || request.Price <= 0)
                {
                    response.Errors.Add("El tamaño y el precio deben ser mayores que cero.");
                    return StatusCode(response.Status, response);
                }

                Size newSize = new Size
                {
                    IsActive = true,
                    Size1 = request.Size1,
                    Price = request.Price,
                    CreateUser = request.CreateUser,
                    CreateDate = DateOnly.FromDateTime(DateTime.Now)
                };
                _dbContext.Sizes.Add(newSize);
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

        [HttpPost("UpdateUserSize")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUserSize(Size request)
        {
            BaseResponse response = new BaseResponse()
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                var entity = _dbContext.Sizes.FirstOrDefault(x => x.IdS == request.IdS);
                if (entity != null)
                {
                    entity.IdS = request.IdS;
                    entity.IsActive = request.IsActive;
                    entity.Size1 = request.Size1;
                    entity.Price = request.Price;
                    entity.ModifyUser = request.ModifyUser;
                    entity.ModifyDate = DateOnly.FromDateTime(DateTime.Now);

                    _dbContext.Sizes.Update(entity);
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

        [HttpPost("DeleteUserSize")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUserSize(int idS)
        {
            BaseResponse response = new BaseResponse()
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                var row = await _dbContext.Sizes.FindAsync(idS);
                if (row == null)
                {
                    response.Errors.Add("No se encontró la entidad con el ID proporcionado.");
                    return StatusCode(response.Status, response);
                }

                row.IsActive = false;
                _dbContext.Sizes.Update(row);
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

        [HttpGet("GetUserSizes")]
        [ProducesResponseType(typeof(ApiResponse<List<Size>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserSizes(int IdU)
        {
            ApiResponse<List<Size>> response = new ApiResponse<List<Size>>()
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                List<Size> sizes = new List<Size>();
                sizes = await _dbContext.Sizes
                        .AsNoTracking()
                        .Where(x => x.CreateUser == IdU && x.IsActive)
                        .Select(x => new Size
                        {
                            IdS = x.IdS,
                            Size1 = x.Size1,
                            Price = x.Price,
                            CreateUser = x.CreateUser
                        })
                        .ToListAsync();
                response.Data = sizes;
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