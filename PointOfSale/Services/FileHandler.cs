using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PointOfSale.Services
{
    public interface IFileHandler
    {
        public string UploadFile(IFormFile file);
        public Task<MemoryStream> DownloadFile(string name);
        public Task DeleteFile(string name);
    }
    public class FileHandler : IFileHandler
    {
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast2;
        private static IAmazonS3 s3Client;
        private static string publicKey;
        private static string secretKey;
        private static string bucketName;
        
        public FileHandler()
        {
            publicKey = Environment.GetEnvironmentVariable("AWS_PUBLIC_KEY");
            secretKey = Environment.GetEnvironmentVariable("AWS_PRIVATE_KEY");
            bucketName = Environment.GetEnvironmentVariable("AWS_BUCKET_NAME");
            s3Client = new AmazonS3Client(publicKey, secretKey, bucketRegion);
        }

        public async Task DeleteFile(string name)
        {
            var fileTransferUtility = new TransferUtility(s3Client);
            await fileTransferUtility.S3Client.DeleteObjectAsync(new DeleteObjectRequest()
            {
                BucketName = bucketName,
                Key = name
            });
        }

        public async Task<MemoryStream> DownloadFile(string name)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = name
            };
            GetObjectResponse response = await s3Client.GetObjectAsync(request);
            Stream responseStream = response.ResponseStream;
            using (MemoryStream memStream = new MemoryStream())
            {
                responseStream.CopyTo(memStream);
                memStream.Seek(0, SeekOrigin.Begin);
                return memStream;
            }
        }

        public string UploadFile(IFormFile file)
        {
            var fileTransferUtility =
                    new TransferUtility(s3Client);
            fileTransferUtility.Upload(file.OpenReadStream(), bucketName, file.FileName);
            return file.FileName;
        }
    }
}
