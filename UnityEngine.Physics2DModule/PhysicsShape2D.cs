using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000014 RID: 20
	[UsedByNativeCode]
	[NativeHeader(Header = "Modules/Physics2D/Public/PhysicsScripting2D.h")]
	public struct PhysicsShape2D
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x000056D8 File Offset: 0x000038D8
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x000056F0 File Offset: 0x000038F0
		public PhysicsShapeType2D shapeType
		{
			get
			{
				return this.m_ShapeType;
			}
			set
			{
				this.m_ShapeType = value;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x000056FC File Offset: 0x000038FC
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x00005714 File Offset: 0x00003914
		public float radius
		{
			get
			{
				return this.m_Radius;
			}
			set
			{
				bool flag = value < 0f;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("radius cannot be negative.");
				}
				bool flag2 = float.IsNaN(value) || float.IsInfinity(value);
				if (flag2)
				{
					throw new ArgumentException("radius contains an invalid value.");
				}
				this.m_Radius = value;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00005760 File Offset: 0x00003960
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x00005778 File Offset: 0x00003978
		public int vertexStartIndex
		{
			get
			{
				return this.m_VertexStartIndex;
			}
			set
			{
				bool flag = value < 0;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("vertexStartIndex cannot be negative.");
				}
				this.m_VertexStartIndex = value;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x000057A0 File Offset: 0x000039A0
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x000057B8 File Offset: 0x000039B8
		public int vertexCount
		{
			get
			{
				return this.m_VertexCount;
			}
			set
			{
				bool flag = value < 1;
				if (flag)
				{
					throw new ArgumentOutOfRangeException("vertexCount cannot be less than one.");
				}
				this.m_VertexCount = value;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x000057E0 File Offset: 0x000039E0
		// (set) Token: 0x060001EA RID: 490 RVA: 0x000057FB File Offset: 0x000039FB
		public bool useAdjacentStart
		{
			get
			{
				return this.m_UseAdjacentStart != 0;
			}
			set
			{
				this.m_UseAdjacentStart = (value ? 1 : 0);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000580C File Offset: 0x00003A0C
		// (set) Token: 0x060001EC RID: 492 RVA: 0x00005827 File Offset: 0x00003A27
		public bool useAdjacentEnd
		{
			get
			{
				return this.m_UseAdjacentEnd != 0;
			}
			set
			{
				this.m_UseAdjacentEnd = (value ? 1 : 0);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060001ED RID: 493 RVA: 0x00005838 File Offset: 0x00003A38
		// (set) Token: 0x060001EE RID: 494 RVA: 0x00005850 File Offset: 0x00003A50
		public Vector2 adjacentStart
		{
			get
			{
				return this.m_AdjacentStart;
			}
			set
			{
				bool flag = float.IsNaN(value.x) || float.IsNaN(value.y) || float.IsInfinity(value.x) || float.IsInfinity(value.y);
				if (flag)
				{
					throw new ArgumentException("adjacentStart contains an invalid value.");
				}
				this.m_AdjacentStart = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060001EF RID: 495 RVA: 0x000058AC File Offset: 0x00003AAC
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x000058C4 File Offset: 0x00003AC4
		public Vector2 adjacentEnd
		{
			get
			{
				return this.m_AdjacentEnd;
			}
			set
			{
				bool flag = float.IsNaN(value.x) || float.IsNaN(value.y) || float.IsInfinity(value.x) || float.IsInfinity(value.y);
				if (flag)
				{
					throw new ArgumentException("adjacentEnd contains an invalid value.");
				}
				this.m_AdjacentEnd = value;
			}
		}

		// Token: 0x0400004D RID: 77
		private PhysicsShapeType2D m_ShapeType;

		// Token: 0x0400004E RID: 78
		private float m_Radius;

		// Token: 0x0400004F RID: 79
		private int m_VertexStartIndex;

		// Token: 0x04000050 RID: 80
		private int m_VertexCount;

		// Token: 0x04000051 RID: 81
		private int m_UseAdjacentStart;

		// Token: 0x04000052 RID: 82
		private int m_UseAdjacentEnd;

		// Token: 0x04000053 RID: 83
		private Vector2 m_AdjacentStart;

		// Token: 0x04000054 RID: 84
		private Vector2 m_AdjacentEnd;
	}
}
