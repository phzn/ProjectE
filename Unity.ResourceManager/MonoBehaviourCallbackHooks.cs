using System;
using UnityEngine;
using UnityEngine.ResourceManagement.Util;

// Token: 0x02000006 RID: 6
internal class MonoBehaviourCallbackHooks : ComponentSingleton<MonoBehaviourCallbackHooks>
{
	// Token: 0x14000003 RID: 3
	// (add) Token: 0x0600001F RID: 31 RVA: 0x000024F9 File Offset: 0x000006F9
	// (remove) Token: 0x06000020 RID: 32 RVA: 0x00002512 File Offset: 0x00000712
	public event Action<float> OnUpdateDelegate
	{
		add
		{
			this.m_OnUpdateDelegate = (Action<float>)Delegate.Combine(this.m_OnUpdateDelegate, value);
		}
		remove
		{
			this.m_OnUpdateDelegate = (Action<float>)Delegate.Remove(this.m_OnUpdateDelegate, value);
		}
	}

	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06000021 RID: 33 RVA: 0x0000252B File Offset: 0x0000072B
	// (remove) Token: 0x06000022 RID: 34 RVA: 0x00002544 File Offset: 0x00000744
	internal event Action<float> OnLateUpdateDelegate
	{
		add
		{
			this.m_OnLateUpdateDelegate = (Action<float>)Delegate.Combine(this.m_OnLateUpdateDelegate, value);
		}
		remove
		{
			this.m_OnLateUpdateDelegate = (Action<float>)Delegate.Remove(this.m_OnLateUpdateDelegate, value);
		}
	}

	// Token: 0x06000023 RID: 35 RVA: 0x0000255D File Offset: 0x0000075D
	protected override string GetGameObjectName()
	{
		return "ResourceManagerCallbacks";
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002564 File Offset: 0x00000764
	internal void Update()
	{
		Action<float> onUpdateDelegate = this.m_OnUpdateDelegate;
		if (onUpdateDelegate == null)
		{
			return;
		}
		onUpdateDelegate(Time.unscaledDeltaTime);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x0000257B File Offset: 0x0000077B
	internal void LateUpdate()
	{
		Action<float> onLateUpdateDelegate = this.m_OnLateUpdateDelegate;
		if (onLateUpdateDelegate == null)
		{
			return;
		}
		onLateUpdateDelegate(Time.unscaledDeltaTime);
	}

	// Token: 0x04000008 RID: 8
	internal Action<float> m_OnUpdateDelegate;

	// Token: 0x04000009 RID: 9
	internal Action<float> m_OnLateUpdateDelegate;
}
