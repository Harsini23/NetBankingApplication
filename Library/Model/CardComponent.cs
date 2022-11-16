using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class CardComponent
    {
        public string Heading { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }

        public CardComponent(string heading, string title, string content, string image)
        {
            Heading = heading;
           Title = title;
            Content = content;
            Image = image;
        }
    }
}
