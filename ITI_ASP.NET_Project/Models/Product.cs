using System.ComponentModel.DataAnnotations;

namespace ITI_ASP.NET_Project.Models {
    public class Product {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Price { get; set; }
        public byte[] ImageData {
            get; set;
        }
        public string ContentType {
            get; set;
        }
    }
}
