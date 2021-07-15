using System;
using System.Collections.Generic;
using System.Text;

namespace Wallet.Core.Models
{
    public class PaystackObjectV1
    {
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
        }

        public class Customer
        {
            public string id { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string customer_code { get; set; }
            public string phone { get; set; }
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
            public string id { get; set; }
            public string domain { get; set; }
            public string status { get; set; }
            public string reference { get; set; }
            public string amount { get; set; }
            public string message { get; set; }
            public string gateway_response { get; set; }
            public string paid_at { get; set; }
            public string created_at { get; set; }
            public string channel { get; set; }
            public string currency { get; set; }
            public string ip_address { get; set; }
            public Metadata metadata { get; set; }
            public Log log { get; set; }
            public string fees { get; set; }
            public object fees_split { get; set; }
            public Authorization authorization { get; set; }
            public Customer customer { get; set; }
            public object plan { get; set; }
            public object order_id { get; set; }
            public string paidAt { get; set; }
            public string createdAt { get; set; }
            public string transaction_date { get; set; }
            public PlanObject plan_object { get; set; }
            public Subaccount subaccount { get; set; }
        }

        public class PayStackbjectV 
        {
            public bool status { get; set; }
            public string message { get; set; }
            public Data data { get; set; }
        }
    }
}
