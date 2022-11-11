/********************************************************************
   	file：		LoadingWnd.cs
	author：		矍铄的金先知
  	created：	2022/6/16 16:51:35
   	mail: 		2816387346@qq.com

	function：  加载进度界面
*********************************************************************/

using UnityEngine;
using UnityEngine.UI;

public class LoadingWnd : WindowRoot 
{
    public Text txtTips;
    public Image imgFG;
    public Image imgPoint;
    public Text txtPrg;


    private float fgWidth;
    protected override void InitWnd()
    {
        //不用ResSvc.Instance......, 并且可以加入其他逻辑
        base.InitWnd();
        fgWidth = imgFG.GetComponent<RectTransform>().sizeDelta.x;
        
        SetText(txtTips, "-----这是一条游戏Tips 巴拉巴拉巴拉-----");
        SetText(txtPrg, "0%");
        imgFG.fillAmount = 0;
        imgPoint.transform.localPosition = new Vector3(-545, 0, 0);

        //resSvc.
    }

    public void SetProgress(float prg)
    {
        SetText(txtTips, (int)(prg * 100) + "%");
        //txtPrg.text = (int)(prg * 100) + "%";
        imgFG.fillAmount = prg;

        float posX = prg * fgWidth - 545;
        imgPoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
    }
} 