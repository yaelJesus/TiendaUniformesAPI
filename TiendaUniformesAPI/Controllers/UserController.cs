using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaUniformesAPI.Models;

namespace TiendaUniformesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TiendaUniformesContext _dbContext;
        private readonly ILogger<UserController> _logger;
        public UserController(TiendaUniformesContext dbContext, ILogger<UserController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpPost("Register")]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register(User request)
        {
            _logger.LogInformation(nameof(Register));
            BaseResponse response = new BaseResponse
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                if (string.IsNullOrEmpty(request.Email) && string.IsNullOrEmpty(request.Pass))
                    response.Errors.Add("Verifica los datos antes de continuar.");
                else
                {
                    string hashPass = BCrypt.Net.BCrypt.HashPassword(request.Pass);
                    bool lenght = request.Pass.Length > 6;
                    if (lenght)
                        response.Errors.Add("La contraseña requiere mínimo 6 caracteres.");
                    else
                    {
                        bool exist = await _dbContext.Users.AsNoTracking().AnyAsync(x => x.Email == request.Email || x.UserName == request.UserName);
                        if (exist)
                            response.Errors.Add("El usuario ya existe");
                        else
                        {
                            User newUser = new User()
                            {
                                UserName = request.UserName,
                                Email = request.Email,
                                Pass = hashPass,
                                IsActive = true
                            };

                            await _dbContext.Users.AddAsync(newUser);
                            await _dbContext.SaveChangesAsync();

                            response.Status = StatusCodes.Status200OK;
                            response.Title = "Creación exitosa";
                        }
                    }
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

        [HttpPost("Login")]
        [ProducesResponseType(typeof(ApiResponse<User>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(User request)
        {
            _logger.LogInformation($"{nameof(Login)} login");
            ApiResponse<User> response = new ApiResponse<User>
            {
                Errors = new List<string>(),
                Status = StatusCodes.Status400BadRequest,
                Data = new User()
            };
            try
            {
                var loginUser = await _dbContext.Users.AsNoTracking()
                    .Where(x => x.Email == request.Email && x.IsActive)
                    .Select(x => new User()
                    {
                        IdU = x.IdU,
                        UserName = x.UserName,
                        Email = x.Email,
                        Pass = x.Pass,
                        IsActive = x.IsActive
                    })
                    .FirstOrDefaultAsync();
                if (loginUser == null)
                    response.Errors.Add("Usuario o contraseña inválidos");
                else
                {
                    response.Data.Pass = string.Empty;
                    response.Data = loginUser;
                    response.Title = "Inicio de sesión éxitoso";
                    response.Status = StatusCodes.Status200OK;
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
                _logger.LogError($"Error Login: {ex.Message}");
            }
            return StatusCode(response.Status, response);
        }

        [HttpPost("UpdateUser")]
        [ProducesResponseType(typeof(ApiResponse<User>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUser(User request)
        {
            BaseResponse response = new BaseResponse
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                var entity = await _dbContext.Users.FirstOrDefaultAsync(x => x.IdU == request.IdU);
                if (entity != null)
                {
                    entity.IdU = request.IdU;
                    entity.UserName = request.UserName;
                    entity.Email = request.Email;

                    _dbContext.Users.Update(entity);
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
                _logger.LogError($"Error Login: {ex.Message}");
            }
            return StatusCode(response.Status, response);
        }

        [HttpPost("DeleteUser")]
        [ProducesResponseType(typeof(ApiResponse<User>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUser(int idU)
        {
            BaseResponse response = new BaseResponse()
            {
                Status = StatusCodes.Status400BadRequest,
                Errors = new List<string>()
            };
            try
            {
                var row = await _dbContext.Users.FindAsync(idU);
                if (row == null)
                    response.Errors.Add("No se encontró la entidad con el ID proporcionado.");
                else
                {
                    row.IsActive = false;
                    _dbContext.Users.Update(row);
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
                _logger.LogError($"Error Login: {ex.Message}");
            }
            return StatusCode(response.Status, response);
        }
    }
}
