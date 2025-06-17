using Microsoft.AspNetCore.Mvc;

namespace UAS_PAA.Models
{
    public class LahanWithImage
    {
        //[FromForm(Name = "nama_lahan")]
        public string Nama_Lahan { get; set; }

        //[FromForm(Name = "luas_lahan")]
        public double Luas_Lahan { get; set; }

        //[FromForm(Name = "satuan_luas")]
        public string Satuan_Luas { get; set; }

        //[FromForm(Name = "koordinat")]
        public string Koordinat { get; set; }

        public double Centroid_Lat { get; set; }
        public double Centroid_Lng { get; set; }

        //[FromForm(Name = "id_users")]
        public int Id_Users { get; set; }

        //[FromForm(Name = "polygon_img")]
        public string Polygon_Img { get; set; }
    }

}
