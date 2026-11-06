using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MoreMountains.Tools
{
	// Token: 0x0200008F RID: 143
	[ExecuteAlways]
	[AddComponentMenu("More Mountains/Tools/Tilemaps/MMTilemapShadow")]
	[RequireComponent(typeof(Tilemap))]
	public class MMTilemapShadow : MonoBehaviour
	{
		// Token: 0x06000327 RID: 807 RVA: 0x00010136 File Offset: 0x0000E336
		public virtual void UpdateShadows()
		{
			if (this.ReferenceTilemap == null)
			{
				return;
			}
			this._tilemap = base.gameObject.GetComponent<Tilemap>();
			MMTilemapShadow.Copy(this.ReferenceTilemap, this._tilemap);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0001016C File Offset: 0x0000E36C
		public static void Copy(Tilemap source, Tilemap destination)
		{
			source.RefreshAllTiles();
			destination.RefreshAllTiles();
			List<Vector3Int> list = new List<Vector3Int>();
			foreach (Vector3Int vector3Int in source.cellBounds.allPositionsWithin)
			{
				Vector3Int vector3Int2 = new Vector3Int(vector3Int.x, vector3Int.y, vector3Int.z);
				if (source.HasTile(vector3Int2))
				{
					list.Add(vector3Int2);
				}
			}
			Vector3Int[] array = new Vector3Int[list.Count];
			TileBase[] array2 = new TileBase[list.Count];
			int num = 0;
			foreach (Vector3Int vector3Int3 in list)
			{
				array[num] = vector3Int3;
				array2[num] = source.GetTile(vector3Int3);
				num++;
			}
			destination.ClearAllTiles();
			destination.RefreshAllTiles();
			destination.size = source.size;
			destination.origin = source.origin;
			destination.ResizeBounds();
			destination.SetTiles(array, array2);
		}

		// Token: 0x04000458 RID: 1112
		public Tilemap ReferenceTilemap;

		// Token: 0x04000459 RID: 1113
		[MMInspectorButton("UpdateShadows")]
		public bool UpdateShadowButton;

		// Token: 0x0400045A RID: 1114
		protected Tilemap _tilemap;
	}
}
