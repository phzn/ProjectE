using System;
using System.Threading;

namespace UnityEngine.Localization.Tables
{
	// Token: 0x0200002C RID: 44
	[Serializable]
	public class DistributedUIDGenerator : IKeyGenerator
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00005C5E File Offset: 0x00003E5E
		public long CustomEpoch
		{
			get
			{
				return this.m_CustomEpoch;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00005C66 File Offset: 0x00003E66
		// (set) Token: 0x0600016D RID: 365 RVA: 0x00005C81 File Offset: 0x00003E81
		public int MachineId
		{
			get
			{
				if (this.m_MachineId == 0)
				{
					this.m_MachineId = DistributedUIDGenerator.GetMachineId();
				}
				return this.m_MachineId;
			}
			set
			{
				this.m_MachineId = Mathf.Clamp(value, 1, DistributedUIDGenerator.kMaxNodeId);
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00005C98 File Offset: 0x00003E98
		public DistributedUIDGenerator()
		{
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00005CC8 File Offset: 0x00003EC8
		public DistributedUIDGenerator(long customEpoch)
		{
			this.m_CustomEpoch = customEpoch;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00005D00 File Offset: 0x00003F00
		public long GetNextKey()
		{
			long num = this.TimeStamp();
			if (num == this.m_LastTimestamp)
			{
				this.m_Sequence = (this.m_Sequence + 1L) & (long)DistributedUIDGenerator.kMaxSequence;
				if (this.m_Sequence == 0L)
				{
					num = this.WaitNextMillis(num);
				}
			}
			else
			{
				this.m_Sequence = 0L;
			}
			this.m_LastTimestamp = num;
			return (num << 22) | (long)((ulong)((ulong)this.MachineId << 12)) | this.m_Sequence;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00005D6C File Offset: 0x00003F6C
		private long TimeStamp()
		{
			return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - this.m_CustomEpoch;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00005D8D File Offset: 0x00003F8D
		private static int GetMachineId()
		{
			return Random.Range(0, DistributedUIDGenerator.kMaxNodeId);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00005D9A File Offset: 0x00003F9A
		private long WaitNextMillis(long currentTimestamp)
		{
			while (currentTimestamp == this.m_LastTimestamp)
			{
				Thread.Sleep(1);
				currentTimestamp = this.TimeStamp();
			}
			return currentTimestamp;
		}

		// Token: 0x0400004F RID: 79
		private const int kMachineIdBits = 10;

		// Token: 0x04000050 RID: 80
		private const int kSequenceBits = 12;

		// Token: 0x04000051 RID: 81
		private static readonly int kMaxNodeId = (int)(Mathf.Pow(2f, 10f) - 1f);

		// Token: 0x04000052 RID: 82
		private static readonly int kMaxSequence = (int)(Mathf.Pow(2f, 12f) - 1f);

		// Token: 0x04000053 RID: 83
		public const string MachineIdPrefKey = "KeyGenerator-MachineId";

		// Token: 0x04000054 RID: 84
		[SerializeField]
		[HideInInspector]
		private long m_CustomEpoch = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

		// Token: 0x04000055 RID: 85
		private long m_LastTimestamp = -1L;

		// Token: 0x04000056 RID: 86
		private long m_Sequence;

		// Token: 0x04000057 RID: 87
		private int m_MachineId;
	}
}
