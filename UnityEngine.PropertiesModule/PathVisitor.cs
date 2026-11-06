using System;
using System.Collections.Generic;
using Unity.Properties.Internal;

namespace Unity.Properties
{
	// Token: 0x0200007A RID: 122
	public abstract class PathVisitor : IPropertyBagVisitor, IPropertyVisitor
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000667E File Offset: 0x0000487E
		// (set) Token: 0x060001DA RID: 474 RVA: 0x00006686 File Offset: 0x00004886
		public PropertyPath Path { get; set; }

		// Token: 0x060001DB RID: 475 RVA: 0x00006690 File Offset: 0x00004890
		public virtual void Reset()
		{
			this.m_PathIndex = 0;
			this.Path = default(PropertyPath);
			this.ReturnCode = VisitReturnCode.Ok;
			this.ReadonlyVisit = false;
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001DC RID: 476 RVA: 0x000066C5 File Offset: 0x000048C5
		// (set) Token: 0x060001DD RID: 477 RVA: 0x000066CD File Offset: 0x000048CD
		private IProperty Property { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001DE RID: 478 RVA: 0x000066D6 File Offset: 0x000048D6
		// (set) Token: 0x060001DF RID: 479 RVA: 0x000066DE File Offset: 0x000048DE
		public bool ReadonlyVisit { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x000066E7 File Offset: 0x000048E7
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x000066EF File Offset: 0x000048EF
		public VisitReturnCode ReturnCode { get; protected set; }

		// Token: 0x060001E2 RID: 482 RVA: 0x000066F8 File Offset: 0x000048F8
		void IPropertyBagVisitor.Visit<TContainer>(IPropertyBag<TContainer> properties, ref TContainer container)
		{
			PropertyPath path = this.Path;
			int pathIndex = this.m_PathIndex;
			this.m_PathIndex = pathIndex + 1;
			PropertyPathPart propertyPathPart = path[pathIndex];
			switch (propertyPathPart.Kind)
			{
			case PropertyPathPartKind.Name:
			{
				INamedProperties<TContainer> namedProperties = properties as INamedProperties<TContainer>;
				IProperty<TContainer> property;
				bool flag = namedProperties != null && namedProperties.TryGetProperty(ref container, propertyPathPart.Name, out property);
				if (flag)
				{
					property.Accept(this, ref container);
				}
				else
				{
					this.ReturnCode = VisitReturnCode.InvalidPath;
				}
				break;
			}
			case PropertyPathPartKind.Index:
			{
				IIndexedProperties<TContainer> indexedProperties = properties as IIndexedProperties<TContainer>;
				IProperty<TContainer> property;
				bool flag2 = indexedProperties != null && indexedProperties.TryGetProperty(ref container, propertyPathPart.Index, out property);
				if (flag2)
				{
					using ((property as IAttributes).CreateAttributesScope(this.Property as IAttributes))
					{
						property.Accept(this, ref container);
					}
				}
				else
				{
					this.ReturnCode = VisitReturnCode.InvalidPath;
				}
				break;
			}
			case PropertyPathPartKind.Key:
			{
				IKeyedProperties<TContainer, object> keyedProperties = properties as IKeyedProperties<TContainer, object>;
				IProperty<TContainer> property;
				bool flag3 = keyedProperties != null && keyedProperties.TryGetProperty(ref container, propertyPathPart.Key, out property);
				if (flag3)
				{
					using ((property as IAttributes).CreateAttributesScope(this.Property as IAttributes))
					{
						property.Accept(this, ref container);
					}
				}
				else
				{
					this.ReturnCode = VisitReturnCode.InvalidPath;
				}
				break;
			}
			default:
				this.ReturnCode = VisitReturnCode.InvalidPath;
				break;
			}
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00006890 File Offset: 0x00004A90
		void IPropertyVisitor.Visit<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container)
		{
			TValue value = property.GetValue(ref container);
			bool flag = this.m_PathIndex >= this.Path.Length;
			if (flag)
			{
				this.VisitPath<TContainer, TValue>(property, ref container, ref value);
			}
			else
			{
				IPropertyBag propertyBag;
				bool flag2 = PropertyBag.TryGetPropertyBagForValue<TValue>(ref value, out propertyBag);
				if (flag2)
				{
					bool flag3 = TypeTraits<TValue>.CanBeNull && EqualityComparer<TValue>.Default.Equals(value, default(TValue));
					if (flag3)
					{
						this.ReturnCode = VisitReturnCode.InvalidPath;
					}
					else
					{
						using (new PathVisitor.PropertyScope(this, property))
						{
							PropertyContainer.Accept<TValue>(this, ref value, default(VisitParameters));
						}
						bool flag4 = !property.IsReadOnly && !this.ReadonlyVisit;
						if (flag4)
						{
							property.SetValue(ref container, value);
						}
					}
				}
				else
				{
					this.ReturnCode = VisitReturnCode.InvalidPath;
				}
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00005483 File Offset: 0x00003683
		protected virtual void VisitPath<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container, ref TValue value)
		{
		}

		// Token: 0x04000093 RID: 147
		private int m_PathIndex;

		// Token: 0x0200007B RID: 123
		private readonly struct PropertyScope : IDisposable
		{
			// Token: 0x060001E6 RID: 486 RVA: 0x00006988 File Offset: 0x00004B88
			public PropertyScope(PathVisitor visitor, IProperty property)
			{
				this.m_Visitor = visitor;
				this.m_Property = this.m_Visitor.Property;
				this.m_Visitor.Property = property;
			}

			// Token: 0x060001E7 RID: 487 RVA: 0x000069B0 File Offset: 0x00004BB0
			public void Dispose()
			{
				this.m_Visitor.Property = this.m_Property;
			}

			// Token: 0x04000098 RID: 152
			private readonly PathVisitor m_Visitor;

			// Token: 0x04000099 RID: 153
			private readonly IProperty m_Property;
		}
	}
}
