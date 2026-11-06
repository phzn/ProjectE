using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x0200003D RID: 61
	public class GPUIRenderSourceProvider : GPUIDataProvider<int, GPUIRenderSource>
	{
		// Token: 0x0600024E RID: 590 RVA: 0x0000C774 File Offset: 0x0000A974
		public override void Dispose()
		{
			if (this._dataDict != null)
			{
				foreach (GPUIRenderSource gpuirenderSource in base.Values)
				{
					if (gpuirenderSource != null)
					{
						gpuirenderSource.Dispose();
					}
				}
			}
			base.Dispose();
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000C7D8 File Offset: 0x0000A9D8
		internal bool TryCreateRenderSource(Object source, GPUIRenderSourceGroup renderSourceGroup, out GPUIRenderSource renderSource)
		{
			renderSource = new GPUIRenderSource(source, renderSourceGroup);
			if (renderSourceGroup.AddRenderSource(renderSource))
			{
				this.AddOrSet(renderSource.Key, renderSource);
				GPUIRenderingSystem.Instance.OnCreatedRenderSource(renderSource);
				return true;
			}
			return false;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000C80C File Offset: 0x0000AA0C
		internal void DisposeRenderer(int renderKey)
		{
			GPUIRenderSource gpuirenderSource;
			if (this.TryGetData(renderKey, out gpuirenderSource))
			{
				this.Remove(renderKey);
				gpuirenderSource.Dispose();
				GPUIRenderingSystem.Instance.OnRemovedRenderSource(renderKey);
			}
		}
	}
}
