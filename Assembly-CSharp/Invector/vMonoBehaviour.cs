using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004AE RID: 1198
	public class vMonoBehaviour : MonoBehaviour
	{
		// Token: 0x04001E39 RID: 7737
		[SerializeField]
		[HideInInspector]
		private bool openCloseEvents;

		// Token: 0x04001E3A RID: 7738
		[SerializeField]
		[HideInInspector]
		private bool openCloseWindow;

		// Token: 0x04001E3B RID: 7739
		[SerializeField]
		[HideInInspector]
		private int selectedToolbar;
	}
}
