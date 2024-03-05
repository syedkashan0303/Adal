using Adal.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CoreClass
{
    public class UsersDTO
    {
        public UsersDTO()
        {
            //List<City> cities = new List<City>();
            CityList = new List<SelectListItem>();
			LawyerTypeList = new List<SelectListItem>();
        }
        public int Id { get; set; }

        [EmailAddress]
        public string? Email { get; set; } = "";

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [MinimumAge(18)]
        public DateTime? DateOfBirth { get; set; }

        [Phone]
        public string? Contact { get; set; }

        [Required]
        [StringLength(13, ErrorMessage = "Name length can't be more than 13.")]
        public string? CNIC { get; set; }
        public int? UserType { get; set; }
        public string? UserTypeName { get; set; }
        public bool? Active { get; set; }
        public string? Address { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [PasswordStrength]
        public string? Password { get; set; }

        [Compare("Password")]
        [NotMapped]
        public string? ConfirmPassword { get; set; }


        public int? CityId { get; set; }
        public int? UserRoleId { get; set; }

        [NotMapped]
        public string? UserRoleName { get; set; }

        [NotMapped]
        public string? CityName { get; set; }
        public DateTime CreatedOnUTC { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedOnUTC { get; set; } = DateTime.UtcNow;

        public List<SelectListItem> CityList { get; set; }
        public List<SelectListItem> LawyerTypeList { get; set; }
	}
}
