using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000024 RID: 36
	[NativeHeader("Modules/Physics2D/Public/CircleCollider2D.h")]
	public sealed class CircleCollider2D : Collider2D
	{
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600039A RID: 922
		// (set) Token: 0x0600039B RID: 923
		public extern float radius
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600039C RID: 924 RVA: 0x00008370 File Offset: 0x00006570
		// (set) Token: 0x0600039D RID: 925 RVA: 0x00005567 File Offset: 0x00003767
		[Obsolete("CircleCollider2D.center has been deprecated. Use CircleCollider2D.offset instead (UnityUpgradable) -> offset", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public Vector2 center
		{
			get
			{
				return Vector2.zero;
			}
			set
			{
			}
		}
	}
}
