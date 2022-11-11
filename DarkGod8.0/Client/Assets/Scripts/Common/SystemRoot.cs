/********************************************************************
   	file：		SystemRoot.cs
	author：		矍铄的金先知
  	created：	2022/6/18 17:1:27
   	mail: 		2816387346@qq.com

	function：  业务系统基类
*********************************************************************/

using UnityEngine;

public class SystemRoot : MonoBehaviour 
{
    protected ResSvc resSvc;
    protected AudioSvc audioSvc;
    protected NetSvc netSvc;

    public virtual void InitSys()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        netSvc = NetSvc.Instance;
    }
}     