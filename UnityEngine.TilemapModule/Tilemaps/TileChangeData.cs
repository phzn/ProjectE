using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Tilemaps
{
	// Token: 0x02000017 RID: 23
	[RequiredByNativeCode]
	[NativeType(Header = "Modules/Tilemap/TilemapScripting.h")]
	public struct TileChangeData
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600010D RID: 269 RVA: 0x000033F0 File Offset: 0x000015F0
		// (set) Token: 0x0600010E RID: 270 RVA: 0x00003408 File Offset: 0x00001608
		public Vector3Int position
		{
			get
			{
				return this.m_Position;
			}
			set
			{
				this.m_Position = value;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00003414 File Offset: 0x00001614
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00003431 File Offset: 0x00001631
		public TileBase tile
		{
			get
			{
				return (TileBase)this.m_TileAsset;
			}
			set
			{
				this.m_TileAsset = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000111 RID: 273 RVA: 0x0000343C File Offset: 0x0000163C
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00003454 File Offset: 0x00001654
		public Color color
		{
			get
			{
				return this.m_Color;
			}
			set
			{
				this.m_Color = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00003460 File Offset: 0x00001660
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00003478 File Offset: 0x00001678
		public Matrix4x4 transform
		{
			get
			{
				return this.m_Transform;
			}
			set
			{
				this.m_Transform = value;
			}
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00003482 File Offset: 0x00001682
		public TileChangeData(Vector3Int position, TileBase tile, Color color, Matrix4x4 transform)
		{
			this.m_Position = position;
			this.m_TileAsset = tile;
			this.m_Color = color;
			this.m_Transform = transform;
		}

		// Token: 0x04000058 RID: 88
		private Vector3Int m_Position;

		// Token: 0x04000059 RID: 89
		private Object m_TileAsset;

		// Token: 0x0400005A RID: 90
		private Color m_Color;

		// Token: 0x0400005B RID: 91
		private Matrix4x4 m_Transform;
	}
}
