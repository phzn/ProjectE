using System;

namespace UnityEngine.Localization.SmartFormat.Extensions
{
	// Token: 0x0200006F RID: 111
	[Obsolete("Please use PersistentVariablesSource instead (UnityUpgradable) -> PersistentVariablesSource")]
	[Serializable]
	public class GlobalVariablesSource : PersistentVariablesSource
	{
		// Token: 0x060002E3 RID: 739 RVA: 0x0000B1A4 File Offset: 0x000093A4
		public GlobalVariablesSource(SmartFormatter formatter)
			: base(formatter)
		{
		}
	}
}
