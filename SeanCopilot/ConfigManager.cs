using Kbg.NppPluginNET;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ConfigManager
{
    private Dictionary<string, string> _configData;
    private string _configFilePath;

    private static string DefaultConfig()
    {
        string config = "instructions_path=plugins\\<plugin_name>\\instructions.ini\r\n";
        config += "api_key=your_api_key\r\n";
        config += "gpt_model=gpt-3.5-turbo";

        return config;
    }

    public static string DefaultInstructions()
    {
        return "You are a code assitant, being used as a notpad++ plugin who assists with the writing and analysis of pieces of code."
            + "You must use the provided funciton to return code to the user.If you are provided code in the prompt and asked to refactor "
            + "or rewrite the code in some way, you should always provide the full code in your response with the changes made to it. "
            + "The code you provide using the function will fully replace the user's provided code in their codebase, so code you provide "
            + "must be able to fully replace the provided code in the user's document without introducing bugs by having missing or truncated "
            + "sections.If the prompt contains only code, then the user is most likely just looking for an explanation or summary of the code. "
            + "If you do not choose to use the provided function, you must separate the(optional) code and(mandatory) explanation sections of your "
            + "response like so:\r\ncode: colour = new Color('Red');\r\nexplanation: Changed the colour to red.";
    }

    private static Dictionary<string, string> PlaceholderMapping = new Dictionary<string, string>()
    { 
	    {"plugin_name", Main.PluginName},
    };

    public ConfigManager(string configFilePath)
    {
        _configData = new Dictionary<string, string>();
        _configFilePath = configFilePath;

        if (!File.Exists(_configFilePath))
        {
            // Create config file
            File.WriteAllText(_configFilePath, DefaultConfig());
        }

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