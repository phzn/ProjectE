using System;
using UnityEngine;

namespace Features.Localization
{
	// Token: 0x020010C1 RID: 4289
	public sealed class GameLocalizationRunner : MonoBehaviour
	{
		// Token: 0x06008239 RID: 33337 RVA: 0x003533FA File Offset: 0x003515FA
		private void OnDestroy()
		{
			GameLocalization.ReleaseOwnedTable();
		}
	}
}
