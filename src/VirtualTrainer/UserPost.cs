using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualTrainer
{
    public partial class UserPost
    {
        public int Iduser { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string UserAspNet { get; set; }
        public int Idsubscription { get; set; }
        public virtual UserSubscription UserSubscription { get; set; }
    }
}
