using Audit.Core.Events;
using Audit.Core.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Tornado.Shared.AuditLogEvent;
using Tornado.Shared.ViewModels;

namespace Audit.Core.Services.Interfaces
{
    public interface IAuditService
    {
        Task<List<ValidationResult>> CreateAuditLogAsync(AuditLogIntegrationEvent model); 
    }
}
