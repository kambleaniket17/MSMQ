namespace WebApplication11.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BusinessLogic.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    /// <summary>
    /// Admin Controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        /// <summary>
        /// The customer
        /// </summary>
        IGetCustomer customer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="customer">The customer.</param>
        public AdminController(IGetCustomer customer)
        {
            this.customer = customer;
        }

        /// <summary>
        /// Logins the specified login.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns>Token And Time</returns>
        [HttpPost]
       [Route("login")]
       public List<string> Login(Login login)
        {
           return this.customer.Login(login);
        }

        /// <summary>
        /// Registrations the specified register.
        /// </summary>
        /// <param name="register">The register.</param>
        /// <returns>boolean result</returns>
        [HttpPost]
        [Route("Register")]
        public bool Registration(Register register)
        {
            return this.customer.Register(register);
        }
    }
}