using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace GPUInstancerPro
{
	// Token: 0x02000043 RID: 67
	[RequireComponent(typeof(EventSystem))]
	public class GPUIInputModuleHandler : MonoBehaviour
	{
		// Token: 0x06000270 RID: 624 RVA: 0x0000D790 File Offset: 0x0000B990
		private void Start()
		{
			StandaloneInputModule standaloneInputModule;
			if (base.TryGetComponent<StandaloneInputModule>(out standaloneInputModule))
			{
				Object.Destroy(standaloneInputModule);
			}
			base.gameObject.AddOrGetComponent<InputSystemUIInputModule>(false);
		}
	}
}
