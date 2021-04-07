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
            xml.Load("F:\\projects\\MovingObisFromXmlToNeo4j\\128.gxc");
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
            string type = node.Attributes[NodeName.Type].Value.Substring(6);
            string logicalName, description;
            foreach (XmlNode child in node.ChildNodes)
            {
                if(child.Name == NodeName.LogicalName)
                {
                    logicalName = child.ChildNodes[0].Value;
                }
                else if (child.Name == NodeName.Description)
                {
                    description = child.ChildNodes[0].Value;
                }
            }
        } 
    }
    class NodeName
    {
        public static string LogicalName = "LogicalName";
        public static string Description = "Description";
        public static string Type = "xsi:type";
    }
}
