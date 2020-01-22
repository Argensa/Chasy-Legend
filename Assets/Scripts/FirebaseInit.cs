using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;
using Firebase.Storage;
public class FirebaseInit : MonoBehaviour
{


    public string authCode;
    //static FirebaseStorage DefaultInstance;     
    private void Awake()
    {
        
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestServerAuthCode(false /* Don't force refresh */)
            .Build();



        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        // Get a reference to the storage service, using the default Firebase App
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        Firebase.Storage.StorageReference storage_ref = storage.GetReferenceFromUrl("gs://chasy-legend.appspot.com");
        // Create a child reference
        // images_ref now points to "unlockedCars"
        Firebase.Storage.StorageReference unlockedCars_ref = storage_ref.Child("unlockedCars");
    }

    private void Start()
    {
        Social.localUser.Authenticate((bool success) => {
            // handle success or failure
        });


        Social.localUser.Authenticate((bool success) => {
            if (success)
            {
                authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
            }
        });


        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.Credential credential =
            Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });


        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            string playerName = user.DisplayName;

            // The user's Id, unique to the Firebase project.
            // Do NOT use this value to authenticate with your backend server, if you
            // have one; use User.TokenAsync() instead.
            string uid = user.UserId;
            
        }
        

    }
}
