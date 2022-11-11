/********************************************************************
	file:		ChatSys.cs
	author:		矍铄的金先知
	created:	2022/07/12 22:15:23 
	mail:       2816387346@qq.com
	
	function:	在线聊天系统
*********************************************************************/

using PEProtocol;
using System.Collections.Generic;

public class ChatSys
{
    private CacheSvc cacheSvc = null;
    public static ChatSys instance = null;
    public static ChatSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ChatSys();
            } 
            return instance;
        }
    }

    public void Init()
    {
        cacheSvc = CacheSvc.Instacne;
        PECommon.Log("ChatSys Init Done");
    }

    public void SndChat(MsgPack pack)
    {
        SndChat data = pack.msg.sndChat;
        PlayerData pd = cacheSvc.GetPlayerDataBySession(pack.session);

        GameMsg msg = new GameMsg
        { 
            cmd = (int)CMD.PshChat,
            pshChat = new PshChat
            {
                name = pd.name,
                chat = data.chat
            }
        };
        //广播所有在线客户端
        List<ServerSession> Lst = cacheSvc.GetOnLineServerSession();
        byte[] bytes = PENet.PETool.PackNetMsg(msg);
        for (int i = 0; i < Lst.Count; i ++)
        {
            Lst[i].SendMsg(bytes);
        }
    }
}

