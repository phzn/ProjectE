using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020002F8 RID: 760
	public struct Scale : IEquatable<Scale>
	{
		// Token: 0x060019C6 RID: 6598 RVA: 0x00068004 File Offset: 0x00066204
		public Scale(Vector2 scale)
		{
			this.m_Scale = new Vector3(scale.x, scale.y, 1f);
			this.m_IsNone = false;
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x0006802C File Offset: 0x0006622C
		public Scale(Vector3 scale)
		{
			bool flag = !Mathf.Approximately(1f, scale.z);
			if (flag)
			{
				Debug.LogWarning("Assigning Z scale different than 1.0f, this is not yet supported. Forcing the value to 1.0f.");
				scale.z = 1f;
			}
			this.m_Scale = scale;
			this.m_IsNone = false;
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x00068078 File Offset: 0x00066278
		internal static Scale Initial()
		{
			return new Scale(Vector3.one);
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x00068094 File Offset: 0x00066294
		public static Scale None()
		{
			Scale scale = Scale.Initial();
			scale.m_IsNone = true;
			return scale;
		}

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x060019CA RID: 6602 RVA: 0x000680B5 File Offset: 0x000662B5
		// (set) Token: 0x060019CB RID: 6603 RVA: 0x000680BD File Offset: 0x000662BD
		public Vector3 value
		{
			get
			{
				return this.m_Scale;
			}
			set
			{
				this.m_Scale = value;
			}
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x000680C6 File Offset: 0x000662C6
		internal bool IsNone()
		{
			return this.m_IsNone;
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x000680D0 File Offset: 0x000662D0
		public static implicit operator Scale(Vector2 scale)
		{
			return new Scale(scale);
		}

		// Token: 0x060019CE RID: 6606 RVA: 0x000680E8 File Offset: 0x000662E8
		public static bool operator ==(Scale lhs, Scale rhs)
		{
			return lhs.m_Scale == rhs.m_Scale;
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x0006810C File Offset: 0x0006630C
		public static bool operator !=(Scale lhs, Scale rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x00068128 File Offset: 0x00066328
		public bool Equals(Scale other)
		{
			return other == this;
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x00068148 File Offset: 0x00066348
		public override bool Equals(object obj)
		{
			bool flag;
			if (obj is Scale)
			{
				Scale scale = (Scale)obj;
				flag = this.Equals(scale);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x00068174 File Offset: 0x00066374
		public override int GetHashCode()
		{
			return this.m_Scale.GetHashCode() * 793;
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x000681A0 File Offset: 0x000663A0
		public override string ToString()
		{
			return this.m_Scale.ToString();
		}

		// Token: 0x04000AD5 RID: 2773
		private Vector3 m_Scale;

		// Token: 0x04000AD6 RID: 2774
		private bool m_IsNone;
	}
}
