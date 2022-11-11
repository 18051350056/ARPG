/********************************************************************
	file:		CacheSvc.cs
	author:		矍铄的金先知
	created:	2022/06/22 15:49:50 
	mail:       2816387346@qq.com
	
	function:	缓存层
*********************************************************************/

using PEProtocol;
using System.Collections.Generic;

public class CacheSvc
{
	private static CacheSvc instance = null;

	public static CacheSvc Instacne
    {
		get
        {
			if (instance == null)
            {
				instance = new CacheSvc();
            }
			return instance;
        }
    }
	private DBMgr dbMgr;

	private Dictionary<string, ServerSession> onLineAcctDic = new Dictionary<string, ServerSession>();
	private Dictionary<ServerSession, PlayerData> onLineSessionDic = new Dictionary<ServerSession, PlayerData>(); 
	public void Init()
    {
		dbMgr = DBMgr.Instance;
		PECommon.Log("CacheSvc Init Done");
    }

	//判断当前账号是否上线
	public bool IsAcctOnLine(string acct)
    {
		return onLineAcctDic.ContainsKey(acct);
    }

	/// <summary>
	/// 更具账号密码回应对应账号数据， 密码错误返回null，账号不存在默认创建新账号
	/// </summary>
	public PlayerData GetPlayerData(string acct, string pass)
    {
		// 从数据库中查找账号信息
		return dbMgr.QueryPlayerData(acct, pass);
    }

	/// <summary>
	/// 帐号上线， 缓存数据
	/// </summary>
	public void AcctOnline(string acct, ServerSession session, PlayerData playerData)
    {
		onLineAcctDic.Add(acct, session);
		onLineSessionDic.Add(session, playerData);
    }

	public bool IsNameExist(string name)
    {
		return dbMgr.QueryNameData(name);
    }

	public List<ServerSession> GetOnLineServerSession()
    {
		List<ServerSession> Lst = new List<ServerSession>();
		foreach (var item in onLineSessionDic)
		{
			Lst.Add(item.Key);
		}
		return Lst;
    }

	public PlayerData GetPlayerDataBySession(ServerSession session)
    {
		if (onLineSessionDic.TryGetValue(session,out PlayerData playerData))
        {
			return playerData;
        }
		else
        {
			return null;
        }
    }

	public bool UpdatePlayerData(int id, PlayerData playerData)
    {
		//TODO
		return dbMgr.UpdatePlayerData(id, playerData);
    }


	//清除缓存，在LoginSys中调用
	public void AcctOffLine(ServerSession session)
    {
		foreach(var item in onLineAcctDic)
        {
			if (item.Value == session)
            {
				onLineAcctDic.Remove(item.Key);
				break;
            }
        }
        bool succ = onLineSessionDic.Remove(session);
        PECommon.Log("Offline Result: SessionID:" + session.sessionID + " " + succ);
    }
}
