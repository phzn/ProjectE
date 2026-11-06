using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001BE RID: 446
	public static class MMF_FieldInfo
	{
		// Token: 0x06000A9A RID: 2714 RVA: 0x0002FB2C File Offset: 0x0002DD2C
		public static int GetFieldInfo(MMF_Feedback target, out List<FieldInfo> fieldInfoList)
		{
			Type type = target.GetType();
			int hashCode = type.GetHashCode();
			if (!MMF_FieldInfo.FieldInfoList.TryGetValue(hashCode, out fieldInfoList))
			{
				IList<Type> typeTree = type.GetBaseTypes();
				fieldInfoList = (from x in target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
					orderby typeTree.IndexOf(x.DeclaringType) descending
					select x).ToList<FieldInfo>();
				MMF_FieldInfo.FieldInfoList.Add(hashCode, fieldInfoList);
			}
			return fieldInfoList.Count;
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0002FBA0 File Offset: 0x0002DDA0
		public static int GetFieldInfo(Object target, out List<FieldInfo> fieldInfoList)
		{
			Type type = target.GetType();
			int hashCode = type.GetHashCode();
			if (!MMF_FieldInfo.FieldInfoList.TryGetValue(hashCode, out fieldInfoList))
			{
				IList<Type> typeTree = type.GetBaseTypes();
				fieldInfoList = (from x in target.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
					orderby typeTree.IndexOf(x.DeclaringType) descending
					select x).ToList<FieldInfo>();
				MMF_FieldInfo.FieldInfoList.Add(hashCode, fieldInfoList);
			}
			return fieldInfoList.Count;
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0002FC14 File Offset: 0x0002DE14
		public static IList<Type> GetBaseTypes(this Type t)
		{
			List<Type> list = new List<Type>();
			while (t.BaseType != null)
			{
				list.Add(t);
				t = t.BaseType;
			}
			return list;
		}

		// Token: 0x04000C99 RID: 3225
		public static Dictionary<int, List<FieldInfo>> FieldInfoList = new Dictionary<int, List<FieldInfo>>();
	}
}
