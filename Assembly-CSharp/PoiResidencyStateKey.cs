using System;

// Token: 0x02000094 RID: 148
public readonly struct PoiResidencyStateKey : IEquatable<PoiResidencyStateKey>
{
	// Token: 0x06000644 RID: 1604 RVA: 0x0002D0A2 File Offset: 0x0002B2A2
	public PoiResidencyStateKey(PoiResidencyContext context, string id, string channel)
	{
		this.context = context;
		this.id = id;
		this.channel = channel;
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x0002D0B9 File Offset: 0x0002B2B9
	public bool Equals(PoiResidencyStateKey other)
	{
		return this.context.Equals(other.context) && string.Equals(this.id, other.id, StringComparison.Ordinal) && string.Equals(this.channel, other.channel, StringComparison.Ordinal);
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x0002D0F8 File Offset: 0x0002B2F8
	public override bool Equals(object obj)
	{
		if (obj is PoiResidencyStateKey)
		{
			PoiResidencyStateKey poiResidencyStateKey = (PoiResidencyStateKey)obj;
			return this.Equals(poiResidencyStateKey);
		}
		return false;
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x0002D120 File Offset: 0x0002B320
	public override int GetHashCode()
	{
		int num = this.context.GetHashCode() * 397;
		string text = this.id;
		int num2 = (num ^ ((text != null) ? text.GetHashCode() : 0)) * 397;
		string text2 = this.channel;
		return num2 ^ ((text2 != null) ? text2.GetHashCode() : 0);
	}

	// Token: 0x040004E7 RID: 1255
	public readonly PoiResidencyContext context;

	// Token: 0x040004E8 RID: 1256
	public readonly string id;

	// Token: 0x040004E9 RID: 1257
	public readonly string channel;
}
