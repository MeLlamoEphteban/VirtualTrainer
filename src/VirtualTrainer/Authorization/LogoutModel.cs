using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace VirtualTrainer.Account
{
    public class LogoutModel
    {
        public SignInManager<IdentityUser> _signInManager;
        public ILogger<LogoutModel> _logger;

        public LogoutModel()
        {

        }
    }
}
