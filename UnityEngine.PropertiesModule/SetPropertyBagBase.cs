using System;
using System.Collections.Generic;

namespace Unity.Properties
{
	// Token: 0x02000055 RID: 85
	public class SetPropertyBagBase<TSet, TElement> : PropertyBag<TSet>, ISetPropertyBag<TSet, TElement>, ICollectionPropertyBag<TSet, TElement>, IPropertyBag<TSet>, IPropertyBag, ICollectionPropertyBagAccept<TSet>, ISetPropertyBagAccept<TSet>, ISetPropertyAccept<TSet>, IKeyedProperties<TSet, object> where TSet : ISet<TElement>
	{
		// Token: 0x0600019A RID: 410 RVA: 0x00006310 File Offset: 0x00004510
		public override PropertyCollection<TSet> GetProperties()
		{
			return PropertyCollection<TSet>.Empty;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00006328 File Offset: 0x00004528
		public override PropertyCollection<TSet> GetProperties(ref TSet container)
		{
			return new PropertyCollection<TSet>(this.GetPropertiesEnumerable(container));
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000634B File Offset: 0x0000454B
		private IEnumerable<IProperty<TSet>> GetPropertiesEnumerable(TSet container)
		{
			foreach (TElement element in container)
			{
				this.m_Property.m_Value = element;
				yield return this.m_Property;
				element = default(TElement);
			}
			IEnumerator<TElement> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000550A File Offset: 0x0000370A
		void ICollectionPropertyBagAccept<TSet>.Accept(ICollectionPropertyBagVisitor visitor, ref TSet container)
		{
			visitor.Visit<TSet, TElement>(this, ref container);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00006362 File Offset: 0x00004562
		void ISetPropertyBagAccept<TSet>.Accept(ISetPropertyBagVisitor visitor, ref TSet container)
		{
			visitor.Visit<TSet, TElement>(this, ref container);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00006370 File Offset: 0x00004570
		void ISetPropertyAccept<TSet>.Accept<TContainer>(ISetPropertyVisitor visitor, Property<TContainer, TSet> property, ref TContainer container, ref TSet dictionary)
		{
			using (new AttributesScope(this.m_Property, property))
			{
				visitor.Visit<TContainer, TSet, TElement>(property, ref container, ref dictionary);
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000063BC File Offset: 0x000045BC
		public bool TryGetProperty(ref TSet container, object key, out IProperty<TSet> property)
		{
			bool flag = container.Contains((TElement)((object)key));
			bool flag2;
			if (flag)
			{
				property = new SetPropertyBagBase<TSet, TElement>.SetElementProperty
				{
					m_Value = (TElement)((object)key)
				};
				flag2 = true;
			}
			else
			{
				property = null;
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x04000085 RID: 133
		private readonly SetPropertyBagBase<TSet, TElement>.SetElementProperty m_Property = new SetPropertyBagBase<TSet, TElement>.SetElementProperty();

		// Token: 0x02000056 RID: 86
		private class SetElementProperty : Property<TSet, TElement>, ISetElementProperty<TElement>, ISetElementProperty, ICollectionElementProperty
		{
			// Token: 0x17000042 RID: 66
			// (get) Token: 0x060001A2 RID: 418 RVA: 0x00006414 File Offset: 0x00004614
			public override string Name
			{
				get
				{
					return this.m_Value.ToString();
				}
			}

			// Token: 0x17000043 RID: 67
			// (get) Token: 0x060001A3 RID: 419 RVA: 0x000052B1 File Offset: 0x000034B1
			public override bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060001A4 RID: 420 RVA: 0x00006427 File Offset: 0x00004627
			public override TElement GetValue(ref TSet container)
			{
				return this.m_Value;
			}

			// Token: 0x060001A5 RID: 421 RVA: 0x0000642F File Offset: 0x0000462F
			public override void SetValue(ref TSet container, TElement value)
			{
				throw new InvalidOperationException("Property is ReadOnly.");
			}

			// Token: 0x17000044 RID: 68
			// (get) Token: 0x060001A6 RID: 422 RVA: 0x00006427 File Offset: 0x00004627
			public TElement Key
			{
				get
				{
					return this.m_Value;
				}
			}

			// Token: 0x17000045 RID: 69
			// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000643B File Offset: 0x0000463B
			public object ObjectKey
			{
				get
				{
					return this.m_Value;
				}
			}

			// Token: 0x04000086 RID: 134
			internal TElement m_Value;
		}
	}
}
