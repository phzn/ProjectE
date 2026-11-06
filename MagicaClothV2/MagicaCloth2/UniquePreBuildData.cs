using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x020000D2 RID: 210
	[Serializable]
	public class UniquePreBuildData : ITransform
	{
		// Token: 0x060003B8 RID: 952 RVA: 0x00027F29 File Offset: 0x00026129
		public ResultCode DataValidate()
		{
			if (this.version != 2)
			{
				return new ResultCode(Define.Result.PreBuildData_VersionMismatch);
			}
			if (this.buildResult.IsFaild())
			{
				return this.buildResult;
			}
			return ResultCode.Success;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00027F58 File Offset: 0x00026158
		public void GetUsedTransform(HashSet<Transform> transformSet)
		{
			this.renderSetupDataList.ForEach(delegate(RenderSetupData.UniqueSerializationData x)
			{
				if (x != null)
				{
					x.GetUsedTransform(transformSet);
				}
			});
			VirtualMesh.UniqueSerializationData uniqueSerializationData = this.proxyMesh;
			if (uniqueSerializationData != null)
			{
				uniqueSerializationData.GetUsedTransform(transformSet);
			}
			this.renderMeshList.ForEach(delegate(VirtualMesh.UniqueSerializationData x)
			{
				if (x != null)
				{
					x.GetUsedTransform(transformSet);
				}
			});
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00027FB8 File Offset: 0x000261B8
		public void ReplaceTransform(Dictionary<int, Transform> replaceDict)
		{
			this.renderSetupDataList.ForEach(delegate(RenderSetupData.UniqueSerializationData x)
			{
				if (x != null)
				{
					x.ReplaceTransform(replaceDict);
				}
			});
			VirtualMesh.UniqueSerializationData uniqueSerializationData = this.proxyMesh;
			if (uniqueSerializationData != null)
			{
				uniqueSerializationData.ReplaceTransform(replaceDict);
			}
			this.renderMeshList.ForEach(delegate(VirtualMesh.UniqueSerializationData x)
			{
				if (x != null)
				{
					x.ReplaceTransform(replaceDict);
				}
			});
		}

		// Token: 0x040007D3 RID: 2003
		public int version;

		// Token: 0x040007D4 RID: 2004
		public ResultCode buildResult;

		// Token: 0x040007D5 RID: 2005
		public List<RenderSetupData.UniqueSerializationData> renderSetupDataList = new List<RenderSetupData.UniqueSerializationData>();

		// Token: 0x040007D6 RID: 2006
		public VirtualMesh.UniqueSerializationData proxyMesh;

		// Token: 0x040007D7 RID: 2007
		public List<VirtualMesh.UniqueSerializationData> renderMeshList = new List<VirtualMesh.UniqueSerializationData>();
	}
}
