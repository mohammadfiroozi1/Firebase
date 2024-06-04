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
                // ReadData();  // Read data after initialization
                AddUser();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {task.Result}");
                isFirebaseInitialized = false;
            }
        });
    }
    async void ReadData()
    {
        if (!isFirebaseInitialized)
        {
            Debug.LogError("Firebase is not initialized.");
            return;
        }

        // Specify the collection path to retrieve data from
        CollectionReference stepsRef = db.Collection("steps");

        // Fetch data as a query snapshot
        QuerySnapshot querySnapshot = await stepsRef.GetSnapshotAsync();

        // Process each document in the snapshot
        foreach (DocumentSnapshot document in querySnapshot.Documents)
        {
            // Get the document ID for reference
            string documentId = document.Id;

            // Check if the document exists
            if (document.Exists)
            {
                // Convert the document data to a dictionary
                Dictionary<string, object> data = document.ToDictionary();

                // Deserialize the data into your serializable object
                StepData stepData = JsonConvert.DeserializeObject<StepData>(JsonConvert.SerializeObject(data));

                // Now you have the stepData object with the retrieved data
                // Use stepData.content_url, stepData.desicionQuestion, etc.

                Debug.Log($"Retrieved data for document ID: {documentId}");
                Debug.Log($"Content URL: {stepData.content_url}");
                Debug.Log("questionDesicion id :" + stepData.desicionQuestionID);
                // ... (Log other properties as needed)
            }
            else
            {
                Debug.Log($"Document {documentId} does not exist.");
            }
        }
    }

    //void ReadData()
    //{
    //    if (!isFirebaseInitialized)
    //    {
    //        Debug.LogError("Firebase is not initialized.");
    //        return;
    //    }

    //    CollectionReference usersRef = db.Collection("steps");

    //    usersRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
    //    {
    //        if (task.IsCompleted)
    //        {
    //            QuerySnapshot snapshot = task.Result;
    //            foreach (DocumentSnapshot document in snapshot.Documents)
    //            {
    //                // Get the document data as a dictionary
    //                Dictionary<string, object> documentData = document.ToDictionary();

    //                // Custom conversion for StepData with nested DesicionQuestion
    //                StepData stepData = ConvertDocumentToStepData(documentData);

    //                if (stepData != null)
    //                {
    //                    // Log or use the stepData object here
    //                    Debug.Log("Document ID: " + document.Id);
    //                    Debug.Log("content_url: " + stepData.content_url);
    //                    Debug.Log("desicionQuestion:" + stepData.desicionQuestion.question);
    //                    // ... (log other properties)
    //                }
    //                else
    //                {
    //                    Debug.LogError("Failed to convert document data to StepData object.");
    //                }
    //            }
    //        }
    //        else
    //        {
    //            Debug.LogError("Error getting documents: + task.Exception");
    //        }
    //    });
    //}
    //private StepData ConvertDocumentToStepData(Dictionary<string, object> documentData)
    //{
    //    if (documentData == null)
    //    {
    //        return null;
    //    }

    //    StepData stepData = new StepData();

    //    try
    //    {
    //        stepData.content_url = documentData["content_url"] as string;
    //        stepData.location = documentData["location"] as string;
    //        stepData.narriation = documentData["narriation"] as string;
    //        stepData.nextStep = null; // Handle as needed
    //        stepData.previousStep = null; // Handle as needed
    //        stepData.quizId = documentData["quizId"] as string;

    //        // Handle nested DesicionQuestion
    //        if (documentData.ContainsKey("desicionQuestion"))
    //        {
    //            Dictionary<string, object> desicionQuestionData = documentData["desicionQuestion"] as Dictionary<string, object>;
    //            stepData.desicionQuestion = new DesicionQuestion()
    //            {
    //                question = desicionQuestionData["question"] as string,
    //                answer1 = desicionQuestionData["answer1"] as string,
    //                answer2 = desicionQuestionData["answer2"] as string,
    //                // Handle nested answer1_stepData and answer2_stepData as needed (might require recursion)
    //                answer1_stepData = ConvertDocumentToStepDataToNested(desicionQuestionData, "answer1_stepData"),
    //                answer2_stepData = ConvertDocumentToStepDataToNested(desicionQuestionData, "answer2_stepData"),
    //            };
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogError("Error converting document data to StepData: " + ex.Message);
    //        return null;
    //    }

    //    return stepData;
    //}
    //private StepData ConvertDocumentToStepDataToNested(Dictionary<string, object> data, string key)
    //{
    //    if (!data.ContainsKey(key))
    //    {
    //        return null;
    //    }

    //    Dictionary<string, object> nestedData = data[key] as Dictionary<string, object>;
    //    return ConvertDocumentToStepData(nestedData);
    //}
    void AddUser()
    {
        if (!isFirebaseInitialized)
        {
            Debug.LogError("Firebase is not initialized.");
            return;
        }

        StepData stepBro = new StepData()
        {
            content_url = "previous url",
            desicionQuestionID = "8O18tDN7cYulfqzcgizZ",

            location = "edare jandarmeri",
            narriation = "nafas zaban",
            nextStepID = "",
            previousStepID = "",
            quizId = "4"
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
    public string desicionQuestionID;
    public string location;
    public string narriation;
    public string nextStepID;
    public string previousStepID;
    public string quizId;

}

[Serializable]
public class DesicionQuestion
{
    public string question;
    public string answer1;
    public string answer2;
    public string answer1_stepDataID;
    public string answer2_stepDataID;

}