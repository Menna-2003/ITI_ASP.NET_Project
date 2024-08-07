using System.ComponentModel.DataAnnotations;

namespace ITI_ASP.NET_Project.Models {
    public class Person {
        [Key]
        public int Id {
            get; set;
        }
        [Required]
		[RegularExpression( "^[a-zA-Z0-9_]{3,20}$", ErrorMessage = "Username must be 3-20 characters long and can only contain letters, numbers, and underscores." )]
		public string UserName {
            get; set;
        }
        [Required]
		[RegularExpression( "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character." )]
		public string Password {
            get; set;
        }
        [Required( ErrorMessage = "Type is required" )]
        public string Type {
            get; set;
        }
    }
}
