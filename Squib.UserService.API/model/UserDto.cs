using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Squib.UserService.API.Model;
[Table("UserDto_New")]
public class UserDto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Email { get; set; }

    [Required]
    [StringLength(255)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(255)]
    public string LastName { get; set; }
}
