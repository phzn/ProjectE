using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020000B9 RID: 185
	public struct RTHandleStaticHelpers
	{
		// Token: 0x06000581 RID: 1409 RVA: 0x0001C921 File Offset: 0x0001AB21
		public static void SetRTHandleStaticWrapper(RenderTargetIdentifier rtId)
		{
			if (RTHandleStaticHelpers.s_RTHandleWrapper == null)
			{
				RTHandleStaticHelpers.s_RTHandleWrapper = RTHandles.Alloc(rtId);
				return;
			}
			RTHandleStaticHelpers.s_RTHandleWrapper.SetTexture(rtId);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0001C944 File Offset: 0x0001AB44
		public static void SetRTHandleUserManagedWrapper(ref RTHandle rtWrapper, RenderTargetIdentifier rtId)
		{
			if (rtWrapper == null)
			{
				return;
			}
			if (rtWrapper.m_RT != null)
			{
				throw new ArgumentException("Input wrapper must be a wrapper around RenderTargetIdentifier. Passed in warpper contains valid RenderTexture " + rtWrapper.m_RT.name + " and cannot be used as warpper.");
			}
			if (rtWrapper.m_ExternalTexture != null)
			{
				throw new ArgumentException("Input wrapper must be a wrapper around RenderTargetIdentifier. Passed in warpper contains valid Texture " + rtWrapper.m_ExternalTexture.name + " and cannot be used as warpper.");
			}
			rtWrapper.SetTexture(rtId);
		}

		// Token: 0x04000405 RID: 1029
		public static RTHandle s_RTHandleWrapper;
	}
}
