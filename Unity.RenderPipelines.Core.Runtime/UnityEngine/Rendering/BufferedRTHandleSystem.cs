using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
	// Token: 0x020000B5 RID: 181
	public class BufferedRTHandleSystem : IDisposable
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x0001C062 File Offset: 0x0001A262
		public int maxWidth
		{
			get
			{
				return this.m_RTHandleSystem.GetMaxWidth();
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x0001C06F File Offset: 0x0001A26F
		public int maxHeight
		{
			get
			{
				return this.m_RTHandleSystem.GetMaxHeight();
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600055F RID: 1375 RVA: 0x0001C07C File Offset: 0x0001A27C
		public RTHandleProperties rtHandleProperties
		{
			get
			{
				return this.m_RTHandleSystem.rtHandleProperties;
			}
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0001C089 File Offset: 0x0001A289
		public RTHandle GetFrameRT(int bufferId, int frameIndex)
		{
			if (!this.m_RTHandles.ContainsKey(bufferId))
			{
				return null;
			}
			return this.m_RTHandles[bufferId][frameIndex];
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001C0AC File Offset: 0x0001A2AC
		public void AllocBuffer(int bufferId, Func<RTHandleSystem, int, RTHandle> allocator, int bufferCount)
		{
			RTHandle[] array = new RTHandle[bufferCount];
			this.m_RTHandles.Add(bufferId, array);
			array[0] = allocator(this.m_RTHandleSystem, 0);
			int i = 1;
			int num = array.Length;
			while (i < num)
			{
				array[i] = allocator(this.m_RTHandleSystem, i);
				this.m_RTHandleSystem.SwitchResizeMode(array[i], RTHandleSystem.ResizeMode.OnDemand);
				i++;
			}
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0001C10C File Offset: 0x0001A30C
		public void ReleaseBuffer(int bufferId)
		{
			RTHandle[] array;
			if (this.m_RTHandles.TryGetValue(bufferId, out array))
			{
				foreach (RTHandle rthandle in array)
				{
					this.m_RTHandleSystem.Release(rthandle);
				}
			}
			this.m_RTHandles.Remove(bufferId);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0001C156 File Offset: 0x0001A356
		public void SwapAndSetReferenceSize(int width, int height)
		{
			this.Swap();
			this.m_RTHandleSystem.SetReferenceSize(width, height);
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0001C16B File Offset: 0x0001A36B
		public void ResetReferenceSize(int width, int height)
		{
			this.m_RTHandleSystem.ResetReferenceSize(width, height);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001C17A File Offset: 0x0001A37A
		public int GetNumFramesAllocated(int bufferId)
		{
			if (!this.m_RTHandles.ContainsKey(bufferId))
			{
				return 0;
			}
			return this.m_RTHandles[bufferId].Length;
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0001C19C File Offset: 0x0001A39C
		public Vector2 CalculateRatioAgainstMaxSize(int width, int height)
		{
			RTHandleSystem rthandleSystem = this.m_RTHandleSystem;
			Vector2Int vector2Int = new Vector2Int(width, height);
			return rthandleSystem.CalculateRatioAgainstMaxSize(in vector2Int);
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0001C1C0 File Offset: 0x0001A3C0
		private void Swap()
		{
			foreach (KeyValuePair<int, RTHandle[]> keyValuePair in this.m_RTHandles)
			{
				if (keyValuePair.Value.Length > 1)
				{
					RTHandle rthandle = keyValuePair.Value[keyValuePair.Value.Length - 1];
					int i = 0;
					int num = keyValuePair.Value.Length - 1;
					while (i < num)
					{
						keyValuePair.Value[i + 1] = keyValuePair.Value[i];
						i++;
					}
					keyValuePair.Value[0] = rthandle;
					this.m_RTHandleSystem.SwitchResizeMode(keyValuePair.Value[0], RTHandleSystem.ResizeMode.Auto);
					this.m_RTHandleSystem.SwitchResizeMode(keyValuePair.Value[1], RTHandleSystem.ResizeMode.OnDemand);
				}
				else
				{
					this.m_RTHandleSystem.SwitchResizeMode(keyValuePair.Value[0], RTHandleSystem.ResizeMode.Auto);
				}
			}
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001C2AC File Offset: 0x0001A4AC
		private void Dispose(bool disposing)
		{
			if (!this.m_DisposedValue)
			{
				if (disposing)
				{
					this.ReleaseAll();
					this.m_RTHandleSystem.Dispose();
					this.m_RTHandleSystem = null;
				}
				this.m_DisposedValue = true;
			}
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001C2D8 File Offset: 0x0001A4D8
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001C2E4 File Offset: 0x0001A4E4
		public void ReleaseAll()
		{
			foreach (KeyValuePair<int, RTHandle[]> keyValuePair in this.m_RTHandles)
			{
				int i = 0;
				int num = keyValuePair.Value.Length;
				while (i < num)
				{
					this.m_RTHandleSystem.Release(keyValuePair.Value[i]);
					i++;
				}
			}
			this.m_RTHandles.Clear();
		}

		// Token: 0x040003F4 RID: 1012
		private Dictionary<int, RTHandle[]> m_RTHandles = new Dictionary<int, RTHandle[]>();

		// Token: 0x040003F5 RID: 1013
		private RTHandleSystem m_RTHandleSystem = new RTHandleSystem();

		// Token: 0x040003F6 RID: 1014
		private bool m_DisposedValue;
	}
}
