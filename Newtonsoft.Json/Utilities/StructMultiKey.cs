using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200006C RID: 108
	[NullableContext(1)]
	[Nullable(0)]
	internal readonly struct StructMultiKey<[Nullable(2)] T1, [Nullable(2)] T2> : IEquatable<StructMultiKey<T1, T2>>
	{
		// Token: 0x060005DB RID: 1499 RVA: 0x00018841 File Offset: 0x00016A41
		public StructMultiKey(T1 v1, T2 v2)
		{
			this.Value1 = v1;
			this.Value2 = v2;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00018854 File Offset: 0x00016A54
		public override int GetHashCode()
		{
			T1 value = this.Value1;
			int num = ((value != null) ? value.GetHashCode() : 0);
			T2 value2 = this.Value2;
			return num ^ ((value2 != null) ? value2.GetHashCode() : 0);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x000188AC File Offset: 0x00016AAC
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			if (obj is StructMultiKey<T1, T2>)
			{
				StructMultiKey<T1, T2> structMultiKey = (StructMultiKey<T1, T2>)obj;
				return this.Equals(structMultiKey);
			}
			return false;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x000188D3 File Offset: 0x00016AD3
		public bool Equals([Nullable(new byte[] { 0, 1, 1 })] StructMultiKey<T1, T2> other)
		{
			return object.Equals(this.Value1, other.Value1) && object.Equals(this.Value2, other.Value2);
		}

		// Token: 0x04000225 RID: 549
		public readonly T1 Value1;

		// Token: 0x04000226 RID: 550
		public readonly T2 Value2;
	}
}
