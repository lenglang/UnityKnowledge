using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Text;
using SimpleJSON;

public class ExcelToJson{

	[MenuItem("小工具/ExcelCSVToJson")]
	static void excelToJson()
	{
		string dataFolderPath=Application.dataPath+"/WZK/Scripts/ExcelToJson/Data";
        string outJsonPath = Application.dataPath + "/WZK/Scripts/ExcelToJson/Json";
		if(!Directory.Exists(dataFolderPath))
		{
			Debug.LogError("请建立"+dataFolderPath+" 文件夹，并且把csv文件放入此文件夹内");
			return;
		}
		string[] allCSVFiles=Directory.GetFiles(dataFolderPath,"*.csv");
		if(allCSVFiles==null||allCSVFiles.Length<=0)
		{
			Debug.LogError(""+dataFolderPath+" 文件夹没有csv文件,请放入csv文件到此文件夹内");
			return;
		}
		if(!Directory.Exists(outJsonPath))
		{
			Directory.CreateDirectory(outJsonPath);
		}

		for(int i=0;i<allCSVFiles.Length;i++)
		{
			string dictName=new DirectoryInfo(Path.GetDirectoryName(allCSVFiles[i])).Name;
			string fileName=Path.GetFileNameWithoutExtension(allCSVFiles[i]);

			string jsonData=readExcelData(allCSVFiles[i]);
			outJsonContentToFile(jsonData,outJsonPath+"/"+dictName+"/"+fileName+".json");
		}

	}
	static string readExcelData(string fileName)
	{
		if(!File.Exists(fileName))
		{
			return null;
		}
		string fileContent=File.ReadAllText(fileName,UnicodeEncoding.Default);
		string[] fileLineContent=fileContent.Split(new string[]{"\r\n"},System.StringSplitOptions.None);
		if(fileLineContent!=null)
		{
			//注释的名字
			//string[] noteContents=fileLineContent[0].Split(new string[]{","},System.StringSplitOptions.None);
			//变量的名字
			string[] VariableNameContents=fileLineContent[1].Split(new string[]{","},System.StringSplitOptions.None);

			JSONClass jsonData=new JSONClass();
			for(int i=2;i<fileLineContent.Length-1;i++)
			{
				string[] lineContents=fileLineContent[i].Split(new string[]{","},System.StringSplitOptions.None);
				JSONClass classLine=new JSONClass();
				for(int j=1;j<lineContents.Length;j++)
				{
					classLine[VariableNameContents[j]]=lineContents[j];
				}
				jsonData[lineContents[0]]=classLine;
			}
			string resultJson=jsonData.ToString("");
			return resultJson;
		}
		return null;
	}

	static void outJsonContentToFile(string jsonData,string jsonFilePath)
	{
		string directName=Path.GetDirectoryName(jsonFilePath);
		if(!Directory.Exists(directName))
		{
			Directory.CreateDirectory(directName);
		}
		File.WriteAllText(jsonFilePath,jsonData,Encoding.UTF8);
		Debug.Log("成功输出Json文件  :"+jsonFilePath);
	}

}
