using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using LitJson;
using System.Text;
public class XMLManager:MonoBehaviour {
    private ArrayList Adialogue = new ArrayList();
    private ArrayList Bdialogue = new ArrayList();
    private ArrayList textList = new ArrayList();
    /// <summary>
    /// 加载XMl
    /// </summary>
    void LoadXml()
    {
        //创建xml文档
        XmlDocument xml = new XmlDocument();
        XmlReaderSettings set = new XmlReaderSettings();
        set.IgnoreComments = true;//这个设置是忽略xml注释文档的影响。有时候注释会影响到xml的读取
        xml.Load(XmlReader.Create((Application.dataPath + "/data.xml"), set));
        //得到objects节点下的所有子节点
        XmlNodeList xmlNodeList = xml.SelectSingleNode("objects").ChildNodes;
        //遍历所有子节点
        foreach (XmlElement xl1 in xmlNodeList)
        {
            if (xl1.GetAttribute("id") == "1")
            {
                //继续遍历id为1的节点下的子节点
                foreach (XmlElement xl2 in xl1.ChildNodes)
                {
                    //放到一个textlist文本里
                    textList.Add(xl2.GetAttribute("name") + ": " + xl2.InnerText);
                    //得到name为a的节点里的内容。放到TextList里
                    if (xl2.GetAttribute("name") == "a")
                    {
                        Adialogue.Add(xl2.GetAttribute("name") + ": " + xl2.InnerText);
                    }
                    //得到name为b的节点里的内容。放到TextList里
                    else if (xl2.GetAttribute("name") == "b")
                    {
                        Bdialogue.Add(xl2.GetAttribute("name") + ": " + xl2.InnerText);
                    }
                }
            }
        }
        print(xml.OuterXml);
    }
    /// <summary>
    /// 更新XML
    /// </summary>
    void updateXML()
    {
        string path = Application.dataPath + "/data.xml";
        if (File.Exists(path))
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            XmlNodeList xmlNodeList = xml.SelectSingleNode("objects").ChildNodes;
            foreach (XmlElement xl1 in xmlNodeList)
            {
                if (xl1.GetAttribute("id") == "1")
                {
                    //把messages里id为1的属性改为5
                    xl1.SetAttribute("id", "5");
                }

                if (xl1.GetAttribute("id") == "2")
                {
                    foreach (XmlElement xl2 in xl1.ChildNodes)
                    {
                        if (xl2.GetAttribute("map") == "abc")
                        {
                            //把mission里map为abc的属性改为df，并修改其里面的内容
                            xl2.SetAttribute("map", "df");
                            xl2.InnerText = "我成功改变了你";
                        }

                    }
                }
            }
            xml.Save(path);
        }
    }
    /// <summary>
    /// 创建XML
    /// </summary>
    void CreateXML()
    {
        string path = Application.dataPath + "/data2.xml";
        if (!File.Exists(path))
        {
            //创建最上一层的节点。
            XmlDocument xml = new XmlDocument();
            //创建最上一层的节点。
            XmlElement root = xml.CreateElement("objects");
            //创建子节点
            XmlElement element = xml.CreateElement("messages");
            //设置节点的属性
            element.SetAttribute("id", "1");
            XmlElement elementChild1 = xml.CreateElement("contents");

            elementChild1.SetAttribute("name", "a");
            //设置节点内面的内容
            elementChild1.InnerText = "这就是你，你就是天狼";
            XmlElement elementChild2 = xml.CreateElement("mission");
            elementChild2.SetAttribute("map", "abc");
            elementChild2.InnerText = "去吧，少年，去实现你的梦想";
            //把节点一层一层的添加至xml中，注意他们之间的先后顺序，这是生成XML文件的顺序
            element.AppendChild(elementChild1);
            element.AppendChild(elementChild2);

            root.AppendChild(element);

            xml.AppendChild(root);
            //最后保存文件
            xml.Save(path);
        }
    }
    /// <summary>
    /// 添加XML内容
    /// </summary>
    void addXMLData()
    {
        string path = Application.dataPath + "/data2.xml";
        if (File.Exists(path))
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            XmlNode root = xml.SelectSingleNode("objects");
            //下面的东西就跟上面创建xml元素是一样的。我们把他复制过来就行了
            XmlElement element = xml.CreateElement("messages");
            //设置节点的属性
            element.SetAttribute("id", "2");
            XmlElement elementChild1 = xml.CreateElement("contents");

            elementChild1.SetAttribute("name", "b");
            //设置节点内面的内容
            elementChild1.InnerText = "天狼，你的梦想就是。。。。。";
            XmlElement elementChild2 = xml.CreateElement("mission");
            elementChild2.SetAttribute("map", "def");
            elementChild2.InnerText = "我要妹子。。。。。。。。。。";
            //把节点一层一层的添加至xml中，注意他们之间的先后顺序，这是生成XML文件的顺序
            element.AppendChild(elementChild1);
            element.AppendChild(elementChild2);

            root.AppendChild(element);

            xml.AppendChild(root);
            //最后保存文件
            xml.Save(path);
        }
    }
    /// <summary>
    /// 删除XML
    /// </summary>
    public void deleteXml()
    {
        string filepath = Application.dataPath + @"/my.xml";
        if (File.Exists(filepath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filepath);
            XmlNodeList nodeList = xmlDoc.SelectSingleNode("transforms").ChildNodes;
            foreach (XmlElement xe in nodeList)
            {
                if (xe.GetAttribute("id") == "1")
                {
                    xe.RemoveAttribute("id");
                }

                foreach (XmlElement x1 in xe.ChildNodes)
                {
                    if (x1.Name == "z")
                    {
                        x1.RemoveAll();

                    }
                }
            }
            xmlDoc.Save(filepath);
            Debug.Log("deleteXml OK!");
        }

    }
    /// <summary>
    /// 解析JSON字符串显示字典键值
    /// </summary>
    public void ResolveJson()
    {
        //定义的JSON字符串，注意JSON的格式
        string str = @"
            {
                ""Name""     : ""yusong"",
                ""Age""      : 26,
                ""Birthday"" : ""1986-11-21"",
 				""Thumbnail"":[
				{
           			""Url"":    ""http://xuanyusong.com"",
           			""Height"": 256,
           			""Width"":  ""200""
				},
				{
           			""Url"":    ""http://baidu.com"",
           			""Height"": 1024,
           			""Width"":  ""500""
				}
 
				]
            }";
        //这里是解析，包括整形与字符串
        JsonData jd = JsonMapper.ToObject(str);
        Debug.Log("name = " + (string)jd["Name"]);
        Debug.Log("Age = " + (int)jd["Age"]);
        Debug.Log("Birthday = " + (string)jd["Birthday"]);
        JsonData jdItems = jd["Thumbnail"];

        for (int i = 0; i < jdItems.Count; i++)
        {
            Debug.Log("URL = " + jdItems[i]["Url"]);
            Debug.Log("Height = " + (int)jdItems[i]["Height"]);
            Debug.Log("Width = " + jdItems[i]["Width"]);
        }
    }
    /// <summary>
    /// 合成JSON字符串，先合成 然后在输出。
    /// </summary>
    public void MergerJson()
    {
        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);

        writer.WriteObjectStart();

        writer.WritePropertyName("Name");
        writer.Write("yusong");

        writer.WritePropertyName("Age");
        writer.Write(26);

        writer.WritePropertyName("Girl");

        writer.WriteArrayStart();

        writer.WriteObjectStart();
        writer.WritePropertyName("name");
        writer.Write("ruoruo");
        writer.WritePropertyName("age");
        writer.Write(24);
        writer.WriteObjectEnd();

        writer.WriteObjectStart();
        writer.WritePropertyName("name");
        writer.Write("momo");
        writer.WritePropertyName("age");
        writer.Write(26);
        writer.WriteObjectEnd();

        writer.WriteArrayEnd();

        writer.WriteObjectEnd();
        Debug.Log(sb.ToString());

        JsonData jd = JsonMapper.ToObject(sb.ToString());
        Debug.Log("name = " + (string)jd["Name"]);
        Debug.Log("Age = " + (int)jd["Age"]);
        JsonData jdItems = jd["Girl"];
        for (int i = 0; i < jdItems.Count; i++)
        {
            Debug.Log("Girl name = " + jdItems[i]["name"]);
            Debug.Log("Girl age = " + (int)jdItems[i]["age"]);
        }
    }
}
