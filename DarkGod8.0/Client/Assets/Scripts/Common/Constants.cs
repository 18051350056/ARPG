/********************************************************************
   	file：		Constants.cs
	author：		矍铄的金先知
  	created：	2022/6/16 16:22:31
   	mail: 		2816387346@qq.com

	function：  常量配置
*********************************************************************/

using UnityEngine;
public enum TxtColor
{
    Red, 
    Green,
    Blue,
    Yellow
}

public class Constants
{
    private const string ColorRed = "<color=#FF0000FF>";
    private const string ColorGreen = "<color=#00FF00FF>";
    private const string ColorBlue = "<color=#00B4FFFF>";
    private const string ColorYellow = "<color=#FFFF00FF>";
    private const string ColorEnd = "</color>";

    public static string Color(string str, TxtColor c)
    {
        string result = "";
        switch(c)
        {
            case TxtColor.Red:
                result = ColorRed + str + ColorEnd;
                break;
            case TxtColor.Green:
                result = ColorGreen + str + ColorEnd;
                break;
            case TxtColor.Blue:
                result = ColorBlue + str + ColorEnd;
                break;
            case TxtColor.Yellow:
                result = ColorYellow + str + ColorEnd;
                break;
        }
        return result;
    }
    //AutoGuideNPC
    public const int NPCWiseMan = 0;
    public const int NPCGeneral = 1;
    public const int NPCArtisan = 2;
    public const int NPCTrader = 3;

    //场景名称/ID
    public const string SceneLogin = "SceneLogin";
    public const int MainCityMapID = 10000;
    //public const string SceneMainCity = "SceneMainCity";

    //音效名称
    public const string BGLogin = "bgLogin";
    public const string BGMainCity = "bgMainCity";

    //登陆按钮音效
    public const string UILoginBtn = "uiLoginBtn";

    //常规UI点击音效
    public const string UIClickBtn = "uiClickBtn";
    public const string UIExtenBtn = "uiExtenBtn";
    public const string UIOpenPage = "uiOpenPage";
    public const string FBItemEnter = "fbitem";

    //屏幕宽高比
    public const int ScreenStandardwidth = 1334;
    public const int ScreenStandardHeight = 750;

    //遥感点标准距离
    public const int ScreenOPDis = 90;

    //混合参数
    public const int  BlendIdle= 0;
    public const int  BlendWalk = 1;


    //角色移动速度
    public const int PlayerMoveSpeed = 8;
    public const int MonsterMoveSpeed = 5;

    //运动平滑
    public const float AccelerSpeed = 5;
}