using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002FF RID: 767
	internal struct StyleDataRef<T> : IEquatable<StyleDataRef<T>> where T : struct, IEquatable<T>, IStyleDataGroup<T>
	{
		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x06001A37 RID: 6711 RVA: 0x00068CDE File Offset: 0x00066EDE
		public int refCount
		{
			get
			{
				StyleDataRef<T>.RefCounted @ref = this.m_Ref;
				return (@ref != null) ? @ref.refCount : 0;
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x06001A38 RID: 6712 RVA: 0x00068CF2 File Offset: 0x00066EF2
		public uint id
		{
			get
			{
				StyleDataRef<T>.RefCounted @ref = this.m_Ref;
				return (@ref != null) ? @ref.id : 0U;
			}
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x00068D08 File Offset: 0x00066F08
		public StyleDataRef<T> Acquire()
		{
			this.m_Ref.Acquire();
			return this;
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x00068D2C File Offset: 0x00066F2C
		public void Release()
		{
			this.m_Ref.Release();
			this.m_Ref = null;
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x00068D44 File Offset: 0x00066F44
		public void CopyFrom(StyleDataRef<T> other)
		{
			bool flag = this.m_Ref.refCount == 1;
			if (flag)
			{
				this.m_Ref.value.CopyFrom(ref other.m_Ref.value);
			}
			else
			{
				this.m_Ref.Release();
				this.m_Ref = other.m_Ref;
				this.m_Ref.Acquire();
			}
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x00068DB0 File Offset: 0x00066FB0
		public readonly ref T Read()
		{
			return ref this.m_Ref.value;
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x00068DC0 File Offset: 0x00066FC0
		public ref T Write()
		{
			bool flag = this.m_Ref.refCount == 1;
			ref T ptr;
			if (flag)
			{
				ptr = ref this.m_Ref.value;
			}
			else
			{
				StyleDataRef<T>.RefCounted @ref = this.m_Ref;
				this.m_Ref = this.m_Ref.Copy();
				@ref.Release();
				ptr = ref this.m_Ref.value;
			}
			return ref ptr;
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x00068E1C File Offset: 0x0006701C
		public static StyleDataRef<T> Create()
		{
			return new StyleDataRef<T>
			{
				m_Ref = new StyleDataRef<T>.RefCounted()
			};
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x00068E44 File Offset: 0x00067044
		public override int GetHashCode()
		{
			return (this.m_Ref != null) ? this.m_Ref.value.GetHashCode() : 0;
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x00068E78 File Offset: 0x00067078
		public static bool operator ==(StyleDataRef<T> lhs, StyleDataRef<T> rhs)
		{
			return lhs.m_Ref == rhs.m_Ref || lhs.m_Ref.value.Equals(rhs.m_Ref.value);
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x00068EBC File Offset: 0x000670BC
		public static bool operator !=(StyleDataRef<T> lhs, StyleDataRef<T> rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x00068ED8 File Offset: 0x000670D8
		public bool Equals(StyleDataRef<T> other)
		{
			return other == this;
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x00068EF8 File Offset: 0x000670F8
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is StyleDataRef<T>)
			{
				StyleDataRef<T> styleDataRef = (StyleDataRef<T>)obj;
				flag = this.Equals(styleDataRef);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x00068F24 File Offset: 0x00067124
		public bool ReferenceEquals(StyleDataRef<T> other)
		{
			return this.m_Ref == other.m_Ref;
		}

		// Token: 0x04000AE3 RID: 2787
		private StyleDataRef<T>.RefCounted m_Ref;

		// Token: 0x02000300 RID: 768
		private class RefCounted
		{
			// Token: 0x1700066A RID: 1642
			// (get) Token: 0x06001A45 RID: 6725 RVA: 0x00068F44 File Offset: 0x00067144
			public int refCount
			{
				get
				{
					return this.m_RefCount;
				}
			}

			// Token: 0x1700066B RID: 1643
			// (get) Token: 0x06001A46 RID: 6726 RVA: 0x00068F4C File Offset: 0x0006714C
			public uint id
			{
				get
				{
					return this.m_Id;
				}
			}

			// Token: 0x06001A47 RID: 6727 RVA: 0x00068F54 File Offset: 0x00067154
			public RefCounted()
			{
				this.m_RefCount = 1;
				this.m_Id = (StyleDataRef<T>.RefCounted.m_NextId += 1U);
			}

			// Token: 0x06001A48 RID: 6728 RVA: 0x00068F78 File Offset: 0x00067178
			public void Acquire()
			{
				this.m_RefCount++;
			}

			// Token: 0x06001A49 RID: 6729 RVA: 0x00068F88 File Offset: 0x00067188
			public void Release()
			{
				this.m_RefCount--;
			}

			// Token: 0x06001A4A RID: 6730 RVA: 0x00068F9C File Offset: 0x0006719C
			public StyleDataRef<T>.RefCounted Copy()
			{
				return new StyleDataRef<T>.RefCounted
				{
					value = this.value.Copy()
				};
			}

			// Token: 0x04000AE4 RID: 2788
			private static uint m_NextId = 1U;

			// Token: 0x04000AE5 RID: 2789
			private int m_RefCount;

			// Token: 0x04000AE6 RID: 2790
			private readonly uint m_Id;

			// Token: 0x04000AE7 RID: 2791
			public T value;
		}
	}
}
