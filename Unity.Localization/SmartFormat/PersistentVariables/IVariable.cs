using System;
using UnityEngine.Localization.SmartFormat.Core.Extensions;

namespace UnityEngine.Localization.SmartFormat.PersistentVariables
{
	// Token: 0x0200005A RID: 90
	public interface IVariable
	{
		// Token: 0x060002AA RID: 682
		object GetSourceValue(ISelectorInfo selector);
	}
}
