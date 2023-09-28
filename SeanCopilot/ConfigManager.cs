using Kbg.NppPluginNET;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ConfigManager
{
    private Dictionary<string, string> _configData;
    private string _configFilePath;

    private static Dictionary<string, string> PlaceholderMapping = new Dictionary<string, string>()
    { 
	    {"plugin_name", Main.PluginName},
    };

    public ConfigManager(string configFilePath)
    {
        _configData = new Dictionary<string, string>();
        _configFilePath = configFilePath;

        LoadConfig();
    }

    public void LoadConfig()
    {
        _configData = File.ReadAllLines(_configFilePath)
                     .Select(l => l.Split('='))
                     .ToDictionary(p => p[0].Trim(), p => p[1].Trim());
    }

    public string GetConfigValue(string configKey, bool placeholders = true)
    {
        if (_configData.ContainsKey(configKey))
        {
            string processedValue = _configData[configKey];
            if (placeholders && processedValue.Contains("<"))
            {
                foreach (var mapping in PlaceholderMapping)
                {
                    processedValue = processedValue.Replace($"<{mapping.Key}>", mapping.Value);
                }
            }
            return processedValue;
        }
        else
        {
            return "not found"; // or throw an exception
        }
    }

    public void SetConfigValue(string configKey, string configValue)
    {
        _configData[configKey] = configValue;
        File.WriteAllLines(_configFilePath, _configData.Select(kv => $"{kv.Key}={kv.Value}"));
    }
}