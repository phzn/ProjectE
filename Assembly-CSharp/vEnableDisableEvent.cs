using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000114 RID: 276
public class vEnableDisableEvent : MonoBehaviour
{
	// Token: 0x060009AA RID: 2474 RVA: 0x000448DC File Offset: 0x00042ADC
	private void OnEnable()
	{
		this.onEnable.Invoke();
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x000448E9 File Offset: 0x00042AE9
	private void OnDisable()
	{
		this.onDisable.Invoke();
	}

	// Token: 0x040008FE RID: 2302
	public UnityEvent onEnable;

	// Token: 0x040008FF RID: 2303
	public UnityEvent onDisable;
}
