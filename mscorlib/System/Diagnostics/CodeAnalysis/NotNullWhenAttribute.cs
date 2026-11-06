using System;

namespace System.Diagnostics.CodeAnalysis
{
	// Token: 0x02000A08 RID: 2568
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	public sealed class NotNullWhenAttribute : Attribute
	{
		// Token: 0x06005B5F RID: 23391 RVA: 0x001349B6 File Offset: 0x00132BB6
		public NotNullWhenAttribute(bool returnValue)
		{
			this.ReturnValue = returnValue;
		}

		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x06005B60 RID: 23392 RVA: 0x001349C5 File Offset: 0x00132BC5
		public bool ReturnValue { get; }
	}
}
