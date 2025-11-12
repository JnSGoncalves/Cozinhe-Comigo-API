using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cozinhe_Comigo_API.Models {
    [Table("tokens")]
    public class Token {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("userid")]
        public int UserId { get; set; }
        [Required]
        [Column("tokencode")]
        public string TokenCode { get; set; }
        [Column("devicespecs")]
        public string? DeviceSpecs { get; set; }
       
        private DateTime _lastLoginAt;
        [Required]
        [Column("lastloginat")]
        public DateTime LastLoginAt { 
            get => _lastLoginAt; 
            set{
                ExpiredAt = DateTime.UtcNow.AddMinutes(60);
                _lastLoginAt = value;
            } 
        }
        [Required]
        [Column("expiredat")]
        public DateTime ExpiredAt { get; set; }

        public Token(int userId, string tokenCode, string? deviceSpecs = null) {
            UserId = userId;
            TokenCode = tokenCode;
            DeviceSpecs = deviceSpecs;
            LastLoginAt = DateTime.UtcNow;
            ExpiredAt = DateTime.UtcNow.AddMinutes(60);
        }
    } 
}