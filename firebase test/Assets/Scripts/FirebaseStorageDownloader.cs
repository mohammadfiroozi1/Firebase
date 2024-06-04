using UnityEngine;
using UnityEngine.UI;
using Firebase.Storage;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase;

public class FirebaseStorageDownloader : MonoBehaviour
{
    public RawImage rawImage; // Assign this from the Inspector

    private FirebaseStorage storage;
    private StorageReference storageRef;

    void Start()
    {
        // Ensure Firebase is initialized
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                InitializeFirebase();
                DownloadFile("row-1-column-7.png");
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    void InitializeFirebase()
    {
        // Get a reference to the storage service, using the default Firebase App
        storage = FirebaseStorage.DefaultInstance;
        storageRef = storage.GetReferenceFromUrl("gs://embodytest-b96c0.appspot.com");
    }

    public void DownloadFile(string filePath)
    {
        StorageReference fileRef = storageRef.Child(filePath);

        // Download the file as a byte array without specifying max size
        fileRef.GetBytesAsync(long.MaxValue).ContinueWithOnMainThread(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Download failed: " + task.Exception);
            }
            else
            {
                Debug.Log("File downloaded successfully");
                byte[] fileData = task.Result;

                // Create a texture and load the image data into it
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(fileData);

                // Assign the texture to the RawImage
                rawImage.texture = texture;
             //   rawImage.SetNativeSize();
            }
        });
    }
}
