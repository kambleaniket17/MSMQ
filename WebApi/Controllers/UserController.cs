namespace WebApplication11.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using BusinessLogic.Interfaces;
    using Experimental.System.Messaging;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    /// <summary>
    /// Controller for controlling the User Operations
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        /// <summary>
        /// The get customer
        /// </summary>
        private readonly IGetCustomer getCustomer;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="getCustomer">The get customer.</param>
        public UserController(IGetCustomer getCustomer)
        {
            this.getCustomer = getCustomer;
        }

        /// <summary>
        /// Gets all customers.
        /// </summary>
        /// <returns>IEnumerable Customers Data</returns>
        [HttpGet]
        [Route("getCustomerData")]
        public IEnumerable<Customer> GetAllCustomers()
        {
            return this.getCustomer.GetCustomers();
        }

        /// <summary>
        /// Gets all merchant.
        /// </summary>
        /// <returns>IEnumerable Merchant Data</returns>
        [HttpGet]
        [Route("getMerchantData")]
        public IEnumerable<Merchant> GetAllMerchant()
        {
            return this.getCustomer.GetAllMerchant();
        }

        /// <summary>
        /// Adds the customer.
        /// </summary>
        /// <param name="cutomer">The cutomer.</param>
        /// <returns>Boolean result</returns>
        [HttpPost]
        [Route("addCustomer")]
        public bool AddCustomer(Customer cutomer)
        {
            return this.getCustomer.AddCustomer(cutomer);
        }

        /// <summary>
        /// Adds the merchant data.
        /// </summary>
        /// <param name="merchant">The merchant.</param>
        /// <returns>Boolean Result</returns>
        [HttpPost]
        [Route("addMerchant")]
        public void AddMerchantData(Merchant merchant)
        {
            this.getCustomer.AddMerchantData(merchant);
        }

        /// <summary>
        /// Gets the redis time.
        /// </summary>
        /// <returns>time</returns>
        [HttpGet]
        [Route("getRedis")]
        public string GetRedisTime()
        {
           return this.getCustomer.GetRedis();
        }
    }
}