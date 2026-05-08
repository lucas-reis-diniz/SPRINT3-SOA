using CarePlus.MindfulnessAPI.Models.DTOs;
using CarePlus.MindfulnessAPI.Models.Entities;
using CarePlus.MindfulnessAPI.Repositories.Interfaces;
using CarePlus.MindfulnessAPI.Services.Interfaces;

namespace CarePlus.MindfulnessAPI.Services;

public class MeditationSessionService : IMeditationSessionService
{
    private readonly IMeditationSessionRepository _repository;
    private readonly IUserRepository _userRepository;

    public MeditationSessionService(IMeditationSessionRepository repository, IUserRepository userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<SessionResponseDTO>> GetAllAsync()
    {
        var sessions = await _repository.GetAllAsync();
        return sessions.Select(MapToDTO);
    }

    public async Task<IEnumerable<SessionResponseDTO>> GetByUserIdAsync(Guid userId)
    {
        var sessions = await _repository.GetByUserIdAsync(userId);
        return sessions.Select(MapToDTO);
    }

    public async Task<SessionResponseDTO?> GetByIdAsync(Guid id)
    {
        var session = await _repository.GetByIdAsync(id);
        return session == null ? null : MapToDTO(session);
    }

    public async Task<SessionResponseDTO> CreateAsync(SessionCreateDTO dto)
    {
        var userExists = await _userRepository.ExistsAsync(dto.UserId);
        if (!userExists)
            throw new InvalidOperationException($"Usuário com ID '{dto.UserId}' não encontrado.");

        if (dto.DuracaoMinutos <= 0)
            throw new InvalidOperationException("A duração deve ser maior que zero.");

        var session = new MeditationSession
        {
            UserId = dto.UserId,
            Tipo = dto.Tipo,
            Titulo = dto.Titulo,
            DuracaoMinutos = dto.DuracaoMinutos,
            Observacoes = dto.Observacoes
        };

        var created = await _repository.CreateAsync(session);

        // Reload with navigation
        var loaded = await _repository.GetByIdAsync(created.Id);
        return MapToDTO(loaded!);
    }

    public async Task<SessionResponseDTO?> UpdateAsync(Guid id, SessionUpdateDTO dto)
    {
        var session = await _repository.GetByIdAsync(id);
        if (session == null) return null;

        session.Tipo = dto.Tipo;
        session.Titulo = dto.Titulo;
        session.DuracaoMinutos = dto.DuracaoMinutos;
        session.Concluida = dto.Concluida;
        session.Observacoes = dto.Observacoes;

        var updated = await _repository.UpdateAsync(session);
        return MapToDTO(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static SessionResponseDTO MapToDTO(MeditationSession s) => new(
        s.Id,
        s.UserId,
        s.User?.Nome ?? "Desconhecido",
        s.Tipo,
        s.Titulo,
        s.DuracaoMinutos,
        s.Concluida,
        s.Observacoes,
        s.RealizadaEm
    );
}
