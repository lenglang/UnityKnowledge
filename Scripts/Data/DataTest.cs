using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class DataTest : MonoBehaviour {
    private void Start()
    {
        //将字典存储为可见的xml数据格式
        Dictionary<string, int> _dictionary = new Dictionary<string, int>();
        _dictionary.Add("数值1", 1);
        _dictionary.Add("数值2", 1);
        _dictionary.Add("数值3", 10086);
        _dictionary.Add("数值4", 1);
        XmlUtility.SerializeXmlToFile(@"C:\Users\Administrator.SKY-20160818IDX\Desktop\XmlUtility.xml", _dictionary.ToList());
        Dictionary<string, int> dictionary = XmlUtility.DeserializeXml<List<KeyValuePair<string, int>>>(File.ReadAllBytes(@"C:\Users\Administrator.SKY-20160818IDX\Desktop\XmlUtility.xml")).ToDictionary(item => item.Key, item => item.Value);
        Debug.Log(dictionary["数值3"]);


        //将字典存储为不可见数据
        BinaryUtility.SerializeBinary(@"C:\Users\Administrator.SKY-20160818IDX\Desktop\666", _dictionary);
        Dictionary<string, int> dictionary1 = BinaryUtility.DeserializeBinary<Dictionary<string, int>>(File.ReadAllBytes(@"C:\Users\Administrator.SKY-20160818IDX\Desktop\666"));
        Debug.Log(dictionary1["数值3"]);

        //创建xml格式数据
        DataFormat dataFormat;
        TitleItem tittleItem;
        List<DataFormat> dataFormatList = new List<DataFormat>();
        for (int i = 0; i < 1000; i++)
        {
            dataFormat = new DataFormat();
            dataFormat.theme = "测试"+i.ToString();
            dataFormat.items = new List<TitleItem>();
            for (int j = 0; j < 10; j++)
            {
                tittleItem = new TitleItem();
                tittleItem.keyword = "关键字" + i.ToString();
                dataFormat.items.Add(tittleItem);
            }
            dataFormatList.Add(dataFormat);
        }
        //将xml格式数据存储为不可见数据
        BinaryUtility.SerializeBinary(@"C:\Users\Administrator.SKY-20160818IDX\Desktop\888", dataFormatList);
        dataFormatList.Clear();
        dataFormatList= BinaryUtility.DeserializeBinary<List<DataFormat>>(File.ReadAllBytes(@"C:\Users\Administrator.SKY-20160818IDX\Desktop\888"));
        Debug.Log(dataFormatList[8].items[8].keyword);

        //将xml格式数据存储为可见数据
        XmlUtility.SerializeXmlToFile(@"C:\Users\Administrator.SKY-20160818IDX\Desktop\888.xml",dataFormatList);
    }
}
