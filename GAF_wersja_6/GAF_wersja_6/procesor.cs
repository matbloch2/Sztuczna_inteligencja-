using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nowy__Algorytm_wyrzażania_dla_n_procesów
{
    internal class procesor
    {
       public  int id;
       public  List<int> pocesy;

        public procesor(int id, List<int> pocesy)
        {
            this.id = id;
            this.pocesy = pocesy;
        }
    }
}
