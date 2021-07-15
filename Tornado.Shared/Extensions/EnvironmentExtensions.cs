using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tornado.Shared.Extensions
{
    public static class EnvironmentExtensions
    {
        //const string StagingEnvironment = "Staging";
        //const string TestingEnvironment = "Testing";

        public static bool IsStaging(this IWebHostEnvironment env)
        {
            return env.IsStaging();
        }

        public static bool IsTesting(this IWebHostEnvironment env)
        {
            return env.IsTesting();
        }
    }
}
