using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

namespace Firewrap
{
    public class Firestore
    {
        /// <summary>
        /// The Default Firebase Firestore Instance used to provide access to the game data servers.
        /// </summary>
        public static FirebaseFirestore FirestoreDbInstance = FirebaseFirestore.DefaultInstance;

        /// <summary>
        /// Asynchronously gets the contents of a Document, then converts it to the given type.
        /// </summary>
        /// <typeparam name="T">The custom class Type to which the document will be converted. (Generally these classes share the same name with the Collection to which the Document is filed under.)</typeparam>
        /// <param name="pathFragments">The path to the requested document. Can be either a full string or multiple segments passed as separate parameters; these will be resolved automatically</param>
        /// <returns></returns>
        public static async Task<T> GetDocumentAsync<T>(params string[] pathFragments)
        {
            string documentPath = BuildFirestoreSafePath(pathFragments);

            IsValidDocumentPath_Dirty(documentPath);

            DocumentReference documentRef = FirestoreDbInstance.Document(documentPath);
            DocumentSnapshot documentSnap = await documentRef.GetSnapshotAsync();
            //Debug.Log(JsonUtility.ToJson(documentSnap.ToDictionary()));
            T documentData = documentSnap.ConvertTo<T>();
            return documentData;
        }

        /// <summary>
        /// Asynchronously sets the contents of a Document, overwriting its original contents.
        /// </summary>
        /// <param name="documentPath">The full path to the Document.</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task SaveDocumentDataAsync(string documentPath, object data)
        {
            IsValidDocumentPath_Dirty(documentPath);

            DocumentReference documentRef = FirestoreDbInstance.Document(documentPath);
            await documentRef.SetAsync(data);
        }

        public static string BuildFirestoreSafePath(params string[] pathFragments)
        {
            string firestoreSafePath = string.Join("/", pathFragments).Trim().TrimEnd('/');
            return firestoreSafePath;
        }

        private static bool IsValidDocumentPath_Dirty(string documentPath)
        {
            string[] pathFragments = documentPath.Split('/');
            int pathLength = pathFragments.Length;
            bool isValid = false;
            // Documents always make even length paths:
            // collection/document/collection/document
            if (pathLength % 2 == 0)
            {
                isValid = true;
            }
            if (!isValid)
            {
                throw new ArgumentException("Invalid document path. Document paths must have an even number of segments.", nameof(documentPath));
            }
            return true;
        }
    }
}