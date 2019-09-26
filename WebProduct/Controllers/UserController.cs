namespace WebApplication11.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
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
        /// Adds the products data.
        /// </summary>
        /// <param name="productInformation">The productInformation.</param>
        [HttpPost]
        [Route("addProduct")]
        public async Task<IActionResult> AddProduct(IFormFile file)
        {
            try
            {
                var result = await this.getCustomer.AddProduct(file);
                return this.Ok(result);
            }
            catch(Exception exception)
            {
                throw new Exception(exception.Message);
            }
           
        }

        /// <summary>
        /// Adds the product.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateProduct")]
        public async Task<IActionResult> UpdateProduct(IFormFile file)
        {
            try
            {
                var result = await this.getCustomer.UpdateProduct(file);
                return this.Ok(result);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

        }

        /// <summary>
        /// Updates the product.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>boolean result</returns>
        [HttpDelete]
        [Route("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(IFormFile file)
        {
            try
            {
                var result = await this.getCustomer.DeleteProduct(file);
                return this.Ok(result);
            }
            catch(Exception exception)
            {
                throw new Exception(exception.Message);
            }
          
        }

    }
}