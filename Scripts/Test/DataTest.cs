using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class DataTest : MonoBehaviour {
    private void Start()
    {
        Dictionary<string, int> _dictionary = new Dictionary<string, int>();
        _dictionary.Add("数值1", 1);
        _dictionary.Add("数值2", 1);
        _dictionary.Add("数值3", 10086);
        _dictionary.Add("数值4", 1);
        XmlUtility.SerializeXmlToFile(@"C:\Users\Administrator.SKY-20160818IDX\Desktop\1.xml", _dictionary.ToList());
        Dictionary<string,int> dictionary = XmlUtility.DeserializeXml<List<KeyValuePair<string, int>>>(File.ReadAllBytes(@"C:\Users\Administrator.SKY-20160818IDX\Desktop\1.xml")).ToDictionary(item => item.Key, item => item.Value);
        Debug.Log(dictionary["数值3"]);

    }
}
