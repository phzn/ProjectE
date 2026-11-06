using System;

namespace System.Reflection
{
	// Token: 0x020008DA RID: 2266
	public static class PropertyInfoExtensions
	{
		// Token: 0x06004B7C RID: 19324 RVA: 0x000F087C File Offset: 0x000EEA7C
		public static MethodInfo[] GetAccessors(PropertyInfo property)
		{
			Requires.NotNull(property, "property");
			return property.GetAccessors();
		}

		// Token: 0x06004B7D RID: 19325 RVA: 0x000F088F File Offset: 0x000EEA8F
		public static MethodInfo[] GetAccessors(PropertyInfo property, bool nonPublic)
		{
			Requires.NotNull(property, "property");
			return property.GetAccessors(nonPublic);
		}

		// Token: 0x06004B7E RID: 19326 RVA: 0x000F08A3 File Offset: 0x000EEAA3
		public static MethodInfo GetGetMethod(PropertyInfo property)
		{
			Requires.NotNull(property, "property");
			return property.GetGetMethod();
		}

		// Token: 0x06004B7F RID: 19327 RVA: 0x000F08B6 File Offset: 0x000EEAB6
		public static MethodInfo GetGetMethod(PropertyInfo property, bool nonPublic)
		{
			Requires.NotNull(property, "property");
			return property.GetGetMethod(nonPublic);
		}

		// Token: 0x06004B80 RID: 19328 RVA: 0x000F08CA File Offset: 0x000EEACA
		public static MethodInfo GetSetMethod(PropertyInfo property)
		{
			Requires.NotNull(property, "property");
			return property.GetSetMethod();
		}

		// Token: 0x06004B81 RID: 19329 RVA: 0x000F08DD File Offset: 0x000EEADD
		public static MethodInfo GetSetMethod(PropertyInfo property, bool nonPublic)
		{
			Requires.NotNull(property, "property");
			return property.GetSetMethod(nonPublic);
		}
	}
}
