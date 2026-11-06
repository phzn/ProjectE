using System;
using UnityEngine;

// Token: 0x020000A8 RID: 168
[Serializable]
public sealed class UniversityMaterialBinding
{
	// Token: 0x0400059B RID: 1435
	public int[] siblingPath = Array.Empty<int>();

	// Token: 0x0400059C RID: 1436
	public int componentIndex = -1;

	// Token: 0x0400059D RID: 1437
	public int slotIndex = -1;

	// Token: 0x0400059E RID: 1438
	public int slotCount;

	// Token: 0x0400059F RID: 1439
	public Material material;
}
