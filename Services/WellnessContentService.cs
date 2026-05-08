using CarePlus.MindfulnessAPI.Models.DTOs;
using CarePlus.MindfulnessAPI.Models.Entities;
using CarePlus.MindfulnessAPI.Repositories.Interfaces;
using CarePlus.MindfulnessAPI.Services.Interfaces;

namespace CarePlus.MindfulnessAPI.Services;

public class WellnessContentService : IWellnessContentService
{
    private readonly IWellnessContentRepository _repository;

    public WellnessContentService(IWellnessContentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ContentResponseDTO>> GetAllAsync()
    {
        var contents = await _repository.GetAllAsync();
        return contents.Select(MapToDTO);
    }

    public async Task<IEnumerable<ContentResponseDTO>> GetActiveAsync()
    {
        var contents = await _repository.GetActiveAsync();
        return contents.Select(MapToDTO);
    }

    public async Task<ContentResponseDTO?> GetByIdAsync(Guid id)
    {
        var content = await _repository.GetByIdAsync(id);
        return content == null ? null : MapToDTO(content);
    }

    public async Task<ContentResponseDTO> CreateAsync(ContentCreateDTO dto)
    {
        var content = new WellnessContent
        {
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            Categoria = dto.Categoria,
            UrlRecurso = dto.UrlRecurso,
            DuracaoEstimadaMin = dto.DuracaoEstimadaMin
        };

        var created = await _repository.CreateAsync(content);
        return MapToDTO(created);
    }

    public async Task<ContentResponseDTO?> UpdateAsync(Guid id, ContentUpdateDTO dto)
    {
        var content = await _repository.GetByIdAsync(id);
        if (content == null) return null;

        content.Titulo = dto.Titulo;
        content.Descricao = dto.Descricao;
        content.Categoria = dto.Categoria;
        content.UrlRecurso = dto.UrlRecurso;
        content.DuracaoEstimadaMin = dto.DuracaoEstimadaMin;
        content.Ativo = dto.Ativo;

        var updated = await _repository.UpdateAsync(content);
        return MapToDTO(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static ContentResponseDTO MapToDTO(WellnessContent c) => new(
        c.Id,
        c.Titulo,
        c.Descricao,
        c.Categoria,
        c.UrlRecurso,
        c.DuracaoEstimadaMin,
        c.Ativo,
        c.CriadoEm
    );
}
