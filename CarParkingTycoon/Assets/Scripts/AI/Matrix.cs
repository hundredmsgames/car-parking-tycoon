using System;

namespace NeuralNetwork
{
	public class Matrix
	{
		int rows;
		int cols;
		public double[,] data;

		static Random randomize = new Random();

		public Matrix (int rows, int cols)
		{
			this.rows = rows;
			this.cols = cols;

			data = new double[rows, cols];
		}

		// Copy Constructor
		public Matrix(Matrix m)
		{
			this.rows = m.rows;
			this.cols = m.cols;

			this.data = new double[rows, cols];

			for(int i = 0; i < rows; i++)
				for(int j = 0; j < cols; j++)
					this.data[i, j] = m.data[i, j];
		}

		// Element-wise add
		public static Matrix operator+(Matrix m1, Matrix m2)
		{
			if(m1.rows != m2.rows || m1.cols != m2.cols)
			{
				Console.WriteLine("Error: rows and cols should be same!");
				return null;
			}

			Matrix sum = new Matrix(m1.rows, m1.cols);
			for(int i = 0; i < sum.rows; i++)
			{
				for(int j = 0; j < sum.cols; j++)
				{
					sum.data[i, j] = m1.data[i, j] + m2.data[i, j];
				}
			}

			return sum;
		}

		// Element-wise subtract
		public static Matrix operator-(Matrix m1, Matrix m2)
		{
			if(m1.rows != m2.rows || m1.cols != m2.cols)
			{
				Console.WriteLine("Error: rows and cols should be same!");
				return null;
			}

			Matrix sub = new Matrix(m1.rows, m1.cols);
			for(int i = 0; i < sub.rows; i++)
			{
				for(int j = 0; j < sub.cols; j++)
				{
					sub.data[i, j] = m1.data[i, j] - m2.data[i, j];
				}
			}

			return sub;
		}

		// Element-wise multiply
		public static Matrix operator*(Matrix m1, Matrix m2)
		{
			if(m1.rows != m2.rows || m1.cols != m2.cols)
			{
				Console.WriteLine("Error: rows and cols should be same!");
				return null;
			}

			Matrix mult = new Matrix(m1.rows, m1.cols);
			for(int i = 0; i < mult.rows; i++)
			{
				for(int j = 0; j < mult.cols; j++)
				{
					mult.data[i, j] = m1.data[i, j] * m2.data[i, j];
				}
			}

			return mult;
		}

		// Element-wise divide
		public static Matrix operator/(Matrix m1, Matrix m2)
		{
			if(m1.rows != m2.rows || m1.cols != m2.cols)
			{
				Console.WriteLine("Error: rows and cols should be same!");
				return null;
			}

			Matrix div = new Matrix(m1.rows, m1.cols);
			for(int i = 0; i < div.rows; i++)
			{
				for(int j = 0; j < div.cols; j++)
				{
					if(m2.data[i, j] == 0.0)
					{
						Console.WriteLine("Error: Cannot divide by zero!");
						return null;
					}

					div.data[i, j] = m1.data[i, j] / m2.data[i, j];
				}
			}

			return div;
		}

		// Scalar add
		public static Matrix operator+(Matrix m, double x)
		{
			Matrix sum = new Matrix(m.rows, m.cols);
			for(int i = 0; i < sum.rows; i++)
			{
				for(int j = 0; j < sum.cols; j++)
				{
					sum.data[i, j] = m.data[i, j] + x;
				}
			}

			return sum;
		}

		public static Matrix operator+(double x, Matrix m)
		{
			return m + x;
		}

		// Scalar subtract
		public static Matrix operator-(Matrix m, double x)
		{
			Matrix sub = new Matrix(m.rows, m.cols);
			for(int i = 0; i < sub.rows; i++)
			{
				for(int j = 0; j < sub.cols; j++)
				{
					sub.data[i, j] = m.data[i, j] - x;
				}
			}

			return sub;
		}

		public static Matrix operator-(double x, Matrix m)
		{
			return -(m - x);
		}

		// Scalar multiply
		public static Matrix operator*(Matrix m, double x)
		{
			Matrix mult = new Matrix(m.rows, m.cols);
			for(int i = 0; i < mult.rows; i++)
			{
				for(int j = 0; j < mult.cols; j++)
				{
					mult.data[i, j] = m.data[i, j] * x;
				}
			}

			return mult;
		}

