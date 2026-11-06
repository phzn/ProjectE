using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MoreMountains.Tools
{
	// Token: 0x0200008D RID: 141
	public class MMTilemapCleaner : MonoBehaviour
	{
		// Token: 0x0600031D RID: 797 RVA: 0x0000FEEC File Offset: 0x0000E0EC
		public virtual void Clean()
		{
			this._tilemap = base.gameObject.GetComponent<Tilemap>();
			if (this._tilemap != null)
			{
				this._tilemap.ClearAllTiles();
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000FF18 File Offset: 0x0000E118
		public virtual void CleanAllChildren()
		{
			this._tilemaps = base.GetComponentsInChildren<Tilemap>();
			Tilemap[] tilemaps = this._tilemaps;
			for (int i = 0; i < tilemaps.Length; i++)
			{
				tilemaps[i].ClearAllTiles();
			}
		}

		// Token: 0x04000454 RID: 1108
		[MMInspectorButton("Clean")]
		public bool CleanButton;

		// Token: 0x04000455 RID: 1109
		[MMInspectorButton("CleanAllChildren")]
		public bool CleanAllButton;

		// Token: 0x04000456 RID: 1110
		protected Tilemap _tilemap;

		// Token: 0x04000457 RID: 1111
		protected Tilemap[] _tilemaps;
	}
}
