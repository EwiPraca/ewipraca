namespace EwiPraca.Model.EmployeeArea
{
    public class Contract
    {
        public int Id { get; set; }
        public virtual Employee Employee { get; set; }
        public int EmployeeId { get; set; }
    }
}