/********************************************************************
	file:		StrongSys.cs
	author:		矍铄的金先知
	created:	2022/07/11 17:04:54 
	mail:       2816387346@qq.com
	
	function:	强化升级系统
*********************************************************************/

using PEProtocol;

public class StrongSys
{
    private CacheSvc cacheSvc = null;
    public static StrongSys instance = null;
    public static StrongSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new StrongSys();
            }
            return instance;
        }
    }

    public void Init()
    {
        cacheSvc = CacheSvc.Instacne;
        PECommon.Log("StrongSys Init Done");
    }

    public void ReqStrong(MsgPack pack)
    {
        ReqStrong data = pack.msg.reqStrong;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspStrong
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        int curtStarLv = pd.strongArr[data.pos];
        StrongCfg nextSd = CfgSvc.Instance.GetStrongCfg(data.pos, curtStarLv + 1);
        
        //条件判断
        if (pd.lv < nextSd.minlv)
        {
            msg.err = (int)ErrorCode.LackLevel;
        }
        else if (pd.coin < nextSd.coin)
        {
            msg.err = (int)ErrorCode.LackCoin;
        }
        else if (pd.crystal < nextSd.crystal)
        {
            msg.err = (int)ErrorCode.LackCrystal;
        }
        else
        {
            //资源扣除
            pd.coin -= nextSd.coin;
            pd.crystal -= nextSd.crystal;

            pd.strongArr[data.pos] += 1;

            //增加属性
            pd.hp += nextSd.addhp;
            pd.ad += nextSd.addhurt;
            pd.ap += nextSd.addhurt;
            pd.addef += nextSd.adddef;
            pd.apdef += nextSd.adddef;
        }

        //更新数据库
        if (!cacheSvc.UpdatePlayerData(pd.id, pd))
        {
            msg.err = (int)ErrorCode.UpdateDBError;
        }
        else
        {
            msg.rspStrong = new RspStrong
            {
                coin = pd.coin,
                crystal = pd.crystal,
                hp = pd.hp,
                ad = pd.ad,
                ap = pd.ap,
                addef = pd.addef,
                apdef = pd.apdef,
                strongArr = pd.strongArr
            };
        }
        pack.session.SendMsg(msg);
    }
}

