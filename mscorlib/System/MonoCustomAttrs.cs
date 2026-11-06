using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200023F RID: 575
	internal static class MonoCustomAttrs
	{
		// Token: 0x06001A34 RID: 6708 RVA: 0x000602BC File Offset: 0x0005E4BC
		private static bool IsUserCattrProvider(object obj)
		{
			Type type = obj as Type;
			if (type is RuntimeType || (RuntimeFeature.IsDynamicCodeSupported && type is TypeBuilder))
			{
				return false;
			}
			if (obj is Type)
			{
				return true;
			}
			if (MonoCustomAttrs.corlib == null)
			{
				MonoCustomAttrs.corlib = typeof(int).Assembly;
			}
			return obj.GetType().Assembly != MonoCustomAttrs.corlib;
		}

		// Token: 0x06001A35 RID: 6709
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Attribute[] GetCustomAttributesInternal(ICustomAttributeProvider obj, Type attributeType, bool pseudoAttrs);

		// Token: 0x06001A36 RID: 6710 RVA: 0x0006032C File Offset: 0x0005E52C
		internal static object[] GetPseudoCustomAttributes(ICustomAttributeProvider obj, Type attributeType)
		{
			object[] array = null;
			RuntimeMethodInfo runtimeMethodInfo = obj as RuntimeMethodInfo;
			if (runtimeMethodInfo != null)
			{
				array = runtimeMethodInfo.GetPseudoCustomAttributes();
			}
			else
			{
				RuntimeFieldInfo runtimeFieldInfo = obj as RuntimeFieldInfo;
				if (runtimeFieldInfo != null)
				{
					array = runtimeFieldInfo.GetPseudoCustomAttributes();
				}
				else
				{
					RuntimeParameterInfo runtimeParameterInfo = obj as RuntimeParameterInfo;
					if (runtimeParameterInfo != null)
					{
						array = runtimeParameterInfo.GetPseudoCustomAttributes();
					}
					else
					{
						Type type = obj as Type;
						if (type != null)
						{
							array = MonoCustomAttrs.GetPseudoCustomAttributes(type);
						}
					}
				}
			}
			if (attributeType != null && array != null)
			{
				int i = 0;
				while (i < array.Length)
				{
					if (attributeType.IsAssignableFrom(array[i].GetType()))
					{
						if (array.Length == 1)
						{
							return array;
						}
						return new object[] { array[i] };
					}
					else
					{
						i++;
					}
				}
				return Array.Empty<object>();
			}
			return array;
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x000603D4 File Offset: 0x0005E5D4
		private static object[] GetPseudoCustomAttributes(Type type)
		{
			int num = 0;
			TypeAttributes attributes = type.Attributes;
			if ((attributes & TypeAttributes.Serializable) != TypeAttributes.NotPublic)
			{
				num++;
			}
			if ((attributes & TypeAttributes.Import) != TypeAttributes.NotPublic)
			{
				num++;
			}
			if (num == 0)
			{
				return null;
			}
			object[] array = new object[num];
			num = 0;
			if ((attributes & TypeAttributes.Serializable) != TypeAttributes.NotPublic)
			{
				array[num++] = new SerializableAttribute();
			}
			if ((attributes & TypeAttributes.Import) != TypeAttributes.NotPublic)
			{
				array[num++] = new ComImportAttribute();
			}
			return array;
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x00060440 File Offset: 0x0005E640
		internal static object[] GetCustomAttributesBase(ICustomAttributeProvider obj, Type attributeType, bool inheritedOnly)
		{
			object[] array;
			if (MonoCustomAttrs.IsUserCattrProvider(obj))
			{
				array = obj.GetCustomAttributes(attributeType, true);
			}
			else
			{
				object[] array2 = MonoCustomAttrs.GetCustomAttributesInternal(obj, attributeType, false);
				array = array2;
			}
			if (!inheritedOnly)
			{
				object[] pseudoCustomAttributes = MonoCustomAttrs.GetPseudoCustomAttributes(obj, attributeType);
				if (pseudoCustomAttributes != null)
				{
					object[] array2 = new Attribute[array.Length + pseudoCustomAttributes.Length];
					object[] array3 = array2;
					Array.Copy(array, array3, array.Length);
					Array.Copy(pseudoCustomAttributes, 0, array3, array.Length, pseudoCustomAttributes.Length);
					return array3;
				}
			}
			return array;
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x000604A4 File Offset: 0x0005E6A4
		internal static object[] GetCustomAttributes(ICustomAttributeProvider obj, Type attributeType, bool inherit)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (attributeType == typeof(MonoCustomAttrs))
			{
				attributeType = null;
			}
			object[] array = MonoCustomAttrs.GetCustomAttributesBase(obj, attributeType, false);
			if (!inherit && array.Length == 1)
			{
				if (array[0] == null)
				{
					throw new CustomAttributeFormatException("Invalid custom attribute format");
				}
				object[] array2;
				if (attributeType != null)
				{
					if (attributeType.IsAssignableFrom(array[0].GetType()))
					{
						array2 = (object[])Array.CreateInstance(attributeType, 1);
						array2[0] = array[0];
					}
					else
					{
						array2 = (object[])Array.CreateInstance(attributeType, 0);
					}
				}
				else
				{
					array2 = (object[])Array.CreateInstance(array[0].GetType(), 1);
					array2[0] = array[0];
				}
				return array2;
			}
			else
			{
				if (inherit && MonoCustomAttrs.GetBase(obj) == null)
				{
					inherit = false;
				}
				if (attributeType != null && attributeType.IsSealed && inherit && !MonoCustomAttrs.RetrieveAttributeUsage(attributeType).Inherited)
				{
					inherit = false;
				}
				int num = Math.Max(array.Length, 16);
				ICustomAttributeProvider customAttributeProvider = obj;
				List<object> list;
				object[] array4;
				if (inherit)
				{
					Dictionary<Type, MonoCustomAttrs.AttributeInfo> dictionary = new Dictionary<Type, MonoCustomAttrs.AttributeInfo>(num);
					int num2 = 0;
					list = new List<object>(num);
					for (;;)
					{
						foreach (object obj2 in array)
						{
							if (obj2 == null)
							{
								goto Block_22;
							}
							Type type = obj2.GetType();
							if (!(attributeType != null) || attributeType.IsAssignableFrom(type))
							{
								MonoCustomAttrs.AttributeInfo attributeInfo;
								AttributeUsageAttribute attributeUsageAttribute;
								if (dictionary.TryGetValue(type, out attributeInfo))
								{
									attributeUsageAttribute = attributeInfo.Usage;
								}
								else
								{
									attributeUsageAttribute = MonoCustomAttrs.RetrieveAttributeUsage(type);
								}
								if ((num2 == 0 || attributeUsageAttribute.Inherited) && (attributeUsageAttribute.AllowMultiple || attributeInfo == null || (attributeInfo != null && attributeInfo.InheritanceLevel == num2)))
								{
									list.Add(obj2);
								}
								if (attributeInfo == null)
								{
									dictionary.Add(type, new MonoCustomAttrs.AttributeInfo(attributeUsageAttribute, num2));
								}
							}
						}
						if ((customAttributeProvider = MonoCustomAttrs.GetBase(customAttributeProvider)) != null)
						{
							num2++;
							array = MonoCustomAttrs.GetCustomAttributesBase(customAttributeProvider, attributeType, true);
						}
						if (!inherit || customAttributeProvider == null)
						{
							goto IL_02CF;
						}
					}
					Block_22:
					throw new CustomAttributeFormatException("Invalid custom attribute format");
					IL_02CF:
					if (attributeType == null || attributeType.IsValueType)
					{
						object[] array3 = new Attribute[list.Count];
						array4 = array3;
					}
					else
					{
						array4 = Array.CreateInstance(attributeType, list.Count) as object[];
					}
					list.CopyTo(array4, 0);
					return array4;
				}
				if (attributeType == null)
				{
					object[] array3 = array;
					for (int i = 0; i < array3.Length; i++)
					{
						if (array3[i] == null)
						{
							throw new CustomAttributeFormatException("Invalid custom attribute format");
						}
					}
					Attribute[] array5 = new Attribute[array.Length];
					array.CopyTo(array5, 0);
					return array5;
				}
				list = new List<object>(num);
				foreach (object obj3 in array)
				{
					if (obj3 == null)
					{
						throw new CustomAttributeFormatException("Invalid custom attribute format");
					}
					Type type2 = obj3.GetType();
					if (!(attributeType != null) || attributeType.IsAssignableFrom(type2))
					{
						list.Add(obj3);
					}
				}
				if (attributeType == null || attributeType.IsValueType)
				{
					object[] array3 = new Attribute[list.Count];
					array4 = array3;
				}
				else
				{
					array4 = Array.CreateInstance(attributeType, list.Count) as object[];
				}
				list.CopyTo(array4, 0);
				return array4;
			}
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x000607C2 File Offset: 0x0005E9C2
		internal static object[] GetCustomAttributes(ICustomAttributeProvider obj, bool inherit)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (!inherit)
			{
				return (object[])MonoCustomAttrs.GetCustomAttributesBase(obj, null, false).Clone();
			}
			return MonoCustomAttrs.GetCustomAttributes(obj, typeof(MonoCustomAttrs), inherit);
		}

		// Token: 0x06001A3B RID: 6715
		[PreserveDependency(".ctor(System.Reflection.ConstructorInfo,System.Reflection.Assembly,System.IntPtr,System.UInt32)", "System.Reflection.CustomAttributeData")]
		[PreserveDependency(".ctor(System.Type,System.Object)", "System.Reflection.CustomAttributeTypedArgument")]
		[PreserveDependency(".ctor(System.Reflection.MemberInfo,System.Object)", "System.Reflection.CustomAttributeNamedArgument")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CustomAttributeData[] GetCustomAttributesDataInternal(ICustomAttributeProvider obj);

		// Token: 0x06001A3C RID: 6716 RVA: 0x000607F9 File Offset: 0x0005E9F9
		internal static IList<CustomAttributeData> GetCustomAttributesData(ICustomAttributeProvider obj, bool inherit = false)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (!inherit)
			{
				return MonoCustomAttrs.GetCustomAttributesDataBase(obj, null, false);
			}
			return MonoCustomAttrs.GetCustomAttributesData(obj, typeof(MonoCustomAttrs), inherit);
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x00060828 File Offset: 0x0005EA28
		internal static IList<CustomAttributeData> GetCustomAttributesData(ICustomAttributeProvider obj, Type attributeType, bool inherit)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (attributeType == typeof(MonoCustomAttrs))
			{
				attributeType = null;
			}
			IList<CustomAttributeData> list = MonoCustomAttrs.GetCustomAttributesDataBase(obj, attributeType, false);
			if (!inherit && list.Count == 1)
			{
				if (list[0] == null)
				{
					throw new CustomAttributeFormatException("Invalid custom attribute data format");
				}
				IList<CustomAttributeData> list2;
				if (attributeType != null)
				{
					if (attributeType.IsAssignableFrom(list[0].AttributeType))
					{
						list2 = new CustomAttributeData[] { list[0] };
					}
					else
					{
						list2 = Array.Empty<CustomAttributeData>();
					}
				}
				else
				{
					list2 = new CustomAttributeData[] { list[0] };
				}
				return list2;
			}
			else
			{
				if (inherit && MonoCustomAttrs.GetBase(obj) == null)
				{
					inherit = false;
				}
				if (attributeType != null && attributeType.IsSealed && inherit && !MonoCustomAttrs.RetrieveAttributeUsage(attributeType).Inherited)
				{
					inherit = false;
				}
				int num = Math.Max(list.Count, 16);
				List<CustomAttributeData> list3 = null;
				ICustomAttributeProvider customAttributeProvider = obj;
				if (inherit)
				{
					Dictionary<Type, MonoCustomAttrs.AttributeInfo> dictionary = new Dictionary<Type, MonoCustomAttrs.AttributeInfo>(num);
					int num2 = 0;
					list3 = new List<CustomAttributeData>(num);
					do
					{
						foreach (CustomAttributeData customAttributeData in list)
						{
							if (customAttributeData == null)
							{
								throw new CustomAttributeFormatException("Invalid custom attribute data format");
							}
							Type attributeType2 = customAttributeData.AttributeType;
							if (!(attributeType != null) || attributeType.IsAssignableFrom(attributeType2))
							{
								MonoCustomAttrs.AttributeInfo attributeInfo;
								AttributeUsageAttribute attributeUsageAttribute;
								if (dictionary.TryGetValue(attributeType2, out attributeInfo))
								{
									attributeUsageAttribute = attributeInfo.Usage;
								}
								else
								{
									attributeUsageAttribute = MonoCustomAttrs.RetrieveAttributeUsage(attributeType2);
								}
								if ((num2 == 0 || attributeUsageAttribute.Inherited) && (attributeUsageAttribute.AllowMultiple || attributeInfo == null || (attributeInfo != null && attributeInfo.InheritanceLevel == num2)))
								{
									list3.Add(customAttributeData);
								}
								if (attributeInfo == null)
								{
									dictionary.Add(attributeType2, new MonoCustomAttrs.AttributeInfo(attributeUsageAttribute, num2));
								}
							}
						}
						if ((customAttributeProvider = MonoCustomAttrs.GetBase(customAttributeProvider)) != null)
						{
							num2++;
							list = MonoCustomAttrs.GetCustomAttributesDataBase(customAttributeProvider, attributeType, true);
						}
					}
					while (inherit && customAttributeProvider != null);
					return list3.ToArray();
				}
				if (attributeType == null)
				{
					using (IEnumerator<CustomAttributeData> enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current == null)
							{
								throw new CustomAttributeFormatException("Invalid custom attribute data format");
							}
						}
					}
					CustomAttributeData[] array = new CustomAttributeData[list.Count];
					list.CopyTo(array, 0);
					return array;
				}
				list3 = new List<CustomAttributeData>(num);
				foreach (CustomAttributeData customAttributeData2 in list)
				{
					if (customAttributeData2 == null)
					{
						throw new CustomAttributeFormatException("Invalid custom attribute data format");
					}
					if (attributeType.IsAssignableFrom(customAttributeData2.AttributeType))
					{
						list3.Add(customAttributeData2);
					}
				}
				return list3.ToArray();
			}
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x00060B0C File Offset: 0x0005ED0C
		internal static IList<CustomAttributeData> GetCustomAttributesDataBase(ICustomAttributeProvider obj, Type attributeType, bool inheritedOnly)
		{
			CustomAttributeData[] array;
			if (MonoCustomAttrs.IsUserCattrProvider(obj))
			{
				array = Array.Empty<CustomAttributeData>();
			}
			else
			{
				array = MonoCustomAttrs.GetCustomAttributesDataInternal(obj);
			}
			if (!inheritedOnly)
			{
				CustomAttributeData[] pseudoCustomAttributesData = MonoCustomAttrs.GetPseudoCustomAttributesData(obj, attributeType);
				if (pseudoCustomAttributesData != null)
				{
					if (array.Length == 0)
					{
						return Array.AsReadOnly<CustomAttributeData>(pseudoCustomAttributesData);
					}
					CustomAttributeData[] array2 = new CustomAttributeData[array.Length + pseudoCustomAttributesData.Length];
					Array.Copy(array, array2, array.Length);
					Array.Copy(pseudoCustomAttributesData, 0, array2, array.Length, pseudoCustomAttributesData.Length);
					return Array.AsReadOnly<CustomAttributeData>(array2);
				}
			}
			return Array.AsReadOnly<CustomAttributeData>(array);
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x00060B7C File Offset: 0x0005ED7C
		internal static CustomAttributeData[] GetPseudoCustomAttributesData(ICustomAttributeProvider obj, Type attributeType)
		{
			CustomAttributeData[] array = null;
			RuntimeMethodInfo runtimeMethodInfo = obj as RuntimeMethodInfo;
			if (runtimeMethodInfo != null)
			{
				array = runtimeMethodInfo.GetPseudoCustomAttributesData();
			}
			else
			{
				RuntimeFieldInfo runtimeFieldInfo = obj as RuntimeFieldInfo;
				if (runtimeFieldInfo != null)
				{
					array = runtimeFieldInfo.GetPseudoCustomAttributesData();
				}
				else
				{
					RuntimeParameterInfo runtimeParameterInfo = obj as RuntimeParameterInfo;
					if (runtimeParameterInfo != null)
					{
						array = runtimeParameterInfo.GetPseudoCustomAttributesData();
					}
					else
					{
						Type type = obj as Type;
						if (type != null)
						{
							array = MonoCustomAttrs.GetPseudoCustomAttributesData(type);
						}
					}
				}
			}
			if (attributeType != null && array != null)
			{
				int i = 0;
				while (i < array.Length)
				{
					if (attributeType.IsAssignableFrom(array[i].AttributeType))
					{
						if (array.Length == 1)
						{
							return array;
						}
						return new CustomAttributeData[] { array[i] };
					}
					else
					{
						i++;
					}
				}
				return Array.Empty<CustomAttributeData>();
			}
			return array;
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x00060C24 File Offset: 0x0005EE24
		private static CustomAttributeData[] GetPseudoCustomAttributesData(Type type)
		{
			int num = 0;
			TypeAttributes attributes = type.Attributes;
			if ((attributes & TypeAttributes.Serializable) != TypeAttributes.NotPublic)
			{
				num++;
			}
			if ((attributes & TypeAttributes.Import) != TypeAttributes.NotPublic)
			{
				num++;
			}
			if (num == 0)
			{
				return null;
			}
			CustomAttributeData[] array = new CustomAttributeData[num];
			num = 0;
			if ((attributes & TypeAttributes.Serializable) != TypeAttributes.NotPublic)
			{
				array[num++] = new CustomAttributeData(typeof(SerializableAttribute).GetConstructor(Type.EmptyTypes));
			}
			if ((attributes & TypeAttributes.Import) != TypeAttributes.NotPublic)
			{
				array[num++] = new CustomAttributeData(typeof(ComImportAttribute).GetConstructor(Type.EmptyTypes));
			}
			return array;
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x00060CB8 File Offset: 0x0005EEB8
		internal static bool IsDefined(ICustomAttributeProvider obj, Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			AttributeUsageAttribute attributeUsageAttribute = null;
			while (!MonoCustomAttrs.IsUserCattrProvider(obj))
			{
				if (MonoCustomAttrs.IsDefinedInternal(obj, attributeType))
				{
					return true;
				}
				object[] pseudoCustomAttributes = MonoCustomAttrs.GetPseudoCustomAttributes(obj, attributeType);
				if (pseudoCustomAttributes != null)
				{
					for (int i = 0; i < pseudoCustomAttributes.Length; i++)
					{
						if (attributeType.IsAssignableFrom(pseudoCustomAttributes[i].GetType()))
						{
							return true;
						}
					}
				}
				if (attributeUsageAttribute == null)
				{
					if (!inherit)
					{
						return false;
					}
					attributeUsageAttribute = MonoCustomAttrs.RetrieveAttributeUsage(attributeType);
					if (!attributeUsageAttribute.Inherited)
					{
						return false;
					}
				}
				obj = MonoCustomAttrs.GetBase(obj);
				if (obj == null)
				{
					return false;
				}
			}
			return obj.IsDefined(attributeType, inherit);
		}

		// Token: 0x06001A42 RID: 6722
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsDefinedInternal(ICustomAttributeProvider obj, Type AttributeType);

		// Token: 0x06001A43 RID: 6723 RVA: 0x00060D48 File Offset: 0x0005EF48
		private static PropertyInfo GetBasePropertyDefinition(RuntimePropertyInfo property)
		{
			MethodInfo methodInfo = property.GetGetMethod(true);
			if (methodInfo == null || !methodInfo.IsVirtual)
			{
				methodInfo = property.GetSetMethod(true);
			}
			if (methodInfo == null || !methodInfo.IsVirtual)
			{
				return null;
			}
			MethodInfo baseMethod = ((RuntimeMethodInfo)methodInfo).GetBaseMethod();
			if (!(baseMethod != null) || !(baseMethod != methodInfo))
			{
				return null;
			}
			ParameterInfo[] indexParameters = property.GetIndexParameters();
			if (indexParameters != null && indexParameters.Length != 0)
			{
				Type[] array = new Type[indexParameters.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = indexParameters[i].ParameterType;
				}
				return baseMethod.DeclaringType.GetProperty(property.Name, property.PropertyType, array);
			}
			return baseMethod.DeclaringType.GetProperty(property.Name, property.PropertyType);
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x00060E10 File Offset: 0x0005F010
		private static EventInfo GetBaseEventDefinition(RuntimeEventInfo evt)
		{
			MethodInfo methodInfo = evt.GetAddMethod(true);
			if (methodInfo == null || !methodInfo.IsVirtual)
			{
				methodInfo = evt.GetRaiseMethod(true);
			}
			if (methodInfo == null || !methodInfo.IsVirtual)
			{
				methodInfo = evt.GetRemoveMethod(true);
			}
			if (methodInfo == null || !methodInfo.IsVirtual)
			{
				return null;
			}
			MethodInfo baseMethod = ((RuntimeMethodInfo)methodInfo).GetBaseMethod();
			if (baseMethod != null && baseMethod != methodInfo)
			{
				BindingFlags bindingFlags = (methodInfo.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic);
				bindingFlags |= (methodInfo.IsStatic ? BindingFlags.Static : BindingFlags.Instance);
				return baseMethod.DeclaringType.GetEvent(evt.Name, bindingFlags);
			}
			return null;
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x00060EBC File Offset: 0x0005F0BC
		private static ICustomAttributeProvider GetBase(ICustomAttributeProvider obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is Type)
			{
				return ((Type)obj).BaseType;
			}
			MethodInfo methodInfo = null;
			if (obj is RuntimePropertyInfo)
			{
				return MonoCustomAttrs.GetBasePropertyDefinition((RuntimePropertyInfo)obj);
			}
			if (obj is RuntimeEventInfo)
			{
				return MonoCustomAttrs.GetBaseEventDefinition((RuntimeEventInfo)obj);
			}
			if (obj is RuntimeMethodInfo)
			{
				methodInfo = (MethodInfo)obj;
			}
			RuntimeParameterInfo runtimeParameterInfo = obj as RuntimeParameterInfo;
			if (runtimeParameterInfo != null)
			{
				MemberInfo member = runtimeParameterInfo.Member;
				if (member is MethodInfo)
				{
					methodInfo = (MethodInfo)member;
					MethodInfo baseMethod = ((RuntimeMethodInfo)methodInfo).GetBaseMethod();
					if (baseMethod == methodInfo)
					{
						return null;
					}
					return baseMethod.GetParameters()[runtimeParameterInfo.Position];
				}
			}
			if (methodInfo == null || !methodInfo.IsVirtual)
			{
				return null;
			}
			MethodInfo baseMethod2 = ((RuntimeMethodInfo)methodInfo).GetBaseMethod();
			if (baseMethod2 == methodInfo)
			{
				return null;
			}
			return baseMethod2;
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x00060F90 File Offset: 0x0005F190
		private static AttributeUsageAttribute RetrieveAttributeUsageNoCache(Type attributeType)
		{
			if (attributeType == typeof(AttributeUsageAttribute))
			{
				return new AttributeUsageAttribute(AttributeTargets.Class);
			}
			AttributeUsageAttribute attributeUsageAttribute = null;
			object[] customAttributes = MonoCustomAttrs.GetCustomAttributes(attributeType, typeof(AttributeUsageAttribute), false);
			if (customAttributes.Length == 0)
			{
				if (attributeType.BaseType != null)
				{
					attributeUsageAttribute = MonoCustomAttrs.RetrieveAttributeUsage(attributeType.BaseType);
				}
				if (attributeUsageAttribute != null)
				{
					return attributeUsageAttribute;
				}
				return MonoCustomAttrs.DefaultAttributeUsage;
			}
			else
			{
				if (customAttributes.Length > 1)
				{
					throw new FormatException("Duplicate AttributeUsageAttribute cannot be specified on an attribute type.");
				}
				return (AttributeUsageAttribute)customAttributes[0];
			}
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x0006100C File Offset: 0x0005F20C
		private static AttributeUsageAttribute RetrieveAttributeUsage(Type attributeType)
		{
			AttributeUsageAttribute attributeUsageAttribute = null;
			if (MonoCustomAttrs.usage_cache == null)
			{
				MonoCustomAttrs.usage_cache = new Dictionary<Type, AttributeUsageAttribute>();
			}
			if (MonoCustomAttrs.usage_cache.TryGetValue(attributeType, out attributeUsageAttribute))
			{
				return attributeUsageAttribute;
			}
			attributeUsageAttribute = MonoCustomAttrs.RetrieveAttributeUsageNoCache(attributeType);
			MonoCustomAttrs.usage_cache[attributeType] = attributeUsageAttribute;
			return attributeUsageAttribute;
		}

		// Token: 0x0400172B RID: 5931
		private static Assembly corlib;

		// Token: 0x0400172C RID: 5932
		[ThreadStatic]
		private static Dictionary<Type, AttributeUsageAttribute> usage_cache;

		// Token: 0x0400172D RID: 5933
		private static readonly AttributeUsageAttribute DefaultAttributeUsage = new AttributeUsageAttribute(AttributeTargets.All);

		// Token: 0x02000240 RID: 576
		private class AttributeInfo
		{
			// Token: 0x06001A49 RID: 6729 RVA: 0x00061062 File Offset: 0x0005F262
			public AttributeInfo(AttributeUsageAttribute usage, int inheritanceLevel)
			{
				this._usage = usage;
				this._inheritanceLevel = inheritanceLevel;
			}

			// Token: 0x170002F4 RID: 756
			// (get) Token: 0x06001A4A RID: 6730 RVA: 0x00061078 File Offset: 0x0005F278
			public AttributeUsageAttribute Usage
			{
				get
				{
					return this._usage;
				}
			}

			// Token: 0x170002F5 RID: 757
			// (get) Token: 0x06001A4B RID: 6731 RVA: 0x00061080 File Offset: 0x0005F280
			public int InheritanceLevel
			{
				get
				{
					return this._inheritanceLevel;
				}
			}

			// Token: 0x0400172E RID: 5934
			private AttributeUsageAttribute _usage;

			// Token: 0x0400172F RID: 5935
			private int _inheritanceLevel;
		}
	}
}
