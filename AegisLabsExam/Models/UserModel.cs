using System.ComponentModel.DataAnnotations.Schema;
using AegisLabsExam.Common;

namespace AegisLabsExam.Models;

[Table("users")]
public class UserModel : BaseModel
{
    [Column(name: "first_name")]
    public string FirstName { get; set; } = null!;
    
    [Column(name: "last_name")]
    public string LastName { get; set; } = null!;
 
    [Column(name: "username")]
    public string Username { get; set; } = null!;
    
    [Column(name: "email")]
    public string Email { get; set; } = null!;
    
    [Column(name: "password")]
    public Pbkdf2Hash Password { get; set; } = null!;
    
    [Column(name: "role")]
    public string Role { get; set; } = null!;
}