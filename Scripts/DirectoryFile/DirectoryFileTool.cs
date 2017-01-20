using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
public class DirectoryFileTool
{
    #region 更改文件后缀
    /// <summary>
	/// 更改文件后缀
	/// </summary>
	/// <param name="filePath">要更改的文件路径</param>
	/// <param name="extensionName">后缀名</param>
	public static void ChangeExtension(string filePath,string extensionName)
	{
		if (File.Exists (filePath)) {
			string dfileName = Path.ChangeExtension(filePath, "."+extensionName);
			File.Move(filePath,dfileName);
		}
	}
    #endregion
    #region 根据指定的 Assets下的文件路径 返回这个路径下的所有文件名
    public static List<string> nameArray = new List<string>();
    /// <summary>  
    /// 根据指定的 Assets下的文件路径 返回这个路径下的所有文件名//  
    /// </summary>  
    /// <returns>文件名数组</returns>  
    /// <param name="path">Assets下“一"级路径</param>  
    /// <typeparam name="T">函数模板的类型名t</typeparam>  
    public static List<string> GetObjectNameToArray(string path)
    {
        string objPath = Application.dataPath + "/" + path;
        string[] directoryEntries;
        try
        {
            //返回指定的目录中文件和子目录的名称的数组或空数组  
            directoryEntries = System.IO.Directory.GetFileSystemEntries(objPath);
            for (int i = 0; i < directoryEntries.Length; i++)
            {
                string p = directoryEntries[i];
                //得到要求目录下的文件或者文件夹（一级的）//  
                string[] tempPaths = StringExtention.SplitWithString(p, "/Assets/" + path + "\\");

                //tempPaths 分割后的不可能为空,只要directoryEntries不为空//  
                if (tempPaths[1].EndsWith(".meta"))
                    continue;
                string[] pathSplit = StringExtention.SplitWithString(tempPaths[1], ".");
                //文件  
                if (pathSplit.Length > 1)
                {
                    nameArray.Add(pathSplit[0]);
                }
                //遍历子目录下 递归吧！  
                else
                {
                    GetObjectNameToArray(path + "/" + pathSplit[0]);
                    continue;
                }
            }
        }
        catch (System.IO.DirectoryNotFoundException)
        {
            Debug.Log("The path encapsulated in the " + objPath + "Directory object does not exist.");
        }
        return nameArray;
    }
    /// 自定义的字符串分割的方法  
    /// </summary>  
    public class StringExtention
    {
        public static string[] SplitWithString(string sourceString, string splitString)
        {
            List<string> arrayList = new List<string>();
            string s = string.Empty;
            while (sourceString.IndexOf(splitString) > -1)  //分割  
            {
                s = sourceString.Substring(0, sourceString.IndexOf(splitString));
                sourceString = sourceString.Substring(sourceString.IndexOf(splitString) + splitString.Length);
                arrayList.Add(s);
            }
            arrayList.Add(sourceString);
            return arrayList.ToArray();
        }
    }
    #endregion
    #region 更改目录下所有文件名
    public static void ChangeFilesName()
    {
        string path = @"D:\vs2012\测试ierp\ierp-2\Web";
        DirectoryInfo TheFolder = new DirectoryInfo(path);
        foreach (FileInfo item in TheFolder.GetFiles())
        {
            string name = item.Name;
            if (name.Contains("_"))
            {
                string xnm = name.Replace("_", "-");
                string xingname = path + "\\转换后的文件\\" + xnm;
                item.MoveTo(xingname);
            }
            else
            {
                string xingname = path + "\\转换后的文件\\" + name;
                item.MoveTo(xingname);
            }
        }
    }
    #endregion
}
