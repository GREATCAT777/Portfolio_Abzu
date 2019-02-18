using System.IO;
using UnityEngine;

using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class GameSettings
{
    readonly public string SavePath = "/gamesettings.json";

    public int renderResIndex = 0;
    public int fishQuality = 0;
    public int shadows = 0;
    public int reflections = 0;
    public int vSync = 0;
    public bool fullscreen = true;
    public string language = "English";
    public bool swimPitch = false;
    public bool cameraPitch = false;
    public bool cameraYaw = true;
        
    public void SaveOption(GameSettings gameSettings)
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
    }

    public GameSettings LoadOption()
    {
        GameSettings gameSettings = new GameSettings();
        try
        {
            gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
        }
        catch
        {
            if (gameSettings == null)
            {
                gameSettings = new GameSettings();
            }
        }

        return gameSettings;
    }
}
