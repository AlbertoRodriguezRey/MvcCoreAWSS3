using Amazon.S3;
using Amazon.S3.Model;

namespace MvcCoreAWSS3.Services
{
    public class ServiceStorageS3
    {
        private string BucketName;
        private IAmazonS3 clientS3;
        public ServiceStorageS3(IConfiguration configuration, IAmazonS3 clientS3)
        {
            this.BucketName = configuration.GetValue<string>("AWS:BucketName");
            this.clientS3 = clientS3;
        }

        //METODO PARA SUBIR FICHEROS
        public async Task<int> UploadFileAsync(string fileName, Stream FileStream)
        {
            PutObjectRequest request = new PutObjectRequest
            {
                Key = fileName,
                BucketName = this.BucketName,
                InputStream = FileStream
            };
            PutObjectResponse response = await this.clientS3.PutObjectAsync(request);
            return (int)response.HttpStatusCode;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            DeleteObjectRequest request = new DeleteObjectRequest
            {
                Key = fileName,
                BucketName = this.BucketName
            };
            await this.clientS3.DeleteObjectAsync(request);
        }

        public async Task<List<string>> GetFilesAsync()
        {
            ListVersionsResponse response = await this.clientS3.ListVersionsAsync(this.BucketName);
            if (response.Versions == null)
            {
                return new List<string>();
            }
            List<string> ficheros = response.Versions.Select(v => v.Key).ToList();
            return ficheros;
        }
    }
}

