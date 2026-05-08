using CarePlus.MindfulnessAPI.Models.DTOs;
using CarePlus.MindfulnessAPI.Models.Entities;
using CarePlus.MindfulnessAPI.Repositories.Interfaces;
using CarePlus.MindfulnessAPI.Services.Interfaces;

namespace CarePlus.MindfulnessAPI.Services;

public class MoodEntryService : IMoodEntryService
{
    private readonly IMoodEntryRepository _repository;
    private readonly IUserRepository _userRepository;

    public MoodEntryService(IMoodEntryRepository repository, IUserRepository userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<MoodResponseDTO>> GetAllAsync()
    {
        var entries = await _repository.GetAllAsync();
        return entries.Select(MapToDTO);
    }

    public async Task<IEnumerable<MoodResponseDTO>> GetByUserIdAsync(Guid userId)
    {
        var entries = await _repository.GetByUserIdAsync(userId);
        return entries.Select(MapToDTO);
    }

    public async Task<MoodResponseDTO?> GetByIdAsync(Guid id)
    {
        var entry = await _repository.GetByIdAsync(id);
        return entry == null ? null : MapToDTO(entry);
    }

    public async Task<MoodResponseDTO> CreateAsync(MoodCreateDTO dto)
    {
        var userExists = await _userRepository.ExistsAsync(dto.UserId);
        if (!userExists)
            throw new InvalidOperationException($"Usuário com ID '{dto.UserId}' não encontrado.");

        var entry = new MoodEntry
        {
            UserId = dto.UserId,
            NivelHumor = dto.NivelHumor,
            Notas = dto.Notas
        };

        var created = await _repository.CreateAsync(entry);
        var loaded = await _repository.GetByIdAsync(created.Id);
        return MapToDTO(loaded!);
    }

    public async Task<MoodResponseDTO?> UpdateAsync(Guid id, MoodUpdateDTO dto)
    {
        var entry = await _repository.GetByIdAsync(id);
        if (entry == null) return null;

        entry.NivelHumor = dto.NivelHumor;
        entry.Notas = dto.Notas;

        var updated = await _repository.UpdateAsync(entry);
        return MapToDTO(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static MoodResponseDTO MapToDTO(MoodEntry m) => new(
        m.Id,
        m.UserId,
        m.User?.Nome ?? "Desconhecido",
        m.NivelHumor,
        m.NivelHumor.ToString(),
        m.Notas,
        m.DataRegistro
    );
}
