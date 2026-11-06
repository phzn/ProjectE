using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x02000046 RID: 70
	internal struct UnsafeNodeParser : IDisposable
	{
		// Token: 0x0600017F RID: 383 RVA: 0x00006698 File Offset: 0x00004898
		public unsafe UnsafeNodeParser(UnsafeJsonTokenStream* stream, int batchSize, Allocator label)
		{
			this.m_Label = label;
			this.m_Stream = stream;
			this.TokenNextIndex = 0;
			this.TokenParentIndex = -1;
			this.Nodes = (int*)UnsafeUtility.Malloc((long)(4 * batchSize), 4, this.m_Label);
			this.NodeLength = batchSize;
			this.NodeNextIndex = 0;
			this.NodeType = NodeType.None;
			this.m_TargetNodeCount = 0;
			this.m_TargetNodeType = NodeType.None;
			this.m_TargetParentIndex = -1;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00006701 File Offset: 0x00004901
		public unsafe void Dispose()
		{
			UnsafeUtility.Free((void*)this.Nodes, this.m_Label);
			this.Nodes = null;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000671C File Offset: 0x0000491C
		public void Reset()
		{
			this.NodeNextIndex = 0;
			this.NodeType = NodeType.None;
			this.TokenNextIndex = 0;
			this.TokenParentIndex = -1;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000673A File Offset: 0x0000493A
		public void Seek(int index, int parent)
		{
			this.TokenNextIndex = index;
			this.TokenParentIndex = parent;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000674A File Offset: 0x0000494A
		public NodeType Step()
		{
			this.Step(NodeType.Any, -2);
			return this.NodeType;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000675B File Offset: 0x0000495B
		public void Step(NodeType type, int parent = -2)
		{
			this.StepBatch(1, type, parent);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00006768 File Offset: 0x00004968
		public unsafe int StepBatch(int count, NodeType type, int parent = -2)
		{
			this.NodeNextIndex = 0;
			this.m_TargetNodeCount = count;
			this.m_TargetNodeType = type;
			this.m_TargetParentIndex = parent;
			if (this.NodeLength < this.m_TargetNodeCount)
			{
				UnsafeUtility.Free((void*)this.Nodes, this.m_Label);
				this.NodeLength = this.m_TargetNodeCount;
				this.Nodes = (int*)UnsafeUtility.Malloc((long)(4 * this.NodeLength), 4, this.m_Label);
			}
			while (this.TokenNextIndex < this.m_Stream->TokenNextIndex)
			{
				NodeType nodeType = NodeType.None;
				Token token = this.m_Stream->Tokens[this.TokenNextIndex];
				while (this.m_Stream->Tokens[this.TokenNextIndex].Parent < this.TokenParentIndex)
				{
					int tokenParentIndex = this.TokenParentIndex;
					nodeType = this.PopToken();
					if (this.Evaluate(nodeType, tokenParentIndex))
					{
						if (this.TokenParentIndex < this.m_TargetParentIndex)
						{
							this.TokenParentIndex = tokenParentIndex;
						}
						this.NodeType = ((nodeType == NodeType.None) ? NodeType.Any : nodeType);
						return this.NodeNextIndex;
					}
				}
				int num = this.TokenNextIndex;
				switch (token.Type)
				{
				case TokenType.Object:
				case TokenType.Array:
					nodeType |= ((token.Type == TokenType.Array) ? NodeType.BeginArray : NodeType.BeginObject);
					this.TokenParentIndex = this.TokenNextIndex;
					break;
				case TokenType.String:
				case TokenType.Primitive:
					if (token.End != -1)
					{
						nodeType |= ((token.Type == TokenType.Primitive) ? NodeType.Primitive : NodeType.String);
						while (token.Start == -1)
						{
							num = token.Parent;
							token = this.m_Stream->Tokens[num];
						}
						if (token.Parent == -1 || this.m_Stream->Tokens[token.Parent].Type == TokenType.Object)
						{
							nodeType |= NodeType.ObjectKey;
							this.TokenParentIndex = this.TokenNextIndex;
						}
					}
					break;
				case TokenType.Comment:
					if (token.End != -1)
					{
						nodeType |= NodeType.Comment;
						while (token.Start == -1)
						{
							num = token.Parent;
							token = this.m_Stream->Tokens[num];
						}
					}
					break;
				}
				if (this.Evaluate(nodeType, num))
				{
					this.TokenNextIndex++;
					this.NodeType = ((nodeType == NodeType.None) ? NodeType.Any : nodeType);
					return this.NodeNextIndex;
				}
				this.TokenNextIndex++;
			}
			while (this.TokenParentIndex >= 0)
			{
				int tokenParentIndex2 = this.TokenParentIndex;
				Token token2 = this.m_Stream->Tokens[tokenParentIndex2];
				if (token2.End == -1 && (token2.Type == TokenType.Object || token2.Type == TokenType.Array))
				{
					this.NodeType = NodeType.None;
					return this.NodeNextIndex;
				}
				NodeType nodeType2 = this.PopToken();
				if (this.Evaluate(nodeType2, tokenParentIndex2))
				{
					if (this.TokenParentIndex < parent)
					{
						this.TokenParentIndex = tokenParentIndex2;
					}
					this.NodeType = ((nodeType2 == NodeType.None) ? NodeType.Any : nodeType2);
					return this.NodeNextIndex;
				}
			}
			this.NodeType = NodeType.None;
			return this.NodeNextIndex;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00006A80 File Offset: 0x00004C80
		private unsafe bool Evaluate(NodeType node, int index)
		{
			if (this.TokenParentIndex <= this.m_TargetParentIndex)
			{
				if (node == NodeType.None || ((node & this.m_TargetNodeType) == node && this.TokenParentIndex == this.m_TargetParentIndex))
				{
					ref int nodes = ref *this.Nodes;
					int num = this.NodeNextIndex;
					this.NodeNextIndex = num + 1;
					*((ref nodes) + (IntPtr)num * 4) = index;
					if (this.NodeNextIndex < this.m_TargetNodeCount)
					{
						return false;
					}
				}
				return true;
			}
			if (node == NodeType.None)
			{
				return false;
			}
			if ((node & this.m_TargetNodeType) != NodeType.None && (this.m_TargetParentIndex == -2 || this.m_TargetParentIndex >= this.TokenParentIndex))
			{
				ref int nodes2 = ref *this.Nodes;
				int num = this.NodeNextIndex;
				this.NodeNextIndex = num + 1;
				*((ref nodes2) + (IntPtr)num * 4) = index;
				if (this.NodeNextIndex >= this.m_TargetNodeCount)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00006B3C File Offset: 0x00004D3C
		private unsafe NodeType PopToken()
		{
			NodeType nodeType = NodeType.None;
			Token token = this.m_Stream->Tokens[this.TokenParentIndex];
			TokenType type = token.Type;
			if (type != TokenType.Object)
			{
				if (type == TokenType.Array)
				{
					nodeType = NodeType.EndArray;
				}
			}
			else
			{
				nodeType = NodeType.EndObject;
			}
			int i;
			Token token2;
			for (i = token.Parent; i >= 0; i = token2.Parent)
			{
				token2 = this.m_Stream->Tokens[i];
				if ((token2.Type != TokenType.Primitive && token2.Type != TokenType.String && token2.Type != TokenType.Comment) || token2.Start != -1 || token2.Parent == -1)
				{
					break;
				}
			}
			this.TokenParentIndex = i;
			return nodeType;
		}

		// Token: 0x040000AE RID: 174
		public const int IgnoreParent = -2;

		// Token: 0x040000AF RID: 175
		private readonly Allocator m_Label;

		// Token: 0x040000B0 RID: 176
		private unsafe readonly UnsafeJsonTokenStream* m_Stream;

		// Token: 0x040000B1 RID: 177
		public int TokenNextIndex;

		// Token: 0x040000B2 RID: 178
		public int TokenParentIndex;

		// Token: 0x040000B3 RID: 179
		public unsafe int* Nodes;

		// Token: 0x040000B4 RID: 180
		public int NodeLength;

		// Token: 0x040000B5 RID: 181
		public int NodeNextIndex;

		// Token: 0x040000B6 RID: 182
		public NodeType NodeType;

		// Token: 0x040000B7 RID: 183
		private int m_TargetNodeCount;

		// Token: 0x040000B8 RID: 184
		private NodeType m_TargetNodeType;

		// Token: 0x040000B9 RID: 185
		private int m_TargetParentIndex;
	}
}
