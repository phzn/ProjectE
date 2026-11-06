using System;

namespace Unity.Entities
{
	// Token: 0x02000047 RID: 71
	internal sealed class UnsafeMatchingArchetypePtrListDebugView
	{
		// Token: 0x06000208 RID: 520 RVA: 0x0000A5B7 File Offset: 0x000087B7
		public UnsafeMatchingArchetypePtrListDebugView(UnsafeMatchingArchetypePtrList MatchingArchetypeList)
		{
			this.m_MatchingArchetypeList = MatchingArchetypeList;
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000A5C8 File Offset: 0x000087C8
		public unsafe MatchingArchetype*[] Items
		{
			get
			{
				MatchingArchetype*[] array = new MatchingArchetype*[this.m_MatchingArchetypeList.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = *(IntPtr*)(this.m_MatchingArchetypeList.Ptr + (IntPtr)i * (IntPtr)sizeof(MatchingArchetype*) / (IntPtr)sizeof(MatchingArchetype*));
				}
				return array;
			}
		}

		// Token: 0x04000088 RID: 136
		private UnsafeMatchingArchetypePtrList m_MatchingArchetypeList;
	}
}
