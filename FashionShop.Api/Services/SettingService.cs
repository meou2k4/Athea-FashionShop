using FashionShop.Api.Models; // Note: SettingDto should be defined here
using FashionShop.Data.Entities;
using FashionShop.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FashionShop.Api.Services
{
    // Need to move SettingDto here since it was defined inside SettingsController
    public class SettingDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public interface ISettingService
    {
        Task<IEnumerable<SettingDto>> GetSettingsAsync();
        Task<bool> UpdateSettingsAsync(List<SettingDto> requests);
    }

    public class SettingService : ISettingService
    {
        private readonly IRepository<Setting> _repository;

        public SettingService(IRepository<Setting> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SettingDto>> GetSettingsAsync()
        {
             var settings = await _repository.GetAllAsync();
             return settings.Select(s => new SettingDto
             {
                 Key = s.Key,
                 Value = s.Value,
                 Description = s.Description
             });
        }

        public async Task<bool> UpdateSettingsAsync(List<SettingDto> requests)
        {
            foreach (var req in requests)
            {
                var setting = await _repository.FirstOrDefaultAsync(s => s.Key == req.Key);
                if (setting == null)
                {
                    await _repository.AddAsync(new Setting
                    {
                        Key = req.Key,
                        Value = req.Value,
                        Description = req.Description
                    });
                }
                else
                {
                    setting.Value = req.Value;
                    if (!string.IsNullOrEmpty(req.Description))
                    {
                        setting.Description = req.Description;
                    }
                    _repository.Update(setting);
                }
            }
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
