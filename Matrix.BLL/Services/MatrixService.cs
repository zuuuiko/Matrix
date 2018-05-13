using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrix.BLL.Models.Enums;

namespace Matrix.BLL
{
    public class MatrixService : IMatrixService
    {
        public int[][] GenerateRandomMatrix()
        {
            var random = new Random();
            var order = random.Next(2, 11);
            var matrix = new int[order][];
            for (int i = 0; i < matrix.Length; i++)
            {
                var line = new byte[order];
                random.NextBytes(line);
                matrix[i] = Array.ConvertAll(line, x => (int)x);
            }
            return matrix;
        }
        public int[][] RotateMatrix(int[][] matrix, bool clockwise = true, AngleRotation angleRotation = AngleRotation.Angle_90)
        {
            ValidateMatrix(matrix);
            var n = matrix.Length;
            var p = n / 2;
            for (int i = 0; i < p; i++)
            {
                for (int j = i; j < n - 1 - i; j++)
                {
                    var temp = matrix[i][j];
                    matrix[i][j] = matrix[n - j - 1][i];
                    matrix[n - j - 1][i] = matrix[n - i - 1][n - j - 1];
                    matrix[n - i - 1][n - j - 1] = matrix[j][n - i - 1];
                    matrix[j][n - i - 1] = temp;
                }
            }
            return matrix;
        }
        private void ValidateMatrix(int[][] matrix)
        {
            var valid = true;

            if (matrix == null || matrix.Length < 2)
            {
                valid = false;
            }
            else
            {
                for (int i = 0; i < matrix.Length; i++)
                {
                    if (matrix[i].Length != matrix.Length)
                    {
                        valid = false;
                        break;
                    }
                }
            }
            
            if (!valid) throw new ArgumentException("Invalid square matrix");
        }
    }
}
