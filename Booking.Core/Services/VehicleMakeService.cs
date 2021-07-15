using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.Validators;
using Booking.Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.Timing;
using Tornado.Shared.ViewModels;
using System.Linq;
using Tornado.Shared.AspNetCore;

namespace Booking.Core.Services
{
    public class VehicleMakeService : Service<VehicleMake>, IVehicleMakeService
    {
        private readonly IHttpUserService _currentUserService;

        public VehicleMakeService(IUnitOfWork unitOfWork, IHttpUserService currentUserService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<VehicleMakeViewModel>> CreateVehicleMakeAsync(VehicleMakeViewModel model)
        {
            var response = new ApiResponse<VehicleMakeViewModel> { Code = ApiResponseCodes.OK };
            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new CreateVehicleMakeValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            var existingRecord = FirstOrDefault(p => string.Equals(p.Title, model.Title, StringComparison.OrdinalIgnoreCase));
            if (existingRecord != null)
            {
                response.Code = ApiResponseCodes.FAILED;
                response.Description = "Record already exists";
                return response;
            }

            var make = new VehicleMake
            {
                Title = model.Title,
                Description = model.Description,
                ShortDescription = model.ShortDescription,
                CreatedBy = _currentUserService.GetCurrentUser().UserId,
                CreatedOn = Clock.Now
            };
            await AddAsync(make);


            model.Id = make.Id.ToString();
            model.CreatedId = make.Id;
            response.Code = ApiResponseCodes.OK;
            response.Payload = (VehicleMakeViewModel)make;
            response.Description = "Successful";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<VehicleMakeViewModel>> DeleteVehicleMakeAsync(DeleteVehicleMakeViewModel model)
        {
            var response = new ApiResponse<VehicleMakeViewModel> { Code = ApiResponseCodes.OK };
            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new DeleteVehicleMakeValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.MakeId, out Guid makeId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid vehicle make id");
                goto ReturnToCaller;
            }

            var existingVehicleMake = await GetByIdAsync(makeId);

            if (existingVehicleMake == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Vehicle make not found");
                goto ReturnToCaller;
            }

            existingVehicleMake.IsDeleted = true;
            existingVehicleMake.ModifiedOn = Clock.Now;
            existingVehicleMake.ModifiedBy = _currentUserService.GetCurrentUser().UserId;

            Delete(existingVehicleMake);

            response.Code = ApiResponseCodes.OK;
            response.Description = "Successful";
            response.Payload = (VehicleMakeViewModel)existingVehicleMake;

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<EditVehicleMakeViewModel>> EditVehicleMakeAsync(EditVehicleMakeViewModel model)
        {
            var response = new ApiResponse<EditVehicleMakeViewModel> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new EditVehicleMakeValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.Id, out Guid makeId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid vehicle make id");
                goto ReturnToCaller;
            }

            var existingData = await GetByIdAsync(makeId);

            if (existingData == null)
            {
                response.Code = ApiResponseCodes.NOT_FOUND;
                response.Errors.Add("Model does not exist");
                goto ReturnToCaller;
            }

            existingData.Description = model.Description;
            existingData.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            existingData.ModifiedOn = Clock.Now;
            existingData.ShortDescription = model.ShortDescription;
            existingData.Title = model.Title;

            var isVehicleMakeUpdated = await UpdateAsync(existingData) > 0;

            response.Code = isVehicleMakeUpdated ? ApiResponseCodes.OK : ApiResponseCodes.FAILED;
            response.Description = isVehicleMakeUpdated ? "Successful" : $"Could not update {model.Title ?? "vehicle make"} at this time, please try again later";
            response.Payload = (EditVehicleMakeViewModel)existingData;

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<PaginatedList<VehicleMakeViewModel>>> GetAllVehicleMakeAsync(BasePaginatedViewModel model)
        {
            var response = new ApiResponse<PaginatedList<VehicleMakeViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            model.PageIndex ??= 1;
            model.PageIndex ??= 50;

            var data = await Task.Run(() =>
            {
                return (from dataModel in UnitOfWork.Repository<VehicleMake>().GetAll()

                        select new VehicleMakeViewModel
                        {
                            CreatedId = dataModel.CreatedBy ?? Guid.Empty,
                            Description = dataModel.Description,
                            Id = dataModel.Id.ToString(),
                            ShortDescription = dataModel.ShortDescription,
                            Title = dataModel.Title,
                            CreatedBy = dataModel.CreatedBy ?? Guid.Empty,
                            ModifiedBy = dataModel.ModifiedBy ?? Guid.Empty
                        });
            });

            response.Payload = data.ToPaginatedList(model.PageIndex.Value, model.PageTotal.Value); ;
            response.PayloadMetaData = new PayloadMetaData(pageIndex: response.Payload.PageIndex, pageSize: response.Payload.PageSize, totalPageCount: response.Payload.TotalPageCount, totalCount: response.Payload.TotalCount);

            response.TotalCount = response.Payload.TotalCount;
            response.Code = response.TotalCount == 0 ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK;
            response.Description = response.TotalCount > 0 ? "Successful" : "No vehicle make record found";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<VehicleMakeViewModel>> GetVehicleMakeAsync(GetVehicleMakeByIdViewModel model)
        {
            var response = new ApiResponse<VehicleMakeViewModel> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new GetVehicleMakeValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.MakeId, out Guid makeId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid vehicle make id");
                goto ReturnToCaller;
            }

            var data = await GetByIdAsync(makeId);
            response.TotalCount = data != null ? 1 : 0;
            response.Payload = data != null ? (VehicleMakeViewModel)data : null;
            response.Code = data == null ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK;
            response.Description = data != null ? "Successful" : "No vehicle make record found";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<PaginatedList<VehicleMakeViewModel>>> SearchVehicleMakeAsync(VehicleMakeSearchViewModel searchModel)
        {
            var response = new ApiResponse<PaginatedList<VehicleMakeViewModel>> { Code = ApiResponseCodes.OK };

            if (searchModel == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            searchModel.PageIndex ??= 1;
            searchModel.PageIndex ??= 50;


            var validationResult = new VehicleMakeSearchValidator().Validate(searchModel);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            var data = await Task.Run(() =>
            {
              return from dataModel in UnitOfWork.Repository<VehicleMake>().GetAll()
                where dataModel.Title.Contains(searchModel.Keyword) 
                || (string.IsNullOrEmpty(searchModel.Keyword))
                select new VehicleMakeViewModel
                {
                    CreatedId = dataModel.CreatedBy ?? Guid.Empty,
                    Description = dataModel.Description,
                    Id = dataModel.Id.ToString(),
                    ShortDescription = dataModel.ShortDescription,
                    Title = dataModel.Title,
                    CreatedBy = dataModel.CreatedBy ?? Guid.Empty,
                    ModifiedBy = dataModel.ModifiedBy ?? Guid.Empty
                };
            });

            response.Payload = data.ToPaginatedList(searchModel.PageIndex.Value, searchModel.PageTotal.Value);
            response.PayloadMetaData = new PayloadMetaData(pageIndex: response.Payload.PageIndex, pageSize: response.Payload.PageSize, totalPageCount: response.Payload.TotalPageCount, totalCount: response.Payload.TotalCount);
            response.TotalCount = response.Payload.TotalCount;
            response.Code = response.TotalCount == 0 ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK;
            response.Description = response.TotalCount > 0 ? "Successful" : "No vehicle make record found";


        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }
    }
}
