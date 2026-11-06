using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Tilemaps
{
	// Token: 0x02000016 RID: 22
	[RequiredByNativeCode]
	[NativeType(Header = "Modules/Tilemap/TilemapScripting.h")]
	internal struct TileDataNative
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00003274 File Offset: 0x00001474
		// (set) Token: 0x06000101 RID: 257 RVA: 0x0000328C File Offset: 0x0000148C
		public int sprite
		{
			get
			{
				return this.m_Sprite;
			}
			set
			{
				this.m_Sprite = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00003298 File Offset: 0x00001498
		// (set) Token: 0x06000103 RID: 259 RVA: 0x000032B0 File Offset: 0x000014B0
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

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000032BC File Offset: 0x000014BC
		// (set) Token: 0x06000105 RID: 261 RVA: 0x000032D4 File Offset: 0x000014D4
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

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000106 RID: 262 RVA: 0x000032E0 File Offset: 0x000014E0
		// (set) Token: 0x06000107 RID: 263 RVA: 0x000032F8 File Offset: 0x000014F8
		public int gameObject
		{
			get
			{
				return this.m_GameObject;
			}
			set
			{
				this.m_GameObject = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00003304 File Offset: 0x00001504
		// (set) Token: 0x06000109 RID: 265 RVA: 0x0000331C File Offset: 0x0000151C
		public TileFlags flags
		{
			get
			{
				return this.m_Flags;
			}
			set
			{
				this.m_Flags = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00003328 File Offset: 0x00001528
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00003340 File Offset: 0x00001540
		public Tile.ColliderType colliderType
		{
			get
			{
				return this.m_ColliderType;
			}
			set
			{
				this.m_ColliderType = value;
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x0000334C File Offset: 0x0000154C
		public static implicit operator TileDataNative(TileData td)
		{
			return new TileDataNative
			{
				sprite = ((td.sprite != null) ? td.sprite.GetInstanceID() : 0),
				color = td.color,
				transform = td.transform,
				gameObject = ((td.gameObject != null) ? td.gameObject.GetInstanceID() : 0),
				flags = td.flags,
				colliderType = td.colliderType
			};
		}

		// Token: 0x04000052 RID: 82
		private int m_Sprite;

		// Token: 0x04000053 RID: 83
		private Color m_Color;

		// Token: 0x04000054 RID: 84
		private Matrix4x4 m_Transform;

		// Token: 0x04000055 RID: 85
		private int m_GameObject;

		// Token: 0x04000056 RID: 86
		private TileFlags m_Flags;

		// Token: 0x04000057 RID: 87
		private Tile.ColliderType m_ColliderType;
	}
}
