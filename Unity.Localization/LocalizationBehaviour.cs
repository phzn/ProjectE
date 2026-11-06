using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Util;

namespace UnityEngine.Localization
{
	// Token: 0x02000023 RID: 35
	internal class LocalizationBehaviour : ComponentSingleton<LocalizationBehaviour>
	{
		// Token: 0x06000106 RID: 262 RVA: 0x0000484A File Offset: 0x00002A4A
		protected override string GetGameObjectName()
		{
			return "Localization Resource Manager";
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00004851 File Offset: 0x00002A51
		public static void ReleaseNextFrame(AsyncOperationHandle handle)
		{
			ComponentSingleton<LocalizationBehaviour>.Instance.DoReleaseNextFrame(handle);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000485E File Offset: 0x00002A5E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static long TimeSinceStartupMs()
		{
			return (long)(Time.realtimeSinceStartup * 1000f);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000486C File Offset: 0x00002A6C
		private void DoReleaseNextFrame(AsyncOperationHandle handle)
		{
			base.enabled = true;
			this.m_ReleaseQueue.Enqueue(new ValueTuple<int, AsyncOperationHandle>(Time.frameCount, handle));
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000488C File Offset: 0x00002A8C
		private void LateUpdate()
		{
			int frameCount = Time.frameCount;
			long num = LocalizationBehaviour.TimeSinceStartupMs() + 10L;
			while (this.m_ReleaseQueue.Count > 0 && this.m_ReleaseQueue.Peek().Item1 < frameCount && LocalizationBehaviour.TimeSinceStartupMs() < num)
			{
				AddressablesInterface.SafeRelease(this.m_ReleaseQueue.Dequeue().Item2);
			}
			if (this.m_ReleaseQueue.Count == 0)
			{
				base.enabled = false;
			}
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00004900 File Offset: 0x00002B00
		public static void ForceRelease()
		{
			foreach (ValueTuple<int, AsyncOperationHandle> valueTuple in ComponentSingleton<LocalizationBehaviour>.Instance.m_ReleaseQueue)
			{
				AddressablesInterface.SafeRelease(valueTuple.Item2);
			}
			ComponentSingleton<LocalizationBehaviour>.Instance.m_ReleaseQueue.Clear();
		}

		// Token: 0x0400003D RID: 61
		[TupleElementNames(new string[] { "frame", "handle" })]
		private Queue<ValueTuple<int, AsyncOperationHandle>> m_ReleaseQueue = new Queue<ValueTuple<int, AsyncOperationHandle>>();

		// Token: 0x0400003E RID: 62
		private const long k_MaxMsPerUpdate = 10L;

		// Token: 0x0400003F RID: 63
		private const bool k_DisableThrottling = false;
	}
}
