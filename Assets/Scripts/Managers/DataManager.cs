using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; } // Singleton

    private string saveDirectory;
    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Kaydetme dizinini belirle
            saveDirectory = Path.Combine(Application.persistentDataPath, "saves");
            saveFilePath = Path.Combine(saveDirectory, "game_data.json");

            // Dizini olu�tur
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Veriyi JSON dosyas�na kaydet
    public void SaveData(GameData data)
    {
        string json = JsonUtility.ToJson(data, true); // JSON format�na �evir
        File.WriteAllText(saveFilePath, json); // Dosyaya yaz
        Debug.Log("Veri kaydedildi: " + saveFilePath);
    }

    // JSON dosyas�ndan veriyi y�kle
    public GameData LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath); // Dosyadan oku
            GameData data = JsonUtility.FromJson<GameData>(json); // JSON'u obje olarak geri y�kle
            Debug.Log("Veri y�klendi: " + saveFilePath);

            return data;
        }
        else
        {
            Debug.LogWarning("Kay�t dosyas� bulunamad�, varsay�lan veri d�nd�r�ld�.");
            return new GameData(); // Varsay�lan bir obje d�nd�r
        }
    }
}
    