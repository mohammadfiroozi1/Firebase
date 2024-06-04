using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class AddUserToFirestore : MonoBehaviour
{
    FirebaseFirestore db;
    bool isFirebaseInitialized = false;

    void Start()
    {
        InitializeFirebase();
    }

    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                db = FirebaseFirestore.DefaultInstance;
                isFirebaseInitialized = true;
                Debug.Log("Firebase Initialized");
                ReadData();  // Read data after initialization
               // AddUser();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
                isFirebaseInitialized = false;
            }
        });
    }

    void ReadData()
    {
        if (!isFirebaseInitialized)
        {
            Debug.LogError("Firebase is not initialized.");
            return;
        }

        CollectionReference usersRef = db.Collection("steps");

        usersRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                QuerySnapshot snapshot = task.Result;
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    // Get the document data as a dictionary
                    Dictionary<string, object> documentData = document.ToDictionary();

                    // Custom conversion for StepData with nested DesicionQuestion
                    StepData stepData = ConvertDocumentToStepData(documentData);

                    if (stepData != null)
                    {
                        // Log or use the stepData object here
                        Debug.Log("Document ID: " + document.Id);
                        Debug.Log("content_url: " + stepData.content_url);
                        // ... (log other properties)
                    }
                    else
                    {
                        Debug.LogError("Failed to convert document data to StepData object.");
                    }
                }
            }
            else
            {
                Debug.LogError("Error getting documents: + task.Exception");
            }
        });
    }
    private StepData ConvertDocumentToStepData(Dictionary<string, object> documentData)
    {
        if (documentData == null)
        {
            return null;
        }

        StepData stepData = new StepData();

        try
        {
            stepData.content_url = documentData["content_url"] as string;
            stepData.location = documentData["location"] as string;
            stepData.narriation = documentData["narriation"] as string;
            stepData.nextStep = null; // Handle as needed
            stepData.previousStep = null; // Handle as needed
            stepData.quizId = documentData["quizId"] as string;

            // Handle nested DesicionQuestion
            if (documentData.ContainsKey("desicionQuestion"))
            {
                Dictionary<string, object> desicionQuestionData = documentData["desicionQuestion"] as Dictionary<string, object>;
                stepData.desicionQuestion = new DesicionQuestion()
                {
                    question = desicionQuestionData["question"] as string,
                    answer1 = desicionQuestionData["answer1"] as string,
                    answer2 = desicionQuestionData["answer2"] as string,
                    // Handle nested answer1_stepData and answer2_stepData as needed (might require recursion)
                    answer1_stepData = null, // Handle as needed
                    answer2_stepData = null, // Handle as needed
                };
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error converting document data to StepData: " + ex.Message);
            return null;
        }

        return stepData;
    }
    void AddUser()
    {
        if (!isFirebaseInitialized)
        {
            Debug.LogError("Firebase is not initialized.");
            return;
        }

        StepData stepBro = new StepData()
        {
            content_url = "felan url",
            desicionQuestion = new DesicionQuestion()
            {
                question = "soal 1",
                answer1 = "javabe 1",
                answer2 = "javabe 2",
                answer1_stepData = null,
                answer2_stepData = null
            },
            location = "otaghe MRI",
            narriation = "guyande text",
            nextStep = null,
            previousStep = null,
            quizId = "2"
        };
        var jsonData = JsonUtility.ToJson(stepBro);

        // Generate a unique document ID to prevent overwrites
        string documentId = db.Collection("steps").Document().Id;

        CollectionReference userRef = db.Collection("steps");
        userRef.Document(documentId).SetAsync(JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonData))
               .ContinueWithOnMainThread(task =>
               {
                   if (task.IsFaulted)
                   {
                       Debug.LogError("Error saving data: " + task.Exception);
                   }
                   else
                   {
                       Debug.Log($"Data saved successfully! Document ID: {documentId}");
                   }
               });
        //  Data for the new user

        //Dictionary<string, object> user = new Dictionary<string, object>
        //{
        //     { "content_url", "fl" },
        //     { "email", "kalids@ga.com" },
        //     { "age", 22 },
        //     { "isPremiumUser", false },
        //     { "createdAt", Timestamp.FromDateTime(DateTime.UtcNow) }
        //};

        //// Add the new user document

        //userRef.AddAsync(user).ContinueWithOnMainThread(task =>
        //{
        //    if (task.IsCompleted)
        //    {
        //        Debug.Log("User added successfully.");
        //    }
        //    else
        //    {
        //        Debug.LogError("Error adding user: " + task.Exception);
        //    }
        //});
    }
}

[Serializable]
public class StepData
{
    public string content_url;
    public DesicionQuestion desicionQuestion;
    public string location;
    public string narriation;
    public StepData nextStep;
    public StepData previousStep;
    public string quizId;

}

[Serializable]
public class DesicionQuestion
{
    public string question;
    public string answer1;
    public string answer2;
    public StepData answer1_stepData;
    public StepData answer2_stepData;

}