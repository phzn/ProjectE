using System;

// Token: 0x0200002C RID: 44
[Serializable]
public struct ZonePoolKey
{
	// Token: 0x06000197 RID: 407 RVA: 0x0000BB0B File Offset: 0x00009D0B
	public ZonePoolKey(ChunkSize size, ZoneOccupancy occ)
	{
		this.chunkSize = size;
		this.occupancy = occ;
	}

	// Token: 0x06000198 RID: 408 RVA: 0x0000BB1C File Offset: 0x00009D1C
	public override bool Equals(object obj)
	{
		if (!(obj is ZonePoolKey))
		{
			return false;
		}
		ZonePoolKey zonePoolKey = (ZonePoolKey)obj;
		return this.chunkSize == zonePoolKey.chunkSize && this.occupancy == zonePoolKey.occupancy;
	}

	// Token: 0x06000199 RID: 409 RVA: 0x0000BB58 File Offset: 0x00009D58
	public override int GetHashCode()
	{
		return (int)((this.chunkSize * (ChunkSize)397) ^ (ChunkSize)this.occupancy);
	}

	// Token: 0x0600019A RID: 410 RVA: 0x0000BB6D File Offset: 0x00009D6D
	public override string ToString()
	{
		return string.Format("({0}, {1})", this.chunkSize, this.occupancy);
	}

	// Token: 0x04000151 RID: 337
	public ChunkSize chunkSize;

	// Token: 0x04000152 RID: 338
	public ZoneOccupancy occupancy;
}
