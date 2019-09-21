namespace BusinessLogic.Interfaces
{
    using System;
    using Models;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Interface for Employee Operations
    /// </summary>
    public interface IGetCustomer
    {
        /// <summary>
        /// Gets the customers.
        /// </summary>
        /// <returns>IEnumerable Customer List</returns>
        IEnumerable<Customer> GetCustomers();

        /// <summary>
        /// Gets all merchant.
        /// </summary>
        /// <returns>IEnumerable Merchant List</returns>
        IEnumerable<Merchant> GetAllMerchant();

        /// <summary>
        /// Adds the customer.
        /// </summary>
        /// <param name="customer">The cutomer.</param>
        /// <returns>Boolean Result</returns>
        bool AddCustomer(Customer cutomer);

        /// <summary>
        /// Adds the merchant data.
        /// </summary>
        /// <param name="merchant">The merchant.</param>
        void AddMerchantData(Merchant merchant);

        /// <summary>
        /// Gets the redis.
        /// </summary>
        /// <returns>Redis Data</returns>
        string GetRedis();

        /// <summary>
        /// Logins the specified login.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns>String last Time</returns>
        List<string> Login(Login login);

        /// <summary>
        /// Registers the specified register.
        /// </summary>
        /// <param name="register">The register.</param>
        /// <returns>Boolean Value</returns>
        bool Register(Register register);
    }
}
