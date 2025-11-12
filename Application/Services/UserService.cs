using PodpiskaNaSemena.Application.Models.User;
using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Exceptions;
using PodpiskaNaSemena.Domain.Repositories.Abstractions;
using PodpiskaNaSemena.Domain.ValueObjects;
using AutoMapper;
using PodpiskaNaSemena.Application.Services.Abstractions;

namespace PodpiskaNaSemena.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponse> CreateUserAsync(CreateUserRequest request, CancellationToken ct = default)
        {
            // Проверяем уникальность email
            var isEmailUnique = await _userRepository.IsEmailUniqueAsync(request.Email, ct);
            if (!isEmailUnique)
                throw new DomainException("Email уже используется");

            // Создаем пользователя через конструктор
            var user = new User(0, new Username(request.Username), new Email(request.Email));

            await _userRepository.AddAsync(user, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> GetUserAsync(int id, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByIdAsync(id, ct);
            return user == null
                ? throw new EntityNotFoundException(nameof(User), id)
                : _mapper.Map<UserResponse>(user);
        }

        public async Task<UserDetailsResponse> GetUserDetailsAsync(int id, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByIdAsync(id, ct);
            if (user == null)
                throw new EntityNotFoundException(nameof(User), id);

            return _mapper.Map<UserDetailsResponse>(user);
        }

        public async Task MakeUserAdminAsync(int userId, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, ct);
            if (user == null)
                throw new EntityNotFoundException(nameof(User), userId);

            user.MakeAdmin();
            await _userRepository.UpdateAsync(user, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task RemoveUserAdminAsync(int userId, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, ct);
            if (user == null)
                throw new EntityNotFoundException(nameof(User), userId);

            user.RemoveAdmin();
            await _userRepository.UpdateAsync(user, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<UserResponse?> GetUserByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByEmailAsync(email, ct);
            return user != null ? _mapper.Map<UserResponse>(user) : null;
        }

        public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken ct = default)
        {
            return await _userRepository.IsEmailUniqueAsync(email, ct);
        }
        // В КОНЕЦ 

        public async Task<IReadOnlyList<UserResponse>> GetAllUsersAsync(CancellationToken ct = default)
        {
            var users = await _userRepository.GetAllAsync(ct);
            return _mapper.Map<IReadOnlyList<UserResponse>>(users);
        }

        public async Task DeleteUserAsync(int userId, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, ct);
            if (user == null)
                throw new EntityNotFoundException(nameof(User), userId);

            await _userRepository.DeleteAsync(user, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<UserResponse?> GetUserByUsernameAsync(string username, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByUsernameAsync(username, ct);
            return user != null ? _mapper.Map<UserResponse>(user) : null;
        }
        public async Task<UserResponse> UpdateUserAsync(int id, UpdateUserRequest request, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByIdAsync(id, ct);
            if (user == null)
                throw new EntityNotFoundException(nameof(User), id);

            // TODO: Реализовать нормальное обновление когда будут методы в домене
            // Пока возвращаем того же пользователя
            await _userRepository.UpdateAsync(user, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return _mapper.Map<UserResponse>(user);
        }
    }

}