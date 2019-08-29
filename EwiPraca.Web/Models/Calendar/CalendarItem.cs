using EwiPraca.Enumerations;

namespace EwiPraca.Models.Calendar
{
    public class CalendarItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Color { get; set; }
        public CalendarEventType Type { get; set; }
    }
}