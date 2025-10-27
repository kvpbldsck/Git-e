using YamlDotNet.Serialization;

namespace GitE.Models;

[YamlSerializable]
public sealed class UserSettings
{
    public List<Project> Projects { get; set; }

    public void Deconstruct(out List<Project> Projects)
    {
        Projects = this.Projects;
    }
}
