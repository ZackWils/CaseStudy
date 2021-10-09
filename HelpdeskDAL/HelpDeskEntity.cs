using System.ComponentModel.DataAnnotations;

namespace HelpdeskDAL
{

    public class HelpDeskEntity
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] Timer { get; set; }
    }
}