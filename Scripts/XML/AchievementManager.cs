using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System;
public class Reward
{
    public Task task;
    public Attribute attribute;
    public Reward() { }
    public struct Task
    {
        [XmlAttribute("taskNo")]
        public int taskNo { get; set; }
        [XmlAttribute("taskReward")]
        public int taskReward { get; set; }
        public Id id1;
        public Id id2;
        public Id id3;
    }
    public struct Id
    {
        [XmlAttribute("flag")]
        public bool flag { get; set; }
        [XmlAttribute("name")]
        public string name { get; set; }
    }
}

public class AchievementManager : MonoBehaviour
{
    Reward reward;
    FileInfo t;
    string _data;
    void Start()
    {
        reward = new Reward();
        t = new FileInfo(Application.dataPath + "\\" + "Achievement.xml");
        //LoadXML();
        reward.task.id1.name = "杨纯/nate";
        reward.task.id2.name = "有点笨";
        reward.task.id3.name = "dlnuchunge";

        Save();
    }

    void LoadXML()
    {
        if (t.Exists)
        {
            StreamReader r = t.OpenText();
            string _info = r.ReadToEnd();
            r.Close();
            _data = _info;
            if (_data.ToString() != "")
            {
                reward = (Reward)DeserializeObject(_data);
            }
        }
    }
    public void Save()
    {
        _data = SerializeObject(reward);
        StreamWriter writer;
        if (t.Exists)
        {
            t.Delete();
        }
        writer = t.CreateText();
        writer.Write(_data);
        writer.Close();
    }
    string UTF8ByteArrayToString(byte[] characters)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        string constructedString = encoding.GetString(characters);
        return (constructedString);
    }

    byte[] StringToUTF8ByteArray(string pXmlString)
    {
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] byteArray = encoding.GetBytes(pXmlString);
        return byteArray;
    }

    // Here we serialize our Reward object of reward   
    string SerializeObject(object pObject)
    {
        string XmlizedString = null;
        MemoryStream memoryStream = new MemoryStream();
        XmlSerializer xs = new XmlSerializer(typeof(Reward));
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        xs.Serialize(xmlTextWriter, pObject);
        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
        return XmlizedString;
    }

    // Here we deserialize it back into its original form   
    object DeserializeObject(string pXmlizedString)
    {
        XmlSerializer xs = new XmlSerializer(typeof(Reward));
        /*对象数组序列化
         * ObjArr = new ObjectClass[2];
         * ObjArr[0] = new ObjectClass(8, "arr1", 80);
         * XmlSerializer xs = new XmlSerializer(typeof(ObjectClass[]));
         * **/


        /*对象List序列化
         * ObjectClass objC = new ObjectClass(4, "ni", 40); 
         * ListObjC.Add (objC);
         * XmlSerializer xsL = new XmlSerializer(typeof(List<ObjectClass>));
         * */
        MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
        //XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        return xs.Deserialize(memoryStream);
    }
}