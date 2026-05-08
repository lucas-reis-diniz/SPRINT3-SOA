using CarePlus.MindfulnessAPI.Models.DTOs;
using CarePlus.MindfulnessAPI.Models.Entities;
using CarePlus.MindfulnessAPI.Repositories.Interfaces;
using CarePlus.MindfulnessAPI.Services.Interfaces;

namespace CarePlus.MindfulnessAPI.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<UserResponseDTO>> GetAllAsync()
    {
        var users = await _repository.GetAllAsync();
        return users.Select(MapToDTO);
    }

    public async Task<UserResponseDTO?> GetByIdAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        return user == null ? null : MapToDTO(user);
    }

    public async Task<UserResponseDTO> CreateAsync(UserCreateDTO dto)
    {
        var existingUser = await _repository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new InvalidOperationException($"Já existe um usuário com o email '{dto.Email}'.");

        var user = new User
        {
            Nome = dto.Nome,
            Email = dto.Email,
            DataNascimento = dto.DataNascimento
        };

        var created = await _repository.CreateAsync(user);
        return MapToDTO(created);
    }

    public async Task<UserResponseDTO?> UpdateAsync(Guid id, UserUpdateDTO dto)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) return null;

        user.Nome = dto.Nome;
        user.Email = dto.Email;
        user.DataNascimento = dto.DataNascimento;

        var updated = await _repository.UpdateAsync(user);
        return MapToDTO(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static UserResponseDTO MapToDTO(User user) => new(
        user.Id,
        user.Nome,
        user.Email,
        user.DataNascimento,
        user.CriadoEm,
        user.Sessions.Count,
        user.MoodEntries.Count
    );
}
