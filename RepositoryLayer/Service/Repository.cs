namespace RepositoryLayer
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IdentityModel.Tokens.Jwt;
    using System.Net;
    using System.Net.Mail;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Experimental.System.Messaging;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using RepositoryLayer.Interface;
    using ServiceStack.Redis;

    /// <summary>
    /// Repository class
    /// </summary>
    /// <seealso cref="RepositoryLayer.Interface.IRepository" />
    public class Repository : IRepository
    {
        /// <summary>
        /// Gets or sets the application settings.
        /// </summary>
        /// <value>
        /// The application settings.
        /// </value>
        private AppSettings AppSettings { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public Repository(IOptions<AppSettings> settings)
        {
            AppSettings = settings.Value;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Repository()
        {
        }

        // string connectionstring=
        /// <summary>
        /// Gets the customers.
        /// </summary>
        /// <returns>IEnumerable Data</returns>
        /// <exception cref="Exception">Check the Exceptions</exception>
        public IEnumerable<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();
            try
            {
                using (SqlConnection connection = new SqlConnection(AppSettings.IdentityConnections))
                {
                    SqlCommand sqlCommand = new SqlCommand("getData", connection); //// Adding Store procedure Name
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    ////Reading Records
                    while (sqlDataReader.Read())
                    {
                        Customer customer = new Customer();

                        customer.Name = sqlDataReader["Name"].ToString();
                        customer.Mobile = sqlDataReader["Mobile"].ToString();
                        customer.Email = sqlDataReader["Email"].ToString();

                        //// Add The Data into list
                        customers.Add(customer);
                    }

                    connection.Close();
                }

                return customers;
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
            var merchants = new List<Merchant>();
            try
            {
                using (SqlConnection connection = new SqlConnection(AppSettings.IdentityConnections))
                {
                    SqlCommand sqlCommand = new SqlCommand("getMerchantData", connection); //// Adding Store procedure Name
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                    //// Reading Records
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Merchant merchant = new Merchant();
                        //// Read The records
                        merchant.Name = sqlDataReader["Name"].ToString();
                        merchant.Mobile = sqlDataReader["Mobile"].ToString();
                        merchant.Email = sqlDataReader["Email"].ToString();
                        merchant.City = sqlDataReader["City"].ToString();
                        merchant.Product = sqlDataReader["Product"].ToString();
                        //// Adding Data Into list
                        merchants.Add(merchant);
                    }

                    connection.Close();
                }

                return merchants;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Adds the customer.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>Boolean Result</returns>
        /// <exception cref="Exception">Check The Exceptions</exception>
        public bool AddCustomer(Customer customer)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(AppSettings.IdentityConnections))
                {
                    SqlCommand sqlCommand = new SqlCommand("AddData", connection); //// Adding Store procedure Name
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    //// Adding Parameters
                    sqlCommand.Parameters.AddWithValue("@Name", customer.Name);
                    sqlCommand.Parameters.AddWithValue("@Mobile", customer.Mobile);
                    sqlCommand.Parameters.AddWithValue("@Email", customer.Email);

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                }

                return true;
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
                using (SqlConnection connection = new SqlConnection(AppSettings.IdentityConnections))
                {
                    SqlCommand sqlCommand = new SqlCommand("AddMerchantData", connection); //// Adding Store procedure Name
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Name", merchant.Name);
                    sqlCommand.Parameters.AddWithValue("@Mobile", merchant.Mobile);
                    sqlCommand.Parameters.AddWithValue("@Email", merchant.Email);
                    sqlCommand.Parameters.AddWithValue("@City", merchant.City);
                    sqlCommand.Parameters.AddWithValue("@Product", merchant.Product);
                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                }
                //// Calling SendEmail Method
                this.SendEmail(merchant);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="merchant">The merchant.</param>
        /// <returns>Boolean Result</returns>
        /// <exception cref="Exception">Check The Exceptions</exception>
        public void SendEmail(Merchant merchant)
        {
            try
            {
                string msmqQueuePath = @".\Private$\Mail"; //// Messaging Queue Path

                MessageQueue msmqQueue = new MessageQueue();

                //// Checking Message Queue Path Exist or Not
                if (!MessageQueue.Exists(msmqQueuePath))
                {
                    msmqQueue = MessageQueue.Create(msmqQueuePath);
                }
                else
                {
                    //// Adding Message Queue Path if not exist 
                    msmqQueue = new MessageQueue(msmqQueuePath);
                }

                msmqQueue.Formatter = new BinaryMessageFormatter();
                var jsondata = JsonConvert.SerializeObject(merchant);
                //// Send Data into MessageQueue
                msmqQueue.Send(jsondata);

                //// Calling SendEmails Method
                this.SendEmails();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Sends the emails.
        /// </summary>
        /// <returns>Boolean result</returns>
        /// <exception cref="Exception">Check the Exceptions</exception>
        public void SendEmails()
        {
            try
            {
                string messageQueuePath = @".\Private$\Mail"; //// Messaging Queue Path
                MessageQueue msmqQueue = new MessageQueue(messageQueuePath);
                msmqQueue.Formatter = new BinaryMessageFormatter();
                msmqQueue.MessageReadPropertyFilter.SetAll();
                msmqQueue.ReceiveCompleted += new
                ReceiveCompletedEventHandler(MyReceiveCompleted);
                msmqQueue.BeginReceive();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

        }

        /// <summary>
        /// Meyhod for Send Message
        /// </summary>
        /// <param name="messages">The messages.</param>
        /// <param name="AppSettings">The AppSettings.</param>
        public void SendingMessage(string messages, AppSettings AppSettings)
        {
            try
            {
                //// Create MailMessage And smtp client 
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(AppSettings.UserId);
                message.To.Add(new MailAddress(AppSettings.ToAddress));
                message.Subject = "Notification From Server";
                message.IsBodyHtml = true;

                var data = JObject.Parse(messages);

                message.IsBodyHtml = true;

                //// Add Table with Data into Body
                string messageBody = " <table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#4da6ff'><td><b>Name</b></td> <td> <b> Mobile</b> </td>" +
                    "<td> <b> Email</b> </td><td> <b> City</b> </td><td> <b> Product</b> </td></tr>";

                messageBody += "<tr><td>" + data["Name"].ToString() + "</td><td> " + data["Mobile"].ToString() + "</td> <td> " + data["Email"].ToString() + "</td><td> " + data["City"].ToString() + "</td><td> " + data["Product"].ToString() + "</td></tr>";

                messageBody += "</table>";
                message.Body = "Added New Merchant to list. Data of Merchants Are:<br />" + messageBody + "<br />";
                smtp.Port = AppSettings.SMTPPort;
                smtp.Host = AppSettings.SmtpClient;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(AppSettings.UserId, AppSettings.Password);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);

            }
        }

        /// <summary>
        /// Delegates Method for Receive MSMQ message
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="asyncResult">The asyncResult.</param>
        private void MyReceiveCompleted(Object source,
             ReceiveCompletedEventArgs asyncResult)
        {
            //// Connect to the queue.
            try
            {
                //// retrive the data From MessageQueue
                MessageQueue messageQueue = (MessageQueue)source;

                Message retriveMessage = messageQueue.EndReceive(asyncResult.AsyncResult);

                //// Storing Body of MessageQueue into Variable
                string message = retriveMessage.Body.ToString();

                messageQueue.BeginReceive();
                var appSettings = AppSettings;

                Repository repository = new Repository();
                repository.SendingMessage(message, appSettings);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Method for Set And Get Redis Catche
        /// </summary>
        /// <returns>String Time</returns>
        public string GetRedis(string userName)
        {
            //// create Redis client
            using (var redis = new RedisClient())
            {
                //// Check Null or Not
                if (redis.Get("Time of User" + userName) == null)
                {
                    //// Set Redis
                    redis.Set("Time of User" + userName, "Last login Time of " + userName + "=" + "" + DateTime.Now.ToString());
                }
                //// Get Redis And Return Data
                var cache = redis.Get("Time of User" + userName);
                var stringFromByteArray = System.Text.Encoding.UTF8.GetString(cache);
                redis.Remove("Time of User" + userName);
                redis.Set("Time of User" + userName, "Last login Time of " + userName + "=" + "" + DateTime.Now.ToString());
                return stringFromByteArray;
            }
        }

        /// <summary>
        /// User Login Method
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns>string token And Time</returns>
        public List<string> Login(Login login)
        {
            var Result = string.Empty;
            string jsonString = string.Empty;
            var list = new List<string>();
            //// Call Check Method For Checking user present or not
            List<Register> user = Check(login);
            if (user.Count != 0)
            {
                //// Calling Redis Method
                Result = GetRedis(login.UserName);
                //// Creating Token
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[] { new Claim("UserId", login.UserName) }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.AppSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(token);
                list.Add(Result);
                list.Add(jsonString);
                return list;
            }
            else
            {
                return list;

            }

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
                if (register != null)
                {
                    //// Sql Add Using Store procedure
                    using (SqlConnection connection = new SqlConnection(AppSettings.IdentityConnections))
                    {
                        SqlCommand sqlCommand = new SqlCommand("RegisterUser", connection); //// Adding Store procedure Name
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@Name", register.Name);
                        sqlCommand.Parameters.AddWithValue("@Mobile", register.Mobile);
                        sqlCommand.Parameters.AddWithValue("@Email", register.Email);
                        sqlCommand.Parameters.AddWithValue("@Password", register.Password);
                        connection.Open();
                        sqlCommand.ExecuteNonQuery();
                        connection.Close();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);

            }

        }

        /// <summary>
        /// Checking Valide User
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns>Data of User</returns>
        public List<Register> Check(Login login)
        {
            try
            {
                var user = new List<Register>();
                if (login.UserName != null && login.Password != null)
                {


                    using (SqlConnection connection = new SqlConnection(AppSettings.IdentityConnections))
                    {
                        SqlCommand sqlCommand = new SqlCommand("CheckAuthorize", connection); //// Adding Store procedure Name
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        sqlCommand.Parameters.AddWithValue("@UserName", login.UserName);
                        sqlCommand.Parameters.AddWithValue("@Password", login.Password);
                        connection.Open();
                        //// Reading Records
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                        while (sqlDataReader.Read())
                        {
                            Register register = new Register();
                            //// Read The records
                            register.Name = sqlDataReader["Name"].ToString();
                            register.Mobile = sqlDataReader["Mobile"].ToString();
                            register.Email = sqlDataReader["Email"].ToString();
                            register.Password = sqlDataReader["Password"].ToString();
                            //// Adding Data Into list
                            user.Add(register);
                        }

                        connection.Close();
                    }

                }
                return user;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Adding Product
        /// </summary>
        /// <param name="merchant">productInformation</param>
        public async Task<bool> AddProduct(List<ProductInformation> products)
            {

            try
            {
                if (products != null)
                {
                    using (SqlConnection connection = new SqlConnection(AppSettings.IdentityConnections))
                    {
                        foreach (var product in products)
                        {
                            SqlCommand sqlCommand = new SqlCommand("AddProduct", connection); //// Adding Store procedure
                            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                            sqlCommand.Parameters.AddWithValue("@ProductId", product.ProductId);
                            sqlCommand.Parameters.AddWithValue("@ProductName", product.ProductName);
                            sqlCommand.Parameters.AddWithValue("@Price", product.Price);
                            sqlCommand.Parameters.AddWithValue("@Quantity", product.Quantity);
                            sqlCommand.Parameters.AddWithValue("@Total_Amount", product.TotalAmount);
                           
                            connection.Open();
                            await sqlCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                            connection.Close();
                        }
                    }
                    return true;
                }
                return false;

            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<bool> UpdateProduct(List<ProductInformation> products)
        {
            try
            {
                if (products != null)
                {
                    using (SqlConnection connection = new SqlConnection(AppSettings.IdentityConnections))
                    {
                        foreach (var product in products)
                        {
                            SqlCommand sqlCommand = new SqlCommand("UpdateProduct", connection); //// Adding Store procedure
                            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                            sqlCommand.Parameters.AddWithValue("@ProductId", product.ProductId);
                            sqlCommand.Parameters.AddWithValue("@ProductName", product.ProductName);
                            sqlCommand.Parameters.AddWithValue("@Price", product.Price);
                            sqlCommand.Parameters.AddWithValue("@Quantity", product.Quantity);
                            sqlCommand.Parameters.AddWithValue("@Total_Amount", product.TotalAmount);
                            connection.Open();
                            await sqlCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                            connection.Close();
                        }
                    }
                    return true;
                }
                return false;
                
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public List<ProductInformation> GetProductDetails()
        {
            try
            {
                var products = new List<ProductInformation>();
                    using (SqlConnection connection = new SqlConnection(AppSettings.IdentityConnections))
                    {
                        SqlCommand sqlCommand = new SqlCommand("GetProductDetails", connection); //// Adding Store procedure Name
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        connection.Open();
                        //// Reading Records
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                        while (sqlDataReader.Read())
                        {
                            ProductInformation product = new ProductInformation();
                            //// Read The records
                            product.ProductId = Convert.ToInt32(sqlDataReader["ProductId"].ToString());
                            product.ProductName = sqlDataReader["ProductName"].ToString();
                            product.Price =Convert.ToDouble(sqlDataReader["Price"].ToString());
                            product.Quantity = Convert.ToInt32(sqlDataReader["Quantity"].ToString());
                            product.TotalAmount = Convert.ToDouble(sqlDataReader["Total_Amount"].ToString());
                        //// Adding Data Into list
                        products.Add(product);
                        }

                        connection.Close();
                    }
                return products;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<bool> DeleteProduct(List<ProductInformation> products)
        {
            try
            {
                if (products != null)
                {
                    using (SqlConnection connection = new SqlConnection(AppSettings.IdentityConnections))
                    {
                        foreach (var product in products)
                        {
                            SqlCommand sqlCommand = new SqlCommand("DeleteProduct", connection); //// Adding Store procedure
                            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                            sqlCommand.Parameters.AddWithValue("@ProductId", product.ProductId);
                            connection.Open();
                            await sqlCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
                           // sqlCommand.ExecuteNonQuery();
                            connection.Close();
                        }
}
                   
                    return true;
                }
                return false;
                
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
    }

        
    }
}