using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.Models;

namespace Wallet.Core.Models
{
    public class PaymentDetail : BaseEntity
    {
        [MaxLength(1000)]
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        [MaxLength(300)]
        public string Email { get; set; }
        [MaxLength(200)]
        public string PhoneNumber { get; set; }
        //public string Mobileno { get; set; }
        public Guid CustomerId { get; set; }
        [MaxLength(300)]
        public string Ip_address { get; set; }
        [MaxLength(300)]
        public string Currency { get; set; }
        [MaxLength(300)]
        public string TransStatus { get; set; }
        [MaxLength(1000)]
        public string Message { get; set; }
        //[MaxLength(1000)]
        public string Gateway_response { get; set; }
        public bool IsUsed { get; set; }

        //just added
        [MaxLength(300)]
        public string DanfoReference { get; set; }
    }

    public class PaymentDetaildto : BaseEntity
    {
        [MaxLength(1000)]
        public string Reference { get; set; } 
        public decimal Amount { get; set; }
        [MaxLength(300)]
        public string Email { get; set; }
        [MaxLength(200)]
        public string PhoneNumber { get; set; } 
        public Guid CustomerId { get; set; }       
        [MaxLength(300)]
        public string Currency { get; set; }
        [MaxLength(300)]
        public string TransStatus { get; set; }
        [MaxLength(1000)]
        public string Message { get; set; }       
    }

    public class CustomField
    {
        public string display_name { get; set; }
        public string variable_name { get; set; }
        public string value { get; set; }
    }

    public class Metadata
    {
        public List<CustomField> custom_fields { get; set; }
        public string referrer { get; set; }
    }

    public class History
    {
        public string type { get; set; }
        public string message { get; set; }
        public int time { get; set; }
    }

    public class Log
    {
        public int start_time { get; set; }
        public int time_spent { get; set; }
        public int attempts { get; set; }
        public string authentication { get; set; }
        public int errors { get; set; }
        public bool success { get; set; }
        public bool mobile { get; set; }
        public List<object> input { get; set; }
        public List<History> history { get; set; }
    }

    


    public class Authorization
    {
        public string authorization_code { get; set; }
        public string bin { get; set; }
        public string last4 { get; set; }
        public string exp_month { get; set; }
        public string exp_year { get; set; }
        public string channel { get; set; }
        public string card_type { get; set; }
        public string bank { get; set; }
        public string country_code { get; set; }
        public string brand { get; set; }
        public bool reusable { get; set; }
        public string signature { get; set; }
        public object account_name { get; set; }
    }

    public class Customer
    {
        public int id { get; set; }
        public object first_name { get; set; }
        public object last_name { get; set; }
        public string email { get; set; }
        public string customer_code { get; set; }
        public object phone { get; set; }
        public object metadata { get; set; }
        public string risk_action { get; set; }
    }

    public class PlanObject
    {
    }

    public class Subaccount
    {
    }

    public class Data
    {
        public int id { get; set; }
        public string domain { get; set; }
        public string status { get; set; }
        public string reference { get; set; }
        public string amount { get; set; }
        public object message { get; set; }
        public string gateway_response { get; set; }
        public DateTime paid_at { get; set; }
        public DateTime created_at { get; set; }
        public string channel { get; set; }
        public string currency { get; set; }
        public object ip_address { get; set; }
        public int metadata { get; set; }
        public object log { get; set; }
        public int fees { get; set; }
        public object fees_split { get; set; }
        public Authorization authorization { get; set; }
        public Customer customer { get; set; }
        public object plan { get; set; }
        public object order_id { get; set; }
        public DateTime paidAt { get; set; }
        public DateTime createdAt { get; set; }
        public object requested_amount { get; set; }
        public DateTime transaction_date { get; set; }
        public PlanObject plan_object { get; set; }
        public Subaccount subaccount { get; set; }
    }

    public class PayStackbject 
    {
        public bool status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }
}

