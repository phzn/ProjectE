using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DA_Assets.DAI;
using DA_Assets.Extensions;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
	// Token: 0x020003B7 RID: 951
	[Serializable]
	public class InputFieldDrawer : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x0600187D RID: 6269 RVA: 0x0009C229 File Offset: 0x0009A429
		public List<FObject> InputFields
		{
			get
			{
				return this.inputFields;
			}
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x0009C231 File Offset: 0x0009A431
		public void Init()
		{
			this.inputFields = new List<FObject>();
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x0009C240 File Offset: 0x0009A440
		public void Draw(FObject fobject)
		{
			TextComponent textComponent = this.monoBeh.Settings.TextFontsSettings.TextComponent;
			if (textComponent != TextComponent.UnityText)
			{
				if (textComponent == TextComponent.TextMeshPro)
				{
					TMP_InputField tmp_InputField;
					fobject.Data.GameObject.TryAddComponent<TMP_InputField>(out tmp_InputField, false);
				}
			}
			else
			{
				InputField inputField;
				fobject.Data.GameObject.TryAddComponent<InputField>(out inputField, false);
			}
			this.inputFields.Add(fobject);
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x0009C2A4 File Offset: 0x0009A4A4
		public async Task SetTargetGraphics()
		{
			TextComponent textComponent = this.monoBeh.Settings.TextFontsSettings.TextComponent;
			if (textComponent != TextComponent.UnityText)
			{
				if (textComponent == TextComponent.TextMeshPro)
				{
					await this.SetTargetGraphicsTmpInputFields();
				}
			}
			else
			{
				await this.SetTargetGraphicsInputFields();
			}
			this.inputFields.Clear();
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x0009C2E8 File Offset: 0x0009A4E8
		private async Task SetTargetGraphicsInputFields()
		{
			foreach (FObject fobject in this.inputFields)
			{
				InputField inputField;
				if (fobject.Data.GameObject.TryGetComponentSafe<InputField>(out inputField))
				{
					InputFieldModel graphics = this.GetGraphics(fobject.Data);
					RectTransform rectTransform;
					if (graphics.TextArea.TryGetComponentSafe<RectTransform>(out rectTransform))
					{
						rectTransform.SetSmartAnchor(AnchorType.StretchAll);
					}
					Graphic graphic;
					if (graphics.Background.TryGetComponentSafe<Graphic>(out graphic))
					{
						inputField.targetGraphic = graphic;
					}
					Graphic graphic2;
					if (graphics.Placeholder.TryGetComponentSafe<Graphic>(out graphic2))
					{
						graphic2.gameObject.SetActive(true);
						inputField.placeholder = graphic2;
					}
					Text text;
					if (graphics.TextComponent.TryGetComponentSafe<Text>(out text))
					{
						text.gameObject.SetActive(true);
						inputField.textComponent = text;
						inputField.textComponent.supportRichText = false;
						text.resizeTextForBestFit = false;
					}
					if (fobject.ContainsTag(FcuTag.PasswordField))
					{
						inputField.contentType = InputField.ContentType.Password;
						inputField.asteriskChar = FcuConfig.AsterisksChar;
					}
					else
					{
						inputField.contentType = InputField.ContentType.Standard;
					}
					inputField.enabled = false;
					await Task.Delay(10);
					inputField.enabled = true;
					inputField = null;
				}
			}
			List<FObject>.Enumerator enumerator = default(List<FObject>.Enumerator);
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x0009C32C File Offset: 0x0009A52C
		private async Task SetTargetGraphicsTmpInputFields()
		{
			foreach (FObject fobject in this.inputFields)
			{
				TMP_InputField inputField;
				if (fobject.Data.GameObject.TryGetComponentSafe<TMP_InputField>(out inputField))
				{
					InputFieldModel graphics = this.GetGraphics(fobject.Data);
					RectTransform rectTransform;
					if (graphics.TextArea.TryGetComponentSafe<RectTransform>(out rectTransform))
					{
						rectTransform.SetSmartAnchor(AnchorType.StretchAll);
						inputField.textViewport = rectTransform;
						RectMask2D rectMask2D;
						rectTransform.gameObject.TryAddComponent<RectMask2D>(out rectMask2D, false);
					}
					Graphic graphic;
					if (graphics.Background.TryGetComponentSafe<Graphic>(out graphic))
					{
						inputField.targetGraphic = graphic;
					}
					Graphic graphic2;
					if (graphics.Placeholder.TryGetComponentSafe<Graphic>(out graphic2))
					{
						graphic2.gameObject.SetActive(true);
						inputField.placeholder = graphic2;
					}
					TMP_Text tmp_Text;
					if (graphics.TextComponent.TryGetComponentSafe<TMP_Text>(out tmp_Text))
					{
						tmp_Text.gameObject.SetActive(true);
						inputField.textComponent = tmp_Text;
						tmp_Text.enableAutoSizing = false;
					}
					if (fobject.ContainsTag(FcuTag.PasswordField))
					{
						inputField.contentType = TMP_InputField.ContentType.Password;
						inputField.asteriskChar = FcuConfig.AsterisksChar;
					}
					else
					{
						inputField.contentType = TMP_InputField.ContentType.Standard;
					}
					inputField.enabled = false;
					await Task.Delay(10);
					inputField.enabled = true;
					inputField = null;
				}
			}
			List<FObject>.Enumerator enumerator = default(List<FObject>.Enumerator);
			await Task.Yield();
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x0009C370 File Offset: 0x0009A570
		private InputFieldModel GetGraphics(SyncData syncData)
		{
			SyncHelper[] childs = syncData.GameObject.GetChilds<SyncHelper>();
			InputFieldModel inputFieldModel = default(InputFieldModel);
			foreach (SyncHelper syncHelper in childs)
			{
				if (inputFieldModel.Background == null && syncHelper.ContainsTag(FcuTag.Background))
				{
					inputFieldModel.Background = syncHelper.gameObject;
					break;
				}
			}
			foreach (SyncHelper syncHelper2 in childs)
			{
				if (inputFieldModel.TextArea == null && syncHelper2.name.IsInputTextArea())
				{
					inputFieldModel.TextArea = syncHelper2.gameObject;
				}
				else if (inputFieldModel.TextComponent == null && syncHelper2.ContainsTag(FcuTag.Text) && !syncHelper2.ContainsTag(FcuTag.Placeholder))
				{
					inputFieldModel.TextComponent = syncHelper2.gameObject;
				}
				else if (inputFieldModel.Placeholder == null && syncHelper2.ContainsTag(FcuTag.Placeholder))
				{
					inputFieldModel.Placeholder = syncHelper2.gameObject;
				}
				else if (inputFieldModel.Background == null && syncHelper2.ContainsTag(FcuTag.Image))
				{
					inputFieldModel.Background = syncHelper2.gameObject;
				}
			}
			return inputFieldModel;
		}

		// Token: 0x040019A9 RID: 6569
		private List<FObject> inputFields;
	}
}
