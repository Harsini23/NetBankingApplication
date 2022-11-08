using Library.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Card
    {
        public string CardNumber { get; set; }
        public CardType CardType { get; set; }
        public string PinNumber { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTill { get; set; }

        public Card(string cardNumber, CardType cardType, string pinNumber, DateTime validTill, DateTime validFrom)
        {
            CardNumber = cardNumber;
            CardType = cardType;
            PinNumber = pinNumber;
            ValidTill = validTill;
            ValidFrom = validFrom;
        }
    }
}
