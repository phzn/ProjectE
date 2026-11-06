using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Binary
{
	// Token: 0x0200009A RID: 154
	public readonly struct BinaryDeserializationContext<TValue> : IBinaryDeserializationContext
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060003C9 RID: 969 RVA: 0x0000FB96 File Offset: 0x0000DD96
		public unsafe UnsafeAppendBuffer.Reader* Reader
		{
			get
			{
				return this.m_Visitor.Reader;
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000FBA3 File Offset: 0x0000DDA3
		internal BinaryDeserializationContext(BinaryPropertyReader visitor, BinaryAdapterCollection.Enumerator adapters, bool isRoot)
		{
			this.m_Visitor = visitor;
			this.m_Adapters = adapters;
			this.m_IsRoot = isRoot;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000FBBC File Offset: 0x0000DDBC
		public TValue ContinueVisitation()
		{
			TValue tvalue = default(TValue);
			this.m_Visitor.ReadValueWithAdapters<TValue>(ref tvalue, this.m_Adapters, this.m_IsRoot);
			return tvalue;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000FBEB File Offset: 0x0000DDEB
		public void ContinueVisitation(ref TValue value)
		{
			this.m_Visitor.ReadValueWithAdapters<TValue>(ref value, this.m_Adapters, this.m_IsRoot);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000FC08 File Offset: 0x0000DE08
		public TValue ContinueVisitationWithoutAdapters()
		{
			TValue tvalue = default(TValue);
			this.m_Visitor.ReadValueWithoutAdapters<TValue>(ref tvalue, this.m_IsRoot);
			return tvalue;
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000FC31 File Offset: 0x0000DE31
		public void ContinueVisitationWithoutAdapters(ref TValue value)
		{
			this.m_Visitor.ReadValueWithoutAdapters<TValue>(ref value, this.m_IsRoot);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000FC45 File Offset: 0x0000DE45
		object IBinaryDeserializationContext.ContinueVisitation()
		{
			return this.ContinueVisitation();
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000FC52 File Offset: 0x0000DE52
		object IBinaryDeserializationContext.ContinueVisitationWithoutAdapters()
		{
			return this.ContinueVisitationWithoutAdapters();
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000FC60 File Offset: 0x0000DE60
		public T DeserializeValue<T>()
		{
			T t = default(T);
			this.m_Visitor.ReadValue<T>(ref t, false);
			return t;
		}

		// Token: 0x040001DE RID: 478
		private readonly BinaryPropertyReader m_Visitor;

		// Token: 0x040001DF RID: 479
		private readonly BinaryAdapterCollection.Enumerator m_Adapters;

		// Token: 0x040001E0 RID: 480
		private readonly bool m_IsRoot;
	}
}
