using System;
using UnityEngine;

namespace MagicaCloth2
{
	// Token: 0x02000024 RID: 36
	[AddComponentMenu("MagicaCloth2/MagicaCapsuleCollider")]
	[HelpURL("https://magicasoft.jp/en/mc2_capsulecollidercomponent/")]
	public class MagicaCapsuleCollider : ColliderComponent
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00007863 File Offset: 0x00005A63
		public override ColliderManager.ColliderType GetColliderType()
		{
			if (this.direction == MagicaCapsuleCollider.Direction.X)
			{
				if (!this.alignedOnCenter)
				{
					return ColliderManager.ColliderType.CapsuleX_Start;
				}
				return ColliderManager.ColliderType.CapsuleX_Center;
			}
			else if (this.direction == MagicaCapsuleCollider.Direction.Y)
			{
				if (!this.alignedOnCenter)
				{
					return ColliderManager.ColliderType.CapsuleY_Start;
				}
				return ColliderManager.ColliderType.CapsuleY_Center;
			}
			else
			{
				if (!this.alignedOnCenter)
				{
					return ColliderManager.ColliderType.CapsuleZ_Start;
				}
				return ColliderManager.ColliderType.CapsuleZ_Center;
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00007899 File Offset: 0x00005A99
		public void SetSize(float startRadius, float endRadius, float length)
		{
			base.SetSize(new Vector3(startRadius, endRadius, length));
			this.radiusSeparation = startRadius != endRadius;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000078B6 File Offset: 0x00005AB6
		public override Vector3 GetSize()
		{
			if (this.radiusSeparation)
			{
				return this.size;
			}
			return new Vector3(this.size.x, this.size.x, this.size.z);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000078F0 File Offset: 0x00005AF0
		public Vector3 GetLocalDir()
		{
			float num = (float)(this.reverseDirection ? (-1) : 1);
			if (this.direction == MagicaCapsuleCollider.Direction.X)
			{
				return Vector3.right * num;
			}
			if (this.direction == MagicaCapsuleCollider.Direction.Y)
			{
				return Vector3.up * num;
			}
			return Vector3.forward * num;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000793F File Offset: 0x00005B3F
		public Vector3 GetLocalUp()
		{
			if (this.direction == MagicaCapsuleCollider.Direction.X)
			{
				return Vector3.up;
			}
			if (this.direction == MagicaCapsuleCollider.Direction.Y)
			{
				return Vector3.forward;
			}
			return Vector3.up;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00007963 File Offset: 0x00005B63
		public override bool IsReverseDirection()
		{
			return this.reverseDirection;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000796C File Offset: 0x00005B6C
		public override void DataValidate()
		{
			this.size.x = Mathf.Max(this.size.x, 0.001f);
			this.size.y = Mathf.Max(this.size.y, 0.001f);
			this.size.z = Mathf.Max(this.size.z, 0.001f);
		}

		// Token: 0x040000FD RID: 253
		public MagicaCapsuleCollider.Direction direction;

		// Token: 0x040000FE RID: 254
		public bool reverseDirection;

		// Token: 0x040000FF RID: 255
		public bool radiusSeparation;

		// Token: 0x04000100 RID: 256
		public bool alignedOnCenter = true;

		// Token: 0x02000025 RID: 37
		public enum Direction
		{
			// Token: 0x04000102 RID: 258
			[InspectorName("X-Axis")]
			X,
			// Token: 0x04000103 RID: 259
			[InspectorName("Y-Axis")]
			Y,
			// Token: 0x04000104 RID: 260
			[InspectorName("Z-Axis")]
			Z
		}
	}
}
