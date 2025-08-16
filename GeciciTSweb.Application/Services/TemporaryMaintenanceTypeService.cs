using AutoMapper;
using GeciciTSweb.Application.DTOs;
using GeciciTSweb.Application.Interfaces;
using GeciciTSweb.Infrastructure.Interfaces;
using GeciciTSweb.Infrastructure.Entities;
using GeciciTSweb.Application.Caching;

namespace GeciciTSweb.Application.Services
{
    public class TemporaryMaintenanceTypeService : ITemporaryMaintenanceTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheManager _memoryCache;
        private readonly string maintenanceTypeCacheName = "maintenance-type";

        public TemporaryMaintenanceTypeService(IUnitOfWork unitOfWork, IMapper mapper, ICacheManager memoryCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public async Task<List<TemporaryMaintenanceTypeListDto>> GetAllAsync()
        {
            // Cache'de varsa okuyorum
            var data = _memoryCache.Get<List<TemporaryMaintenanceTypeListDto>>(maintenanceTypeCacheName);

            if (data == null) 
            {
                //Cache'de yoksa, veritabanından kayıtları alıyorum
                var types = await _unitOfWork.TemporaryMaintenanceTypes.FindAsync(x => !x.IsDeleted);
                
                data = _mapper.Map<List<TemporaryMaintenanceTypeListDto>>(types);

                //Veritabanından aldığım kayıtları cache'e yazıyorum
                _memoryCache.Add(maintenanceTypeCacheName, data);
            }
            
            return data;
        }

        public async Task<TemporaryMaintenanceTypeListDto> GetByIdAsync(int id)
        {
            var type = await _unitOfWork.TemporaryMaintenanceTypes
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (type == null)
                throw new Exception("Geçici tamir tipi bulunamadı.");

            return _mapper.Map<TemporaryMaintenanceTypeListDto>(type);
        }

        public async Task<int> CreateAsync(CreateTemporaryMaintenanceTypeDto dto)
        {
            // Veritabanı ve cache arasındaki veri bütünlüğünü korumak için cache'i temizliyorum
            _memoryCache.Remove(maintenanceTypeCacheName);
            var entity = _mapper.Map<TemporaryMaintenanceType>(dto);
            await _unitOfWork.TemporaryMaintenanceTypes.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity.Id;
        }
    }
}
