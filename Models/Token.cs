namespace Cozinhe_Comigo_API.Models {
    public class Token {
        public int Id { get; set; }
        public int FkUser { get; set; }
        public required string TokenCode { get; set; }
        public required string DeviceSpecs { get; set; }
        public DateTime LastLoginAt { get; set; }
        public DateTime ExpiredTime { get; set; }
    } 
}