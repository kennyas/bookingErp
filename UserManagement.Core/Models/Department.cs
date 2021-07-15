using Tornado.Shared.Models;

namespace UserManagement.Core.Models
{
    public class Department : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
