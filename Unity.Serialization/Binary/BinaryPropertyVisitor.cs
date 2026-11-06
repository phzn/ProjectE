using System;

namespace Unity.Serialization.Binary
{
	// Token: 0x020000A6 RID: 166
	internal abstract class BinaryPropertyVisitor : BinaryAdapter
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x0001105C File Offset: 0x0000F25C
		// (set) Token: 0x06000434 RID: 1076 RVA: 0x00011064 File Offset: 0x0000F264
		internal bool IsLocked { get; private set; }

		// Token: 0x06000435 RID: 1077 RVA: 0x0001106D File Offset: 0x0000F26D
		internal BinaryPropertyVisitor.LockScope Lock()
		{
			return new BinaryPropertyVisitor.LockScope(this);
		}

		// Token: 0x04000200 RID: 512
		protected const byte k_TokenNone = 0;

		// Token: 0x04000201 RID: 513
		protected const byte k_TokenNull = 1;

		// Token: 0x04000202 RID: 514
		protected const byte k_TokenPolymorphic = 2;

		// Token: 0x04000203 RID: 515
		protected const byte k_TokenUnityEngineObjectReference = 3;

		// Token: 0x04000204 RID: 516
		protected const byte k_TokenSerializedReference = 4;

		// Token: 0x020000A7 RID: 167
		internal readonly struct LockScope : IDisposable
		{
			// Token: 0x06000437 RID: 1079 RVA: 0x0001107D File Offset: 0x0000F27D
			public LockScope(BinaryPropertyVisitor visitor)
			{
				this.m_Visitor = visitor;
				visitor.IsLocked = true;
			}

			// Token: 0x06000438 RID: 1080 RVA: 0x0001108D File Offset: 0x0000F28D
			public void Dispose()
			{
				this.m_Visitor.IsLocked = false;
			}

			// Token: 0x04000206 RID: 518
			private readonly BinaryPropertyVisitor m_Visitor;
		}
	}
}
