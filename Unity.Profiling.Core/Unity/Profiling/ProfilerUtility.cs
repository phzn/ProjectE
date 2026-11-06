using System;

namespace Unity.Profiling
{
	// Token: 0x0200000B RID: 11
	internal struct ProfilerUtility
	{
		// Token: 0x06000025 RID: 37 RVA: 0x00002368 File Offset: 0x00000568
		public static byte GetProfilerMarkerDataType<T>()
		{
			switch (Type.GetTypeCode(typeof(T)))
			{
			case TypeCode.Int32:
				return 2;
			case TypeCode.UInt32:
				return 3;
			case TypeCode.Int64:
				return 4;
			case TypeCode.UInt64:
				return 5;
			case TypeCode.Single:
				return 6;
			case TypeCode.Double:
				return 7;
			case TypeCode.String:
				return 9;
			}
			throw new ArgumentException(string.Format("Type {0} is unsupported by ProfilerCounter.", typeof(T)));
		}
	}
}
