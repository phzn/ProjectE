using System;

namespace Unity.Properties
{
	// Token: 0x02000065 RID: 101
	public readonly struct ExcludeContext<TContainer>
	{
		// Token: 0x060001C2 RID: 450 RVA: 0x0000664C File Offset: 0x0000484C
		internal static ExcludeContext<TContainer> FromProperty<TValue>(PropertyVisitor visitor, Property<TContainer, TValue> property)
		{
			return new ExcludeContext<TContainer>(visitor, property);
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00006665 File Offset: 0x00004865
		public IProperty<TContainer> Property { get; }

		// Token: 0x060001C4 RID: 452 RVA: 0x0000666D File Offset: 0x0000486D
		private ExcludeContext(PropertyVisitor visitor, IProperty<TContainer> property)
		{
			this.m_Visitor = visitor;
			this.Property = property;
		}

		// Token: 0x04000091 RID: 145
		private readonly PropertyVisitor m_Visitor;
	}
}
