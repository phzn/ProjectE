using System;
using CompassNavigatorPro;
using UnityEngine;

namespace CompassNavigatorProDemos
{
	// Token: 0x020002B4 RID: 692
	public class MiniMapScrollWheel : MonoBehaviour
	{
		// Token: 0x06001392 RID: 5010 RVA: 0x0007ECE8 File Offset: 0x0007CEE8
		private void Start()
		{
			this.compass = CompassPro.instance;
			this.compass.OnMiniMapMouseEnter.AddListener(delegate(Vector2 p)
			{
				this.canScrollWheel = true;
			});
			this.compass.OnMiniMapMouseExit.AddListener(delegate(Vector2 p)
			{
				this.canScrollWheel = false;
			});
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x0007ED38 File Offset: 0x0007CF38
		private void Update()
		{
			if (this.canScrollWheel)
			{
				float axis = Input.GetAxis("Mouse ScrollWheel");
				if (axis != 0f)
				{
					this.compass.MiniMapZoomIn(this.zoomSpeed * axis);
				}
			}
		}

		// Token: 0x0400156D RID: 5485
		public float zoomSpeed = 100f;

		// Token: 0x0400156E RID: 5486
		private CompassPro compass;

		// Token: 0x0400156F RID: 5487
		private bool canScrollWheel;
	}
}
