/********************************************************************
   	file：		ChatWnd.cs
	author：		矍铄的金先知
  	created：	2022/7/12 17:3:13
   	mail: 		2816387346@qq.com

	function：   聊天界面
*********************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PEProtocol;

public class ChatWnd : WindowRoot 
{
    public InputField iptChat;
    public Text txtChat;
    public Image imgWorld;
    public Image imgGuild;
    public Image imgFriend;

    private int chatType;
    private List<string> chatList = new List<string>();
    protected override void InitWnd()
    {
        base.InitWnd();
        chatType = 0;
        RefreshUI();
    }

    public void AddChatMsg(GameMsg msg)
    {
        string name = msg.pshChat.name;
        string chat = msg.pshChat.chat;
        chatList.Add(Constants.Color(name + "：", TxtColor.Blue) + chat);
        if (chatList.Count > 12)
        {
            chatList.RemoveAt(0);
        }
        if (GetWndState())
        {
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        if (chatType == 0)
        {
            //世界
            string chatMsg = "";
            for (int i = 0; i < chatList.Count; i++)
            {
                chatMsg += chatList[i] + "\n";
            }
            SetText(txtChat, chatMsg);
            SetSprite(imgWorld, PathDefine.SpType1);
            SetSprite(imgGuild, PathDefine.SpType2);
            SetSprite(imgFriend, PathDefine.SpType2);
        }
        else if (chatType == 1)
        {
            //公会
            SetText(txtChat, "未加入公会");

            SetSprite(imgWorld, PathDefine.SpType2);
            SetSprite(imgGuild, PathDefine.SpType1);
            SetSprite(imgFriend, PathDefine.SpType2);
        }
        else if (chatType == 2)
        {
            //好友 ··
            SetText(txtChat, "无好友消息");

            SetSprite(imgWorld, PathDefine.SpType2);
            SetSprite(imgGuild, PathDefine.SpType2);
            SetSprite(imgFriend, PathDefine.SpType1);
        }
    }

    public void ClickWorldBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;
        RefreshUI();
    }
    public void ClickGuildBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 1;
        RefreshUI();
    }
    public void ClickFriendBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 2;
        RefreshUI();
    }
    public void ClickSendBtn()
    {
        if (iptChat.text != null && iptChat.text != "" && iptChat.text != " ")
        {
            //消息长度判定
            if (iptChat.text.Length > 12)
            {
                GameRoot.AddTips("字数超出限制");
            }
            else
            {
                //发送网络消息到服务器
                GameMsg msg = new GameMsg
                {
                    cmd = (int)CMD.SndChat,
                    sndChat = new SndChat
                    {
                        chat = iptChat.text
                    }
                };
                iptChat.text = "";
                netSvc.SendMsg(msg);
            }
        }
        else
        {
            GameRoot.AddTips("请输入有效聊天信息");
        }
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
    }
    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;
        SetWndState(false);
    }
}