/********************************************************************
   	file：		ClientSession.cs
	author：		矍铄的金先知
  	created：	2022/6/21 21:10:7
   	mail: 		2816387346@qq.com

	function：  客户端网络会话
*********************************************************************/

using PENet;
using PEProtocol;

public class ClientSession : PESession<GameMsg>
{
    protected override void OnConnected()
    {
        //base.OnConnected();
        GameRoot.AddTips("服务器连接成功");
        PECommon.Log("Connect To Server Suss");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        //base.OnReciveMsg(msg);
        PECommon.Log("RcvPack CMD: " + ((CMD)msg.cmd).ToString());
        NetSvc.Instance.AddNetPkg(msg);
    }
    protected override void OnDisConnected()
    {
        //base.OnDisConnected();
        GameRoot.AddTips("服务器断开链接");
        PECommon.Log("DisConnect To Server");
    }
}