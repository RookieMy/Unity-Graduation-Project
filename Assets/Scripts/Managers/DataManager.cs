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

            // Dizini oluþtur
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

    // Veriyi JSON dosyasýna kaydet
    public void SaveData(GameData data)
    {
        string json = JsonUtility.ToJson(data, true); // JSON formatýna çevir
        File.WriteAllText(saveFilePath, json); // Dosyaya yaz
        Debug.Log("Veri kaydedildi: " + saveFilePath);
    }

    // JSON dosyasýndan veriyi yükle
    public GameData LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath); // Dosyadan oku
            GameData data = JsonUtility.FromJson<GameData>(json); // JSON'u obje olarak geri yükle
            Debug.Log("Veri yüklendi: " + saveFilePath);

            return data;
        }
        else
        {
            Debug.LogWarning("Kayýt dosyasý bulunamadý, varsayýlan veri döndürüldü.");
            return new GameData(); // Varsayýlan bir obje döndür
        }
    }
}
    