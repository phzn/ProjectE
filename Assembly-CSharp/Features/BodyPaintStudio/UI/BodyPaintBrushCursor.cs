using System;
using UnityEngine;

namespace Features.BodyPaintStudio.UI
{
	// Token: 0x02001518 RID: 5400
	public class BodyPaintBrushCursor : MonoBehaviour
	{
		// Token: 0x0600A6F2 RID: 42738 RVA: 0x0045CAC8 File Offset: 0x0045ACC8
		public void SetActive(bool active)
		{
			if (this._active == active)
			{
				return;
			}
			this._active = active;
			if (active && this.cursorTexture != null)
			{
				Cursor.SetCursor(this.cursorTexture, this.hotspot, CursorMode.Auto);
				return;
			}
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		}

		// Token: 0x0600A6F3 RID: 42739 RVA: 0x0045CB16 File Offset: 0x0045AD16
		private void OnDisable()
		{
			if (this._active)
			{
				this.SetActive(false);
			}
		}

		// Token: 0x0400ABC0 RID: 43968
		[Tooltip("Texture del cursore (readable, uncompressed, generata dal builder)")]
		[SerializeField]
		private Texture2D cursorTexture;

		// Token: 0x0400ABC1 RID: 43969
		[Tooltip("Hotspot in pixel della texture, origine in alto a sinistra (punta del pennello)")]
		[SerializeField]
		private Vector2 hotspot;

		// Token: 0x0400ABC2 RID: 43970
		private bool _active;
	}
}
