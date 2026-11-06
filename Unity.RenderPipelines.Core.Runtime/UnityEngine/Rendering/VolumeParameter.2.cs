using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UnityEngine.Rendering
{
	// Token: 0x020000EF RID: 239
	[DebuggerDisplay("{m_Value} ({m_OverrideState})")]
	[Serializable]
	public class VolumeParameter<T> : VolumeParameter, IEquatable<VolumeParameter<T>>
	{
		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x00026981 File Offset: 0x00024B81
		// (set) Token: 0x060007E8 RID: 2024 RVA: 0x00026989 File Offset: 0x00024B89
		public virtual T value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value = value;
			}
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00026994 File Offset: 0x00024B94
		public VolumeParameter()
			: this(default(T), false)
		{
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x000269B1 File Offset: 0x00024BB1
		protected VolumeParameter(T value, bool overrideState)
		{
			this.m_Value = value;
			this.overrideState = overrideState;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x000269C7 File Offset: 0x00024BC7
		internal override void Interp(VolumeParameter from, VolumeParameter to, float t)
		{
			this.Interp((from as VolumeParameter<T>).value, (to as VolumeParameter<T>).value, t);
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x000269E6 File Offset: 0x00024BE6
		public virtual void Interp(T from, T to, float t)
		{
			this.m_Value = ((t > 0f) ? to : from);
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x000269FA File Offset: 0x00024BFA
		public void Override(T x)
		{
			this.overrideState = true;
			this.m_Value = x;
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00026A0A File Offset: 0x00024C0A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override void SetValue(VolumeParameter parameter)
		{
			this.m_Value = ((VolumeParameter<T>)parameter).m_Value;
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00026A20 File Offset: 0x00024C20
		public override int GetHashCode()
		{
			int num = 17;
			num = num * 23 + this.overrideState.GetHashCode();
			if (!EqualityComparer<T>.Default.Equals(this.value, default(T)))
			{
				int num2 = num * 23;
				T value = this.value;
				num = num2 + value.GetHashCode();
			}
			return num;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00026A7A File Offset: 0x00024C7A
		public override string ToString()
		{
			return string.Format("{0} ({1})", this.value, this.overrideState);
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00026A9C File Offset: 0x00024C9C
		public static bool operator ==(VolumeParameter<T> lhs, T rhs)
		{
			if (lhs != null && lhs.value != null)
			{
				T value = lhs.value;
				return value.Equals(rhs);
			}
			return false;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00026AD5 File Offset: 0x00024CD5
		public static bool operator !=(VolumeParameter<T> lhs, T rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00026AE1 File Offset: 0x00024CE1
		public bool Equals(VolumeParameter<T> other)
		{
			return other != null && (this == other || EqualityComparer<T>.Default.Equals(this.m_Value, other.m_Value));
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00026B04 File Offset: 0x00024D04
		public override bool Equals(object obj)
		{
			return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((VolumeParameter<T>)obj)));
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00026B32 File Offset: 0x00024D32
		public override object Clone()
		{
			return new VolumeParameter<T>(base.GetValue<T>(), this.overrideState);
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00026B45 File Offset: 0x00024D45
		public static explicit operator T(VolumeParameter<T> prop)
		{
			return prop.m_Value;
		}

		// Token: 0x040004D4 RID: 1236
		[SerializeField]
		protected T m_Value;
	}
}
