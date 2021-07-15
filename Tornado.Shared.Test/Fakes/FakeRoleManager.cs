using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models;

namespace Tornado.Shared.Test.Fakes
{
    public class FakeRoleManager : RoleManager<GigmRole>
    {
        public FakeRoleManager(IRoleStore<GigmRole> store, IEnumerable<IRoleValidator<GigmRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<GigmRole>> logger) :
            base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }
    }
}
