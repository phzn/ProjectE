using System;
using System.Collections.Generic;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x020000CD RID: 205
	[CreateAssetMenu(fileName = "Data", menuName = "MagicaCloth2/PreBuildScriptableObject")]
	public class PreBuildScriptableObject : ScriptableObject
	{
		// Token: 0x060003A3 RID: 931 RVA: 0x00027B91 File Offset: 0x00025D91
		public bool HasPreBuildData(string buildId)
		{
			return this.GetPreBuildData(buildId) != null;
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00027BA0 File Offset: 0x00025DA0
		public SharePreBuildData GetPreBuildData(string buildId)
		{
			foreach (SharePreBuildData sharePreBuildData in this.sharePreBuildDataList)
			{
				if (sharePreBuildData.CheckBuildId(buildId))
				{
					return sharePreBuildData;
				}
			}
			return null;
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00027BFC File Offset: 0x00025DFC
		public void AddPreBuildData(SharePreBuildData sdata)
		{
			int num = this.sharePreBuildDataList.FindIndex((SharePreBuildData x) => x.buildId == sdata.buildId);
			if (num >= 0)
			{
				this.sharePreBuildDataList[num] = sdata;
				return;
			}
			this.sharePreBuildDataList.Add(sdata);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00027C56 File Offset: 0x00025E56
		public void Warmup()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.sharePreBuildDataList.ForEach(delegate(SharePreBuildData sdata)
			{
				MagicaManager.PreBuild.RegisterPreBuildData(sdata, false);
			});
		}

		// Token: 0x040007C1 RID: 1985
		public List<SharePreBuildData> sharePreBuildDataList = new List<SharePreBuildData>();
	}
}
