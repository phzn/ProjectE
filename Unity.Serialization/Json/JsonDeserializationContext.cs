using System;
using Unity.Serialization.Json.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x02000026 RID: 38
	public readonly struct JsonDeserializationContext<TValue> : IJsonDeserializationContext
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003234 File Offset: 0x00001434
		public SerializedValueView SerializedValue
		{
			get
			{
				return this.m_View.AsSafe();
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003241 File Offset: 0x00001441
		internal JsonDeserializationContext(JsonPropertyReader visitor, JsonAdapterCollection.Enumerator adapters, TValue value, UnsafeValueView view, bool isRoot)
		{
			this.m_Visitor = visitor;
			this.m_Adapters = adapters;
			this.m_Value = value;
			this.m_View = view;
			this.m_IsRoot = isRoot;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003268 File Offset: 0x00001468
		object IJsonDeserializationContext.GetInstance()
		{
			return this.GetInstance();
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003275 File Offset: 0x00001475
		public TValue GetInstance()
		{
			return this.m_Value;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003280 File Offset: 0x00001480
		public TValue ContinueVisitation()
		{
			TValue tvalue = default(TValue);
			this.m_Visitor.ReadValueWithAdapters<TValue>(ref tvalue, this.m_View, this.m_Adapters, this.m_IsRoot);
			return tvalue;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000032B8 File Offset: 0x000014B8
		public TValue ContinueVisitation(SerializedValueView view)
		{
			TValue tvalue = default(TValue);
			this.m_Visitor.ReadValueWithAdapters<TValue>(ref tvalue, view.AsUnsafe(), this.m_Adapters, this.m_IsRoot);
			return tvalue;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000032EE File Offset: 0x000014EE
		public void ContinueVisitation(ref TValue value)
		{
			this.m_Visitor.ReadValueWithAdapters<TValue>(ref value, this.m_View, this.m_Adapters, this.m_IsRoot);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x0000330E File Offset: 0x0000150E
		public void ContinueVisitation(ref TValue value, SerializedValueView view)
		{
			this.m_Visitor.ReadValueWithAdapters<TValue>(ref value, view.AsUnsafe(), this.m_Adapters, this.m_IsRoot);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003330 File Offset: 0x00001530
		public TValue ContinueVisitationWithoutAdapters()
		{
			TValue tvalue = default(TValue);
			this.m_Visitor.ReadValueWithoutAdapters<TValue>(ref tvalue, this.m_View, this.m_IsRoot);
			return tvalue;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003360 File Offset: 0x00001560
		public TValue ContinueVisitationWithoutAdapters(SerializedValueView view)
		{
			TValue tvalue = default(TValue);
			this.m_Visitor.ReadValueWithoutAdapters<TValue>(ref tvalue, view.AsUnsafe(), this.m_IsRoot);
			return tvalue;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003390 File Offset: 0x00001590
		public void ContinueVisitationWithoutAdapters(ref TValue value)
		{
			this.m_Visitor.ReadValueWithoutAdapters<TValue>(ref value, this.m_View, this.m_IsRoot);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000033AA File Offset: 0x000015AA
		public void ContinueVisitationWithoutAdapters(ref TValue value, SerializedValueView view)
		{
			this.m_Visitor.ReadValueWithoutAdapters<TValue>(ref value, view.AsUnsafe(), this.m_IsRoot);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x000033C5 File Offset: 0x000015C5
		object IJsonDeserializationContext.ContinueVisitation()
		{
			return this.ContinueVisitation();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000033D2 File Offset: 0x000015D2
		object IJsonDeserializationContext.ContinueVisitationWithoutAdapters()
		{
			return this.ContinueVisitationWithoutAdapters();
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000033E0 File Offset: 0x000015E0
		public T DeserializeValue<T>(SerializedValueView view)
		{
			T t = default(T);
			this.m_Visitor.ReadValue<T>(ref t, view.AsUnsafe(), false);
			return t;
		}

		// Token: 0x0400002F RID: 47
		private readonly JsonPropertyReader m_Visitor;

		// Token: 0x04000030 RID: 48
		private readonly JsonAdapterCollection.Enumerator m_Adapters;

		// Token: 0x04000031 RID: 49
		private readonly TValue m_Value;

		// Token: 0x04000032 RID: 50
		private readonly UnsafeValueView m_View;

		// Token: 0x04000033 RID: 51
		private readonly bool m_IsRoot;
	}
}
