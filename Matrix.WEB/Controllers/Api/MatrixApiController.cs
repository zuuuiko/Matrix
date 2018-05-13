using Matrix.BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Matrix.WEB.Controllers.Api
{
    public class MatrixApiController : ApiController
    {
        private readonly IMatrixService matrixService;
        public MatrixApiController(IMatrixService matrixService)
        {
            this.matrixService = matrixService;
        }
        [Route("api/MatrixApi/GetRandomMatrix")]
        public int[][] GetRandomMatrix()
        {
            return matrixService.GenerateRandomMatrix();
        }
        [HttpGet]
        [Route("api/MatrixApi/RotateMatrix")]
        public int[][] RotateMatrix([FromUri]int[][] matrix)
        {
            return matrixService.RotateMatrix(matrix);
        }
        [HttpPost]
        [Route("api/MatrixApi/GetMatrixFromFile")]
        public async Task<HttpResponseMessage> GetMatrixFromFile()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "The request doesn't contain valid content!");
            }

            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                var file = provider.Contents.First();
                var stream = await file.ReadAsStreamAsync();
                int[][] matrix = null;
                using (var reader = new StreamReader(stream))
                {
                    int i = 0;
                    while (reader.ReadLine() != null) { i++; }
                    matrix = new int[i][];
                    stream.Position = 0;
                    reader.DiscardBufferedData();
                    i = 0; 
                    while (!reader.EndOfStream)
                    {
                        matrix[i] = reader.ReadLine().Split(',').Select(x=>int.Parse(x)).ToArray();
                        i++;
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, matrix);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("api/MatrixApi/ExportMatrixToFile")]
        public HttpResponseMessage ExportMatrixToFile([FromUri]string[][] matrix)
        {
            var stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            try
            {
                for (int i = 0; i < matrix.Length; i++)
                {
                    writer.WriteLine(string.Join(",", matrix[i]));
                }
                writer.Flush();
                stream.Position = 0;
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "export.csv"
                };

                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}