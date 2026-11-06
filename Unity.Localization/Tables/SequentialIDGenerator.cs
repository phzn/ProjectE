using System;

namespace UnityEngine.Localization.Tables
{
	// Token: 0x0200002E RID: 46
	public class SequentialIDGenerator : IKeyGenerator
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00005DEE File Offset: 0x00003FEE
		public long NextAvailableId
		{
			get
			{
				return this.m_NextAvailableId;
			}
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00005DF6 File Offset: 0x00003FF6
		public SequentialIDGenerator()
		{
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00005E06 File Offset: 0x00004006
		public SequentialIDGenerator(long startingId)
		{
			this.m_NextAvailableId = startingId;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00005E20 File Offset: 0x00004020
		public long GetNextKey()
		{
			long nextAvailableId = this.m_NextAvailableId;
			this.m_NextAvailableId = nextAvailableId + 1L;
			return nextAvailableId;
		}

		// Token: 0x04000058 RID: 88
		[SerializeField]
		private long m_NextAvailableId = 1L;
	}
}
