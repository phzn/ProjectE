using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000044 RID: 68
	public class GPUILODBiasModifier : MonoBehaviour
	{
		// Token: 0x06000272 RID: 626 RVA: 0x0000D7BA File Offset: 0x0000B9BA
		private void OnEnable()
		{
			this._previousLODBias = QualitySettings.lodBias;
			QualitySettings.lodBias = this.lodBias;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000D7D2 File Offset: 0x0000B9D2
		private void OnDisable()
		{
			QualitySettings.lodBias = this._previousLODBias;
		}

		// Token: 0x0400019F RID: 415
		public float lodBias = 2f;

		// Token: 0x040001A0 RID: 416
		private float _previousLODBias;
	}
}
