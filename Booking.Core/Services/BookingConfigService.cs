using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.Extensions;
using Tornado.Shared.Timing;
using System.Linq;
using Tornado.Shared.ViewModels;

namespace Booking.Core.Services
{
    public class BookingConfigService : Service<BookingConfig>, IBookingConfigService
    {

        public const string cacheKeyPrefix = "booking_";
        private readonly IHttpUserService _currentUserService;


        private readonly IDistributedCache _distributedCache;

        public BookingConfigService(IUnitOfWork unitOfWork,IDistributedCache distributedCache, IHttpUserService currentUserService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
            _distributedCache = distributedCache;
        }


        public async Task<BookingConfigViewModel> GetConfigSettingByName(string name)
        {
            var key = cacheKeyPrefix + name;

            var cachedSetting = await _distributedCache.GetAsync(key).ConfigureAwait(false);

            if (cachedSetting == null)
            {
                var setting = FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));

                if (setting == null)  throw new Exception("Settings config does not exist!");

                var settingModel = new BookingConfigViewModel { IsCollection = setting.IsCollection, Name = setting.Name, Value = setting.Value };
               
                cachedSetting = await _distributedCache.SetRedisCacheAsync(key, settingModel).ConfigureAwait(false);
            }
            if (cachedSetting == null) return null;
            var deserializedSetting = await _distributedCache.GetRedisCachedObjectAsync<BookingConfigViewModel>(cachedSetting).ConfigureAwait(false);

            return deserializedSetting;
        }
        public async Task<BookingConfigViewModel> CreateConfigSetting(CreateBookingConfigViewModel model)
        {
            var key = cacheKeyPrefix + model.Name;


            var setting = FirstOrDefault(p => string.Equals(p.Name, model.Name, StringComparison.OrdinalIgnoreCase));


            if (setting != null)
            {
                throw new Exception("Settings config already exists");
            }
            
            var config = new BookingConfig
            {
                CreatedBy = _currentUserService.GetCurrentUser().UserId,
                Name = model.Name,
                Value = model.Value,
                IsCollection = model.IsCollection,
                CreatedOn = Clock.Now,
            };

            var configCreated = await AddAsync(config).ConfigureAwait(false);

            if (configCreated <= 0) return null;
            var settingModel = new BookingConfigViewModel { Name = model.Name, Value = model.Value, DataType = model.DataType, IsCollection = model.IsCollection};

            await _distributedCache.SetRedisCacheAsync(key, settingModel).ConfigureAwait(false);

            return settingModel;
        }
        public async Task<EditBookingConfigViewModel> EditConfigSetting(EditBookingConfigRequestViewModel model)
        {
            var key = cacheKeyPrefix + model.Name;

            var setting = FirstOrDefault(p => string.Equals(p.Name, model.Name, StringComparison.OrdinalIgnoreCase));

            if (setting == null)
            {
                throw new Exception("Settings config does not exist");
            }

            setting.ModifiedOn = Clock.Now;
            setting.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
     
            setting.Value = model.Value;
            setting.IsCollection = model.IsCollection;

            var updatedEntity = await UpdateAsync(setting).ConfigureAwait(false) ;

            if (updatedEntity <= 0) return null;
            var settingModel = new EditBookingConfigViewModel { Name = model.Name, Value = model.Value, DataType = model.DataType, IsCollection = model.IsCollection };

            await _distributedCache.SetRedisCacheAsync(key, settingModel).ConfigureAwait(false);

            return settingModel;
        }

        public async Task<PaginatedList<BookingConfigViewModel>> GetAllConfig(BaseSearchViewModel model)
        {
            if (model == null)
            {
                throw new Exception("Bad Request");
            }
            int pageIndex = model.PageIndex ?? 1;
            int pageSize = model.PageTotal ?? 20;

            var settingsList = await Task.FromResult(from setting in UnitOfWork.Repository<BookingConfig>().GetAll()

                               where setting.IsDeleted == false && 
                               (setting.Name.Contains(model.Keyword, StringComparison.OrdinalIgnoreCase) ||
                               setting.Value.Contains(model.Keyword, StringComparison.OrdinalIgnoreCase) || 
                               string.IsNullOrEmpty(model.Keyword))

                               select new BookingConfigViewModel
                               {
                                   CreatedBy = setting.CreatedBy,
                                   DataType = setting.DataType,
                                   CreatedOn = setting.CreatedOn,
                                   Value = setting.Value,
                                   Name = setting.Name,
                                   IsCollection = setting.IsCollection,
                                   Id = setting.Id
                               }).ConfigureAwait(false);

            return settingsList.ToPaginatedList(pageIndex, pageSize);
        }
    }
}
