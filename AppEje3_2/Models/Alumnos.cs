using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppEje3_2.Models
{
    public class Alumnos
    {
        [PrimaryKey, AutoIncrement]
        public int id { set; get; }
        [MaxLength(100)]
        public string nombres { set; get; }
        [MaxLength(100)]
        public string apellidos { set; get; }
        [MaxLength(100)]
        public string sexo { set; get; }
        [MaxLength(100)]
        public string direccion { set; get; }
        public string foto { get; set; }
    }
}
