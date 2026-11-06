using System;
using Bozo.AnimeCharacters;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BoZo.CharacterCreator.MC
{
	// Token: 0x020007CA RID: 1994
	public class MCGeneralInfoController : MonoBehaviour
	{
		// Token: 0x14000055 RID: 85
		// (add) Token: 0x060034F7 RID: 13559 RVA: 0x0012E0C4 File Offset: 0x0012C2C4
		// (remove) Token: 0x060034F8 RID: 13560 RVA: 0x0012E0FC File Offset: 0x0012C2FC
		public event Action<string> OnPresetNameChanged;

		// Token: 0x14000056 RID: 86
		// (add) Token: 0x060034F9 RID: 13561 RVA: 0x0012E134 File Offset: 0x0012C334
		// (remove) Token: 0x060034FA RID: 13562 RVA: 0x0012E16C File Offset: 0x0012C36C
		public event Action<string> OnBioChanged;

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x060034FB RID: 13563 RVA: 0x0012E1A4 File Offset: 0x0012C3A4
		// (remove) Token: 0x060034FC RID: 13564 RVA: 0x0012E1DC File Offset: 0x0012C3DC
		public event Action OnCapturePhotoRequested;

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x060034FD RID: 13565 RVA: 0x0012E214 File Offset: 0x0012C414
		// (remove) Token: 0x060034FE RID: 13566 RVA: 0x0012E24C File Offset: 0x0012C44C
		public event Action OnRemovePhotoRequested;

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x060034FF RID: 13567 RVA: 0x0012E281 File Offset: 0x0012C481
		public bool IsLivePhotoPreviewVisible
		{
			get
			{
				return this.presetPhotoLivePreview != null && this.presetPhotoLivePreview.isActiveAndEnabled && this.presetPhotoLivePreview.texture != null;
			}
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x0012E2B1 File Offset: 0x0012C4B1
		private void Start()
		{
			this.EnsureInitialized();
			this.UpdateHeaderLabel();
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x0012E2B1 File Offset: 0x0012C4B1
		private void OnEnable()
		{
			this.EnsureInitialized();
			this.UpdateHeaderLabel();
		}

		// Token: 0x06003502 RID: 13570 RVA: 0x0012E2C0 File Offset: 0x0012C4C0
		private string GenerateNextDefaultName()
		{
			int num = PlayerPrefs.GetInt("MCPresetNameCounter", 0) + 1;
			PlayerPrefs.SetInt("MCPresetNameCounter", num);
			PlayerPrefs.Save();
			return string.Format("{0}{1}", this.autoNamePrefix, num);
		}

		// Token: 0x06003503 RID: 13571 RVA: 0x0012E301 File Offset: 0x0012C501
		private void OnPresetNameValueChanged(string value)
		{
			this.currentPresetName = value;
			this.UpdateHeaderLabel();
		}

		// Token: 0x06003504 RID: 13572 RVA: 0x0012E310 File Offset: 0x0012C510
		private void OnPresetNameEndEdit(string value)
		{
			string text = value.Trim();
			if (string.IsNullOrEmpty(text))
			{
				text = this.GenerateNextDefaultName();
				if (this.presetNameInput != null)
				{
					this.presetNameInput.text = text;
				}
			}
			this.currentPresetName = text;
			this.UpdateHeaderLabel();
			Action<string> onPresetNameChanged = this.OnPresetNameChanged;
			if (onPresetNameChanged == null)
			{
				return;
			}
			onPresetNameChanged(this.currentPresetName);
		}

		// Token: 0x06003505 RID: 13573 RVA: 0x0012E370 File Offset: 0x0012C570
		private void OnBioValueChanged(string value)
		{
			this.currentBio = value;
			this.UpdateCharCount();
			Action<string> onBioChanged = this.OnBioChanged;
			if (onBioChanged == null)
			{
				return;
			}
			onBioChanged(this.currentBio);
		}

		// Token: 0x06003506 RID: 13574 RVA: 0x0012E398 File Offset: 0x0012C598
		private void UpdateCharCount()
		{
			if (this.charCountLabel != null && this.bioInput != null)
			{
				LocalizedGameText.BindFontOnly(this.charCountLabel);
				this.charCountLabel.text = string.Format("{0}/{1}", this.bioInput.text.Length, this.maxBioLength);
			}
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x0012E402 File Offset: 0x0012C602
		private void UpdateHeaderLabel()
		{
			if (this.headerCharacterNameLabel != null)
			{
				LocalizedGameText.BindFontOnly(this.headerCharacterNameLabel);
				this.headerCharacterNameLabel.text = this.currentPresetName;
			}
		}

		// Token: 0x06003508 RID: 13576 RVA: 0x0012E430 File Offset: 0x0012C630
		public void EnsureInitialized()
		{
			if (this.isInitialized)
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(this.currentPresetName))
			{
				this.currentPresetName = this.GenerateNextDefaultName();
			}
			if (this.currentBio == null)
			{
				this.currentBio = this.defaultBio;
			}
			if (this.presetNameInput != null)
			{
				this.presetNameInput.characterLimit = this.maxPresetNameLength;
				this.presetNameInput.text = this.currentPresetName;
				this.presetNameInput.onEndEdit.RemoveAllListeners();
				this.presetNameInput.onEndEdit.AddListener(new UnityAction<string>(this.OnPresetNameEndEdit));
				this.presetNameInput.onValueChanged.RemoveAllListeners();
				this.presetNameInput.onValueChanged.AddListener(new UnityAction<string>(this.OnPresetNameValueChanged));
			}
			if (this.bioInput != null)
			{
				this.bioInput.characterLimit = this.maxBioLength;
				this.bioInput.text = this.currentBio;
				this.bioInput.onValueChanged.RemoveAllListeners();
				this.bioInput.onValueChanged.AddListener(new UnityAction<string>(this.OnBioValueChanged));
			}
			if (this.resetButton != null)
			{
				this.resetButton.onClick.RemoveAllListeners();
				this.resetButton.onClick.AddListener(new UnityAction(this.ResetToDefaults));
			}
			if (this.capturePhotoButton != null)
			{
				this.capturePhotoButton.onClick.RemoveAllListeners();
				this.capturePhotoButton.onClick.AddListener(delegate
				{
					Action onCapturePhotoRequested = this.OnCapturePhotoRequested;
					if (onCapturePhotoRequested == null)
					{
						return;
					}
					onCapturePhotoRequested();
				});
			}
			if (this.removePhotoButton != null)
			{
				this.removePhotoButton.onClick.RemoveAllListeners();
				this.removePhotoButton.onClick.AddListener(new UnityAction(this.HandleRemovePhotoClicked));
			}
			this.SetPhotoPreviewState(false);
			this.UpdateCharCount();
			this.isInitialized = true;
		}

		// Token: 0x06003509 RID: 13577 RVA: 0x0012E61C File Offset: 0x0012C81C
		public void ResetToDefaults()
		{
			this.currentPresetName = this.GenerateNextDefaultName();
			this.currentBio = this.defaultBio;
			if (this.presetNameInput != null)
			{
				this.presetNameInput.text = this.currentPresetName;
			}
			if (this.bioInput != null)
			{
				this.bioInput.text = this.currentBio;
			}
			this.UpdateCharCount();
			this.UpdateHeaderLabel();
			Action<string> onPresetNameChanged = this.OnPresetNameChanged;
			if (onPresetNameChanged != null)
			{
				onPresetNameChanged(this.currentPresetName);
			}
			Action<string> onBioChanged = this.OnBioChanged;
			if (onBioChanged != null)
			{
				onBioChanged(this.currentBio);
			}
			this.ClearPresetPhotoPreview();
		}

		// Token: 0x0600350A RID: 13578 RVA: 0x0012E6BF File Offset: 0x0012C8BF
		public string GetPresetName()
		{
			return this.currentPresetName;
		}

		// Token: 0x0600350B RID: 13579 RVA: 0x0012E6C7 File Offset: 0x0012C8C7
		public string GetBio()
		{
			return this.currentBio;
		}

		// Token: 0x0600350C RID: 13580 RVA: 0x0012E6CF File Offset: 0x0012C8CF
		public void CaptureState(CharacterStateData state)
		{
			if (state != null)
			{
				state.mainCharacterBio = this.GetBio() ?? string.Empty;
			}
		}

		// Token: 0x0600350D RID: 13581 RVA: 0x0012E6E9 File Offset: 0x0012C8E9
		public void ApplyPresetMetadata(string presetName, CharacterStateData state)
		{
			this.SetPresetName(presetName);
			this.SetBio((state != null) ? state.mainCharacterBio : null);
		}

		// Token: 0x0600350E RID: 13582 RVA: 0x0012E704 File Offset: 0x0012C904
		public void SetPresetName(string name)
		{
			if (!string.IsNullOrWhiteSpace(name))
			{
				this.currentPresetName = name;
			}
			else if (string.IsNullOrWhiteSpace(this.currentPresetName))
			{
				this.currentPresetName = this.GenerateNextDefaultName();
			}
			if (this.presetNameInput != null)
			{
				this.presetNameInput.text = this.currentPresetName;
			}
			this.UpdateHeaderLabel();
		}

		// Token: 0x0600350F RID: 13583 RVA: 0x0012E760 File Offset: 0x0012C960
		public void SetBio(string bio)
		{
			this.currentBio = bio ?? "";
			if (this.bioInput != null)
			{
				this.bioInput.text = this.currentBio;
			}
			this.UpdateCharCount();
		}

		// Token: 0x06003510 RID: 13584 RVA: 0x0012E797 File Offset: 0x0012C997
		public void SetHeaderCharacterNameLabel(TextMeshProUGUI label)
		{
			this.headerCharacterNameLabel = label;
			this.UpdateHeaderLabel();
		}

		// Token: 0x06003511 RID: 13585 RVA: 0x0012E7A6 File Offset: 0x0012C9A6
		public void SetPresetPhotoPreview(Sprite sprite)
		{
			if (this.presetPhotoPreview == null)
			{
				return;
			}
			this.presetPhotoPreview.sprite = sprite;
			this.SetPhotoPreviewState(sprite != null);
		}

		// Token: 0x06003512 RID: 13586 RVA: 0x0012E7D0 File Offset: 0x0012C9D0
		public void ClearPresetPhotoPreview()
		{
			this.SetPresetPhotoPreview(null);
		}

		// Token: 0x06003513 RID: 13587 RVA: 0x0012E7DC File Offset: 0x0012C9DC
		public void SetLivePhotoPreviewTexture(Texture texture)
		{
			if (this.presetPhotoLivePreview == null)
			{
				return;
			}
			this.presetPhotoLivePreview.texture = texture;
			if (this.presetPhotoPreview == null || this.presetPhotoPreview.sprite == null)
			{
				this.SetPhotoPreviewState(false);
			}
		}

		// Token: 0x06003514 RID: 13588 RVA: 0x0012E82C File Offset: 0x0012CA2C
		private void HandleRemovePhotoClicked()
		{
			this.ClearPresetPhotoPreview();
			Action onRemovePhotoRequested = this.OnRemovePhotoRequested;
			if (onRemovePhotoRequested == null)
			{
				return;
			}
			onRemovePhotoRequested();
		}

		// Token: 0x06003515 RID: 13589 RVA: 0x0012E844 File Offset: 0x0012CA44
		private void SetPhotoPreviewState(bool hasCapturedPhoto)
		{
			if (this.presetPhotoPreview != null)
			{
				this.presetPhotoPreview.gameObject.SetActive(hasCapturedPhoto);
			}
			if (this.presetPhotoLivePreview != null)
			{
				this.presetPhotoLivePreview.gameObject.SetActive(!hasCapturedPhoto);
			}
			if (this.removePhotoButton != null)
			{
				this.removePhotoButton.gameObject.SetActive(hasCapturedPhoto);
				this.removePhotoButton.interactable = hasCapturedPhoto;
			}
		}

		// Token: 0x04003538 RID: 13624
		private const string ProgressiveCounterKey = "MCPresetNameCounter";

		// Token: 0x04003539 RID: 13625
		[Header("UI References")]
		[SerializeField]
		private TMP_InputField presetNameInput;

		// Token: 0x0400353A RID: 13626
		[SerializeField]
		private TMP_InputField bioInput;

		// Token: 0x0400353B RID: 13627
		[Header("Header — Character Name (real-time)")]
		[Tooltip("TextMeshProUGUI on the header that shows the current character name in real-time. Assign externally.")]
		[SerializeField]
		private TextMeshProUGUI headerCharacterNameLabel;

		// Token: 0x0400353C RID: 13628
		[Header("Defaults")]
		[Tooltip("Prefix for auto-generated names. A progressive integer is appended.")]
		[SerializeField]
		private string autoNamePrefix = "Character #";

		// Token: 0x0400353D RID: 13629
		[SerializeField]
		private string defaultBio = "";

		// Token: 0x0400353E RID: 13630
		[Header("Validation")]
		[SerializeField]
		private int maxPresetNameLength = 32;

		// Token: 0x0400353F RID: 13631
		[SerializeField]
		private int maxBioLength = 500;

		// Token: 0x04003540 RID: 13632
		[Header("Optional UI")]
		[SerializeField]
		private TextMeshProUGUI charCountLabel;

		// Token: 0x04003541 RID: 13633
		[SerializeField]
		private Button resetButton;

		// Token: 0x04003542 RID: 13634
		[Header("Preset Photo")]
		[SerializeField]
		private RawImage presetPhotoLivePreview;

		// Token: 0x04003543 RID: 13635
		[SerializeField]
		private Image presetPhotoPreview;

		// Token: 0x04003544 RID: 13636
		[SerializeField]
		private Button capturePhotoButton;

		// Token: 0x04003545 RID: 13637
		[SerializeField]
		private Button removePhotoButton;

		// Token: 0x0400354A RID: 13642
		private string currentPresetName;

		// Token: 0x0400354B RID: 13643
		private string currentBio;

		// Token: 0x0400354C RID: 13644
		private bool isInitialized;
	}
}
