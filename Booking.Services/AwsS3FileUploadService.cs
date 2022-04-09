using Booking.Contracts.Options;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace Booking.Services
{
    public class AwsS3FileUploadService
    {
        private readonly AwsS3Options _options;

        public AwsS3FileUploadService(IOptions<AwsS3Options> options)
        {
            _options = options.Value;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName("ap-northeast-2"),
            };

            var s3 = new AmazonS3Client(
                _options.AccessKey,
                _options.SecretKey,
                config
            );

            var guid = Guid.NewGuid().ToString();

            var request = new PutObjectRequest
            {
                BucketName = _options.BucketName,
                Key = guid + fileName,
                InputStream = fileStream,
                ContentType = contentType,
                CannedACL = S3CannedACL.PublicRead,
                
            };

            await s3.PutObjectAsync(request);
            return $"https://{_options.BucketName}.s3.ap-northeast-2.amazonaws.com/{guid + fileName}";
        }


    }

}
