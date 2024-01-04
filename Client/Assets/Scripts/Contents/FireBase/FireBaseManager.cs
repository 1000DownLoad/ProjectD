using UnityEngine;
using Framework;
using Firebase;
using Firebase.Auth;
using Google;

public class FirebaseManager : TSingleton<FirebaseManager>
{
    FirebaseManager() { }

    public string webClientId = "445256307725-t5a1urc0feqvo2bqv29gkg79pu5anu5k.apps.googleusercontent.com";

    public FirebaseApp  m_firebase_app;
    public FirebaseAuth m_firebase_auth;
    public GoogleSignInConfiguration m_google_configuration;

    protected override void OnCreateSingleton()
    {

    }

    public void InitFirebase()
    {
        m_google_configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                m_firebase_app = FirebaseApp.DefaultInstance;
                m_firebase_auth = FirebaseAuth.DefaultInstance;
            }
        });
    }

    public void CreateUserWithEmail(string in_email, string in_password)
    {
        if (m_firebase_auth == null)
            return;

        m_firebase_auth.CreateUserWithEmailAndPasswordAsync(in_email, in_password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
                return;
        });
    }

    public void LoginWithEmail(string in_email, string in_password)
    {
        if (m_firebase_auth == null)
            return;

        m_firebase_auth.SignInWithEmailAndPasswordAsync(in_email, in_password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
                return;
        });
    }

    public void LoginAnonymous()
    {
        if (m_firebase_auth == null)
            return;

        m_firebase_auth.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
                return;
        });
    }

    public void LoginWithGoogle()
    {
        if (m_firebase_auth == null)
            return;

        if (m_google_configuration == null)
            return;

        GoogleSignIn.Configuration = m_google_configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
                return;

            LoginWithGoogleOnFirebase(task.Result.IdToken);
        });
    }

    private void LoginWithGoogleOnFirebase(string in_idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(in_idToken, null);

        m_firebase_auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
                return;
        });
    }
}