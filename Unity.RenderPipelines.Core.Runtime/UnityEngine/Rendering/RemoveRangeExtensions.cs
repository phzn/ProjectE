using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace UnityEngine.Rendering
{
	// Token: 0x02000055 RID: 85
	public static class RemoveRangeExtensions
	{
		// Token: 0x060002C7 RID: 711 RVA: 0x0000C848 File Offset: 0x0000AA48
		[CollectionAccess(CollectionAccessType.ModifyExistingContent)]
		[MustUseReturnValue]
		public static bool TryRemoveElementsInRange<TValue>([DisallowNull] this IList<TValue> list, int index, int count, [NotNullWhen(false)] out Exception error)
		{
			try
			{
				List<TValue> list2 = list as List<TValue>;
				if (list2 != null)
				{
					list2.RemoveRange(index, count);
				}
				else
				{
					if (index < 0)
					{
						throw new ArgumentOutOfRangeException("index");
					}
					if (count < 0)
					{
						throw new ArgumentOutOfRangeException("count");
					}
					if (list.Count - index < count)
					{
						throw new ArgumentException("index and count do not denote a valid range of elements in the list");
					}
					for (int i = count; i > 0; i--)
					{
						list.RemoveAt(index);
					}
				}
			}
			catch (Exception ex)
			{
				error = ex;
				return false;
			}
			error = null;
			return true;
		}
	}
}
