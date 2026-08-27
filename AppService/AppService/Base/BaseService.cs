using AutoMapper;
using Crosscutting.Constant;
using Crosscutting.CustomException;
using Crosscutting.Helpers;
using Infrastructure.Base;
using Microsoft.AspNetCore.Http;
using TimeZoneConverter;

namespace AppService.AppService
{
    public abstract class BaseService<TRepository, TEntity>
        where TRepository : IGenericRepository<TEntity>
        where TEntity : class
    {
        protected readonly IMapper _mapper;
        protected readonly TRepository _repository;
        protected readonly TimeZoneInfo _timeZone;
        protected readonly string _email;
        protected readonly string _role;
        protected readonly string _cnpj;
        protected readonly string _username;

        protected BaseService(IMapper mapper, TRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
            _timeZone = TZConvert.GetTimeZoneInfo(GeneralConstants.TimeZone);
            _email = string.Empty;
            _role = string.Empty;
            _cnpj = string.Empty;
            _username = string.Empty;
        }

        protected BaseService(IMapper mapper, TRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _repository = repository;
            _timeZone = ResolveTimeZone(httpContextAccessor);
            _email = ResolveClaim(() => JwtHelper.GetEmailByToken(httpContextAccessor));
            _role = ResolveClaim(() => JwtHelper.GetRoleByToken(httpContextAccessor));
            _cnpj = ResolveClaim(() => JwtHelper.GetCnpjByToken(httpContextAccessor));
            _username = ResolveClaim(() => JwtHelper.GetUserNameByToken(httpContextAccessor));
        }

        protected async Task<TEntity> AddAsync(TEntity entity, CancellationToken ct = default)
        {
            if (entity is not null)
                return await _repository.AddAsync(entity, ct);
            else
                throw new CustomBusinessException("Ops... Ocorreu uma falha ao tentar incluir o registro no banco de dados.");
        }

        protected async Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
        {
            if (entities is not null && entities.Any())
                return await _repository.AddAsync(entities, ct);
            else
                throw new CustomBusinessException("Ops... Ocorreu uma falha ao tentar incluir os registros no banco de dados.");
        }

        protected async Task<TEntity> EditAsync(TEntity entity)
        {
            if (entity is not null)
                return await _repository.EditAsync(entity);
            else
                throw new CustomBusinessException("Ops... Ocorreu uma falha ao tentar atualizar o registro no banco de dados.");
        }

        protected async Task DeleteAsync(TEntity entity)
        {
            if (entity is not null)
                await _repository.DeleteAsync(entity);
            else
                throw new CustomBusinessException("Ops... Ocorreu uma falha ao tentar remover o registro no banco de dados.");
        }

        protected async Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            if (entities is not null && entities.Any())
                await _repository.DeleteAsync(entities);
            else
                throw new CustomBusinessException("Ops... Ocorreu uma falha ao tentar remover os registros no banco de dados.");
        }

        protected async Task<IReadOnlyList<TEntity>> GetAllReadOnlyAsync(CancellationToken ct = default)
        {
            return await _repository.GetAllReadOnlyAsync(ct);
        }

        protected async Task<TEntity> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id) ?? throw new CustomBusinessException("Ops... O registro buscado não foi encontrado.");
        }

        protected async Task<TEntity> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id) ?? throw new CustomBusinessException("Ops... O registro buscado não foi encontrado.");
        }

        protected async Task<TEntity?> GetByIdWithoutThrowAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        protected async Task<TEntity?> GetByIdWithoutThrowAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        private static TimeZoneInfo ResolveTimeZone(IHttpContextAccessor httpContextAccessor)
        {
            string timeZone = ResolveClaim(() => JwtHelper.GetTimeZoneByToken(httpContextAccessor));

            if (string.IsNullOrWhiteSpace(timeZone))
                timeZone = GeneralConstants.TimeZone;

            return TZConvert.GetTimeZoneInfo(timeZone);
        }

        private static string ResolveClaim(Func<string> claimResolver)
        {
            try
            {
                return claimResolver();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
