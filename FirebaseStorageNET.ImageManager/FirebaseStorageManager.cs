using Firebase.Auth;

using Firebase.Storage;
using FirebaseStorageNET.DTOs;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FirebaseStorageNET.ImageManager
{
    public class FirebaseStorageManager
    {
        private static readonly string ApiKey = "AIzaSyAc5-dVQX_tddZSGlBpUGy0aPWDfUhmyZ0";
        private static readonly string Bucket = "prueba-qr-hotel.appspot.com";
        private static readonly string AuthEmail = "santiok95@gmail.com";
        private static readonly string AuthPassword = "Admin123";

        public static StreamContent ConvertBase64ToStream(string imageFromRequest)
        {
            byte[] imageStringToBase64 = Convert.FromBase64String(imageFromRequest);
            StreamContent streamContent = new(new MemoryStream(imageStringToBase64));
            return streamContent;
        }

        public static async Task<string> UploadImage(Stream stream, ImageDTO imageDTO)
        {
            string imageFromFirebaseStorage = "";
            FirebaseAuthProvider firebaseConfiguration = new(new FirebaseConfig(ApiKey));

            FirebaseAuthLink authConfiguration = await firebaseConfiguration
                .SignInWithEmailAndPasswordAsync(AuthEmail, AuthPassword);

            CancellationTokenSource cancellationToken = new();

            FirebaseStorageTask storageManager = new FirebaseStorage(Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(authConfiguration.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child(imageDTO.FolderName)
                .Child(imageDTO.ImageName)
                .PutAsync(stream, cancellationToken.Token);

            try
            {
                imageFromFirebaseStorage = await storageManager;
            }
            catch
            {
            }
            return imageFromFirebaseStorage;
        }
    }
}