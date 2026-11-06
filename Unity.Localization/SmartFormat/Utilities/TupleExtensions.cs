using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnityEngine.Localization.SmartFormat.Utilities
{
	// Token: 0x0200004A RID: 74
	public static class TupleExtensions
	{
		// Token: 0x0600028D RID: 653 RVA: 0x00009FC7 File Offset: 0x000081C7
		public static bool IsValueTuple(this object obj)
		{
			return obj.GetType().IsValueTupleType();
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00009FD4 File Offset: 0x000081D4
		public static bool IsValueTupleType(this Type type)
		{
			return type.GetTypeInfo().IsGenericType && TupleExtensions.ValueTupleTypes.Contains(type.GetGenericTypeDefinition());
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00009FF8 File Offset: 0x000081F8
		public static IEnumerable<object> GetValueTupleItemObjects(this object tuple)
		{
			return from f in tuple.GetType().GetValueTupleItemFields()
				select f.GetValue(tuple);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000A033 File Offset: 0x00008233
		public static IEnumerable<Type> GetValueTupleItemTypes(this Type tupleType)
		{
			return from f in tupleType.GetValueTupleItemFields()
				select f.FieldType;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000A060 File Offset: 0x00008260
		public static List<FieldInfo> GetValueTupleItemFields(this Type tupleType)
		{
			List<FieldInfo> list = new List<FieldInfo>();
			int num = 1;
			FieldInfo runtimeField;
			while ((runtimeField = tupleType.GetRuntimeField(string.Format("Item{0}", num))) != null)
			{
				num++;
				list.Add(runtimeField);
			}
			return list;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000A0A3 File Offset: 0x000082A3
		public static IEnumerable<object> GetValueTupleItemObjectsFlattened(this object tuple)
		{
			foreach (object obj in tuple.GetValueTupleItemObjects())
			{
				if (obj.IsValueTuple())
				{
					foreach (object obj2 in obj.GetValueTupleItemObjectsFlattened())
					{
						yield return obj2;
					}
					IEnumerator<object> enumerator2 = null;
				}
				else
				{
					yield return obj;
				}
			}
			IEnumerator<object> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040000B6 RID: 182
		private static readonly HashSet<Type> ValueTupleTypes = new HashSet<Type>(new Type[]
		{
			typeof(ValueTuple<>),
			typeof(ValueTuple<, >),
			typeof(ValueTuple<, , >),
			typeof(ValueTuple<, , , >),
			typeof(ValueTuple<, , , , >),
			typeof(ValueTuple<, , , , , >),
			typeof(ValueTuple<, , , , , , >),
			typeof(ValueTuple<, , , , , , , >)
		});
	}
}
