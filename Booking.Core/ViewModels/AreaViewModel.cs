using Booking.Core.Dtos;
using Booking.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.ViewModels;

namespace Booking.Core.ViewModels
{
    public class AreaListViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string StateName { get; set; }

        public Guid StateId { get; set; }

        public string Code { get; set; }
        public string StateCode { get; set; }
    }

    public class CreateAreaViewModel
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string StateId { get; set; }
        public string Description { get; set; }
        public string AreaCode { get; set; }
    }
    public class EditAreaViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string StateId { get; set; }
        public string Description { get; set; }
        public string AreaCode { get; set; }
    }

    public class AreaViewModel
    {
        public string Title { get; set; }
        public string StateId { get; set; }
        public string Description { get; set; }
        public string AreaCode { get; set; }
        public string StateName { get; set; }
        public string Id { get; set; }



    }
    public class AreaSearchViewModel : BaseSearchViewModel
    {
     
    }
}
