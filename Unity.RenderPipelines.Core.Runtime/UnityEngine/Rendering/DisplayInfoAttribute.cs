using System;

namespace UnityEngine.Rendering
{
	// Token: 0x0200003B RID: 59
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = false)]
	public class DisplayInfoAttribute : Attribute
	{
		// Token: 0x04000142 RID: 322
		public string name;

		// Token: 0x04000143 RID: 323
		public int order;
	}
}
