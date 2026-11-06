using System;
using System.Collections.Generic;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003C5 RID: 965
	[Serializable]
	public class TextDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x060018C3 RID: 6339 RVA: 0x0009D6C7 File Offset: 0x0009B8C7
		public List<FObject> Texts
		{
			get
			{
				return this.texts;
			}
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x0009D6CF File Offset: 0x0009B8CF
		public void Init()
		{
			this.texts = new List<FObject>();
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x0009D6DC File Offset: 0x0009B8DC
		public void Draw(FObject fobject)
		{
			Graphic graphic;
			if (!fobject.Data.GameObject.IsPartOfAnyPrefab() && fobject.Data.GameObject.TryGetComponentSafe<Graphic>(out graphic))
			{
				Type currentTextType = this.monoBeh.GetCurrentTextType();
				if (!graphic.GetType().Equals(currentTextType))
				{
					graphic.Destroy();
				}
			}
			if (this.monoBeh.IsNova())
			{
				this.TextMeshDrawer.DrawNovaTMP(fobject);
			}
			else
			{
				switch (this.monoBeh.Settings.TextFontsSettings.TextComponent)
				{
				case TextComponent.UnityText:
					this.UnityTextDrawer.Draw(fobject);
					break;
				case TextComponent.TextMeshPro:
					this.TextMeshDrawer.DrawTMP(fobject);
					break;
				case TextComponent.RTLTextMeshPro:
					this.TextMeshDrawer.DrawRTL(fobject);
					break;
				}
			}
			this.texts.Add(fobject);
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x060018C6 RID: 6342 RVA: 0x0009D7AC File Offset: 0x0009B9AC
		[SerializeProperty("unityTextDrawer")]
		public UnityTextDrawer UnityTextDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.unityTextDrawer);
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x060018C7 RID: 6343 RVA: 0x0009D7BF File Offset: 0x0009B9BF
		[SerializeProperty("textMeshDrawer")]
		public TextMeshDrawer TextMeshDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.textMeshDrawer);
			}
		}

		// Token: 0x040019D5 RID: 6613
		[SerializeField]
		private List<FObject> texts;

		// Token: 0x040019D6 RID: 6614
		[SerializeField]
		private UnityTextDrawer unityTextDrawer;

		// Token: 0x040019D7 RID: 6615
		[SerializeField]
		private TextMeshDrawer textMeshDrawer;
	}
}
