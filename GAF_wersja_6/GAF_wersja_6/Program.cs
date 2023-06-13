using System;
using System.Collections.Generic;
using System.Diagnostics;
using GAF;
using GAF.Operators;
using nowy__Algorytm_wyrzażania_dla_n_procesów;

namespace GAF_wersja_6
{
    internal class Program
    {
        static int K_procesory = 3;
        static int N_procesów = 2;
        static List<proces> procesy = InicjalizujProcesy(N_procesów);

        private static void Main(string[] args)
        {
            var population = new Population();

            for (int i = 0; i < 100; i++)
            {
                var chromosome = new Chromosome();
                var procesy = InicjalizujProcesy(N_procesów);
                foreach (var procesor in inicjowanie(procesy, K_procesory))
                {
                    chromosome.Genes.Add(new Gene(procesor));
                }
                population.Solutions.Add(chromosome);
            }

            var ga = new GeneticAlgorithm(population, Fitness);

            ga.OnGenerationComplete += Ga_OnGenerationComplete;

            var crossover = new Crossover(0.5, false)
            {
                CrossoverType = CrossoverType.DoublePointOrdered
            };
            var mutation = new SwapMutate(0.5);

            ga.Operators.Add(crossover);
            ga.Operators.Add(mutation);

            ga.Run(TerminateAlgorithm);
        }

        static bool TerminateAlgorithm(Population population, int currentGeneration, long currentEvaluation)
        {
            const int maxGenerationsWithoutImprovement = 50;
            const double minFitnessImprovementThreshold = 0.001;

            if (currentGeneration >= maxGenerationsWithoutImprovement)
            {
                var bestFitness = population.GetTop(1)[0].Fitness;
                var previousBestFitness = population.GetTop(2)[1].Fitness;

                if (bestFitness - previousBestFitness < minFitnessImprovementThreshold)
                {
                    return true;
                }
            }

            return false;
        }

        private static List<proces> InicjalizujProcesy(int N_procesów)
        {
            List<proces> procesy = new List<proces>();

            for (int i = 0; i < N_procesów; i++)
            {
                proces proces = new proces(i, 0, i + 1, i - 1);

                procesy.Add(proces);
            }
            procesy.Add(new proces(N_procesów, -3, 0, -1));
            return procesy;
        }

        private static List<procesor> inicjowanie(List<proces> procesy, int K_procesory)
        {
            List<procesor> procesory = new List<procesor>();

            for (int i = 0; i < K_procesory; i++)
            {
                procesory.Add(new procesor(i, new List<int>()));
            }

            foreach (var proces in procesy)
            {
                int Id_procesora = new Random().Next(procesory.Count);
                proces.id_pocesora = Id_procesora;
                procesory[Id_procesora].pocesy.Add(proces.id);
            }
            return procesory;
        }

        private static void Ga_OnGenerationComplete(object sender, GaEventArgs e)
        {
            Console.WriteLine("Generacja: " + e.Generation);
            Console.WriteLine("Populacja: ");
            var chromosom = e.Population.GetTop(1)[0];

            foreach (var gen in chromosom.Genes)
            {
                Console.WriteLine($"Procesor: {((procesor)gen.ObjectValue).id}");
                foreach (var proces in ((procesor)gen.ObjectValue).pocesy)
                {
                    Console.WriteLine(proces);
                }
            }
            Console.WriteLine($"Wartość funkcji dopasowania: {chromosom.Fitness}");

            Console.WriteLine();
        }

        private static double Fitness(Chromosome chromosome)
        {
            List<procesor> procesory = new List<procesor>();
            for (int i = 0; i < chromosome.Genes.Count; i++)
            {
                procesory.Add((procesor)chromosome.Genes[i].ObjectValue);
            }
            double tmp = fukcja_kosztu(procesory);
            return 1.0 / tmp;
        }

        private static int fukcja_kosztu(List<procesor> procesory)
        {
            int max = 0;
            foreach (var procesor in procesory)
            {
                int czas = obliczanie_kosztu_jednego_procesora(new List<int>(), procesory, procesor.pocesy);
                if (czas > max)
                    max = czas;
            }

            return max;
        }

        private static int obliczanie_kosztu_jednego_procesora(List<int> odwiedzone, List<procesor> procesory, List<int> procesy_procesora)
        {
            int executionTime = 0;
            int tmpTime = 0;
            foreach (var proces_id in procesy_procesora)
            {
                if (procesy[proces_id].proces_wykonywany_przed != -1)
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
                        tmpTime = obliczanie_kosztu_jednego_procesora(odwiedzone, procesory, procesory[index_procesora].pocesy.GetRange(0, index_procesu_na_procesorze));
                        if (tmpTime >= executionTime)
                            return int.MaxValue;
                    }
                }
                executionTime += procesy[proces_id].time;
            }
            return executionTime;
        }
    }
}
