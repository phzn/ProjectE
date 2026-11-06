using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x020000D1 RID: 209
	[Serializable]
	public class SharePreBuildData
	{
		// Token: 0x060003B4 RID: 948 RVA: 0x00027DB4 File Offset: 0x00025FB4
		public ResultCode DataValidate()
		{
			if (this.version != 2)
			{
				return new ResultCode(Define.Result.PreBuildData_VersionMismatch);
			}
			if (this.buildScale.x < 1E-08f)
			{
				return new ResultCode(Define.Result.PreBuildData_InvalidScale);
			}
			if (this.buildResult.IsFaild())
			{
				return this.buildResult;
			}
			return ResultCode.Success;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00027E0B File Offset: 0x0002600B
		public bool CheckBuildId(string buildId)
		{
			return !string.IsNullOrEmpty(buildId) && this.buildId == buildId;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00027E24 File Offset: 0x00026024
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			stringBuilder.AppendLine("<<<<< PreBuildData >>>>>");
			stringBuilder.AppendLine(string.Format("Version:{0}", this.version));
			stringBuilder.AppendLine("BuildID:" + this.buildId);
			stringBuilder.AppendLine("BuildResult:" + this.buildResult.GetResultString());
			stringBuilder.AppendLine(string.Format("BuildScale:{0}", this.buildScale));
			stringBuilder.AppendLine(this.proxyMesh.ToString());
			stringBuilder.AppendLine(string.Format("renderMeshList:{0}", this.renderMeshList.Count));
			stringBuilder.AppendLine(string.Format("renderSetupDataList:{0}", this.renderSetupDataList.Count));
			return stringBuilder.ToString();
		}

		// Token: 0x040007C9 RID: 1993
		public int version;

		// Token: 0x040007CA RID: 1994
		public string buildId;

		// Token: 0x040007CB RID: 1995
		public ResultCode buildResult;

		// Token: 0x040007CC RID: 1996
		public Vector3 buildScale;

		// Token: 0x040007CD RID: 1997
		public List<RenderSetupData.ShareSerializationData> renderSetupDataList = new List<RenderSetupData.ShareSerializationData>();

		// Token: 0x040007CE RID: 1998
		public VirtualMesh.ShareSerializationData proxyMesh;

		// Token: 0x040007CF RID: 1999
		public List<VirtualMesh.ShareSerializationData> renderMeshList = new List<VirtualMesh.ShareSerializationData>();

		// Token: 0x040007D0 RID: 2000
		public DistanceConstraint.ConstraintData distanceConstraintData;

		// Token: 0x040007D1 RID: 2001
		public TriangleBendingConstraint.ConstraintData bendingConstraintData;

		// Token: 0x040007D2 RID: 2002
		public InertiaConstraint.ConstraintData inertiaConstraintData;
	}
}
