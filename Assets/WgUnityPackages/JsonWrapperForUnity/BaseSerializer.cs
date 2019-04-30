using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace WGUnityPackages.JsonWrapperForUnity
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
        private static string GetPath<T> ( 
            string aditionalPath, 
            string fileUniqueNamePart )
        {
                return RelativeDirectoryPath ( aditionalPath ) + 
                typeof ( T ) + "_" + fileUniqueNamePart + ".json";
        }

        private static string RelativeDirectoryPath ( string aditionalPath )
        {
            var ap = Application.dataPath;
            return  ap + "/" + aditionalPath+"/";
        }

        //TODO : Fix this
        private static bool TryToMakePathDirectory ( 
            string path, 
            out string createdPath )
        {
           
                createdPath = Directory.CreateDirectory ( path ).ToString();
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
        public static string SaveToJson<T> ( 
            T objectToSerialize, 
            string fileUniqueNamePart, 
            string aditionalPathPart = "", 
            bool overwrite = false )
        {
            string directoryPath = RelativeDirectoryPath ( aditionalPathPart );
            string filePath = GetPath<T> ( aditionalPathPart, fileUniqueNamePart );

            if ( File.Exists ( filePath ) && !overwrite )
                throw new System.FormatException (
                     @"Cannot save file to this location 
                     because File already Exists! Use Overwrite parameter!" );
            bool de = Directory.Exists ( directoryPath );
            if ( !de &
            !TryToMakePathDirectory ( directoryPath, out directoryPath ) )
                throw new System.FormatException(@"Cannot create directory at path " + directoryPath );

            string json = JsonConvert.SerializeObject ( objectToSerialize, SERIALIZER_SETTINGS );
            
            using(var stream = new FileStream ( filePath, FileMode.Create ))
            {
                byte[] info = new UTF8Encoding ( true ).GetBytes ( json );
                stream.Write ( info, 0, info.Length );
                stream.Close();
            }
#if UNITY_EDITOR
            AssetDatabase.Refresh ();
#endif
            return filePath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static object LoadFromJson( string path )
        {
            if ( string.IsNullOrEmpty ( path ))
            {
                Debug.LogWarning(string.Format("Path {0} is empty", path));
                return null;
            }
            if ( !File.Exists ( path ) )
            {
                Debug.LogWarning(string.Format ("Path {0} is not a file.", path));
                return null;
            }    

            using(var stream  = new FileStream ( path, FileMode.Open ))
            {
                TextReader tr = new StreamReader ( stream );
                var deserializedProduct = JsonConvert.DeserializeObject ( tr.ReadToEnd (), SERIALIZER_SETTINGS );
                stream.Close ();
                tr.Close ();
                return deserializedProduct;
            }
        }
    }
}