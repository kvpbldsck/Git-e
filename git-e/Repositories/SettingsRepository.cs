using GitE.Models;

using YamlDotNet.Serialization;

namespace GitE.Repositories;

public sealed class SettingsRepository(
    IDeserializer deserializer,
    ISerializer serializer)
{
    public UserSettings Load()
    {
        var settingsFilePath = GetFilePath();
        if (!File.Exists(settingsFilePath))
        {
            return new()
            {
                Projects = []
            };
        }

        var content = File.ReadAllText(settingsFilePath);
        UserSettings settings = deserializer.Deserialize<UserSettings>(content);

        return settings;
    }

    public void Save(UserSettings settings)
    {
        EnsureFolderExists();

        var settingsFilePath = GetFilePath();
        var content = serializer.Serialize(settings);
        File.WriteAllText(settingsFilePath, content);
    }

    private static void EnsureFolderExists()
    {
        var folderPath = GetFolderPath();
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    private static string GetFilePath() =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            nameof(GitE),
            "settings.yml");

    private static string GetFolderPath() =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            nameof(GitE));
}
