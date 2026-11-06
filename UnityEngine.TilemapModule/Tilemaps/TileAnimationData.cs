using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Tilemaps
{
	// Token: 0x02000018 RID: 24
	[RequiredByNativeCode]
	[NativeType(Header = "Modules/Tilemap/TilemapScripting.h")]
	public struct TileAnimationData
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000116 RID: 278 RVA: 0x000034A4 File Offset: 0x000016A4
		// (set) Token: 0x06000117 RID: 279 RVA: 0x000034BC File Offset: 0x000016BC
		public Sprite[] animatedSprites
		{
			get
			{
				return this.m_AnimatedSprites;
			}
			set
			{
				this.m_AnimatedSprites = value;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000118 RID: 280 RVA: 0x000034C8 File Offset: 0x000016C8
		// (set) Token: 0x06000119 RID: 281 RVA: 0x000034E0 File Offset: 0x000016E0
		public float animationSpeed
		{
			get
			{
				return this.m_AnimationSpeed;
			}
			set
			{
				this.m_AnimationSpeed = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600011A RID: 282 RVA: 0x000034EC File Offset: 0x000016EC
		// (set) Token: 0x0600011B RID: 283 RVA: 0x00003504 File Offset: 0x00001704
		public float animationStartTime
		{
			get
			{
				return this.m_AnimationStartTime;
			}
			set
			{
				this.m_AnimationStartTime = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00003510 File Offset: 0x00001710
		// (set) Token: 0x0600011D RID: 285 RVA: 0x00003528 File Offset: 0x00001728
		public TileAnimationFlags flags
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

		// Token: 0x0400005C RID: 92
		private Sprite[] m_AnimatedSprites;

		// Token: 0x0400005D RID: 93
		private float m_AnimationSpeed;

		// Token: 0x0400005E RID: 94
		private float m_AnimationStartTime;

		// Token: 0x0400005F RID: 95
		private TileAnimationFlags m_Flags;
	}
}
