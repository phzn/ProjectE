using System;
using UnityEngine;

// Token: 0x020000AC RID: 172
[DisallowMultipleComponent]
[AddComponentMenu("")]
public sealed class UniversityResidencyPayloadTag : MonoBehaviour
{
	// Token: 0x040005A7 RID: 1447
	public const int CurrentFormatVersion = 2;

	// Token: 0x040005A8 RID: 1448
	public string family = "university";

	// Token: 0x040005A9 RID: 1449
	public string variant;

	// Token: 0x040005AA RID: 1450
	public string bindingSetId;

	// Token: 0x040005AB RID: 1451
	public int formatVersion = 2;
}
