using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000156 RID: 342
	internal class CursorManager : ICursorManager
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000B0E RID: 2830 RVA: 0x0002C677 File Offset: 0x0002A877
		// (set) Token: 0x06000B0F RID: 2831 RVA: 0x0002C67F File Offset: 0x0002A87F
		public bool isCursorOverriden { get; private set; }

		// Token: 0x06000B10 RID: 2832 RVA: 0x0002C688 File Offset: 0x0002A888
		public void SetCursor(Cursor cursor)
		{
			bool flag = cursor.texture != null;
			if (flag)
			{
				Cursor.SetCursor(cursor.texture, cursor.hotspot, CursorMode.Auto);
				this.isCursorOverriden = true;
			}
			else
			{
				bool flag2 = cursor.defaultCursorId != 0;
				if (flag2)
				{
					Debug.LogWarning("Runtime cursors other than the default cursor need to be defined using a texture.");
				}
				this.ResetCursor();
			}
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x0002C6EC File Offset: 0x0002A8EC
		public void ResetCursor()
		{
			bool isCursorOverriden = this.isCursorOverriden;
			if (isCursorOverriden)
			{
				Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			}
			this.isCursorOverriden = false;
		}
	}
}
