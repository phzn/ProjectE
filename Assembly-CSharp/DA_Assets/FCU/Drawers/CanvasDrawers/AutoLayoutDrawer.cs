using System;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x0200039F RID: 927
	[Serializable]
	public class AutoLayoutDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x06001827 RID: 6183 RVA: 0x0009999C File Offset: 0x00097B9C
		public void Draw(FObject fobject)
		{
			if (fobject.Data.FRect.absoluteAngle != 0f)
			{
				return;
			}
			foreach (int num in fobject.Data.ChildIndexes)
			{
				FObject fobject2;
				if (this.monoBeh.CurrentProject.TryGetByIndex(num, out fobject2))
				{
					if (fobject2.Data.FRect.absoluteAngle != 0f)
					{
						return;
					}
					if (fobject2.Data.GameObject != null)
					{
						this.LayoutElementDrawer.Draw(fobject2, fobject);
					}
				}
			}
			LayoutGroup layoutGroup;
			if (fobject.Data.GameObject.TryGetComponentSafe<LayoutGroup>(out layoutGroup))
			{
				layoutGroup.Destroy();
			}
			if (fobject.LayoutWrap == LayoutWrap.WRAP)
			{
				this.GridLayoutDrawer.Draw(fobject);
				return;
			}
			if (fobject.LayoutMode == LayoutMode.HORIZONTAL)
			{
				this.HorLayoutDrawer.Draw(fobject);
				return;
			}
			if (fobject.LayoutMode == LayoutMode.VERTICAL)
			{
				this.VertLayoutDrawer.Draw(fobject);
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001828 RID: 6184 RVA: 0x00099AC4 File Offset: 0x00097CC4
		[SerializeProperty("gridLayoutDrawer")]
		public GridLayoutDrawer GridLayoutDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.gridLayoutDrawer);
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001829 RID: 6185 RVA: 0x00099AD7 File Offset: 0x00097CD7
		[SerializeProperty("vertLayoutDrawer")]
		public VertLayoutDrawer VertLayoutDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.vertLayoutDrawer);
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x0600182A RID: 6186 RVA: 0x00099AEA File Offset: 0x00097CEA
		[SerializeProperty("horLayoutDrawer")]
		public HorLayoutDrawer HorLayoutDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.horLayoutDrawer);
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x0600182B RID: 6187 RVA: 0x00099AFD File Offset: 0x00097CFD
		[SerializeProperty("layoutElementDrawer")]
		public LayoutElementDrawer LayoutElementDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.layoutElementDrawer);
			}
		}

		// Token: 0x0400197F RID: 6527
		[SerializeField]
		private GridLayoutDrawer gridLayoutDrawer;

		// Token: 0x04001980 RID: 6528
		[SerializeField]
		private VertLayoutDrawer vertLayoutDrawer;

		// Token: 0x04001981 RID: 6529
		[SerializeField]
		private HorLayoutDrawer horLayoutDrawer;

		// Token: 0x04001982 RID: 6530
		[SerializeField]
		private LayoutElementDrawer layoutElementDrawer;
	}
}
