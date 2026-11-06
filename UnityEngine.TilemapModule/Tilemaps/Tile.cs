using System;
using UnityEngine.Scripting;

namespace UnityEngine.Tilemaps
{
	// Token: 0x02000008 RID: 8
	[RequiredByNativeCode]
	[HelpURL("https://docs.unity3d.com/Manual/Tilemap-TileAsset.html")]
	[Serializable]
	public class Tile : TileBase
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000026B4 File Offset: 0x000008B4
		// (set) Token: 0x0600002A RID: 42 RVA: 0x000026CC File Offset: 0x000008CC
		public Sprite sprite
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

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000026D8 File Offset: 0x000008D8
		// (set) Token: 0x0600002C RID: 44 RVA: 0x000026F0 File Offset: 0x000008F0
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

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000026FC File Offset: 0x000008FC
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002714 File Offset: 0x00000914
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

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002720 File Offset: 0x00000920
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002738 File Offset: 0x00000938
		public GameObject gameObject
		{
			get
			{
				return this.m_InstancedGameObject;
			}
			set
			{
				this.m_InstancedGameObject = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002744 File Offset: 0x00000944
		// (set) Token: 0x06000032 RID: 50 RVA: 0x0000275C File Offset: 0x0000095C
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

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002768 File Offset: 0x00000968
		// (set) Token: 0x06000034 RID: 52 RVA: 0x00002780 File Offset: 0x00000980
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

		// Token: 0x06000035 RID: 53 RVA: 0x0000278C File Offset: 0x0000098C
		public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
		{
			tileData.sprite = this.m_Sprite;
			tileData.color = this.m_Color;
			tileData.transform = this.m_Transform;
			tileData.gameObject = this.m_InstancedGameObject;
			tileData.flags = this.m_Flags;
			tileData.colliderType = this.m_ColliderType;
		}

		// Token: 0x04000019 RID: 25
		[SerializeField]
		private Sprite m_Sprite;

		// Token: 0x0400001A RID: 26
		[SerializeField]
		private Color m_Color = Color.white;

		// Token: 0x0400001B RID: 27
		[SerializeField]
		private Matrix4x4 m_Transform = Matrix4x4.identity;

		// Token: 0x0400001C RID: 28
		[SerializeField]
		private GameObject m_InstancedGameObject;

		// Token: 0x0400001D RID: 29
		[SerializeField]
		private TileFlags m_Flags = TileFlags.LockColor;

		// Token: 0x0400001E RID: 30
		[SerializeField]
		private Tile.ColliderType m_ColliderType = Tile.ColliderType.Sprite;

		// Token: 0x02000009 RID: 9
		public enum ColliderType
		{
			// Token: 0x04000020 RID: 32
			None,
			// Token: 0x04000021 RID: 33
			Sprite,
			// Token: 0x04000022 RID: 34
			Grid
		}
	}
}
