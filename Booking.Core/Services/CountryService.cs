using Booking.Core.Dtos;
using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.Timing;
using Tornado.Shared.ViewModels;

namespace Booking.Core.Services
{
    public class CountryService : Service<Country>, ICountryService
    {
        private readonly IHttpUserService _currentUserService;

        public CountryService(IUnitOfWork unitOfWork, IHttpUserService currentUserService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<CountryCreateModel>> CreateCountryAsync(CountryCreateModel model)
        {
            var response = new ApiResponse<CountryCreateModel> { Code = ApiResponseCodes.OK };
            if (model == null)
            {
                return new ApiResponse<CountryCreateModel>(codes: ApiResponseCodes.INVALID_REQUEST,
                                                         errors: "invalid request");
            }

            var existingCountry = FirstOrDefault(p => string.Equals(model.Name, p?.Name, StringComparison.OrdinalIgnoreCase));

            if (existingCountry != null)
            {
                return new ApiResponse<CountryCreateModel>(errors: "Country already exists", codes: ApiResponseCodes.INVALID_REQUEST);
            }

            var country = new Country
            {
                CreatedBy = _currentUserService.GetCurrentUser().UserId,
                CreatedOn = Clock.Now,
                Code = model.Code,
                Name = model.Name,
                Description = model.Description, DialingCode = model.DialingCode
            };
            var addedEntity = await AddAsync(country);
            model.Id = country.Id.ToString();

            return addedEntity > 0 ? new ApiResponse<CountryCreateModel>(message: "Successful",
                            codes: ApiResponseCodes.OK, data: model)
                : new ApiResponse<CountryCreateModel>(errors: "Could not add county", codes: ApiResponseCodes.FAILED);

        }

        public async Task<ApiResponse<CountryViewModel>> EditCountryAsync(CountryEditModel model)
        {
            var response = new ApiResponse<CountryViewModel> { Code = ApiResponseCodes.OK };
            if (model == null)
            {
                return new ApiResponse<CountryViewModel>(errors: "model cannot be null or empty", codes: ApiResponseCodes.INVALID_REQUEST);
            }
            
            var existingCountry = GetById(Guid.Parse(model.Id));

            if (existingCountry == null)
            {
                return new ApiResponse<CountryViewModel>(errors: "Country does not exist or has been deleted.", codes: ApiResponseCodes.NOT_FOUND);
            }
            //this line is to ensure that i'm not updating to an already existing another existing country
            //e.g If i am trying to update Nigeria to Niger
            var conflictingCountry = FirstOrDefault(p => string.Equals(p.Name, model.Name, StringComparison.OrdinalIgnoreCase)
                                                    && !Equals(p.Id, Guid.Parse(model.Id)));

            if (conflictingCountry != null)
            {
                return new ApiResponse<CountryViewModel>(errors: "Country you're updating to already exists", codes: ApiResponseCodes.EXCEPTION);
            }
             
            existingCountry.ModifiedBy = _currentUserService.GetCurrentUser().UserId;

            existingCountry.ModifiedOn = Clock.Now;
            existingCountry.Code = model.Code;
            existingCountry.Name = model.Name;
            existingCountry.Description = model.Description;
            existingCountry.DialingCode = model.DialingCode;
            var addedEntity = await UpdateAsync(existingCountry);

            var country = new CountryViewModel
            {
                CreatedBy = existingCountry.CreatedBy,
                ModifiedBy = existingCountry.ModifiedBy,
                Name = model.Name,
                Description = model.Description,
                Code = model.Code,
                Id = model.Id                
            };

            return new ApiResponse<CountryViewModel>(message: addedEntity > 0 ? "Successful" : "No country record found",
                                                        codes: ApiResponseCodes.OK, data: country);
        }

        public async Task<ApiResponse<CountryViewModel>> DeleteCountryAsync(string id)
        {
            var response = new ApiResponse<CountryViewModel> { Code = ApiResponseCodes.OK };
            if (!Guid.TryParse(id, out Guid guidId) || string.IsNullOrEmpty(id))
            {
                return new ApiResponse<CountryViewModel>(codes: ApiResponseCodes.INVALID_REQUEST,
                                                         errors: "invalid request");
            }

            var existingCountry = await this.GetByIdAsync(guidId);

            if (existingCountry == null)
            {
                return new ApiResponse<CountryViewModel>(codes: ApiResponseCodes.INVALID_REQUEST, message: "Country does not exists",
                                                         errors: "Country does not exists");
            }

            //modify properties to update 
            existingCountry.IsDeleted = true;
            existingCountry.ModifiedOn = Clock.Now;

            //modified by current user
            existingCountry.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            Delete(existingCountry);

            return new ApiResponse<CountryViewModel>(codes: ApiResponseCodes.OK, message: "successful");
        }

        /// <summary>
        /// Get should take an id of the country..
        /// if we have to use search that takes more than one parameters..
        /// then the can use the GetAllCountryAsync method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<CountryViewModel>> GetCountryAsync(string id)
        {
            var response = new ApiResponse<CountryViewModel> { Code = ApiResponseCodes.OK };


            if (!Guid.TryParse(id, out Guid guidId) || string.IsNullOrEmpty(id))
            {
                return new ApiResponse<CountryViewModel>(errors: "model cannot be null or empty", codes: ApiResponseCodes.INVALID_REQUEST);
            }

            var existingCountry = await GetByIdAsync(guidId);

            if (existingCountry == null)
            {
                return new ApiResponse<CountryViewModel>(errors: "Country does not exist", codes: ApiResponseCodes.NOT_FOUND);
            }

            return new ApiResponse<CountryViewModel>(message: "Successful", codes: ApiResponseCodes.OK, data: (CountryViewModel)existingCountry);
        }
        /// <summary>
        /// Search View Model takes in a search view which inherits from BaseSearchViewModel
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PaginatedList<CountryViewModel>>> GetAllCountryAsync(BaseSearchViewModel searchModel)
        {
            var response = new ApiResponse<PaginatedList<CountryViewModel>> { Code = ApiResponseCodes.OK };
            var keyword = string.IsNullOrEmpty(searchModel.Keyword) ? null : searchModel.Keyword.ToLower();
            int pageStart = searchModel.PageIndex ?? 0;
            int pageEnd = searchModel.PageTotal ?? 50;


            var countryModel = SqlQuery<CountryDto>("[dbo].[Sp_GetCountries] @p0, @p1, @p2",
                                                     keyword, pageStart, pageEnd)
                                                    .Select(p => (CountryViewModel)p)
                                                    .ToList();

            //var test = countryModel != null && countryModel.Any();
           // int total = countryModel.FirstOrDefault()?.TotalCount ?? 0;

            return await Task.FromResult(new ApiResponse<PaginatedList<CountryViewModel>> (message: (countryModel != null && countryModel.Any()) ? "Successful" : "No country record found",
                                                                                           codes: ApiResponseCodes.OK,
                                                                                           totalCount: countryModel != null && countryModel.Any() ? countryModel.FirstOrDefault().TotalCount : 0 ,
                                                                                           data: countryModel?.ToPaginatedList(pageStart, pageEnd, response.TotalCount)));
        }

        public async Task<ApiResponse<List<string>>> GetCountryDialingCodes()
        {
            var response = new ApiResponse<string> { Code = ApiResponseCodes.OK };

            var countries = await Task.FromResult(UnitOfWork.Repository<Country>().GetAll());

            var dialingCodes = countries.Select(p => string.IsNullOrEmpty(p.DialingCode) ? string.Empty : p.DialingCode);


            if (!dialingCodes.Any())
                return new ApiResponse<List<string>>(codes: ApiResponseCodes.NOT_FOUND, message: "Country codes list is empty");

            return new ApiResponse<List<string>>(codes: ApiResponseCodes.OK, message: "Successful", data: dialingCodes.ToList());
        }
    }
}
