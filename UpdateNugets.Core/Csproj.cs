using System;
using System.Collections.Generic;
using System.Xml;

namespace UpdateNugets.Core
{
    enum VersionType
    {
        VersionAttribute,
        VersionAsChild,
        Undefined
    }

    public class Csproj : ICsproj
    {
        private const string _packageReferenceConstant = "PackageReference";
        private const string _includeConstant = "Include";
        private const string _updateConstant = "Update";
        private const string _versionConstant = "Version";

        private string _filePath;
        private readonly XmlDocument _doc;

        public Csproj(string filePath)
        {
            _filePath = filePath;
            _doc = new XmlDocument();
        }

        public IDictionary<string, string> GetAllNugets()
        {
            var result = new Dictionary<string, string>();

            _doc.Load(_filePath);
            var allPackageReferences = GetAllPackageReferencesNode();

            foreach (var item in allPackageReferences)
            {
                var name = item.Attributes[_includeConstant] != null ? item.Attributes[_includeConstant].Value : item.Attributes[_updateConstant].Value;
                var versionAttribute = item.Attributes[_versionConstant]?.Value;
                var versionChild = item.FirstChild?.InnerText;

                var versionType = GetVersionType(item, name, versionAttribute, versionChild);

                switch (versionType)
                {
                    case VersionType.VersionAttribute:
                        result.Add(name, versionAttribute);
                        break;
                    case VersionType.VersionAsChild:
                        result.Add(name, versionChild);
                        break;
                    default:
                        //throw new Exception($"Something went wrong in {_filePath}");
                        break;
                }
            }
            return result;
        }

        public void UpdateANuget(string nugetName, string newNuGetVersion)
        {
            _doc.Load(_filePath);
            var allPackageReferences = GetAllPackageReferencesNode();

            foreach (var item in allPackageReferences)
            {
                var name = item.Attributes[_includeConstant];

                if (name == null || !name.Value.Equals(nugetName))
                {
                    continue;
                }

                var versionAttribute = item.Attributes[_versionConstant];
                var versionChild = item.FirstChild;

                var versionType = GetVersionType(item, name?.Value, versionAttribute?.Value, versionChild?.InnerText);

                switch (versionType)
                {
                    case VersionType.VersionAttribute:
                        versionAttribute.Value = newNuGetVersion;
                        break;
                    case VersionType.VersionAsChild:
                        versionChild.InnerText = newNuGetVersion;
                        break;
                    default:
                        throw new Exception($"Something went wrong in {_filePath}");
                }

                break;
            }

            _doc.Save(_filePath);
        }

        private IList<XmlNode> GetAllPackageReferencesNode()
        {
            var result = new List<XmlNode>();

            foreach (XmlNode node in _doc.DocumentElement.ChildNodes)
            {
                if (node.FirstChild?.Name == _packageReferenceConstant)
                {
                    foreach (XmlNode item in node.ChildNodes)
                    {
                        if (item.NodeType != XmlNodeType.Comment)
                        {
                            result.Add(item);
                        }
                    }
                }
            }

            return result;
        }

        private VersionType GetVersionType(XmlNode node, string name, string versionAttribute, string versionChild)
        {
            var isVersionAsAttribute = name != null && versionAttribute != null;
            var isVersionAsChild = name != null && versionChild != null && versionChild != null;

            if (isVersionAsAttribute)
            {
                return VersionType.VersionAttribute;
            }
            else if (isVersionAsChild)
            {
                return VersionType.VersionAsChild;
            }
            else
            {
                return VersionType.Undefined;
            }
        }
    }
}
