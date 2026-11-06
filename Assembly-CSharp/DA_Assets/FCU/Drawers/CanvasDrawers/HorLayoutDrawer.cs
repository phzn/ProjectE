using System;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003A2 RID: 930
	[Serializable]
	public class HorLayoutDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x06001831 RID: 6193 RVA: 0x00099BA8 File Offset: 0x00097DA8
		public void Draw(FObject fobject)
		{
			HorizontalLayoutGroup horizontalLayoutGroup;
			fobject.Data.GameObject.TryAddComponent<HorizontalLayoutGroup>(out horizontalLayoutGroup, false);
			horizontalLayoutGroup.childAlignment = fobject.GetHorLayoutAnchor();
			horizontalLayoutGroup.padding = fobject.Data.FRect.padding.ToRectOffset();
			horizontalLayoutGroup.reverseArrangement = false;
			horizontalLayoutGroup.childScaleWidth = false;
			horizontalLayoutGroup.childScaleHeight = false;
			ValueTuple<bool, bool> childControlByLayoutMode = fobject.GetChildControlByLayoutMode(LayoutMode.HORIZONTAL);
			horizontalLayoutGroup.childControlWidth = childControlByLayoutMode.Item1;
			horizontalLayoutGroup.childControlHeight = childControlByLayoutMode.Item2;
			horizontalLayoutGroup.spacing = fobject.GetHorSpacing();
			if (horizontalLayoutGroup.childControlWidth)
			{
				horizontalLayoutGroup.childForceExpandWidth = true;
			}
			else if (fobject.PrimaryAxisAlignItems == PrimaryAxisAlignItem.SPACE_BETWEEN)
			{
				horizontalLayoutGroup.childForceExpandWidth = true;
			}
			else
			{
				horizontalLayoutGroup.childForceExpandWidth = false;
			}
			if (horizontalLayoutGroup.childControlHeight)
			{
				horizontalLayoutGroup.childForceExpandHeight = true;
			}
			else if (fobject.CounterAxisAlignItems == CounterAxisAlignItem.SPACE_BETWEEN)
			{
				horizontalLayoutGroup.childForceExpandHeight = true;
			}
			else
			{
				horizontalLayoutGroup.childForceExpandHeight = false;
			}
			ContentSizeFitter contentSizeFitter;
			fobject.Data.GameObject.TryAddComponent<ContentSizeFitter>(out contentSizeFitter, false);
			if (fobject.PrimaryAxisSizingMode == PrimaryAxisSizingMode.AUTO || fobject.PrimaryAxisSizingMode == PrimaryAxisSizingMode.NONE)
			{
				contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			}
			else
			{
				contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
			}
			if (fobject.CounterAxisSizingMode == CounterAxisSizingMode.AUTO || fobject.CounterAxisSizingMode == CounterAxisSizingMode.NONE)
			{
				contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
				return;
			}
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
		}
	}
}
