using System;

namespace UnityEngine.Localization.SmartFormat.PersistentVariables
{
	// Token: 0x0200005C RID: 92
	public interface IVariableValueChanged : IVariable
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060002AC RID: 684
		// (remove) Token: 0x060002AD RID: 685
		event Action<IVariable> ValueChanged;
	}
}
