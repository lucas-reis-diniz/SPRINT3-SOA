using CarePlus.MindfulnessAPI.Models.DTOs;
using CarePlus.MindfulnessAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarePlus.MindfulnessAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WellnessContentsController : ControllerBase
{
    private readonly IWellnessContentService _service;

    public WellnessContentsController(IWellnessContentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ContentResponseDTO>>>> GetAll()
    {
        var contents = await _service.GetAllAsync();
        return Ok(new ApiResponse<IEnumerable<ContentResponseDTO>>(true, "Conteúdos listados com sucesso.", contents));
    }

    [HttpGet("active")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ContentResponseDTO>>>> GetActive()
    {
        var contents = await _service.GetActiveAsync();
        return Ok(new ApiResponse<IEnumerable<ContentResponseDTO>>(true, "Conteúdos ativos listados com sucesso.", contents));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<ContentResponseDTO>>> GetById(Guid id)
    {
        var content = await _service.GetByIdAsync(id);
        if (content == null)
            return NotFound(new ApiErrorResponse(false, $"Conteúdo com ID '{id}' não encontrado.", null));

        return Ok(new ApiResponse<ContentResponseDTO>(true, "Conteúdo encontrado.", content));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ContentResponseDTO>>> Create([FromBody] ContentCreateDTO dto)
    {
        var content = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = content.Id },
            new ApiResponse<ContentResponseDTO>(true, "Conteúdo criado com sucesso.", content));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<ContentResponseDTO>>> Update(Guid id, [FromBody] ContentUpdateDTO dto)
    {
        var content = await _service.UpdateAsync(id, dto);
        if (content == null)
            return NotFound(new ApiErrorResponse(false, $"Conteúdo com ID '{id}' não encontrado.", null));

        return Ok(new ApiResponse<ContentResponseDTO>(true, "Conteúdo atualizado com sucesso.", content));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiErrorResponse>> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound(new ApiErrorResponse(false, $"Conteúdo com ID '{id}' não encontrado.", null));

        return Ok(new ApiErrorResponse(true, "Conteúdo removido com sucesso.", null));
    }
}
