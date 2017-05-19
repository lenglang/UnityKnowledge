using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class GUIExample : MonoBehaviour
{
    private string _setLevel = "0";
    public Texture img;//图片/ 
    private string userName="";//用户名  
    private string userPassword="";//密码  
    private string info="";//信息 
    //记录Toolbar按钮的ID  
    private int toolbarID;
    //Toolbar按钮上的信息  
    private string[] toolbarInfo;

    //问题  
    private string question;
    //四个Toggle按钮是否按下  
    private bool toggle0 = false;
    private bool toggle1 = false;
    private bool toggle2 = false;
    private bool toggle3 = false;
    //用来保证只有一个选项被选中  
    private bool[] isChanages = new bool[] { false, false, false, false };

    void OnGUI()
    {

        GUI.Label(new Rect(0, 0, 200, 20), "Hello World!");
        //GUI.Label(new Rect(10, 50, 200, 200), img);


        if (GUI.Button(new Rect(0, 30, 100, 20), "Hello World"))
        {
           
        }


        //用户名  
        GUI.Label(new Rect(20, 80, 50, 20), "用户名");
        userName = GUI.TextField(new Rect(80, 80, 100, 20), userName, 15);//15为最大字符串长度  
        //密码  
        GUI.Label(new Rect(20, 100, 50, 20), "密  码");
        userPassword = GUI.PasswordField(new Rect(80, 100, 100, 20), userPassword, '*');//'*'为密码遮罩  
        //信息  
        //GUI.Label(new Rect(20, 100, 100, 20), info);
        //登录按钮  
        if (GUI.Button(new Rect(80, 120, 50, 20), "登录"))
        {
            if (userName == "zuoyamin" && userPassword == "123")
            {
                info = "登录成功！";
            }
            else
            {
                info = "登录失败！";
            }
        }


        info = " 悯农-李绅 \n锄禾日当午，\n汗滴禾下土。\n谁知盘中餐，\n粒粒皆辛苦。";
        GUI.TextArea(new Rect(320, 20, 90, 100), info);


        _setLevel = GUI.TextField(new Rect(0,400,200,100), _setLevel);
        _setLevel = Regex.Replace(_setLevel, "[^0-9]", "");
        //(int)float.Parse(_setLevel)

        //初始化  
        info = "";
        toolbarInfo = new string[] { "File", "Edit", "Assets", "GameObject", "Help" };
        //绘制Toolbar  
        toolbarID = GUI.Toolbar(new Rect(20, 200, 500, 20), toolbarID, toolbarInfo);
        //根据toolbarID来获得info  
        info = toolbarInfo[toolbarID];
        //绘制标签  
        GUI.Label(new Rect(40, 260, 200, 20), info + " 被选中！");

        //初始化  
        info = "";
        question = "桌子上原来有12支点燃的蜡烛，先被风吹灭了3根，不久又一阵风吹灭了2根，最后桌子上还剩几根蜡烛呢?";
        //使用Label来显示问题  
        GUI.Label(new Rect(440, 240, 300, 50), question);
        //四个选项  
        toggle0 = GUI.Toggle(new Rect(445, 300, 100, 20), toggle0, "  A.  2");
        toggle1 = GUI.Toggle(new Rect(445, 320, 100, 20), toggle1, "  B.  3");
        toggle2 = GUI.Toggle(new Rect(445, 340, 100, 20), toggle2, "  C.  5");
        toggle3 = GUI.Toggle(new Rect(445, 360, 100, 20), toggle3, "  D.  12");
        //提交按钮  
        if (GUI.Button(new Rect(500,480, 100, 20), "提交"))
        {
            if (toggle2)
            {
                info = "恭喜您答对了！";
            }
            else
            {
                info = "不好意思，您答错了！";
            }
        }
        //显示答题对错信息  
        GUI.Label(new Rect(440, 400, 200, 20), info);
        //确保只有一个选项被选中  
        //备注：我也就只能想到这么土的方法了，如果大家有好的方法请告诉我，感激不尽！  
        if (GUI.changed)
        {
            if (toggle0 && !isChanages[0])
            {
                toggle1 = false;
                toggle2 = false;
                toggle3 = false;
                isChanages = new bool[] { true, false, false, false };

            }
            if (toggle1 && !isChanages[1])
            {
                toggle0 = false;
                toggle2 = false;
                toggle3 = false;
                isChanages = new bool[] { false, true, false, false };
            }
            if (toggle2 && !isChanages[2])
            {
                toggle1 = false;
                toggle0 = false;
                toggle3 = false;
                isChanages = new bool[] { false, false, true, false };
            }
            if (toggle3 && !isChanages[3])
            {
                toggle1 = false;
                toggle2 = false;
                toggle0 = false;
                isChanages = new bool[] { false, false, false, true };
            }
        }
    }
}
