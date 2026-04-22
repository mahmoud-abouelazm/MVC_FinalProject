namespace Library.Web.Core.ViewModel.Rentals
{
    public class ReturnConfirmVM
    {
        public int RentalId { get; set; }
        public string MemberName { get; set; } = null!;
        public string MemberEmail { get; set; } = null!;
        public string CopyNames { get; set; } = "";
        public string BookTitles { get; set; } = "";
        public DateTime RentedAt { get; set; }
        public DateTime DueAt { get; set; }
        public decimal BookPrice { get; set; }  
        public int DaysRented { get; set; }
        public bool IsLate { get; set; }
        public int DaysLate { get; set; }
        public decimal BaseAmount { get; set; }   
        public decimal PenaltyAmount { get; set; } 
        public decimal TotalAmount { get; set; }
    }
}
