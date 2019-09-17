using BusinessLogic.Interfaces;
using Experimental.System.Messaging;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using System.Net;
using System.Net.Mail;
using System.Text;


namespace BusinessLogic
{
    public class EmployeeDatabase: IGetCustomer
    {
        string conn = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=UserDatabase;Integrated Security=True";
        public IEnumerable<Cutomer> GetCustomers()
        {
            List<Cutomer> cutomers = new List<Cutomer>();
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand sqlCommand = new SqlCommand("getData", connection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();
                   
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Cutomer cutomer = new Cutomer();

                        cutomer.Name = sqlDataReader["Name"].ToString();
                         cutomer.Mobile = sqlDataReader["Mobile"].ToString();
                       cutomer.Email = sqlDataReader["Email"].ToString();


                        cutomers.Add(cutomer);
                    }

                    connection.Close();
                }

                return cutomers;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
    }
        public IEnumerable<Merchant> GetAllMerchant()
        {
            var merchants = new List<Merchant>();
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand sqlCommand = new SqlCommand("getMerchantData", connection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    connection.Open();

                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        Merchant merchant = new Merchant();

                        merchant.Name = sqlDataReader["Name"].ToString();
                        merchant.Mobile = sqlDataReader["Mobile"].ToString();
                        merchant.Email = sqlDataReader["Email"].ToString();
                        merchant.City = sqlDataReader["City"].ToString();
                        merchant.Product = sqlDataReader["Product"].ToString();

                        merchants.Add(merchant);
                    }

                    connection.Close();
                }

                return merchants;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int AddCustomer(Cutomer cutomer)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand sqlCommand = new SqlCommand("AddData", connection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@Name", cutomer.Name);
                    sqlCommand.Parameters.AddWithValue("@Mobile", cutomer.Mobile);
                    sqlCommand.Parameters.AddWithValue("@Email", cutomer.Email);

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                }

                return 1;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public int AddMerchantData(Merchant merchant)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand sqlCommand = new SqlCommand("AddMerchantData", connection);
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
               var result= SendEmail();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public int SendEmail()
        {
            try
            {
                string msmqQueuePath = @".\Private$\Mail";
                // Create new instance of Message object
                Message msmqMsg = null;
              
                MessageQueue msmqQueue = new MessageQueue();

                if (!MessageQueue.Exists(msmqQueuePath))
                {
                    msmqQueue = MessageQueue.Create(msmqQueuePath);
                }
                else
                {
                    msmqQueue = new MessageQueue(msmqQueuePath);
                }

                msmqQueue.Formatter = new BinaryMessageFormatter();

                msmqQueue.Send("kambleaniket17@gmail.com");

               var result = SendEmails();
                return result;
            }
           catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int SendEmails()
        {
            try
            { 
            string messageQueuePath = @".\Private$\Mail";

            MessageQueue _msmqQueue = new MessageQueue(messageQueuePath);

            _msmqQueue.Formatter = new BinaryMessageFormatter();
            _msmqQueue.MessageReadPropertyFilter.SetAll();

           _msmqQueue.ReceiveCompleted += new
                ReceiveCompletedEventHandler(MyReceiveCompleted);

            _msmqQueue.BeginReceive();

            return 1;
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        private static void MyReceiveCompleted(Object source,
             ReceiveCompletedEventArgs asyncResult)
        {
            // Connect to the queue.
            try
            {
                MessageQueue mq = (MessageQueue)source;


                Message m = mq.EndReceive(asyncResult.AsyncResult);


                string msg = (string)m.Body;

                mq.BeginReceive();
                EmployeeDatabase employeeDatabase = new EmployeeDatabase();


                employeeDatabase.SendingEmails(msg);
               
            }
         catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

  
        public int SendingEmails(string msg)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("privateuser1199@gmail.com");
                message.To.Add(new MailAddress("kambleaniket17@gmail.com"));
                message.Subject = "Notification From Server";
                message.IsBodyHtml = true;
                var list = new List<Merchant>();
                var list1 = new List<string>();

                EmployeeDatabase employeeDatabase = new EmployeeDatabase();

                list = employeeDatabase.GetAllMerchant().ToList();

                message.IsBodyHtml = true;
                string textBody = " <table border=" + 1 + " cellpadding=" + 0 + " cellspacing=" + 0 + " width = " + 400 + "><tr bgcolor='#4da6ff'><td><b>Name</b></td> <td> <b> Mobile</b> </td>" +
                    "<td> <b> Email</b> </td><td> <b> City</b> </td><td> <b> Product</b> </td></tr>";
                foreach (var result in list)
                {

                    textBody += "<tr><td>" + result.Name + "</td><td> " + result.Mobile + "</td> <td> " + result.Email + "</td><td> " + result.City + "</td><td> " + result.Product + "</td></tr>";

                }
                textBody += "</table>";
                message.Body = "Added New Merchant to list. Data of Merchants Are:<br />" + textBody + "<br />";
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("privateuser1199@gmail.com", "private252");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                return 1;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
              
            }
        }
    }
}