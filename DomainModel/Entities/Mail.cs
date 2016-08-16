using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
namespace DomainModel.Entities
{

    public class Mail
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}
