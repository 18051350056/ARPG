/********************************************************************
	file:		LoginSys.cs
	author:		矍铄的金先知
	created:	2022/06/21 16:45:25 
	mail:       2816387346@qq.com
	
	function:	登录业务系统
*********************************************************************/

using PEProtocol;

public class LoginSys
{
    private CacheSvc cacheSvc = null;
    public static LoginSys instance = null;
    public static LoginSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LoginSys();
            }
            return instance;
        }
    }

    public void Init()
    {
        cacheSvc = CacheSvc.Instacne;
        PECommon.Log("LoginSys Init Done");
    }

    public void ReqLogin(MsgPack pack)
    {
        ReqLogin data = pack.msg.reqLogin;
        //当前账号是否已经上线
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspLogin
        };

            //已上线：返回错误信息
        if (cacheSvc.IsAcctOnLine(data.acct))
        {
            msg.err = (int)ErrorCode.AcctIsOnline;
        }
        else
        {
            //未上线：
            //账号是否存在
            PlayerData pd = cacheSvc.GetPlayerData(data.acct, data.pass);
            if (pd == null)
            {
                //存在检测密码
                msg.err = (int)ErrorCode.WrongPass;
            }
            else
            {
                //有效的账号数据返回客户端
                msg.rspLogin = new RspLogin
                {
                    playerData = pd
                };
                //缓存账号信息
                cacheSvc.AcctOnline(data.acct, pack.session, pd);
            }

        }
        //回应客户端（发消息，拿到session才可以）
        pack.session.SendMsg(msg);
    }

    public void ReqRename(MsgPack pack)
    {
        ReqRename data = pack.msg.reqRename;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspRename,
        };

        //判断当前名字是否存在(缓存层)
        if (cacheSvc.IsNameExist(data.name))
        {
            //存在返回错误码
            msg.err = (int)ErrorCode.NameIsExist;
        }
        else
        {
            //不存在:更新缓存以及数据库，在返回给客户端 
            PlayerData playerData = cacheSvc.GetPlayerDataBySession(pack.session);
            playerData.name = data.name;

            if (!cacheSvc.UpdatePlayerData(playerData.id, playerData))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspRename = new RspRename
                {
                    name = data.name
                };
            }
        }
        pack.session.SendMsg(msg);
    }

    public void ClearOfflineData(ServerSession session)
    {
        cacheSvc.AcctOffLine(session);
    }
}
