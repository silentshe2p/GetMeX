using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace GetMeX.Models
{
    public class ImageSiteStructure
    {
        //private static string processedTag = "processed";
        // External file that specifies how to get images from sites
        private string externalStructureFilePath = @"{0}\iss.xml";
        // XML root name
        private string externalStructureFileRoot = "iss";

        private static List<string> _keyList = new List<string>();
        // Dictionary format: {"site", [desc, link, order, page]}
        private static Dictionary<string, string[]> _structureDict = new Dictionary<string, string[]>();

        public ImageSiteStructure()
        {
            var path = Directory.GetCurrentDirectory();
            var externalFile = string.Format(externalStructureFilePath, path);
            if (File.Exists(externalFile))
            {
                XmlDocument doc = new XmlDocument();
                try
                {
                    doc.Load(externalFile);
                    var root = doc.DocumentElement;
                    // processed = doc.GetElementsByTagName(processedTag);
                    if (root.Name == externalStructureFileRoot)
                    {
                        foreach (XmlNode node in root.ChildNodes)
                        {
                            var site = node.Name.ToLower();
                            if (!_structureDict.ContainsKey(site))
                            {
                                var structure = new string[4];
                                var elemCount = 0;
                                foreach (XmlNode n in node.ChildNodes)
                                {
                                    var elem = n.Name;
                                    var value = n.InnerText;
                                    if (elem == StructureElement.desc.ToString())
                                    {
                                        structure[(int)StructureElement.desc] = value;
                                    }
                                    else if (elem == StructureElement.link.ToString())
                                    {
                                        structure[(int)StructureElement.link] = value;
                                    }
                                    else if (elem == StructureElement.page.ToString())
                                    {
                                        structure[(int)StructureElement.page] = value;
                                    }
                                    else if (elem == StructureElement.order.ToString())
                                    {
                                        structure[(int)StructureElement.order] = value;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    elemCount++;
                                }
                                if (elemCount > 2)
                                {
                                    _structureDict.Add(site, structure);
                                }
                            }
                        }
                        //XmlNode processedElem = doc.CreateNode("element", processedTag, "");
                        //root.AppendChild(processedElem);
                        //doc.Save(externalFile);
                    }
                }
                catch (XmlException) { }
                finally
                {
                    _keyList = _structureDict.Keys.ToList();
                }
            }
        }

        public string SearchKnownSite(string site)
        {
            foreach (var key in _keyList)
            {
                if (site.Contains(key))
                {
                    return key;
                }
            }
            return null;
        }

        public string[] GetSiteStructure(string site)
        {
            var key = SearchKnownSite(site);
            return (key == null) ? null : _structureDict[key];
        }
    }

    public enum StructureElement
    {
        desc = 0,   // Image description
        link,           // Image link
        order,        // "default" if desc before link or anything else
        page          // page parameter for viewer to get next page
    }
}
