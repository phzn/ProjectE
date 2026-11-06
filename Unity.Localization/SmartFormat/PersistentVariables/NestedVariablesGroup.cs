using System;

namespace UnityEngine.Localization.SmartFormat.PersistentVariables
{
	// Token: 0x0200005D RID: 93
	[DisplayName("Nested Variables Group", null)]
	[Serializable]
	public class NestedVariablesGroup : Variable<VariablesGroupAsset>, IVariableGroup
	{
		// Token: 0x060002AE RID: 686 RVA: 0x0000A285 File Offset: 0x00008485
		public bool TryGetValue(string name, out IVariable value)
		{
			if (base.Value != null)
			{
				return base.Value.TryGetValue(name, out value);
			}
			value = null;
			return false;
		}
	}
}
