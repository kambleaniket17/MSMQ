using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Interfaces
{
   public interface IGetCustomer
    {
         IEnumerable<Cutomer> GetCustomers();
         IEnumerable<Merchant> GetAllMerchant();

         int AddCustomer(Cutomer cutomer);
         int AddMerchantData(Merchant merchant);
    }
}
