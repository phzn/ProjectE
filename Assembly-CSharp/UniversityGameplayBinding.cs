using System;
using UnityEngine;

// Token: 0x020000A7 RID: 167
[Serializable]
public sealed class UniversityGameplayBinding
{
	// Token: 0x04000595 RID: 1429
	public int[] siblingPath = Array.Empty<int>();

	// Token: 0x04000596 RID: 1430
	public int componentIndex = -1;

	// Token: 0x04000597 RID: 1431
	public UniversityGameplayField field;

	// Token: 0x04000598 RID: 1432
	public int elementIndex = -1;

	// Token: 0x04000599 RID: 1433
	public int expectedElementCount = -1;

	// Token: 0x0400059A RID: 1434
	public Object asset;
}
