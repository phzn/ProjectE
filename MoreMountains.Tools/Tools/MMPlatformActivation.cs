using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000011 RID: 17
	[AddComponentMenu("More Mountains/Tools/Activation/MMPlatformActivation")]
	public class MMPlatformActivation : MonoBehaviour
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002B33 File Offset: 0x00000D33
		protected virtual void OnEnable()
		{
			if (this.ExecutionTime == MMPlatformActivation.ExecutionTimes.OnEnable)
			{
				this.Process();
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002B44 File Offset: 0x00000D44
		protected virtual void Awake()
		{
			if (this.ExecutionTime == MMPlatformActivation.ExecutionTimes.Awake)
			{
				this.Process();
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002B54 File Offset: 0x00000D54
		protected virtual void Start()
		{
			if (this.ExecutionTime == MMPlatformActivation.ExecutionTimes.Start)
			{
				this.Process();
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002B65 File Offset: 0x00000D65
		protected virtual void Process()
		{
			this.DisableIfNeeded(this.UNITY_STANDALONE_WIN, "Windows");
			this.DisableIfNeeded(this.UNITY_STANDALONE, "Standalone");
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002B8C File Offset: 0x00000D8C
		protected virtual void DisableIfNeeded(MMPlatformActivation.PlatformActions platform, string platformName)
		{
			if (base.gameObject.activeInHierarchy && platform == MMPlatformActivation.PlatformActions.Disable)
			{
				base.gameObject.SetActive(false);
				if (this.DebugToTheConsole)
				{
					Debug.LogFormat(base.gameObject.name + " got disabled via MMPlatformActivation, platform : " + platformName + ".", Array.Empty<object>());
				}
			}
		}

		// Token: 0x04000036 RID: 54
		[Header("Settings")]
		public MMPlatformActivation.ExecutionTimes ExecutionTime;

		// Token: 0x04000037 RID: 55
		public bool DebugToTheConsole;

		// Token: 0x04000038 RID: 56
		[Header("Desktop")]
		public MMPlatformActivation.PlatformActions UNITY_STANDALONE_WIN;

		// Token: 0x04000039 RID: 57
		public MMPlatformActivation.PlatformActions UNITY_STANDALONE_OSX;

		// Token: 0x0400003A RID: 58
		public MMPlatformActivation.PlatformActions UNITY_STANDALONE_LINUX;

		// Token: 0x0400003B RID: 59
		public MMPlatformActivation.PlatformActions UNITY_STANDALONE;

		// Token: 0x0400003C RID: 60
		[Header("Mobile")]
		public MMPlatformActivation.PlatformActions UNITY_IOS;

		// Token: 0x0400003D RID: 61
		public MMPlatformActivation.PlatformActions UNITY_IPHONE;

		// Token: 0x0400003E RID: 62
		public MMPlatformActivation.PlatformActions UNITY_ANDROID;

		// Token: 0x0400003F RID: 63
		public MMPlatformActivation.PlatformActions UNITY_TIZEN;

		// Token: 0x04000040 RID: 64
		[Header("Console")]
		public MMPlatformActivation.PlatformActions UNITY_WII;

		// Token: 0x04000041 RID: 65
		public MMPlatformActivation.PlatformActions UNITY_PS4;

		// Token: 0x04000042 RID: 66
		public MMPlatformActivation.PlatformActions UNITY_XBOXONE;

		// Token: 0x04000043 RID: 67
		[Header("Others")]
		public MMPlatformActivation.PlatformActions UNITY_WEBGL;

		// Token: 0x04000044 RID: 68
		public MMPlatformActivation.PlatformActions UNITY_LUMIN;

		// Token: 0x04000045 RID: 69
		public MMPlatformActivation.PlatformActions UNITY_TVOS;

		// Token: 0x04000046 RID: 70
		public MMPlatformActivation.PlatformActions UNITY_WSA;

		// Token: 0x04000047 RID: 71
		public MMPlatformActivation.PlatformActions UNITY_FACEBOOK;

		// Token: 0x04000048 RID: 72
		public MMPlatformActivation.PlatformActions UNITY_ADS;

		// Token: 0x04000049 RID: 73
		public MMPlatformActivation.PlatformActions UNITY_ANALYTICS;

		// Token: 0x0400004A RID: 74
		[Header("Active in Editor")]
		public MMPlatformActivation.PlatformActions UNITY_EDITOR;

		// Token: 0x0400004B RID: 75
		public MMPlatformActivation.PlatformActions UNITY_EDITOR_WIN;

		// Token: 0x0400004C RID: 76
		public MMPlatformActivation.PlatformActions UNITY_EDITOR_OSX;

		// Token: 0x0400004D RID: 77
		public MMPlatformActivation.PlatformActions UNITY_EDITOR_LINUX;

		// Token: 0x0200039B RID: 923
		public enum ExecutionTimes
		{
			// Token: 0x04001D40 RID: 7488
			Awake,
			// Token: 0x04001D41 RID: 7489
			Start,
			// Token: 0x04001D42 RID: 7490
			OnEnable
		}

		// Token: 0x0200039C RID: 924
		public enum PlatformActions
		{
			// Token: 0x04001D44 RID: 7492
			DoNothing,
			// Token: 0x04001D45 RID: 7493
			Disable
		}
	}
}
