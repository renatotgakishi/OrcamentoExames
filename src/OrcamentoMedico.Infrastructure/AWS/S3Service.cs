using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using OrcamentoMedico.Application.Interfaces;

namespace OrcamentoMedico.Infrastructure.Aws
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3;
        private readonly string _bucket = string.Empty;

        public S3Service(IConfiguration config)
        {
            var awsConfig = new AmazonS3Config
            {
                ServiceURL = config["AWS:ServiceURL"],
                ForcePathStyle = true
            };
            _s3 = new AmazonS3Client(config["AWS:AccessKey"], config["AWS:SecretKey"], awsConfig);

            _bucket = config["AWS:S3Bucket"] ?? throw new InvalidOperationException("S3Bucket config is missing.");
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName)
        {
            await EnsureBucketExistsAsync(); // garante que o bucket existe

            var key = $"{Guid.NewGuid()}_{fileName}";
            await _s3.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _bucket,
                Key = key,
                InputStream = fileStream
            });
            return key;
        }
        private bool _bucketChecked = false;

        private async Task EnsureBucketExistsAsync()
        {
            if (_bucketChecked) return;

            var buckets = await _s3.ListBucketsAsync();
            if (!buckets.Buckets.Any(b => b.BucketName == _bucket))
            {
                await _s3.PutBucketAsync(new PutBucketRequest
                {
                    BucketName = _bucket
                });
            }

            _bucketChecked = true;
        }
        public async Task InitializeAsync()
        {
            await EnsureBucketExistsAsync();
        }
        
    }

}
