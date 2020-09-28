using Microsoft.Data.SqlClient.Server;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;

namespace TabloidMVC.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        [DisplayName("Display Name")]
        public string DisplayName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email Address")]
        public string Email { get; set; }

        [DisplayName("Account created on")]
        public DateTime CreateDateTime { get; set; }

        [DisplayName("Picture")]
        public string ImageLocation { get; set; }

        public int UserTypeId { get; set; }

        [DisplayName("Account status")]
        public int IdIsActive { get; set; }

        [DisplayName("User Role")]
        public UserType UserType { get; set; }
        [DisplayName("Full Name")]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}