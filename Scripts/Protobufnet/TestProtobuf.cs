using UnityEngine;
using System.Collections;
using ProtoBuf;
using System.IO;

public class TestProtobuf : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        string dataPath = Application.dataPath + "/UnityKnowledge/Scripts/Protobufnet/user.bin";
        if (File.Exists(dataPath) == false)
        {
            User user = new User();
            user.ID = 100;
            user.Username = "siki";
            user.Password = "123456";
            user.Level = 100;
            user._UserType = User.UserType.Master;
            ////FileStream fs = File.Create(Application.dataPath+"/user.bin");
            ////print(Application.dataPath + "/user.bin");
            ////Serializer.Serialize<User>(fs, user);
            ////fs.Close();
            using (var fs = File.Create(dataPath))
            {
                Serializer.Serialize<User>(fs, user);
            }
        }


        User nuser = null;

        using (var fs = File.OpenRead(dataPath))
        {
            nuser = Serializer.Deserialize<User>(fs);
        }
        print(nuser.ID);
        print(nuser._UserType);
        print(nuser.Username);
        print(nuser.Password);
        print(nuser.Level);

    }
    /// <summary>
    /// 将文件内容转成数据流
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    private byte[] ReadFile(string fileName)
    {
        FileStream pFileStream = null;
        byte[] pReadByte = new byte[0];
        try
        {
            pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(pFileStream);
            r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开  
            pReadByte = r.ReadBytes((int)r.BaseStream.Length);
            return pReadByte;
        }
        catch
        {
            return pReadByte;
        }

        finally
        {
            if (pFileStream != null)
                pFileStream.Close();
        }
    }
    /// <summary>
    /// 数据流写入文件
    /// </summary>
    /// <param name="pReadByte"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    private bool writeFile(byte[] pReadByte, string fileName)
    {

        FileStream pFileStream = null;
        try
        {
            pFileStream = new FileStream(fileName, FileMode.OpenOrCreate);
            pFileStream.Write(pReadByte, 0, pReadByte.Length);
        }
        catch
        {
            return false;
        }
        finally
        {
            if (pFileStream != null)
                pFileStream.Close();
        }
        return true;
    }  
}
