using UnityEngine;
using System.Collections;
using System.IO;
using SimpleFileBrowser;
using UnityEngine.UI;
using Firebase.Storage;
using Firebase.Extensions;
using Unity.VisualScripting;

public class FirebaseFileUploader : MonoBehaviour
{

    [SerializeField] Button loadFileBtn;
    private FirebaseStorage storage;
    private StorageReference storageRef;
    void Start()
    {

        InitializeFirebase();


        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));

        FileBrowser.SetDefaultFilter(".jpg");

        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");


        FileBrowser.AddQuickLink("Users", "C:\\Users", null);
        loadFileBtn.onClick.AddListener(LoadFile);
    }
    void InitializeFirebase()
    {
        // Get a reference to the storage service, using the default Firebase App
        storage = FirebaseStorage.DefaultInstance;
        storageRef = storage.GetReferenceFromUrl("gs://embodytest-b96c0.appspot.com");
    }
    void LoadFile()
    {
        StartCoroutine(ShowLoadDialogCoroutine());
    }
    IEnumerator ShowLoadDialogCoroutine()
    {

        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true, null, null, "Select Files", "Load");

        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
            OnFilesSelected(FileBrowser.Result); // FileBrowser.Result is null, if FileBrowser.Success is false
    }

    void OnFilesSelected(string[] filePaths)
    {
        for (int i = 0; i < filePaths.Length; i++)
            Debug.Log(filePaths[i]);

        string filePath = filePaths[0];

        byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(filePath);
        var newmetaData = new MetadataChange();
        newmetaData.ContentType ="image/jpeg/png";

        StorageReference uploadRef = storageRef.Child("Uploads/images.png");
        uploadRef.PutBytesAsync(bytes, newmetaData).ContinueWithOnMainThread((task) =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                Debug.Log("file Uploaded successfully");

            }
        });
    }
}