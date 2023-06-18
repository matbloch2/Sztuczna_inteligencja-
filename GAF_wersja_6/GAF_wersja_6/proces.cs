using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nowy__Algorytm_wyrzażania_dla_n_procesów
{
    internal class proces
    {
      public int id_pocesora;
      public int id;
      public int time;
      public List<proces> proces_wykonywany_przed;

        public proces(int id, int time)
        {

            this.id_pocesora = 0;
            this.id = id; 
            this.proces_wykonywany_przed =new List<proces>();
            this.time = time;
        }

        public static implicit operator proces(List<proces> v)
        {
            throw new NotImplementedException();
        }
    }
}
