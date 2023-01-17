using Library.Model.Enum;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Card
    {
        public string UserID { get; set; }

        [PrimaryKey]
        public string CardNumber { get; set; }
        public CardType CardType { get; set; }
        public string PinNumber { get; set; }
        public double TotalCharges { get; set; }
        public string CardHolder { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTill { get; set; }
      
        public Card(string cardNumber, CardType cardType, string pinNumber, string validTill, string validFrom)
        {
            CardNumber = cardNumber;
            CardType = cardType;
            PinNumber = pinNumber;
            ValidTill = validTill;
            ValidFrom = validFrom;
        }
    }

    public class CreditCard : Card
    {
        public CreditCard(string cardNumber, CardType cardType, string pinNumber, string validTill, string validFrom) : base(cardNumber, cardType, pinNumber, validTill, validFrom)
        {
        }

        public double TotalLimit { get; set; }
        public double AvailableCredit { get; set; }
        public string PaymentDueDate { get; set; }
        public double TotalAmoutDue { get; set; }
        public double DeducedAmount {get; set;}

    }
}
