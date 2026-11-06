using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;

namespace System.Collections.Generic
{
	// Token: 0x02000ACA RID: 2762
	[Serializable]
	internal class EnumEqualityComparer<T> : EqualityComparer<T>, ISerializable where T : struct
	{
		// Token: 0x060062AD RID: 25261 RVA: 0x0014A414 File Offset: 0x00148614
		public override bool Equals(T x, T y)
		{
			int num = JitHelpers.UnsafeEnumCast<T>(x);
			int num2 = JitHelpers.UnsafeEnumCast<T>(y);
			return num == num2;
		}

		// Token: 0x060062AE RID: 25262 RVA: 0x0014A434 File Offset: 0x00148634
		public override int GetHashCode(T obj)
		{
			return JitHelpers.UnsafeEnumCast<T>(obj).GetHashCode();
		}

		// Token: 0x060062AF RID: 25263 RVA: 0x0014A0B9 File Offset: 0x001482B9
		public EnumEqualityComparer()
		{
		}

		// Token: 0x060062B0 RID: 25264 RVA: 0x0014A0B9 File Offset: 0x001482B9
		protected EnumEqualityComparer(SerializationInfo information, StreamingContext context)
		{
		}

		// Token: 0x060062B1 RID: 25265 RVA: 0x0014A44F File Offset: 0x0014864F
		[SecurityCritical]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (Type.GetTypeCode(Enum.GetUnderlyingType(typeof(T))) != TypeCode.Int32)
			{
				info.SetType(typeof(ObjectEqualityComparer<T>));
			}
		}

		// Token: 0x060062B2 RID: 25266 RVA: 0x0014A479 File Offset: 0x00148679
		public override bool Equals(object obj)
		{
			return obj is EnumEqualityComparer<T>;
		}

		// Token: 0x060062B3 RID: 25267 RVA: 0x00149C92 File Offset: 0x00147E92
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}
