using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models;

namespace Tornado.Shared.Test.Fakes
{
    public class FakeUserManager : UserManager<GigmUser>
    {
        public FakeUserManager(
            IUserStore<GigmUser> store,
                  IOptions<IdentityOptions> options,
            IPasswordHasher<GigmUser> password,
             IEnumerable<IUserValidator<GigmUser>> userValidator,
             IEnumerable<IPasswordValidator<GigmUser>> passwordValidator,
                  ILookupNormalizer lookupNormalizer,
                 IdentityErrorDescriber identityErrorDescriber,
                IServiceProvider serviceProvider,
               ILogger<FakeUserManager> logger)
            : base(store, options, password, userValidator, passwordValidator, lookupNormalizer
                  , identityErrorDescriber, serviceProvider, logger)
        {

        }



    }
}
