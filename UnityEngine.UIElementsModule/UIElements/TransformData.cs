using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002EF RID: 751
	internal struct TransformData : IStyleDataGroup<TransformData>, IEquatable<TransformData>
	{
		// Token: 0x0600196B RID: 6507 RVA: 0x00062A90 File Offset: 0x00060C90
		public TransformData Copy()
		{
			return this;
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x00062AA8 File Offset: 0x00060CA8
		public void CopyFrom(ref TransformData other)
		{
			this = other;
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x00062AB8 File Offset: 0x00060CB8
		public static bool operator ==(TransformData lhs, TransformData rhs)
		{
			return lhs.rotate == rhs.rotate && lhs.scale == rhs.scale && lhs.transformOrigin == rhs.transformOrigin && lhs.translate == rhs.translate;
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x00062B18 File Offset: 0x00060D18
		public static bool operator !=(TransformData lhs, TransformData rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x00062B34 File Offset: 0x00060D34
		public bool Equals(TransformData other)
		{
			return other == this;
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x00062B54 File Offset: 0x00060D54
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			return !flag && obj is TransformData && this.Equals((TransformData)obj);
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x00062B8C File Offset: 0x00060D8C
		public override int GetHashCode()
		{
			int num = this.rotate.GetHashCode();
			num = (num * 397) ^ this.scale.GetHashCode();
			num = (num * 397) ^ this.transformOrigin.GetHashCode();
			return (num * 397) ^ this.translate.GetHashCode();
		}

		// Token: 0x04000AAA RID: 2730
		public Rotate rotate;

		// Token: 0x04000AAB RID: 2731
		public Scale scale;

		// Token: 0x04000AAC RID: 2732
		public TransformOrigin transformOrigin;

		// Token: 0x04000AAD RID: 2733
		public Translate translate;
	}
}
