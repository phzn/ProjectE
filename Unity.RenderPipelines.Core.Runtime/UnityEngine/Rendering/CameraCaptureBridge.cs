using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x020000D0 RID: 208
	public static class CameraCaptureBridge
	{
		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x00021648 File Offset: 0x0001F848
		// (set) Token: 0x060006D1 RID: 1745 RVA: 0x0002164F File Offset: 0x0001F84F
		public static bool enabled
		{
			get
			{
				return CameraCaptureBridge._enabled;
			}
			set
			{
				CameraCaptureBridge._enabled = value;
			}
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00021658 File Offset: 0x0001F858
		public static IEnumerator<Action<RenderTargetIdentifier, CommandBuffer>> GetCaptureActions(Camera camera)
		{
			HashSet<Action<RenderTargetIdentifier, CommandBuffer>> hashSet;
			if (!CameraCaptureBridge.actionDict.TryGetValue(camera, out hashSet) || hashSet.Count == 0)
			{
				return null;
			}
			return hashSet.GetEnumerator();
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0002168C File Offset: 0x0001F88C
		public static void AddCaptureAction(Camera camera, Action<RenderTargetIdentifier, CommandBuffer> action)
		{
			HashSet<Action<RenderTargetIdentifier, CommandBuffer>> hashSet;
			CameraCaptureBridge.actionDict.TryGetValue(camera, out hashSet);
			if (hashSet == null)
			{
				hashSet = new HashSet<Action<RenderTargetIdentifier, CommandBuffer>>();
				CameraCaptureBridge.actionDict.Add(camera, hashSet);
			}
			hashSet.Add(action);
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x000216C4 File Offset: 0x0001F8C4
		public static void RemoveCaptureAction(Camera camera, Action<RenderTargetIdentifier, CommandBuffer> action)
		{
			if (camera == null)
			{
				return;
			}
			HashSet<Action<RenderTargetIdentifier, CommandBuffer>> hashSet;
			if (CameraCaptureBridge.actionDict.TryGetValue(camera, out hashSet))
			{
				hashSet.Remove(action);
			}
		}

		// Token: 0x04000477 RID: 1143
		private static Dictionary<Camera, HashSet<Action<RenderTargetIdentifier, CommandBuffer>>> actionDict = new Dictionary<Camera, HashSet<Action<RenderTargetIdentifier, CommandBuffer>>>();

		// Token: 0x04000478 RID: 1144
		private static bool _enabled;
	}
}
