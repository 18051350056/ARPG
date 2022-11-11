/********************************************************************
	file:		GameMsg.cs
	author:		矍铄的金先知
	created:	2022/06/21 16:56:26 
	mail:       2816387346@qq.com
	
	function:	网络通信协议(客户端服务共用)
*********************************************************************/
using PENet;
using System;

namespace PEProtocol
{
	[Serializable] //序列化标签
    public class GameMsg :PEMsg
    {
		public ReqLogin reqLogin;
		public RspLogin rspLogin;

		public ReqRename reqRename;
		public RspRename rspRename;

		public ReqGuide reqGuide;
		public RspGuide rspGuide;

        public ReqStrong reqStrong;
        public RspStrong rspStrong;

		public SndChat sndChat;
		public PshChat pshChat;

		public ReqBuy reqBuy;
		public RspBuy rspBuy;
    }
	#region 登陆相关
	[Serializable]
	public class ReqLogin
    {

		public string acct;
		public string pass;
    }

	[Serializable]
	public class RspLogin
    {
		//TODO
		public PlayerData playerData;
    }

	[Serializable]
	public class PlayerData
    {
		public int id;
		public string name;
		public int lv;
		public int exp;
		public int power;
		public int coin;
		public int diamond;
		public int crystal;

        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int dodge;		//闪避概率
        public int pierce;		//穿透比率
        public int critical;	//暴击概率

		public int guideid;
		public int[] strongArr;		//索引号代表位置，表示的是星级
    }

	[Serializable]
	public class ReqRename
    {
		public string name;
    }

	[Serializable]
	public class RspRename
    {
		public string name;
    }
    #endregion

    #region 引导相关
	//请求引导ID变化
	[Serializable]
	public class ReqGuide
    {
		public int guideid;
    }

    [Serializable]
    public class RspGuide
    {
        public int guideid;
        public int coin;
        public int lv;
        public int exp;
    }
    #endregion

	#region 强化相关
	[Serializable]
	public class ReqStrong
    {
		public int pos;
    }

    [Serializable]
    public class RspStrong 
	{
		public int coin;
		public int crystal;
		public int hp;
		public int ad;
		public int ap;
		public int addef;
		public int apdef;
		public int[] strongArr;
	}
	#endregion

	#region 聊天相关
	[Serializable]
	public class SndChat
    {
		public string chat;
    }

	[Serializable]
	public class PshChat
    {
		public string name;
		public string chat;
    }
	#endregion

	#region 资源交易相关
	[Serializable]
	public class ReqBuy
    {
		public int type;
		public int cost;
    }

	[Serializable]
	public class RspBuy
    {
        public int type;
        public int diamond;
        public int coin;
        public int power;
    }
    #endregion

    public enum ErrorCode
    {
		None = 0,		//无错误
		ServerDataError,//遇到挂b
		UpdateDBError,	//更新数据库出错

		AcctIsOnline,	//已上线
		WrongPass,		//密码错误
		NameIsExist,	//名字已经存在

		LackLevel,
		LackCoin,
		LackCrystal,
		LackDiamond,
    }

	public enum CMD
    {
		None = 0,
		//登陆相关 100
		ReqLogin = 101,
		RspLogin = 102,

		ReqRename = 103,
		RspRename = 104,

		//主城相关 200
		ReqGuide = 201,
        RspGuide = 202,

		ReqStrong = 203,
		RspStrong = 204,

		SndChat = 205,
		PshChat = 206,

		ReqBuy = 207,
		RspBuy = 208
    }

	public class SrvCfg
    {
		public const string srvIP = "127.0.0.1";
		public const int srvPort = 17666;
    }
}
