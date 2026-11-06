using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x02000053 RID: 83
	public class DC_SingleSourcesProvider : IDC_SourcesProvider
	{
		// Token: 0x0600020A RID: 522 RVA: 0x0000929B File Offset: 0x0000749B
		public DC_Source GetSource(ICullingTarget cullingTarget)
		{
			DC_SingleSource dc_SingleSource = new GameObject("DC_SingleSource").AddComponent<DC_SingleSource>();
			dc_SingleSource.SetCullingTarget(cullingTarget);
			return dc_SingleSource;
		}
	}
}
