using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm_2
{
    // Тестирование эффективности генетического алгоритма функцией Растригина.
    class GA : Chromosome
    {
        public int N; // Размерность популяции
        public int populationCount; // Счётчик популяций
        public double crossProbability, mutProbability; // Вероятность скрещивания и мутации
        List<Chromosome> population; // Исходная популяция хромосом
        List<Chromosome> parents; // Набор родительских пар
        List<Chromosome> relativeChildrens; // Набор потомков-немутантов
        List<Chromosome> currentGeneration; // Вся популяция 
        List<Chromosome> nextGeneration;// Следующее поколение эволюции

        public GA(double XoverRate, double mutRate, int popSize)
            : base()
        {
            this.crossProbability = XoverRate;
            this.mutProbability = mutRate;
            this.N = popSize;
        }

        public void CreatePopulation()
        {
            int bestFFcount = 0; // Число поколений одной и той же лучшей особи
            populationCount = 0; 
            double bestFF = 10; // Фитнесс-функция лучшей особи
            population = new List<Chromosome>();
            for (int i = 0; i < N; i++)
                population.Add(new Chromosome { });
            currentGeneration = new List<Chromosome>(population);
            foreach (Chromosome chr in currentGeneration)
                BitToDec(chr);
            foreach (Chromosome chr in currentGeneration)
                Calculate(chr);
            foreach (Chromosome chr in currentGeneration)
                FitnessFunctionCalculation(chr);

            while (bestFFcount != 10)
            {
                // Формирование популяции
                parents = new List<Chromosome>();
                for (int i = 0; i < N; i++)
                    parents.Add(new Chromosome { });
                relativeChildrens = new List<Chromosome>(parents);
                BreedingPairCreation(population, parents);
                CrossPopulation(parents, crossProbability);
                relativeChildrens = parents.Select(item => new Chromosome(item)).ToList(); 
                MutPopulation(parents, mutProbability);
                parents.AddRange(relativeChildrens);
                foreach (Chromosome chr in parents)
                    BitToDec(chr);
                foreach (Chromosome chr in parents)
                    Calculate(chr);
                foreach (Chromosome chr in parents)
                    FitnessFunctionCalculation(chr);
                foreach (Chromosome c in parents)
                    currentGeneration.Add(c);

                // Селекция особей методом колеса рулетки
                nextGeneration = new List<Chromosome>();
                for (int i = 0; i < N; i++)
                    RouletteWheel(currentGeneration, nextGeneration);
                nextGeneration.Sort((a,b) => a.FF.CompareTo(b.FF));
                double min = nextGeneration[0].FF;

                // Проверка условия остановки
                if (bestFF > nextGeneration[0].FF)
                {
                    bestFF = nextGeneration[0].FF;
                    bestFFcount = 0;
                }
                bestFFcount++;
                populationCount++;
                population = new List<Chromosome>(nextGeneration);
                currentGeneration = new List<Chromosome>(nextGeneration);
                parents.Clear();
                relativeChildrens.Clear();
            }
        }

        // Создание родительских пар методом фенотипного инбридинга
        public void BreedingPairCreation(List<Chromosome> Population, List<Chromosome> Parents)  
        {
            List<Chromosome> tmpPopulation = Population.OrderBy(a =>a.FF).ToList();
            int index = 0;
            for (int i = 0; i < N; i += 2)
            {
                if (tmpPopulation.Count == 2)
                {
                    Parents[i].X1 = tmpPopulation[0].X1;
                    Parents[i].X2 = tmpPopulation[0].X2;
                    Parents[i + 1].X1 = tmpPopulation[1].X1;
                    Parents[i + 1].X2 = tmpPopulation[1].X2;
                    tmpPopulation.RemoveRange(0, 2);
                    continue;
                }

                index = rand.Next(1, tmpPopulation.Count-1);
                double min1 = Math.Abs(tmpPopulation[index + 1].FF - tmpPopulation[index].FF);
                double min2 = Math.Abs(tmpPopulation[index - 1].FF - tmpPopulation[index].FF);
                if (min1 < min2)
                {
                    Parents[i].X1 = tmpPopulation[index].X1;
                    Parents[i].X2 = tmpPopulation[index].X2;
                    Parents[i + 1].X1 = tmpPopulation[index + 1].X1;
                    Parents[i + 1].X2 = tmpPopulation[index + 1].X2;
                    tmpPopulation.RemoveRange(index, 2);
                }
                else
                {
                    Parents[i].X1 = tmpPopulation[index].X1;
                    Parents[i].X2 = tmpPopulation[index].X2;
                    Parents[i + 1].X1 = tmpPopulation[index - 1].X1;
                    Parents[i + 1].X2 = tmpPopulation[index - 1].X2;
                    tmpPopulation.RemoveRange(index - 1, 2);
                }
            }
        }

        // Четырёхточечное простое скрещивание
        public void CrossPopulation(List<Chromosome> Parents, double p_cross) 
        {
            double n = p_cross * (double)N;
            int firstcrossPoint = 0;
            int secondcrossPoint = 0;
            int thirdcrossPoint = 0;
            int forthcrossPoint = 0;
            int tmp = 0;
            for (int i = 0; i < n; i += 2)
            {
                firstcrossPoint = rand.Next(0, l - 4);
                secondcrossPoint = rand.Next(firstcrossPoint + 1, l - 3);
                thirdcrossPoint = rand.Next(secondcrossPoint + 1, l - 2);
                forthcrossPoint = rand.Next(thirdcrossPoint + 1, l - 1);
                for (int j = firstcrossPoint; j < secondcrossPoint; j++)
                {
                    tmp = Parents[i].X1[j];
                    Parents[i].X1[j] = Parents[i + 1].X1[j];
                    Parents[i + 1].X1[j] = tmp;

                    tmp = Parents[i].X2[j];
                    Parents[i].X2[j] = Parents[i + 1].X2[j];
                    Parents[i + 1].X2[j] = tmp;
                }
                for (int j = thirdcrossPoint; j < forthcrossPoint; j++)
                {
                    tmp = Parents[i].X1[j];
                    Parents[i].X1[j] = Parents[i + 1].X1[j];
                    Parents[i + 1].X1[j] = tmp;

                    tmp = Parents[i].X2[j];
                    Parents[i].X2[j] = Parents[i + 1].X2[j];
                    Parents[i + 1].X2[j] = tmp;
                }
            }
        }

        // Мутация методом двухточечной инверсии
        public void MutPopulation(List<Chromosome> Parents, double mutProbability) 
        {
            double n = mutProbability * (double)N;
            int firstMutPoint = 0;
            int secondMutPoint = 0;
            for (int i = 0; i < n; i++)
            {
                firstMutPoint = rand.Next(1, l - 2);
                secondMutPoint = rand.Next(firstMutPoint + 1, l - 1);
                int diff = secondMutPoint - firstMutPoint;
                int[] MutArr1 = new int[diff + 1];
                int[] MutArr2 = new int[diff + 1];
                    for (int j = 0, k = firstMutPoint; j <= diff; j++, k++)
                    {
                        MutArr1[j] = Parents[i].X1[k];
                        MutArr2[j] = Parents[i].X2[k];
                    }
                Array.Reverse(MutArr1);
                Array.Reverse(MutArr2);
                    for (int j = firstMutPoint, k = 0; k <= diff; j++, k++)
                    {
                        Parents[i].X1[j] = MutArr1[k];
                        Parents[i].X2[j] = MutArr2[k];
                    }
            }
        }

        // Селекция методом колеса рулетки
        public void RouletteWheel(List<Chromosome> Childrens, List<Chromosome> NextGeneration) 
        {
            double fitnessfunctionTotalSum = 0;
            for (int i = 0; i < Childrens.Count; i++)
                fitnessfunctionTotalSum += Childrens[i].FF;
            double[] intervalSize = new double[Childrens.Count];
            double intervalTotalSum = 0;
            intervalSize[0] = (1 / Childrens[0].FF) / (1 / fitnessfunctionTotalSum);
            for (int i = 1; i < Childrens.Count; i++)
            {
                intervalSize[i] = (1/Childrens[i].FF)/(1/fitnessfunctionTotalSum) + intervalSize[i - 1];
                intervalTotalSum = intervalSize[i];
            }
            double rouletteChoice = rand.NextDouble() * intervalTotalSum;
            for (int i = 0; i < Childrens.Count; i++)
            {
                if(intervalSize[i] > rouletteChoice)
                {
                    if (i == 0)
                    {
                        NextGeneration.Add(Childrens[i]);
                        break;
                    }
                    NextGeneration.Add(Childrens[i-1]);
                    break;
                }
            }
        }

        // Показатель качества решения алгоритма
        public double QualityIndicator()
        {
            double q = 0;
            q = (0 + 1) / (population[0].FF + 1);
            q = Math.Round(q, 2);
            return q;
        }

        //Скорость сходимости алгоритма
        public double ConvergenceRate()
        {
            double c = 0;
            c = populationCount - 10;
            return c;
        }
    }

    class Chromosome
    {
        public static Random rand = new Random();
        public int[] X1; // Набор генов хромосомы №1
        public int[] X2; // Набор генов хромосомы №2
        public int l = 8; // Размер хромосомы
        public double a = -16, b = 16; // Границы функции
        public double x1; // Десятичное представление набора генов хромосомы №1
        public double x2; // Десятичное представление набора генов хромосомы №2
        public double FF; // Значение фитнесс-функции
        public double Xi_1; // Числовое значение хромосомы №1
        public double Xi_2; // Числовое значение хромосомы №2

        public Chromosome()
        {
            this.x1 = 0;
            this.x2 = 0;
            this.FF = 0;
            this.X1 = new int[l];
            this.X2 = new int[l];
            for (int i = 0; i < l; i++) this.X1[i] = rand.Next(2);
            for (int i = 0; i < l; i++) this.X2[i] = rand.Next(2);
        }

        public Chromosome(Chromosome chrom)
        {
            this.x1 = chrom.x1;
            this.x2 = chrom.x2;
            this.FF = chrom.FF;
            this.X1 = new int[chrom.X1.Length];
            this.X2 = new int[chrom.X2.Length];
            Array.Copy(chrom.X1, this.X1, chrom.X1.Length);
            Array.Copy(chrom.X2, this.X2, chrom.X2.Length);
        }

        // Перевод двоичного кода в представленной хромосоме в десятичную систему
        public void BitToDec(Chromosome c)
        {
            for (int i = l - 1, count = 0; i >= 0; i--, count++)
                if (c.X1[i] == 1) c.x1 += (int)Math.Pow(2, count);
                else continue;
            for (int j = l - 1, count = 0; j >= 0; j--, count++)
                if (c.X2[j] == 1) c.x2 += (int)Math.Pow(2, count);
                else continue;
        }

        // Вычисление Xi по формуле Xi = |b-a|/(2^l-1) * X10 + a
        public void Calculate(Chromosome c)
        {
            c.Xi_1 = ((Math.Abs(b) + Math.Abs(-a)) / (Math.Pow(2, l) - 1)) * c.x1 + a;
            c.Xi_2 = ((Math.Abs(b) + Math.Abs(-a)) / (Math.Pow(2, l) - 1)) * c.x2 + a;
        }

        // Вычисление значения фитнесс-функции. Функция Растригина: I(x,y) = 0.1x^2 + 0.1y^2 - 4 cos(0.8x) - 4 cos(0.8y) + 8 
        public void FitnessFunctionCalculation(Chromosome c)
        {
            c.FF = 0.1 * Math.Pow(c.Xi_1, 2) + 0.1 * Math.Pow(c.Xi_2, 2) - 4 * Math.Cos(0.8 * c.Xi_1) - 4 * Math.Cos(0.8 * c.Xi_2) + 8;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            double[,] quality = new double[10, 10]; // Хранение результатов точности решений
            double[,] speed = new double[10, 10]; // Хранение результатов скорости сходимости
            Console.WriteLine("Исследование зависимости параметров качества решений ГА от размерности популяций");
            for (int j = 0; j < 10; j++)
            {
                int count = 1;
                for (int i = 0; i < 10; i++)
                {
                    GA ga = new GA(1, 1, count * 10);
                    ga.CreatePopulation();
                    quality[j, i] = ga.QualityIndicator();
                    speed[j, i] = ga.ConvergenceRate();
                    count++;
                }
            }

            int number = 1;
            for (int j = 0; j < 10; j++)
            {
                double Quality = 0;
                double Speed = 0;
                for (int i = 0; i < 10; i++)
                {
                    Quality += quality[i, j];
                    Speed += speed[i, j];
                }
                Console.WriteLine("Точность решения для " + number * 10 + " особей: " + Quality / 10);
                Console.WriteLine("Скорость сходимости для " + number * 10 + " особей: " + Speed / 10);
                number++;
            }

            Console.WriteLine("\nИсследование зависимости параметров от вероятности скрещивания");
            for (int j = 0; j < 10; j++)
            {
                int count = 1;
                for (int i = 0; i < 10; i++)
                {
                    GA ga = new GA(count * 0.1, 1, 100);
                    ga.CreatePopulation();
                    quality[j, i] = ga.QualityIndicator();
                    speed[j, i] = ga.ConvergenceRate();
                    count++;
                }
            }

            number = 1;
            for (int j = 0; j < 10; j++)
            {
                double Quality = 0;
                double Speed = 0;
                for (int i = 0; i < 10; i++)
                {
                    Quality += quality[i, j];
                    Speed += speed[i, j];
                }
                Console.WriteLine("Точность решения при " + number * 0.1 + " вероятности скрещивания: " + Quality / 10);
                Console.WriteLine("Скорость сходимости при " + number * 0.1 + " вероятности скрещивания: " + Speed / 10);
                number++;
            }

            Console.WriteLine("\nИсследование зависимости параметров от вероятности мутации");
            for (int j = 0; j < 10; j++)
            {
                int count = 1;
                for (int i = 0; i < 10; i++)
                {
                    GA ga = new GA(1, count * 0.1, 100);
                    ga.CreatePopulation();
                    quality[j, i] = ga.QualityIndicator();
                    speed[j, i] = ga.ConvergenceRate();
                    count++;
                }
            }

            number = 1;
            for (int j = 0; j < 10; j++)
            {
                double Quality = 0;
                double Speed = 0;
                for (int i = 0; i < 10; i++)
                {
                    Quality += quality[i, j];
                    Speed += speed[i, j];
                }
                Console.WriteLine("Точность решения при " + number * 0.1 + " вероятности мутации: " + Quality / 10);
                Console.WriteLine("Скорость сходимости при " + number * 0.1 + " вероятности мутации: " + Speed / 10);
                number++;
            }
            Console.Read();
        }
    }
}
