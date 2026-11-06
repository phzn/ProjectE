using System;
using System.Collections.Generic;

namespace GPUInstancerPro
{
	// Token: 0x0200003C RID: 60
	public class GPUIRenderSourceGroupProvider : GPUIDataProvider<int, GPUIRenderSourceGroup>
	{
		// Token: 0x06000249 RID: 585 RVA: 0x0000C624 File Offset: 0x0000A824
		public override void Dispose()
		{
			if (this._dataDict != null)
			{
				foreach (GPUIRenderSourceGroup gpuirenderSourceGroup in base.Values)
				{
					if (gpuirenderSourceGroup != null)
					{
						gpuirenderSourceGroup.Dispose();
					}
				}
			}
			base.Dispose();
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000C688 File Offset: 0x0000A888
		public override bool Remove(int key)
		{
			GPUIRenderingSystem.Instance.OnRemovedRenderSourceGroup(key);
			return base.Remove(key);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000C69C File Offset: 0x0000A89C
		internal GPUIRenderSourceGroup GetOrCreateRenderSourceGroup(int prototypeKey, GPUILODGroupData lodGroupData, GPUIProfile profile, int groupID = 0, GPUITransformBufferType transformBufferType = GPUITransformBufferType.Default, List<string> shaderKeywords = null)
		{
			int key = GPUIRenderSourceGroup.GetKey(prototypeKey, profile, groupID, shaderKeywords);
			GPUIRenderSourceGroup gpuirenderSourceGroup;
			if (!this.TryGetData(key, out gpuirenderSourceGroup))
			{
				gpuirenderSourceGroup = new GPUIRenderSourceGroup(prototypeKey, profile, groupID, transformBufferType, shaderKeywords, lodGroupData);
				this._dataDict.Add(key, gpuirenderSourceGroup);
				profile.SetParameterBufferData();
				lodGroupData.SetParameterBufferData();
				GPUIRenderingSystem.Instance.UpdateCommandBuffers(gpuirenderSourceGroup);
				GPUIRenderingSystem.Instance.OnCreatedRenderSourceGroup(gpuirenderSourceGroup);
			}
			return gpuirenderSourceGroup;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000C700 File Offset: 0x0000A900
		internal void DisposeCameraData(GPUICameraData cameraData)
		{
			if (this._dataDict != null)
			{
				foreach (GPUIRenderSourceGroup gpuirenderSourceGroup in base.Values)
				{
					if (gpuirenderSourceGroup != null && gpuirenderSourceGroup.TransformBufferData != null)
					{
						gpuirenderSourceGroup.TransformBufferData.Dispose(cameraData);
					}
				}
			}
		}
	}
}
