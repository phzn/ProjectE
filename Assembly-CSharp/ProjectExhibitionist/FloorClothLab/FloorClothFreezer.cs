using System;
using UnityEngine;

namespace ProjectExhibitionist.FloorClothLab
{
	// Token: 0x020009D1 RID: 2513
	public class FloorClothFreezer : MonoBehaviour
	{
		// Token: 0x060046AA RID: 18090 RVA: 0x001AB51B File Offset: 0x001A971B
		private void Awake()
		{
			this._mf = base.GetComponent<MeshFilter>();
		}

		// Token: 0x060046AB RID: 18091 RVA: 0x001AB52C File Offset: 0x001A972C
		private void Update()
		{
			if (this.manual || this._done)
			{
				return;
			}
			this._t += Time.deltaTime;
			if (this._t < this.delay)
			{
				return;
			}
			this._done = true;
			this.SaveNow();
		}

		// Token: 0x060046AC RID: 18092 RVA: 0x001AB579 File Offset: 0x001A9779
		public Vector3 SaveNow()
		{
			return Vector3.zero;
		}

		// Token: 0x04004641 RID: 17985
		public float delay = 0.2f;

		// Token: 0x04004642 RID: 17986
		public bool manual = true;

		// Token: 0x04004643 RID: 17987
		public string savePath = "Assets/Features/Inventory/Scripts/FloorClothLab/Generated/Leggings_FloorSettled.asset";

		// Token: 0x04004644 RID: 17988
		private float _t;

		// Token: 0x04004645 RID: 17989
		private bool _done;

		// Token: 0x04004646 RID: 17990
		private MeshFilter _mf;
	}
}
