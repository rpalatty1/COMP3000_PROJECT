using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogWebApp.Models
{
    public class BaseEntity
    {
        [DatabaseGeneratedAttribute (DatabaseGeneratedOption.Identity), Key()]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }  = DateTime.Now;

        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set;}

    }
}
