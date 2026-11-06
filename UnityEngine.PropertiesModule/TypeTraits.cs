using System;

namespace Unity.Properties
{
	// Token: 0x0200008A RID: 138
	public static class TypeTraits
	{
		// Token: 0x060002F4 RID: 756 RVA: 0x0000AD70 File Offset: 0x00008F70
		public static bool IsContainer(Type type)
		{
			bool flag = null == type;
			if (flag)
			{
				throw new ArgumentNullException("type");
			}
			return !type.IsPrimitive && !type.IsPointer && !type.IsEnum && !(type == typeof(string));
		}
	}
}
