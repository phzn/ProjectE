using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000073 RID: 115
	[Serializable]
	public struct AffineTransform
	{
		// Token: 0x060002E3 RID: 739 RVA: 0x00009750 File Offset: 0x00007950
		public AffineTransform(Vector3 t, Quaternion r)
		{
			this.translation = t;
			this.rotation = r;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00009760 File Offset: 0x00007960
		public void Set(Vector3 t, Quaternion r)
		{
			this.translation = t;
			this.rotation = r;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00009770 File Offset: 0x00007970
		public Vector3 Transform(Vector3 p)
		{
			return this.rotation * p + this.translation;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00009789 File Offset: 0x00007989
		public Vector3 InverseTransform(Vector3 p)
		{
			return Quaternion.Inverse(this.rotation) * (p - this.translation);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x000097A8 File Offset: 0x000079A8
		public AffineTransform Inverse()
		{
			Quaternion quaternion = Quaternion.Inverse(this.rotation);
			return new AffineTransform(quaternion * -this.translation, quaternion);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x000097D8 File Offset: 0x000079D8
		public AffineTransform InverseMul(AffineTransform transform)
		{
			Quaternion quaternion = Quaternion.Inverse(this.rotation);
			return new AffineTransform(quaternion * (transform.translation - this.translation), quaternion * transform.rotation);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00009819 File Offset: 0x00007A19
		public static Vector3 operator *(AffineTransform lhs, Vector3 rhs)
		{
			return lhs.rotation * rhs + lhs.translation;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00009832 File Offset: 0x00007A32
		public static AffineTransform operator *(AffineTransform lhs, AffineTransform rhs)
		{
			return new AffineTransform(lhs.Transform(rhs.translation), lhs.rotation * rhs.rotation);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00009857 File Offset: 0x00007A57
		public static AffineTransform operator *(Quaternion lhs, AffineTransform rhs)
		{
			return new AffineTransform(lhs * rhs.translation, lhs * rhs.rotation);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00009876 File Offset: 0x00007A76
		public static AffineTransform operator *(AffineTransform lhs, Quaternion rhs)
		{
			return new AffineTransform(lhs.translation, lhs.rotation * rhs);
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000988F File Offset: 0x00007A8F
		public static AffineTransform identity { get; } = new AffineTransform(Vector3.zero, Quaternion.identity);

		// Token: 0x04000134 RID: 308
		public Vector3 translation;

		// Token: 0x04000135 RID: 309
		public Quaternion rotation;
	}
}
