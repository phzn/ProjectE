using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000065 RID: 101
	[NullableContext(2)]
	[Nullable(0)]
	internal class ReflectionMember
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x00016DCE File Offset: 0x00014FCE
		// (set) Token: 0x06000574 RID: 1396 RVA: 0x00016DD6 File Offset: 0x00014FD6
		public Type MemberType { get; set; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x00016DDF File Offset: 0x00014FDF
		// (set) Token: 0x06000576 RID: 1398 RVA: 0x00016DE7 File Offset: 0x00014FE7
		[Nullable(new byte[] { 2, 1, 2 })]
		public Func<object, object> Getter
		{
			[return: Nullable(new byte[] { 2, 1, 2 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 2 })]
			set;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x00016DF0 File Offset: 0x00014FF0
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x00016DF8 File Offset: 0x00014FF8
		[Nullable(new byte[] { 2, 1, 2 })]
		public Action<object, object> Setter
		{
			[return: Nullable(new byte[] { 2, 1, 2 })]
			get;
			[param: Nullable(new byte[] { 2, 1, 2 })]
			set;
		}
	}
}
