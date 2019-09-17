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


namespace WebApplication11.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IGetCustomer getCustomer;
        public UserController(IGetCustomer getCustomer)
        {
            this.getCustomer = getCustomer;
        }


        [HttpGet]
        [Route("getCustomerData")]
        public IEnumerable<Cutomer> GetAllCustomers()
        {
            return this.getCustomer.GetCustomers();
        }

        [HttpGet]
        [Route("getMerchantData")]
        public IEnumerable<Merchant> GetAllMerchant()
        {
            return this.getCustomer.GetAllMerchant();
        }



        [HttpPost]
        [Route("addCustomer")]
        public int AddCustomer(Cutomer cutomer)
        {
            return this.getCustomer.AddCustomer(cutomer);
        }

        [HttpPost]
        [Route("addMerchant")]
        public int AddMerchantData(Merchant merchant)
        {
            return this.getCustomer.AddMerchantData(merchant);
        }


    }
}