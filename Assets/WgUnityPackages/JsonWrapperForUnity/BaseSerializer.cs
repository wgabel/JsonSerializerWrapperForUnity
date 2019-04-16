using Newtonsoft.Json;
using System.IO;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace WGPackages.JsonWrapperForUnity
{
    /// <summary>
    /// Basic Json Serializer
    /// </summary>
    public static class BaseSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly JsonSerializerSettings SERIALIZER_SETTINGS =
            new JsonSerializerSettings () { TypeNameHandling = TypeNameHandling.All };

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileUniqueNamePart"></param>
        /// <param name="pathRoot"></param>
        /// <returns></returns>
        private static string GetPath<T> ( string fileUniqueNamePart, string pathRoot = "" )
        {
            return GetDirectoryPath ( pathRoot ) + Path.DirectorySeparatorChar + typeof ( T ) + "_" + fileUniqueNamePart + ".json";
        }
           
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathRoot"></param>
        /// <returns></returns>
        private static string GetDirectoryPath ( string pathRoot )
        {
            return Application.dataPath + Path.DirectorySeparatorChar + pathRoot;
        }

        //TODO : Fix this
        private static bool TryToMakePath ( string pathRoot )
        {
            var newDir = Directory.CreateDirectory ( pathRoot );
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToSerialize"></param>
        /// <param name="fileUniqueNamePart"></param>
        /// <param name="pathRoot"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static string SaveToJson<T> ( T objectToSerialize, string fileUniqueNamePart, string pathRoot = "", bool overwrite = false )
        {
            string path = GetPath<T> ( fileUniqueNamePart, pathRoot );
            if ( File.Exists ( path ) && !overwrite )
                throw new System.FormatException ( "File Exists! Use Overwrite parameter!" );

            TryToMakePath ( GetDirectoryPath ( pathRoot ) );

            if ( !Directory.Exists ( GetDirectoryPath ( pathRoot ) ) )
                throw new System.FormatException ( "Directory does not exist!"+ GetDirectoryPath ( pathRoot ) );

            string json = JsonConvert.SerializeObject ( objectToSerialize, SERIALIZER_SETTINGS );
            // TODO : Use using
            var stream = new FileStream ( path, FileMode.Create );
            byte[] info = new UTF8Encoding ( true ).GetBytes ( json );
            stream.Write ( info, 0, info.Length );
            stream.Close ();
#if UNITY_EDITOR
            AssetDatabase.Refresh ();
#endif
            return path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static object LoadFromJson ( string path )
        {
            if ( string.IsNullOrEmpty ( path ) )
                throw new System.FormatException ( "Path is Empty!" );

            if ( !File.Exists ( path ) )
                throw new System.FormatException ( "Path is not a file!" );
            //TODO : Use using
            var stream = new FileStream ( path, FileMode.Open );
            TextReader tr = new StreamReader ( stream );
            var deserializedProduct = JsonConvert.DeserializeObject ( tr.ReadToEnd (), SERIALIZER_SETTINGS );
            stream.Close ();
            tr.Close ();
            return deserializedProduct;
        }
    }
}