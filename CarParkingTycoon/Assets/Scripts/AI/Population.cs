using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
	public class Population
	{
		System.Random randomize;

		NeuralNetwork[] individuals;

		float[] distanceFitness;
		float[] timeFitness;

		public int populationSize;

		int mutationRate;

		public int generationNumber;

		public int currIndiv;

		public Population(int populationSize, int[] topology, double learningRate)
		{
			this.populationSize = populationSize;
			this.mutationRate = 30;
			this.generationNumber = 1;
			this.currIndiv = -1;

			randomize = new System.Random();

			distanceFitness = new float[populationSize];
			timeFitness     = new float[populationSize];

			individuals = new NeuralNetwork[populationSize];
			for(int i = 0; i < individuals.Length; i++)
				individuals[i] = new NeuralNetwork(topology[0], topology[1], topology[2], learningRate);
		}

		public NeuralNetwork Next()
		{
			currIndiv++;
			if(currIndiv == populationSize)
				NewGeneration();
			
			return individuals[currIndiv];
		}

		public void SetFitnessOfCurrIndividual(float dist, float time)
		{
			this.distanceFitness[currIndiv] = dist;
			this.timeFitness[currIndiv]     = time;
		}

		private void NewGeneration()
		{
			NeuralNetwork first, second;
			FindFirstAndSecond(out first, out second);

			NeuralNetwork child = Crossover(first, second);

			for(int i = 0; i < populationSize; i++)
			{
				individuals[i] = Mutation(child);
			}
				
			distanceFitness = new float[populationSize];
			timeFitness     = new float[populationSize];

			generationNumber++;
			currIndiv = 0;
		}

		private void FindFirstAndSecond(out NeuralNetwork first, out NeuralNetwork second)
		{
			int firstIndex = 0;
			int secondIndex = 1;

			if(distanceFitness[firstIndex] < distanceFitness[secondIndex])
			{
				int temp = firstIndex;
				firstIndex = secondIndex;
				secondIndex = temp;
			}

			for(int i = 2; i < distanceFitness.Length; i++)
			{
				if(distanceFitness[i] > distanceFitness[secondIndex])
				{
					secondIndex = i;
					if(distanceFitness[secondIndex] > distanceFitness[firstIndex])
					{
						int temp = firstIndex;
						firstIndex = secondIndex;
						secondIndex = temp;
					}
				}
			}

			first = individuals[firstIndex];
			second = individuals[secondIndex];
		}

		// Method should be get the some (populationSize / 2) genome
		// with best distance and pick the genome with min time.
		// FIXME: It has some bugs.
		private void FindFirstAndSecond2(out NeuralNetwork first, out NeuralNetwork second)
		{
			int[] bestDistance = new int[populationSize / 2];

			for(int i = 0; i < distanceFitness.Length; i++)
			{
				for(int j = bestDistance.Length - 1; j >= 0; j--)
				{
					if(distanceFitness[i] > distanceFitness[bestDistance[j]])
					{
						if(j + 1 != bestDistance.Length)
							bestDistance[j + 1] = bestDistance[j];

						bestDistance[j] = i;
					}
				}
			}

			int firstIndex = bestDistance[0];
			int secondIndex = bestDistance[1];

			if(timeFitness[firstIndex] > timeFitness[secondIndex])
			{
				int temp = firstIndex;
				firstIndex = secondIndex;
				secondIndex = temp;
			}

			for(int i = 2; i < bestDistance.Length; i++)
			{
				if(timeFitness[bestDistance[i]] < timeFitness[secondIndex])
				{
					secondIndex = bestDistance[i];
					if(timeFitness[secondIndex] < timeFitness[firstIndex])
					{
						int temp = firstIndex;
						firstIndex = secondIndex;
						secondIndex = temp;
					}
				}
			}

//			Debug.Log(firstIndex);
//			Debug.Log(secondIndex);

			first = individuals[firstIndex];
			second = individuals[secondIndex];

//			for(int i = 0; i < distanceFitness.Length; i++)
//				Debug.Log("distanceFitness[" + i + "]: " + distanceFitness[i]);
//
//			for(int i = 0; i < bestDistance.Length; i++)
//				Debug.Log("bestDistance[" + i + "]: " + bestDistance[i]);
//
//			for(int i = 0; i < timeFitness.Length; i++)
//				Debug.Log(string.Format("time[{0}]: {1}", i, timeFitness[i]));
		}

		private NeuralNetwork Mutation(NeuralNetwork child)
		{
			// Get copy of child
			NeuralNetwork mutated = new NeuralNetwork(child);

			for(int j = 0; j < mutationRate; j++)
			{
				int rows = mutated.weights_ih.data.GetLength(0);
				int cols = mutated.weights_ih.data.GetLength(1);

				int mutatedRow = randomize.Next(rows);
				int mutatedCol = randomize.Next(cols);

				mutated.weights_ih.data[mutatedRow, mutatedCol] = randomize.NextDouble() * 2.0 - 1.0;

				rows = mutated.weights_ho.data.GetLength(0);
				cols = mutated.weights_ho.data.GetLength(1);

				mutatedRow = randomize.Next(rows);
				mutatedCol = randomize.Next(cols);

				mutated.weights_ho.data[mutatedRow, mutatedCol] = randomize.NextDouble() * 2.0 - 1.0;
			}

			return mutated;
		}

		private NeuralNetwork Crossover(NeuralNetwork nn1, NeuralNetwork nn2)
		{
			NeuralNetwork child = new NeuralNetwork(nn1.inputNodes, nn1.hiddenNodes, nn1.outputNodes, nn1.learningRate);

            int crossOver = 2;

            if (crossOver == 2)
            {
                Crossover2(nn1.weights_ih, nn2.weights_ih, child.weights_ih);
                Crossover2(nn1.weights_ho, nn2.weights_ho, child.weights_ho);
                Crossover2(nn1.bias_h, nn2.bias_h, child.bias_h);
                Crossover2(nn1.bias_o, nn2.bias_o, child.bias_o);
            }
            else
            {
                Crossover3(nn1.weights_ih, nn2.weights_ih, child.weights_ih);
                Crossover3(nn1.weights_ho, nn2.weights_ho, child.weights_ho);
                Crossover3(nn1.bias_h, nn2.bias_h, child.bias_h);
                Crossover3(nn1.bias_o, nn2.bias_o, child.bias_o);
            }
			return child;
		}
        //we are crossing over the rows like "take rows from matrix1 till
        //middle and then take rest of the matrix2 (from middle)
		private void Crossover2(Matrix m1, Matrix m2, Matrix child)
		{
			int rows = m1.data.GetLength(0);
			int cols = m2.data.GetLength(1);
			int middle = rows / 2;

			for(int i = 0; i < rows; i++)
			{
				for(int j = 0; j < cols; j++)
					if(i < middle)
						child.data[i, j] = m1.data[i, j];
					else
						child.data[i, j] = m2.data[i, j];
			}
		}
        //we are crossing over every data in two matrises
        ///m1
        /// 1 2 3 
        /// 5 6 7
        /// 
        /// m2
        /// 4 8 9
        /// 0 3 2
        /// 
        /// 4 2 9
        /// 0 6 2


    
        private void Crossover3(Matrix m1, Matrix m2, Matrix child)
        {
            int rows = m1.data.GetLength(0);
            int cols = m2.data.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    if (i % 2  == 0 )
                        child.data[i, j] = m1.data[i, j];
                    else
                        child.data[i, j] = m2.data[i, j];
            }
        }

    }
}

