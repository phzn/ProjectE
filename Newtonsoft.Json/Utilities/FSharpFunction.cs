using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000059 RID: 89
	[NullableContext(2)]
	[Nullable(0)]
	internal class FSharpFunction
	{
		// Token: 0x06000512 RID: 1298 RVA: 0x0001561C File Offset: 0x0001381C
		public FSharpFunction(object instance, [Nullable(new byte[] { 1, 2, 1 })] MethodCall<object, object> invoker)
		{
			this._instance = instance;
			this._invoker = invoker;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00015632 File Offset: 0x00013832
		[NullableContext(1)]
		public object Invoke(params object[] args)
		{
			return this._invoker(this._instance, args);
		}

		// Token: 0x040001E5 RID: 485
		private readonly object _instance;

		// Token: 0x040001E6 RID: 486
		[Nullable(new byte[] { 1, 2, 1 })]
		private readonly MethodCall<object, object> _invoker;
	}
}
