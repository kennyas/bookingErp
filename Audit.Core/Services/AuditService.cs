using Audit.Core.Events;
using Audit.Core.Models;
using Audit.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.AuditLogEvent;
using Tornado.Shared.Dapper.Interfaces;
using Tornado.Shared.Dapper.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.Timing;
using Tornado.Shared.ViewModels;

namespace Audit.Core.Services
{
    public class AuditService : Service<AuditLog>, IAuditService
    {
        private readonly IHttpUserService _currentUserService;
        private readonly IConfiguration configuration;

        public AuditService(IUnitOfWork unitOfWork, IHttpUserService currentUserService, IConfiguration iconfig) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
            configuration = iconfig;
        }
        public async Task<List<ValidationResult>> CreateAuditLogAsync(AuditLogIntegrationEvent model)
        {
            try
            {
                if (model == null)
                {
                    //  return new ApiResponse<AuditLog>(codes: ApiResponseCodes.INVALID_REQUEST, errors: "invalid request");
                    results.Add(new ValidationResult($"AuditLog: invalid request"));
                    return results;
                }

                var currentUserId = _currentUserService.GetCurrentUser().UserId;
                // Guid.TryParse(model.UserId, out Guid UserId);

                var audit = new AuditLog
                {
                    CreatedBy = currentUserId,
                    CreatedOn = Clock.Now,
                    IsDeleted = false,
                    ActionType = model.ActionType,
                    UserId = model.UserId,
                    ModifiedBy = currentUserId,
                    Channel = model.Channel,
                    Comments = model.Comments,
                    Ipaddress = model.Ipaddress,
                    ModifiedOn = Clock.Now,
                    ModuleName = model.ModuleName,
                    Parameters = model.TransParams
                };
                await AddAsync(audit);
                // return new ApiResponse<AuditLog>(message: "Successful", codes: ApiResponseCodes.OK, data: audit);
                results.Add(new ValidationResult($"AuditLog: Successful"));
                return results;
            }
            catch (Exception ex)
            {
                //return new ApiResponse<AuditLog>(errors: "Could not create audit log: " + ex.Message, codes: ApiResponseCodes.EXCEPTION);
                results.Add(new ValidationResult($"AuditLog: Could not create audit log|" + ex.Message));
                return results;
            }
        }
    }
}
