using System.ComponentModel.DataAnnotations;

namespace IptProjectWeb.Models
{
    public class Item
    {
        public Item() { }
        public Item(string id, string name, int quantity, int pricePerItem)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            PricePerItem = pricePerItem;
        }

        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int PricePerItem { get; set; }
        public int Quantity { get; set; }
    }
}
