using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x02000324 RID: 804
	[StructLayout(LayoutKind.Explicit, Size = 4)]
	public struct TypeIndex : IComparable<TypeIndex>, IEquatable<TypeIndex>
	{
		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06001561 RID: 5473 RVA: 0x00050ED0 File Offset: 0x0004F0D0
		public static TypeIndex Null
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return default(TypeIndex);
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06001562 RID: 5474 RVA: 0x00050EE6 File Offset: 0x0004F0E6
		public bool IsBuffer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 67108864) != 0;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06001563 RID: 5475 RVA: 0x00050EF7 File Offset: 0x0004F0F7
		public bool IsCleanupComponent
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 33554432) != 0;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06001564 RID: 5476 RVA: 0x00050F08 File Offset: 0x0004F108
		public bool IsCleanupSharedComponent
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 167772160) == 167772160;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06001565 RID: 5477 RVA: 0x00050F1D File Offset: 0x0004F11D
		public bool IsCleanupBufferComponent
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 100663296) == 100663296;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06001566 RID: 5478 RVA: 0x00050F32 File Offset: 0x0004F132
		public bool IsComponentType
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 201326592) == 0;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06001567 RID: 5479 RVA: 0x00050F43 File Offset: 0x0004F143
		public bool IsSharedComponentType
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 134217728) != 0;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06001568 RID: 5480 RVA: 0x00050F54 File Offset: 0x0004F154
		public bool IsIEquatable
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 8388608) != 0;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06001569 RID: 5481 RVA: 0x00050F65 File Offset: 0x0004F165
		public bool IsManagedComponent
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 939524096) == 268435456;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600156A RID: 5482 RVA: 0x00050F7A File Offset: 0x0004F17A
		public bool IsManagedSharedComponent
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 402653184) == 402653184;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600156B RID: 5483 RVA: 0x00050F8F File Offset: 0x0004F18F
		public bool IsManagedType
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 268435456) != 0;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600156C RID: 5484 RVA: 0x00050FA0 File Offset: 0x0004F1A0
		public bool IsZeroSized
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 1073741824) != 0;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600156D RID: 5485 RVA: 0x00050FB1 File Offset: 0x0004F1B1
		public bool IsChunkComponent
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 536870912) != 0;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x0600156E RID: 5486 RVA: 0x00050FC2 File Offset: 0x0004F1C2
		public bool IsEnableable
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 16777216) != 0;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600156F RID: 5487 RVA: 0x00050FD3 File Offset: 0x0004F1D3
		public bool IsRefCounted
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 4194304) != 0;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06001570 RID: 5488 RVA: 0x00050FE4 File Offset: 0x0004F1E4
		public bool HasEntityReferences
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 131072) == 0;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06001571 RID: 5489 RVA: 0x00050FF5 File Offset: 0x0004F1F5
		public bool HasNativeContainer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 524288) != 0;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06001572 RID: 5490 RVA: 0x00051006 File Offset: 0x0004F206
		public bool IsChunkSerializable
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 262144) == 0;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06001573 RID: 5491 RVA: 0x00051017 File Offset: 0x0004F217
		public bool IsTemporaryBakingType
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 2097152) != 0;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06001574 RID: 5492 RVA: 0x00051028 File Offset: 0x0004F228
		public bool IsBakingOnlyType
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.Value & 1048576) != 0;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06001575 RID: 5493 RVA: 0x00051039 File Offset: 0x0004F239
		public int Index
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.Value & 8191;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06001576 RID: 5494 RVA: 0x00051047 File Offset: 0x0004F247
		public int Flags
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.Value & -8192;
			}
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x00051055 File Offset: 0x0004F255
		public static implicit operator int(TypeIndex ti)
		{
			return ti.Value;
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x00051060 File Offset: 0x0004F260
		public static implicit operator TypeIndex(int value)
		{
			return new TypeIndex
			{
				Value = value
			};
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x0005107E File Offset: 0x0004F27E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(TypeIndex lhs, TypeIndex rhs)
		{
			return lhs.Value == rhs.Value;
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x0005108E File Offset: 0x0004F28E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(TypeIndex lhs, TypeIndex rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x0005109A File Offset: 0x0004F29A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(TypeIndex lhs, TypeIndex rhs)
		{
			return lhs.Value < rhs.Value;
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x000510AA File Offset: 0x0004F2AA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(TypeIndex lhs, TypeIndex rhs)
		{
			return lhs.Value > rhs.Value;
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x000510BA File Offset: 0x0004F2BA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(TypeIndex lhs, TypeIndex rhs)
		{
			return lhs.Value <= rhs.Value;
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x000510CD File Offset: 0x0004F2CD
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(TypeIndex lhs, TypeIndex rhs)
		{
			return lhs.Value >= rhs.Value;
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x000510E0 File Offset: 0x0004F2E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(TypeIndex other)
		{
			return this.Value - other.Value;
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x000510F0 File Offset: 0x0004F2F0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object compare)
		{
			if (compare is TypeIndex)
			{
				TypeIndex typeIndex = (TypeIndex)compare;
				return this.Equals(typeIndex);
			}
			return false;
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x00051055 File Offset: 0x0004F255
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return this.Value;
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x00051115 File Offset: 0x0004F315
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(TypeIndex typeIndex)
		{
			return typeIndex.Value == this.Value;
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x00051128 File Offset: 0x0004F328
		public override string ToString()
		{
			return this.ToFixedString().ToString();
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x0005114C File Offset: 0x0004F34C
		[GenerateTestsForBurstCompatibility]
		public FixedString128Bytes ToFixedString()
		{
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			NativeText.ReadOnly debugTypeName = TypeManager.GetTypeInfo(this).DebugTypeName;
			(ref fixedString128Bytes).Append<FixedString128Bytes, NativeText.ReadOnly>(in debugTypeName);
			return fixedString128Bytes;
		}

		// Token: 0x040007CC RID: 1996
		[FieldOffset(0)]
		public int Value;
	}
}
