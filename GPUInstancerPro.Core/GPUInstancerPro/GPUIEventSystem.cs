using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x0200005B RID: 91
	[HelpURL("https://wiki.gurbu.com/index.php?title=GPU_Instancer_Pro:GettingStarted#GPUI_Event_System")]
	public class GPUIEventSystem : MonoBehaviour
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000359 RID: 857 RVA: 0x000125D4 File Offset: 0x000107D4
		// (set) Token: 0x0600035A RID: 858 RVA: 0x000125DB File Offset: 0x000107DB
		public static GPUIEventSystem Instance { get; private set; }

		// Token: 0x0600035B RID: 859 RVA: 0x000125E4 File Offset: 0x000107E4
		private void Awake()
		{
			if (GPUIEventSystem.Instance != null && GPUIEventSystem.Instance != this)
			{
				Debug.LogError(GPUIConstants.LOG_PREFIX + "Duplicate GPUI Event System detected. Destroying second event system.", GPUIEventSystem.Instance);
				Object.Destroy(base.gameObject);
				return;
			}
			if (GPUIEventSystem.Instance == null)
			{
				GPUIEventSystem.Instance = this;
			}
			if (!GPUIRuntimeSettings.IsSupportedPlatform())
			{
				base.enabled = false;
				return;
			}
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00012654 File Offset: 0x00010854
		private void OnEnable()
		{
			GPUIRenderingSystem.InitializeRenderingSystem();
			GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
			instance.OnPreCull = (Action<GPUICameraData>)Delegate.Remove(instance.OnPreCull, new Action<GPUICameraData>(this.OnPreCull.Invoke));
			GPUIRenderingSystem instance2 = GPUIRenderingSystem.Instance;
			instance2.OnPreRender = (Action<GPUICameraData>)Delegate.Remove(instance2.OnPreRender, new Action<GPUICameraData>(this.OnPreRender.Invoke));
			GPUIRenderingSystem instance3 = GPUIRenderingSystem.Instance;
			instance3.OnPostRender = (Action<GPUICameraData>)Delegate.Remove(instance3.OnPostRender, new Action<GPUICameraData>(this.OnPostRender.Invoke));
			GPUIRenderingSystem instance4 = GPUIRenderingSystem.Instance;
			instance4.OnPreCull = (Action<GPUICameraData>)Delegate.Combine(instance4.OnPreCull, new Action<GPUICameraData>(this.OnPreCull.Invoke));
			GPUIRenderingSystem instance5 = GPUIRenderingSystem.Instance;
			instance5.OnPreRender = (Action<GPUICameraData>)Delegate.Combine(instance5.OnPreRender, new Action<GPUICameraData>(this.OnPreRender.Invoke));
			GPUIRenderingSystem instance6 = GPUIRenderingSystem.Instance;
			instance6.OnPostRender = (Action<GPUICameraData>)Delegate.Combine(instance6.OnPostRender, new Action<GPUICameraData>(this.OnPostRender.Invoke));
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00012768 File Offset: 0x00010968
		private void OnDisable()
		{
			if (!GPUIRenderingSystem.IsActive)
			{
				return;
			}
			GPUIRenderingSystem instance = GPUIRenderingSystem.Instance;
			instance.OnPreCull = (Action<GPUICameraData>)Delegate.Remove(instance.OnPreCull, new Action<GPUICameraData>(this.OnPreCull.Invoke));
			GPUIRenderingSystem instance2 = GPUIRenderingSystem.Instance;
			instance2.OnPreRender = (Action<GPUICameraData>)Delegate.Remove(instance2.OnPreRender, new Action<GPUICameraData>(this.OnPreRender.Invoke));
			GPUIRenderingSystem instance3 = GPUIRenderingSystem.Instance;
			instance3.OnPostRender = (Action<GPUICameraData>)Delegate.Remove(instance3.OnPostRender, new Action<GPUICameraData>(this.OnPostRender.Invoke));
		}

		// Token: 0x0400026E RID: 622
		[SerializeField]
		public GPUICameraEvent OnPreCull;

		// Token: 0x0400026F RID: 623
		[SerializeField]
		public GPUICameraEvent OnPreRender;

		// Token: 0x04000270 RID: 624
		[SerializeField]
		public GPUICameraEvent OnPostRender;
	}
}
