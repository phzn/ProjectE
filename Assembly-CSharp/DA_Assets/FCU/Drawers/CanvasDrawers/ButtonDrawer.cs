using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DA_Assets.DAB;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003A5 RID: 933
	[Serializable]
	public class ButtonDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06001837 RID: 6199 RVA: 0x00099E9A File Offset: 0x0009809A
		public List<FObject> Buttons
		{
			get
			{
				return this.buttons;
			}
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x00099EA2 File Offset: 0x000980A2
		public void Init()
		{
			this.buttons.Clear();
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x00099EB0 File Offset: 0x000980B0
		public void Draw(FObject fobject)
		{
			fobject.Data.ButtonComponent = this.monoBeh.Settings.ButtonSettings.ButtonComponent;
			if (this.monoBeh.Settings.ButtonSettings.ButtonComponent == ButtonComponent.DAButton)
			{
				DAButton dabutton;
				fobject.Data.GameObject.TryAddComponent<DAButton>(out dabutton, false);
			}
			else
			{
				Button button;
				fobject.Data.GameObject.TryAddComponent<Button>(out button, false);
			}
			this.buttons.Add(fobject);
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x00099F30 File Offset: 0x00098130
		public async Task SetTargetGraphics()
		{
			foreach (FObject fobject in this.buttons)
			{
				if (fobject.Data.ButtonComponent == ButtonComponent.DAButton)
				{
					this.DAButtonDrawer.SetupDAButton(fobject.Data);
				}
				else
				{
					this.UnityButtonDrawer.SetupUnityButton(fobject.Data);
				}
				await Task.Yield();
			}
			List<FObject>.Enumerator enumerator = default(List<FObject>.Enumerator);
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x0600183B RID: 6203 RVA: 0x00099F73 File Offset: 0x00098173
		[SerializeProperty("dabDrawer")]
		public DAButtonDrawer DAButtonDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.dabDrawer);
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x0600183C RID: 6204 RVA: 0x00099F86 File Offset: 0x00098186
		[SerializeProperty("ubDrawer")]
		public UnityButtonDrawer UnityButtonDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.ubDrawer);
			}
		}

		// Token: 0x04001983 RID: 6531
		[SerializeField]
		private List<FObject> buttons = new List<FObject>();

		// Token: 0x04001984 RID: 6532
		[SerializeField]
		private DAButtonDrawer dabDrawer;

		// Token: 0x04001985 RID: 6533
		[SerializeField]
		private UnityButtonDrawer ubDrawer;
	}
}
