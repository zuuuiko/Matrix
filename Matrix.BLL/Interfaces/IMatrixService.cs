namespace Matrix.BLL
{
    public interface IMatrixService
    {
        /// <summary>
        /// Generate random square matrix of order n, where n = {2...10}
        /// </summary>
        /// <returns></returns>
        int[][] GenerateRandomMatrix();
        /// <summary>
        /// Rotate matrix for 90° clockwise.
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="clockwise"></param>
        /// <param name="angleRotation"></param>
        /// <returns></returns>
        int[][] RotateMatrix(int[][] matrix, bool clockwise = true, 
            Models.Enums.AngleRotation angleRotation = Models.Enums.AngleRotation.Angle_90);
    }
}
