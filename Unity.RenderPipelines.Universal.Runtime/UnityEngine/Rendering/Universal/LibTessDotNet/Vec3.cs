using System;

namespace UnityEngine.Rendering.Universal.LibTessDotNet
{
	// Token: 0x0200010B RID: 267
	internal struct Vec3
	{
		// Token: 0x17000206 RID: 518
		public float this[int index]
		{
			get
			{
				if (index == 0)
				{
					return this.X;
				}
				if (index == 1)
				{
					return this.Y;
				}
				if (index == 2)
				{
					return this.Z;
				}
				throw new IndexOutOfRangeException();
			}
			set
			{
				if (index == 0)
				{
					this.X = value;
					return;
				}
				if (index == 1)
				{
					this.Y = value;
					return;
				}
				if (index == 2)
				{
					this.Z = value;
					return;
				}
				throw new IndexOutOfRangeException();
			}
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x000354CA File Offset: 0x000336CA
		public static void Sub(ref Vec3 lhs, ref Vec3 rhs, out Vec3 result)
		{
			result.X = lhs.X - rhs.X;
			result.Y = lhs.Y - rhs.Y;
			result.Z = lhs.Z - rhs.Z;
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00035505 File Offset: 0x00033705
		public static void Neg(ref Vec3 v)
		{
			v.X = -v.X;
			v.Y = -v.Y;
			v.Z = -v.Z;
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x0003552E File Offset: 0x0003372E
		public static void Dot(ref Vec3 u, ref Vec3 v, out float dot)
		{
			dot = u.X * v.X + u.Y * v.Y + u.Z * v.Z;
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x0003555C File Offset: 0x0003375C
		public static void Normalize(ref Vec3 v)
		{
			float num = v.X * v.X + v.Y * v.Y + v.Z * v.Z;
			num = 1f / (float)Math.Sqrt((double)num);
			v.X *= num;
			v.Y *= num;
			v.Z *= num;
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x000355C4 File Offset: 0x000337C4
		public static int LongAxis(ref Vec3 v)
		{
			int num = 0;
			if (Math.Abs(v.Y) > Math.Abs(v.X))
			{
				num = 1;
			}
			if (Math.Abs(v.Z) > Math.Abs((num == 0) ? v.X : v.Y))
			{
				num = 2;
			}
			return num;
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x00035613 File Offset: 0x00033813
		public override string ToString()
		{
			return string.Format("{0}, {1}, {2}", this.X, this.Y, this.Z);
		}

		// Token: 0x040007BA RID: 1978
		public static readonly Vec3 Zero;

		// Token: 0x040007BB RID: 1979
		public float X;

		// Token: 0x040007BC RID: 1980
		public float Y;

		// Token: 0x040007BD RID: 1981
		public float Z;
	}
}
