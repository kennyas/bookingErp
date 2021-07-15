using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.Models;

namespace Wallet.Core.ViewModels
{
    public class PaymentDetailViewModel : BaseEntity
    {      
        public string Reference { get; set; }       
        public string Amount { get; set; }       
        public string Email { get; set; }
        //public string Mobileno { get; set; }     
        public string PhoneNumber { get; set; }       
        public string UserId { get; set; }         
        public string Currency { get; set; }       
    }

    public class CreatePaymentDetail 
    {
     //   [Required]
     //   public string Reference { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Mobileno { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public string Currency { get; set; }
    }

    public class CreatePaymentDetailDto 
    {       
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        // public string Mobileno { get; set; }
        [Required]
        public string UserId { get; set; } 
        [Required]
        public string Currency { get; set; }
        public string PaystackReference { get; set; }
    }
}

