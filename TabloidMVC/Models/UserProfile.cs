﻿using Microsoft.Data.SqlClient.Server;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;

namespace TabloidMVC.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        [DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }

        [DisplayName("Display Name")]
        [Required]
        public string DisplayName { get; set; }

        [Required]
        public string Email { get; set; }
        public DateTime CreateDateTime { get; set; }

        [DisplayName("Profile Image")]
        public string ImageLocation { get; set; }


        public int UserTypeId { get; set; }
        public UserType UserType { get; set; }
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}