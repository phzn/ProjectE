using System;
using Unity.Properties;

namespace Unity.Serialization.Json
{
	// Token: 0x0200005F RID: 95
	internal abstract class JsonPropertyVisitor
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000A3A5 File Offset: 0x000085A5
		// (set) Token: 0x0600024B RID: 587 RVA: 0x0000A3AD File Offset: 0x000085AD
		internal bool IsLocked { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000A3B6 File Offset: 0x000085B6
		// (set) Token: 0x0600024D RID: 589 RVA: 0x0000A3BE File Offset: 0x000085BE
		internal IProperty Property { get; private set; }

		// Token: 0x0600024E RID: 590 RVA: 0x0000A3C7 File Offset: 0x000085C7
		internal JsonPropertyVisitor.PropertyScope CreatePropertyScope(IProperty property)
		{
			return new JsonPropertyVisitor.PropertyScope(this, property);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000A3D0 File Offset: 0x000085D0
		internal JsonPropertyVisitor.LockScope Lock()
		{
			return new JsonPropertyVisitor.LockScope(this);
		}

		// Token: 0x0400012A RID: 298
		internal const string k_SerializedId = "$id";

		// Token: 0x0400012B RID: 299
		internal const string k_SerializedReferenceKey = "$ref";

		// Token: 0x0400012C RID: 300
		internal const string k_SerializedTypeKey = "$type";

		// Token: 0x0400012D RID: 301
		internal const string k_SerializedVersionKey = "$version";

		// Token: 0x0400012E RID: 302
		internal const string k_SerializedElementsKey = "$elements";

		// Token: 0x02000060 RID: 96
		internal readonly struct LockScope : IDisposable
		{
			// Token: 0x06000251 RID: 593 RVA: 0x0000A3D8 File Offset: 0x000085D8
			public LockScope(JsonPropertyVisitor visitor)
			{
				this.m_Visitor = visitor;
				visitor.IsLocked = true;
			}

			// Token: 0x06000252 RID: 594 RVA: 0x0000A3E8 File Offset: 0x000085E8
			public void Dispose()
			{
				this.m_Visitor.IsLocked = false;
			}

			// Token: 0x04000131 RID: 305
			private readonly JsonPropertyVisitor m_Visitor;
		}

		// Token: 0x02000061 RID: 97
		internal readonly struct PropertyScope : IDisposable
		{
			// Token: 0x06000253 RID: 595 RVA: 0x0000A3F6 File Offset: 0x000085F6
			public PropertyScope(JsonPropertyVisitor visitor, IProperty property)
			{
				this.m_Visitor = visitor;
				this.m_Property = this.m_Visitor.Property;
				this.m_Visitor.Property = property;
			}

			// Token: 0x06000254 RID: 596 RVA: 0x0000A41C File Offset: 0x0000861C
			public void Dispose()
			{
				this.m_Visitor.Property = this.m_Property;
			}

			// Token: 0x04000132 RID: 306
			private readonly JsonPropertyVisitor m_Visitor;

			// Token: 0x04000133 RID: 307
			private readonly IProperty m_Property;
		}
	}
}
