namespace UAS_PAA.Models
{
    public class Users
    {
        public int Id_Users { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Id_Role { get; set; } // Sesuai dengan foreign key ke tabel Role
        public Role? Role { get; set; } // Navigation property untuk join data
    }

    public class Role
    {
        public int Id_Role { get; set; }
        public string Nama_Role { get; set; } = string.Empty;
    }
}