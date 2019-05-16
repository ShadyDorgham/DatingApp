using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaingApp.API.Dtos
{
    public class MessageFromCreationDto
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime MessageSent { get; set; }
        public string Content { get; set; }

        public MessageFromCreationDto()
        {
                MessageSent = DateTime.Now;
                
        }






    }
}
