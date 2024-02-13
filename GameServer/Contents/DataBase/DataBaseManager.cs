using System;
using System.Collections.Generic;
using Google.Cloud.Firestore;
using System.Linq;
using System.IO;

namespace DataBase
{
    public partial class DatabaseManager : TSingleton<DatabaseManager>
    {
        private FirestoreDb m_firestore_DB;

        public void Initialize()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../", "projectd-2c989-firebase-adminsdk-wki0o-2cd2e9d8ca.json");
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            m_firestore_DB = FirestoreDb.Create("projectd-2c989");
        }

        public void UpdateField(string in_collection_name, string in_doc_name, Dictionary<string, object> in_data)
        {
            if (m_firestore_DB == null)
                return;

            DocumentReference doc_ref = m_firestore_DB.Collection(in_collection_name).Document(in_doc_name);
            doc_ref.SetAsync(in_data);
        }

        public void UpdateField(string in_collection_name, string in_doc_name, string in_field_name, object in_value)
        {
            if (m_firestore_DB == null)
                return;

            DocumentReference doc_ref = m_firestore_DB.Collection(in_collection_name).Document(in_doc_name);
            doc_ref.UpdateAsync(in_field_name, in_value);
        }

        public List<DocumentSnapshot> GetCollectionData(string in_collection_name)
        {
            if (m_firestore_DB == null)
                return null;

            var collection_ref = m_firestore_DB.Collection(in_collection_name);
            if (collection_ref == null)
                return null;

            List<DocumentSnapshot> collection_data = new List<DocumentSnapshot>();
            var snap_shot = collection_ref.GetSnapshotAsync().Result;

            foreach (var document in snap_shot.Documents)
            {
                collection_data.Add(document);
            }

            return collection_data;
        }

        public Dictionary<string, object> GetDocumentData(string in_collection_name, string in_doc_name)
        {
            if (m_firestore_DB == null)
                return null;

            DocumentReference doc_ref = m_firestore_DB.Collection(in_collection_name).Document(in_doc_name);
            if (doc_ref == null)
                return null;

            var snap_shot = doc_ref.GetSnapshotAsync().Result;
            if (snap_shot.Exists)
                return snap_shot.ToDictionary();

            return null;
        }

        public void ReadDocumentsByCondition(string in_collection_name, string in_field, string in_op, object in_value, Action<Dictionary<string, object>> in_action)
        {
            if (m_firestore_DB == null)
                return;

            CollectionReference collection_ref = m_firestore_DB.Collection(in_collection_name);
            if (collection_ref == null)
                return;

            Query query = null;

            switch (in_op)
            {
                case "=" : query = collection_ref.WhereEqualTo(in_field, in_value);                 break;
                case "<" : query = collection_ref.WhereGreaterThan(in_field, in_value);             break;
                case "<=": query = collection_ref.WhereGreaterThanOrEqualTo(in_field, in_value);    break;
                case ">" : query = collection_ref.WhereLessThan(in_field, in_value);                break;
                case ">=": query = collection_ref.WhereLessThanOrEqualTo(in_field, in_value);       break;
            }

            if (query == null)
                return;

            query.GetSnapshotAsync().ContinueWith(in_task =>
            {
                QuerySnapshot snap_shot = in_task.Result;

                foreach (DocumentSnapshot document in snap_shot.Documents)
                {
                    if (document.Exists)
                    {
                        var field = document.ToDictionary();

                        if(in_action != null)
                            in_action(field);
                    }
                }
            });
        }
    }
}