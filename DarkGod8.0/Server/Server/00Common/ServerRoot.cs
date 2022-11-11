/********************************************************************
	file:		ServerRoot.cs
	author:		矍铄的金先知
	created:	2022/06/21 16:33:44 
	mail:       2816387346@qq.com
	
	function:	服务器初始化
*********************************************************************/

public  class ServerRoot
{
	public static ServerRoot instance = null;
	public static ServerRoot Instance
    {
        get
        {
			if (instance == null)
            {
				instance = new ServerRoot();
            }
			return instance;
        }
    }



	public void Init()
    {
		//数据层
		DBMgr.Instance.Init();

		//服务层
		CfgSvc.Instance.Init();
		CacheSvc.Instacne.Init();
		NetSvc.Instance.Init();

		//业务系统
		LoginSys.Instance.Init();
		GuideSys.Instance.Init();
		StrongSys.Instance.Init();
		ChatSys.Instance.Init();
		BuySys.Instance.Init();
    }

	public void Update()
    {
		NetSvc.Instance.Update();
    }

	private int SessionID = 0;
	public int GetSessionID()
    {
		if (SessionID == int.MaxValue)
        {
			SessionID = 0;
        }
		return SessionID += 1;
    }
}

