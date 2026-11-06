using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000025 RID: 37
	[NativeHeader("Modules/Physics2D/Public/CapsuleCollider2D.h")]
	public sealed class CapsuleCollider2D : Collider2D
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600039F RID: 927 RVA: 0x00008388 File Offset: 0x00006588
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x0000839E File Offset: 0x0000659E
		public Vector2 size
		{
			get
			{
				Vector2 vector;
				this.get_size_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_size_Injected(ref value);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060003A1 RID: 929
		// (set) Token: 0x060003A2 RID: 930
		public extern CapsuleDirection2D direction
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060003A4 RID: 932
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_size_Injected(out Vector2 ret);

		// Token: 0x060003A5 RID: 933
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_size_Injected(ref Vector2 value);
	}
}
