/********************************************************************
   	file：		LoginSys.cs
	author：		矍铄的金先知
  	created：	2022/6/16 14:54:40
   	mail: 		2816387346@qq.com

	function：  登录注册业务系统
*********************************************************************/

using PEProtocol;
using UnityEngine;

public class LoginSys : SystemRoot 
{
    public static LoginSys Instance = null;

    public LoginWnd loginWnd;
    public CreateWnd createWnd;
    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init LoginSys...");
    }

    /// <summary>
    /// 进入登陆场景
    /// </summary>
    public void EnterLogin()
    {
        //TODO
        //异步加载登陆场景，并显示加载的进度
        resSvc.AsyncLoadScene(Constants.SceneLogin, () =>
        {
            //加载完以后再打开注册登录界面
            loginWnd.SetWndState();
            audioSvc.PlayBGMusic(Constants.BGLogin);
        });
    }
    

    public void RspLogin(GameMsg msg)
    {
        GameRoot.AddTips("登陆成功");
        GameRoot.Instance.SetPlayerData(msg.rspLogin);

        if (msg.rspLogin.playerData.name == "")
        {
            //打开角色创建界面
            createWnd.SetWndState();
        }
        else
        {
            //进入主城
            MainCitySys.Instance.EnterMainCity();
        }

        //关闭登陆界面
        loginWnd.SetWndState(false);
    }

    public void RspRename(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerName(msg.rspRename.name);

        //跳转场景进入主城
        MainCitySys.Instance.EnterMainCity();

        //关闭创建界面
        createWnd.SetWndState(false);
    }
}