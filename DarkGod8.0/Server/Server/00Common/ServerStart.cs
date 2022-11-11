/********************************************************************
	file:		ServerStart.cs
	author:		矍铄的金先知
	created:	2022/06/21 16:31:30 
	mail:       2816387346@qq.com
	
	function:	服务器入口
*********************************************************************/
class ServerStart
{
    static void Main(string[] args)
    {
		ServerRoot.Instance.Init();

		while (true)
        {
			ServerRoot.Instance.Update();
        }
    }
}
