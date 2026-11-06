using System;
using System.Runtime.CompilerServices;

namespace Pathfinding.Collections
{
	// Token: 0x02000274 RID: 628
	public static class SlabListExtensions
	{
		// Token: 0x06000FB4 RID: 4020 RVA: 0x0005FB5C File Offset: 0x0005DD5C
		public static void Remove<[IsUnmanaged] T>(this SlabAllocator<T>.List list, T value) where T : struct, ValueType, IEquatable<T>
		{
			int num = list.span.IndexOf(value);
			if (num != -1)
			{
				list.RemoveAt(num);
			}
		}
	}
}
