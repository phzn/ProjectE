using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel
{
	// Token: 0x0200005C RID: 92
	internal static class GenericServices
	{
		// Token: 0x06000249 RID: 585 RVA: 0x00006F8C File Offset: 0x0000518C
		internal static IList<Type> GetPureGenericParameters(this Type type)
		{
			Assumes.NotNull<Type>(type);
			if (type.IsGenericType && type.ContainsGenericParameters)
			{
				List<Type> pureGenericParameters = new List<Type>();
				GenericServices.TraverseGenericType(type, delegate(Type t)
				{
					if (t.IsGenericParameter)
					{
						pureGenericParameters.Add(t);
					}
				});
				return pureGenericParameters;
			}
			return Type.EmptyTypes;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00006FE0 File Offset: 0x000051E0
		internal static int GetPureGenericArity(this Type type)
		{
			Assumes.NotNull<Type>(type);
			int genericArity = 0;
			if (type.IsGenericType && type.ContainsGenericParameters)
			{
				new List<Type>();
				GenericServices.TraverseGenericType(type, delegate(Type t)
				{
					if (t.IsGenericParameter)
					{
						int genericArity2 = genericArity;
						genericArity = genericArity2 + 1;
					}
				});
			}
			return genericArity;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00007030 File Offset: 0x00005230
		private static void TraverseGenericType(Type type, Action<Type> onType)
		{
			if (type.IsGenericType)
			{
				Type[] genericArguments = type.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					GenericServices.TraverseGenericType(genericArguments[i], onType);
				}
			}
			onType(type);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000706A File Offset: 0x0000526A
		public static int[] GetGenericParametersOrder(Type type)
		{
			return (from parameter in type.GetPureGenericParameters()
				select parameter.GenericParameterPosition).ToArray<int>();
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000709C File Offset: 0x0000529C
		public static string GetGenericName(string originalGenericName, int[] genericParametersOrder, int genericArity)
		{
			string[] array = new string[genericArity];
			for (int i = 0; i < genericParametersOrder.Length; i++)
			{
				array[genericParametersOrder[i]] = string.Format(CultureInfo.InvariantCulture, "{{{0}}}", i);
			}
			IFormatProvider invariantCulture = CultureInfo.InvariantCulture;
			object[] array2 = array;
			return string.Format(invariantCulture, originalGenericName, array2);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x000070E8 File Offset: 0x000052E8
		public static T[] Reorder<T>(T[] original, int[] genericParametersOrder)
		{
			T[] array = new T[genericParametersOrder.Length];
			for (int i = 0; i < genericParametersOrder.Length; i++)
			{
				array[i] = original[genericParametersOrder[i]];
			}
			return array;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00007120 File Offset: 0x00005320
		public static IEnumerable<Type> CreateTypeSpecializations(this Type[] types, Type[] specializationTypes)
		{
			if (types == null)
			{
				return null;
			}
			return types.Select<Type, Type>((Type type) => type.CreateTypeSpecialization(specializationTypes));
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00007154 File Offset: 0x00005354
		public static Type CreateTypeSpecialization(this Type type, Type[] specializationTypes)
		{
			if (!type.ContainsGenericParameters)
			{
				return type;
			}
			if (type.IsGenericParameter)
			{
				return specializationTypes[type.GenericParameterPosition];
			}
			Type[] genericArguments = type.GetGenericArguments();
			Type[] array = new Type[genericArguments.Length];
			for (int i = 0; i < genericArguments.Length; i++)
			{
				Type type2 = genericArguments[i];
				array[i] = (type2.IsGenericParameter ? specializationTypes[type2.GenericParameterPosition] : type2);
			}
			return type.GetGenericTypeDefinition().MakeGenericType(array);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x000071C0 File Offset: 0x000053C0
		public static bool CanSpecialize(Type type, IEnumerable<Type> constraints, GenericParameterAttributes attributes)
		{
			return GenericServices.CanSpecialize(type, constraints) && GenericServices.CanSpecialize(type, attributes);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x000071D4 File Offset: 0x000053D4
		public static bool CanSpecialize(Type type, IEnumerable<Type> constraintTypes)
		{
			if (constraintTypes == null)
			{
				return true;
			}
			foreach (Type type2 in constraintTypes)
			{
				if (type2 != null && !type2.IsAssignableFrom(type))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00007234 File Offset: 0x00005434
		public static bool CanSpecialize(Type type, GenericParameterAttributes attributes)
		{
			if (attributes == GenericParameterAttributes.None)
			{
				return true;
			}
			if ((attributes & GenericParameterAttributes.ReferenceTypeConstraint) != GenericParameterAttributes.None && type.IsValueType)
			{
				return false;
			}
			if ((attributes & GenericParameterAttributes.DefaultConstructorConstraint) != GenericParameterAttributes.None && !type.IsValueType && type.GetConstructor(Type.EmptyTypes) == null)
			{
				return false;
			}
			if ((attributes & GenericParameterAttributes.NotNullableValueTypeConstraint) != GenericParameterAttributes.None)
			{
				if (!type.IsValueType)
				{
					return false;
				}
				if (Nullable.GetUnderlyingType(type) != null)
				{
					return false;
				}
			}
			return true;
		}
	}
}
