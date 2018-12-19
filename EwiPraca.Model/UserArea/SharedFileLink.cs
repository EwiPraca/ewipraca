using System;

namespace EwiPraca.Model
{
    public class SharedFileLink
    {
        public int Id { get; set; }
        public string FileGuid { get; set; }
        public string GuidLink { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
