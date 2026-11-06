using System;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003A3 RID: 931
	[Serializable]
	public class LayoutElementDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x06001833 RID: 6195 RVA: 0x00099CEC File Offset: 0x00097EEC
		public void Draw(FObject fobject, FObject parent)
		{
			LayoutElement layoutElement;
			fobject.Data.GameObject.TryAddComponent<LayoutElement>(out layoutElement, false);
			if (parent.LayoutWrap == LayoutWrap.WRAP)
			{
				layoutElement.minWidth = fobject.Size.x;
				layoutElement.minHeight = fobject.Size.y;
			}
			if (fobject.LayoutPositioning == LayoutPositioning.ABSOLUTE)
			{
				layoutElement.ignoreLayout = true;
				return;
			}
			layoutElement.ignoreLayout = false;
		}
	}
}
