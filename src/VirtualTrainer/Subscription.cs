using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class Subscription
    {
        public Subscription()
        {
            Invoices = new HashSet<Invoice>();
            UserSubscriptions = new HashSet<UserSubscription>();
        }

        public int Idsubscription { get; set; }
        public string SubName { get; set; }
        public string AllowedTimeInterval { get; set; }
        public int Price { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<UserSubscription> UserSubscriptions { get; set; }
    }
}
