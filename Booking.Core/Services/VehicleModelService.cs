using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.ViewModels;
using System.Linq;
using Booking.Core.Dtos;
using Tornado.Shared.Timing;
using Microsoft.EntityFrameworkCore;
using Booking.Core.Validators;
using Tornado.Shared.AspNetCore;

namespace Booking.Core.Services
{
    public class VehicleModelService : Service<VehicleModel>, IVehicleModelService
    {

        private readonly IHttpUserService _currentUserService;
        public VehicleModelService(IUnitOfWork unitOfWork, IHttpUserService currentUserService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// This method creates a new vehicle model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResponse<VehicleModelViewModel>> CreateVehicleModelAsync(VehicleModelViewModel model)
        {
            var response = new ApiResponse<VehicleModelViewModel> { };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new CreateVehicleModelValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.VehicleMakeId, out Guid makeId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid vehicle make id");
                goto ReturnToCaller;
            }

            var existingModel = FirstOrDefault(p => p.VehicleMakeId == makeId
                                && string.Equals(p.Title, model.Title, StringComparison.OrdinalIgnoreCase));

            if (existingModel != null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add($"Model already exist in vehicle ({model.VehicleMake}).");
                goto ReturnToCaller;
            }

            var selectedMake = await UnitOfWork.Repository<VehicleMake>().GetByIdAsync(makeId);
            if (selectedMake == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add($"Invalid vehicle make iD");
                goto ReturnToCaller;
            }

            var vModel = new VehicleModel
            {
                Description = model.Description,
                ShortDescription = model.ShortDescription,
                Title = model.Title,
                VehicleMakeId = selectedMake.Id,
                CreatedBy = _currentUserService.GetCurrentUser().UserId,
                NoOfSeats = model.NoOfSeats
            };

            var isVehicleModelCreated = await AddAsync(vModel) > 0;

            response.Payload = (VehicleModelViewModel)vModel;
            response.Description = isVehicleModelCreated ? "Successful" : "Could not create vehicle model at this time, please try again later";
            response.Code = isVehicleModelCreated ? ApiResponseCodes.OK : ApiResponseCodes.FAILED;
            response.TotalCount = isVehicleModelCreated ? 1 : 0;

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<EditVehicleModelViewModel>> EditVehicleModelAsync(EditVehicleModelViewModel model)
        {
            var response = new ApiResponse<EditVehicleModelViewModel> { };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new EditVehicleModelValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.Id, out Guid modelId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid vehicle model id");
                goto ReturnToCaller;
            }
            var existingData = await GetByIdAsync(modelId);

            if (existingData == null)
            {
                response.Code = ApiResponseCodes.NOT_FOUND;
                response.Errors.Add("Model does not exist");
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.VehicleMakeId, out Guid makeId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid vehicle make id");
                goto ReturnToCaller;
            }

            var selectedMake = await UnitOfWork.Repository<VehicleMake>().GetByIdAsync(makeId);
            if (selectedMake == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add($"Invalid vehicle make iD");
                goto ReturnToCaller;
            }

            //e.g I should be able to create another Delta... If its a state in Ghana
            var conflictingModel = FirstOrDefault(p => string.Equals(p.Title, model.Title, StringComparison.OrdinalIgnoreCase)
                                                    && !Equals(p.Id, model.Id) && Guid.Equals(p.VehicleMakeId, model.VehicleMakeId));

            if (conflictingModel != null)
            {
                response.ResponseCode = ((int)ApiResponseCodes.INVALID_REQUEST).ToString();
                response.Code = ApiResponseCodes.EXCEPTION;
                response.Errors.Add($"Model already exists in the Vehicle make");
                goto ReturnToCaller;
            }

            existingData.VehicleMakeId = makeId;
            existingData.ShortDescription = model.ShortDescription;
            existingData.ModifiedOn = Clock.Now;
            existingData.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            existingData.Description = model.Description;
            existingData.Title = model.Title;
            existingData.VehicleMakeId = makeId;

            var isVehicleModelUpdated = await UpdateAsync(existingData) > 0;

            response.Code = isVehicleModelUpdated ? ApiResponseCodes.OK : ApiResponseCodes.FAILED;
            response.Description = isVehicleModelUpdated ? "Successful" : $"Could not update {model.Title ?? "vehicle model"} at this time, please try again later";
            response.Payload = model;

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        /// <summary>
        /// Parse the Id of the vehicle to be deleted
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResponse<VehicleModelViewModel>> DeleteVehicleModelAsync(DeleteVehicleModel model)
        {
            var response = new ApiResponse<VehicleModelViewModel> { };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new DeleteVehicleModelValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.ModelId, out Guid modelId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid vehicle model id");
                goto ReturnToCaller;
            }

            var existingVehicleModel = await GetByIdAsync(modelId);

            if (existingVehicleModel == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Vehicle model does not exist or has been deleted");
                goto ReturnToCaller;
            }

            existingVehicleModel.IsDeleted = true;
            existingVehicleModel.ModifiedOn = Clock.Now;
            existingVehicleModel.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            Delete(existingVehicleModel);

            response.Code = ApiResponseCodes.OK;
            response.Description = "Successful";
            response.Payload = null;

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        /// <summary>
        /// This method returns all the registered vehicle models in paginated format
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse<PaginatedList<VehicleModelViewModel>>> GetAllVehicleModelPaginatedAsync(VehicleModelSearch searchModel)
        {
            var response = new ApiResponse<PaginatedList<VehicleModelViewModel>>
            {
                Code = ApiResponseCodes.OK,
            };

            if (searchModel == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            searchModel.PageIndex ??= 0;
            searchModel.PageTotal ??= 50;

            var validationResult = new GetAllVehicleModelValidator().Validate(searchModel);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            var modelEntity = from model in UnitOfWork.Repository<VehicleModel>().GetAll().AsNoTracking()
                              join make in UnitOfWork.Repository<VehicleMake>().GetAll().AsNoTracking()
                              on model.VehicleMakeId equals make.Id
                              where model.Title.Contains(searchModel.Keyword)
                              || make.Title.Contains(searchModel.Keyword) 

                              || (string.IsNullOrEmpty(searchModel.Keyword))
                              select new VehicleModelDto
                              {
                                  Id = model.Id,
                                  VehicleMake = make.Title,
                                  Title = model.Title,
                                  Description = model.Description,
                                  VehicleMakeId = make.Id,
                                  CreatedId = make.CreatedBy
                              };


            var modelEntities = modelEntity.Select(p => (VehicleModelViewModel)p);

            response.Payload = modelEntities.ToPaginatedList(searchModel.PageIndex.Value, searchModel.PageTotal.Value);
            response.PayloadMetaData = new PayloadMetaData
                (
                   pageIndex: response.Payload.PageIndex,
                   pageSize: response.Payload.PageSize,
                   totalPageCount: response.Payload.TotalPageCount,
                   totalCount: response.Payload.TotalCount
                );

            response.TotalCount = response.Payload.TotalCount;
            response.Description = response.TotalCount > 0 ? "Successful" : "No vehicle model record found";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return await Task.FromResult(response);
        }
        /// <summary>
        /// This method gets vehicle model details by model id
        /// just parse the Guid
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ApiResponse<VehicleModelViewModel>> GetVehicleModelAsync(VehicleModelById model)
        {
            var response = new ApiResponse<VehicleModelViewModel> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new GetVehicleModelByIdValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.Id, out Guid modelId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid vehicle model id");
                goto ReturnToCaller;
            }

            var existingModel = await UnitOfWork.Repository<VehicleModel>().GetByIdIncludingAsync(modelId, e => e.VehicleMake);
            if (existingModel == null)
            {
                response.Code = ApiResponseCodes.NOT_FOUND;
                response.Errors.Add($"Vehicle model does not exist or has been deleted");
                goto ReturnToCaller;
            }

            response.Payload = (VehicleModelViewModel)existingModel;
            response.Code = existingModel == null ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK;
            response.TotalCount = existingModel != null ? 1 : 0;
            response.Description = existingModel != null ? "Successful" : "No vehicle model record found";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        #region
        /*
        i think an extensive search covers this method like the GetAllVehicleModelsPaginated method 
        which would be replaced by an sp, 
        
        this method below should be return a list because of the possiblity of having another model in a different vehicle make
        
        it should also use contains because of front end on OnKeyUp search.

        In that case it is already covered in the GetAllVehicleModelsPaginated method which would use an SP
        
        else get by vehicle model Id like the GetVehicleModel above
        */

        /// <summary>
        /// This method gets vehicle model details by model id
        /// just parse the Guid
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public async Task<ApiResponse<List<VehicleModel>>> GetTopTenVehicleModel(VehicleModelSearch model)
        //{
        //    var response = new ApiResponse<List<VehicleModel>> { Code = ApiResponseCodes.OK };

        //    if (string.IsNullOrEmpty(model.Keyword))
        //    {
        //        response.Code = ApiResponseCodes.INVALID_REQUEST;
        //        response.Description = "Model can not be empty";
        //        goto ReturnToCaller;
        //    }
        //    var data = await Task.Run(() => UnitOfWork.Repository<VehicleModel>()
        //                         .GetAll(vehicleModel => vehicleModel.Title.Contains(model.Keyword, StringComparison.OrdinalIgnoreCase)));
        //    //var data = await Task.Run(() => GetAllAsync(vehicleModel => 
        //    //    (vehicleModel.Title.Contains(model.Keyword, StringComparison.OrdinalIgnoreCase))));

        //    response.Payload = data.Take(10).ToList();
        //    response.Code = data == null ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK;
        //    response.TotalCount = data != null ? data.Count() : 0;
        //    response.Description = data?.Count() > 0 ? "Successful" : "No vehicle model record found";

        //    ReturnToCaller:
        //    return response;
        //}
        #endregion

        /// <summary>
        /// This method gets vehicle models by thier make
        /// there's a basesearch view model... we can extend it when we want to add extra data
        /// 
        /// Just use it instead
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Task<ApiResponse<List<VehicleModel>>></returns>
        public async Task<ApiResponse<PaginatedList<VehicleModelViewModel>>> GetVehicleModelByMakeAsync(VehicleModelByMake model)
        {
            var response = new ApiResponse<PaginatedList<VehicleModelViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            model.PageIndex ??= 1;
            model.PageIndex ??= 50;


            var validationResult = new GetVehicleModelsByMakeValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.MakeId, out Guid makeId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid vehicle model id");
                goto ReturnToCaller;
            }

            var data = await Task.Run(() =>
            {
                return (from dataModel in UnitOfWork.Repository<VehicleModel>().GetAll().AsNoTracking()
                        join dataMake in UnitOfWork.Repository<VehicleMake>().GetAll().AsNoTracking() on dataModel.VehicleMakeId equals dataMake.Id
                        where dataModel.VehicleMakeId.Equals(makeId)
                        select new VehicleModelViewModel
                        {
                            Description = dataModel.Description,
                            Title = dataModel.Title,
                            VehicleMake = dataMake.Title,
                            Id = dataModel.Id.ToString(),
                            ShortDescription = dataModel.ShortDescription,
                        }
                        );
            });


            response.Payload = data.ToPaginatedList(model.PageIndex.Value, model.PageTotal.Value);
            response.TotalCount = response.TotalCount;
            response.PayloadMetaData = new PayloadMetaData(
                pageIndex: response.Payload.PageIndex,
                pageSize: response.Payload.PageSize,
                totalPageCount: response.Payload.TotalPageCount,
                totalCount: response.Payload.TotalCount);

            response.Code = response.TotalCount == 0 ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK;

            response.TotalCount = data.Count();
            response.Description = response.TotalCount > 0 ? "Successful" : "No vehicle model record found";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return await Task.FromResult(response);
        }

    }
}
