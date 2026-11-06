using System;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000011 RID: 17
	[UsedByNativeCode]
	[Obsolete("SplatPrototype is obsolete. Use TerrainLayer instead.", false)]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class SplatPrototype
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00002E14 File Offset: 0x00001014
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00002E2C File Offset: 0x0000102C
		public Texture2D texture
		{
			get
			{
				return this.m_Texture;
			}
			set
			{
				this.m_Texture = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00002E38 File Offset: 0x00001038
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00002E50 File Offset: 0x00001050
		public Texture2D normalMap
		{
			get
			{
				return this.m_NormalMap;
			}
			set
			{
				this.m_NormalMap = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00002E5C File Offset: 0x0000105C
		// (set) Token: 0x060000D7 RID: 215 RVA: 0x00002E74 File Offset: 0x00001074
		public Vector2 tileSize
		{
			get
			{
				return this.m_TileSize;
			}
			set
			{
				this.m_TileSize = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00002E80 File Offset: 0x00001080
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00002E98 File Offset: 0x00001098
		public Vector2 tileOffset
		{
			get
			{
				return this.m_TileOffset;
			}
			set
			{
				this.m_TileOffset = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00002EA4 File Offset: 0x000010A4
		// (set) Token: 0x060000DB RID: 219 RVA: 0x00002EDC File Offset: 0x000010DC
		public Color specular
		{
			get
			{
				return new Color(this.m_SpecularMetallic.x, this.m_SpecularMetallic.y, this.m_SpecularMetallic.z);
			}
			set
			{
				this.m_SpecularMetallic.x = value.r;
				this.m_SpecularMetallic.y = value.g;
				this.m_SpecularMetallic.z = value.b;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00002F14 File Offset: 0x00001114
		// (set) Token: 0x060000DD RID: 221 RVA: 0x00002F31 File Offset: 0x00001131
		public float metallic
		{
			get
			{
				return this.m_SpecularMetallic.w;
			}
			set
			{
				this.m_SpecularMetallic.w = value;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00002F40 File Offset: 0x00001140
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00002F58 File Offset: 0x00001158
		public float smoothness
		{
			get
			{
				return this.m_Smoothness;
			}
			set
			{
				this.m_Smoothness = value;
			}
		}

		// Token: 0x0400003F RID: 63
		internal Texture2D m_Texture;

		// Token: 0x04000040 RID: 64
		internal Texture2D m_NormalMap;

		// Token: 0x04000041 RID: 65
		internal Vector2 m_TileSize = new Vector2(15f, 15f);

		// Token: 0x04000042 RID: 66
		internal Vector2 m_TileOffset = new Vector2(0f, 0f);

		// Token: 0x04000043 RID: 67
		internal Vector4 m_SpecularMetallic = new Vector4(0f, 0f, 0f, 0f);

		// Token: 0x04000044 RID: 68
		internal float m_Smoothness = 0f;
	}
}
