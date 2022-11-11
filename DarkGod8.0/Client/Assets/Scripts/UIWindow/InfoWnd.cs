/********************************************************************
   	file：		InfoWnd.cs
	author：		矍铄的金先知
  	created：	2022/6/30 21:15:8
   	mail: 		2816387346@qq.com

	function：  角色信息展示
*********************************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoWnd : WindowRoot 
{
    #region UI Define
    public RawImage imgChar;

    public Text txtInfo;
    public Text txtExp;
    public Image imgExpPrg;
    public Text txtPower;
    public Image imgPowerPrg;

    public Text txtJob;
    public Text txtFight;
    public Text txtHP;
    public Text txtHurt;
    public Text txtDef;

    public Button btnClose;
    public Button btnDetail;
    public Button btnCloseDetail;
    public Transform transDetail;

    public Text dtxhp;
    public Text dtxad;
    public Text dtxap;
    public Text dtxaddef;
    public Text dtxapdef;
    public Text dtxdodoge;
    public Text dtxpierce;
    public Text dtxcritical;
    #endregion

    private Vector2 startPos;
    protected override void InitWnd()
    {
        base.InitWnd();
        ReqTouchEvt();
        SetActive(transDetail.gameObject, false);
        RefreshUI();
    }

    //拖动距离转化为旋转距离
    private void ReqTouchEvt()
    {
        OnClickDown(imgChar.gameObject, (PointerEventData evt) => 
        {
            startPos = evt.position;
            MainCitySys.Instance.SetStartRotate();
        });
        OnDrag(imgChar.gameObject, (PointerEventData evt) =>
        {
            float rotate = -(evt.position.x - startPos.x) * 0.4f;
            MainCitySys.Instance.SetPlayerRotate(rotate);
        });
    }

    private void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(txtInfo, pd.name + " LV." + pd.lv);
        SetText(txtExp, pd.exp + "/" + PECommon.GetExpUpValByLv(pd.lv));
        imgExpPrg.fillAmount = pd.exp * 1.0f / PECommon.GetExpUpValByLv(pd.lv);
        SetText(txtPower, pd.power + "/" + PECommon.GetPowerLimit(pd.lv));
        imgPowerPrg.fillAmount = pd.power * 1.0f / PECommon.GetPowerLimit(pd.lv);

        SetText(txtJob, " 职业    暗夜刺客");
        SetText(txtFight, " 战力    " + PECommon.GetFightBYProps(pd));
        SetText(txtHP, " 血量    " + pd.hp);
        SetText(txtHurt, " 伤害    " + (pd.ad + pd.ap));
        SetText(txtDef, " 防御    " + (pd.addef + pd.apdef));

        //detail
        SetText(dtxhp, pd.hp);
        SetText(dtxad, pd.ad);
        SetText(dtxap, pd.ap);
        SetText(dtxaddef, pd.addef);
        SetText(dtxapdef, pd.apdef);
        SetText(dtxdodoge, pd.dodge + "%");
        SetText(dtxpierce, pd.pierce + "%");
        SetText(dtxcritical, pd.critical + "%");
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        SetWndState(false);
        MainCitySys.Instance.CloseInfoWnd();
    }

    public void ClickDetailBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        SetActive(transDetail.gameObject);
    }

    public void ClickCloseDetailBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIOpenPage);
        SetActive(transDetail.gameObject, false);
    }
}