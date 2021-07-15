using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Tornado.Shared.EF
{
    public static class SprocRunSetup
    {
        public static void ApplyStoredProcedures(this IDbContext context, string[] sprocs)
        {
            foreach (var i in sprocs)
            {
                context.ExecuteSqlCommand($@"IF EXISTS (SELECT * FROM dbo.sysobjects 
                                        where id = object_id(N'[dbo].[{i}]')
                                        and OBJECTPROPERTY(id, N'IsProcedure') = 1)
                                        DROP PROCEDURE [dbo].[{i}];");

                string content = GetFileContentWithName($"{i}.sql");
                context.ExecuteSqlCommand(content);
            }
        }

        public static String GetFileContentWithName(string filePath)
        {
            string sqlContent = "";
            var baseDir = $@"{AppDomain.CurrentDomain.BaseDirectory}";
            if (Directory.Exists($"{baseDir}\bin"))
            {
                Debug.WriteLine("Test");
                sqlContent = File.ReadAllText(String.Format(@"{0}\bin\Scripts\{1}", baseDir, filePath));
                Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
                sqlContent = appPathMatcher.Match(sqlContent).Value;
            }
            else
            {
                Debug.WriteLine("Second Test");
                sqlContent = File.ReadAllText(String.Format(@"{0}\Scripts\{1}", baseDir, filePath));
                //Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
                //sqlContent = appPathMatcher.Match(sqlContent).Value;
            }

            return sqlContent;
        }
    }
}
