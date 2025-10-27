using GitE.Models;

using YamlDotNet.Serialization;

namespace GitE.Utils;

[YamlStaticContext]
[YamlSerializable(typeof(Project))]
[YamlSerializable(typeof(UserSettings))]
public sealed partial class YamlStaticContext;
