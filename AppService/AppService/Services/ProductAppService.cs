using AppService.AppService.Interfaces;
using AutoMapper;
using Crosscutting.DTO.Product;
using Crosscutting.DTO.StockMovement;
using Crosscutting.CustomException;
using Domain.Entity;
using Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AppService.AppService.Services
{
    public class ProductAppService : BaseService<IProductRepository, ProductEntity>, IProductAppService
    {
        private readonly IProductStockRepository _productStockRepository;
        private readonly IProductPhotoRepository _productPhotoRepository;
        private readonly IProposalRepository _proposalRepository;
        private readonly IStockMovementRepository _stockMovementRepository;

        public ProductAppService(
            IMapper mapper,
            IProductRepository repository,
            IProductStockRepository productStockRepository,
            IProductPhotoRepository productPhotoRepository,
            IProposalRepository proposalRepository,
            IStockMovementRepository stockMovementRepository,
            IHttpContextAccessor httpContextAccessor) : base(mapper, repository, httpContextAccessor)
        {
            _productStockRepository = productStockRepository;
            _productPhotoRepository = productPhotoRepository;
            _proposalRepository = proposalRepository;
            _stockMovementRepository = stockMovementRepository;
        }

        public async Task<IReadOnlyList<ProductResponseDTO>> GetAllAsync(CancellationToken ct = default)
        {
            IReadOnlyList<ProductEntity> entities = await GetAllReadOnlyAsync(ct);
            return _mapper.Map<IReadOnlyList<ProductResponseDTO>>(entities);
        }

        public async Task<ProductResponseDTO> GetResponseByIdAsync(int id)
        {
            ProductEntity entity = await base.GetByIdAsync(id);
            return _mapper.Map<ProductResponseDTO>(entity);
        }

        public async Task<ProductStockSummaryResponseDTO> GetStockSummaryAsync(int id, CancellationToken ct = default)
        {
            _ = await base.GetByIdAsync(id);

            ProductStockEntity? stock = await _productStockRepository.GetByProductIdAsync(id, ct);
            decimal physical = stock?.Quantity ?? 0m;
            decimal reserved = await _proposalRepository.GetReservedQuantityByProductIdAsync(id, ct);

            return new ProductStockSummaryResponseDTO
            {
                ProductId = id,
                Physical = physical,
                Reserved = reserved,
                Available = physical - reserved
            };
        }

        public async Task<IReadOnlyList<StockMovementResponseDTO>> GetMovementsAsync(int id, CancellationToken ct = default)
        {
            _ = await base.GetByIdAsync(id);

            IReadOnlyList<StockMovementEntity> movements = await _stockMovementRepository.GetByProductIdAsync(id, ct);
            return _mapper.Map<IReadOnlyList<StockMovementResponseDTO>>(movements);
        }

        public async Task<ProductResponseDTO> CreateAsync(CreateProductRequestDTO request, CancellationToken ct = default)
        {
            ProductEntity entity = _mapper.Map<ProductEntity>(request);
            entity.CreatedAt = GetCurrentDateTime();
            entity.CreatedBy = GetCurrentUserEmail();

            ProductEntity createdEntity = await AddAsync(entity, ct);

            await _productStockRepository.AddAsync(new ProductStockEntity
            {
                ProductId = createdEntity.Id,
                Quantity = 0,
                CreatedAt = GetCurrentDateTime(),
                CreatedBy = GetCurrentUserEmail()
            }, ct);

            return _mapper.Map<ProductResponseDTO>(createdEntity);
        }

        public async Task<ProductResponseDTO> UpdateAsync(int id, UpdateProductRequestDTO request)
        {
            ProductEntity entity = await base.GetByIdAsync(id);

            entity.UnitOfMeasureId = request.UnitOfMeasureId;
            entity.Name = request.Name;
            entity.MaskName = request.MaskName;
            entity.Sku = request.Sku;
            entity.Description = request.Description;
            entity.UnitPrice = request.UnitPrice;
            entity.CostPrice = request.CostPrice;
            entity.MarkupPercent = request.MarkupPercent;
            entity.IcmsPercent = request.IcmsPercent;
            entity.IssPercent = request.IssPercent;
            entity.PisPercent = request.PisPercent;
            entity.CofinsPercent = request.CofinsPercent;
            entity.IsActive = request.IsActive;
            entity.UpdatedAt = GetCurrentDateTime();
            entity.UpdatedBy = GetCurrentUserEmail();

            ProductEntity updatedEntity = await EditAsync(entity);
            return _mapper.Map<ProductResponseDTO>(updatedEntity);
        }

        public async Task DeleteAsync(int id)
        {
            ProductEntity entity = await base.GetByIdAsync(id);
            await DeleteAsync(entity);
        }

        public async Task<IReadOnlyList<ProductPhotoResponseDTO>> GetPhotosAsync(int productId, CancellationToken ct = default)
        {
            _ = await base.GetByIdAsync(productId);

            IReadOnlyList<ProductPhotoEntity> photos = await _productPhotoRepository.GetByProductIdAsync(productId, ct);
            return _mapper.Map<IReadOnlyList<ProductPhotoResponseDTO>>(photos);
        }

        public async Task<ProductPhotoResponseDTO> CreatePhotoAsync(int productId, CreateProductPhotoRequestDTO request, CancellationToken ct = default)
        {
            _ = await base.GetByIdAsync(productId);

            ProductPhotoEntity entity = _mapper.Map<ProductPhotoEntity>(request);
            entity.ProductId = productId;
            entity.CreatedAt = GetCurrentDateTime();
            entity.CreatedBy = GetCurrentUserEmail();

            ProductPhotoEntity createdEntity = await _productPhotoRepository.AddAsync(entity, ct);
            return _mapper.Map<ProductPhotoResponseDTO>(createdEntity);
        }

        public async Task<ProductPhotoResponseDTO> UpdatePhotoAsync(int productId, int photoId, UpdateProductPhotoRequestDTO request)
        {
            ProductPhotoEntity? entity = await _productPhotoRepository.GetByIdAndProductIdAsync(photoId, productId);
            if (entity is null)
            {
                throw new CustomBusinessException("Ops... Foto do produto não encontrada.");
            }

            entity.ImageUrl = request.ImageUrl;
            entity.UpdatedAt = GetCurrentDateTime();
            entity.UpdatedBy = GetCurrentUserEmail();

            ProductPhotoEntity updatedEntity = await _productPhotoRepository.EditAsync(entity);
            return _mapper.Map<ProductPhotoResponseDTO>(updatedEntity);
        }

        public async Task DeletePhotoAsync(int productId, int photoId)
        {
            ProductPhotoEntity? entity = await _productPhotoRepository.GetByIdAndProductIdAsync(photoId, productId);
            if (entity is null)
            {
                throw new CustomBusinessException("Ops... Foto do produto não encontrada.");
            }

            await _productPhotoRepository.DeleteAsync(entity);
        }

        private DateTime GetCurrentDateTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone);
        }

        private string GetCurrentUserEmail()
        {
            return _email;
        }
    }
}
