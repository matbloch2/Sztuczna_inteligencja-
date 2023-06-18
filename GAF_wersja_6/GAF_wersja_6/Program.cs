using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using GAF;
using GAF.Operators;
using nowy__Algorytm_wyrzażania_dla_n_procesów;

namespace GAF_wersja_6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            int K_Procesorów = inicjowanie_procesorów();
            List<proces> procesy = InicjalizujProcesy();
            
            Population population = new Population();
            for (int i = 0; i < 20; i++)
            {
                var chromosome = new Chromosome();

                foreach (proces proces in inicjowanie_procesów(procesy, K_Procesorów, random))
                { 
                    chromosome.Genes.Add(new Gene(proces));
                }
                population.Solutions.Add(chromosome);
            }

            GeneticAlgorithm ga = new GeneticAlgorithm(population, Fitness);

            ga.OnGenerationComplete += ga_OnGenerationComplete;



            ga.Operators.Add(new Crossover(0.5, true) { CrossoverType = CrossoverType.DoublePointOrdered });
            ga.Operators.Add(new SwapMutate(0.5));

            ga.Run(TerminateAlgorithm);


        }

        private static List<proces> InicjalizujProcesy()
        {
          List<proces> procesy = new List<proces>();

            Console.WriteLine("podaj lidzbę procesów :");

            int n_proces=int.Parse(Console.ReadLine());
            for (int i = 0; i < n_proces; i++)
            {
               
                Console.WriteLine($"dane procesu:{i}:");
                Console.Write("id:");
                int id = int.Parse(Console.ReadLine());
                Console.Write("czas trawnia: ");
                int time = int.Parse(Console.ReadLine());

                proces proces = new proces(id,time);

                Console.Write("lidzba procesów wykonywanycb przed : ");
                int N_procesów_przed=int.Parse(Console.ReadLine());

                for (int j = 0; j < N_procesów_przed; j++)
                {
                    Console.Write($"proces wykonywany przed:{j}: ");
                    int id_ = int.Parse(Console.ReadLine());
                    proces.proces_wykonywany_przed.Add(procesy.Find(t => t.id == id_));    
                }

                procesy.Add(proces);
            }

            return procesy;
        }






        static void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {

            foreach (Gene procesor in e.Population.GetTop(1)[0].Genes)
            {


                Console.WriteLine($"{(((proces)procesor.ObjectValue)).id} procesor:{(((proces)procesor.ObjectValue)).id_pocesora}");

            }
            Console.WriteLine($"generacja: {e.Generation} pozią dopasowania: {e.Population.MaximumFitness}");
        }


        static double Fitness(Chromosome chromosome)
        {
            List<proces> procesory = new List<proces>();

            for (int i = 0; i < chromosome.Genes.Count; i++)
            {
                procesory.Add((proces)chromosome.Genes[i].ObjectValue);
            }

            double tmp = obliczanie_kosztu_jednego_procesora(procesory);
            if (tmp < int.MaxValue)
                return 1.0 / tmp;
            else
                return 1.0 / tmp;

        }


        private static int obliczanie_kosztu_jednego_procesora(List<proces> procesy_procesora)
        {
            int executionTime = 0;

            foreach (var proces_id in procesy_procesora)
            {
                if (proces_id.proces_wykonywany_przed.Count > 0)
                {
                    for (int i = 0; i < proces_id.proces_wykonywany_przed.Count; i++)
                    {
                        int id_procesu_wykonywanego_przed = procesy_procesora.FindIndex(t => t.id == proces_id.proces_wykonywany_przed[i].id);
                        if (proces_id.id_pocesora == proces_id.proces_wykonywany_przed[i].id_pocesora)
                        {
                            int id_procesu_wykonywanego_teraz = procesy_procesora.FindIndex(t => t.id == proces_id.id);


                            if (id_procesu_wykonywanego_przed > id_procesu_wykonywanego_teraz)
                                return int.MaxValue;
                        }
                        else
                            return int.MaxValue;
                    }
                }

                executionTime += proces_id.time;
            }

            return executionTime;
        }



        static List<proces> inicjowanie_procesów(List<proces> procesy, int K_procesorów, Random random)
        {
            List<proces> procesory = new List<proces>();
            HashSet<proces> tmp_proces = new HashSet<proces>(procesy);
            foreach (var proces in tmp_proces)
            {

                proces.id_pocesora = random.Next(K_procesorów);
                procesory.Add(proces);
            }
            return procesory;
        }

        static bool TerminateAlgorithm(Population population, int currentGeneration, long currentEvaluation)
        {

            if (currentGeneration > 100)
                // istniej coś takiego jak eksremum wartości Fitness i po nich często maleję dlatego sprawdzam wartość 
                if (population.GetTop(1)[0].Fitness == population.GetTop(2)[1].Fitness)
                    return true;


            return false;

        }



        static int inicjowanie_procesorów()
        {
            Console.WriteLine("ile K procesów:");
            return int.Parse(Console.ReadLine());
        }

    }

}

