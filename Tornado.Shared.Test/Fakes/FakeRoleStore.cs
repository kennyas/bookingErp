using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Context;
using Tornado.Shared.Models;

namespace Tornado.Shared.Test.Fakes
{
    public class FakeRoleStore<TContext> : RoleStore<GigmRole, TContext, Guid> where TContext : AuthDbContext
    {
        public FakeRoleStore(TContext context, IdentityErrorDescriber describer = null) :
            base(context, describer)
        {
        }
    }
}
