using System;
using UnityEngine;

namespace ProjektSumperk
{
	// Token: 0x02000405 RID: 1029
	public class RotateUI : MonoBehaviour
	{
		// Token: 0x06001A1F RID: 6687 RVA: 0x000A5B3C File Offset: 0x000A3D3C
		private void Update()
		{
			float num = (this.clockwise ? 1f : (-1f));
			base.transform.Rotate(Vector3.forward * num * this.rotationSpeed * Time.deltaTime);
		}

		// Token: 0x04001B44 RID: 6980
		public float rotationSpeed = 60f;

		// Token: 0x04001B45 RID: 6981
		public bool clockwise = true;
	}
}
