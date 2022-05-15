using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime IssuedDate { get; set; }
        public string Value { get; set; }

        public virtual Subscription IdSubscriptionNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
    }
}
