using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Properties;

namespace Unity.Serialization.Json
{
	// Token: 0x02000023 RID: 35
	public readonly struct JsonSerializationContext<TValue> : IJsonSerializationContext
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000030A6 File Offset: 0x000012A6
		public JsonWriter Writer
		{
			get
			{
				return this.m_Visitor.Writer;
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000030B3 File Offset: 0x000012B3
		internal JsonSerializationContext(JsonPropertyWriter visitor, JsonAdapterCollection.Enumerator adapters, TValue value, bool isRoot)
		{
			this.m_Visitor = visitor;
			this.m_Adapters = adapters;
			this.m_Value = value;
			this.m_IsRoot = isRoot;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000030D2 File Offset: 0x000012D2
		public void ContinueVisitation()
		{
			this.m_Visitor.WriteValueWithAdapters<TValue>(this.m_Value, this.m_Adapters, this.m_IsRoot);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000030F1 File Offset: 0x000012F1
		public void ContinueVisitationWithoutAdapters()
		{
			this.m_Visitor.WriteValueWithoutAdapters<TValue>(this.m_Value, this.m_IsRoot);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000310C File Offset: 0x0000130C
		public void SerializeValue<T>(T value)
		{
			JsonSerializationContext<TValue>.k_AdapterProperty.DeclaredType = typeof(T);
			using (this.m_Visitor.CreatePropertyScope(JsonSerializationContext<TValue>.k_AdapterProperty))
			{
				this.m_Visitor.WriteValue<T>(ref value, false);
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000316C File Offset: 0x0000136C
		public void SerializeValue<T>(string key, T value)
		{
			JsonSerializationContext<TValue>.k_AdapterProperty.DeclaredType = typeof(T);
			using (this.m_Visitor.CreatePropertyScope(JsonSerializationContext<TValue>.k_AdapterProperty))
			{
				this.m_Visitor.Writer.WriteKey(key);
				this.m_Visitor.WriteValue<T>(ref value, false);
			}
		}

		// Token: 0x04000029 RID: 41
		private static readonly JsonSerializationContext<TValue>.AdapterProperty k_AdapterProperty = new JsonSerializationContext<TValue>.AdapterProperty();

		// Token: 0x0400002A RID: 42
		private readonly JsonPropertyWriter m_Visitor;

		// Token: 0x0400002B RID: 43
		private readonly JsonAdapterCollection.Enumerator m_Adapters;

		// Token: 0x0400002C RID: 44
		private readonly TValue m_Value;

		// Token: 0x0400002D RID: 45
		private readonly bool m_IsRoot;

		// Token: 0x02000024 RID: 36
		private class AdapterProperty : IProperty
		{
			// Token: 0x17000008 RID: 8
			// (get) Token: 0x0600007B RID: 123 RVA: 0x000031EC File Offset: 0x000013EC
			// (set) Token: 0x0600007C RID: 124 RVA: 0x000031F4 File Offset: 0x000013F4
			public Type DeclaredType { get; set; }

			// Token: 0x17000009 RID: 9
			// (get) Token: 0x0600007D RID: 125 RVA: 0x000031FD File Offset: 0x000013FD
			public string Name
			{
				get
				{
					return string.Empty;
				}
			}

			// Token: 0x1700000A RID: 10
			// (get) Token: 0x0600007E RID: 126 RVA: 0x00003204 File Offset: 0x00001404
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x0600007F RID: 127 RVA: 0x00003207 File Offset: 0x00001407
			public Type DeclaredValueType()
			{
				return this.DeclaredType;
			}

			// Token: 0x06000080 RID: 128 RVA: 0x0000262E File Offset: 0x0000082E
			public bool HasAttribute<TAttribute>() where TAttribute : Attribute
			{
				return false;
			}

			// Token: 0x06000081 RID: 129 RVA: 0x00003210 File Offset: 0x00001410
			public TAttribute GetAttribute<TAttribute>() where TAttribute : Attribute
			{
				return default(TAttribute);
			}

			// Token: 0x06000082 RID: 130 RVA: 0x00003226 File Offset: 0x00001426
			public IEnumerable<TAttribute> GetAttributes<TAttribute>() where TAttribute : Attribute
			{
				return Enumerable.Empty<TAttribute>();
			}

			// Token: 0x06000083 RID: 131 RVA: 0x0000322D File Offset: 0x0000142D
			public IEnumerable<Attribute> GetAttributes()
			{
				return Enumerable.Empty<Attribute>();
			}
		}
	}
}
