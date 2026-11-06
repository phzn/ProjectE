using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MoreMountains.Tools
{
	// Token: 0x0200004D RID: 77
	[AddComponentMenu("More Mountains/Tools/GUI/MMGetFocusOnEnable")]
	public class MMGetFocusOnEnable : MonoBehaviour
	{
		// Token: 0x060001AE RID: 430 RVA: 0x00007F14 File Offset: 0x00006114
		protected virtual void OnEnable()
		{
			EventSystem.current.SetSelectedGameObject(base.gameObject, null);
		}
	}
}
