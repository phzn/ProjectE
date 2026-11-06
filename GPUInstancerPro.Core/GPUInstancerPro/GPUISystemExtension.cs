using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000055 RID: 85
	public abstract class GPUISystemExtension : MonoBehaviour, IGPUIDisposable, IDisposable
	{
		// Token: 0x06000334 RID: 820
		public abstract void Dispose();

		// Token: 0x06000335 RID: 821 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual void ReleaseBuffers()
		{
		}

		// Token: 0x06000336 RID: 822 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual void OnCreatedRenderSourceGroup(GPUIRenderSourceGroup renderSourceGroup)
		{
		}

		// Token: 0x06000337 RID: 823 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual void OnRemovedRenderSourceGroup(int renderSourceGroupKey)
		{
		}

		// Token: 0x06000338 RID: 824 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual void OnRenderSourceGroupBufferSizeChanged(GPUIRenderSourceGroup renderSourceGroup)
		{
		}

		// Token: 0x06000339 RID: 825 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual void OnCreatedRenderSource(GPUIRenderSource renderSource)
		{
		}

		// Token: 0x0600033A RID: 826 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual void OnRemovedRenderSource(int key)
		{
		}

		// Token: 0x0600033B RID: 827 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual void OnRenderSourceBufferSizeChanged(GPUIRenderSource renderSource, int previousBufferSize)
		{
		}

		// Token: 0x0600033C RID: 828 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual void ExecuteOnPreCull(GPUICameraData cameraData)
		{
		}

		// Token: 0x0600033D RID: 829 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual void ExecuteOnPreRender(GPUICameraData cameraData)
		{
		}

		// Token: 0x0600033E RID: 830 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual void ExecuteOnPostRender(GPUICameraData cameraData)
		{
		}
	}
}
