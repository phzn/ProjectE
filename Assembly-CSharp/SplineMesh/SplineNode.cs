using System;
using UnityEngine;

namespace SplineMesh
{
	// Token: 0x02000186 RID: 390
	[Serializable]
	public class SplineNode
	{
		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000C27 RID: 3111 RVA: 0x000542B1 File Offset: 0x000524B1
		// (set) Token: 0x06000C28 RID: 3112 RVA: 0x000542BC File Offset: 0x000524BC
		public Vector3 Position
		{
			get
			{
				return this.position;
			}
			set
			{
				if (this.position.Equals(value))
				{
					return;
				}
				this.position.x = value.x;
				this.position.y = value.y;
				this.position.z = value.z;
				if (this.Changed != null)
				{
					this.Changed(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000C29 RID: 3113 RVA: 0x00054324 File Offset: 0x00052524
		// (set) Token: 0x06000C2A RID: 3114 RVA: 0x0005432C File Offset: 0x0005252C
		public Vector3 Direction
		{
			get
			{
				return this.direction;
			}
			set
			{
				if (this.direction.Equals(value))
				{
					return;
				}
				this.direction.x = value.x;
				this.direction.y = value.y;
				this.direction.z = value.z;
				if (this.Changed != null)
				{
					this.Changed(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000C2B RID: 3115 RVA: 0x00054394 File Offset: 0x00052594
		// (set) Token: 0x06000C2C RID: 3116 RVA: 0x0005439C File Offset: 0x0005259C
		public Vector3 Up
		{
			get
			{
				return this.up;
			}
			set
			{
				if (this.up.Equals(value))
				{
					return;
				}
				this.up.x = value.x;
				this.up.y = value.y;
				this.up.z = value.z;
				if (this.Changed != null)
				{
					this.Changed(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000C2D RID: 3117 RVA: 0x00054404 File Offset: 0x00052604
		// (set) Token: 0x06000C2E RID: 3118 RVA: 0x0005440C File Offset: 0x0005260C
		public Vector2 Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				if (this.scale.Equals(value))
				{
					return;
				}
				this.scale.x = value.x;
				this.scale.y = value.y;
				if (this.Changed != null)
				{
					this.Changed(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000C2F RID: 3119 RVA: 0x00054463 File Offset: 0x00052663
		// (set) Token: 0x06000C30 RID: 3120 RVA: 0x0005446B File Offset: 0x0005266B
		public float Roll
		{
			get
			{
				return this.roll;
			}
			set
			{
				if (this.roll == value)
				{
					return;
				}
				this.roll = value;
				if (this.Changed != null)
				{
					this.Changed(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x00054497 File Offset: 0x00052697
		public SplineNode(Vector3 position, Vector3 direction)
		{
			this.Position = position;
			this.Direction = direction;
		}

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x06000C32 RID: 3122 RVA: 0x000544C4 File Offset: 0x000526C4
		// (remove) Token: 0x06000C33 RID: 3123 RVA: 0x000544FC File Offset: 0x000526FC
		[HideInInspector]
		public event EventHandler Changed;

		// Token: 0x04000C21 RID: 3105
		[SerializeField]
		private Vector3 position;

		// Token: 0x04000C22 RID: 3106
		[SerializeField]
		private Vector3 direction;

		// Token: 0x04000C23 RID: 3107
		[SerializeField]
		private Vector3 up = Vector3.up;

		// Token: 0x04000C24 RID: 3108
		[SerializeField]
		private Vector2 scale = Vector2.one;

		// Token: 0x04000C25 RID: 3109
		[SerializeField]
		private float roll;
	}
}
