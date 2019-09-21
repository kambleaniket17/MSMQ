namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Class MyData Model
    /// </summary>
    public class MyData
    {
        /// <summary>
        /// Gets address.
        /// </summary>
        /// <value>
        /// To address.
        /// </value>
        public string ToAddress { get; set; }

        /// <summary>
        /// Gets or sets the SMTP client.
        /// </summary>
        /// <value>
        /// The SMTP client.
        /// </value>
        public string SmtpClient { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the SMTP port.
        /// </summary>
        /// <value>
        /// The SMTP port.
        /// </value>
        public string SMTPPort { get; set; }

        /// <summary>
        /// Gets or sets the enable SSL.
        /// </summary>
        /// <value>
        /// The enable SSL.
        /// </value>
        public string EnableSSL { get; set; }
    }
}
