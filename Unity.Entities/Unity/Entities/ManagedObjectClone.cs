using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Properties;
using UnityEngine;

namespace Unity.Entities
{
	// Token: 0x0200024F RID: 591
	internal class ManagedObjectClone : IPropertyBagVisitor, IListPropertyBagVisitor, ISetPropertyBagVisitor, IDictionaryPropertyBagVisitor, IPropertyVisitor
	{
		// Token: 0x06000F37 RID: 3895 RVA: 0x000446F4 File Offset: 0x000428F4
		public object Clone(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			ICloneable cloneable = obj as ICloneable;
			if (cloneable != null)
			{
				return cloneable.Clone();
			}
			Type type = obj.GetType();
			if (typeof(Object).IsAssignableFrom(type))
			{
				return obj;
			}
			object obj2;
			if (!type.IsArray)
			{
				obj2 = Activator.CreateInstance(type);
			}
			else
			{
				Type elementType = type.GetElementType();
				IList list = obj as IList;
				obj2 = Array.CreateInstance(elementType, (list != null) ? list.Count : 0);
			}
			object obj3 = obj2;
			IPropertyBag propertyBag = PropertyBag.GetPropertyBag(type);
			if (propertyBag == null)
			{
				throw new MissingPropertyBagException(type);
			}
			Dictionary<object, object> references = this.m_References;
			if (references != null)
			{
				references.Clear();
			}
			this.m_RootSource = obj;
			this.m_RootDestination = obj3;
			this.m_Stack = obj3;
			propertyBag.Accept(this, ref obj);
			return this.m_Stack;
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x000447AC File Offset: 0x000429AC
		void IPropertyBagVisitor.Visit<TContainer>(IPropertyBag<TContainer> properties, ref TContainer srcContainer)
		{
			foreach (IProperty<TContainer> property in properties.GetProperties(ref srcContainer))
			{
				property.Accept(this, ref srcContainer);
			}
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x00044804 File Offset: 0x00042A04
		void IListPropertyBagVisitor.Visit<TList, TElement>(IListPropertyBag<TList, TElement> properties, ref TList srcContainer)
		{
			TList tlist = (TList)((object)this.m_Stack);
			if (typeof(TList).IsArray)
			{
				int num = 0;
				using (IEnumerator<TElement> enumerator = srcContainer.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TElement telement = enumerator.Current;
						TElement telement2 = default(TElement);
						this.CloneValue<TElement>(ref telement2, telement);
						tlist[num++] = telement2;
					}
					goto IL_00C7;
				}
			}
			tlist.Clear();
			foreach (TElement telement3 in srcContainer)
			{
				TElement telement4 = default(TElement);
				this.CloneValue<TElement>(ref telement4, telement3);
				tlist.Add(telement4);
			}
			IL_00C7:
			this.m_Stack = tlist;
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x00044900 File Offset: 0x00042B00
		void ISetPropertyBagVisitor.Visit<TSet, TElement>(ISetPropertyBag<TSet, TElement> properties, ref TSet srcContainer)
		{
			TSet tset = (TSet)((object)this.m_Stack);
			tset.Clear();
			foreach (TElement telement in srcContainer)
			{
				TElement telement2 = default(TElement);
				this.CloneValue<TElement>(ref telement2, telement);
				tset.Add(telement2);
			}
			this.m_Stack = tset;
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x0004498C File Offset: 0x00042B8C
		void IDictionaryPropertyBagVisitor.Visit<TDictionary, TKey, TValue>(IDictionaryPropertyBag<TDictionary, TKey, TValue> properties, ref TDictionary srcContainer)
		{
			TDictionary tdictionary = (TDictionary)((object)this.m_Stack);
			tdictionary.Clear();
			foreach (KeyValuePair<TKey, TValue> keyValuePair in srcContainer)
			{
				TKey tkey = default(TKey);
				TValue tvalue = default(TValue);
				this.CloneValue<TKey>(ref tkey, keyValuePair.Key);
				this.CloneValue<TValue>(ref tvalue, keyValuePair.Value);
				tdictionary.Add(tkey, tvalue);
			}
			this.m_Stack = tdictionary;
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x00044A38 File Offset: 0x00042C38
		void IPropertyVisitor.Visit<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer srcContainer)
		{
			TContainer tcontainer = (TContainer)((object)this.m_Stack);
			TValue value = property.GetValue(ref srcContainer);
			TValue value2 = property.GetValue(ref tcontainer);
			this.CloneValue<TValue>(ref value2, value);
			if (property.IsReadOnly)
			{
				return;
			}
			property.SetValue(ref tcontainer, value2);
			this.m_Stack = tcontainer;
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x00044A8C File Offset: 0x00042C8C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CloneValue<TValue>(ref TValue dstValue, TValue srcValue)
		{
			if (!TypeTraits<TValue>.IsContainer)
			{
				dstValue = srcValue;
				return;
			}
			if (TypeTraits<TValue>.CanBeNull && srcValue == null)
			{
				dstValue = default(TValue);
				return;
			}
			if (TypeTraits<TValue>.IsValueType)
			{
				dstValue = default(TValue);
			}
			else
			{
				Type type = srcValue.GetType();
				if (typeof(Object).IsAssignableFrom(type))
				{
					dstValue = srcValue;
					return;
				}
				if (!TypeTraits.IsContainer(type))
				{
					dstValue = srcValue;
					return;
				}
				bool flag = !type.IsValueType && !(type == typeof(string));
				if (flag)
				{
					if (this.m_RootSource == srcValue)
					{
						dstValue = (TValue)((object)this.m_RootDestination);
						return;
					}
					object obj;
					if (this.m_References != null && this.m_References.TryGetValue(srcValue, out obj))
					{
						dstValue = (TValue)((object)obj);
						return;
					}
				}
				if (type.IsArray)
				{
					IList list = srcValue as IList;
					int num = ((list != null) ? list.Count : 0);
					if (dstValue != null)
					{
						Array array = dstValue as Array;
						if (array != null && array.Length == num)
						{
							goto IL_0180;
						}
					}
					dstValue = (TValue)((object)Array.CreateInstance(type.GetElementType(), num));
				}
				else if (dstValue == null || dstValue.GetType() != type)
				{
					dstValue = (TValue)((object)Activator.CreateInstance(type));
				}
				IL_0180:
				if (flag)
				{
					if (this.m_References == null)
					{
						this.m_References = new Dictionary<object, object>();
					}
					this.m_References.Add(srcValue, dstValue);
				}
			}
			object stack = this.m_Stack;
			this.m_Stack = dstValue;
			VisitReturnCode visitReturnCode;
			PropertyContainer.TryAccept<TValue>(this, ref srcValue, out visitReturnCode, default(VisitParameters));
			dstValue = (TValue)((object)this.m_Stack);
			this.m_Stack = stack;
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x00044C90 File Offset: 0x00042E90
		public void ClearGCRefs()
		{
			Dictionary<object, object> references = this.m_References;
			if (references != null)
			{
				references.Clear();
			}
			this.m_Stack = null;
			this.m_RootSource = null;
			this.m_RootDestination = null;
		}

		// Token: 0x04000617 RID: 1559
		private Dictionary<object, object> m_References;

		// Token: 0x04000618 RID: 1560
		private object m_RootSource;

		// Token: 0x04000619 RID: 1561
		private object m_RootDestination;

		// Token: 0x0400061A RID: 1562
		private object m_Stack;
	}
}
