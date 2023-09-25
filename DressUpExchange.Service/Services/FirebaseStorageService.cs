using DressUpExchange.Service.Ultilities;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.Services
{
    public interface IFileStorageService
    {
        Task<string> UploadFileToDefaultAsync(Stream fileStream, string fileName);
        Task<string> UploadFileToDefaultAsyncV2(IFormFile fileStream, string folderName);

        Task<bool> DeleteFileByName(string fileName);
    }
    public class FirebaseStorageService : IFileStorageService
    {
        private readonly IConfiguration _config;
        private String ApiKey;
        private static string Bucket;
        private static string AuthEmail;
        private static string AuthPassword;
        public FirebaseStorageService(IConfiguration config)
        {
            _config = config;
            ApiKey = _config["Firebase:ApiKey"];
            Bucket = _config["Firebase:Bucket"];
            AuthEmail = _config["EmailUserName"];
            AuthPassword = _config["EmailPassword"];
        }
        public async Task<string> UploadFileToDefaultAsync(Stream fileStream, string fileName)
        {
            var auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
            var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            var cancellation = new CancellationTokenSource();
            var task = new FirebaseStorage(Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                }
                ).Child("images").Child(fileName).PutAsync(fileStream, cancellation.Token);
            try
            {
                string link = await task;
                return link;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string> UploadFileToDefaultAsyncV2(IFormFile fileStream, string folderName)
        {
            FireBaseFile fireBaseFile = await FileUtils.UploadFileAsync(fileStream, "picture");
            return fireBaseFile.URL.ToString();
        }

        public async Task<bool> DeleteFileByName(string fileName)
        {
            await FileUtils.RemoveFileAsync(fileName, "picture");
            return true;
        }
    }
}
