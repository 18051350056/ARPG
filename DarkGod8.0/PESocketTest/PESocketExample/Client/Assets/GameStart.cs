/****************************************************
    文件：GameStart.cs
	作者：SIKI学院——Plane
    邮箱: 1785275942@qq.com
    日期：#CreateTime#
	功能：Nothing
*****************************************************/

using Protocal;
using UnityEngine;

public class GameStart : MonoBehaviour {
    PENet.PESocket<ClientSession, NetMsg> client = null;

    private void Start() {
        client = new PENet.PESocket<ClientSession, NetMsg>();
        client.StartAsClient(IPCfg.srvIP, IPCfg.srvPort);

        client.SetLog(true, (string msg, int lv) => {
            switch (lv) {
                case 0:
                    msg = "Log:" + msg;
                    Debug.Log(msg);
                    break;
                case 1:
                    msg = "Warn:" + msg;
                    Debug.LogWarning(msg);
                    break;
                case 2:
                    msg = "Error:" + msg;
                    Debug.LogError(msg);
                    break;
                case 3:
                    msg = "Info:" + msg;
                    Debug.Log(msg);
                    break;
            }
        });
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            client.session.SendMsg(new NetMsg {
                text = "hello unity"
            });
        }
    }
}