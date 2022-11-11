/********************************************************************
	file:		BaseData.cs
	author:		矍铄的金先知
	created:	2022/06/29 20:48:17 
	mail:       2816387346@qq.com
	
	function:	配置数据类
*********************************************************************/

using UnityEngine;

public class StrongCfg : BaseData<StrongCfg>
{
	public int pos;
	public int starlv;
	public int addhp;
	public int addhurt;
	public int adddef;
	public int minlv;
	public int coin;
	public int crystal;
}

public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
	public int npcID;   //出发任务目标NPC索引号
	public string dilogArr;
	public int actID;
	public int coin;
	public int exp;
}

public class MapCfg : BaseData<MapCfg>
{
	public string mapName;
	public string sceneName;
	public Vector3 mainCamPos;
	public Vector3 mainCamRote;
	public Vector3 playerBornPos;
	public Vector3 playerBornRote;
}

public class BaseData<T>
{
	public int ID;
}

