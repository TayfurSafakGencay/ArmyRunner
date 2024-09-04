using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace Managers
{
  public class SaveManager : MonoBehaviour
  {
    private static string _applicationDataPath;
    private void Awake()
    {
#if UNITY_STANDALONE_WIN || UNITY_64
    _applicationDataPath = Application.dataPath;
#endif
#if UNITY_ANDROID
      _applicationDataPath = Application.persistentDataPath;
#endif
    }

    public static void SaveJson<T>(T obj, string path)
    {
      string jsonString = JsonUtility.ToJson(obj);
      File.WriteAllText(_applicationDataPath + "/Resources" + path + ".json", jsonString);
      Debug.Log("JSON saved at: " + Application.dataPath + "/Resources" + path + ".json");
    }

    // Load JSON from the Resources folder
    public static T LoadJson<T>(string resourcePath)
    {
      TextAsset jsonFile = Resources.Load<TextAsset>(resourcePath);

      if (jsonFile != null)
      {
        return JsonConvert.DeserializeObject<T>(jsonFile.text);
      }

      throw new FileNotFoundException("File not found: " + resourcePath);
    }
  }

// Use constants to define resource paths
  public struct JsonKey
  {
    public const string GunData = "Data/Gun/GunData";
  }
}