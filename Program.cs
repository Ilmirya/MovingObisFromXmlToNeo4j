using System;
using System.Xml;

namespace MovingObisFromXmlToNeo4j
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument xml = new XmlDocument();
            Console.WriteLine("Write full path to file:");
            string path = Console.ReadLine();//F:\projects\MovingObisFromXmlToNeo4j\128.gxc
            //xml.Load(path);
            xml.Load("E:\\128.gxc");
            XmlElement xmlElement = xml.DocumentElement;
            foreach(XmlNode node in xmlElement)
            {
                Child(node);
            }
        }
        private static void Child(XmlNode node)
        {
            if(node.Name == "GXDLMSObject")
            {
                DLMSObject(node);
            }
            else
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    Child(child);
                }
            }
        }
        private static void DLMSObject(XmlNode node)
        {
            string class_ = node.Attributes[XmlNodeName.Type].Value.Substring(6);
            Node parameter = new Node("ParameterTMPTMP", class_);
            foreach (XmlNode child in node.ChildNodes)
            {
                if(child.Name == XmlNodeName.LogicalName)
                {
                    parameter.SetObis(child.ChildNodes[0].Value);
                }
                else if (child.Name == XmlNodeName.Description)
                {
                    parameter.description = child.ChildNodes[0].Value;
                }
            }
            Neo4j.Instance.AddNode(parameter.id, parameter.type, parameter);
        } 
    }

    class Node
    {
        public Guid id { get; private set; }
        public string class_;
        public string type;
        public string obis { get; private set; }
        public string description;
        public string resource { get; private set; }

        private enum Resources
        {
            energy = 1
        }

        public Node(string type, string class_)
        {
            id = Guid.NewGuid();
            this.type = type;
            this.class_ = class_;
        }

        public void SetObis(string obis)
        {
            if (string.IsNullOrWhiteSpace(obis) || obis.Contains(".") == false) 
                throw new ArgumentException($"OBIS code does not meet the requirements: {obis}");
            this.obis = obis;
            resource = Enum.GetName(typeof(Resources), int.Parse(obis[0].ToString()));
        }
    }

    struct XmlNodeName
    {
        public static string LogicalName = "LogicalName";
        public static string Description = "Description";
        public static string Type = "xsi:type";
    }
}
