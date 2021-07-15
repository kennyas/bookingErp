using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.ViewModels;
using UserManagement.Core.Models;
using UserManagement.Core.Dtos;

namespace UserManagement.Core.ViewModels
{
  public  class DepartmentViewModel : BaseViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }    
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }


        public static explicit operator DepartmentViewModel(DepartmentDto source)
        {
            var department = new DepartmentViewModel
            {
                Id = source?.Id.ToString(),
                Name = source.Name,
                CreatedBy = source.CreatedBy,
                ModifiedBy = source.ModifiedBy
            };
            return department;
        }

        public static explicit operator DepartmentViewModel(Department source)
        {
            var department = new DepartmentViewModel
            {
                Id = source?.Id.ToString(),
                Name = source.Name
            };
            return department;
        }
    }

    public class DepartmentSearchVModel : BaseSearchViewModel
    {
       // [Required]
       // public string DepartmentId { get; set; }
    }
}
