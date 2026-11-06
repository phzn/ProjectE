using System;
using UnityEngine;

// Token: 0x02000069 RID: 105
internal sealed class HeavyZoneAddressableResidencyRunner : MonoBehaviour
{
	// Token: 0x06000521 RID: 1313 RVA: 0x00026B62 File Offset: 0x00024D62
	private void Update()
	{
		HeavyZoneAddressableResidency.Tick();
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x00026B69 File Offset: 0x00024D69
	private void OnApplicationQuit()
	{
		HeavyZoneAddressableResidency.BeginShutdown();
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x00026B70 File Offset: 0x00024D70
	private void OnDestroy()
	{
		HeavyZoneAddressableResidency.RunnerDestroyed(this);
	}
}
