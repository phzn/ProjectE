using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Unity.Mathematics
{
	// Token: 0x0200002E RID: 46
	[DebuggerTypeProxy(typeof(int2.DebuggerProxy))]
	[Il2CppEagerStaticClassConstruction]
	[Serializable]
	public struct int2 : IEquatable<int2>, IFormattable
	{
		// Token: 0x0600186F RID: 6255 RVA: 0x0004446D File Offset: 0x0004266D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x0004447D File Offset: 0x0004267D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2(int2 xy)
		{
			this.x = xy.x;
			this.y = xy.y;
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x00044497 File Offset: 0x00042697
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2(int v)
		{
			this.x = v;
			this.y = v;
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x000444A7 File Offset: 0x000426A7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2(bool v)
		{
			this.x = (v ? 1 : 0);
			this.y = (v ? 1 : 0);
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x000444C3 File Offset: 0x000426C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2(bool2 v)
		{
			this.x = (v.x ? 1 : 0);
			this.y = (v.y ? 1 : 0);
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x000444E9 File Offset: 0x000426E9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2(uint v)
		{
			this.x = (int)v;
			this.y = (int)v;
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x000444F9 File Offset: 0x000426F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2(uint2 v)
		{
			this.x = (int)v.x;
			this.y = (int)v.y;
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x00044513 File Offset: 0x00042713
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2(float v)
		{
			this.x = (int)v;
			this.y = (int)v;
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x00044525 File Offset: 0x00042725
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2(float2 v)
		{
			this.x = (int)v.x;
			this.y = (int)v.y;
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x00044541 File Offset: 0x00042741
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2(double v)
		{
			this.x = (int)v;
			this.y = (int)v;
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x00044553 File Offset: 0x00042753
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int2(double2 v)
		{
			this.x = (int)v.x;
			this.y = (int)v.y;
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x0004456F File Offset: 0x0004276F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int2(int v)
		{
			return new int2(v);
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x00044577 File Offset: 0x00042777
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2(bool v)
		{
			return new int2(v);
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x0004457F File Offset: 0x0004277F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2(bool2 v)
		{
			return new int2(v);
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x00044587 File Offset: 0x00042787
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2(uint v)
		{
			return new int2(v);
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x0004458F File Offset: 0x0004278F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2(uint2 v)
		{
			return new int2(v);
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x00044597 File Offset: 0x00042797
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2(float v)
		{
			return new int2(v);
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x0004459F File Offset: 0x0004279F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2(float2 v)
		{
			return new int2(v);
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x000445A7 File Offset: 0x000427A7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2(double v)
		{
			return new int2(v);
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x000445AF File Offset: 0x000427AF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static explicit operator int2(double2 v)
		{
			return new int2(v);
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x000445B7 File Offset: 0x000427B7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator *(int2 lhs, int2 rhs)
		{
			return new int2(lhs.x * rhs.x, lhs.y * rhs.y);
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x000445D8 File Offset: 0x000427D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator *(int2 lhs, int rhs)
		{
			return new int2(lhs.x * rhs, lhs.y * rhs);
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x000445EF File Offset: 0x000427EF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator *(int lhs, int2 rhs)
		{
			return new int2(lhs * rhs.x, lhs * rhs.y);
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x00044606 File Offset: 0x00042806
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator +(int2 lhs, int2 rhs)
		{
			return new int2(lhs.x + rhs.x, lhs.y + rhs.y);
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x00044627 File Offset: 0x00042827
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator +(int2 lhs, int rhs)
		{
			return new int2(lhs.x + rhs, lhs.y + rhs);
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x0004463E File Offset: 0x0004283E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator +(int lhs, int2 rhs)
		{
			return new int2(lhs + rhs.x, lhs + rhs.y);
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x00044655 File Offset: 0x00042855
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator -(int2 lhs, int2 rhs)
		{
			return new int2(lhs.x - rhs.x, lhs.y - rhs.y);
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x00044676 File Offset: 0x00042876
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator -(int2 lhs, int rhs)
		{
			return new int2(lhs.x - rhs, lhs.y - rhs);
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x0004468D File Offset: 0x0004288D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator -(int lhs, int2 rhs)
		{
			return new int2(lhs - rhs.x, lhs - rhs.y);
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x000446A4 File Offset: 0x000428A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator /(int2 lhs, int2 rhs)
		{
			return new int2(lhs.x / rhs.x, lhs.y / rhs.y);
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x000446C5 File Offset: 0x000428C5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator /(int2 lhs, int rhs)
		{
			return new int2(lhs.x / rhs, lhs.y / rhs);
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x000446DC File Offset: 0x000428DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator /(int lhs, int2 rhs)
		{
			return new int2(lhs / rhs.x, lhs / rhs.y);
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x000446F3 File Offset: 0x000428F3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator %(int2 lhs, int2 rhs)
		{
			return new int2(lhs.x % rhs.x, lhs.y % rhs.y);
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x00044714 File Offset: 0x00042914
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator %(int2 lhs, int rhs)
		{
			return new int2(lhs.x % rhs, lhs.y % rhs);
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x0004472B File Offset: 0x0004292B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator %(int lhs, int2 rhs)
		{
			return new int2(lhs % rhs.x, lhs % rhs.y);
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x00044744 File Offset: 0x00042944
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator ++(int2 val)
		{
			int num = val.x + 1;
			val.x = num;
			int num2 = num;
			num = val.y + 1;
			val.y = num;
			return new int2(num2, num);
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x00044774 File Offset: 0x00042974
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator --(int2 val)
		{
			int num = val.x - 1;
			val.x = num;
			int num2 = num;
			num = val.y - 1;
			val.y = num;
			return new int2(num2, num);
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x000447A4 File Offset: 0x000429A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <(int2 lhs, int2 rhs)
		{
			return new bool2(lhs.x < rhs.x, lhs.y < rhs.y);
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x000447C7 File Offset: 0x000429C7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <(int2 lhs, int rhs)
		{
			return new bool2(lhs.x < rhs, lhs.y < rhs);
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x000447E0 File Offset: 0x000429E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <(int lhs, int2 rhs)
		{
			return new bool2(lhs < rhs.x, lhs < rhs.y);
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x000447F9 File Offset: 0x000429F9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <=(int2 lhs, int2 rhs)
		{
			return new bool2(lhs.x <= rhs.x, lhs.y <= rhs.y);
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x00044822 File Offset: 0x00042A22
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <=(int2 lhs, int rhs)
		{
			return new bool2(lhs.x <= rhs, lhs.y <= rhs);
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x00044841 File Offset: 0x00042A41
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator <=(int lhs, int2 rhs)
		{
			return new bool2(lhs <= rhs.x, lhs <= rhs.y);
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x00044860 File Offset: 0x00042A60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >(int2 lhs, int2 rhs)
		{
			return new bool2(lhs.x > rhs.x, lhs.y > rhs.y);
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x00044883 File Offset: 0x00042A83
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >(int2 lhs, int rhs)
		{
			return new bool2(lhs.x > rhs, lhs.y > rhs);
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x0004489C File Offset: 0x00042A9C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >(int lhs, int2 rhs)
		{
			return new bool2(lhs > rhs.x, lhs > rhs.y);
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x000448B5 File Offset: 0x00042AB5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >=(int2 lhs, int2 rhs)
		{
			return new bool2(lhs.x >= rhs.x, lhs.y >= rhs.y);
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x000448DE File Offset: 0x00042ADE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >=(int2 lhs, int rhs)
		{
			return new bool2(lhs.x >= rhs, lhs.y >= rhs);
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x000448FD File Offset: 0x00042AFD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator >=(int lhs, int2 rhs)
		{
			return new bool2(lhs >= rhs.x, lhs >= rhs.y);
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x0004491C File Offset: 0x00042B1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator -(int2 val)
		{
			return new int2(-val.x, -val.y);
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x00044931 File Offset: 0x00042B31
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator +(int2 val)
		{
			return new int2(val.x, val.y);
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x00044944 File Offset: 0x00042B44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator <<(int2 x, int n)
		{
			return new int2(x.x << n, x.y << n);
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x00044961 File Offset: 0x00042B61
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator >>(int2 x, int n)
		{
			return new int2(x.x >> n, x.y >> n);
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x0004497E File Offset: 0x00042B7E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ==(int2 lhs, int2 rhs)
		{
			return new bool2(lhs.x == rhs.x, lhs.y == rhs.y);
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x000449A1 File Offset: 0x00042BA1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ==(int2 lhs, int rhs)
		{
			return new bool2(lhs.x == rhs, lhs.y == rhs);
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x000449BA File Offset: 0x00042BBA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator ==(int lhs, int2 rhs)
		{
			return new bool2(lhs == rhs.x, lhs == rhs.y);
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x000449D3 File Offset: 0x00042BD3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !=(int2 lhs, int2 rhs)
		{
			return new bool2(lhs.x != rhs.x, lhs.y != rhs.y);
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x000449FC File Offset: 0x00042BFC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !=(int2 lhs, int rhs)
		{
			return new bool2(lhs.x != rhs, lhs.y != rhs);
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x00044A1B File Offset: 0x00042C1B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool2 operator !=(int lhs, int2 rhs)
		{
			return new bool2(lhs != rhs.x, lhs != rhs.y);
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x00044A3A File Offset: 0x00042C3A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator ~(int2 val)
		{
			return new int2(~val.x, ~val.y);
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x00044A4F File Offset: 0x00042C4F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator &(int2 lhs, int2 rhs)
		{
			return new int2(lhs.x & rhs.x, lhs.y & rhs.y);
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x00044A70 File Offset: 0x00042C70
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator &(int2 lhs, int rhs)
		{
			return new int2(lhs.x & rhs, lhs.y & rhs);
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x00044A87 File Offset: 0x00042C87
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator &(int lhs, int2 rhs)
		{
			return new int2(lhs & rhs.x, lhs & rhs.y);
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x00044A9E File Offset: 0x00042C9E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator |(int2 lhs, int2 rhs)
		{
			return new int2(lhs.x | rhs.x, lhs.y | rhs.y);
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x00044ABF File Offset: 0x00042CBF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator |(int2 lhs, int rhs)
		{
			return new int2(lhs.x | rhs, lhs.y | rhs);
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x00044AD6 File Offset: 0x00042CD6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator |(int lhs, int2 rhs)
		{
			return new int2(lhs | rhs.x, lhs | rhs.y);
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x00044AED File Offset: 0x00042CED
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator ^(int2 lhs, int2 rhs)
		{
			return new int2(lhs.x ^ rhs.x, lhs.y ^ rhs.y);
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x00044B0E File Offset: 0x00042D0E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator ^(int2 lhs, int rhs)
		{
			return new int2(lhs.x ^ rhs, lhs.y ^ rhs);
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x00044B25 File Offset: 0x00042D25
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int2 operator ^(int lhs, int2 rhs)
		{
			return new int2(lhs ^ rhs.x, lhs ^ rhs.y);
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x060018B4 RID: 6324 RVA: 0x00044B3C File Offset: 0x00042D3C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.x, this.x);
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x060018B5 RID: 6325 RVA: 0x00044B5B File Offset: 0x00042D5B
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.x, this.y);
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x060018B6 RID: 6326 RVA: 0x00044B7A File Offset: 0x00042D7A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.y, this.x);
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x060018B7 RID: 6327 RVA: 0x00044B99 File Offset: 0x00042D99
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.x, this.y, this.y);
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x060018B8 RID: 6328 RVA: 0x00044BB8 File Offset: 0x00042DB8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.x, this.x);
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x060018B9 RID: 6329 RVA: 0x00044BD7 File Offset: 0x00042DD7
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.x, this.y);
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x060018BA RID: 6330 RVA: 0x00044BF6 File Offset: 0x00042DF6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.y, this.x);
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x060018BB RID: 6331 RVA: 0x00044C15 File Offset: 0x00042E15
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 xyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.x, this.y, this.y, this.y);
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x060018BC RID: 6332 RVA: 0x00044C34 File Offset: 0x00042E34
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.x, this.x);
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x060018BD RID: 6333 RVA: 0x00044C53 File Offset: 0x00042E53
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.x, this.y);
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x060018BE RID: 6334 RVA: 0x00044C72 File Offset: 0x00042E72
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.y, this.x);
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x060018BF RID: 6335 RVA: 0x00044C91 File Offset: 0x00042E91
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yxyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.x, this.y, this.y);
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x060018C0 RID: 6336 RVA: 0x00044CB0 File Offset: 0x00042EB0
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.x, this.x);
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x060018C1 RID: 6337 RVA: 0x00044CCF File Offset: 0x00042ECF
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.x, this.y);
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x060018C2 RID: 6338 RVA: 0x00044CEE File Offset: 0x00042EEE
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.y, this.x);
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x060018C3 RID: 6339 RVA: 0x00044D0D File Offset: 0x00042F0D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int4 yyyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int4(this.y, this.y, this.y, this.y);
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x060018C4 RID: 6340 RVA: 0x00044D2C File Offset: 0x00042F2C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.x, this.x);
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x060018C5 RID: 6341 RVA: 0x00044D45 File Offset: 0x00042F45
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.x, this.y);
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x060018C6 RID: 6342 RVA: 0x00044D5E File Offset: 0x00042F5E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.y, this.x);
			}
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x060018C7 RID: 6343 RVA: 0x00044D77 File Offset: 0x00042F77
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 xyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.x, this.y, this.y);
			}
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x060018C8 RID: 6344 RVA: 0x00044D90 File Offset: 0x00042F90
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yxx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.x, this.x);
			}
		}

		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x060018C9 RID: 6345 RVA: 0x00044DA9 File Offset: 0x00042FA9
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yxy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.x, this.y);
			}
		}

		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x060018CA RID: 6346 RVA: 0x00044DC2 File Offset: 0x00042FC2
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yyx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.y, this.x);
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x060018CB RID: 6347 RVA: 0x00044DDB File Offset: 0x00042FDB
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int3 yyy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int3(this.y, this.y, this.y);
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x060018CC RID: 6348 RVA: 0x00044DF4 File Offset: 0x00042FF4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 xx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.x, this.x);
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x060018CD RID: 6349 RVA: 0x00044E07 File Offset: 0x00043007
		// (set) Token: 0x060018CE RID: 6350 RVA: 0x00044E1A File Offset: 0x0004301A
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 xy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.x, this.y);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.x = value.x;
				this.y = value.y;
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x060018CF RID: 6351 RVA: 0x00044E34 File Offset: 0x00043034
		// (set) Token: 0x060018D0 RID: 6352 RVA: 0x00044E47 File Offset: 0x00043047
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 yx
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.y, this.x);
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.y = value.x;
				this.x = value.y;
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x060018D1 RID: 6353 RVA: 0x00044E61 File Offset: 0x00043061
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int2 yy
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new int2(this.y, this.y);
			}
		}

		// Token: 0x170007CC RID: 1996
		public unsafe int this[int index]
		{
			get
			{
				fixed (int2* ptr = &this)
				{
					return ((int*)ptr)[index];
				}
			}
			set
			{
				fixed (int* ptr = &this.x)
				{
					ptr[index] = value;
				}
			}
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x00044EAC File Offset: 0x000430AC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(int2 rhs)
		{
			return this.x == rhs.x && this.y == rhs.y;
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x00044ECC File Offset: 0x000430CC
		public override bool Equals(object o)
		{
			if (o is int2)
			{
				int2 @int = (int2)o;
				return this.Equals(@int);
			}
			return false;
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x00044EF1 File Offset: 0x000430F1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return (int)math.hash(this);
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x00044EFE File Offset: 0x000430FE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("int2({0}, {1})", this.x, this.y);
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x00044F20 File Offset: 0x00043120
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string ToString(string format, IFormatProvider formatProvider)
		{
			return string.Format("int2({0}, {1})", this.x.ToString(format, formatProvider), this.y.ToString(format, formatProvider));
		}

		// Token: 0x040000BD RID: 189
		public int x;

		// Token: 0x040000BE RID: 190
		public int y;

		// Token: 0x040000BF RID: 191
		public static readonly int2 zero;

		// Token: 0x02000061 RID: 97
		internal sealed class DebuggerProxy
		{
			// Token: 0x060024CE RID: 9422 RVA: 0x0006930C File Offset: 0x0006750C
			public DebuggerProxy(int2 v)
			{
				this.x = v.x;
				this.y = v.y;
			}

			// Token: 0x0400016E RID: 366
			public int x;

			// Token: 0x0400016F RID: 367
			public int y;
		}
	}
}
