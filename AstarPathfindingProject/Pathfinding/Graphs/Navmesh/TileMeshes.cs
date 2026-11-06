using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001D2 RID: 466
	public struct TileMeshes
	{
		// Token: 0x06000D13 RID: 3347 RVA: 0x0004ACD4 File Offset: 0x00048ED4
		public void Rotate(int rotation)
		{
			rotation = -rotation;
			rotation = (rotation % 4 + 4) % 4;
			if (rotation == 0)
			{
				return;
			}
			int2x2 int2x = new int2x2(0, -1, 1, 0);
			int2x2 int2x2 = int2x2.identity;
			for (int i = 0; i < rotation; i++)
			{
				int2x2 = math.mul(int2x2, int2x);
			}
			Int3 @int = (Int3)new Vector3(this.tileWorldSize.x, 0f, this.tileWorldSize.y);
			int2 int2 = -math.min(int2.zero, math.mul(int2x2, new int2(@int.x, @int.z)));
			int2 int3 = new int2(this.tileRect.Width, this.tileRect.Height);
			int2 int4 = -math.min(int2.zero, math.mul(int2x2, int3 - 1));
			TileMesh[] array = new TileMesh[this.tileMeshes.Length];
			int2 int5 = ((rotation % 2 == 0) ? int3 : new int2(int3.y, int3.x));
			for (int j = 0; j < int3.y; j++)
			{
				for (int k = 0; k < int3.x; k++)
				{
					Int3[] verticesInTileSpace = this.tileMeshes[k + j * int3.x].verticesInTileSpace;
					for (int l = 0; l < verticesInTileSpace.Length; l++)
					{
						Int3 int6 = verticesInTileSpace[l];
						int2 int7 = math.mul(int2x2, new int2(int6.x, int6.z)) + int2;
						verticesInTileSpace[l] = new Int3(int7.x, int6.y, int7.y);
					}
					int2 int8 = math.mul(int2x2, new int2(k, j)) + int4;
					array[int8.x + int8.y * int5.x] = this.tileMeshes[k + j * int3.x];
				}
			}
			this.tileMeshes = array;
			this.tileWorldSize = ((rotation % 2 == 0) ? this.tileWorldSize : new Vector2(this.tileWorldSize.y, this.tileWorldSize.x));
			this.tileRect = new IntRect(this.tileRect.xmin, this.tileRect.ymin, this.tileRect.xmin + int5.x - 1, this.tileRect.ymin + int5.y - 1);
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0004AF5C File Offset: 0x0004915C
		public byte[] Serialize()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(new DeflateStream(memoryStream, CompressionMode.Compress));
			binaryWriter.Write(0);
			binaryWriter.Write(this.tileRect.Width);
			binaryWriter.Write(this.tileRect.Height);
			binaryWriter.Write(this.tileWorldSize.x);
			binaryWriter.Write(this.tileWorldSize.y);
			for (int i = 0; i < this.tileRect.Height; i++)
			{
				for (int j = 0; j < this.tileRect.Width; j++)
				{
					TileMesh tileMesh = this.tileMeshes[i * this.tileRect.Width + j];
					tileMesh.Validate();
					binaryWriter.Write(tileMesh.triangles.Length);
					binaryWriter.Write(tileMesh.verticesInTileSpace.Length);
					binaryWriter.Write(MemoryMarshal.AsBytes<Int3>(tileMesh.verticesInTileSpace.AsSpan<Int3>()));
					binaryWriter.Write(MemoryMarshal.AsBytes<int>(tileMesh.triangles.AsSpan<int>()));
					binaryWriter.Write(MemoryMarshal.AsBytes<uint>(tileMesh.tags.AsSpan<uint>()));
				}
			}
			binaryWriter.Close();
			return memoryStream.ToArray();
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0004B0A0 File Offset: 0x000492A0
		public static TileMeshes Deserialize(byte[] bytes)
		{
			BinaryReader binaryReader = new BinaryReader(new DeflateStream(new MemoryStream(bytes), CompressionMode.Decompress));
			if (binaryReader.ReadInt32() != 0)
			{
				throw new Exception("Invalid data. Unexpected version number.");
			}
			int num = binaryReader.ReadInt32();
			int num2 = binaryReader.ReadInt32();
			Vector2 vector = new Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle());
			if (num < 0 || num2 < 0)
			{
				throw new Exception("Invalid bounds");
			}
			IntRect intRect = new IntRect(0, 0, num - 1, num2 - 1);
			TileMesh[] array = new TileMesh[num * num2];
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num; j++)
				{
					int num3 = binaryReader.ReadInt32();
					int num4 = binaryReader.ReadInt32();
					TileMesh tileMesh = new TileMesh
					{
						triangles = new int[num3],
						verticesInTileSpace = new Int3[num4],
						tags = new uint[num3 / 3]
					};
					TileMeshes.<Deserialize>g__ReadTo|5_0<Int3>(binaryReader, tileMesh.verticesInTileSpace);
					TileMeshes.<Deserialize>g__ReadTo|5_0<int>(binaryReader, tileMesh.triangles);
					TileMeshes.<Deserialize>g__ReadTo|5_0<uint>(binaryReader, tileMesh.tags);
					tileMesh.Validate();
					array[j + i * num] = tileMesh;
				}
			}
			return new TileMeshes
			{
				tileMeshes = array,
				tileRect = intRect,
				tileWorldSize = vector
			};
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0004B1F8 File Offset: 0x000493F8
		[CompilerGenerated]
		internal static void <Deserialize>g__ReadTo|5_0<[IsUnmanaged] T>(BinaryReader reader, T[] span) where T : struct, ValueType
		{
			Span<byte> span2 = MemoryMarshal.AsBytes<T>(span.AsSpan<T>());
			int num;
			for (int i = 0; i < span2.Length; i += num)
			{
				num = reader.Read(span2.Slice(i));
				if (num == 0)
				{
					throw new Exception("Unexpected end of stream");
				}
			}
		}

		// Token: 0x040008C8 RID: 2248
		public TileMesh[] tileMeshes;

		// Token: 0x040008C9 RID: 2249
		public IntRect tileRect;

		// Token: 0x040008CA RID: 2250
		public Vector2 tileWorldSize;
	}
}
