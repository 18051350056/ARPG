/********************************************************************
   	file：		NetSvc.cs
	author：		矍铄的金先知
  	created：	2022/6/21 20:54:53
   	mail: 		2816387346@qq.com

	function：  网络服务
*********************************************************************/

using PENet;
using UnityEngine;
using System.Collections.Generic;
using PEProtocol;

public class NetSvc : MonoBehaviour 
{
    public static NetSvc Instance = null;

    private static readonly string obj = "lock";
    PESocket<ClientSession, GameMsg> client = null;
    private Queue<GameMsg> msgQue = new Queue<GameMsg>();

    public void InitSvc()
    {
        Instance = this;

        client = new PESocket<ClientSession, GameMsg>(); 

        client.SetLog(true, (string msg, int lv) =>
        {
            switch (lv)
            {
                case 0:
                    msg = "Log: " + msg;
                    Debug.Log(msg);
                    break;
                case 1:
                    msg = "Warn: " + msg;
                    Debug.LogWarning(msg);
                    break;
                case 2:
                    msg = "Error: " + msg;
                    Debug.LogError(msg)  ;
                    break;
                case 3:
                    msg = "Info: " + msg;
                    Debug.Log(msg);
                    break;
            }
        });
        //最好先设置日志接口再启动
        client.StartAsClient(SrvCfg.srvIP, SrvCfg.srvPort);
        PECommon.Log("Init NetSvc...");
    }

    public void SendMsg(GameMsg msg)
    {
        if (client.session != null)
        {
            client.session.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("服务器未连接");
            InitSvc();
        }
    }

    public void AddNetPkg(GameMsg msg)
    {
        lock (obj)
        {
            msgQue.Enqueue(msg);
        }
    }

    private void Update()
    {
        if (msgQue.Count > 0)
        {
            lock (obj)
            {
                GameMsg msg = msgQue.Dequeue();
                ProcessMsg(msg);
            }
        }
    }

    private void ProcessMsg(GameMsg msg)
    {
        if (msg.err != (int)ErrorCode.None)
        {
            switch ((ErrorCode)msg.err)
            {
                case ErrorCode.ServerDataError:
                    PECommon.Log("服务器数据异常", LogType.Error);
                    GameRoot.AddTips("客户端数据异常");
                    break;
                case ErrorCode.UpdateDBError:
                    PECommon.Log("数据库更新异常", LogType.Error);
                    GameRoot.AddTips("网络不稳定");
                    break;
                case ErrorCode.AcctIsOnline:
                    GameRoot.AddTips("账号已上线");
                        break;
                case ErrorCode.WrongPass:
                    GameRoot.AddTips("密码错误");
                    break;
                case ErrorCode.LackLevel:
                    GameRoot.AddTips("角色等级不足");
                    break;
                case ErrorCode.LackCoin:
                    GameRoot.AddTips("金币不足");
                    break;
                case ErrorCode.LackCrystal:
                    GameRoot.AddTips("水晶不足");
                    break;
                case ErrorCode.LackDiamond:
                    GameRoot.AddTips("钻石不足");
                    break;
            }
        }
        switch ((CMD)msg.cmd)
        {
            case CMD.RspLogin:
                LoginSys.Instance.RspLogin(msg);
                break;
            case CMD.RspRename:
                LoginSys.Instance.RspRename(msg);
                break;
            case CMD.RspGuide:
                MainCitySys.Instance.RspGuide(msg);
                break;
            case CMD.RspStrong:
                MainCitySys.Instance.RspStrong(msg);
                break;
            case CMD.PshChat:
                MainCitySys.Instance.PshChat(msg);
                break;
            case CMD.RspBuy:
                MainCitySys.Instance.RspBuy(msg);
                break;
        }

    }

}