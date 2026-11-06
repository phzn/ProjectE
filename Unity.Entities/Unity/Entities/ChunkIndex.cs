using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Unity.Entities
{
	// Token: 0x020002F0 RID: 752
	[DebuggerTypeProxy(typeof(ChunkIndexDebugProxy))]
	internal struct ChunkIndex : IComparable<ChunkIndex>
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x0004E100 File Offset: 0x0004C300
		public static ChunkIndex Null
		{
			get
			{
				return default(ChunkIndex);
			}
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x0004E116 File Offset: 0x0004C316
		public ChunkIndex(int value)
		{
			this.Value = value;
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0004E11F File Offset: 0x0004C31F
		public static implicit operator int(ChunkIndex index)
		{
			return index.Value;
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0004E127 File Offset: 0x0004C327
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe Chunk* GetPtr()
		{
			return EntityComponentStore.s_chunkStore.Data.GetChunkPointer(this.Value);
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x0004E13E File Offset: 0x0004C33E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe bool MatchesFilter(MatchingArchetype* match, ref EntityQueryFilter filter)
		{
			return match->ChunkMatchesFilter(this.ListIndex, ref filter);
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x0004E14D File Offset: 0x0004C34D
		public static bool operator ==(ChunkIndex a, ChunkIndex b)
		{
			return a.Value == b.Value;
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0004E15D File Offset: 0x0004C35D
		public static bool operator !=(ChunkIndex a, ChunkIndex b)
		{
			return !(a == b);
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x0004E169 File Offset: 0x0004C369
		public static bool operator <(ChunkIndex a, ChunkIndex b)
		{
			return a.Value < b.Value;
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x0004E179 File Offset: 0x0004C379
		public static bool operator >(ChunkIndex a, ChunkIndex b)
		{
			return a.Value > b.Value;
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x0004E14D File Offset: 0x0004C34D
		public bool Equals(ChunkIndex other)
		{
			return this.Value == other.Value;
		}

		// Token: 0x0600143E RID: 5182 RVA: 0x0004E18C File Offset: 0x0004C38C
		public override bool Equals(object obj)
		{
			if (obj is ChunkIndex)
			{
				ChunkIndex chunkIndex = (ChunkIndex)obj;
				return this.Equals(chunkIndex);
			}
			return false;
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x0004E11F File Offset: 0x0004C31F
		public override int GetHashCode()
		{
			return this.Value;
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06001440 RID: 5184 RVA: 0x0004E1B1 File Offset: 0x0004C3B1
		// (set) Token: 0x06001441 RID: 5185 RVA: 0x0004E1BE File Offset: 0x0004C3BE
		internal unsafe ulong SequenceNumber
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.GetPtr()->SequenceNumber;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.GetPtr()->SequenceNumber = value;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06001442 RID: 5186 RVA: 0x0004E1CC File Offset: 0x0004C3CC
		// (set) Token: 0x06001443 RID: 5187 RVA: 0x0004E1E7 File Offset: 0x0004C3E7
		internal unsafe int ListIndex
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return EntityComponentStore.PerChunkArray.ChunkData[this.Value].ListIndex;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				EntityComponentStore.PerChunkArray.ChunkData[this.Value].ListIndex = value;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06001444 RID: 5188 RVA: 0x0004E203 File Offset: 0x0004C403
		// (set) Token: 0x06001445 RID: 5189 RVA: 0x0004E21E File Offset: 0x0004C41E
		internal unsafe int Count
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return EntityComponentStore.PerChunkArray.ChunkData[this.Value].EntityCount;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				EntityComponentStore.PerChunkArray.ChunkData[this.Value].EntityCount = value;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06001446 RID: 5190 RVA: 0x0004E23A File Offset: 0x0004C43A
		// (set) Token: 0x06001447 RID: 5191 RVA: 0x0004E247 File Offset: 0x0004C447
		internal unsafe int ListWithEmptySlotsIndex
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.GetPtr()->ListWithEmptySlotsIndex;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.GetPtr()->ListWithEmptySlotsIndex = value;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06001448 RID: 5192 RVA: 0x0004E255 File Offset: 0x0004C455
		// (set) Token: 0x06001449 RID: 5193 RVA: 0x0004E262 File Offset: 0x0004C462
		internal unsafe Entity MetaChunkEntity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.GetPtr()->metaChunkEntity;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.GetPtr()->metaChunkEntity = value;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600144A RID: 5194 RVA: 0x0004E270 File Offset: 0x0004C470
		internal unsafe byte* Buffer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return &this.GetPtr()->Buffer.FixedElementField;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600144B RID: 5195 RVA: 0x0004E283 File Offset: 0x0004C483
		// (set) Token: 0x0600144C RID: 5196 RVA: 0x0004E290 File Offset: 0x0004C490
		internal unsafe uint Flags
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.GetPtr()->Flags;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.GetPtr()->Flags = value;
			}
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x0004E29E File Offset: 0x0004C49E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(ChunkIndex other)
		{
			return this.Value.CompareTo(other.Value);
		}

		// Token: 0x0400076F RID: 1903
		private int Value;
	}
}
