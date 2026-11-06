using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x0200034C RID: 844
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	public struct SystemTypeIndex : IComparable<SystemTypeIndex>, IEquatable<SystemTypeIndex>
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06001668 RID: 5736 RVA: 0x0005579C File Offset: 0x0005399C
		public static SystemTypeIndex Null
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return default(SystemTypeIndex);
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06001669 RID: 5737 RVA: 0x000557B2 File Offset: 0x000539B2
		public bool IsManaged
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 536870912) != 0;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600166A RID: 5738 RVA: 0x000557C3 File Offset: 0x000539C3
		public bool IsGroup
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 1073741824) != 0;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600166B RID: 5739 RVA: 0x000557D4 File Offset: 0x000539D4
		internal bool HasDefaultCtor
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 134217728) != 0;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600166C RID: 5740 RVA: 0x000557E5 File Offset: 0x000539E5
		public bool IsISystemStartStop
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 268435456) != 0;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600166D RID: 5741 RVA: 0x000557F6 File Offset: 0x000539F6
		public int Index
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.Value & 134217727;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600166E RID: 5742 RVA: 0x00055804 File Offset: 0x00053A04
		// (set) Token: 0x0600166F RID: 5743 RVA: 0x00055812 File Offset: 0x00053A12
		public int Flags
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.Value & -134217728;
			}
			set
			{
				this.Value = (this.Value & 134217727) | value;
			}
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x00055828 File Offset: 0x00053A28
		public static implicit operator int(SystemTypeIndex ti)
		{
			return ti.Value;
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x00055830 File Offset: 0x00053A30
		public static implicit operator SystemTypeIndex(int value)
		{
			return new SystemTypeIndex
			{
				Value = value
			};
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x0005584E File Offset: 0x00053A4E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(SystemTypeIndex lhs, SystemTypeIndex rhs)
		{
			return lhs.Value == rhs.Value;
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x0005585E File Offset: 0x00053A5E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(SystemTypeIndex lhs, SystemTypeIndex rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x0005586A File Offset: 0x00053A6A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(SystemTypeIndex lhs, SystemTypeIndex rhs)
		{
			return lhs.Value < rhs.Value;
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x0005587A File Offset: 0x00053A7A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(SystemTypeIndex lhs, SystemTypeIndex rhs)
		{
			return lhs.Value > rhs.Value;
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x0005588A File Offset: 0x00053A8A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(SystemTypeIndex lhs, SystemTypeIndex rhs)
		{
			return lhs.Value <= rhs.Value;
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x0005589D File Offset: 0x00053A9D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(SystemTypeIndex lhs, SystemTypeIndex rhs)
		{
			return lhs.Value >= rhs.Value;
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x000558B0 File Offset: 0x00053AB0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(SystemTypeIndex other)
		{
			return this.Value - other.Value;
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x000558C0 File Offset: 0x00053AC0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object compare)
		{
			if (compare is SystemTypeIndex)
			{
				SystemTypeIndex systemTypeIndex = (SystemTypeIndex)compare;
				return this.Equals(systemTypeIndex);
			}
			return false;
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x00055828 File Offset: 0x00053A28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return this.Value;
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x000558E5 File Offset: 0x00053AE5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(SystemTypeIndex typeIndex)
		{
			return typeIndex.Value == this.Value;
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x000558F8 File Offset: 0x00053AF8
		public override string ToString()
		{
			return this.ToFixedString().ToString();
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x0005591C File Offset: 0x00053B1C
		[GenerateTestsForBurstCompatibility]
		public FixedString128Bytes ToFixedString()
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			NativeText.ReadOnly systemName = TypeManager.GetSystemName(this);
			(ref fixedString128Bytes).Append<FixedString128Bytes, NativeText.ReadOnly>(in systemName);
			return fixedString128Bytes;
		}

		// Token: 0x04000874 RID: 2164
		[FieldOffset(0)]
		public int Value;
	}
}
