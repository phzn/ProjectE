using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Drawers.CanvasDrawers;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Logging;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers
{
	// Token: 0x02000390 RID: 912
	[Serializable]
	public class CanvasDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x060017EE RID: 6126 RVA: 0x00097ED0 File Offset: 0x000960D0
		public async Task DrawToCanvas(List<FObject> fobjects)
		{
			this.monoBeh.AssetTools.SelectFcu();
			this.TextDrawer.Init();
			this.ButtonDrawer.Init();
			this.ToggleDrawer.Init();
			this.InputFieldDrawer.Init();
			this.ScrollViewDrawer.Init();
			this.LocalizationDrawer.Init();
			await this.DrawComponents(fobjects, new DrawByTag(this.DrawByTag));
			await this.ButtonDrawer.SetTargetGraphics();
			await this.ToggleDrawer.SetTargetGraphics();
			await this.InputFieldDrawer.SetTargetGraphics();
			await this.ScrollViewDrawer.SetTargetGraphics();
			this.LocalizationDrawer.SaveAndConnectTable();
			await this.FixAutolayoutMargins(fobjects);
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x00097F1C File Offset: 0x0009611C
		private async Task DrawByTag(FObject fobject, FcuTag tag, Action onDraw)
		{
			try
			{
				if (fobject.Data.GameObject == null)
				{
					return;
				}
				if (tag <= FcuTag.Text)
				{
					switch (tag)
					{
					case FcuTag.AutoLayoutGroup:
						this.AutoLayoutDrawer.Draw(fobject);
						goto IL_01BC;
					case FcuTag.ContentSizeFitter:
						this.ContentSizeFitterDrawer.Draw(fobject);
						goto IL_01BC;
					case FcuTag.AspectRatioFitter:
						this.AspectRatioFitterDrawer.Draw(fobject);
						goto IL_01BC;
					default:
						if (tag != FcuTag.Text)
						{
							goto IL_01BC;
						}
						break;
					}
				}
				else
				{
					if (tag == FcuTag.Image)
					{
						this.ImageDrawer.Draw(fobject, null);
						goto IL_01BC;
					}
					switch (tag)
					{
					case FcuTag.Button:
						this.ButtonDrawer.Draw(fobject);
						goto IL_01BC;
					case FcuTag.InputField:
					case FcuTag.PasswordField:
						this.InputFieldDrawer.Draw(fobject);
						goto IL_01BC;
					case FcuTag.Placeholder:
						break;
					case FcuTag.ScrollView:
						this.ScrollViewDrawer.Draw(fobject);
						goto IL_01BC;
					case FcuTag.Toggle:
					case FcuTag.ToggleGroup:
						this.ToggleDrawer.Draw(fobject);
						goto IL_01BC;
					default:
						switch (tag)
						{
						case FcuTag.Shadow:
							this.ShadowDrawer.Draw(fobject);
							goto IL_01BC;
						case FcuTag.CanvasGroup:
							this.CanvasGroupDrawer.Draw(fobject);
							goto IL_01BC;
						case FcuTag.Mask:
							this.MaskDrawer.Draw(fobject);
							goto IL_01BC;
						default:
							goto IL_01BC;
						}
						break;
					}
				}
				this.TextDrawer.Draw(fobject);
				this.LocalizationDrawer.Draw(fobject);
				IL_01BC:;
			}
			catch (Exception ex)
			{
				DALogger.LogError(FcuLocKey.log_cant_draw_object.Localize(new object[] { fobject.Data.NameHierarchy }));
				Debug.LogException(ex);
			}
			onDraw();
			await Task.Yield();
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x00097F78 File Offset: 0x00096178
		public async Task FixSpriteRenderers(List<FObject> fobjects)
		{
			CanvasDrawer.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			List<Transform> topLevelChilds = this.monoBeh.transform.GetTopLevelChilds();
			CS$<>8__locals1.maxOrder = 32767;
			foreach (Transform transform in topLevelChilds)
			{
				int num = 0;
				this.<FixSpriteRenderers>g__SetOrderInLayerRecursively|2_0(transform, ref num, ref CS$<>8__locals1);
			}
			foreach (FObject fobject in fobjects)
			{
				SpriteRenderer spriteRenderer;
				if (!(fobject.Data.GameObject == null) && fobject.ContainsTag(FcuTag.Image) && fobject.Data.GameObject.TryGetComponentSafe<SpriteRenderer>(out spriteRenderer))
				{
					fobject.Data.GameObject.SetActive(false);
					await Task.Delay(10);
					fobject.Data.GameObject.SetActive(true);
					fobject = default(FObject);
				}
			}
			List<FObject>.Enumerator enumerator2 = default(List<FObject>.Enumerator);
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x00097FC4 File Offset: 0x000961C4
		public async Task FixDttImages(List<FObject> fobjects)
		{
			foreach (FObject fobject in fobjects)
			{
				if (!(fobject.Data.GameObject == null))
				{
					fobject.ContainsTag(FcuTag.Image);
				}
			}
			this.monoBeh.AssetTools.SelectFcu();
			SceneManager.GetActiveScene().SetExpanded(false);
			await Task.Yield();
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x00098010 File Offset: 0x00096210
		private async Task FixAutolayoutMargins(List<FObject> fobjects)
		{
			foreach (FObject fobject in fobjects)
			{
				FObject fobject2;
				HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup;
				if (!(fobject.Data.GameObject == null) && this.monoBeh.CurrentProject.TryGetByIndex(fobject.Data.ParentIndex, out fobject2) && fobject2.IsInsideAutoLayout(out horizontalOrVerticalLayoutGroup))
				{
					int left = horizontalOrVerticalLayoutGroup.padding.left;
					int right = horizontalOrVerticalLayoutGroup.padding.right;
					float x = fobject.Size.x;
					Vector2 size = fobject.Size;
					float x2 = fobject2.Data.FRect.size.x;
					if ((float)(left + right) + x > x2)
					{
						float num = (float)(left + right) + x - x2;
						float num2 = (float)(left + right);
						float num3 = (float)left / num2;
						float num4 = (float)right / num2;
						int num5 = left - (int)Math.Floor((double)(num * num3));
						int num6 = right - (int)Math.Floor((double)(num * num4));
						if (num5 > 0 && num6 > 0)
						{
							horizontalOrVerticalLayoutGroup.padding.left = num5;
							horizontalOrVerticalLayoutGroup.padding.right = num6;
						}
					}
				}
			}
			await Task.Yield();
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x0009805C File Offset: 0x0009625C
		private async Task FixTextSizes(List<FObject> fobjects)
		{
			foreach (FObject fobject in fobjects)
			{
				if (fobject.ContainsTag(FcuTag.Text) && !(fobject.Data.GameObject == null))
				{
					RectTransform component = fobject.Data.GameObject.GetComponent<RectTransform>();
					Vector2 vector = new Vector2(component.rect.width, component.rect.height);
					Vector2 vector2 = new Vector2((fobject.Size.x - vector.x) / 2f, (fobject.Size.y - vector.y) / 2f);
					Vector4 vector3 = default(Vector4);
					if (fobject.Size.y > vector.y && fobject.Size.x > vector.x)
					{
						vector3 = new Vector4(vector2.x, vector2.y, vector2.x, vector2.y);
					}
					else if (fobject.Size.y > vector.y)
					{
						vector3 = new Vector4(0f, vector2.y, 0f, vector2.y);
					}
					else if (fobject.Size.x > vector.x)
					{
						vector3 = new Vector4(vector2.x, 0f, vector2.x, 0f);
					}
					TMP_Text tmp_Text;
					if (fobject.Data.GameObject.TryGetComponentSafe<TMP_Text>(out tmp_Text))
					{
						tmp_Text.margin = vector3;
					}
				}
			}
			await Task.Yield();
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x000980A0 File Offset: 0x000962A0
		public async Task DrawComponents(List<FObject> fobjects, DrawByTag drawByTag)
		{
			CanvasDrawer.<>c__DisplayClass6_0 CS$<>8__locals1 = new CanvasDrawer.<>c__DisplayClass6_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.drawByTag = drawByTag;
			Array values = Enum.GetValues(typeof(FcuTag));
			using (IEnumerator enumerator = values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CanvasDrawer.<>c__DisplayClass6_1 CS$<>8__locals2 = new CanvasDrawer.<>c__DisplayClass6_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.tag = (FcuTag)enumerator.Current;
					if (CS$<>8__locals2.tag.GetTagConfig().HasComponent)
					{
						CS$<>8__locals2.drawnObjectsCount = 0;
						int objectsToDrawCount = this.monoBeh.TagSetter.TagsCounter[CS$<>8__locals2.tag];
						if (objectsToDrawCount >= 1)
						{
							DACycles.ForEach<FObject>(fobjects, delegate(FObject fobject)
							{
								if (!fobject.ContainsTag(CS$<>8__locals2.tag))
								{
									return;
								}
								Action action = delegate
								{
									int drawnObjectsCount = CS$<>8__locals2.drawnObjectsCount;
									CS$<>8__locals2.drawnObjectsCount = drawnObjectsCount + 1;
									UnityEvent<FigmaConverterUnity, FObject, FcuTag> onAddComponent = CS$<>8__locals2.CS$<>8__locals1.<>4__this.monoBeh.Events.OnAddComponent;
									if (onAddComponent == null)
									{
										return;
									}
									onAddComponent.Invoke(CS$<>8__locals2.CS$<>8__locals1.<>4__this.monoBeh, fobject, CS$<>8__locals2.tag);
								};
								CS$<>8__locals2.CS$<>8__locals1.drawByTag(fobject, CS$<>8__locals2.tag, action);
							}, 0.01f, 150);
							int tempCount = -1;
							while (FcuLogger.WriteLogBeforeEqual(ref CS$<>8__locals2.drawnObjectsCount, ref objectsToDrawCount, FcuLocKey.log_drawn_count.Localize(new object[]
							{
								string.Format("{0}", CS$<>8__locals2.tag),
								CS$<>8__locals2.drawnObjectsCount,
								objectsToDrawCount
							}), ref tempCount))
							{
								await Task.Delay(1000);
							}
							CS$<>8__locals2 = null;
						}
					}
				}
			}
			IEnumerator enumerator = null;
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x000980F4 File Offset: 0x000962F4
		public void AddCanvasComponent()
		{
			Canvas canvas;
			this.monoBeh.gameObject.TryAddComponent<Canvas>(out canvas, false);
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			CanvasScaler canvasScaler;
			if (this.monoBeh.gameObject.TryGetComponentSafe<CanvasScaler>(out canvasScaler))
			{
				canvasScaler.enabled = false;
			}
			GraphicRaycaster graphicRaycaster;
			this.monoBeh.gameObject.TryAddComponent<GraphicRaycaster>(out graphicRaycaster, false);
			if (!MonoBehExtensions.IsExistsOnScene<EventSystem>())
			{
				GameObject gameObject = MonoBehExtensions.CreateEmptyGameObject(null, null);
				gameObject.AddComponent<EventSystem>();
				gameObject.AddComponent<StandaloneInputModule>();
				gameObject.name = "EventSystem";
			}
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x00098174 File Offset: 0x00096374
		public async Task FixJoshPui()
		{
			List<Transform> topLevelChilds = this.monoBeh.transform.GetTopLevelChilds();
			foreach (Transform frame in topLevelChilds)
			{
				frame.gameObject.SetActive(false);
				await Task.Delay(100);
				frame.gameObject.SetActive(true);
				frame = null;
			}
			List<Transform>.Enumerator enumerator = default(List<Transform>.Enumerator);
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x060017F7 RID: 6135 RVA: 0x000981B7 File Offset: 0x000963B7
		[SerializeProperty("imageDrawer")]
		public ImageDrawer ImageDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.imageDrawer);
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x060017F8 RID: 6136 RVA: 0x000981CA File Offset: 0x000963CA
		[SerializeProperty("textDrawer")]
		public TextDrawer TextDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.textDrawer);
			}
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x060017F9 RID: 6137 RVA: 0x000981DD File Offset: 0x000963DD
		[SerializeProperty("autoLayoutDrawer")]
		public AutoLayoutDrawer AutoLayoutDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.autoLayoutDrawer);
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x060017FA RID: 6138 RVA: 0x000981F0 File Offset: 0x000963F0
		[SerializeProperty("contentSizeFitterDrawer")]
		public ContentSizeFitterDrawer ContentSizeFitterDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.contentSizeFitterDrawer);
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x060017FB RID: 6139 RVA: 0x00098203 File Offset: 0x00096403
		[SerializeProperty("aspectRatioFitterDrawer")]
		public AspectRatioFitterDrawer AspectRatioFitterDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.aspectRatioFitterDrawer);
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x060017FC RID: 6140 RVA: 0x00098216 File Offset: 0x00096416
		[SerializeProperty("maskDrawer")]
		public MaskDrawer MaskDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.maskDrawer);
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x060017FD RID: 6141 RVA: 0x00098229 File Offset: 0x00096429
		[SerializeProperty("toggleDrawer")]
		public ToggleDrawer ToggleDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.toggleDrawer);
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x060017FE RID: 6142 RVA: 0x0009823C File Offset: 0x0009643C
		[SerializeProperty("buttonDrawer")]
		public ButtonDrawer ButtonDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.buttonDrawer);
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x060017FF RID: 6143 RVA: 0x0009824F File Offset: 0x0009644F
		[SerializeProperty("scriptGenerator")]
		public ScriptGenerator ScriptGenerator
		{
			get
			{
				return this.monoBeh.Link(ref this.scriptGenerator);
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001800 RID: 6144 RVA: 0x00098262 File Offset: 0x00096462
		[SerializeProperty("inputFieldDrawer")]
		public InputFieldDrawer InputFieldDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.inputFieldDrawer);
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06001801 RID: 6145 RVA: 0x00098275 File Offset: 0x00096475
		[SerializeProperty("scrollViewDrawer")]
		public ScrollViewDrawer ScrollViewDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.scrollViewDrawer);
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001802 RID: 6146 RVA: 0x00098288 File Offset: 0x00096488
		[SerializeProperty("localizationDrawer")]
		public LocalizationDrawer LocalizationDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.localizationDrawer);
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001803 RID: 6147 RVA: 0x0009829B File Offset: 0x0009649B
		[SerializeProperty("shadowDrawer")]
		public ShadowDrawer ShadowDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.shadowDrawer);
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001804 RID: 6148 RVA: 0x000982AE File Offset: 0x000964AE
		[SerializeProperty("canvasGroupDrawer")]
		public CanvasGroupDrawer CanvasGroupDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.canvasGroupDrawer);
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001805 RID: 6149 RVA: 0x000982C1 File Offset: 0x000964C1
		[SerializeProperty("gameObjectDrawer")]
		public GameObjectDrawer GameObjectDrawer
		{
			get
			{
				return this.monoBeh.Link(ref this.gameObjectDrawer);
			}
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x000982D4 File Offset: 0x000964D4
		[CompilerGenerated]
		private void <FixSpriteRenderers>g__SetOrderInLayerRecursively|2_0(Transform trans, ref int order, ref CanvasDrawer.<>c__DisplayClass2_0 A_3)
		{
			SpriteRenderer component = trans.GetComponent<SpriteRenderer>();
			if (component != null)
			{
				component.sortingOrder = order;
				order += this.monoBeh.Settings.SpriteRendererSettings.NextOrderStep;
				if (order > A_3.maxOrder)
				{
					order = A_3.maxOrder;
				}
			}
			for (int i = 0; i < trans.childCount; i++)
			{
				this.<FixSpriteRenderers>g__SetOrderInLayerRecursively|2_0(trans.GetChild(i), ref order, ref A_3);
			}
		}

		// Token: 0x04001933 RID: 6451
		[SerializeField]
		private ImageDrawer imageDrawer;

		// Token: 0x04001934 RID: 6452
		[SerializeField]
		private TextDrawer textDrawer;

		// Token: 0x04001935 RID: 6453
		[SerializeField]
		private AutoLayoutDrawer autoLayoutDrawer;

		// Token: 0x04001936 RID: 6454
		[SerializeField]
		private ContentSizeFitterDrawer contentSizeFitterDrawer;

		// Token: 0x04001937 RID: 6455
		[SerializeField]
		private AspectRatioFitterDrawer aspectRatioFitterDrawer;

		// Token: 0x04001938 RID: 6456
		[SerializeField]
		private MaskDrawer maskDrawer;

		// Token: 0x04001939 RID: 6457
		[SerializeField]
		private ToggleDrawer toggleDrawer;

		// Token: 0x0400193A RID: 6458
		[SerializeField]
		private ButtonDrawer buttonDrawer;

		// Token: 0x0400193B RID: 6459
		[SerializeField]
		private ScriptGenerator scriptGenerator;

		// Token: 0x0400193C RID: 6460
		[SerializeField]
		private InputFieldDrawer inputFieldDrawer;

		// Token: 0x0400193D RID: 6461
		[SerializeField]
		private ScrollViewDrawer scrollViewDrawer;

		// Token: 0x0400193E RID: 6462
		[SerializeField]
		private LocalizationDrawer localizationDrawer;

		// Token: 0x0400193F RID: 6463
		[SerializeField]
		private ShadowDrawer shadowDrawer;

		// Token: 0x04001940 RID: 6464
		[SerializeField]
		private CanvasGroupDrawer canvasGroupDrawer;

		// Token: 0x04001941 RID: 6465
		[SerializeField]
		private GameObjectDrawer gameObjectDrawer;
	}
}
