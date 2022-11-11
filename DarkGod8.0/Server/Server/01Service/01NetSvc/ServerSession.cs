/********************************************************************
	file:		ServerSession.cs
	author:		矍铄的金先知
	created:	2022/06/21 17:29:22 
	mail:       2816387346@qq.com
	
	function:	网络会话连接
*********************************************************************/
using PENet;
using PEProtocol;

public class ServerSession : PESession<GameMsg>
{
	public int sessionID = 0;
    protected override void OnConnected()
    {
		sessionID = ServerRoot.Instance.GetSessionID();
		PECommon.Log("SessionID: " + sessionID + "    Client Connect");
    }

	//此步多线程（网络IO部分较慢所以用多线程）
	protected override void OnReciveMsg(GameMsg msg)
    {
		PECommon.Log("SessionID:" + sessionID + "  PcvPack CMD: " + ((CMD)msg.cmd).ToString());
		//单线程处理，不加锁就会导致数据竞争
		NetSvc.Instance.AddMsgQue(this, msg);
    }

    protected override void OnDisConnected()
    {
		LoginSys.Instance.ClearOfflineData(this);
		PECommon.Log("SessionID: " + sessionID + "  Client Offline");
    }
}
