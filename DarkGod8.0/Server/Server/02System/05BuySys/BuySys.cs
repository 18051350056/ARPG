/********************************************************************
	file:		BuySys.cs
	author:		矍铄的金先知
	created:	2022/07/15 21:30:30 
	mail:       2816387346@qq.com
	
	function:	资源交易系统
*********************************************************************/

using PEProtocol;

public class BuySys
{
    private CacheSvc cacheSvc = null;
    public static BuySys instance = null;
    public static BuySys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BuySys();
            }
            return instance;
        }
    }

    public void Init()
    {
        cacheSvc = CacheSvc.Instacne;
        PECommon.Log("BuySys Init Done");
    }

    public void ReqBuy(MsgPack pack)
    {
        ReqBuy data = pack.msg.reqBuy;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspBuy
        };

        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);
        if (pd.diamond < data.cost)
        {
            msg.err = (int) ErrorCode.LackDiamond;
        }
        else
        {
            pd.diamond -= data.cost;
            switch (data.type)
            {
                case 0:
                    pd.power += 100;
                    break;
                case 1:
                    pd.coin += 500;
                    break;
            }

            if (!cacheSvc.UpdatePlayerData(pd.id, pd))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                RspBuy rspBuy = new RspBuy
                {
                    type = data.type,
                    coin = pd.coin,
                    diamond = pd.diamond,
                    power = pd.power,
                };
                msg.rspBuy = rspBuy;
            }
        }
        pack.session.SendMsg(msg);
    }
}

