using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200001A RID: 26
	[NativeHeader("Modules/Physics2D/Public/PhysicsScripting2D.h")]
	[NativeClass("ScriptingContactPoint2D", "struct ScriptingContactPoint2D;")]
	[RequiredByNativeCode(Optional = false, GenerateProxy = true)]
	public struct ContactPoint2D
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00006B70 File Offset: 0x00004D70
		public Vector2 point
		{
			get
			{
				return this.m_Point;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000238 RID: 568 RVA: 0x00006B88 File Offset: 0x00004D88
		public Vector2 normal
		{
			get
			{
				return this.m_Normal;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00006BA0 File Offset: 0x00004DA0
		public float separation
		{
			get
			{
				return this.m_Separation;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600023A RID: 570 RVA: 0x00006BB8 File Offset: 0x00004DB8
		public float normalImpulse
		{
			get
			{
				return this.m_NormalImpulse;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600023B RID: 571 RVA: 0x00006BD0 File Offset: 0x00004DD0
		public float tangentImpulse
		{
			get
			{
				return this.m_TangentImpulse;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600023C RID: 572 RVA: 0x00006BE8 File Offset: 0x00004DE8
		public Vector2 relativeVelocity
		{
			get
			{
				return this.m_RelativeVelocity;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00006C00 File Offset: 0x00004E00
		public Collider2D collider
		{
			get
			{
				return Object.FindObjectFromInstanceID(this.m_Collider) as Collider2D;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600023E RID: 574 RVA: 0x00006C24 File Offset: 0x00004E24
		public Collider2D otherCollider
		{
			get
			{
				return Object.FindObjectFromInstanceID(this.m_OtherCollider) as Collider2D;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00006C48 File Offset: 0x00004E48
		public Rigidbody2D rigidbody
		{
			get
			{
				return Object.FindObjectFromInstanceID(this.m_Rigidbody) as Rigidbody2D;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000240 RID: 576 RVA: 0x00006C6C File Offset: 0x00004E6C
		public Rigidbody2D otherRigidbody
		{
			get
			{
				return Object.FindObjectFromInstanceID(this.m_OtherRigidbody) as Rigidbody2D;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000241 RID: 577 RVA: 0x00006C90 File Offset: 0x00004E90
		public bool enabled
		{
			get
			{
				return this.m_Enabled == 1;
			}
		}

		// Token: 0x04000074 RID: 116
		[NativeName("point")]
		private Vector2 m_Point;

		// Token: 0x04000075 RID: 117
		[NativeName("normal")]
		private Vector2 m_Normal;

		// Token: 0x04000076 RID: 118
		[NativeName("relativeVelocity")]
		private Vector2 m_RelativeVelocity;

		// Token: 0x04000077 RID: 119
		[NativeName("separation")]
		private float m_Separation;

		// Token: 0x04000078 RID: 120
		[NativeName("normalImpulse")]
		private float m_NormalImpulse;

		// Token: 0x04000079 RID: 121
		[NativeName("tangentImpulse")]
		private float m_TangentImpulse;

		// Token: 0x0400007A RID: 122
		[NativeName("collider")]
		private int m_Collider;

		// Token: 0x0400007B RID: 123
		[NativeName("otherCollider")]
		private int m_OtherCollider;

		// Token: 0x0400007C RID: 124
		[NativeName("rigidbody")]
		private int m_Rigidbody;

		// Token: 0x0400007D RID: 125
		[NativeName("otherRigidbody")]
		private int m_OtherRigidbody;

		// Token: 0x0400007E RID: 126
		[NativeName("enabled")]
		private int m_Enabled;
	}
}
