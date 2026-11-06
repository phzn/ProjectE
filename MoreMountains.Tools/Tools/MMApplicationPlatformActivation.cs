using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000007 RID: 7
	[AddComponentMenu("More Mountains/Tools/Activation/MMApplicationPlatformActivation")]
	public class MMApplicationPlatformActivation : MonoBehaviour
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00002571 File Offset: 0x00000771
		protected virtual void OnEnable()
		{
			if (this.ExecutionTime == MMApplicationPlatformActivation.ExecutionTimes.OnEnable)
			{
				this.Process();
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002582 File Offset: 0x00000782
		protected virtual void Awake()
		{
			if (this.ExecutionTime == MMApplicationPlatformActivation.ExecutionTimes.Awake)
			{
				this.Process();
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002592 File Offset: 0x00000792
		protected virtual void Start()
		{
			if (this.ExecutionTime == MMApplicationPlatformActivation.ExecutionTimes.Start)
			{
				this.Process();
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000025A4 File Offset: 0x000007A4
		protected virtual void Process()
		{
			foreach (PlatformBindings platformBindings in this.Platforms)
			{
				if (platformBindings.Platform == Application.platform)
				{
					this.DisableIfNeeded(platformBindings.PlatformAction, platformBindings.Platform.ToString());
				}
			}
			RuntimePlatform platform = Application.platform;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002624 File Offset: 0x00000824
		protected virtual void DisableIfNeeded(PlatformBindings.PlatformActions platform, string platformName)
		{
			if (base.gameObject.activeInHierarchy && platform == PlatformBindings.PlatformActions.Disable)
			{
				base.gameObject.SetActive(false);
				if (this.DebugToTheConsole)
				{
					Debug.LogFormat(base.gameObject.name + " got disabled via MMPlatformActivation, platform : " + platformName + ".", Array.Empty<object>());
				}
			}
		}

		// Token: 0x04000017 RID: 23
		[Header("Settings")]
		public MMApplicationPlatformActivation.ExecutionTimes ExecutionTime;

		// Token: 0x04000018 RID: 24
		public bool DebugToTheConsole;

		// Token: 0x04000019 RID: 25
		[Header("Platforms")]
		public List<PlatformBindings> Platforms;

		// Token: 0x02000399 RID: 921
		public enum ExecutionTimes
		{
			// Token: 0x04001D38 RID: 7480
			Awake,
			// Token: 0x04001D39 RID: 7481
			Start,
			// Token: 0x04001D3A RID: 7482
			OnEnable
		}
	}
}
