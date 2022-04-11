using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualTrainer.Models;

namespace VirtualTrainer.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SaliFitnessContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
