using System;
using System.Collections.Generic;
using Features.Detection;
using Features.Localization;
using Features.Rank;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E6D RID: 3693
	[DisallowMultipleComponent]
	public class SelfiePosePicker : MonoBehaviour
	{
		// Token: 0x1700120D RID: 4621
		// (get) Token: 0x06006E31 RID: 28209 RVA: 0x002C4E5C File Offset: 0x002C305C
		public bool MenuVisible
		{
			get
			{
				return this._menuVisible;
			}
		}

		// Token: 0x1700120E RID: 4622
		// (get) Token: 0x06006E32 RID: 28210 RVA: 0x002C4E64 File Offset: 0x002C3064
		public int CurrentPoseIndex
		{
			get
			{
				return this._currentPoseIndex;
			}
		}

		// Token: 0x1700120F RID: 4623
		// (get) Token: 0x06006E33 RID: 28211 RVA: 0x002C4E6C File Offset: 0x002C306C
		public bool BodyPosesAllowed
		{
			get
			{
				return this._canUseBodyPoses == null || this._canUseBodyPoses();
			}
		}

		// Token: 0x06006E34 RID: 28212 RVA: 0x002C4E83 File Offset: 0x002C3083
		public void ConfigureBodyPoseAvailability(Func<bool> canUseBodyPoses)
		{
			this._canUseBodyPoses = canUseBodyPoses;
			this.RefreshBodyPoseAvailability();
		}

		// Token: 0x06006E35 RID: 28213 RVA: 0x002C4E94 File Offset: 0x002C3094
		public void RefreshBodyPoseAvailability()
		{
			bool bodyPosesAllowed = this.BodyPosesAllowed;
			if (this._bodyPosesAllowed == bodyPosesAllowed)
			{
				return;
			}
			this._bodyPosesAllowed = bodyPosesAllowed;
			if (!bodyPosesAllowed)
			{
				if (this._currentPoseIndex != 0 || this.HasPoseLayerInfluence)
				{
					this.SetPoseIndex(0, null);
				}
				this._poseLayerTargetWeight = 0f;
				if (this._animator != null && this._poseLayerIndex >= 0)
				{
					this._animator.SetLayerWeight(this._poseLayerIndex, 0f);
				}
				this.RestorePoseBlendShapes(true);
			}
			if (this._poseDropdown != null)
			{
				this._poseDropdown.Hide();
				this.RefreshLegacyPoseOptions();
				this._poseDropdown.interactable = bodyPosesAllowed;
			}
		}

		// Token: 0x17001210 RID: 4624
		// (get) Token: 0x06006E36 RID: 28214 RVA: 0x002C4F3E File Offset: 0x002C313E
		public bool HasPoseLayerInfluence
		{
			get
			{
				return this._currentPoseIndex != 0 || (this._animator != null && this._poseLayerIndex >= 0 && this._animator.GetLayerWeight(this._poseLayerIndex) > 0f);
			}
		}

		// Token: 0x17001211 RID: 4625
		// (get) Token: 0x06006E37 RID: 28215 RVA: 0x002C4F7B File Offset: 0x002C317B
		public bool CurrentPoseHidesFace
		{
			get
			{
				return this._currentPoseEntry != null && this._currentPoseEntry.hidesFace;
			}
		}

		// Token: 0x06006E38 RID: 28216 RVA: 0x002C4F94 File Offset: 0x002C3194
		public void Initialize(Animator animator, SelfiePoseConfigSO config, Action<bool> onMenuVisibilityChanged, Action<SelfiePoseConfigSO.PoseEntry> onPoseChanged, PlayerEmotionController playerEmotion)
		{
			this._animator = animator;
			this._config = config;
			this._onMenuVisibilityChanged = onMenuVisibilityChanged;
			this._onPoseChanged = onPoseChanged;
			this._playerEmotion = playerEmotion;
			this._poseLayerIndex = ((this._animator != null) ? this._animator.GetLayerIndex("SelfiePose") : (-1));
			if (this._poseLayerIndex < 0)
			{
				Debug.LogWarning("[SelfiePosePicker] Layer 'SelfiePose' non trovato sull'Animator. Esegui Tools > Phone Companion > Setup Selfie Pose Layer.", this);
			}
		}

		// Token: 0x06006E39 RID: 28217 RVA: 0x002C5004 File Offset: 0x002C3204
		public void SetActive(bool active)
		{
			if (active)
			{
				this.RefreshBodyPoseAvailability();
				if (this.buildLegacyDropdownMenu)
				{
					if (this._menuRoot == null)
					{
						this.BuildMenu();
					}
					this.SetMenuVisible(false);
					if (this._canvas != null)
					{
						this._canvas.gameObject.SetActive(true);
						return;
					}
				}
			}
			else
			{
				this.SetMenuVisible(false);
				if (this._canvas != null)
				{
					this._canvas.gameObject.SetActive(false);
				}
				this.SetPoseIndex(0, null);
				if (this._animator != null && this._poseLayerIndex >= 0)
				{
					this._animator.SetLayerWeight(this._poseLayerIndex, 0f);
				}
				this.RestorePoseBlendShapes(true);
				this.SetFaceIndex(0);
			}
		}

		// Token: 0x06006E3A RID: 28218 RVA: 0x002C50CC File Offset: 0x002C32CC
		private void Update()
		{
			this.RefreshBodyPoseAvailability();
			if (this._currentPoseEntry != null && !this._currentPoseEntry.IsAvailable(RankProgressionService.CurrentRank))
			{
				this.SetPoseIndex(0, null);
			}
			this.TickPoseLayerWeight();
			if (!this.buildLegacyDropdownMenu)
			{
				return;
			}
			if (this._canvas == null || !this._canvas.gameObject.activeSelf)
			{
				return;
			}
			if (GameplayInput.GetKeyDown(this.toggleMenuKey))
			{
				this.SetMenuVisible(!this._menuVisible);
			}
		}

		// Token: 0x06006E3B RID: 28219 RVA: 0x002C5150 File Offset: 0x002C3350
		private void SetMenuVisible(bool visible)
		{
			if (this._menuVisible == visible)
			{
				return;
			}
			if (visible && this._poseDropdown != null)
			{
				this.RefreshLegacyPoseOptions();
			}
			this._menuVisible = visible;
			if (this._menuRoot != null)
			{
				this._menuRoot.SetActive(visible);
			}
			Action<bool> onMenuVisibilityChanged = this._onMenuVisibilityChanged;
			if (onMenuVisibilityChanged == null)
			{
				return;
			}
			onMenuVisibilityChanged(visible);
		}

		// Token: 0x06006E3C RID: 28220 RVA: 0x002C51B0 File Offset: 0x002C33B0
		private void SetPoseIndex(int index, SelfiePoseConfigSO.PoseEntry entry = null)
		{
			this.CapturePoseBlendShapes(entry);
			this._currentPoseIndex = index;
			this._currentPoseEntry = entry;
			this.RestorePoseBlendShapes(false);
			Action<SelfiePoseConfigSO.PoseEntry> onPoseChanged = this._onPoseChanged;
			if (onPoseChanged != null)
			{
				onPoseChanged(entry);
			}
			if (this._animator != null && this._poseLayerIndex >= 0)
			{
				this._animator.SetInteger("PoseIndex", index);
				this._poseLayerTargetWeight = ((index > 0) ? 1f : 0f);
			}
		}

		// Token: 0x06006E3D RID: 28221 RVA: 0x002C522C File Offset: 0x002C342C
		private void CapturePoseBlendShapes(SelfiePoseConfigSO.PoseEntry entry)
		{
			foreach (SelfiePosePicker.PoseBlendShapeState poseBlendShapeState in this._poseBlendShapeStates)
			{
				poseBlendShapeState.ownedByCurrentPose = false;
			}
			if (this._animator == null || entry == null || entry.animatedBlendShapes == null)
			{
				return;
			}
			foreach (SelfiePoseConfigSO.PoseBlendShapeBinding poseBlendShapeBinding in entry.animatedBlendShapes)
			{
				if (poseBlendShapeBinding != null && !string.IsNullOrEmpty(poseBlendShapeBinding.blendShapeName))
				{
					Transform transform = (string.IsNullOrEmpty(poseBlendShapeBinding.rendererPath) ? this._animator.transform : this._animator.transform.Find(poseBlendShapeBinding.rendererPath));
					SkinnedMeshRenderer renderer = ((transform != null) ? transform.GetComponent<SkinnedMeshRenderer>() : null);
					Mesh mesh = ((renderer != null) ? renderer.sharedMesh : null);
					int index = ((mesh != null) ? mesh.GetBlendShapeIndex(poseBlendShapeBinding.blendShapeName) : (-1));
					if (index >= 0)
					{
						SelfiePosePicker.PoseBlendShapeState poseBlendShapeState2 = this._poseBlendShapeStates.Find((SelfiePosePicker.PoseBlendShapeState saved) => saved.renderer == renderer && saved.mesh == mesh && saved.index == index);
						if (poseBlendShapeState2 == null)
						{
							poseBlendShapeState2 = new SelfiePosePicker.PoseBlendShapeState
							{
								renderer = renderer,
								mesh = mesh,
								index = index,
								originalWeight = renderer.GetBlendShapeWeight(index)
							};
							this._poseBlendShapeStates.Add(poseBlendShapeState2);
						}
						poseBlendShapeState2.ownedByCurrentPose = true;
					}
				}
			}
		}

		// Token: 0x06006E3E RID: 28222 RVA: 0x002C5420 File Offset: 0x002C3620
		private void RestorePoseBlendShapes(bool release)
		{
			foreach (SelfiePosePicker.PoseBlendShapeState poseBlendShapeState in this._poseBlendShapeStates)
			{
				if ((release || !poseBlendShapeState.ownedByCurrentPose) && poseBlendShapeState.renderer != null && poseBlendShapeState.renderer.sharedMesh == poseBlendShapeState.mesh)
				{
					poseBlendShapeState.renderer.SetBlendShapeWeight(poseBlendShapeState.index, poseBlendShapeState.originalWeight);
				}
			}
			if (release)
			{
				this._poseBlendShapeStates.Clear();
			}
		}

		// Token: 0x06006E3F RID: 28223 RVA: 0x002C54C4 File Offset: 0x002C36C4
		private void LateUpdate()
		{
			this.RestorePoseBlendShapes(!this.HasPoseLayerInfluence);
		}

		// Token: 0x06006E40 RID: 28224 RVA: 0x002C54D8 File Offset: 0x002C36D8
		private void SetFaceIndex(int index)
		{
			this._currentFaceIndex = index;
			bool flag = index != 0;
			if (flag && !this._faceOverrideSuspended)
			{
				if (this._playerEmotion != null)
				{
					this._playerEmotion.DisableFacialExpressionForAction();
				}
				this._faceOverrideSuspended = true;
			}
			else if (!flag && this._faceOverrideSuspended)
			{
				if (this._playerEmotion != null)
				{
					this._playerEmotion.RestoreFacialExpression();
				}
				this._faceOverrideSuspended = false;
				return;
			}
			if (this._animator != null)
			{
				this._animator.SetInteger("FaceExpressionIndex", index);
			}
		}

		// Token: 0x06006E41 RID: 28225 RVA: 0x002C556C File Offset: 0x002C376C
		private void TickPoseLayerWeight()
		{
			if (this._animator == null || this._poseLayerIndex < 0)
			{
				return;
			}
			float num = this._animator.GetLayerWeight(this._poseLayerIndex);
			if (Mathf.Approximately(num, this._poseLayerTargetWeight))
			{
				return;
			}
			num = Mathf.MoveTowards(num, this._poseLayerTargetWeight, this.poseLayerFadeSpeed * Time.deltaTime);
			this._animator.SetLayerWeight(this._poseLayerIndex, num);
		}

		// Token: 0x06006E42 RID: 28226 RVA: 0x002C55E0 File Offset: 0x002C37E0
		private void BuildMenu()
		{
			GameObject gameObject = new GameObject("SelfiePosePickerCanvas", new Type[]
			{
				typeof(Canvas),
				typeof(CanvasScaler),
				typeof(GraphicRaycaster)
			});
			this._canvas = gameObject.GetComponent<Canvas>();
			this._canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this._canvas.sortingOrder = 1050;
			GameplayHudVisibility.RegisterCanvas(this._canvas);
			CanvasScaler component = gameObject.GetComponent<CanvasScaler>();
			component.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component.referenceResolution = new Vector2(1920f, 1080f);
			component.matchWidthOrHeight = 0.5f;
			this._menuRoot = new GameObject("Menu", new Type[] { typeof(RectTransform) });
			RectTransform rectTransform = (RectTransform)this._menuRoot.transform;
			rectTransform.SetParent(this._canvas.transform, false);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.offsetMin = new Vector2(0f, -120f);
			rectTransform.offsetMax = new Vector2(0f, 0f);
			List<string> list = new List<string>();
			if (this._config != null && this._config.poses != null)
			{
				foreach (SelfiePoseConfigSO.PoseEntry poseEntry in this._config.poses)
				{
					list.Add(SelfiePoseConfigSO.PoseLabel(poseEntry));
				}
			}
			List<string> list2 = new List<string>();
			if (this._config != null && this._config.faces != null)
			{
				foreach (SelfiePoseConfigSO.FaceEntry faceEntry in this._config.faces)
				{
					list2.Add(SelfiePoseConfigSO.FaceLabel(faceEntry));
				}
			}
			this._poseDropdown = this.BuildDropdown(rectTransform, "Pose", new Vector2(40f, -30f), list, new UnityAction<int>(this.OnPoseSelected));
			this._faceDropdown = this.BuildDropdown(rectTransform, "Face", new Vector2(360f, -30f), list2, new UnityAction<int>(this.OnFaceSelected));
			this.RefreshLegacyPoseOptions();
			this._poseDropdown.interactable = this.BodyPosesAllowed;
		}

		// Token: 0x06006E43 RID: 28227 RVA: 0x002C588C File Offset: 0x002C3A8C
		private Dropdown BuildDropdown(RectTransform parent, string label, Vector2 pos, List<string> optionLabels, UnityAction<int> onChanged)
		{
			GameObject gameObject = new GameObject(label + "Label", new Type[]
			{
				typeof(RectTransform),
				typeof(Text)
			});
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(parent, false);
			Vector2 vector = new Vector2(0f, 1f);
			rectTransform.anchorMax = vector;
			rectTransform.anchorMin = vector;
			rectTransform.pivot = new Vector2(0f, 1f);
			rectTransform.anchoredPosition = pos + new Vector2(0f, 4f);
			rectTransform.sizeDelta = new Vector2(300f, 24f);
			Text component = gameObject.GetComponent<Text>();
			component.text = label;
			component.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			component.fontSize = 20;
			component.fontStyle = FontStyle.Bold;
			component.color = Color.white;
			LocalizedLegacyGameText.BindSource(component, "phone_story", label).FitTranslationToRectangle();
			GameObject gameObject2 = new GameObject(label + "Dropdown", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Dropdown)
			});
			RectTransform rectTransform2 = (RectTransform)gameObject2.transform;
			rectTransform2.SetParent(parent, false);
			RectTransform rectTransform3 = rectTransform2;
			RectTransform rectTransform4 = rectTransform2;
			vector = new Vector2(0f, 1f);
			rectTransform4.anchorMax = vector;
			rectTransform3.anchorMin = vector;
			rectTransform2.pivot = new Vector2(0f, 1f);
			rectTransform2.anchoredPosition = pos - new Vector2(0f, 26f);
			rectTransform2.sizeDelta = new Vector2(280f, 44f);
			Image component2 = gameObject2.GetComponent<Image>();
			component2.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd");
			component2.type = Image.Type.Sliced;
			component2.color = new Color(0.12f, 0.12f, 0.14f, 0.95f);
			Dropdown component3 = gameObject2.GetComponent<Dropdown>();
			GameObject gameObject3 = new GameObject("Label", new Type[]
			{
				typeof(RectTransform),
				typeof(Text)
			});
			RectTransform rectTransform5 = (RectTransform)gameObject3.transform;
			rectTransform5.SetParent(rectTransform2, false);
			SelfiePosePicker.StretchWithPadding(rectTransform5, 10f, 4f, 28f, 4f);
			Text component4 = gameObject3.GetComponent<Text>();
			component4.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			component4.fontSize = 18;
			component4.color = Color.white;
			component4.alignment = TextAnchor.MiddleLeft;
			LocalizedLegacyGameText.BindFontOnly(component4);
			component3.captionText = component4;
			Text text;
			RectTransform rectTransform6 = this.BuildDropdownTemplate(rectTransform2, out text);
			component3.template = rectTransform6;
			component3.itemText = text;
			List<Dropdown.OptionData> list = new List<Dropdown.OptionData>
			{
				new Dropdown.OptionData(SelfiePosePicker.LocalizedPoseLabel("None"))
			};
			foreach (string text2 in optionLabels)
			{
				list.Add(new Dropdown.OptionData(SelfiePosePicker.LocalizedPoseLabel(text2)));
			}
			component3.options = list;
			component3.value = 0;
			component3.RefreshShownValue();
			component3.onValueChanged.AddListener(onChanged);
			return component3;
		}

		// Token: 0x06006E44 RID: 28228 RVA: 0x002C5BBC File Offset: 0x002C3DBC
		private RectTransform BuildDropdownTemplate(RectTransform ddrt, out Text itemText)
		{
			GameObject gameObject = new GameObject("Template", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(ScrollRect),
				typeof(CanvasGroup)
			});
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(ddrt, false);
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = new Vector2(1f, 0f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			rectTransform.anchoredPosition = new Vector2(0f, 2f);
			rectTransform.sizeDelta = new Vector2(0f, 220f);
			Image component = gameObject.GetComponent<Image>();
			component.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd");
			component.type = Image.Type.Sliced;
			component.color = new Color(0.1f, 0.1f, 0.12f, 0.98f);
			GameObject gameObject2 = new GameObject("Viewport", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Mask)
			});
			RectTransform rectTransform2 = (RectTransform)gameObject2.transform;
			rectTransform2.SetParent(rectTransform, false);
			rectTransform2.anchorMin = Vector2.zero;
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.pivot = new Vector2(0f, 1f);
			rectTransform2.sizeDelta = Vector2.zero;
			gameObject2.GetComponent<Mask>().showMaskGraphic = false;
			gameObject2.GetComponent<Image>().sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd");
			RectTransform rectTransform3 = (RectTransform)new GameObject("Content", new Type[] { typeof(RectTransform) }).transform;
			rectTransform3.SetParent(rectTransform2, false);
			rectTransform3.anchorMin = new Vector2(0f, 1f);
			rectTransform3.anchorMax = new Vector2(1f, 1f);
			rectTransform3.pivot = new Vector2(0.5f, 1f);
			rectTransform3.sizeDelta = new Vector2(0f, 44f);
			GameObject gameObject3 = new GameObject("Item", new Type[]
			{
				typeof(RectTransform),
				typeof(Toggle)
			});
			RectTransform rectTransform4 = (RectTransform)gameObject3.transform;
			rectTransform4.SetParent(rectTransform3, false);
			rectTransform4.anchorMin = new Vector2(0f, 0.5f);
			rectTransform4.anchorMax = new Vector2(1f, 0.5f);
			rectTransform4.pivot = new Vector2(0.5f, 0.5f);
			rectTransform4.sizeDelta = new Vector2(0f, 40f);
			GameObject gameObject4 = new GameObject("Item Background", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			RectTransform rectTransform5 = (RectTransform)gameObject4.transform;
			rectTransform5.SetParent(rectTransform4, false);
			rectTransform5.anchorMin = Vector2.zero;
			rectTransform5.anchorMax = Vector2.one;
			rectTransform5.sizeDelta = Vector2.zero;
			gameObject4.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.06f);
			GameObject gameObject5 = new GameObject("Item Label", new Type[]
			{
				typeof(RectTransform),
				typeof(Text)
			});
			RectTransform rectTransform6 = (RectTransform)gameObject5.transform;
			rectTransform6.SetParent(rectTransform4, false);
			SelfiePosePicker.StretchWithPadding(rectTransform6, 10f, 2f, 10f, 2f);
			itemText = gameObject5.GetComponent<Text>();
			itemText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			itemText.fontSize = 18;
			itemText.color = Color.white;
			itemText.alignment = TextAnchor.MiddleLeft;
			LocalizedLegacyGameText.BindFontOnly(itemText);
			Toggle component2 = gameObject3.GetComponent<Toggle>();
			component2.targetGraphic = gameObject4.GetComponent<Image>();
			component2.isOn = false;
			ScrollRect component3 = gameObject.GetComponent<ScrollRect>();
			component3.content = rectTransform3;
			component3.viewport = rectTransform2;
			component3.horizontal = false;
			component3.movementType = ScrollRect.MovementType.Clamped;
			component3.scrollSensitivity = 20f;
			gameObject.SetActive(false);
			return rectTransform;
		}

		// Token: 0x06006E45 RID: 28229 RVA: 0x002C5FF3 File Offset: 0x002C41F3
		public void SelectPose(int menuIndex)
		{
			this.TrySelectPose(menuIndex);
		}

		// Token: 0x06006E46 RID: 28230 RVA: 0x002C6000 File Offset: 0x002C4200
		public bool IsPoseAvailable(int menuIndex)
		{
			this.RefreshBodyPoseAvailability();
			if (menuIndex == 0)
			{
				return true;
			}
			if (!this.BodyPosesAllowed)
			{
				return false;
			}
			List<SelfiePoseConfigSO.PoseEntry> list = ((this._config != null) ? this._config.poses : null);
			return menuIndex > 0 && list != null && menuIndex <= list.Count && list[menuIndex - 1] != null && list[menuIndex - 1].IsAvailable(RankProgressionService.CurrentRank);
		}

		// Token: 0x06006E47 RID: 28231 RVA: 0x002C6070 File Offset: 0x002C4270
		public List<int> GetAvailablePoseMenuIndices()
		{
			List<int> list = new List<int> { 0 };
			if (this._config != null && this._config.poses != null)
			{
				for (int i = 1; i <= this._config.poses.Count; i++)
				{
					if (this.IsPoseAvailable(i))
					{
						list.Add(i);
					}
				}
			}
			return list;
		}

		// Token: 0x06006E48 RID: 28232 RVA: 0x002C60D4 File Offset: 0x002C42D4
		public bool TrySelectPose(int menuIndex)
		{
			if (!this.IsPoseAvailable(menuIndex))
			{
				return false;
			}
			List<SelfiePoseConfigSO.PoseEntry> list = ((this._config != null) ? this._config.poses : null);
			SelfiePoseConfigSO.PoseEntry poseEntry = ((menuIndex <= 0 || list == null || menuIndex - 1 >= list.Count) ? null : list[menuIndex - 1]);
			this.SetPoseIndex((poseEntry != null) ? poseEntry.poseIndex : 0, poseEntry);
			return true;
		}

		// Token: 0x06006E49 RID: 28233 RVA: 0x002C613C File Offset: 0x002C433C
		private void RefreshLegacyPoseOptions()
		{
			this._legacyPoseIndices = this.GetAvailablePoseMenuIndices();
			List<string> list = new List<string>();
			int num = 0;
			for (int i = 0; i < this._legacyPoseIndices.Count; i++)
			{
				int num2 = this._legacyPoseIndices[i];
				SelfiePoseConfigSO.PoseEntry poseEntry = ((num2 == 0) ? null : this._config.poses[num2 - 1]);
				list.Add(SelfiePosePicker.LocalizedPoseLabel((poseEntry == null) ? "Default" : SelfiePoseConfigSO.PoseLabel(poseEntry)));
				if (poseEntry != null && poseEntry.poseIndex == this._currentPoseIndex)
				{
					num = i;
				}
			}
			this._poseDropdown.ClearOptions();
			this._poseDropdown.AddOptions(list);
			this._poseDropdown.SetValueWithoutNotify(num);
		}

		// Token: 0x06006E4A RID: 28234 RVA: 0x002C61F0 File Offset: 0x002C43F0
		public void SelectFace(int menuIndex)
		{
			List<SelfiePoseConfigSO.FaceEntry> list = ((this._config != null) ? this._config.faces : null);
			int num = ((menuIndex <= 0 || list == null || menuIndex - 1 >= list.Count) ? 0 : list[menuIndex - 1].faceExpressionIndex);
			this.SetFaceIndex(num);
		}

		// Token: 0x17001212 RID: 4626
		// (get) Token: 0x06006E4B RID: 28235 RVA: 0x002C6245 File Offset: 0x002C4445
		public SelfiePoseConfigSO Config
		{
			get
			{
				return this._config;
			}
		}

		// Token: 0x06006E4C RID: 28236 RVA: 0x002C624D File Offset: 0x002C444D
		private void OnPoseSelected(int index)
		{
			if (index >= 0 && index < this._legacyPoseIndices.Count)
			{
				this.SelectPose(this._legacyPoseIndices[index]);
			}
			this.SetMenuVisible(false);
		}

		// Token: 0x06006E4D RID: 28237 RVA: 0x002C627A File Offset: 0x002C447A
		private void OnFaceSelected(int index)
		{
			this.SelectFace(index);
			this.SetMenuVisible(false);
		}

		// Token: 0x06006E4E RID: 28238 RVA: 0x002C628A File Offset: 0x002C448A
		private static void StretchWithPadding(RectTransform rt, float l, float t, float r, float b)
		{
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMin = new Vector2(l, b);
			rt.offsetMax = new Vector2(-r, -t);
		}

		// Token: 0x06006E4F RID: 28239 RVA: 0x002C62C0 File Offset: 0x002C44C0
		private static string LocalizedPoseLabel(string english)
		{
			string text;
			if (!GameLocalization.TryGetSourceKey("gameplay_prompts", english, out text))
			{
				return GameLocalization.GetSource("phone_story", english);
			}
			return GameLocalization.Get(text, english);
		}

		// Token: 0x06006E50 RID: 28240 RVA: 0x002C62EF File Offset: 0x002C44EF
		private void OnEnable()
		{
			GameLocalization.LocaleChanged += this.RefreshLocalizedOptions;
		}

		// Token: 0x06006E51 RID: 28241 RVA: 0x002C6304 File Offset: 0x002C4504
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.RefreshLocalizedOptions;
			this.SetPoseIndex(0, null);
			this._poseLayerTargetWeight = 0f;
			if (this._animator != null && this._poseLayerIndex >= 0)
			{
				this._animator.SetLayerWeight(this._poseLayerIndex, 0f);
			}
			this.RestorePoseBlendShapes(true);
			this.SetFaceIndex(0);
		}

		// Token: 0x06006E52 RID: 28242 RVA: 0x002C6370 File Offset: 0x002C4570
		private void RefreshLocalizedOptions()
		{
			if (this._poseDropdown != null)
			{
				int num = 0;
				while (num < this._poseDropdown.options.Count && num < this._legacyPoseIndices.Count)
				{
					int num2 = this._legacyPoseIndices[num];
					this._poseDropdown.options[num].text = SelfiePosePicker.LocalizedPoseLabel((num2 == 0) ? "Default" : SelfiePoseConfigSO.PoseLabel(this._config.poses[num2 - 1]));
					num++;
				}
				this._poseDropdown.RefreshShownValue();
			}
			if (this._faceDropdown != null)
			{
				for (int i = 0; i < this._faceDropdown.options.Count; i++)
				{
					this._faceDropdown.options[i].text = SelfiePosePicker.LocalizedPoseLabel((i == 0) ? "None" : SelfiePoseConfigSO.FaceLabel(this._config.faces[i - 1]));
				}
				this._faceDropdown.RefreshShownValue();
			}
		}

		// Token: 0x06006E53 RID: 28243 RVA: 0x002C647A File Offset: 0x002C467A
		private void OnDestroy()
		{
			if (this._canvas != null)
			{
				Object.Destroy(this._canvas.gameObject);
			}
		}

		// Token: 0x040070E2 RID: 28898
		public KeyCode toggleMenuKey = KeyCode.Tab;

		// Token: 0x040070E3 RID: 28899
		[Tooltip("Se false, il dropdown TAB legacy NON viene costruito ne' mostrato (la UI e' il radiale). La logica di selezione resta usabile via SelectPose/SelectFace.")]
		public bool buildLegacyDropdownMenu = true;

		// Token: 0x040070E4 RID: 28900
		[Tooltip("Velocita' (peso/sec) con cui il layer SelfiePose entra/esce. Il blend TRA pose e' invece dato dalle transizioni del controller (vedi Setup Selfie Pose Layer).")]
		public float poseLayerFadeSpeed = 4f;

		// Token: 0x040070E5 RID: 28901
		private Animator _animator;

		// Token: 0x040070E6 RID: 28902
		private SelfiePoseConfigSO _config;

		// Token: 0x040070E7 RID: 28903
		private Action<bool> _onMenuVisibilityChanged;

		// Token: 0x040070E8 RID: 28904
		private Action<SelfiePoseConfigSO.PoseEntry> _onPoseChanged;

		// Token: 0x040070E9 RID: 28905
		private PlayerEmotionController _playerEmotion;

		// Token: 0x040070EA RID: 28906
		private bool _faceOverrideSuspended;

		// Token: 0x040070EB RID: 28907
		private Func<bool> _canUseBodyPoses;

		// Token: 0x040070EC RID: 28908
		private bool _bodyPosesAllowed = true;

		// Token: 0x040070ED RID: 28909
		private const string PoseParam = "PoseIndex";

		// Token: 0x040070EE RID: 28910
		private const string FaceParam = "FaceExpressionIndex";

		// Token: 0x040070EF RID: 28911
		private const string PoseLayerName = "SelfiePose";

		// Token: 0x040070F0 RID: 28912
		private const int PoseNoneIndex = 0;

		// Token: 0x040070F1 RID: 28913
		private const int FaceNeutralIndex = 0;

		// Token: 0x040070F2 RID: 28914
		private int _poseLayerIndex = -1;

		// Token: 0x040070F3 RID: 28915
		private float _poseLayerTargetWeight;

		// Token: 0x040070F4 RID: 28916
		private int _currentPoseIndex;

		// Token: 0x040070F5 RID: 28917
		private int _currentFaceIndex;

		// Token: 0x040070F6 RID: 28918
		private SelfiePoseConfigSO.PoseEntry _currentPoseEntry;

		// Token: 0x040070F7 RID: 28919
		private readonly List<SelfiePosePicker.PoseBlendShapeState> _poseBlendShapeStates = new List<SelfiePosePicker.PoseBlendShapeState>();

		// Token: 0x040070F8 RID: 28920
		private Canvas _canvas;

		// Token: 0x040070F9 RID: 28921
		private GameObject _menuRoot;

		// Token: 0x040070FA RID: 28922
		private bool _menuVisible;

		// Token: 0x040070FB RID: 28923
		private Dropdown _poseDropdown;

		// Token: 0x040070FC RID: 28924
		private Dropdown _faceDropdown;

		// Token: 0x040070FD RID: 28925
		private List<int> _legacyPoseIndices = new List<int> { 0 };

		// Token: 0x02000E6E RID: 3694
		private sealed class PoseBlendShapeState
		{
			// Token: 0x040070FE RID: 28926
			public SkinnedMeshRenderer renderer;

			// Token: 0x040070FF RID: 28927
			public Mesh mesh;

			// Token: 0x04007100 RID: 28928
			public int index;

			// Token: 0x04007101 RID: 28929
			public float originalWeight;

			// Token: 0x04007102 RID: 28930
			public bool ownedByCurrentPose;
		}
	}
}
