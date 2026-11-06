using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000466 RID: 1126
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
	public class vButtonAttribute : PropertyAttribute
	{
		// Token: 0x06001BEC RID: 7148 RVA: 0x000AF6DF File Offset: 0x000AD8DF
		public vButtonAttribute(string label, string function, Type type, bool enabledJustInPlayMode = true)
		{
			this.label = label;
			this.function = function;
			this.type = type;
			this.enabledJustInPlayMode = enabledJustInPlayMode;
		}

		// Token: 0x04001D45 RID: 7493
		public readonly string label;

		// Token: 0x04001D46 RID: 7494
		public readonly string function;

		// Token: 0x04001D47 RID: 7495
		public readonly int id;

		// Token: 0x04001D48 RID: 7496
		public readonly Type type;

		// Token: 0x04001D49 RID: 7497
		public readonly bool enabledJustInPlayMode;
	}
}
