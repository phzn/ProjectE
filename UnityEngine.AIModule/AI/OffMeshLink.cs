using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.AI
{
	// Token: 0x0200000F RID: 15
	[MovedFrom("UnityEngine")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.ai.navigation@1.1/manual/OffMeshLink.html")]
	public sealed class OffMeshLink : Behaviour
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000DB RID: 219
		// (set) Token: 0x060000DC RID: 220
		public extern bool activated
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000DD RID: 221
		public extern bool occupied
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000DE RID: 222
		// (set) Token: 0x060000DF RID: 223
		public extern float costOverride
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000E0 RID: 224
		// (set) Token: 0x060000E1 RID: 225
		public extern bool biDirectional
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060000E2 RID: 226
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void UpdatePositions();

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00002B2C File Offset: 0x00000D2C
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00002B44 File Offset: 0x00000D44
		[Obsolete("Use area instead.")]
		public int navMeshLayer
		{
			get
			{
				return this.area;
			}
			set
			{
				this.area = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000E5 RID: 229
		// (set) Token: 0x060000E6 RID: 230
		public extern int area
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000E7 RID: 231
		// (set) Token: 0x060000E8 RID: 232
		public extern bool autoUpdatePositions
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000E9 RID: 233
		// (set) Token: 0x060000EA RID: 234
		public extern Transform startTransform
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000EB RID: 235
		// (set) Token: 0x060000EC RID: 236
		public extern Transform endTransform
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
