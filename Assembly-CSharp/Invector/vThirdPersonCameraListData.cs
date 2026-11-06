using System;
using System.Collections.Generic;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000449 RID: 1097
	[Serializable]
	public class vThirdPersonCameraListData : ScriptableObject
	{
		// Token: 0x06001B83 RID: 7043 RVA: 0x000AD6E8 File Offset: 0x000AB8E8
		public vThirdPersonCameraListData()
		{
			this.tpCameraStates = new List<vThirdPersonCameraState>();
			this.tpCameraStates.Add(new vThirdPersonCameraState("Default"));
		}

		// Token: 0x04001CB2 RID: 7346
		[SerializeField]
		public string Name;

		// Token: 0x04001CB3 RID: 7347
		[SerializeField]
		public List<vThirdPersonCameraState> tpCameraStates;
	}
}
