using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020002EE RID: 750
	internal static class CalculateReaderWriterDependency
	{
		// Token: 0x06001431 RID: 5169 RVA: 0x0004E058 File Offset: 0x0004C258
		public static bool Add(ComponentType type, ref UnsafeList<TypeIndex> reading, ref UnsafeList<TypeIndex> writing)
		{
			if (type.IsZeroSized && !type.IsEnableable)
			{
				return false;
			}
			if (type.AccessModeType == ComponentType.AccessMode.ReadOnly)
			{
				return CalculateReaderWriterDependency.AddReaderTypeIndex(type.TypeIndex, ref reading, ref writing);
			}
			return CalculateReaderWriterDependency.AddWriterTypeIndex(type.TypeIndex, ref reading, ref writing);
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x0004E092 File Offset: 0x0004C292
		public static bool AddReaderTypeIndex(TypeIndex typeIndex, ref UnsafeList<TypeIndex> reading, ref UnsafeList<TypeIndex> writing)
		{
			if (reading.Contains(typeIndex))
			{
				return false;
			}
			if (writing.Contains(typeIndex))
			{
				return false;
			}
			reading.Add(in typeIndex);
			return true;
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x0004E0C0 File Offset: 0x0004C2C0
		public static bool AddWriterTypeIndex(TypeIndex typeIndex, ref UnsafeList<TypeIndex> reading, ref UnsafeList<TypeIndex> writing)
		{
			if (writing.Contains(typeIndex))
			{
				return false;
			}
			int num = reading.IndexOf(typeIndex);
			if (num != -1)
			{
				reading.RemoveAtSwapBack(num);
			}
			writing.Add(in typeIndex);
			return true;
		}
	}
}
