using System;

namespace UnityEngine.Localization.SmartFormat.PersistentVariables
{
	// Token: 0x02000059 RID: 89
	public interface IVariableGroup
	{
		// Token: 0x060002A9 RID: 681
		bool TryGetValue(string key, out IVariable value);
	}
}
