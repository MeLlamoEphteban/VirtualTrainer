using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class Invoice
    {
        public int IdInvoice { get; set; }
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public int IdSubscription { get; set; }
        public string SubName { get; set; }
        public DateTime IssuedDate { get; set; }

        public virtual Subscription IdSubscriptionNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
    }
}
