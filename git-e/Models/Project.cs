using YamlDotNet.Serialization;

namespace GitE.Models;

[YamlSerializable]
public sealed class Project
{
    public string Name { get; set; }
    public string Path { get; set; }

    public void Deconstruct(out string Name, out string Path)
    {
        Name = this.Name;
        Path = this.Path;
    }
}
