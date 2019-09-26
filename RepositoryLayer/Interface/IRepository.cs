namespace RepositoryLayer.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Models;

    /// <summary>
    /// Interface IRepository
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Gets the customers.
        /// </summary>
        /// <returns>List OF Customer</returns>
        IEnumerable<Customer> GetCustomers();

        /// <summary>
        /// Gets all merchant.
        /// </summary>
        /// <returns>List OF Merchant</returns>
        IEnumerable<Merchant> GetAllMerchant();

        /// <summary>
        /// Adds the customer.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>Boolean Result</returns>
        bool AddCustomer(Customer customer);

        /// <summary>
        /// Adds the merchant data.
        /// </summary>
        /// <param name="merchant">The merchant.</param>
        void AddMerchantData(Merchant merchant);

        /// <summary>
        /// Logins the specified login.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns>Token And Redis Time</returns>
        List<string> Login(Login login);

        /// <summary>
        /// Registers the specified register.
        /// </summary>
        /// <param name="register">The register.</param>
        /// <returns>Boolean Result</returns>
        bool Register(Register register);

        /// <summary>
        /// Adds the product.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Boolean result</returns>
        Task<bool> AddProduct(List<ProductInformation> products);

        /// <summary>
        /// Updates the product.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>boilean result</returns>
        Task<bool> UpdateProduct(List<ProductInformation> products);

        /// <summary>
        /// Gets the product details.
        /// </summary>
        /// <returns>list of Products</returns>
        List<ProductInformation> GetProductDetails();

        /// <summary>
        /// Deletes the product.
        /// </summary>
        /// <param name="products">The products.</param>
        /// <returns>boolean result</returns>
        Task<bool> DeleteProduct(List<ProductInformation> products);
    }
}