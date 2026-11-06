using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000079 RID: 121
	[NullableContext(1)]
	[Nullable(0)]
	public class ErrorEventArgs : EventArgs
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x0001AF27 File Offset: 0x00019127
		[Nullable(2)]
		public object CurrentObject
		{
			[NullableContext(2)]
			get;
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x0001AF2F File Offset: 0x0001912F
		public ErrorContext ErrorContext { get; }

		// Token: 0x0600065E RID: 1630 RVA: 0x0001AF37 File Offset: 0x00019137
		public ErrorEventArgs([Nullable(2)] object currentObject, ErrorContext errorContext)
		{
			this.CurrentObject = currentObject;
			this.ErrorContext = errorContext;
		}
	}
}
