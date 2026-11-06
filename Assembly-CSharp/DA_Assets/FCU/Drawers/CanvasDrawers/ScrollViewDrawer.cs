using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003BF RID: 959
	[Serializable]
	public class ScrollViewDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x060018A2 RID: 6306 RVA: 0x0009D06E File Offset: 0x0009B26E
		public List<FObject> ScrollViews
		{
			get
			{
				return this.scrollViews;
			}
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x0009D076 File Offset: 0x0009B276
		public void Init()
		{
			this.scrollViews = new List<FObject>();
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x0009D084 File Offset: 0x0009B284
		public void Draw(FObject fobject)
		{
			ScrollRect scrollRect;
			fobject.Data.GameObject.TryAddComponent<ScrollRect>(out scrollRect, false);
			this.scrollViews.Add(fobject);
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x0009D0B4 File Offset: 0x0009B2B4
		public async Task SetTargetGraphics()
		{
			await this.SetTargetGraphicsScrollViews();
			this.scrollViews.Clear();
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x0009D0F8 File Offset: 0x0009B2F8
		private async Task SetTargetGraphicsScrollViews()
		{
			foreach (FObject fobject in this.scrollViews)
			{
				ScrollRect scrollRect;
				if (fobject.Data.GameObject.TryGetComponentSafe<ScrollRect>(out scrollRect))
				{
					ScrollViewModel graphics = this.GetGraphics(fobject.Data);
					RectTransform rectTransform;
					if (graphics.Viewport.TryGetComponentSafe<RectTransform>(out rectTransform))
					{
						RectMask2D rectMask2D;
						rectTransform.gameObject.TryAddComponent<RectMask2D>(out rectMask2D, false);
						rectTransform.SetSmartAnchor(AnchorType.StretchAll);
						rectTransform.SetSmartPivot(PivotType.TopLeft);
						SyncHelper syncHelper;
						rectTransform.TryGetComponent<SyncHelper>(out syncHelper);
						FObject fobject2;
						this.monoBeh.CurrentProject.TryGetById(syncHelper.Data.Id, out fobject2);
						scrollRect.viewport = rectTransform;
						if (rectTransform != null)
						{
							switch (fobject2.OverflowDirection)
							{
							case OverflowDirection.HORIZONTAL_SCROLLING:
								scrollRect.horizontal = true;
								scrollRect.vertical = false;
								break;
							case OverflowDirection.VERTICAL_SCROLLING:
								scrollRect.horizontal = false;
								scrollRect.vertical = true;
								break;
							case OverflowDirection.HORIZONTAL_AND_VERTICAL_SCROLLING:
								scrollRect.horizontal = true;
								scrollRect.vertical = true;
								break;
							default:
								scrollRect.horizontal = false;
								scrollRect.vertical = false;
								break;
							}
						}
					}
					RectTransform rectTransform2;
					if (graphics.Content.TryGetComponentSafe<RectTransform>(out rectTransform2))
					{
						rectTransform2.gameObject.TryDestroyComponent<RectMask2D>();
						rectTransform2.SetSmartAnchor(AnchorType.HorStretchTop);
						rectTransform2.SetSmartPivot(PivotType.TopLeft);
						scrollRect.content = rectTransform2;
					}
					scrollRect.enabled = false;
					await Task.Delay(10);
					scrollRect.enabled = true;
					scrollRect = null;
				}
			}
			List<FObject>.Enumerator enumerator = default(List<FObject>.Enumerator);
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x0009D13C File Offset: 0x0009B33C
		private ScrollViewModel GetGraphics(SyncData syncData)
		{
			SyncHelper[] childs = syncData.GameObject.GetChilds<SyncHelper>();
			ScrollViewModel scrollViewModel = default(ScrollViewModel);
			foreach (SyncHelper syncHelper in childs)
			{
				if (scrollViewModel.Viewport == null && syncHelper.name.IsScrollViewport())
				{
					scrollViewModel.Viewport = syncHelper.gameObject;
				}
				else if (scrollViewModel.Content == null && syncHelper.name.IsScrollContent())
				{
					scrollViewModel.Content = syncHelper.gameObject;
				}
			}
			return scrollViewModel;
		}

		// Token: 0x040019C1 RID: 6593
		private List<FObject> scrollViews;
	}
}
