using System.IO;

public static class JSONExportImporter
{
    public static void ExportToJson<T>(string path, T serializedObject)
    {
        string json = UnityEngine.JsonUtility.ToJson(serializedObject);

        using (StreamWriter sw = new StreamWriter(path))
        {
            sw.Write(json);
        }
    }

    public static T ImportFromJson<T>(string path) where T : new ()
    {
        T serializedObject = new T();
        using (StreamReader sr = new StreamReader(path))
        {
            string json = sr.ReadToEnd();
            UnityEngine.JsonUtility.FromJsonOverwrite(json, serializedObject);
        }
        return serializedObject;
    }
}
