using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200004C RID: 76
	[AddComponentMenu("More Mountains/Tools/GUI/MMCursorVisible")]
	public class MMCursorVisible : MonoBehaviour
	{
		// Token: 0x060001AC RID: 428 RVA: 0x00007EF5 File Offset: 0x000060F5
		protected virtual void Update()
		{
			if (this.CursorVisibility == MMCursorVisible.CursorVisibilities.Visible)
			{
				Cursor.visible = true;
				return;
			}
			Cursor.visible = false;
		}

		// Token: 0x04000232 RID: 562
		public MMCursorVisible.CursorVisibilities CursorVisibility;

		// Token: 0x020003C1 RID: 961
		public enum CursorVisibilities
		{
			// Token: 0x04001DCD RID: 7629
			Visible,
			// Token: 0x04001DCE RID: 7630
			Invisible
		}
	}
}
