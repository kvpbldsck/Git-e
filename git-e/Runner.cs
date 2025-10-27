using GitE.Models;
using GitE.Repositories;

using YamlDotNet.Serialization;

namespace GitE;

public sealed class Runner(
    SettingsRepository settingsRepository,
    ISerializer serializer)
{
    public void Run()
    {
        var project = new Project
        {
            Name = "gite",
            Path = @"D:\projects\personal\git-e",
        };
        var settings = new UserSettings
        {
            Projects = [ project ],
        };

        settingsRepository.Save(settings);
        UserSettings loadedSettings = settingsRepository.Load();
        Console.WriteLine(serializer.Serialize(loadedSettings));
    }
}
