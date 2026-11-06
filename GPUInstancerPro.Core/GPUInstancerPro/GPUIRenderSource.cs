using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x0200002B RID: 43
	public class GPUIRenderSource : IGPUIDisposable, IDisposable
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00009218 File Offset: 0x00007418
		// (set) Token: 0x06000194 RID: 404 RVA: 0x00009220 File Offset: 0x00007420
		public int Key { get; private set; }

		// Token: 0x06000195 RID: 405 RVA: 0x00009229 File Offset: 0x00007429
		public GPUIRenderSource(Object source, GPUIRenderSourceGroup renderSourceGroup)
		{
			this.source = source;
			this.renderSourceGroup = renderSourceGroup;
			this.Key = GPUIRenderSource.GetKey(source, renderSourceGroup);
			this.bufferStartIndex = -1;
			this.bufferSize = 0;
			this.instanceCount = 0;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00009261 File Offset: 0x00007461
		public void SetBufferSize(int bufferSize, bool isCopyPreviousData)
		{
			this.renderSourceGroup.SetBufferSize(this, bufferSize, isCopyPreviousData);
			if (this.instanceCount < 0 || this.instanceCount > bufferSize)
			{
				this.SetInstanceCount(bufferSize);
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000928A File Offset: 0x0000748A
		public void SetInstanceCount(int instanceCount)
		{
			this.renderSourceGroup.SetInstanceCount(this, instanceCount);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00009299 File Offset: 0x00007499
		public void SetTransformBufferData<[IsUnmanaged] T>(NativeArray<T> matrices, int managedBufferStartIndex, int graphicsBufferStartIndex, int count, bool isOverwritePreviousFrameBuffer) where T : struct, ValueType
		{
			this.renderSourceGroup.SetTransformBufferData<T>(this, matrices, managedBufferStartIndex, graphicsBufferStartIndex, count, isOverwritePreviousFrameBuffer);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000092AE File Offset: 0x000074AE
		public void SetTransformBufferData<[IsUnmanaged] T>(T[] matrices, int managedBufferStartIndex, int graphicsBufferStartIndex, int count, bool isOverwritePreviousFrameBuffer) where T : struct, ValueType
		{
			this.renderSourceGroup.SetTransformBufferData<T>(this, matrices, managedBufferStartIndex, graphicsBufferStartIndex, count, isOverwritePreviousFrameBuffer);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000092C3 File Offset: 0x000074C3
		public void SetTransformBufferData<[IsUnmanaged] T>(List<T> matrices, int managedBufferStartIndex, int graphicsBufferStartIndex, int count, bool isOverwritePreviousFrameBuffer) where T : struct, ValueType
		{
			this.renderSourceGroup.SetTransformBufferData<T>(this, matrices, managedBufferStartIndex, graphicsBufferStartIndex, count, isOverwritePreviousFrameBuffer);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x000092D8 File Offset: 0x000074D8
		public void Dispose()
		{
			if (this.isDisposed)
			{
				return;
			}
			this.renderSourceGroup.Dispose(this);
			this.DisposeRenderSource();
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000092F8 File Offset: 0x000074F8
		internal void DisposeRenderSource()
		{
			if (this.isDisposed)
			{
				return;
			}
			this.isDisposed = true;
			if (GPUIRenderingSystem.IsActive)
			{
				GPUIRenderingSystem.Instance.RenderSourceProvider.Remove(this.Key);
			}
			this.bufferStartIndex = -1;
			this.bufferSize = 0;
			this.instanceCount = 0;
			GPUIManager gpuimanager = this.source as GPUIManager;
			if (gpuimanager != null && gpuimanager.IsInitialized)
			{
				gpuimanager.OnRenderSourceDisposed(this.Key);
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000936C File Offset: 0x0000756C
		public void ReleaseBuffers()
		{
			IGPUIDisposable igpuidisposable = this.source as IGPUIDisposable;
			if (igpuidisposable != null)
			{
				igpuidisposable.ReleaseBuffers();
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000938E File Offset: 0x0000758E
		public static int GetKey(Object source, GPUIRenderSourceGroup renderSourceGroup)
		{
			return GPUIUtility.GenerateHash(new int[]
			{
				source.GetInstanceIDGPUI(),
				renderSourceGroup.Key
			});
		}

		// Token: 0x04000103 RID: 259
		public GPUIRenderSourceGroup renderSourceGroup;

		// Token: 0x04000104 RID: 260
		public Object source;

		// Token: 0x04000105 RID: 261
		public int bufferStartIndex;

		// Token: 0x04000106 RID: 262
		public int bufferSize;

		// Token: 0x04000107 RID: 263
		public int instanceCount;

		// Token: 0x04000108 RID: 264
		public bool isDisposed;
	}
}
