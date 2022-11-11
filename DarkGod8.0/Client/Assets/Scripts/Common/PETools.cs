/********************************************************************
	file:		PETools.cs
	file: 		H:\UN\XM\DarkSolder\Assets\Scripts\Common\PETools.cs
	author:		矍铄的金先知
	created:	2022/06/20 16:02:26 
	mail:       2816387346@qq.com
	
	function:	工具类
*********************************************************************/

public class PETools
{
	public static int RDInt(int min, int max, System.Random rd = null)
    {
		if (rd == null)
        {
			rd = new System.Random();
		}		
		int val = rd.Next(min, max + 1);
		return val;
    }
}
 