using System;
using System.Collections.Generic;

#nullable disable

namespace VirtualTrainer
{
    public partial class Contact
    {
        public int ContactId { get; set; }
        public string OwnerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
    }
}
