namespace HexaSort.Storage
{
    public interface ISaveDataStorage
    {
        int GetInt(string key);
        float GetFloat(string key);
        string GetString(string key);
        void SetInt(string key, int value);
        void SetFloat(string key, float value);
        void SetString(string key, string value);
    }
}