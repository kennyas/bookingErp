using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Context;
using Tornado.Shared.Models;

namespace Tornado.Shared.Test.Fakes
{
    public class FakeUserStore<TContext> : UserStore<GigmUser, GigmRole, TContext, Guid,
        GigmUserClaim, GigmUserRole,
        GigmUserLogin, GigmUserToken, GigmRoleClaim> where TContext : AuthDbContext
    {
        public FakeUserStore(TContext context, IdentityErrorDescriber describer = null) :
            base(context, describer)
        {
        }
    }
}
