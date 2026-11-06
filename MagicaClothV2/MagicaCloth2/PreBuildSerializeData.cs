using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x020000D0 RID: 208
	[Serializable]
	public class PreBuildSerializeData : ITransform
	{
		// Token: 0x060003AD RID: 941 RVA: 0x00027CD0 File Offset: 0x00025ED0
		public bool UsePreBuild()
		{
			return this.enabled;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00027CD8 File Offset: 0x00025ED8
		public ResultCode DataValidate()
		{
			if (this.uniquePreBuildData == null)
			{
				return new ResultCode(Define.Result.PreBuildData_Empty);
			}
			SharePreBuildData sharePreBuildData = this.GetSharePreBuildData();
			if (sharePreBuildData == null)
			{
				return new ResultCode(Define.Result.PreBuildData_Empty);
			}
			ResultCode resultCode = sharePreBuildData.DataValidate();
			if (resultCode.IsFaild())
			{
				return resultCode;
			}
			resultCode = this.uniquePreBuildData.DataValidate();
			if (resultCode.IsFaild())
			{
				return resultCode;
			}
			return ResultCode.Success;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00027D3B File Offset: 0x00025F3B
		public SharePreBuildData GetSharePreBuildData()
		{
			if (this.preBuildScriptableObject == null)
			{
				return null;
			}
			if (string.IsNullOrEmpty(this.buildId))
			{
				return null;
			}
			return this.preBuildScriptableObject.GetPreBuildData(this.buildId);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00027D70 File Offset: 0x00025F70
		public static string GenerateBuildID()
		{
			return Guid.NewGuid().ToString().Substring(0, 8);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00027D97 File Offset: 0x00025F97
		public void GetUsedTransform(HashSet<Transform> transformSet)
		{
			this.uniquePreBuildData.GetUsedTransform(transformSet);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00027DA5 File Offset: 0x00025FA5
		public void ReplaceTransform(Dictionary<int, Transform> replaceDict)
		{
			this.uniquePreBuildData.ReplaceTransform(replaceDict);
		}

		// Token: 0x040007C5 RID: 1989
		public bool enabled;

		// Token: 0x040007C6 RID: 1990
		public string buildId;

		// Token: 0x040007C7 RID: 1991
		public PreBuildScriptableObject preBuildScriptableObject;

		// Token: 0x040007C8 RID: 1992
		public UniquePreBuildData uniquePreBuildData;
	}
}
