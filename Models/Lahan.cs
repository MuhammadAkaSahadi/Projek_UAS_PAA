using System.Text.Json.Serialization;
namespace UAS_PAA.Models
{
    public class Lahan
    {
        //[JsonPropertyName("nama_lahan")]
        public string Nama_Lahan { get; set; }

        //[JsonPropertyName("luas_lahan")]
        public double Luas_Lahan { get; set; }

        //[JsonPropertyName("satuan_luas")]
        public string Satuan_Luas { get; set; }

        //[JsonPropertyName("koordinat")]
        public string Koordinat { get; set; }

        //[JsonPropertyName("centroid_lat")]
        public double Centroid_Lat { get; set; }

        //[JsonPropertyName("centroid_lng")]
        public double Centroid_Lng { get; set; }

        //[JsonPropertyName("id_users")]
        public int Id_Users { get; set; }
    }

}
