using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

namespace Unity.Entities
{
	// Token: 0x02000252 RID: 594
	internal class ManagedObjectRemap : IPropertyBagVisitor, IPropertyVisitor, ITypedVisit<Entity>
	{
		// Token: 0x06000F49 RID: 3913 RVA: 0x00045198 File Offset: 0x00043398
		private IPropertyBag GetPropertyBag(object obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			Type type = obj.GetType();
			if (TypeManager.GetTypeInfo(TypeManager.GetTypeIndex(type)).Category == TypeManager.TypeCategory.UnityEngineObject)
			{
				throw new ArgumentException("Cannot remap hybrid components", "obj");
			}
			IPropertyBag propertyBag = PropertyBag.GetPropertyBag(type);
			if (propertyBag == null)
			{
				throw new MissingPropertyBagException(type);
			}
			return propertyBag;
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x000451EF File Offset: 0x000433EF
		public unsafe void RemapEntityReferences(ref object obj, EntityRemapUtility.EntityRemapInfo* entityRemapInfo)
		{
			this.m_Info = entityRemapInfo;
			this.m_PrefabSrc = null;
			this.m_PrefabDst = null;
			this.m_PrefabCount = 0;
			HashSet<object> references = this.m_References;
			if (references != null)
			{
				references.Clear();
			}
			this.GetPropertyBag(obj).Accept(this, ref obj);
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x0004522F File Offset: 0x0004342F
		public unsafe void RemapEntityReferencesForPrefab(ref object obj, Entity* remapSrc, Entity* remapDst, int remapInfoCount)
		{
			this.m_Info = null;
			this.m_PrefabSrc = remapSrc;
			this.m_PrefabDst = remapDst;
			this.m_PrefabCount = remapInfoCount;
			HashSet<object> references = this.m_References;
			if (references != null)
			{
				references.Clear();
			}
			this.GetPropertyBag(obj).Accept(this, ref obj);
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x00045270 File Offset: 0x00043470
		void IPropertyBagVisitor.Visit<TContainer>(IPropertyBag<TContainer> properties, ref TContainer container)
		{
			foreach (IProperty<TContainer> property in properties.GetProperties(ref container))
			{
				property.Accept(this, ref container);
			}
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x000452C8 File Offset: 0x000434C8
		void IPropertyVisitor.Visit<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container)
		{
			TValue value = property.GetValue(ref container);
			if (TypeTraits<TValue>.CanBeNull && value == null)
			{
				return;
			}
			if (!TypeTraits<TValue>.IsValueType && typeof(string) != typeof(TValue))
			{
				if (this.m_References == null)
				{
					this.m_References = new HashSet<object>();
				}
				if (!this.m_References.Add(value))
				{
					return;
				}
			}
			if (value is Object)
			{
				return;
			}
			ITypedVisit<TValue> typedVisit = this as ITypedVisit<TValue>;
			if (typedVisit != null)
			{
				typedVisit.Visit<TContainer>(property, ref container, ref value);
				property.SetValue(ref container, value);
				return;
			}
			PropertyContainer.Accept<TValue>(this, ref value, default(VisitParameters));
			if (!property.IsReadOnly && TypeTraits<TValue>.IsValueType)
			{
				property.SetValue(ref container, value);
			}
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x0004538C File Offset: 0x0004358C
		void ITypedVisit<Entity>.Visit<TContainer>(Property<TContainer, Entity> property, ref TContainer container, ref Entity value)
		{
			value = ((null != this.m_Info) ? EntityRemapUtility.RemapEntity(this.m_Info, value) : EntityRemapUtility.RemapEntityForPrefab(this.m_PrefabSrc, this.m_PrefabDst, this.m_PrefabCount, value));
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x000453D9 File Offset: 0x000435D9
		public void ClearGCRefs()
		{
			HashSet<object> references = this.m_References;
			if (references == null)
			{
				return;
			}
			references.Clear();
		}

		// Token: 0x04000620 RID: 1568
		private HashSet<object> m_References;

		// Token: 0x04000621 RID: 1569
		private unsafe EntityRemapUtility.EntityRemapInfo* m_Info;

		// Token: 0x04000622 RID: 1570
		private unsafe Entity* m_PrefabSrc;

		// Token: 0x04000623 RID: 1571
		private unsafe Entity* m_PrefabDst;

		// Token: 0x04000624 RID: 1572
		private int m_PrefabCount;
	}
}
