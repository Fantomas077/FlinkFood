using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlinkFood.Data
{
    public class OrderHeader
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        [Display(Name = "Order Total ")]
        public decimal OrderTotal { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        [Display(Name ="Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name ="Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public List<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
    }
}
