using System;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000035 RID: 53
	public abstract class InputProcessor
	{
		// Token: 0x06000465 RID: 1125
		public abstract object ProcessAsObject(object value, InputControl control);

		// Token: 0x06000466 RID: 1126
		public unsafe abstract void Process(void* buffer, int bufferSize, InputControl control);

		// Token: 0x06000467 RID: 1127 RVA: 0x0001378C File Offset: 0x0001198C
		internal static Type GetValueTypeFromType(Type processorType)
		{
			if (processorType == null)
			{
				throw new ArgumentNullException("processorType");
			}
			return TypeHelpers.GetGenericTypeArgumentFromHierarchy(processorType, typeof(InputProcessor<>), 0);
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x000137B3 File Offset: 0x000119B3
		public virtual InputProcessor.CachingPolicy cachingPolicy
		{
			get
			{
				return InputProcessor.CachingPolicy.CacheResult;
			}
		}

		// Token: 0x0400014D RID: 333
		internal static TypeTable s_Processors;

		// Token: 0x0200019B RID: 411
		public enum CachingPolicy
		{
			// Token: 0x040008F1 RID: 2289
			CacheResult,
			// Token: 0x040008F2 RID: 2290
			EvaluateOnEveryRead
		}
	}
}
