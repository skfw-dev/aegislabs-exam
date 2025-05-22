using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AegisLabsExam.Models;

public class BaseModel
{
    [Key]
    [Column(name: "id")]
    public int Id { get; set; }
    
    [Required]
    [Column(name: "uuid")]
    public Guid Uuid { get; set; }
    
    [Column(name: "created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [Column(name: "updated_at")] 
    public DateTimeOffset UpdatedAt { get; set; }

    [Column(name: "deleted_at")] 
    public DateTimeOffset? DeletedAt { get; set; }
}