using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Binary
{
	// Token: 0x02000098 RID: 152
	public readonly struct BinarySerializationContext<TValue> : IBinarySerializationContext
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0000FB23 File Offset: 0x0000DD23
		public unsafe UnsafeAppendBuffer* Writer
		{
			get
			{
				return this.m_Visitor.Writer;
			}
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000FB30 File Offset: 0x0000DD30
		internal BinarySerializationContext(BinaryPropertyWriter visitor, BinaryAdapterCollection.Enumerator adapters, TValue value, bool isRoot)
		{
			this.m_Visitor = visitor;
			this.m_Adapters = adapters;
			this.m_Value = value;
			this.m_IsRoot = isRoot;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000FB4F File Offset: 0x0000DD4F
		public void ContinueVisitation()
		{
			this.m_Visitor.WriteValueWithAdapters<TValue>(this.m_Value, this.m_Adapters, this.m_IsRoot);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000FB6E File Offset: 0x0000DD6E
		public void ContinueVisitationWithoutAdapters()
		{
			this.m_Visitor.WriteValueWithoutAdapters<TValue>(this.m_Value, this.m_IsRoot);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000FB87 File Offset: 0x0000DD87
		public void SerializeValue<T>(T value)
		{
			this.m_Visitor.WriteValue<T>(value, false);
		}

		// Token: 0x040001DA RID: 474
		private readonly BinaryPropertyWriter m_Visitor;

		// Token: 0x040001DB RID: 475
		private readonly BinaryAdapterCollection.Enumerator m_Adapters;

		// Token: 0x040001DC RID: 476
		private readonly TValue m_Value;

		// Token: 0x040001DD RID: 477
		private readonly bool m_IsRoot;
	}
}
