using System;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008A0 RID: 2208
	internal sealed class PresetIconLoadHost : MonoBehaviour
	{
		// Token: 0x06003B6E RID: 15214 RVA: 0x0016176C File Offset: 0x0015F96C
		private void OnDestroy()
		{
			PresetIconCache.ReleaseLoadHost(this);
		}
	}
}
