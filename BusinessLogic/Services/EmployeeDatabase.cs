namespace BusinessLogic
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.IdentityModel.Tokens.Jwt;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using BusinessLogic.Interfaces;
    using Bytescout.Spreadsheet;
    using Experimental.System.Messaging;
    using GemBox.Spreadsheet;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using OfficeOpenXml;
    using RepositoryLayer.Interface;
    using ServiceStack.Redis;
    using StackExchange.Redis;


    /// <summary>
    /// class For Employee Operations
    /// </summary>
    /// <seealso cref="BusinessLogic.Interfaces.IGetCustomer" />
    public class EmployeeDatabase : IGetCustomer
    {
        /// <summary>
        /// The get customer
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="getCustomer">The get customer.</param>
        public EmployeeDatabase(IRepository repository)
        {
            this.repository = repository;
        }


        // string connectionstring=
        /// <summary>
        /// Gets the customers.
        /// </summary>
        /// <returns>IEnumerable Data</returns>
        /// <exception cref="Exception">Check the Exceptions</exception>
        public IEnumerable<Customer> GetCustomers()
        {

            try
            {
                return repository.GetCustomers();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Gets all merchant.
        /// </summary>
        /// <returns>IEnumerable Data</returns>
        /// <exception cref="Exception">Check The Exceptions</exception>
        public IEnumerable<Merchant> GetAllMerchant()
        {

            try
            {
                return repository.GetAllMerchant();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Adds the customer.
        /// </summary>
        /// <param name="cutomer">The cutomer.</param>
        /// <returns>Boolean Result</returns>
        /// <exception cref="Exception">Check The Exceptions</exception>
        public bool AddCustomer(Customer cutomer)
        {
            try
            {
                return repository.AddCustomer(cutomer);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Adds the merchant data.
        /// </summary>
        /// <param name="merchant">The merchant.</param>
        /// <returns>Boolean Result</returns>
        /// <exception cref="Exception">Check the Exceptions</exception>
        public void AddMerchantData(Merchant merchant)
        {
            try
            {
                repository.AddMerchantData(merchant);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// User Login Method
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns>string token And Time</returns>
        public List<string> Login(Login login)
        {
            return repository.Login(login);
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="register">The register.</param>
        /// <returns></returns>
        public bool Register(Register register)
        {
            try
            {
                return repository.Register(register);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);

            }

        }

        /// <summary>
        /// Adds the product.
        /// </summary>
        /// <param name="file">The .file</param>
        /// <returns>
        /// Boolean Result
        /// </returns>
        public async Task<bool> AddProduct(IFormFile file)
        {
            var list = new List<ProductInformation>();
            var products = new List<ProductInformation>();
            //// read the Excel file
            using (var stream = new MemoryStream())
            {
               
               await file.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    //// create worksheet
                    OfficeOpenXml.ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    //// iterate record without header
                    for (int row = 2; row <= rowCount; row++)
                    {
                        list.Add(new ProductInformation
                        {
                            ProductId = int.Parse(worksheet.Cells[row,1].Value.ToString().Trim()),
                            ProductName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            Price = int.Parse(worksheet.Cells[row, 3].Value.ToString().Trim()),
                            Quantity = int.Parse(worksheet.Cells[row,4].Value.ToString().Trim())
                          
                        });
                       
                    }
                   foreach(var product in list)
                    {
                        if(product.Quantity >= 1)
                        {
                            product.TotalAmount = product.Quantity * product.Price;
                        }
                        products.Add(product);
                    }

                }
            }
           return await this.repository.AddProduct(products);
           
        }

        /// <summary>
        /// Updates the product.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>
        /// boolean result
        /// </returns>
        public async Task<List<bool>> UpdateProduct(IFormFile file)
        {
           // bool updateproductResult;
            var list = new List<ProductInformation>();
            var products = new List<ProductInformation>();
            var addProducts = new List<ProductInformation>();
            var updateProducts = new List<ProductInformation>();
            var response = new List<bool>();
            using (var stream = new MemoryStream())
            {
               await file.CopyToAsync(stream);
                
                using (var package = new ExcelPackage(stream))
                {
                    //// Create Excelsheet
                    OfficeOpenXml.ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    //// iterate record without header
                    for (int row = 2; row <= rowCount; row++)
                    {
                        list.Add(new ProductInformation
                        {
                            ProductId = int.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                            ProductName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            Price = int.Parse(worksheet.Cells[row, 3].Value.ToString().Trim()),
                            Quantity = int.Parse(worksheet.Cells[row, 4].Value.ToString().Trim())

                        });

                    }
                    foreach (var product in list)
                    {
                        if (product.Quantity >= 1)
                        {
                            product.TotalAmount = product.Quantity * product.Price;
                        }
                        products.Add(product);
                    }
                }
               // list.Clear();
               // list = this.repository.GetProductDetails();

              //  var abc = list.Except(products);
                foreach (var product in products)
                {
                   var result = CheckProduct(product.ProductId);
                    if(result == true)
                    {
                        updateProducts.Add(product);
                    }
                    else
                    {
                        addProducts.Add(product);
                    }
                    
                }
                  response.Add(await this.repository.AddProduct(addProducts));
                response.Add(await this.repository.UpdateProduct(updateProducts));
            }
            return response;
        }

        /// <summary>
        /// Checks the product.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns>boolean result</returns>
        public bool CheckProduct(int productId)
        {
            var products = new List<int>();
            bool check = false;
            products.Add(productId);
           var list = this.repository.GetProductDetails();
            foreach (var checkproduct in list)
            {
               
                if (checkproduct.ProductId == productId)
                {
                    check = true;
                }
            }

            return check;
        }

        /// <summary>
        /// DeleteProduct the product.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>
        /// boolean result
        /// </returns>
        public async Task<bool> DeleteProduct(IFormFile file)
        {
            bool deleteProduct;
            var list = new List<ProductInformation>();
            using (var stream = new MemoryStream())
            {
               await file.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    OfficeOpenXml.ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    //// Iterate Records in Excel
                    for (int row = 2; row <= rowCount; row++)
                    {
                        list.Add(new ProductInformation
                        {
                            ProductId = int.Parse(worksheet.Cells[row, 1].Value.ToString().Trim()),
                            ProductName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            Price = int.Parse(worksheet.Cells[row, 3].Value.ToString().Trim()),
                            Quantity = int.Parse(worksheet.Cells[row, 4].Value.ToString().Trim())

                        });
                    }
                }
                deleteProduct = await this.repository.DeleteProduct(list);
            }
            return deleteProduct;
        }
    }
}