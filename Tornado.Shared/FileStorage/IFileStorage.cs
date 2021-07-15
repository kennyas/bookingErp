using Microsoft.Extensions.FileProviders;
using System.IO;

namespace Tornado.Shared.FileStorage
{
    public interface IFileStorage : IFileInfo
    {
        string GetFileType();

        Stream OpenRead();

        Stream OpenWrite();

        Stream CreateFile();
    }
}