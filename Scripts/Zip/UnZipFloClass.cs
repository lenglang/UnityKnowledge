using System;
using System.Data;

using System.Text;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;

using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Checksums;




/// <summary>
/// UnZipFloClass 的摘要说明
/// </summary>
public class UnZipFloClass
{

    public string unZipFile(string TargetFile, string fileDir)
    {
        string rootFile = " ";
        try
        {
            //读取压缩文件(zip文件)，准备解压缩
            ZipInputStream s = new ZipInputStream(File.OpenRead(TargetFile.Trim()));
            ZipEntry theEntry;
            string path = fileDir;
            //解压出来的文件保存的路径

            string rootDir = " ";
            //根目录下的第一个子文件夹的名称
            while ((theEntry = s.GetNextEntry()) != null)
            {
                rootDir = Path.GetDirectoryName(theEntry.Name);
                //得到根目录下的第一级子文件夹的名称
                if (rootDir.IndexOf("\\") >= 0)
                {
                    rootDir = rootDir.Substring(0, rootDir.IndexOf("\\") + 1);
                }
                string dir = Path.GetDirectoryName(theEntry.Name);
                //根目录下的第一级子文件夹的下的文件夹的名称
                string fileName = Path.GetFileName(theEntry.Name);
                //根目录下的文件名称
                if (dir != " ")
                //创建根目录下的子文件夹,不限制级别
                {
                    if (!Directory.Exists(fileDir + "\\" + dir))
                    {
                        path = fileDir + "\\" + dir;
                        //在指定的路径创建文件夹
                        Directory.CreateDirectory(path);
                    }
                }
                else if (dir == " " && fileName != "")
                //根目录下的文件
                {
                    path = fileDir;
                    rootFile = fileName;
                }
                else if (dir != " " && fileName != "")
                //根目录下的第一级子文件夹下的文件
                {
                    if (dir.IndexOf("\\") > 0)
                    //指定文件保存的路径
                    {
                        path = fileDir + "\\" + dir;
                    }
                }

                if (dir == rootDir)
                //判断是不是需要保存在根目录下的文件
                {
                    path = fileDir + "\\" + rootDir;
                }

                //以下为解压缩zip文件的基本步骤
                //基本思路就是遍历压缩文件里的所有文件，创建一个相同的文件。
                if (fileName != String.Empty)
                {
                    FileStream streamWriter = File.Create(path + "\\" + fileName);

                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }

                    streamWriter.Close();
                }
            }
            s.Close();

            return rootFile;
        }
        catch (Exception ex)
        {
            return "1; " + ex.Message;
        }
    }
}
