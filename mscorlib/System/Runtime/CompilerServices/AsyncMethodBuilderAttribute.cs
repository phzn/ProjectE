using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020007DB RID: 2011
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false, AllowMultiple = false)]
	public sealed class AsyncMethodBuilderAttribute : Attribute
	{
		// Token: 0x060045C1 RID: 17857 RVA: 0x000E51CF File Offset: 0x000E33CF
		public AsyncMethodBuilderAttribute(Type builderType)
		{
			this.BuilderType = builderType;
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x060045C2 RID: 17858 RVA: 0x000E51DE File Offset: 0x000E33DE
		public Type BuilderType { get; }
	}
}
