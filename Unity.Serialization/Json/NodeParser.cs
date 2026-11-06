using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x02000047 RID: 71
	internal struct NodeParser : IDisposable
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00006BEC File Offset: 0x00004DEC
		public unsafe int* Nodes
		{
			get
			{
				return this.m_Data->Nodes;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00006BF9 File Offset: 0x00004DF9
		public unsafe int NodeNextIndex
		{
			get
			{
				return this.m_Data->NodeNextIndex;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00006C06 File Offset: 0x00004E06
		public unsafe NodeType NodeType
		{
			get
			{
				return this.m_Data->NodeType;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00006C13 File Offset: 0x00004E13
		public unsafe int Node
		{
			get
			{
				if (this.m_Data->NodeNextIndex > 0)
				{
					return this.m_Data->Nodes[this.m_Data->NodeNextIndex - 1];
				}
				return -1;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00006C42 File Offset: 0x00004E42
		public unsafe int TokenNextIndex
		{
			get
			{
				return this.m_Data->TokenNextIndex;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00006C4F File Offset: 0x00004E4F
		public unsafe int TokenParentIndex
		{
			get
			{
				return this.m_Data->TokenParentIndex;
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00006C5C File Offset: 0x00004E5C
		public NodeParser(JsonTokenStream stream, Allocator label)
		{
			this = new NodeParser(stream, 64, label);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00006C68 File Offset: 0x00004E68
		public unsafe NodeParser(JsonTokenStream stream, int batchSize, Allocator label)
		{
			if (batchSize < 1)
			{
				throw new ArgumentException("batchSize < 1");
			}
			this.m_Label = label;
			this.m_Data = (UnsafeNodeParser*)UnsafeUtility.Malloc((long)sizeof(UnsafeNodeParser), UnsafeUtility.AlignOf<UnsafeNodeParser>(), label);
			*this.m_Data = new UnsafeNodeParser(stream.GetUnsafePtr(), batchSize, label);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00006CBC File Offset: 0x00004EBC
		public unsafe void Reset()
		{
			this.m_Data->Reset();
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00006CC9 File Offset: 0x00004EC9
		public unsafe void Seek(int index, int parent)
		{
			this.m_Data->Seek(index, parent);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00006CD8 File Offset: 0x00004ED8
		public unsafe NodeType Step()
		{
			return this.m_Data->Step();
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00006CE5 File Offset: 0x00004EE5
		public unsafe void Step(NodeType type, int parent = -2)
		{
			this.m_Data->Step(type, parent);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00006CF4 File Offset: 0x00004EF4
		public unsafe int StepBatch(int count, NodeType type, int parent = -2)
		{
			return this.m_Data->StepBatch(count, type, parent);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00006D04 File Offset: 0x00004F04
		public unsafe void Dispose()
		{
			this.m_Data->Dispose();
			UnsafeUtility.Free((void*)this.m_Data, this.m_Label);
			this.m_Data = null;
		}

		// Token: 0x040000BA RID: 186
		public const int IgnoreParent = -2;

		// Token: 0x040000BB RID: 187
		private const int k_DefaultBatchSize = 64;

		// Token: 0x040000BC RID: 188
		private readonly Allocator m_Label;

		// Token: 0x040000BD RID: 189
		[NativeDisableUnsafePtrRestriction]
		private unsafe UnsafeNodeParser* m_Data;
	}
}
