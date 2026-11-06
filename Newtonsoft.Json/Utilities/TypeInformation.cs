using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000049 RID: 73
	[NullableContext(1)]
	[Nullable(0)]
	internal class TypeInformation
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x00011432 File Offset: 0x0000F632
		public Type Type { get; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x0001143A File Offset: 0x0000F63A
		public PrimitiveTypeCode TypeCode { get; }

		// Token: 0x06000474 RID: 1140 RVA: 0x00011442 File Offset: 0x0000F642
		public TypeInformation(Type type, PrimitiveTypeCode typeCode)
		{
			this.Type = type;
			this.TypeCode = typeCode;
		}
	}
}
