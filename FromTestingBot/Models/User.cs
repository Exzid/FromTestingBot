using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Models
{
    public class User
    {
        public long Id { get; set; }

        public long ChatId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(12)]
        public string Phone { get; set; }
        public DateTime Subscription { get; set; }

        public bool IsActive { get; set; }

        public bool IsAdmin { get; set; }

        public int IsWait { get; set; }
    }
}
