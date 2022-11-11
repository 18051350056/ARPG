/********************************************************************
   	file：		WindowRoot.cs
	author：		矍铄的金先知
  	created：	2022/6/17 20:23:29
   	mail: 		2816387346@qq.com

	function：  UI界面基类
*********************************************************************/

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour 
{
    protected ResSvc resSvc = null;
    protected AudioSvc audioSvc = null;
    protected NetSvc netSvc = null;
    //控制当前窗口的开关
    public void SetWndState(bool isActive = true)
    {
        if(gameObject.activeSelf != isActive)
        {
           SetActive(gameObject, isActive);
        }
        if (isActive)
        {
            InitWnd();
        }
        else
        {
            ClearWnd();
        }
    }
    protected bool GetWndState()
    {
        return gameObject.activeSelf;
    }

    protected virtual void InitWnd()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        netSvc = NetSvc.Instance;
    }

    protected virtual void ClearWnd()
    {
        resSvc = null;
        audioSvc = null;
        netSvc = null;
    }

    #region Tool Functions
    protected void SetActive(GameObject go, bool isActive = true)
    {
        go.SetActive(isActive);
    }
    protected void SetActive(RectTransform rectTrans, bool state = true)
    {
        rectTrans.gameObject.SetActive(state);
    }
    protected void SetActive(Image img, bool state = true)
    {
        img.transform.gameObject.SetActive(state);
    }
    protected void SetActive(Text txt, bool state = true)
    {
        txt.transform.gameObject.SetActive(state);
    }

    protected void SetText(Text txt, string context = "")
    {
        txt.text = context;
    }
    protected void SetText(Transform trans, int num = 0)
    {
        SetText(trans.GetComponent<Text>(), num);
    }
    protected void SetText(Transform trans, string context = "")
    {
        SetText(trans.GetComponent<Text>(), context);
    }
    protected void SetText(Text txt, int num = 0)
    {
        SetText(txt, num.ToString());
    }

    protected void SetSprite(Image img, string path)
    {
        Sprite sp = resSvc.LoadeSprite(path, true);
        img.sprite = sp;
    }

    protected T GetOrAddComponent<T>(GameObject go) where T: Component
    {
        T t = go.GetComponent<T>();
        if(t == null)
        {
            t = go.AddComponent<T>();
        }
        return t;
    }
    #endregion

#region Click Evts
    protected void OnClick(GameObject go, Action<object> cb, object args)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClick = cb;
        listener.args = args;
    }
    protected void OnClickDown(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickDown = cb;
    } 
    protected void OnClickUp(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickUp = cb;
    } 
    protected void OnDrag(GameObject go, Action<PointerEventData> cb)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onDrag = cb;
    } 
    
#endregion
} 