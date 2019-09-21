namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    /// <summary>
    /// Class Merchant Model
    /// </summary>
    public class Merchant
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the mobile.
        /// </summary>
        /// <value>
        /// The mobile.
        /// </value>
        [Display(Name = "Mobile Number:")]
        [Required(ErrorMessage = "Mobile Number is required.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the product.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        [Required(ErrorMessage = "Product is required.")]
        public string Product { get; set; }
    }
}
