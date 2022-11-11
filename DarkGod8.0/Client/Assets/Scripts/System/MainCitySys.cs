/********************************************************************
   	file：		MainCitySys.cs
	author：		矍铄的金先知
  	created：	2022/6/25 21:7:5
   	mail: 		2816387346@qq.com

	function：  主城业务系统
*********************************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.AI;


public class MainCitySys : SystemRoot 
{
    public static MainCitySys Instance = null;

    public MainCityWnd maincityWnd;
    public InfoWnd infoWnd;
    public GuideWnd guideWnd;
    public StrongWnd strongWnd;
    public ChatWnd chatWnd;
    public BuyWnd buyWnd;

    private PlayerController playerCtrl;
    private Transform charCamTrans;
    private AutoGuideCfg curTaskData;
    private Transform[] npcPosTrans;
    private NavMeshAgent nav;
    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init MainCitySys...");
    }

    public void EnterMainCity()
    {
        //通过id获取主城配置之后再加载场景可能会好一点
        MapCfg mapData = resSvc.GetMapCfgData(Constants.MainCityMapID);
        resSvc.AsyncLoadScene(mapData.sceneName, () => 
        {
            PECommon.Log("Enter MainCity...");
            //加载游戏主角
            LoadPlayer(mapData);

            //打开主城UI
            maincityWnd.SetWndState();

            //播放主城音乐
            audioSvc.PlayBGMusic(Constants.BGMainCity);
            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            MainCityMap mcm = map.GetComponent<MainCityMap>();
            npcPosTrans = mcm.NpcPosTrans;

            //设置人物展示相机
            if (charCamTrans != null)
            {
                charCamTrans.gameObject.SetActive(false);
            }
        });
    }

    private void LoadPlayer(MapCfg mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssissnCityPlayerPrefab);
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = new Vector3(1.5F, 1.5F, 1.5F);

        //相机位置初始化
        Camera.main.transform.position = mapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mapData.mainCamRote;

        playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();
        nav = player.GetComponent<NavMeshAgent>();
    }

    public void SetMoveDir(Vector2 dir)
    {
        StopNavTask();
        if (dir == Vector2.zero)
        {
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
        else
        {
            playerCtrl.SetBlend(Constants.BlendWalk);
        }
        playerCtrl.Dir = dir;
    }
    #region Buy Wnd
    public void OpenBuyWnd(int type)
    {
        StopNavTask();
        buyWnd.SetBuyType(type);
        buyWnd.SetWndState();
    }

    public void RspBuy(GameMsg msg)
    {
        RspBuy data = msg.rspBuy;
        GameRoot.Instance.SetPlayerDataByBuy(data);

        GameRoot.AddTips("购买成功");
        maincityWnd.RefreshUI();
        buyWnd.SetWndState(false);
    }
    #endregion

    #region Chat Wnd
    public void OpenChatWnd()
    {
        StopNavTask();
        chatWnd.SetWndState();
    }

    public void PshChat(GameMsg msg)
    {
        chatWnd.AddChatMsg(msg);
    }
    #endregion
    #region Strong Wnd
    public void OpenStrongWnd()
    {
        StopNavTask();
        strongWnd.SetWndState();
    }

    public void RspStrong(GameMsg msg)
    {
        int fightPre = PECommon.GetFightBYProps(GameRoot.Instance.PlayerData);
        GameRoot.Instance.SetPlayerDataByStrong(msg.rspStrong);
        int fightNow = PECommon.GetFightBYProps(GameRoot.Instance.PlayerData);
        GameRoot.AddTips(Constants.Color("战斗力提升:" + (fightNow - fightPre), TxtColor.Blue));

        strongWnd.UpdateUI();
        maincityWnd.RefreshUI();
    }
    #endregion

    #region Info Wnd
    public void OpenInfoWnd()
    {
        StopNavTask();
        if (charCamTrans == null)
        {
            charCamTrans = GameObject.FindGameObjectWithTag("CharShowCam").transform;
        }
        //设置人物展示相机相对位置
        charCamTrans.localPosition = playerCtrl.transform.position + playerCtrl.transform.forward * 3.8f + new Vector3(0, 1.2f, 0);
        charCamTrans.localEulerAngles = new Vector3(0, 180 + playerCtrl.transform.localEulerAngles.y, 0);
        charCamTrans.localScale = Vector3.one;
        charCamTrans.gameObject.SetActive(true);
        infoWnd.SetWndState();
    }   

    //关闭人物展示相机
    public void CloseInfoWnd()
    {
        if (charCamTrans != null)
        {
            charCamTrans.gameObject.SetActive(false);
            infoWnd.SetWndState(false);
        }
    }

    //初始旋转位置角度记录
    private float startRotate = 0;  
    public void SetStartRotate()
    {
        startRotate = playerCtrl.transform.localEulerAngles.y;
    }

    //操作人物旋转
    public void SetPlayerRotate(float rotate)
    {
        playerCtrl.transform.localEulerAngles = new Vector3(0, startRotate + rotate, 0);
    }
    #endregion

    #region  Guide Wnd
    private bool isNavGuide = false;     //设置导航标志变量
    public void RunTask(AutoGuideCfg gd)
    {
        if (gd != null)
        {
            curTaskData = gd;
        }

        //解析任务数据
        nav.enabled = true;
        if (curTaskData.npcID != -1)
        {
            //寻路
            float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[gd.npcID].position);
            if (dis < 0.5f)
            {
                isNavGuide = false;
                nav.isStopped = true;
                playerCtrl.SetBlend(Constants.BlendIdle);
                nav.enabled = false;

                OpenGuideWnd();
            }
            else
            {
                isNavGuide = true;
                nav.enabled = true;
                nav.speed = Constants.PlayerMoveSpeed;
                nav.SetDestination(npcPosTrans[gd.npcID].position);
                playerCtrl.SetBlend(Constants.BlendWalk);
            }
            //判定找到npc
        }
        else
        {
            OpenGuideWnd();
        }
    }

    public void Update()
    {
        if (isNavGuide)
        {
            ISArriveNavPos();
            playerCtrl.SetCam();
        }
    }

    private void ISArriveNavPos()
    {
        //寻路
        float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[curTaskData.npcID].position);
        if (dis < 0.5f)
        {
            isNavGuide = false;
            nav.isStopped = true;
            playerCtrl.SetBlend(Constants.BlendIdle);
            nav.enabled = false;

            OpenGuideWnd();
        }
    }

    //关闭导航界面  自行移动或者打开别的人物面板时运行
    private void StopNavTask()
    {
        if (isNavGuide)
        {
            isNavGuide = false;

            nav.isStopped = true;
            nav.enabled = false;
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
    }

    //打开引导界面
    private void OpenGuideWnd()
    {
        //TODO
        Debug.Log("Open DiaLog Wnd");
        guideWnd.SetWndState();
    }

    public AutoGuideCfg GetCurtTaskData()
    {
        return curTaskData;
    }

    public void RspGuide(GameMsg msg)
    {
        RspGuide data = msg.rspGuide;
        GameRoot.AddTips(Constants.Color("任务奖励 金币 +" + curTaskData.coin + "  经验 +" + curTaskData.exp, TxtColor.Blue));

        switch(curTaskData.actID)
        {
            case 0:
                //与智者对话
                break;
            case 1:
                //进入副本 
                break;
            case 2:
                //进入强化界面
                break;
            case 3:
                //进入体力购买
                break;
            case 4:
                //进入金币铸造
                break;
            case 5:
                //进入世界聊天
                break;
        }
        GameRoot.Instance.SetPlayerDataByGuide(data);
        maincityWnd.RefreshUI();
    }
    #endregion
}