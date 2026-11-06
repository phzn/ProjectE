using System;

namespace Unity.Properties
{
	// Token: 0x02000064 RID: 100
	public readonly struct ExcludeContext<TContainer, TValue>
	{
		// Token: 0x060001BF RID: 447 RVA: 0x00006618 File Offset: 0x00004818
		internal static ExcludeContext<TContainer, TValue> FromProperty(PropertyVisitor visitor, Property<TContainer, TValue> property)
		{
			return new ExcludeContext<TContainer, TValue>(visitor, property);
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00006631 File Offset: 0x00004831
		public Property<TContainer, TValue> Property { get; }

		// Token: 0x060001C1 RID: 449 RVA: 0x00006639 File Offset: 0x00004839
		private ExcludeContext(PropertyVisitor visitor, Property<TContainer, TValue> property)
		{
			this.m_Visitor = visitor;
			this.Property = property;
		}

		// Token: 0x0400008F RID: 143
		private readonly PropertyVisitor m_Visitor;
	}
}
