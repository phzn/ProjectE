using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x0200045B RID: 1115
	public class vFootStepHandler : MonoBehaviour
	{
		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001BCF RID: 7119 RVA: 0x000AEEC3 File Offset: 0x000AD0C3
		public int material_ID
		{
			get
			{
				return this.materialIndex;
			}
		}

		// Token: 0x04001D0C RID: 7436
		[Tooltip("Use this to select a specific material or texture if your mesh has multiple materials, the footstep will play only the selected index.")]
		[SerializeField]
		private int materialIndex;

		// Token: 0x04001D0D RID: 7437
		public vFootStepHandler.StepHandleType stepHandleType;

		// Token: 0x0200045C RID: 1116
		public enum StepHandleType
		{
			// Token: 0x04001D0F RID: 7439
			materialName,
			// Token: 0x04001D10 RID: 7440
			textureName
		}
	}
}
