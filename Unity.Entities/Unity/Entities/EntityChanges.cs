using System;

namespace Unity.Entities
{
	// Token: 0x02000074 RID: 116
	public readonly struct EntityChanges : IDisposable
	{
		// Token: 0x060002BF RID: 703 RVA: 0x0000C7DD File Offset: 0x0000A9DD
		public EntityChanges(EntityChangeSet forwardChangeSet, EntityChangeSet reverseChangeSet)
		{
			this.m_ForwardChangeSet = forwardChangeSet;
			this.m_ReverseChangeSet = reverseChangeSet;
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000C7ED File Offset: 0x0000A9ED
		public bool AnyChanges
		{
			get
			{
				return this.HasForwardChangeSet || this.HasReverseChangeSet;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000C7FF File Offset: 0x0000A9FF
		public bool HasForwardChangeSet
		{
			get
			{
				return this.m_ForwardChangeSet.IsCreated && this.m_ForwardChangeSet.HasChanges;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000C81B File Offset: 0x0000AA1B
		public bool HasReverseChangeSet
		{
			get
			{
				return this.m_ReverseChangeSet.IsCreated && this.m_ReverseChangeSet.HasChanges;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x0000C837 File Offset: 0x0000AA37
		public EntityChangeSet ForwardChangeSet
		{
			get
			{
				return this.m_ForwardChangeSet;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000C83F File Offset: 0x0000AA3F
		public EntityChangeSet ReverseChangeSet
		{
			get
			{
				return this.m_ReverseChangeSet;
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000C847 File Offset: 0x0000AA47
		public void Dispose()
		{
			if (this.m_ForwardChangeSet.IsCreated)
			{
				this.m_ForwardChangeSet.Dispose();
			}
			if (this.m_ReverseChangeSet.IsCreated)
			{
				this.m_ReverseChangeSet.Dispose();
			}
		}

		// Token: 0x040000BB RID: 187
		private readonly EntityChangeSet m_ForwardChangeSet;

		// Token: 0x040000BC RID: 188
		private readonly EntityChangeSet m_ReverseChangeSet;
	}
}
