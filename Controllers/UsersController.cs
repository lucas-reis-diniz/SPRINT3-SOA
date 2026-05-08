using CarePlus.MindfulnessAPI.Models.DTOs;
using CarePlus.MindfulnessAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarePlus.MindfulnessAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna todos os usuários cadastrados.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserResponseDTO>>>> GetAll()
    {
        var users = await _service.GetAllAsync();
        return Ok(new ApiResponse<IEnumerable<UserResponseDTO>>(true, "Usuários listados com sucesso.", users));
    }

    /// <summary>
    /// Retorna um usuário pelo ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<UserResponseDTO>>> GetById(Guid id)
    {
        var user = await _service.GetByIdAsync(id);
        if (user == null)
            return NotFound(new ApiErrorResponse(false, $"Usuário com ID '{id}' não encontrado.", null));

        return Ok(new ApiResponse<UserResponseDTO>(true, "Usuário encontrado.", user));
    }

    /// <summary>
    /// Cadastra um novo usuário.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<UserResponseDTO>>> Create([FromBody] UserCreateDTO dto)
    {
        try
        {
            var user = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id },
                new ApiResponse<UserResponseDTO>(true, "Usuário criado com sucesso.", user));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiErrorResponse(false, ex.Message, null));
        }
    }

    /// <summary>
    /// Atualiza um usuário existente.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<UserResponseDTO>>> Update(Guid id, [FromBody] UserUpdateDTO dto)
    {
        var user = await _service.UpdateAsync(id, dto);
        if (user == null)
            return NotFound(new ApiErrorResponse(false, $"Usuário com ID '{id}' não encontrado.", null));

        return Ok(new ApiResponse<UserResponseDTO>(true, "Usuário atualizado com sucesso.", user));
    }

    /// <summary>
    /// Remove um usuário pelo ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiErrorResponse>> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound(new ApiErrorResponse(false, $"Usuário com ID '{id}' não encontrado.", null));

        return Ok(new ApiErrorResponse(true, "Usuário removido com sucesso.", null));
    }
}
