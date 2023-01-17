using System.IO;
using System.Linq;
using System.Xml.Serialization;
using JetBrains.Annotations;
using UnityEngine;
using XNode;

/// <summary>
/// Graph for creating progression flow
/// </summary>
/// TODO System to keep ProgressionTag names unique
[CreateAssetMenu(menuName = "ProjectU/Progression Graph")]
public class ProgressionGraph : NodeGraph
{
    [SerializeField]
    private SerializedGUID guid = SerializedGUID.Generate();

    public void SaveCurrentState([NotNull] string directoryPath)
    {
        var data = from node in nodes.OfType<TagNode>().Where(tag => !string.IsNullOrEmpty(tag.Name))
            select (node.Name.ToString(), node.flags);

        var xml = new XmlSerializer(typeof((string, TagNode.Flags)[]));

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        var filepath = directoryPath + "/ProgressionGraph_" + guid + ".data";

        var writer = new StreamWriter(filepath);

        xml.Serialize(writer, data.ToArray());

        writer.Close();
    }

    public void LoadState([NotNull] string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
            return;

        var filepath = directoryPath + "/ProgressionGraph_" + guid + ".data";

        if (!File.Exists(filepath))
            return;


        var xml = new XmlSerializer(typeof((string, TagNode.Flags)[]));

        var fileStream = new FileStream(filepath, FileMode.Open);

        var data = ((string, TagNode.Flags)[])xml.Deserialize(fileStream);

        fileStream.Close();


        var tagDictionary = nodes.OfType<TagNode>().Where(tag => !string.IsNullOrEmpty(tag.Name)).ToDictionary(tag => tag.Name);

        foreach (var (tagName, flags) in data)
            tagDictionary[tagName].flags = flags;
    }
}