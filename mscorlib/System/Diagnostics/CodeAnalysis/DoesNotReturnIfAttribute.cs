using System;

namespace System.Diagnostics.CodeAnalysis
{
	// Token: 0x02000A0B RID: 2571
	[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
	public sealed class DoesNotReturnIfAttribute : Attribute
	{
		// Token: 0x06005B64 RID: 23396 RVA: 0x001349E4 File Offset: 0x00132BE4
		public DoesNotReturnIfAttribute(bool parameterValue)
		{
			this.ParameterValue = parameterValue;
		}

		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x06005B65 RID: 23397 RVA: 0x001349F3 File Offset: 0x00132BF3
		public bool ParameterValue { get; }
	}
}
