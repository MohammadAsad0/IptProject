using System.ComponentModel.DataAnnotations;

namespace IptProjectWeb.Models
{
    public class BorrowedItem
    {
        [Key]
        public string Id { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public int QuantityBorrowed { get; set; }
        public string TimeBorrowed { get; set; }
        public string TimeToBeReturned { get; set; }
    }
}
