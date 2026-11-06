using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Tilemaps
{
	// Token: 0x02000015 RID: 21
	[RequiredByNativeCode]
	[NativeType(Header = "Modules/Tilemap/TilemapScripting.h")]
	public struct TileData
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00003110 File Offset: 0x00001310
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00003132 File Offset: 0x00001332
		public Sprite sprite
		{
			get
			{
				return Object.ForceLoadFromInstanceID(this.m_Sprite) as Sprite;
			}
			set
			{
				this.m_Sprite = ((value != null) ? value.GetInstanceID() : 0);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00003150 File Offset: 0x00001350
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00003168 File Offset: 0x00001368
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

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00003174 File Offset: 0x00001374
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x0000318C File Offset: 0x0000138C
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

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00003198 File Offset: 0x00001398
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x000031BA File Offset: 0x000013BA
		public GameObject gameObject
		{
			get
			{
				return Object.ForceLoadFromInstanceID(this.m_GameObject) as GameObject;
			}
			set
			{
				this.m_GameObject = ((value != null) ? value.GetInstanceID() : 0);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000FA RID: 250 RVA: 0x000031D8 File Offset: 0x000013D8
		// (set) Token: 0x060000FB RID: 251 RVA: 0x000031F0 File Offset: 0x000013F0
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

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000FC RID: 252 RVA: 0x000031FC File Offset: 0x000013FC
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00003214 File Offset: 0x00001414
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

		// Token: 0x060000FE RID: 254 RVA: 0x00003220 File Offset: 0x00001420
		private static TileData CreateDefault()
		{
			return new TileData
			{
				color = Color.white,
				transform = Matrix4x4.identity,
				flags = TileFlags.None,
				colliderType = Tile.ColliderType.None
			};
		}

		// Token: 0x0400004B RID: 75
		private int m_Sprite;

		// Token: 0x0400004C RID: 76
		private Color m_Color;

		// Token: 0x0400004D RID: 77
		private Matrix4x4 m_Transform;

		// Token: 0x0400004E RID: 78
		private int m_GameObject;

		// Token: 0x0400004F RID: 79
		private TileFlags m_Flags;

		// Token: 0x04000050 RID: 80
		private Tile.ColliderType m_ColliderType;

		// Token: 0x04000051 RID: 81
		internal static readonly TileData Default = TileData.CreateDefault();
	}
}
