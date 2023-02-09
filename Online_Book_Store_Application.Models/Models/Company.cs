using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Book_Store_Application.Models.Models;

public class Company
{
    [Key,DatabaseGenerated((DatabaseGeneratedOption.Identity))]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
}