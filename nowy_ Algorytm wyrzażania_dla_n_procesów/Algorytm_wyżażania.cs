using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace nowy__Algorytm_wyrzażania_dla_n_procesów
{
    internal class Algorytm_wyżażania
    {
        private Random random;

       public  Algorytm_wyżażania()
        {
            this.random = new Random();
        }

        public void Szukanie_Oprymalnego_przypisania_N_procesów(List<procesor> procesory,List<proces> procesy,double temperatura,double schładznie)
        {
            int actualny_koszt=0;
            int nowy_koszt=0;
            int minum_koszt;

            

            inicjowanie(procesy,procesory);
            actualny_koszt = fukcja_kosztu(procesory, procesy);
            List<procesor> rozwiąnzania = kopia_procesory(procesory);
            minum_koszt = actualny_koszt;
            while (temperatura > 1)
            {

                List<procesor> stare_rozwiąnzanie = kopia_procesory(procesory);

                int Index_procesu = this.random.Next(procesy.Count);
                procesory[procesy[Index_procesu].id_pocesora].pocesy.Remove(procesy[Index_procesu].id);
                int randomProcedureId = this.random.Next(procesory.Count);
                procesy[Index_procesu].id_pocesora = procesory[randomProcedureId].id;
                int random_index=this.random.Next(procesory[randomProcedureId].pocesy.Count);
                procesory[randomProcedureId].pocesy.Insert(random_index,procesy[Index_procesu].id);
                nowy_koszt = fukcja_kosztu(procesory,procesy);
                
                if (funkcja_Akceptująca(actualny_koszt, nowy_koszt, temperatura) > this.random.NextDouble())
                {
                    actualny_koszt = nowy_koszt;
                    if (actualny_koszt < minum_koszt)
                        rozwiąnzania = kopia_procesory(procesory);
                }
                else
                {
                    procesory = stare_rozwiąnzanie;
                    przepisanie_satarej_tablicy_procesów(procesory, procesy);
                }
                ///zakładam że ukłąd ma rozwiąnzanie 
                if (actualny_koszt < int.MaxValue | temperatura>5)
                temperatura *= schładznie;
            }
            Wypisz(rozwiąnzania, fukcja_kosztu(rozwiąnzania, procesy));
        }




        private int fukcja_kosztu(List<procesor> procesory , List<proces> procesy)
        {
            int max = 0;
            foreach (var procesor in procesory)
            {
                int czas = obliczanie_kosztu_jednego_procesora(new List<int>(),procesory,procesor.pocesy,procesy);
                if (czas > max)
                    max= czas;
            }

            return max;
        }

        private int obliczanie_kosztu_jednego_procesora(List<int> odwiedzone,List<procesor> procesory, List<int> procesy_procesora, List<proces> procesy)
        {
            int executionTime = 0;
            int tmptime = 0;
            foreach (var proces_id in procesy_procesora)
            {
               

                if (procesy[proces_id].proces_wykonywany_przed !=-1)
                {
                    if (procesy[procesy[proces_id].proces_wykonywany_przed].id_pocesora == procesy[proces_id].id_pocesora)
                    {
                        int index_procesu_wykonywanego_przed = procesory[procesy[procesy[proces_id].proces_wykonywany_przed].id_pocesora].pocesy.IndexOf(procesy[proces_id].proces_wykonywany_przed);
                        int index_procesu_obecnego = procesory[procesy[proces_id].id_pocesora].pocesy.IndexOf(proces_id); 
                        if (index_procesu_obecnego < index_procesu_wykonywanego_przed)
                            return int.MaxValue;
                    }
                    else
                    {
                        int index_procesora = procesy[procesy[proces_id].proces_wykonywany_przed].id_pocesora;
                        int index_procesu_na_procesorze = procesory[index_procesora].pocesy.IndexOf(procesy[proces_id].proces_wykonywany_przed);
                        if (odwiedzone.Contains(proces_id))
                        {
                            return int.MaxValue;
                        }
                        odwiedzone.Add(proces_id);
                        tmptime = obliczanie_kosztu_jednego_procesora(odwiedzone,procesory, procesory[index_procesora].pocesy.Take<int>(index_procesu_na_procesorze).ToList(),procesy);
                        
                        
                        if (tmptime >= executionTime)
                            return int.MaxValue;
                    }
                }
                executionTime += procesy[proces_id].time;
            }
            return executionTime;
        }
        private void inicjowanie(List<proces> procesy, List<procesor> procesory)
        {
            foreach(var procesor in procesory)
            {
                procesor.pocesy.Clear();
            }
            
            foreach (var proces in procesy)
            {
                int Id_procesora = new Random().Next(procesory.Count);
                proces.id_pocesora = procesory[Id_procesora].id;
                
                procesory[Id_procesora].pocesy.Add(proces.id);
            }
        }

        private List<procesor> kopia_procesory(List<procesor> procesory)
        {

            List <procesor> kopia=new List<procesor>();
            foreach (procesor procesor in procesory)
            {
                
                procesor tmp_procesor = new procesor(procesor.id,new List<int>(procesor.pocesy));
                kopia.Add(tmp_procesor);
            }
            return kopia;
        }

        private void przepisanie_satarej_tablicy_procesów(List<procesor> procesory,List<proces> procesy )
        {
           foreach (procesor procesor in procesory)
           {
                foreach(int proces in procesor.pocesy)
                {
                    procesy[proces].id_pocesora = procesor.id;
                }
           }
        }

        private double funkcja_Akceptująca(double initialCost, double newCost, double temperature)
        {
            if (newCost < initialCost)
            {
                return 1.0;
            }
            else
            {
                return Math.Exp((initialCost - newCost) / temperature);
            }
        }

       public void Wypisz(List<procesor> procesory, int koszt)
       {
            foreach (procesor procesor in procesory)
            {
                Console.WriteLine($"numer procesora:{procesor.id}\nnumery przypisanych procesów:");
                foreach(int i in procesor.pocesy)
                {
                    Console.WriteLine(i);
                }    
            }

            Console.WriteLine($"koszt :{koszt}");
       }


    }
}
