using Firebase.Auth;
using Firebase.Storage;

namespace hotelDemo.Services
{
    public class FirebaseStorageManager
    {
        private static readonly string ApiKey = "AIzaSyAc5-dVQX_tddZSGlBpUGy0aPWDfUhmyZ0";
        private static readonly string Bucket = "prueba-qr-hotel.appspot.com";
        private static readonly string AuthEmail = "santiok95@gmail.com";
        private static readonly string AuthPassword = "Admin123";

      

        public static async Task<string> UploadImage(Stream stream, Dtos.ImageDTO imageDTO)
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
