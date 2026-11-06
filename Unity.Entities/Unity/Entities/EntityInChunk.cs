using System;

namespace Unity.Entities
{
	// Token: 0x0200016F RID: 367
	public struct EntityInChunk : IComparable<EntityInChunk>, IEquatable<EntityInChunk>
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600087B RID: 2171 RVA: 0x0002C920 File Offset: 0x0002AB20
		public static EntityInChunk Null
		{
			get
			{
				return default(EntityInChunk);
			}
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x0002C938 File Offset: 0x0002AB38
		public int CompareTo(EntityInChunk other)
		{
			int num = ((this.Chunk < other.Chunk) ? (-1) : 1);
			int num2 = this.IndexInChunk - other.IndexInChunk;
			if (!(this.Chunk != other.Chunk))
			{
				return num2;
			}
			return num;
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x0002C981 File Offset: 0x0002AB81
		public bool Equals(EntityInChunk other)
		{
			return this.CompareTo(other) == 0;
		}

		// Token: 0x04000466 RID: 1126
		internal ChunkIndex Chunk;

		// Token: 0x04000467 RID: 1127
		internal int IndexInChunk;
	}
}
