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
      public int proces_wykonywany_przed;

        public proces(int id,int id_procesora, int time,int proces_wykonywany_przed)
        {

            this.id_pocesora = id_procesora;
            this.id = id;
            this.proces_wykonywany_przed = proces_wykonywany_przed;
            this.time = time;
        }
    }
}
