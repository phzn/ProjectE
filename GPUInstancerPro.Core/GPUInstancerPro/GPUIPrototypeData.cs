using System;
using Unity.Collections;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000010 RID: 16
	public abstract class GPUIPrototypeData : IGPUIDisposable, IDisposable, IGPUIParameterBufferData
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00003558 File Offset: 0x00001758
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00003560 File Offset: 0x00001760
		public bool IsInitialized { get; protected set; }

		// Token: 0x0600007E RID: 126 RVA: 0x00003569 File Offset: 0x00001769
		public virtual bool IsValid(bool logError, GPUIPrototype prototype)
		{
			return true;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000356C File Offset: 0x0000176C
		public virtual bool Initialize(GPUIPrototype prototype)
		{
			if (this.IsValid(Application.isPlaying, prototype))
			{
				this.IsInitialized = true;
				this.SetParameterBufferData();
				return true;
			}
			return false;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual void ReleaseBuffers()
		{
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000358C File Offset: 0x0000178C
		public virtual void Dispose()
		{
			this.ReleaseBuffers();
			this.IsInitialized = false;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual void FillRequiredFields()
		{
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual void SetParameterBufferData()
		{
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000359B File Offset: 0x0000179B
		public virtual bool TryGetParameterBufferIndex(out int index)
		{
			return GPUIRenderingSystem.Instance.ParameterBufferIndexes.TryGetValue(this, out index);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000035B0 File Offset: 0x000017B0
		public virtual NativeArray<Matrix4x4> GetTransformationMatrixArray()
		{
			return default(NativeArray<Matrix4x4>);
		}
	}
}
