using System;
using System.IO;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Pooling;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Pathfinding.Serialization
{
	// Token: 0x02000243 RID: 579
	public class GraphSerializationContext
	{
		// Token: 0x06000E7B RID: 3707 RVA: 0x000592BC File Offset: 0x000574BC
		public GraphSerializationContext(BinaryReader reader, GraphNode[] id2NodeMapping, uint graphIndex, GraphMeta meta)
		{
			this.reader = reader;
			this.id2NodeMapping = id2NodeMapping;
			this.graphIndex = graphIndex;
			this.meta = meta;
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x000592E1 File Offset: 0x000574E1
		public GraphSerializationContext(BinaryWriter writer, bool[] persistentGraphs)
		{
			this.writer = writer;
			this.persistentGraphs = persistentGraphs;
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x000592F7 File Offset: 0x000574F7
		public void SerializeNodeReference(GraphNode node)
		{
			this.writer.Write((int)((node == null) ? uint.MaxValue : node.NodeIndex));
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x00059310 File Offset: 0x00057510
		public void SerializeConnections(Connection[] connections, bool serializeMetadata)
		{
			if (connections == null)
			{
				this.writer.Write(-1);
				return;
			}
			int num = 0;
			for (int i = 0; i < connections.Length; i++)
			{
				num += (this.persistentGraphs[(int)connections[i].node.GraphIndex] ? 1 : 0);
			}
			this.writer.Write(num);
			for (int j = 0; j < connections.Length; j++)
			{
				if (this.persistentGraphs[(int)connections[j].node.GraphIndex])
				{
					this.SerializeNodeReference(connections[j].node);
					this.writer.Write(connections[j].cost);
					if (serializeMetadata)
					{
						this.writer.Write(connections[j].shapeEdgeInfo);
					}
				}
			}
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x000593D8 File Offset: 0x000575D8
		public Connection[] DeserializeConnections(bool deserializeMetadata)
		{
			int num = this.reader.ReadInt32();
			if (num == -1)
			{
				return null;
			}
			Connection[] array = ArrayPool<Connection>.ClaimWithExactLength(num);
			for (int i = 0; i < num; i++)
			{
				GraphNode graphNode = this.DeserializeNodeReference();
				uint num2 = this.reader.ReadUInt32();
				if (deserializeMetadata)
				{
					byte b = 15;
					if (this.meta.version < AstarSerializer.V4_3_87)
					{
						if (!(this.meta.version < AstarSerializer.V4_1_0))
						{
							if (this.meta.version < AstarSerializer.V4_3_68)
							{
								this.reader.ReadByte();
							}
							else
							{
								b = this.reader.ReadByte();
							}
						}
						if (this.meta.version < AstarSerializer.V4_3_85)
						{
							b &= 79;
						}
						if (this.meta.version < AstarSerializer.V4_3_87)
						{
							b |= 48;
						}
					}
					else
					{
						b = this.reader.ReadByte();
					}
					array[i] = new Connection(graphNode, num2, b);
				}
				else
				{
					array[i] = new Connection(graphNode, num2, true, true);
				}
			}
			return array;
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x00059504 File Offset: 0x00057704
		public GraphNode DeserializeNodeReference()
		{
			int num = this.reader.ReadInt32();
			if (this.id2NodeMapping == null)
			{
				throw new Exception("Calling DeserializeNodeReference when not deserializing node references");
			}
			if (num == -1)
			{
				return null;
			}
			GraphNode graphNode = this.id2NodeMapping[num];
			if (graphNode == null)
			{
				throw new Exception("Invalid id (" + num.ToString() + ")");
			}
			return graphNode;
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x0005955D File Offset: 0x0005775D
		public void SerializeVector3(Vector3 v)
		{
			this.writer.Write(v.x);
			this.writer.Write(v.y);
			this.writer.Write(v.z);
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x00059592 File Offset: 0x00057792
		public Vector3 DeserializeVector3()
		{
			return new Vector3(this.reader.ReadSingle(), this.reader.ReadSingle(), this.reader.ReadSingle());
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x000595BA File Offset: 0x000577BA
		public void SerializeInt3(Int3 v)
		{
			this.writer.Write(v.x);
			this.writer.Write(v.y);
			this.writer.Write(v.z);
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x000595EF File Offset: 0x000577EF
		public Int3 DeserializeInt3()
		{
			return new Int3(this.reader.ReadInt32(), this.reader.ReadInt32(), this.reader.ReadInt32());
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x00059618 File Offset: 0x00057818
		public unsafe UnsafeSpan<T> ReadSpan<[IsUnmanaged] T>(Allocator allocator) where T : struct, ValueType
		{
			UnsafeSpan<T> unsafeSpan = new UnsafeSpan<T>(allocator, this.reader.ReadInt32());
			UnsafeSpan<byte> unsafeSpan2 = unsafeSpan.Reinterpret<byte>(UnsafeUtility.SizeOf<T>());
			if (this.reader.Read(new Span<byte>((void*)unsafeSpan2.ptr, unsafeSpan2.Length)) != unsafeSpan2.Length)
			{
				throw new Exception("Unexpected end of stream");
			}
			return unsafeSpan;
		}

		// Token: 0x04000ADF RID: 2783
		private readonly GraphNode[] id2NodeMapping;

		// Token: 0x04000AE0 RID: 2784
		public readonly BinaryReader reader;

		// Token: 0x04000AE1 RID: 2785
		public readonly BinaryWriter writer;

		// Token: 0x04000AE2 RID: 2786
		public readonly uint graphIndex;

		// Token: 0x04000AE3 RID: 2787
		public readonly GraphMeta meta;

		// Token: 0x04000AE4 RID: 2788
		public bool[] persistentGraphs;
	}
}
