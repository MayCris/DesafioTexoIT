using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DadosRetorno
    {
        public string producer { get; set; }
        public int interval { get; set; }
        public int previousWin { get; set; }
        public int followingWin { get; set; }
    }
}
