// See https://aka.ms/new-console-template for more information



using System.Diagnostics;
using System.Security.Cryptography;

class Program
{
    public static double funkcja_kosztu(double x)
    {
        return Math.Pow(x,2);
    }


    static void Main()
    {
        
        double T = 10000;
        Random random = new Random();

        double X=random.NextDouble()*20-10;
        double wspułczynik_schladzania = 0.10;

        while (T>1)
        {
            double x = random.NextDouble() * 20 -10;
            if (funkcja_kosztu(X) - funkcja_kosztu(x) < 0)
                X = x;
            else
                 if (Math.Exp(-(funkcja_kosztu(X) - funkcja_kosztu(x)) / T) < random.NextDouble())
            {
                X = x;
            }
                ///schładanie
            T = 1 - wspułczynik_schladzania;
        }

        Console.WriteLine("x:"+X );
        Console.WriteLine("Wartość funkcji X^2: " + funkcja_kosztu(X));
    }
}