/********************************************************************
   	file：		LoopDragonAni.cs
	author：		矍铄的金先知
  	created：	2022/6/18 17:25:9
   	mail: 		2816387346@qq.com

	function：  飞龙在天！不断循环
*********************************************************************/

using UnityEngine;

public class LoopDragonAni : MonoBehaviour 
{
    private Animation ani;

    private void Awake()
    {
        ani = transform.GetComponent<Animation>();
    }

    private void Start()
    {
        if (ani != null)
        {
            InvokeRepeating("PlayDrangonAni", 0, 20);
        }
    }

    private void PlayDrangonAni()
    {
        if (ani != null)
        {
            ani.Play();
        }
    }
}