/********************************************************************
   	file：		BuyWnd.cs
	author：		矍铄的金先知
  	created：	2022/7/13 22:8:2
   	mail: 		2816387346@qq.com

	function：  购买交易窗口
*********************************************************************/

using UnityEngine;
using UnityEngine.UI;
using PEProtocol;

public class BuyWnd : WindowRoot 
{
    public Text txtInfo;
    public Button btnSure;

    private int buyType;    //0: power 1: coin

    public void SetBuyType(int type)
    {
        this.buyType = type;
    }
    protected override void InitWnd()
    {
        base.InitWnd();
        btnSure.interactable = true;

        RefreshUI();
    }

    private void RefreshUI()
    {
        switch (buyType)
        {
            case 0:
                txtInfo.text = "是否花费" + Constants.Color("10钻石", TxtColor.Red) + "购买" + Constants.Color("100体力", TxtColor.Green) + "?";
                break;
            case 1:
                txtInfo.text = "是否花费" + Constants.Color("10钻石", TxtColor.Red) + "购买" + Constants.Color("500金币", TxtColor.Green) + "?";
                break;
        }
    }

    public void ClickSureBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        //发送网络消息
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.ReqBuy,
            reqBuy = new ReqBuy
            {
                type = buyType,
                cost = 10
            }
        };
        netSvc.SendMsg(msg);
        btnSure.interactable = false;
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }
}