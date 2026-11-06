using System;

// Token: 0x020000A3 RID: 163
public static class SnapPointCompatibility
{
	// Token: 0x060006A0 RID: 1696 RVA: 0x0002E9F0 File Offset: 0x0002CBF0
	public static bool AreCompatible(SnapPointType typeA, SnapPointType typeB)
	{
		return typeA == SnapPointType.Any || typeB == SnapPointType.Any || typeA == typeB || ((typeA == SnapPointType.Road && typeB == SnapPointType.Intersection) || (typeA == SnapPointType.Intersection && typeB == SnapPointType.Road));
	}
}
