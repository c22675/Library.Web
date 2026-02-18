using Petcu_Cosmina_Lab2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Petcu_Cosmina_Lab2.Models
{
    public class Genre
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
