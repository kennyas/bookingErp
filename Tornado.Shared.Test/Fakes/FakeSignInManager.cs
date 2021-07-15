using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models;

namespace Tornado.Shared.Test.Fakes
{
    public class FakeSignInManager : SignInManager<GigmUser>
    {
        public FakeSignInManager()
            : base(new Mock<FakeUserManager>().Object,
                  new HttpContextAccessor(),
                  new Mock<IUserClaimsPrincipalFactory<GigmUser>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<ILogger<SignInManager<GigmUser>>>().Object,
                  new Mock<IAuthenticationSchemeProvider>().Object, new Mock<IUserConfirmation<GigmUser>>().Object)
        { }
    }
}
