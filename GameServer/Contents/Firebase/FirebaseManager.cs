using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using System;
using System.Threading.Tasks;

public partial class FirebaseManager : TSingleton<FirebaseManager>
{
    private FirebaseAuth m_firebase_auth = null;

    public void Initialize()
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + @"projectd-2c989-firebase-adminsdk-wki0o-2cd2e9d8ca.json";
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(path)
        });

        m_firebase_auth = FirebaseAuth.DefaultInstance;
    }

    public async Task<string> CreateAuthToken(string in_account_id)
    {
        var new_token = await m_firebase_auth.CreateCustomTokenAsync(in_account_id);

        return new_token;
    }

        
}