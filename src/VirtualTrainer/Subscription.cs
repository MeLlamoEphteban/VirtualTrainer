using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class Subscription
    {
        public Subscription()
        {
            UserSubscriptions = new HashSet<UserSubscription>();
        }

        public int Idsubscription { get; set; }
        public string SubName { get; set; }
        public string AllowedTimeInterval { get; set; }

        public virtual ICollection<UserSubscription> UserSubscriptions { get; set; }
    }
}
