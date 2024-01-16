using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;

namespace DataBase
{
    public partial class DataBaseManager : TSingleton<DataBaseManager>
    {
        private FirestoreDb m_firestore_DB;

        public void Initialize()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"projectd-2c989-firebase-adminsdk-wki0o-2cd2e9d8ca.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            m_firestore_DB = FirestoreDb.Create("projectd-2c989");
        }

        public void UpdateDataBase(string in_collection_name, string in_doc_name, Dictionary<string, object> in_data)
        {
            DocumentReference doc_ref = m_firestore_DB.Collection(in_collection_name).Document(in_doc_name);
            doc_ref.SetAsync(in_data);
        }

        public DocumentSnapshot GetSnapshot(string in_collection_name, string in_doc_name)
        {
            if (m_firestore_DB == null)
                return null;

            DocumentReference doc_ref = m_firestore_DB.Collection(in_collection_name).Document(in_doc_name);
            return doc_ref.GetSnapshotAsync().Result;
        }
    }
}