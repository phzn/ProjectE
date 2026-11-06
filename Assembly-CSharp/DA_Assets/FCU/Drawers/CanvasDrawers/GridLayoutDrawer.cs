using System;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003A1 RID: 929
	[Serializable]
	public class GridLayoutDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x0600182F RID: 6191 RVA: 0x00099B7C File Offset: 0x00097D7C
		public void Draw(FObject fobject)
		{
			LayoutGroup layoutGroup;
			if (fobject.Data.GameObject.TryGetComponentSafe<LayoutGroup>(out layoutGroup))
			{
				layoutGroup.Destroy();
			}
		}
	}
}
