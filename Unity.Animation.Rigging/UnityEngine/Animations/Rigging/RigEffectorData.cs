using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000072 RID: 114
	[Serializable]
	public class RigEffectorData
	{
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060002DD RID: 733 RVA: 0x00009710 File Offset: 0x00007910
		public Transform transform
		{
			get
			{
				return this.m_Transform;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00009718 File Offset: 0x00007918
		public RigEffectorData.Style style
		{
			get
			{
				return this.m_Style;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060002DF RID: 735 RVA: 0x00009720 File Offset: 0x00007920
		// (set) Token: 0x060002E0 RID: 736 RVA: 0x00009728 File Offset: 0x00007928
		public bool visible
		{
			get
			{
				return this.m_Visible;
			}
			set
			{
				this.m_Visible = value;
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00009731 File Offset: 0x00007931
		public void Initialize(Transform transform, RigEffectorData.Style style)
		{
			this.m_Transform = transform;
			this.m_Style = style;
		}

		// Token: 0x04000131 RID: 305
		[SerializeField]
		private Transform m_Transform;

		// Token: 0x04000132 RID: 306
		[SerializeField]
		private RigEffectorData.Style m_Style;

		// Token: 0x04000133 RID: 307
		[SerializeField]
		private bool m_Visible = true;

		// Token: 0x02000089 RID: 137
		[Serializable]
		public struct Style
		{
			// Token: 0x04000179 RID: 377
			public Mesh shape;

			// Token: 0x0400017A RID: 378
			public Color color;

			// Token: 0x0400017B RID: 379
			public float size;

			// Token: 0x0400017C RID: 380
			public Vector3 position;

			// Token: 0x0400017D RID: 381
			public Vector3 rotation;
		}
	}
}
