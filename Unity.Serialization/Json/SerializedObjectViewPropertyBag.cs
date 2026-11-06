using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;

namespace Unity.Serialization.Json
{
	// Token: 0x0200007F RID: 127
	internal class SerializedObjectViewPropertyBag : PropertyBag<SerializedObjectView>
	{
		// Token: 0x060002E0 RID: 736 RVA: 0x0000D967 File Offset: 0x0000BB67
		public override PropertyCollection<SerializedObjectView> GetProperties()
		{
			return PropertyCollection<SerializedObjectView>.Empty;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000D96E File Offset: 0x0000BB6E
		public override PropertyCollection<SerializedObjectView> GetProperties(ref SerializedObjectView container)
		{
			return new PropertyCollection<SerializedObjectView>(new SerializedObjectViewPropertyBag.Enumerable(container));
		}

		// Token: 0x040001AB RID: 427
		private static readonly SerializedObjectViewPropertyBag.Property k_Property = new SerializedObjectViewPropertyBag.Property();

		// Token: 0x02000080 RID: 128
		private class Property : Property<SerializedObjectView, SerializedValueView>
		{
			// Token: 0x1700004E RID: 78
			// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000D99C File Offset: 0x0000BB9C
			public override string Name
			{
				get
				{
					return this.m_Member.Name().ToString();
				}
			}

			// Token: 0x1700004F RID: 79
			// (get) Token: 0x060002E5 RID: 741 RVA: 0x00003204 File Offset: 0x00001404
			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060002E6 RID: 742 RVA: 0x0000D9C2 File Offset: 0x0000BBC2
			public override SerializedValueView GetValue(ref SerializedObjectView container)
			{
				return this.m_Member.Value();
			}

			// Token: 0x060002E7 RID: 743 RVA: 0x0000B071 File Offset: 0x00009271
			public override void SetValue(ref SerializedObjectView container, SerializedValueView value)
			{
				throw new InvalidOperationException("Property is ReadOnly.");
			}

			// Token: 0x040001AC RID: 428
			internal SerializedMemberView m_Member;
		}

		// Token: 0x02000081 RID: 129
		private struct Enumerator : IEnumerator<IProperty<SerializedObjectView>>, IEnumerator, IDisposable
		{
			// Token: 0x060002E9 RID: 745 RVA: 0x0000D9D7 File Offset: 0x0000BBD7
			public Enumerator(SerializedObjectView.Enumerator enumerator)
			{
				this.m_Enumerator = enumerator;
			}

			// Token: 0x060002EA RID: 746 RVA: 0x0000D9E0 File Offset: 0x0000BBE0
			public bool MoveNext()
			{
				return this.m_Enumerator.MoveNext();
			}

			// Token: 0x060002EB RID: 747 RVA: 0x0000D9ED File Offset: 0x0000BBED
			public void Reset()
			{
				this.m_Enumerator.Reset();
			}

			// Token: 0x060002EC RID: 748 RVA: 0x0000D9FA File Offset: 0x0000BBFA
			public void Dispose()
			{
				this.m_Enumerator.Dispose();
			}

			// Token: 0x17000050 RID: 80
			// (get) Token: 0x060002ED RID: 749 RVA: 0x0000DA07 File Offset: 0x0000BC07
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x17000051 RID: 81
			// (get) Token: 0x060002EE RID: 750 RVA: 0x0000DA10 File Offset: 0x0000BC10
			public IProperty<SerializedObjectView> Current
			{
				get
				{
					SerializedMemberView serializedMemberView = this.m_Enumerator.Current;
					SerializedObjectViewPropertyBag.k_Property.m_Member = serializedMemberView;
					return SerializedObjectViewPropertyBag.k_Property;
				}
			}

			// Token: 0x040001AD RID: 429
			private SerializedObjectView.Enumerator m_Enumerator;
		}

		// Token: 0x02000082 RID: 130
		private readonly struct Enumerable : IEnumerable<IProperty<SerializedObjectView>>, IEnumerable
		{
			// Token: 0x060002EF RID: 751 RVA: 0x0000DA39 File Offset: 0x0000BC39
			public Enumerable(SerializedObjectView container)
			{
				this.m_Container = container;
			}

			// Token: 0x060002F0 RID: 752 RVA: 0x0000DA42 File Offset: 0x0000BC42
			public IEnumerator<IProperty<SerializedObjectView>> GetEnumerator()
			{
				return new SerializedObjectViewPropertyBag.Enumerator(this.m_Container.GetEnumerator());
			}

			// Token: 0x060002F1 RID: 753 RVA: 0x0000DA42 File Offset: 0x0000BC42
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new SerializedObjectViewPropertyBag.Enumerator(this.m_Container.GetEnumerator());
			}

			// Token: 0x040001AE RID: 430
			private readonly SerializedObjectView m_Container;
		}
	}
}
