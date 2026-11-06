using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MoreMountains.Tools
{
	// Token: 0x0200008C RID: 140
	[AddComponentMenu("More Mountains/Tools/Tilemaps/MMTilemapBoolean")]
	public class MMTilemapBoolean : MonoBehaviour
	{
		// Token: 0x0600031B RID: 795 RVA: 0x0000FE14 File Offset: 0x0000E014
		public virtual void BooleanClean()
		{
			if (this.TilemapToClean == null)
			{
				return;
			}
			this._tilemap = base.gameObject.GetComponent<Tilemap>();
			foreach (Vector3Int vector3Int in this._tilemap.cellBounds.allPositionsWithin)
			{
				Vector3Int vector3Int2 = new Vector3Int(vector3Int.x, vector3Int.y, vector3Int.z);
				if (this._tilemap.HasTile(vector3Int2) && this.TilemapToClean.HasTile(vector3Int2))
				{
					this.TilemapToClean.SetTile(vector3Int2, null);
				}
			}
			this._tilemap.RefreshAllTiles();
		}

		// Token: 0x04000451 RID: 1105
		public Tilemap TilemapToClean;

		// Token: 0x04000452 RID: 1106
		[MMInspectorButton("BooleanClean")]
		public bool BooleanCleanButton;

		// Token: 0x04000453 RID: 1107
		protected Tilemap _tilemap;
	}
}
