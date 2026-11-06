using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Rendering
{
	// Token: 0x020000D9 RID: 217
	internal ref struct HashFNV1A32
	{
		// Token: 0x06000752 RID: 1874 RVA: 0x00024038 File Offset: 0x00022238
		public static HashFNV1A32 Create()
		{
			return new HashFNV1A32
			{
				m_Hash = 2166136261U
			};
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0002405A File Offset: 0x0002225A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(in int input)
		{
			this.m_Hash = (this.m_Hash ^ (uint)input) * 16777619U;
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x00024071 File Offset: 0x00022271
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(in uint input)
		{
			this.m_Hash = (this.m_Hash ^ input) * 16777619U;
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x00024088 File Offset: 0x00022288
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(in bool input)
		{
			this.m_Hash = (this.m_Hash ^ (input ? 1U : 0U)) * 16777619U;
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x000240A5 File Offset: 0x000222A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(in float input)
		{
			this.m_Hash = (this.m_Hash ^ (uint)input.GetHashCode()) * 16777619U;
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x000240C0 File Offset: 0x000222C0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(in double input)
		{
			this.m_Hash = (this.m_Hash ^ (uint)input.GetHashCode()) * 16777619U;
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x000240DC File Offset: 0x000222DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(in Vector2 input)
		{
			uint hash = this.m_Hash;
			Vector2 vector = input;
			this.m_Hash = (hash ^ (uint)vector.GetHashCode()) * 16777619U;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00024110 File Offset: 0x00022310
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(in Vector3 input)
		{
			uint hash = this.m_Hash;
			Vector3 vector = input;
			this.m_Hash = (hash ^ (uint)vector.GetHashCode()) * 16777619U;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00024144 File Offset: 0x00022344
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(in Vector4 input)
		{
			uint hash = this.m_Hash;
			Vector4 vector = input;
			this.m_Hash = (hash ^ (uint)vector.GetHashCode()) * 16777619U;
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00024178 File Offset: 0x00022378
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append<T>(T input) where T : struct
		{
			this.m_Hash = (this.m_Hash ^ (uint)input.GetHashCode()) * 16777619U;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0002419A File Offset: 0x0002239A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(Delegate del)
		{
			this.m_Hash = (this.m_Hash ^ (uint)HashFNV1A32.GetFuncHashCode(del)) * 16777619U;
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x000241B5 File Offset: 0x000223B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int GetFuncHashCode(Delegate del)
		{
			return del.Method.GetHashCode() ^ ((del.Target != null) ? RuntimeHelpers.GetHashCode(del.Target) : 0);
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x000241D9 File Offset: 0x000223D9
		public int value
		{
			get
			{
				return (int)this.m_Hash;
			}
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x000241E1 File Offset: 0x000223E1
		public override int GetHashCode()
		{
			return this.value;
		}

		// Token: 0x0400049B RID: 1179
		private const uint k_Prime = 16777619U;

		// Token: 0x0400049C RID: 1180
		private const uint k_OffsetBasis = 2166136261U;

		// Token: 0x0400049D RID: 1181
		private uint m_Hash;
	}
}
