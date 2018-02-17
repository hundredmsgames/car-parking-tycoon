using System;
using System.Collections;
using UnityEngine;

namespace NeuralNetwork
{
	public class Population
	{
		System.Random randomize;

		NeuralNetwork[] individuals;

		float[] fitness;

		public int populationSize;

		int mutationRate;

		public int generationNumber;

		public int currIndiv;

		public Population(int populationSize, int[] topology, double learningRate)
		{
			this.populationSize = populationSize;
			this.mutationRate = 3;
			this.generationNumber = 1;
			this.currIndiv = -1;

			randomize = new System.Random();

			fitness = new float[populationSize];
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

		public void SetFitnessOfCurrIndividual(float fitness)
		{
			this.fitness[currIndiv] = fitness;
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
				
			fitness = new float[populationSize];

			generationNumber++;
			currIndiv = 0;
		}

		private void FindFirstAndSecond(out NeuralNetwork first, out NeuralNetwork second)
		{
			int firstIndex = 0;
			int secondIndex = 1;

			if(fitness[firstIndex] < fitness[secondIndex])
			{
				int temp = firstIndex;
				firstIndex = secondIndex;
				secondIndex = temp;
			}

			for(int i = 2; i < fitness.Length; i++)
			{
				if(fitness[i] > fitness[secondIndex])
				{
					secondIndex = i;
					if(fitness[secondIndex] > fitness[firstIndex])
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

			Crossover2(nn1.weights_ih, nn2.weights_ih, child.weights_ih);
			Crossover2(nn1.weights_ho, nn2.weights_ho, child.weights_ho);
			Crossover2(nn1.bias_h, nn2.bias_h, child.bias_h);
			Crossover2(nn1.bias_o, nn2.bias_o, child.bias_o);

			return child;
		}

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
	}
}

