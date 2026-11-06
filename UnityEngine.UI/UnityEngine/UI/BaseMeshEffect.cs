using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	// Token: 0x02000040 RID: 64
	[ExecuteAlways]
	public abstract class BaseMeshEffect : UIBehaviour, IMeshModifier
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060004AA RID: 1194 RVA: 0x000169DD File Offset: 0x00014BDD
		protected Graphic graphic
		{
			get
			{
				if (this.m_Graphic == null)
				{
					this.m_Graphic = base.GetComponent<Graphic>();
				}
				return this.m_Graphic;
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x000169FF File Offset: 0x00014BFF
		protected override void OnEnable()
		{
			base.OnEnable();
			if (this.graphic != null)
			{
				this.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00016A20 File Offset: 0x00014C20
		protected override void OnDisable()
		{
			if (this.graphic != null)
			{
				this.graphic.SetVerticesDirty();
			}
			base.OnDisable();
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00016A41 File Offset: 0x00014C41
		protected override void OnDidApplyAnimationProperties()
		{
			if (this.graphic != null)
			{
				this.graphic.SetVerticesDirty();
			}
			base.OnDidApplyAnimationProperties();
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00016A64 File Offset: 0x00014C64
		public virtual void ModifyMesh(Mesh mesh)
		{
			using (VertexHelper vertexHelper = new VertexHelper(mesh))
			{
				this.ModifyMesh(vertexHelper);
				vertexHelper.FillMesh(mesh);
			}
		}

		// Token: 0x060004AF RID: 1199
		public abstract void ModifyMesh(VertexHelper vh);

		// Token: 0x04000190 RID: 400
		[NonSerialized]
		private Graphic m_Graphic;
	}
}
