using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000059 RID: 89
	public class MMUIFollowMouse : MonoBehaviour
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x000096D0 File Offset: 0x000078D0
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x000096D8 File Offset: 0x000078D8
		public virtual Canvas TargetCanvas { get; set; }

		// Token: 0x060001EA RID: 490 RVA: 0x000096E4 File Offset: 0x000078E4
		protected virtual void LateUpdate()
		{
			this._mousePosition = Input.mousePosition;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.TargetCanvas.transform as RectTransform, this._mousePosition, this.TargetCanvas.worldCamera, out this._newPosition);
			base.transform.position = this.TargetCanvas.transform.TransformPoint(this._newPosition);
		}

		// Token: 0x04000295 RID: 661
		protected Vector2 _newPosition;

		// Token: 0x04000296 RID: 662
		protected Vector2 _mousePosition;
	}
}
