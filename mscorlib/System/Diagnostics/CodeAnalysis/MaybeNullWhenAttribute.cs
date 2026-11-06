using System;

namespace System.Diagnostics.CodeAnalysis
{
	// Token: 0x02000A07 RID: 2567
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	public sealed class MaybeNullWhenAttribute : Attribute
	{
		// Token: 0x06005B5D RID: 23389 RVA: 0x0013499F File Offset: 0x00132B9F
		public MaybeNullWhenAttribute(bool returnValue)
		{
			this.ReturnValue = returnValue;
		}

		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x06005B5E RID: 23390 RVA: 0x001349AE File Offset: 0x00132BAE
		public bool ReturnValue { get; }
	}
}
