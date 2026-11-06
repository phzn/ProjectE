using System;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003A4 RID: 932
	[Serializable]
	public class VertLayoutDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x06001835 RID: 6197 RVA: 0x00099D58 File Offset: 0x00097F58
		public void Draw(FObject fobject)
		{
			VerticalLayoutGroup verticalLayoutGroup;
			fobject.Data.GameObject.TryAddComponent<VerticalLayoutGroup>(out verticalLayoutGroup, false);
			verticalLayoutGroup.childAlignment = fobject.GetVertLayoutAnchor();
			verticalLayoutGroup.padding = fobject.Data.FRect.padding.ToRectOffset();
			verticalLayoutGroup.reverseArrangement = false;
			verticalLayoutGroup.childScaleWidth = false;
			verticalLayoutGroup.childScaleHeight = false;
			ValueTuple<bool, bool> childControlByLayoutMode = fobject.GetChildControlByLayoutMode(LayoutMode.VERTICAL);
			verticalLayoutGroup.childControlWidth = childControlByLayoutMode.Item1;
			verticalLayoutGroup.childControlHeight = childControlByLayoutMode.Item2;
			verticalLayoutGroup.spacing = fobject.GetVertSpacing();
			if (verticalLayoutGroup.childControlWidth)
			{
				verticalLayoutGroup.childForceExpandWidth = true;
			}
			else if (fobject.CounterAxisAlignItems == CounterAxisAlignItem.SPACE_BETWEEN)
			{
				verticalLayoutGroup.childForceExpandWidth = true;
			}
			else
			{
				verticalLayoutGroup.childForceExpandWidth = false;
			}
			if (verticalLayoutGroup.childControlHeight)
			{
				verticalLayoutGroup.childForceExpandHeight = true;
			}
			else if (fobject.PrimaryAxisAlignItems == PrimaryAxisAlignItem.SPACE_BETWEEN)
			{
				verticalLayoutGroup.childForceExpandHeight = true;
			}
			else
			{
				verticalLayoutGroup.childForceExpandHeight = false;
			}
			ContentSizeFitter contentSizeFitter;
			fobject.Data.GameObject.TryAddComponent<ContentSizeFitter>(out contentSizeFitter, false);
			if (fobject.PrimaryAxisSizingMode == PrimaryAxisSizingMode.AUTO || fobject.PrimaryAxisSizingMode == PrimaryAxisSizingMode.NONE)
			{
				contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			}
			else
			{
				contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
			}
			if (fobject.CounterAxisSizingMode == CounterAxisSizingMode.AUTO || fobject.CounterAxisSizingMode == CounterAxisSizingMode.NONE)
			{
				contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
				return;
			}
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
		}
	}
}
