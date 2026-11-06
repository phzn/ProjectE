using System;

namespace System.Security.Cryptography
{
	// Token: 0x020004D6 RID: 1238
	public sealed class PbeParameters
	{
		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x06003168 RID: 12648 RVA: 0x000B6C98 File Offset: 0x000B4E98
		public PbeEncryptionAlgorithm EncryptionAlgorithm { get; }

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06003169 RID: 12649 RVA: 0x000B6CA0 File Offset: 0x000B4EA0
		public HashAlgorithmName HashAlgorithm { get; }

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x0600316A RID: 12650 RVA: 0x000B6CA8 File Offset: 0x000B4EA8
		public int IterationCount { get; }

		// Token: 0x0600316B RID: 12651 RVA: 0x000B6CB0 File Offset: 0x000B4EB0
		public PbeParameters(PbeEncryptionAlgorithm encryptionAlgorithm, HashAlgorithmName hashAlgorithm, int iterationCount)
		{
			if (iterationCount < 1)
			{
				throw new ArgumentOutOfRangeException("iterationCount", iterationCount, "Positive number required.");
			}
			this.EncryptionAlgorithm = encryptionAlgorithm;
			this.HashAlgorithm = hashAlgorithm;
			this.IterationCount = iterationCount;
		}
	}
}
