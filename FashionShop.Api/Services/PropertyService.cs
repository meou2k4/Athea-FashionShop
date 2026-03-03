using FashionShop.Api.Models;
using FashionShop.Data.Entities;
using FashionShop.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FashionShop.Api.Services
{
    public interface IPropertyService
    {
        Task<IEnumerable<ColorDto>> GetColorsAsync();
        Task<(ColorDto dto, string error)> CreateColorAsync(CreateColorDto request);
        Task<IEnumerable<SizeDto>> GetSizesAsync();
        Task<(SizeDto dto, string error)> CreateSizeAsync(CreateSizeDto request);
    }

    public class PropertyService : IPropertyService
    {
        private readonly IRepository<Color> _colorRepository;
        private readonly IRepository<Size> _sizeRepository;

        public PropertyService(IRepository<Color> colorRepository, IRepository<Size> sizeRepository)
        {
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;
        }

        public async Task<IEnumerable<ColorDto>> GetColorsAsync()
        {
            var colors = await _colorRepository.GetAllAsync();
            return colors.Select(c => new ColorDto { Id = c.Id, Name = c.Name, HexCode = c.HexCode });
        }

        public async Task<(ColorDto dto, string error)> CreateColorAsync(CreateColorDto request)
        {
            // Kiểm tra tên màu trùng
            var existing = await _colorRepository.FirstOrDefaultAsync(c =>
                c.Name.ToLower() == request.Name.ToLower().Trim());
            if (existing != null)
                return (null, $"Màu '{request.Name}' đã tồn tại.");

            // Kiểm tra HexCode trùng
            var hexExist = await _colorRepository.FirstOrDefaultAsync(c =>
                c.HexCode.ToLower() == request.HexCode.ToLower());
            if (hexExist != null)
                return (null, $"Mã màu '{request.HexCode}' đã được dùng bởi màu '{hexExist.Name}'.");

            var color = new Color
            {
                Name = request.Name.Trim(),
                HexCode = request.HexCode.ToUpper()
            };

            await _colorRepository.AddAsync(color);
            await _colorRepository.SaveChangesAsync();
            return (new ColorDto { Id = color.Id, Name = color.Name, HexCode = color.HexCode }, null);
        }

        public async Task<IEnumerable<SizeDto>> GetSizesAsync()
        {
            var sizes = await _sizeRepository.GetAllAsync();
            return sizes.Select(s => new SizeDto { Id = s.Id, Name = s.Name });
        }

        public async Task<(SizeDto dto, string error)> CreateSizeAsync(CreateSizeDto request)
        {
            // Kiểm tra tên size trùng
            var existing = await _sizeRepository.FirstOrDefaultAsync(s =>
                s.Name.ToLower() == request.Name.ToLower().Trim());
            if (existing != null)
                return (null, $"Kích thước '{request.Name}' đã tồn tại.");

            var size = new Size { Name = request.Name.Trim().ToUpper() };
            await _sizeRepository.AddAsync(size);
            await _sizeRepository.SaveChangesAsync();
            return (new SizeDto { Id = size.Id, Name = size.Name }, null);
        }
    }
}
