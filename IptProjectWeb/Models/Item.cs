using System.ComponentModel.DataAnnotations;

namespace IptProjectWeb.Models
{
    public class Item
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int PricePerItem { get; set; }
        public int Quantity { get; set; }
    }
}
