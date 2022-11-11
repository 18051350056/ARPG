/********************************************************************
	file:		GuideSys.cs
	author:		矍铄的金先知
	created:	2022/07/05 15:40:04 
	mail:       2816387346@qq.com
	
	function:	引导业务系统
*********************************************************************/

using PEProtocol;

public class GuideSys
{
	private CacheSvc cacheSvc = null;
	private CfgSvc cfgSvc = null;
	public static GuideSys instance = null;
	public static GuideSys Instance
    {
        get
        {
			if (instance == null)
             {
				instance = new GuideSys();
            }
            return instance;
        }
    }

	public void Init()
    {
		cacheSvc = CacheSvc.Instacne;
		cfgSvc = CfgSvc.Instance;
		PECommon.Log("GuideSys Init Done");
    }

	public void ReqGuide(MsgPack pack)
    {
		ReqGuide data = pack.msg.reqGuide; 

		GameMsg msg = new GameMsg
		{
			cmd = (int)CMD.RspGuide
		};

		PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
		GuideCfg gc = cfgSvc.GetGuideCfg(data.guideid);
		// 跟新引导任务ID
		if (pd.guideid == data.guideid)
        {
			pd.guideid += 1;

			//获取任务奖励，跟新奖励数据到数据库 
			pd.coin += gc.coin;
			CalcExp(pd, gc.exp);

			if (!cacheSvc.UpdatePlayerData(pd.id, pd))
            {
				msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
				msg.rspGuide = new RspGuide
				{
					guideid = pd.guideid,
					coin = pd.coin,
					lv = pd.lv,
					exp = pd.exp
				};
            }
        }
        else
        {
			msg.err = (int)ErrorCode.ServerDataError;
        }
		//更新结果发回客户端
		pack.session.SendMsg(msg);
    }

	private void CalcExp(PlayerData pd, int addExp)
    {
		int curtLv = pd.lv;
		int curtExp = pd.exp;
		int addRestExp = addExp;
		while (true)
        {
			int upNeedExp = PECommon.GetExpUpValByLv(curtLv) - curtExp;
			if (addRestExp >= upNeedExp)
            {
				curtLv += 1;
				curtExp = 0;
				addRestExp -= upNeedExp;
            }
            else
            {
				pd.lv = curtLv;
				pd.exp = curtExp + addRestExp;
				break;
            }
        }
    }
}

