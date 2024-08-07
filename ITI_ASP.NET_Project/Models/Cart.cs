namespace ITI_ASP.NET_Project.Models {
	public class Cart {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public Product product { get; set; }
       
    }
}
