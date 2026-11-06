using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MoreMountains.Tools
{
	// Token: 0x0200005C RID: 92
	public class MMAutoInputModule : MonoBehaviour
	{
		// Token: 0x0600020A RID: 522 RVA: 0x00009D09 File Offset: 0x00007F09
		protected virtual void Awake()
		{
			base.StartCoroutine(this.InitializeInputModule());
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00009D18 File Offset: 0x00007F18
		protected virtual IEnumerator InitializeInputModule()
		{
			EventSystem eventSystem = Object.FindObjectOfType<EventSystem>();
			if (eventSystem == null)
			{
				yield break;
			}
			eventSystem.gameObject.AddComponent<StandaloneInputModule>();
			yield return null;
			yield break;
		}

		// Token: 0x040002C6 RID: 710
		protected GameObject _eventSystemGameObject;
	}
}
