using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200001F RID: 31
	[RequiredByNativeCode(Optional = true, GenerateProxy = true)]
	[NativeClass("RaycastHit2D", "struct RaycastHit2D;")]
	[NativeHeader("Runtime/Interfaces/IPhysics2D.h")]
	public struct RaycastHit2D
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00006DF0 File Offset: 0x00004FF0
		// (set) Token: 0x06000255 RID: 597 RVA: 0x00006E08 File Offset: 0x00005008
		public Vector2 centroid
		{
			get
			{
				return this.m_Centroid;
			}
			set
			{
				this.m_Centroid = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000256 RID: 598 RVA: 0x00006E14 File Offset: 0x00005014
		// (set) Token: 0x06000257 RID: 599 RVA: 0x00006E2C File Offset: 0x0000502C
		public Vector2 point
		{
			get
			{
				return this.m_Point;
			}
			set
			{
				this.m_Point = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000258 RID: 600 RVA: 0x00006E38 File Offset: 0x00005038
		// (set) Token: 0x06000259 RID: 601 RVA: 0x00006E50 File Offset: 0x00005050
		public Vector2 normal
		{
			get
			{
				return this.m_Normal;
			}
			set
			{
				this.m_Normal = value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00006E5C File Offset: 0x0000505C
		// (set) Token: 0x0600025B RID: 603 RVA: 0x00006E74 File Offset: 0x00005074
		public float distance
		{
			get
			{
				return this.m_Distance;
			}
			set
			{
				this.m_Distance = value;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00006E80 File Offset: 0x00005080
		// (set) Token: 0x0600025D RID: 605 RVA: 0x00006E98 File Offset: 0x00005098
		public float fraction
		{
			get
			{
				return this.m_Fraction;
			}
			set
			{
				this.m_Fraction = value;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600025E RID: 606 RVA: 0x00006EA4 File Offset: 0x000050A4
		public Collider2D collider
		{
			get
			{
				return Object.FindObjectFromInstanceID(this.m_Collider) as Collider2D;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600025F RID: 607 RVA: 0x00006EC8 File Offset: 0x000050C8
		public Rigidbody2D rigidbody
		{
			get
			{
				return (this.collider != null) ? this.collider.attachedRigidbody : null;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00006EF8 File Offset: 0x000050F8
		public Transform transform
		{
			get
			{
				Rigidbody2D rigidbody = this.rigidbody;
				bool flag = rigidbody != null;
				Transform transform;
				if (flag)
				{
					transform = rigidbody.transform;
				}
				else
				{
					bool flag2 = this.collider != null;
					if (flag2)
					{
						transform = this.collider.transform;
					}
					else
					{
						transform = null;
					}
				}
				return transform;
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00006F44 File Offset: 0x00005144
		public static implicit operator bool(RaycastHit2D hit)
		{
			return hit.collider != null;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00006F64 File Offset: 0x00005164
		public int CompareTo(RaycastHit2D other)
		{
			bool flag = this.collider == null;
			int num;
			if (flag)
			{
				num = 1;
			}
			else
			{
				bool flag2 = other.collider == null;
				if (flag2)
				{
					num = -1;
				}
				else
				{
					num = this.fraction.CompareTo(other.fraction);
				}
			}
			return num;
		}

		// Token: 0x04000088 RID: 136
		[NativeName("centroid")]
		private Vector2 m_Centroid;

		// Token: 0x04000089 RID: 137
		[NativeName("point")]
		private Vector2 m_Point;

		// Token: 0x0400008A RID: 138
		[NativeName("normal")]
		private Vector2 m_Normal;

		// Token: 0x0400008B RID: 139
		[NativeName("distance")]
		private float m_Distance;

		// Token: 0x0400008C RID: 140
		[NativeName("fraction")]
		private float m_Fraction;

		// Token: 0x0400008D RID: 141
		[NativeName("collider")]
		private int m_Collider;
	}
}
