using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003C8 RID: 968
	[Serializable]
	public class ToggleDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x060018D8 RID: 6360 RVA: 0x0009DFFA File Offset: 0x0009C1FA
		public List<FObject> Toggles
		{
			get
			{
				return this.toggles;
			}
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x0009E002 File Offset: 0x0009C202
		public void Init()
		{
			this.toggles = new List<FObject>();
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x0009E010 File Offset: 0x0009C210
		public void Draw(FObject fobject)
		{
			if (fobject.ContainsTag(FcuTag.Toggle))
			{
				Toggle toggle;
				fobject.Data.GameObject.TryAddComponent<Toggle>(out toggle, false);
				this.toggles.Add(fobject);
				return;
			}
			if (fobject.ContainsTag(FcuTag.ToggleGroup))
			{
				ToggleGroup toggleGroup;
				fobject.Data.GameObject.TryAddComponent<ToggleGroup>(out toggleGroup, false);
			}
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x0009E070 File Offset: 0x0009C270
		public async Task SetTargetGraphics()
		{
			foreach (FObject fobject in this.toggles)
			{
				Toggle toggle;
				if (fobject.Data.GameObject.TryGetComponentSafe<Toggle>(out toggle))
				{
					ToggleModel graphics = this.GetGraphics(fobject.Data);
					Graphic graphic;
					if (graphics.Checkmark.TryGetComponentSafe<Graphic>(out graphic))
					{
						toggle.graphic = graphic;
					}
					Graphic graphic2;
					if (graphics.Background.TryGetComponentSafe<Graphic>(out graphic2))
					{
						toggle.targetGraphic = graphic2;
					}
					ToggleGroup toggleGroup;
					if (graphics.ToggleGroup.TryGetComponentSafe<ToggleGroup>(out toggleGroup))
					{
						toggle.group = toggleGroup;
					}
					toggle.enabled = false;
					await Task.Delay(10);
					toggle.enabled = true;
					toggle = null;
				}
			}
			List<FObject>.Enumerator enumerator = default(List<FObject>.Enumerator);
			this.toggles.Clear();
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x0009E0B4 File Offset: 0x0009C2B4
		private ToggleModel GetGraphics(SyncData syncData)
		{
			SyncHelper[] childs = syncData.GameObject.GetChilds<SyncHelper>();
			List<Transform> allParents = syncData.GameObject.transform.GetAllParents();
			ToggleModel toggleModel = default(ToggleModel);
			foreach (SyncHelper syncHelper in childs)
			{
				if (toggleModel.Background == null && syncHelper.ContainsTag(FcuTag.Background))
				{
					toggleModel.Background = syncHelper.gameObject;
					break;
				}
			}
			foreach (Transform transform in allParents)
			{
				SyncHelper syncHelper2;
				if (transform.TryGetComponentSafe<SyncHelper>(out syncHelper2) && syncHelper2.ContainsTag(FcuTag.ToggleGroup))
				{
					toggleModel.ToggleGroup = transform.gameObject;
					break;
				}
			}
			foreach (SyncHelper syncHelper3 in childs)
			{
				if (toggleModel.Checkmark == null && syncHelper3.name.IsCheckmark())
				{
					toggleModel.Checkmark = syncHelper3.gameObject;
				}
				else if (toggleModel.Background == null && syncHelper3.ContainsTag(FcuTag.Image))
				{
					toggleModel.Background = syncHelper3.gameObject;
				}
			}
			return toggleModel;
		}

		// Token: 0x040019DA RID: 6618
		private List<FObject> toggles;
	}
}
