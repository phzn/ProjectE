using System;
using Unity.Properties.Internal;

namespace Unity.Properties
{
	// Token: 0x0200007E RID: 126
	public readonly struct VisitContext<TContainer, TValue>
	{
		// Token: 0x060001FD RID: 509 RVA: 0x00006F74 File Offset: 0x00005174
		internal static VisitContext<TContainer, TValue> FromProperty(PropertyVisitor visitor, ReadOnlyAdapterCollection.Enumerator enumerator, Property<TContainer, TValue> property)
		{
			return new VisitContext<TContainer, TValue>(visitor, enumerator, property);
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00006F8E File Offset: 0x0000518E
		public Property<TContainer, TValue> Property { get; }

		// Token: 0x060001FF RID: 511 RVA: 0x00006F96 File Offset: 0x00005196
		private VisitContext(PropertyVisitor visitor, ReadOnlyAdapterCollection.Enumerator enumerator, Property<TContainer, TValue> property)
		{
			this.m_Visitor = visitor;
			this.m_Enumerator = enumerator;
			this.Property = property;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00006FAE File Offset: 0x000051AE
		public void ContinueVisitation(ref TContainer container, ref TValue value)
		{
			this.m_Visitor.ContinueVisitation<TContainer, TValue>(this.Property, this.m_Enumerator, ref container, ref value);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00006FCB File Offset: 0x000051CB
		public void ContinueVisitationWithoutAdapters(ref TContainer container, ref TValue value)
		{
			this.m_Visitor.ContinueVisitationWithoutAdapters<TContainer, TValue>(this.Property, this.m_Enumerator, ref container, ref value);
		}

		// Token: 0x0400009B RID: 155
		private readonly ReadOnlyAdapterCollection.Enumerator m_Enumerator;

		// Token: 0x0400009C RID: 156
		private readonly PropertyVisitor m_Visitor;
	}
}
