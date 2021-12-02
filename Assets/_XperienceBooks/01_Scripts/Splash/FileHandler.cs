using System;
using System.IO;
using UnityEngine;
using System.IO.Compression;
using Intellify.core;

public class FileHandler
{
    //static string fileBasePath = Application.persistentDataPath;

    private void Awake()
    {
       
       // m_FilePath = Application.persistentDataPath;
    }

    public static void ExtractFiles(string zipPath, string FinalPath, bool sameDirectory = false)
    {
        FileInfo m_ZipFileInfo = new FileInfo(zipPath);

        if (sameDirectory)
        {
            FileInfo m_FileInfo = new FileInfo(zipPath);

            //Create directory of zip file name
            var extractDir = Path.Combine(m_FileInfo.Directory.FullName, m_FileInfo.Name.Substring(0, m_FileInfo.Name.Length - m_FileInfo.Extension.Length));
            if (!Directory.Exists(extractDir))
                Directory.CreateDirectory(extractDir);

            FinalPath = extractDir;
        }
        Debug.Log("Extract Zip at: " + FinalPath);
        //Extract zip file on path 
        ZipFile.ExtractToDirectory(m_ZipFileInfo.FullName, FinalPath);
    }


    public static void SaveFile(string path, string fileName, byte[] data , bool needToExtractFile = false) {

        string m_FolderStruct = GameManager.Instance.LocalStoragePath  + path;
        string m_FileName = fileName;
        string m_FinalPath = m_FolderStruct + m_FileName;

        Debug.Log("Folder Structure : " + path);

        ValidateFolderStructure(path);
        try
        {
           // Debug.Log("Before data save , File Path : - "+m_FinalPath);

            //write file on path 
            File.WriteAllBytes(m_FinalPath, data);

            if (needToExtractFile)
            {

                //Get the file name with full path
                FileInfo m_FileInfo = new FileInfo(m_FinalPath);

                //Create directory of zip file name
                var extractDir = Path.Combine(m_FileInfo.Directory.FullName, m_FileInfo.Name.Substring(0, m_FileInfo.Name.Length - m_FileInfo.Extension.Length));
                    if (!Directory.Exists(extractDir)) Directory.CreateDirectory(extractDir);

                //Extract zip file on path 
                ZipFile.ExtractToDirectory(m_FileInfo.FullName, extractDir);
              
            }

            Debug.Log($"{fileName} Save at : " + m_FinalPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Save Data at: " + m_FinalPath.Replace("/", "\\"));
            Debug.LogError("Error: " + e.Message);
        }
    }

    public static void ValidateFolderStructure(string path) {

        //Debug.Log("Before create folder Path : "+path);

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(GameManager.Instance.LocalStoragePath + path)))
        {
            Debug.Log("Folder Not Exist , created new one at : " + GameManager.Instance.LocalStoragePath + path);
            Directory.CreateDirectory(Path.GetDirectoryName(GameManager.Instance.LocalStoragePath + path));
        }
    }


     public static bool ValidateFile(string path) {

        if (File.Exists(GameManager.Instance.LocalStoragePath + path))
        {
            Debug.Log("File Availble");
            return true;
        }
        else {
            Debug.Log("File Not Exit");
            return false;
        }
    }


    public static string FinalPath(string path , string filename) {

        var finalPath = GameManager.Instance.LocalStoragePath + path + filename;
        return finalPath;
    }


    public static void SaveUserData(UserData user) {
        File.WriteAllText(GameManager.Instance.LocalStoragePath + "/UserData.json", JsonUtility.ToJson(user));
    }

    public static void SaveBooksData(SeriesBooks books)
    {
        Debug.Log("FileHandler SaveBooksData");
        File.WriteAllText(GameManager.Instance.LocalStoragePath + "Theme/BooksData.json", JsonUtility.ToJson(books));
    }

    public static void SaveSeriesData(Series series)
    {
        File.WriteAllText(GameManager.Instance.LocalStoragePath + "Theme/SeriesData.json", JsonUtility.ToJson(series));
    }

    public static UserData GetUser() {

         UserData data = new UserData();
        try
        {
            data = JsonUtility.FromJson<UserData>(File.ReadAllText(GameManager.Instance.LocalStoragePath + StaticKeywords.UserData));
        }
        catch (Exception ex) {
            Debug.Log("File Not read : " + ex.Message);
        }
        return data;
    }

    public static SeriesBooks GetSeriesBook()
    {

        SeriesBooks data = new SeriesBooks();
        try
        {
            data = JsonUtility.FromJson<SeriesBooks>(File.ReadAllText(GameManager.Instance.LocalStoragePath + "Theme/BooksData.json"));
        }
        catch (Exception ex)
        {
            Debug.Log("File Not read : " + ex.Message);
        }
        return data;
    }

    public static Series GetSeries()
    {

        Series data = new Series();
        try
        {
            data = JsonUtility.FromJson<Series>(File.ReadAllText(GameManager.Instance.LocalStoragePath + "Theme/SeriesData.json"));
        }
        catch (Exception ex)
        {
            Debug.Log("File Not read : " + ex.Message);
        }
        return data;
    }
}


public enum FileType {

    png,
    zip,
    json

}