using System;
using Unity.Properties.Internal;

namespace Unity.Properties
{
	// Token: 0x0200007F RID: 127
	public readonly struct VisitContext<TContainer>
	{
		// Token: 0x06000202 RID: 514 RVA: 0x00006FE8 File Offset: 0x000051E8
		internal static VisitContext<TContainer> FromProperty<TValue>(PropertyVisitor visitor, ReadOnlyAdapterCollection.Enumerator enumerator, Property<TContainer, TValue> property)
		{
			return new VisitContext<TContainer>(visitor, enumerator, property, delegate(PropertyVisitor v, ReadOnlyAdapterCollection.Enumerator e, IProperty<TContainer> p, ref TContainer c)
			{
				Property<TContainer, TValue> property2 = (Property<TContainer, TValue>)p;
				TValue value = property2.GetValue(ref c);
				v.ContinueVisitation<TContainer, TValue>(property2, e, ref c, ref value);
			}, delegate(PropertyVisitor v, IProperty<TContainer> p, ref TContainer c)
			{
				Property<TContainer, TValue> property3 = (Property<TContainer, TValue>)p;
				TValue value2 = property3.GetValue(ref c);
				v.ContinueVisitation<TContainer, TValue>(property3, ref c, ref value2);
			});
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00007040 File Offset: 0x00005240
		public IProperty<TContainer> Property { get; }

		// Token: 0x06000204 RID: 516 RVA: 0x00007048 File Offset: 0x00005248
		private VisitContext(PropertyVisitor visitor, ReadOnlyAdapterCollection.Enumerator enumerator, IProperty<TContainer> property, VisitContext<TContainer>.VisitDelegate continueVisitation, VisitContext<TContainer>.VisitWithoutAdaptersDelegate continueVisitationWithoutAdapters)
		{
			this.m_Visitor = visitor;
			this.m_Enumerator = enumerator;
			this.Property = property;
			this.m_Continue = continueVisitation;
			this.m_ContinueWithoutAdapters = continueVisitationWithoutAdapters;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00007070 File Offset: 0x00005270
		public void ContinueVisitation(ref TContainer container)
		{
			this.m_Continue(this.m_Visitor, this.m_Enumerator, this.Property, ref container);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00007092 File Offset: 0x00005292
		public void ContinueVisitationWithoutAdapters(ref TContainer container)
		{
			this.m_ContinueWithoutAdapters(this.m_Visitor, this.Property, ref container);
		}

		// Token: 0x0400009E RID: 158
		private readonly ReadOnlyAdapterCollection.Enumerator m_Enumerator;

		// Token: 0x0400009F RID: 159
		private readonly PropertyVisitor m_Visitor;

		// Token: 0x040000A0 RID: 160
		private readonly VisitContext<TContainer>.VisitDelegate m_Continue;

		// Token: 0x040000A1 RID: 161
		private readonly VisitContext<TContainer>.VisitWithoutAdaptersDelegate m_ContinueWithoutAdapters;

		// Token: 0x02000080 RID: 128
		// (Invoke) Token: 0x06000208 RID: 520
		private delegate void VisitDelegate(PropertyVisitor visitor, ReadOnlyAdapterCollection.Enumerator enumerator, IProperty<TContainer> property, ref TContainer container);

		// Token: 0x02000081 RID: 129
		// (Invoke) Token: 0x0600020C RID: 524
		private delegate void VisitWithoutAdaptersDelegate(PropertyVisitor visitor, IProperty<TContainer> property, ref TContainer container);
	}
}
