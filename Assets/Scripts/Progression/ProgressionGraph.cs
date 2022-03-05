using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using XNode;

/// <summary>
/// Graph for creating progression flow
/// </summary>
/// TODO System to keep ProgressionTag names unique
[CreateAssetMenu(menuName = "ProjectU/Progression Graph")]
public class ProgressionGraph : NodeGraph
{
    public void SaveCurrentState(string filepath)
    {
        var data = from node in nodes.OfType<TagNode>().Where(tag => !string.IsNullOrEmpty(tag.Name))
            select (node.Name.ToString(), node.flags);

        var xml = new XmlSerializer(typeof((string, TagNode.Flags)[]));

        if (!File.Exists(filepath))
            Directory.CreateDirectory(Path.GetDirectoryName(filepath) ?? string.Empty);

        var writer = new StreamWriter(filepath);

        xml.Serialize(writer, data.ToArray());

        writer.Close();
    }

    public void LoadState(string filepath)
    {
        if (!Directory.Exists(Path.GetDirectoryName(filepath) ?? string.Empty))
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