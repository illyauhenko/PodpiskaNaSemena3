using PodpiskaNaSemena.Application.Models.User;
using PodpiskaNaSemena.Domain.Entities;
using PodpiskaNaSemena.Domain.Exceptions;
using PodpiskaNaSemena.Domain.Repositories.Abstractions;
using AutoMapper;


namespace PodpiskaNaSemena.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserModel> CreateAsync(UserCreateModel createModel, CancellationToken ct = default)
        {
            var user = _mapper.Map<User>(createModel);
            await _userRepository.AddAsync(user, ct);
            return _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByIdAsync(id, ct);
            return user == null
                ? throw new EntityNotFoundException(nameof(User), id)
                : _mapper.Map<UserModel>(user);
        }
    }
}