using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Online_Book_Store_Application.Models.Models
{
    public class BookAuthor
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [DisplayName("Author Serial")]
        public int AuthorSerialId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        [DisplayName("Phone")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
