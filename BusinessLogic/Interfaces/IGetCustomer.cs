namespace BusinessLogic.Interfaces
{
    using System;
    using Models;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

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

        /// <summary>
        /// Adds the product.
        /// </summary>
        /// <param name="productInformation">The product information.</param>
        /// <returns>Boolean Result</returns>
        Task<bool> AddProduct(IFormFile file);

        /// <summary>
        /// Updates the product.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>boolean result</returns>
        Task<List<bool>> UpdateProduct(IFormFile file);

        /// <summary>
        /// Deletes the product.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>boolean Result</returns>
        Task<bool> DeleteProduct(IFormFile file);

        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>List Of Products</returns>
        IEnumerable<ProductInformation> GetProduct();

    }
}
