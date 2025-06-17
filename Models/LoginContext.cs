using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UAS_PAA.Helpers;

namespace UAS_PAA.Models
{
    public class LoginContext
    {
        private string __constr; // Menyimpan connection string database
        private string __ErrorMsg; // Menyimpan pesan error saat autentikasi

        public LoginContext(string pConstr) // Konstruktor menerima connection string
        {
            __constr = pConstr;
        }

        public List<Login> Autentifikasi(string p_username, string p_password, IConfiguration p_config)
        {
            List<Login> list1 = new List<Login>(); // Menyimpan hasil autentikasi user (dengan token)

            // Query untuk memverifikasi user berdasarkan username & password
            // Join ke tabel peran_users dan peran untuk mendapatkan role
            string query = @"
                            SELECT u.id_users, u.username, u.email, r.id_role, r.nama_role
                            FROM users u
                            JOIN role r ON u.id_role = r.id_role
                            WHERE u.username = @username AND u.password = crypt(@password, u.password)";

            // Logging untuk debugging
            Console.WriteLine($"Query: {query}");
            Console.WriteLine($"Username: {p_username}");
            Console.WriteLine($"Password: {p_password}");

            SqlDbHelpers db = new SqlDbHelpers(this.__constr); // Buat koneksi ke database

            try
            {
                NpgsqlCommand cmd = db.getNpgsqlCommand(query); // Buat command dari query SQL
                cmd.Parameters.AddWithValue("@username", p_username); // Isi parameter username
                cmd.Parameters.AddWithValue("@password", p_password); // Isi parameter password

                using (NpgsqlDataReader reader = cmd.ExecuteReader()) // Jalankan query
                {
                    while (reader.Read())
                    {
                        var user = new Login()
                        {
                            Id_Users = Convert.ToInt32(reader["id_users"]),
                            Username = reader["username"].ToString(),
                            Email = reader["email"].ToString(),
                            Id_Role = Convert.ToInt32(reader["id_role"]),
                            Nama_Role = reader["nama_role"].ToString(),
                            Token = GenerateJwtToken(
                                Convert.ToInt32(reader["id_users"]), // Tambahkan parameter ID
                                reader["username"].ToString(),
                                reader["nama_role"].ToString(),
                                p_config
                            )
                        };

                        Console.WriteLine($"User found: {user.Username}");
                        Console.WriteLine($"Generated Token: {user.Token}");

                        list1.Add(user);
                    }

                }

                cmd.Dispose(); // Bersihkan command
                db.closeConnection(); // Tutup koneksi
            }
            catch (Exception ex)
            {
                __ErrorMsg = ex.Message; // Tangkap error jika gagal
                Console.WriteLine("Error: " + ex.Message); // Log error
            }

            if (list1.Count == 0) // Jika tidak ditemukan user
            {
                Console.WriteLine("No user found with given credentials.");
            }

            return list1; // Kembalikan hasil autentikasi
        }

        private string GenerateJwtToken(int userId, string username, string role, IConfiguration pConfig)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(pConfig["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Id_Users", userId.ToString()), // Claim khusus untuk Flutter
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()), // Standard JWT claim
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()), // ASP.NET default
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                pConfig["Jwt:Issuer"],
                pConfig["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
