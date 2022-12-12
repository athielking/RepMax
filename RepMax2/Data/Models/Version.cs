using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepMax2.Data.Models
{
    public class Version
    {
        [PrimaryKey]
        public int VersionNumber { get; set; }
    }
}
