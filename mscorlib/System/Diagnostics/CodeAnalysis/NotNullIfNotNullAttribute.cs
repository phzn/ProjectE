using System;

namespace System.Diagnostics.CodeAnalysis
{
	// Token: 0x02000A09 RID: 2569
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
	public sealed class NotNullIfNotNullAttribute : Attribute
	{
		// Token: 0x06005B61 RID: 23393 RVA: 0x001349CD File Offset: 0x00132BCD
		public NotNullIfNotNullAttribute(string parameterName)
		{
			this.ParameterName = parameterName;
		}

		// Token: 0x17000FB9 RID: 4025
		// (get) Token: 0x06005B62 RID: 23394 RVA: 0x001349DC File Offset: 0x00132BDC
		public string ParameterName { get; }
	}
}
