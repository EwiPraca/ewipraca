namespace EwiPraca.Importers.Importers
{
    public class EmployeeImportRow
    {
        public int RowNumber { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string City { get; set; }
        public string BirthDate { get; internal set; }
        public string PESEL { get; internal set; }
        public string StreetName { get; internal set; }
        public string StreetNumber { get; internal set; }
        public string PlaceNumber { get; internal set; }
        public string ZIPCode { get; internal set; }
    }
}