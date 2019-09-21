namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// AppSetting Class
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets from address.
        /// </summary>
        /// <value>
        /// From address.
        /// </value>
        public string FromAddress { get; set; }

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
        public int SMTPPort { get; set; }

        /// <summary>
        /// Gets or sets the enable SSL.
        /// </summary>
        /// <value>
        /// The enable SSL.
        /// </value>
        public string EnableSSL { get; set; }

        /// <summary>
        /// Gets or sets the identity connections.
        /// </summary>
        /// <value>
        /// The identity connections.
        /// </value>
        public string IdentityConnections { get; set; }

        /// <summary>
        /// Gets or sets the JWT secret.
        /// </summary>
        /// <value>
        /// The JWT secret.
        /// </value>
        public string JWT_Secret { get; set; }
    }
}