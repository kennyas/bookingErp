using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models;

namespace Wallet.Core.ViewModels
{
    public class CardDetailsViewModel 
    {
        public Guid CustomerId { get; set; }

        public string LastFourDigits { get; set; }

        public string CardType { get; set; }
        public string AuthCode { get; set; }


    }

    public class CreateCardDetailsViewModel
    {
        public string CustomerId { get; set; }
        public string LastFourDigits { get; set; }
        public string CardType { get; set; }
        public string AuthCode { get; set; }

    }


    public class DeleteCardViewModel
    {
        public string CardDetailsId { get; set; }
        public string CustomerId { get; set; }

    }
    public class GetCardDetailsRequestViewModel
    {
        public string CustomerId { get; set; }

    }
}
