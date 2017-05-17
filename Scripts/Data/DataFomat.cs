using System;
using System.Collections.Generic;
using System.Xml.Serialization;
[Serializable]
public class DataFormat
{
    [XmlAttribute]
    public string theme;     // 分类名：形状、动物

    [XmlAttribute]
    public int level;

    [XmlAttribute]
    public bool limited;

    [XmlAttribute]
    public int number;

    [XmlAttribute]
    public bool exist = false;

    [XmlElement("Question")]
    public List<string> questions;

    [XmlElement("Item")]
    public List<TitleItem> items;        // 分类项

    [XmlArray("Right")]
    public List<string> rightAnswers;

    [XmlArray("Wrong")]
    public List<string> wrongAnswers;

    [XmlAttribute]
    public bool New;//是否新上线
}
[Serializable]
public class TitleItem
{
    [XmlAttribute]
    public string keyword;        // 某一类别中的具体名字：圆形、猴子

    [XmlAttribute]
    public string desc;           // 描述

    [XmlElement("FileName")]
    public List<string> fileNames;   // 对应的贴图名
}

