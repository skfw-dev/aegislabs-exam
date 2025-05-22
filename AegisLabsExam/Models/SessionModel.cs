using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AegisLabsExam.Models;

[Table("sessions")]
public class SessionModel : BaseModel
{
    [Column("user_agent")]
    public string UserAgent { get; set; } = null!;
    
    [Column("ip_address")]
    public string IpAddress { get; set; } = null!;
    
    [Required]
    [Column("secret_key")]
    public string SecretKey { get; set; } = null!;
    
    [Column("logged_in")]
    public DateTimeOffset LoggedIn { get; set; }
    
    [Column("last_activity")]
    public DateTimeOffset LastActivity { get; set; }
    
    [Column("logged_out")]
    public DateTimeOffset? LoggedOut { get; set; }
    
    [Required]
    [Column("user_id")]
    public int UserId { get; set; }
    
    public UserModel User { get; set; } = null!;
}