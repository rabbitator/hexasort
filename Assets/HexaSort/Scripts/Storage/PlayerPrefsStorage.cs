using UnityEngine;

namespace HexaSort.Storage
{
    public class PlayerPrefsStorage : ISaveDataStorage
    {
        public int GetInt(string key) => PlayerPrefs.GetInt(key);

        public float GetFloat(string key) => PlayerPrefs.GetFloat(key);

        public string GetString(string key) => PlayerPrefs.GetString(key);

        public void SetInt(string key, int value) => PlayerPrefs.SetInt(key, value);

        public void SetFloat(string key, float value) => PlayerPrefs.SetFloat(key, value);

        public void SetString(string key, string value) => PlayerPrefs.SetString(key, value);
    }
}