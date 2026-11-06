using System;
using UnityEngine;

// Token: 0x020000AB RID: 171
public sealed class UniversityGameplayManifest : ScriptableObject
{
	// Token: 0x040005A2 RID: 1442
	public string family = "university";

	// Token: 0x040005A3 RID: 1443
	public string variant;

	// Token: 0x040005A4 RID: 1444
	public string bindingSetId;

	// Token: 0x040005A5 RID: 1445
	public UniversityGameplayBinding[] records = Array.Empty<UniversityGameplayBinding>();

	// Token: 0x040005A6 RID: 1446
	public UniversityMaterialBinding[] materialRecords = Array.Empty<UniversityMaterialBinding>();
}
