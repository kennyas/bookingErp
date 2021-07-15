using Booking.Core.Dtos;
using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.Validators;
using Booking.Core.ViewModels;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.Timing;
using Tornado.Shared.ViewModels;

namespace Booking.Core.Services
{
    public class VehicleService : Service<Vehicle>, IVehicleService
    {
        private readonly IHttpUserService _currentUserService;
        private readonly IVehicleModelService _vehicleModelService;

        public VehicleService(IUnitOfWork unitOfWork, IHttpUserService currentUserService, IVehicleModelService vehicleModelService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
            _vehicleModelService = vehicleModelService;
        }

        public async Task<ApiResponse<VehicleViewModel>> CreateVehicleAsync(VehicleViewModel model)
        {
            var response = new ApiResponse<VehicleViewModel>();

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new CreateVehicleValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            var existingRecord = FirstOrDefault(p => string.Equals(p.RegistrationNumber, model.RegistrationNumber, StringComparison.OrdinalIgnoreCase));
            if (existingRecord != null)
            {
                response.Code = ApiResponseCodes.FAILED;
                response.Errors.Add("Record already exists");
                goto ReturnToCaller;
            }

            var selectedVehicleModel = _vehicleModelService.GetById(Guid.Parse(model.VehicleModelId));
            if (selectedVehicleModel == null)
            {
                response.Code = ApiResponseCodes.FAILED;
                response.Errors.Add("Vehicle model does not exist");
                goto ReturnToCaller;
            }

            var newVehicle = new Vehicle
            {
                RegistrationNumber = model.RegistrationNumber,
                ChassisNumber = model.ChassisNumber,
                //NoOfSeats = model.NoOfSeats,
                PartnerId = Guid.Parse(model.PartnerId),
                CreatedOn = DateTime.Now,
                VehicleModelId = selectedVehicleModel.Id,
                CreatedBy = _currentUserService.GetCurrentUser().UserId
            };

            await AddAsync(newVehicle);

            response.Code = ApiResponseCodes.OK;
            response.Payload = (VehicleViewModel)newVehicle;
            response.Description = "Successful";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<VehicleViewModel>> DeleteVehicleAsync(DeleteVehicleViewModel model)
        {
            var response = new ApiResponse<VehicleViewModel>();

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new DeleteVehicleValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.ERROR;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }


            var existingVehicleModel = await GetByIdAsync(Guid.Parse(model.Id));

            if (existingVehicleModel == null)
            {
                response.Code = ApiResponseCodes.ERROR;
                response.Errors.Add("Vehicle does not exist or has been deleted");
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

        public async Task<ApiResponse<VehicleViewModel>> EditVehicleAsync(EditVehicleViewModel model)
        {
            var response = new ApiResponse<VehicleViewModel>();

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new EditVehicleValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            var existingData = await GetByIdAsync(Guid.Parse(model.Id));

            if (existingData == null)
            {
                response.Code = ApiResponseCodes.NOT_FOUND;
                response.Errors.Add("Vehicle does not exist");
                goto ReturnToCaller;
            }

            // existingData.ModifiedBy = model. TODO: Add current user id that updated this record
            existingData.ModifiedOn = Clock.Now;
            existingData.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            existingData.ChassisNumber = model.ChassisNumber;
            existingData.RegistrationNumber = model.RegistrationNumber;
            existingData.VehicleModelId = Guid.Parse(model.VehicleModelId);
            //existingData.NoOfSeats = model.NoOfSeats;
            existingData.PartnerId = Guid.Parse(model.PartnerId);


            var isVehicleUpdated = await UpdateAsync(existingData) > 0;

            response.Code = isVehicleUpdated ? ApiResponseCodes.OK : ApiResponseCodes.FAILED;
            response.Description = isVehicleUpdated ? "Successful" : $"Could not update {model.RegistrationNumber ?? "vehicle"} at this time, please try again later";
            response.Payload = null;


        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<PaginatedList<VehicleViewModel>>> GetAllVehicleAsync(BasePaginatedViewModel model)
        {
            var response = new ApiResponse<PaginatedList<VehicleViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            model.PageIndex ??= 1;
            model.PageTotal ??= 50;
            var data = await Task.Run(() =>
            {
                return (from dataModel in UnitOfWork.Repository<Vehicle>().GetAll()
                        select new VehicleViewModel
                        {
                            CreatedId = dataModel.CreatedBy ?? Guid.Empty,
                            ChassisNumber = dataModel.ChassisNumber,
                            Id = dataModel.Id.ToString(),
                            //NoOfSeats = dataModel.NoOfSeats,
                            PartnerId = dataModel.PartnerId.ToString(),
                            RegistrationNumber = dataModel.RegistrationNumber,
                            VehicleModelId = dataModel.VehicleModelId.ToString()
                        });
            });

            response.Payload = data.ToPaginatedList(model.PageIndex.Value, model.PageTotal.Value);
            response.TotalCount = response.Payload.TotalCount;
            response.PayloadMetaData = new PayloadMetaData(
                        pageIndex: response.Payload.PageIndex,
                        pageSize: response.Payload.PageSize,
                        totalPageCount: response.Payload.TotalPageCount,
                        totalCount: response.Payload.TotalCount);
            response.Code = response.TotalCount == 0 ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK;
            response.Description = response.TotalCount > 0 ? "Successful" : "No vehicle record found";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<VehicleViewModel>> GetVehicleByChasisNumberAsync(SearchVehicleByChassisNumberViewModel model)
        {
            var response = new ApiResponse<VehicleViewModel> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new SearchVehicleByChassisNumberValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            var data = await Task.Run(() => FirstOrDefault(vehicle => string.Equals(vehicle.ChassisNumber, model.ChassisNumber, StringComparison.OrdinalIgnoreCase)));

            if (data == null)
            {
                response.Code = ApiResponseCodes.NOT_FOUND;
                response.Errors.Add("Vehicle with chassis number could not be found");
                goto ReturnToCaller;
            }
            response.Payload = (VehicleViewModel)data;
            response.Code = data != null ? ApiResponseCodes.OK : ApiResponseCodes.NOT_FOUND;
            response.TotalCount = data != null ? 1 : 0;
            response.Description = data != null ? "Successful" : "No vehicle record found";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<PaginatedList<VehicleViewModel>>> GetVehicleByPartnerIdAsync(GetVehicleByPartnerIdViewModel model)
        {
            var response = new ApiResponse<PaginatedList<VehicleViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            model.PageIndex ??= 0;
            model.PageTotal ??= 50;

            var validationResult = new SearchVehicleByPartnerIdValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.PartnerId, out Guid partnerId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Invalid partner ID");
                goto ReturnToCaller;
            }

            var data = await Task.Run(() =>
            {
                return (from dataModel in UnitOfWork.Repository<Vehicle>().GetAll()
                        where dataModel.PartnerId == partnerId
                        select new VehicleViewModel
                        {
                            CreatedId = dataModel.CreatedBy ?? Guid.Empty,
                            ChassisNumber = dataModel.ChassisNumber,
                            Id = dataModel.Id.ToString(),
                            //NoOfSeats = dataModel.NoOfSeats,
                            PartnerId = dataModel.PartnerId.ToString(),
                            RegistrationNumber = dataModel.RegistrationNumber,
                            VehicleModelId = dataModel.VehicleModelId.ToString()
                        });
            });

            response.Payload = data.ToPaginatedList(model.PageIndex.Value, model.PageTotal.Value);
            response.TotalCount = response.Payload.TotalCount;
            response.PayloadMetaData = new PayloadMetaData(
                        pageIndex: response.Payload.PageIndex,
                        pageSize: response.Payload.PageSize,
                        totalPageCount: response.Payload.TotalPageCount,
                        totalCount: response.Payload.TotalCount);
            response.Code = response.TotalCount == 0 ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK;
            response.Description = response.TotalCount > 0 ? "Successful" : "No vehicle record found";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<VehicleViewModel>> GetVehicleByRegistrationNumberAsync(SearchVehicleByRegistrationNumberViewModel model)
        {
            var response = new ApiResponse<VehicleViewModel> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new SearchVehicleByRegistrationNumberValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            var data = await Task.Run(() => FirstOrDefault(vehicle => string.Equals(vehicle.RegistrationNumber, model.RegistrationNumber, StringComparison.OrdinalIgnoreCase)));
            if (data == null)
            {
                response.Code = ApiResponseCodes.NOT_FOUND;
                response.Errors.Add("Vehicle with registration number was not found");
                goto ReturnToCaller;
            }
            response.Payload = (VehicleViewModel)data;
            response.Code = data != null ? ApiResponseCodes.OK : ApiResponseCodes.NOT_FOUND;
            response.TotalCount = data != null ? 1 : 0;
            response.Description = data != null ? "Successful" : "No vehicle record found";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<PaginatedList<VehicleViewModel>>> SearchVehicleAsync(VehiclePaginatedViewModel model)
        {
            var response = new ApiResponse<PaginatedList<VehicleViewModel>> { Code = ApiResponseCodes.OK };
            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Model can not be empty";
                goto ReturnToCaller;
            }
            var validationResult = new SearchVehicleValidator().Validate(model);
            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            model.PageTotal ??= 25;
            model.PageIndex ??= 0;


            //WE DONT NEED IF ELSE TO CHECK IF KEYWORD IS NULL
            var data = await Task.Run(() =>
            {

                    return (from v in UnitOfWork.Repository<Vehicle>().GetAll()
                            join vmodel in UnitOfWork.Repository<VehicleModel>().GetAll() on v.VehicleModelId equals vmodel.Id
                            join vmake in UnitOfWork.Repository<VehicleMake>().GetAll() on vmodel.VehicleMakeId equals vmake.Id
                            where v.ChassisNumber.Contains(model.Keyword)
                            || v.RegistrationNumber.Contains(model.Keyword)
                            || vmodel.Title.Contains(model.Keyword)
                            || vmake.Title.Contains(model.Keyword)
                            || (string.IsNullOrEmpty(model.Keyword))

                            select new VehicleViewModel
                            {
                                CreatedId = v.CreatedBy ?? Guid.Empty,
                                ChassisNumber = v.ChassisNumber,
                                Id = v.Id.ToString(),
                                //NoOfSeats = v.NoOfSeats,
                                RegistrationNumber = v.RegistrationNumber,
                                PartnerId = v.PartnerId.ToString(),
                                VehicleModelId = v.VehicleModelId.ToString()
                            });
            });

            response.Payload = data.ToPaginatedList(model.PageIndex.Value, model.PageTotal.Value); ;
            response.PayloadMetaData = new PayloadMetaData(
              pageIndex: response.Payload.PageIndex,
              pageSize: response.Payload.PageSize,
              totalPageCount: response.Payload.TotalPageCount,
              totalCount: response.Payload.TotalCount);

            response.TotalCount = response.Payload.TotalCount;
            response.Code = ApiResponseCodes.OK;
            response.Description = response.TotalCount > 0 ? "Successful" : "No vehicle record found";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<List<TripVehicleViewModel>>> GetUnattachedVehicles(TripVehicleSearchModel model)
        {
            if (model == null)
            {
                return new ApiResponse<List<TripVehicleViewModel>> ( codes : ApiResponseCodes.INVALID_REQUEST, message: "Model  state is invalid");
            }
            if (!Guid.TryParse(model.VehicleModelId, out Guid vehicleModelId))
            {
                return new ApiResponse<List<TripVehicleViewModel>>(codes: ApiResponseCodes.INVALID_REQUEST, message: "Model  state is invalid");
            }

            //SqlParameter vModelId = new SqlParameter { DbType = System.Data.DbType.Guid, Value = vehicleModelId, SqlDbType = System.Data.SqlDbType.UniqueIdentifier
            //                                                                , ParameterName = "@vModelId"
            //};

            var allUnattachedVehicles = await Task.Run(() => SqlQuery<TripVehicleViewModel>($"select distinct v.Id , v.RegistrationNumber from Vehicle v left join Trip tr on v.Id = tr.VehicleId " +
                                                                                            $"where VehicleModelId = '{vehicleModelId.ToString()}'  and tr.Id is null").ToList());
            if (allUnattachedVehicles == null)
            {
                return new ApiResponse<List<TripVehicleViewModel>>(codes : ApiResponseCodes.NOT_FOUND, message : "Unattached vehicles for this vehicle model not found" );
            }

            return new ApiResponse<List<TripVehicleViewModel>>(codes: allUnattachedVehicles.Count() > 0 ? ApiResponseCodes.OK : ApiResponseCodes.NOT_FOUND,
                                                               message: allUnattachedVehicles.Count() > 0 ? "Successful" : "Unsuccessful",
                                                               data : allUnattachedVehicles);
        }

    }
}
