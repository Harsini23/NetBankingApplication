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
}