		public static Matrix operator*(double x, Matrix m)
		{
			return m * x;
		}

		// Scalar divide
		public static Matrix operator/(Matrix m, double x)
		{
			Matrix div = new Matrix(m.rows, m.cols);
			for(int i = 0; i < div.rows; i++)
			{
				for(int j = 0; j < div.cols; j++)
				{
					div.data[i, j] = m.data[i, j] / x;
				}
			}

			return div;
		}

		public static Matrix operator/(double x, Matrix m)
		{
			Matrix div = new Matrix(m.rows, m.cols);
			for(int i = 0; i < div.rows; i++)
			{
				for(int j = 0; j < div.cols; j++)
				{
					div.data[i, j] = x / m.data[i, j];
				}
			}

			return div;
		}
			
		// Negative operator
		public static Matrix operator-(Matrix m)
		{
			Matrix neg = new Matrix(m.rows, m.cols);
			for(int i = 0; i < neg.rows; i++)
			{
				for(int j = 0; j < neg.cols; j++)
				{
					neg.data[i, j] = -m.data[i, j];
				}
			}

			return neg;
		}

		// Matrix product of m1 and m2.
		public static Matrix MatrixProduct(Matrix m1, Matrix m2)
		{
			if(m1.cols != m2.rows)
			{
				Console.WriteLine("Error: Cannot get product of these two matrixes, sizes does not match!");
				return null;
			}

			Matrix product = new Matrix(m1.rows, m2.cols);
			for(int i = 0; i < m1.rows; i++)
			{
				for(int j = 0; j < m2.cols; j++)
				{
					for(int k = 0; k < m1.cols; k++)
					{
						product.data[i, j] += m1.data[i, k] * m2.data[k, j]; 
					}
				}
			}

			return product;
		}

		// Transpose of matrix m.
		public static Matrix Transpose(Matrix m)
		{
			Matrix transpose = new Matrix(m.cols, m.rows);

			for(int i = 0; i < m.rows; i++)
			{
				for(int j = 0; j < m.cols; j++)
				{
					transpose.data[j, i] = m.data[i, j];
				}
			}

			return transpose;
		}

		// Maps the matrix according to given func.
		// i.e: Changes each element according to given func.
		public void Map(Func<double, double> mapFunc)
		{
			for(int i = 0; i < this.rows; i++)
			{
				for(int j = 0; j < this.cols; j++)
				{
					this.data[i, j] = mapFunc(this.data[i, j]);
				}
			}
		}

		// Same as the non-static version of Map
		public static Matrix Map(Matrix m, Func<double, double> mapFunc)
		{
			Matrix mapped = new Matrix(m.rows, m.cols);
			for(int i = 0; i < m.rows; i++)
			{
				for(int j = 0; j < m.cols; j++)
				{
					mapped.data[i, j] = mapFunc(m.data[i, j]);
				}
			}

			return mapped;
		}

		// Randomize numbers in matrix
		public void Randomize()
		{
			for(int i = 0; i < rows; i++)
			{
				for(int j = 0; j < cols; j++)
				{
					this.data[i, j] = randomize.NextDouble() * 2.0 - 1.0;
				}
			}
		}

		// Convert matrix from array.
		public static Matrix FromArray(double[] arr)
		{
			Matrix m = new Matrix(arr.Length, 1);

			for(int i = 0; i < arr.Length; i++)
				m.data[i, 0] = arr[i];

			return m;
		}

		// Convert matrix to array.
		public static double[] ToArray(Matrix m)
		{
			double[] arr = new double[m.rows * m.cols];

			int cnt = 0;
			for(int i = 0; i < m.rows; i++)
			{
				for(int j = 0; j < m.cols; j++)
				{
					arr[cnt++] = m.data[i, j];
				}
			}

			return arr;
		}

		public override string ToString()
		{
			string ret = "";
			ret += string.Format("rows: {0}, cols: {1}\n", rows, cols);

			for(int i = 0; i < rows; i++)
			{
				for(int j = 0; j < cols; j++)
				{
					ret += data[i, j] + "\t";
				}
				ret += "\n";
			}

			return ret;
		}
	}
}

