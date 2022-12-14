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
        public string GetURL();
        public string UploadFile(byte[] file);
        public Task<MemoryStream> DownloadFile(string url);
        public Task DeleteFile(string url);
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
            publicKey = "AKIAYCXVMZ23OLLQWEIR"; //Environment.GetEnvironmentVariable("AWS_PUBLIC_KEY");
            secretKey = "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"; //Environment.GetEnvironmentVariable("AWS_PRIVATE_KEY");
            bucketName = "pos-arnel-bucket"; //Environment.GetEnvironmentVariable("AWS_BUCKET_NAME");
            s3Client = new AmazonS3Client(publicKey, secretKey, bucketRegion);
        }

        public async Task DeleteFile(string url)
        {
            var fileTransferUtility = new TransferUtility(s3Client);
            var fileName = url.Split('/').Last();
            await fileTransferUtility.S3Client.DeleteObjectAsync(new DeleteObjectRequest()
            {
                BucketName = bucketName,
                Key = fileName
            });
        }

        public async Task<MemoryStream> DownloadFile(string url)
        {
            var fileName = url.Split('/').Last();
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileName
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

        public string GetURL()
        {
            return "https://" + bucketName + ".s3." + bucketRegion.SystemName + ".amazonaws.com/";
        }

        public string UploadFile(IFormFile file)
        {
            var fileTransferUtility =
                    new TransferUtility(s3Client);
            fileTransferUtility.Upload(file.OpenReadStream(), bucketName, file.FileName);
            return file.FileName;
        }

        public string UploadFile(byte[] file)
        {
            var name = Guid.NewGuid().ToString();
            var fileTransferUtility =
                    new TransferUtility(s3Client);
            Stream stream = new MemoryStream(file);
            fileTransferUtility.Upload(stream, bucketName, name);
            return name;
        }
    }
}
