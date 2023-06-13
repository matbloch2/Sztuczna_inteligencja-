using nowy__Algorytm_wyrzażania_dla_n_procesów;
using System;
using System.Collections;

public class Program
{
    public static void Main()
    {
        int K_procesorów = 3;
        int N_procesów = 7;
        List<procesor> procesory=new List<procesor>();
        List<proces> procesy = new List<proces>();
        for (int i=0;i<K_procesorów;i++)
        {
            procesor procesor = new procesor(i,new List<int>());
            procesory.Add(procesor);
        }
        for (int i = 0; i < N_procesów; i++)
        {
            proces proces = new proces(i,-1,i+1);
            if(i>0)
            {
                proces.proces_wykonywany_przed = i - 1;
            }
            procesy.Add(proces);
        }

        ///zakłądam że układ ma rozwiąnzanie wy kukonuje go tak długo jak go nie znajdę 
        Algorytm_wyżażania algorytm_Wyżażania = new Algorytm_wyżażania();
        algorytm_Wyżażania.Szukanie_Oprymalnego_przypisania_N_procesów(procesory, procesy, 1000000, 0.999);
    }


    


}