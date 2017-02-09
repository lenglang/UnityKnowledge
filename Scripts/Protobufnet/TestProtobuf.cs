using UnityEngine;
using System.Collections;
using ProtoBuf;
using System.IO;

public class TestProtobuf : MonoBehaviour {

	// Use this for initialization
	void Start () {
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
	
	// Update is called once per frame
	void Update () {
	
	}
}
