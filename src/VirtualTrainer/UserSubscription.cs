using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class UserSubscription
    {
        public int Id { get; set; }
        public int Iduser { get; set; }
        public int Idsubscription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual Subscription IdsubscriptionNavigation { get; set; }
        public virtual User IduserNavigation { get; set; }
    }
}
