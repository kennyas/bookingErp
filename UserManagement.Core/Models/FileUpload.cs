using Tornado.Shared.Models;

namespace UserManagement.Core.Models
{
    public class FileUpload: BaseEntity
    {
        public string Path { get; set; }
        public string Size { get; set; }
        public string Name { get; set; }
    }
}