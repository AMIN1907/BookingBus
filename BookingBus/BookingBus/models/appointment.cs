namespace BookingBus.models
{
    public class appointment
    {
        public int id { get; set; }
        
        public string description { get; set; }

        public string TO { get; set; }
        public string FROM { get; set; }
        public long NumOfTicketIsAvalbel { get; set; }
        public DateTime DataToGo { get; set; }
        public DateTime DataToBack { get; set; }
        public DateTime DeadLineToBook { get; set; }
        public Boolean IsAvailable { get; set; } = true;

    }
}
