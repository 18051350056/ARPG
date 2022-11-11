/********************************************************************
	file:		PECommon.cs
	author:		矍铄的金先知
	created:	2022/06/21 21:49:24 
	mail:       2816387346@qq.com
	
	function:	客户端服务端共用工具类
*********************************************************************/

using PENet;
using PEProtocol;

public enum LogType
{
	Log = 0,
	Warn = 1,
	Error = 2,
	Info = 3
}
public  class PECommon
{

	public static void Log(string msg = "", LogType tp = LogType.Log)
    {
		LogLevel lv = (LogLevel)tp;
		PETool.LogMsg(msg, lv);
    }


	//战斗力计算
	public static int GetFightBYProps(PlayerData pd)
    {
		return pd.lv * 100 + pd.ad + pd.ap + pd.addef + pd.apdef;
    }

	//体力计算
	public static int GetPowerLimit(int lv)
    {
		return (lv - 1) / 10 * 150 + 150;
    }

	//计算经验值
	public static int GetExpUpValByLv(int lv)
    {
		return 50 * (lv * lv  + 5 * lv) - 200;
    }
}

 