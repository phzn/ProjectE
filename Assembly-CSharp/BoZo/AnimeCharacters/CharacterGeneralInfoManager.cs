using System;
using System.Collections.Generic;
using System.IO;
using Michsky.MUIP;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000866 RID: 2150
	public class CharacterGeneralInfoManager : MonoBehaviour
	{
		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x0600391D RID: 14621 RVA: 0x0014DC08 File Offset: 0x0014BE08
		private DialogueOverrideSelector DialogueOverride
		{
			get
			{
				if (!(this.dialogueOverrideSelector != null))
				{
					return this.dialogueOverrideSelector = Object.FindObjectOfType<DialogueOverrideSelector>(true);
				}
				return this.dialogueOverrideSelector;
			}
		}

		// Token: 0x0600391E RID: 14622 RVA: 0x0014DC3C File Offset: 0x0014BE3C
		public void LoadPresetGeneralInfo(CharacterGeneralInfo info, bool genderInteractable)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.usesExplicitPresetSaving = true;
			this.currentCharacter = new CharacterDataExtended
			{
				generalInfo = new CharacterGeneralInfo(info)
			};
			if (this.nameInput != null)
			{
				this.nameInput.SetTextWithoutNotify(info.characterName ?? string.Empty);
			}
			if (this.surnameInput != null)
			{
				this.surnameInput.SetTextWithoutNotify(info.surname ?? string.Empty);
			}
			if (this.descriptionInput != null)
			{
				this.descriptionInput.SetTextWithoutNotify(info.description ?? string.Empty);
			}
			this.SetCurrentAttitude(info.attitude ?? string.Empty);
			this.SetCurrentPersonality(info.personality);
			if (this.birthdayInput != null)
			{
				this.birthdayInput.SetBirthday(info.birthday);
			}
			this.SetCurrentGender(info.gender);
			this.SetCurrentDialogueOverride(info.dialogueOverrideId);
			this.SetGenderInteractable(genderInteractable);
			if (this.stateManager != null)
			{
				this.stateManager.UpdateCharacterName();
			}
		}

		// Token: 0x0600391F RID: 14623 RVA: 0x0014DD65 File Offset: 0x0014BF65
		private void Start()
		{
			this.InitializeManager();
			this.SetupEventListeners();
		}

		// Token: 0x06003920 RID: 14624 RVA: 0x0014DD73 File Offset: 0x0014BF73
		private void InitializeManager()
		{
			this.stateManager = CharacterStateManager.Instance;
			this.SetupDropdowns();
		}

		// Token: 0x06003921 RID: 14625 RVA: 0x0014DD88 File Offset: 0x0014BF88
		private void SetupDropdowns()
		{
			this.NormalizePersonalityOptions();
			CharacterGeneralInfoManager.SetupDropdownPreservingSelection(this.attitudeDropdown);
			CharacterGeneralInfoManager.SetupDropdownPreservingSelection(this.genderDropdown);
			CustomizationDropdownLocalization.Bind(this.attitudeDropdown, false, "dialogues_ui");
			CustomizationDropdownLocalization.Bind(this.genderDropdown, false, "cielos_apps");
		}

		// Token: 0x06003922 RID: 14626 RVA: 0x0014DDD4 File Offset: 0x0014BFD4
		private static void SetupDropdownPreservingSelection(CustomDropdown dropdown)
		{
			if (dropdown == null || dropdown.items.Count == 0)
			{
				return;
			}
			int num = Mathf.Clamp(dropdown.selectedItemIndex, 0, dropdown.items.Count - 1);
			dropdown.selectedItemIndex = num;
			dropdown.SetupDropdown();
			CharacterGeneralInfoManager.SetDropdownSelection(dropdown, num);
		}

		// Token: 0x06003923 RID: 14627 RVA: 0x0014DE28 File Offset: 0x0014C028
		private static void SetDropdownSelection(CustomDropdown dropdown, int selectedIndex)
		{
			if (dropdown == null)
			{
				return;
			}
			dropdown.selectedItemIndex = Mathf.Max(0, selectedIndex);
			if (dropdown.items.Count == 0)
			{
				return;
			}
			dropdown.SetDropdownIndex(Mathf.Clamp(selectedIndex, 0, dropdown.items.Count - 1), true);
		}

		// Token: 0x06003924 RID: 14628 RVA: 0x0014DE78 File Offset: 0x0014C078
		private void SetupEventListeners()
		{
			if (this.nameInput != null)
			{
				this.nameInput.onValueChanged.AddListener(new UnityAction<string>(this.OnNameChanged));
			}
			if (this.surnameInput != null)
			{
				this.surnameInput.onValueChanged.AddListener(new UnityAction<string>(this.OnSurnameChanged));
			}
			if (this.attitudeDropdown != null)
			{
				this.attitudeDropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnAttitudeChanged));
			}
			if (this.genderDropdown != null)
			{
				this.genderDropdown.onValueChanged.AddListener(new UnityAction<int>(this.OnGenderDropdownChanged));
			}
			DialogueOverrideSelector dialogueOverride = this.DialogueOverride;
			if (dialogueOverride != null)
			{
				CustomDropdown component = dialogueOverride.GetComponent<CustomDropdown>();
				if (component != null)
				{
					component.onValueChanged.AddListener(new UnityAction<int>(this.OnDialogueOverrideChanged));
				}
			}
			if (this.descriptionInput != null)
			{
				this.descriptionInput.onValueChanged.AddListener(new UnityAction<string>(this.OnDescriptionChanged));
			}
			if (this.birthdayInput != null)
			{
				this.SetupBirthdayListeners();
			}
		}

		// Token: 0x06003925 RID: 14629 RVA: 0x0014DFA2 File Offset: 0x0014C1A2
		private void SetupBirthdayListeners()
		{
			if (this.birthdayInput != null)
			{
				BirthdayInput birthdayInput = this.birthdayInput;
				birthdayInput.OnBirthdayChanged = (Action<string>)Delegate.Combine(birthdayInput.OnBirthdayChanged, new Action<string>(this.OnBirthdayChanged));
			}
		}

		// Token: 0x06003926 RID: 14630 RVA: 0x0014DFDC File Offset: 0x0014C1DC
		public void LoadCharacterGeneralInfo(CharacterDataExtended characterData)
		{
			if (characterData == null)
			{
				return;
			}
			this.currentCharacter = characterData;
			if (this.nameInput != null)
			{
				this.nameInput.text = characterData.generalInfo.characterName;
			}
			if (this.surnameInput != null)
			{
				this.surnameInput.text = characterData.generalInfo.surname;
			}
			this.SetCurrentGender(characterData.generalInfo.gender);
			this.SetCurrentAttitude(characterData.generalInfo.attitude);
			this.SetCurrentPersonality(characterData.generalInfo.personality);
			this.SetCurrentDialogueOverride(characterData.generalInfo.dialogueOverrideId);
			if (this.descriptionInput != null)
			{
				this.descriptionInput.text = characterData.generalInfo.description;
			}
			if (this.birthdayInput != null)
			{
				string birthday = characterData.generalInfo.birthday;
				Debug.Log("Loading birthday: '" + birthday + "' for character: " + characterData.characterName);
				this.birthdayInput.SetBirthday(birthday);
			}
			Debug.Log("Loaded general info for character: " + characterData.characterName);
		}

		// Token: 0x06003927 RID: 14631 RVA: 0x0014E0FC File Offset: 0x0014C2FC
		public void SaveCurrentGeneralInfo()
		{
			if (this.currentCharacter == null)
			{
				this.CreateNewCharacterData();
			}
			CharacterGeneralInfo generalInfo = this.currentCharacter.generalInfo;
			TMP_InputField tmp_InputField = this.nameInput;
			generalInfo.characterName = ((tmp_InputField != null) ? tmp_InputField.text : null) ?? "";
			CharacterGeneralInfo generalInfo2 = this.currentCharacter.generalInfo;
			TMP_InputField tmp_InputField2 = this.surnameInput;
			generalInfo2.surname = ((tmp_InputField2 != null) ? tmp_InputField2.text : null) ?? "";
			if (this.attitudeDropdown != null && this.attitudeDropdown.selectedItemIndex >= 0 && this.attitudeDropdown.selectedItemIndex < this.attitudeDropdown.items.Count)
			{
				this.currentCharacter.generalInfo.attitude = this.attitudeDropdown.items[this.attitudeDropdown.selectedItemIndex].itemName;
			}
			else
			{
				this.currentCharacter.generalInfo.attitude = "";
			}
			CharacterGeneralInfo generalInfo3 = this.currentCharacter.generalInfo;
			TMP_InputField tmp_InputField3 = this.descriptionInput;
			generalInfo3.description = ((tmp_InputField3 != null) ? tmp_InputField3.text : null) ?? "";
			this.currentCharacter.generalInfo.gender = this.GetCurrentGender();
			this.currentCharacter.generalInfo.personality = this.GetCurrentPersonality();
			this.currentCharacter.generalInfo.dialogueOverrideId = this.GetCurrentDialogueOverride();
			if (this.birthdayInput != null)
			{
				string birthdayOrDefault = this.birthdayInput.GetBirthdayOrDefault();
				this.currentCharacter.generalInfo.birthday = birthdayOrDefault;
				Debug.Log("Saving birthday: " + birthdayOrDefault);
			}
			string text = (this.currentCharacter.generalInfo.characterName + " " + this.currentCharacter.generalInfo.surname).Trim();
			if (!string.IsNullOrEmpty(text))
			{
				this.currentCharacter.characterName = text;
			}
			Debug.Log("Invoking OnGeneralInfoChanged event. Full name: '" + text + "'");
			Action onGeneralInfoChanged = this.OnGeneralInfoChanged;
			if (onGeneralInfoChanged != null)
			{
				onGeneralInfoChanged();
			}
			Action<CharacterGeneralInfo> onGeneralInfoChangedCharacter = this.OnGeneralInfoChangedCharacter;
			if (onGeneralInfoChangedCharacter != null)
			{
				onGeneralInfoChangedCharacter(this.currentCharacter.generalInfo);
			}
			if (CharacterStateManager.Instance != null)
			{
				CharacterStateManager.Instance.UpdateCharacterName();
			}
			Debug.Log("Saved general info for character: " + this.currentCharacter.characterName);
		}

		// Token: 0x06003928 RID: 14632 RVA: 0x0014E34C File Offset: 0x0014C54C
		public void ResetToDefaults()
		{
			if (this.nameInput != null)
			{
				this.nameInput.text = "";
			}
			if (this.surnameInput != null)
			{
				this.surnameInput.text = "";
			}
			CharacterGeneralInfoManager.SetDropdownSelection(this.attitudeDropdown, 0);
			if (this.descriptionInput != null)
			{
				this.descriptionInput.text = "";
			}
			if (this.birthdayInput != null)
			{
				string text = DateTime.Today.AddYears(-21).ToString("yyyyMMdd");
				this.birthdayInput.SetBirthday(text);
			}
			this.CreateNewCharacterData();
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x0014E3FC File Offset: 0x0014C5FC
		public void CreateNewCharacterData()
		{
			this.currentCharacter = new CharacterDataExtended
			{
				characterName = "New Character",
				isMainCharacter = false,
				createdFrom = "character_creator"
			};
			this.currentCharacter.generalInfo = new CharacterGeneralInfo();
			Debug.Log("Created new character data for editing");
		}

		// Token: 0x0600392A RID: 14634 RVA: 0x0014E44C File Offset: 0x0014C64C
		private void OnNameChanged(string newValue)
		{
			Debug.Log("OnNameChanged called with value: '" + newValue + "'");
			if (this.stateManager != null)
			{
				this.stateManager.UpdateCharacterName();
			}
			if (this.stateManager != null)
			{
				string currentCharacterName = this.stateManager.GetCurrentCharacterName();
				if (!string.IsNullOrEmpty(currentCharacterName))
				{
					this.SaveGeneralInfoToFile(currentCharacterName);
				}
			}
		}

		// Token: 0x0600392B RID: 14635 RVA: 0x0014E4B0 File Offset: 0x0014C6B0
		private void OnSurnameChanged(string newValue)
		{
			Debug.Log("OnSurnameChanged called with value: '" + newValue + "'");
			if (this.stateManager != null)
			{
				this.stateManager.UpdateCharacterName();
			}
			if (this.stateManager != null)
			{
				string currentCharacterName = this.stateManager.GetCurrentCharacterName();
				if (!string.IsNullOrEmpty(currentCharacterName))
				{
					this.SaveGeneralInfoToFile(currentCharacterName);
				}
			}
		}

		// Token: 0x0600392C RID: 14636 RVA: 0x0014E514 File Offset: 0x0014C714
		private void OnGenderDropdownChanged(int newValue)
		{
			Action<Gender> onGenderChanged = this.OnGenderChanged;
			if (onGenderChanged == null)
			{
				return;
			}
			onGenderChanged(this.GetCurrentGender());
		}

		// Token: 0x0600392D RID: 14637 RVA: 0x0014E52C File Offset: 0x0014C72C
		private void OnDialogueOverrideChanged(int newValue)
		{
			if (this.stateManager != null)
			{
				string currentCharacterName = this.stateManager.GetCurrentCharacterName();
				if (!string.IsNullOrEmpty(currentCharacterName))
				{
					this.SaveGeneralInfoToFile(currentCharacterName);
				}
			}
		}

		// Token: 0x0600392E RID: 14638 RVA: 0x0014E564 File Offset: 0x0014C764
		private void OnAttitudeChanged(int newValue)
		{
			if (this.stateManager != null)
			{
				string currentCharacterName = this.stateManager.GetCurrentCharacterName();
				if (!string.IsNullOrEmpty(currentCharacterName))
				{
					this.SaveGeneralInfoToFile(currentCharacterName);
				}
			}
		}

		// Token: 0x0600392F RID: 14639 RVA: 0x0014E59C File Offset: 0x0014C79C
		private void OnDescriptionChanged(string newValue)
		{
			if (this.stateManager != null)
			{
				string currentCharacterName = this.stateManager.GetCurrentCharacterName();
				if (!string.IsNullOrEmpty(currentCharacterName))
				{
					this.SaveGeneralInfoToFile(currentCharacterName);
				}
			}
		}

		// Token: 0x06003930 RID: 14640 RVA: 0x0014E5D4 File Offset: 0x0014C7D4
		private void OnBirthdayChanged(string newValue)
		{
			Debug.Log("Birthday changed to: " + newValue);
			if (this.stateManager != null)
			{
				string currentCharacterName = this.stateManager.GetCurrentCharacterName();
				if (!string.IsNullOrEmpty(currentCharacterName))
				{
					this.SaveGeneralInfoToFile(currentCharacterName);
				}
			}
		}

		// Token: 0x06003931 RID: 14641 RVA: 0x0014E61A File Offset: 0x0014C81A
		public string GetCurrentName()
		{
			if (!(this.nameInput != null))
			{
				return "";
			}
			return this.nameInput.text;
		}

		// Token: 0x06003932 RID: 14642 RVA: 0x0014E63B File Offset: 0x0014C83B
		public string GetCurrentSurname()
		{
			if (!(this.surnameInput != null))
			{
				return "";
			}
			return this.surnameInput.text;
		}

		// Token: 0x06003933 RID: 14643 RVA: 0x0014E65C File Offset: 0x0014C85C
		public string GetCurrentDescription()
		{
			if (!(this.descriptionInput != null))
			{
				return "";
			}
			return this.descriptionInput.text;
		}

		// Token: 0x06003934 RID: 14644 RVA: 0x0014E680 File Offset: 0x0014C880
		public string GetCurrentAttitude()
		{
			if (this.attitudeDropdown != null && this.attitudeDropdown.selectedItemIndex >= 0 && this.attitudeDropdown.selectedItemIndex < this.attitudeDropdown.items.Count)
			{
				return this.attitudeDropdown.items[this.attitudeDropdown.selectedItemIndex].itemName;
			}
			return "";
		}

		// Token: 0x06003935 RID: 14645 RVA: 0x0014E6EC File Offset: 0x0014C8EC
		public string GetCurrentBirthday()
		{
			if (this.birthdayInput != null)
			{
				return this.birthdayInput.GetBirthdayOrDefault();
			}
			return "";
		}

		// Token: 0x06003936 RID: 14646 RVA: 0x0014E70D File Offset: 0x0014C90D
		public Gender GetCurrentGender()
		{
			if (this.genderDropdown != null && this.genderDropdown.selectedItemIndex == 1)
			{
				return Gender.Female;
			}
			return Gender.Male;
		}

		// Token: 0x06003937 RID: 14647 RVA: 0x0014E72E File Offset: 0x0014C92E
		public void SetCurrentGender(Gender gender)
		{
			CharacterGeneralInfoManager.SetDropdownSelection(this.genderDropdown, (gender == Gender.Female) ? 1 : 0);
		}

		// Token: 0x06003938 RID: 14648 RVA: 0x0014E743 File Offset: 0x0014C943
		public void SetGenderInteractable(bool interactable)
		{
			if (this.genderDropdown != null)
			{
				this.genderDropdown.Interactable(interactable);
			}
		}

		// Token: 0x06003939 RID: 14649 RVA: 0x0014E760 File Offset: 0x0014C960
		public PersonalityType GetCurrentPersonality()
		{
			if (this.attitudeDropdown == null || this.attitudeDropdown.selectedItemIndex < 0 || this.attitudeDropdown.selectedItemIndex >= this.attitudeDropdown.items.Count)
			{
				return PersonalityType.Neutral;
			}
			return CharacterGeneralInfoManager.ParsePersonalityLabel(this.attitudeDropdown.items[this.attitudeDropdown.selectedItemIndex].itemName);
		}

		// Token: 0x0600393A RID: 14650 RVA: 0x0014E7D0 File Offset: 0x0014C9D0
		public void SetCurrentPersonality(PersonalityType personality)
		{
			if (this.attitudeDropdown == null)
			{
				return;
			}
			if (this.NormalizePersonalityOptions())
			{
				this.attitudeDropdown.SetupDropdown();
			}
			for (int i = 0; i < this.attitudeDropdown.items.Count; i++)
			{
				if (CharacterGeneralInfoManager.ParsePersonalityLabel(this.attitudeDropdown.items[i].itemName) == personality)
				{
					CharacterGeneralInfoManager.SetDropdownSelection(this.attitudeDropdown, i);
					return;
				}
			}
			CharacterGeneralInfoManager.SetDropdownSelection(this.attitudeDropdown, 0);
		}

		// Token: 0x0600393B RID: 14651 RVA: 0x0014E854 File Offset: 0x0014CA54
		private bool NormalizePersonalityOptions()
		{
			if (this.attitudeDropdown == null)
			{
				return false;
			}
			bool flag = false;
			HashSet<PersonalityType> hashSet = new HashSet<PersonalityType>();
			foreach (CustomDropdown.Item item in this.attitudeDropdown.items)
			{
				PersonalityType personalityType = CharacterGeneralInfoManager.ParsePersonalityLabel(item.itemName);
				string text = personalityType.ToString();
				if (item.itemName != text)
				{
					item.itemName = text;
					flag = true;
				}
				hashSet.Add(personalityType);
			}
			foreach (object obj in Enum.GetValues(typeof(PersonalityType)))
			{
				PersonalityType personalityType2 = (PersonalityType)obj;
				if (hashSet.Add(personalityType2))
				{
					this.attitudeDropdown.items.Add(new CustomDropdown.Item
					{
						itemName = personalityType2.ToString()
					});
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x0600393C RID: 14652 RVA: 0x0014E984 File Offset: 0x0014CB84
		private static PersonalityType ParsePersonalityLabel(string label)
		{
			PersonalityType personalityType;
			if (Enum.TryParse<PersonalityType>(label, true, out personalityType))
			{
				return personalityType;
			}
			if (string.Equals(label, "Flirty", StringComparison.OrdinalIgnoreCase) || string.Equals(label, "Submissive", StringComparison.OrdinalIgnoreCase))
			{
				return PersonalityType.Friendly;
			}
			if (string.Equals(label, "Energetic", StringComparison.OrdinalIgnoreCase))
			{
				return PersonalityType.Curious;
			}
			if (string.Equals(label, "Strict", StringComparison.OrdinalIgnoreCase) || string.Equals(label, "Dominant", StringComparison.OrdinalIgnoreCase))
			{
				return PersonalityType.Hostile;
			}
			return PersonalityType.Neutral;
		}

		// Token: 0x0600393D RID: 14653 RVA: 0x0014E9EC File Offset: 0x0014CBEC
		public string GetCurrentDialogueOverride()
		{
			DialogueOverrideSelector dialogueOverride = this.DialogueOverride;
			if (!(dialogueOverride != null))
			{
				return "";
			}
			return dialogueOverride.GetSelection();
		}

		// Token: 0x0600393E RID: 14654 RVA: 0x0014EA18 File Offset: 0x0014CC18
		public void SetCurrentDialogueOverride(string dialogueOverrideId)
		{
			DialogueOverrideSelector dialogueOverride = this.DialogueOverride;
			if (dialogueOverride != null)
			{
				dialogueOverride.SetSelection(dialogueOverrideId ?? "");
			}
		}

		// Token: 0x0600393F RID: 14655 RVA: 0x0014EA45 File Offset: 0x0014CC45
		public CharacterGeneralInfo GetCurrentGeneralInfo()
		{
			if (this.currentCharacter == null)
			{
				return null;
			}
			this.SaveCurrentGeneralInfo();
			return this.currentCharacter.generalInfo;
		}

		// Token: 0x06003940 RID: 14656 RVA: 0x0014EA62 File Offset: 0x0014CC62
		public bool HasValidInfo()
		{
			return this.currentCharacter != null && !string.IsNullOrEmpty(this.currentCharacter.generalInfo.characterName);
		}

		// Token: 0x06003941 RID: 14657 RVA: 0x0014EA86 File Offset: 0x0014CC86
		public CharacterDataExtended GetCurrentCharacter()
		{
			return this.currentCharacter;
		}

		// Token: 0x06003942 RID: 14658 RVA: 0x0014EA8E File Offset: 0x0014CC8E
		public void SetCurrentName(string name)
		{
			if (this.nameInput != null)
			{
				this.nameInput.text = name ?? "";
			}
		}

		// Token: 0x06003943 RID: 14659 RVA: 0x0014EAB3 File Offset: 0x0014CCB3
		public void SetCurrentSurname(string surname)
		{
			if (this.surnameInput != null)
			{
				this.surnameInput.text = surname ?? "";
			}
		}

		// Token: 0x06003944 RID: 14660 RVA: 0x0014EAD8 File Offset: 0x0014CCD8
		public void SetCurrentDescription(string description)
		{
			if (this.descriptionInput != null)
			{
				this.descriptionInput.text = description ?? "";
			}
		}

		// Token: 0x06003945 RID: 14661 RVA: 0x0014EAFD File Offset: 0x0014CCFD
		public void SetCurrentAttitude(string attitude)
		{
			this.SetCurrentPersonality(CharacterGeneralInfoManager.ParsePersonalityLabel(attitude));
		}

		// Token: 0x06003946 RID: 14662 RVA: 0x0014EB0B File Offset: 0x0014CD0B
		public void SetCurrentBirthday(string birthday)
		{
			if (this.birthdayInput != null && !string.IsNullOrEmpty(birthday))
			{
				Debug.Log("Setting birthday to: " + birthday);
				this.birthdayInput.SetBirthday(birthday);
			}
		}

		// Token: 0x06003947 RID: 14663 RVA: 0x0014EB40 File Offset: 0x0014CD40
		private void LoadGeneralInfoFromFile(string characterName)
		{
			if (this.usesExplicitPresetSaving)
			{
				return;
			}
			try
			{
				string text = BMAC_SaveSystem.filePath + "/" + characterName + ".json";
				if (File.Exists(text))
				{
					CharacterData characterData = JsonUtility.FromJson<CharacterData>(File.ReadAllText(text));
					if (characterData != null && characterData.generalInfo != null)
					{
						this.SetCurrentGender(characterData.generalInfo.gender);
						if (this.nameInput != null)
						{
							this.nameInput.text = characterData.generalInfo.characterName ?? "";
						}
						if (this.surnameInput != null)
						{
							this.surnameInput.text = characterData.generalInfo.surname ?? "";
						}
						if (this.descriptionInput != null)
						{
							this.descriptionInput.text = characterData.generalInfo.description ?? "";
						}
						this.SetCurrentPersonality(characterData.generalInfo.personality);
						if (!string.IsNullOrEmpty(characterData.generalInfo.attitude))
						{
							this.SetCurrentAttitude(characterData.generalInfo.attitude);
						}
						this.SetCurrentDialogueOverride(characterData.generalInfo.dialogueOverrideId);
						if (this.birthdayInput != null && !string.IsNullOrEmpty(characterData.generalInfo.birthday))
						{
							this.birthdayInput.SetBirthday(characterData.generalInfo.birthday);
						}
						Debug.Log(string.Concat(new string[]
						{
							"✅ Loaded GeneralInfo from file: Name=",
							characterData.generalInfo.characterName,
							", Birthday=",
							characterData.generalInfo.birthday,
							", Attitude=",
							characterData.generalInfo.attitude
						}));
					}
				}
				else
				{
					Debug.LogWarning("File not found: " + text);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to load GeneralInfo from file: " + ex.Message);
			}
		}

		// Token: 0x06003948 RID: 14664 RVA: 0x0014ED44 File Offset: 0x0014CF44
		private void SaveGeneralInfoToFile(string characterName)
		{
			if (this.usesExplicitPresetSaving)
			{
				return;
			}
			try
			{
				string text = BMAC_SaveSystem.filePath + "/" + characterName + ".json";
				if (File.Exists(text))
				{
					CharacterData characterData = JsonUtility.FromJson<CharacterData>(File.ReadAllText(text));
					if (characterData != null)
					{
						if (characterData.generalInfo == null)
						{
							characterData.generalInfo = new CharacterGeneralInfo();
						}
						CharacterGeneralInfo generalInfo = characterData.generalInfo;
						TMP_InputField tmp_InputField = this.nameInput;
						generalInfo.characterName = ((tmp_InputField != null) ? tmp_InputField.text : null) ?? "";
						CharacterGeneralInfo generalInfo2 = characterData.generalInfo;
						TMP_InputField tmp_InputField2 = this.surnameInput;
						generalInfo2.surname = ((tmp_InputField2 != null) ? tmp_InputField2.text : null) ?? "";
						CharacterGeneralInfo generalInfo3 = characterData.generalInfo;
						TMP_InputField tmp_InputField3 = this.descriptionInput;
						generalInfo3.description = ((tmp_InputField3 != null) ? tmp_InputField3.text : null) ?? "";
						if (this.attitudeDropdown != null && this.attitudeDropdown.selectedItemIndex >= 0 && this.attitudeDropdown.selectedItemIndex < this.attitudeDropdown.items.Count)
						{
							characterData.generalInfo.attitude = this.attitudeDropdown.items[this.attitudeDropdown.selectedItemIndex].itemName;
						}
						else
						{
							characterData.generalInfo.attitude = "";
						}
						characterData.generalInfo.gender = this.GetCurrentGender();
						characterData.generalInfo.personality = this.GetCurrentPersonality();
						characterData.generalInfo.dialogueOverrideId = this.GetCurrentDialogueOverride();
						if (this.birthdayInput != null)
						{
							characterData.generalInfo.birthday = this.birthdayInput.GetBirthdayOrDefault();
						}
						string text2 = JsonUtility.ToJson(characterData, true);
						File.WriteAllText(text, text2);
						Debug.Log(string.Concat(new string[]
						{
							"✅ Saved GeneralInfo to file: Name=",
							characterData.generalInfo.characterName,
							", Birthday=",
							characterData.generalInfo.birthday,
							", Attitude=",
							characterData.generalInfo.attitude
						}));
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to save GeneralInfo to file: " + ex.Message);
			}
		}

		// Token: 0x06003949 RID: 14665 RVA: 0x0014EF78 File Offset: 0x0014D178
		private void OnEnable()
		{
			if (this.stateManager != null)
			{
				string currentCharacterName = this.stateManager.GetCurrentCharacterName();
				if (!string.IsNullOrEmpty(currentCharacterName))
				{
					Debug.Log("CharacterGeneralInfoManager OnEnable - Loading data for: " + currentCharacterName);
					this.LoadGeneralInfoFromFile(currentCharacterName);
				}
			}
		}

		// Token: 0x0600394A RID: 14666 RVA: 0x0014EFC0 File Offset: 0x0014D1C0
		private void OnDisable()
		{
			if (this.stateManager != null)
			{
				string currentCharacterName = this.stateManager.GetCurrentCharacterName();
				if (!string.IsNullOrEmpty(currentCharacterName))
				{
					Debug.Log("CharacterGeneralInfoManager OnDisable - Saving data for: " + currentCharacterName);
					this.SaveGeneralInfoToFile(currentCharacterName);
				}
			}
		}

		// Token: 0x0600394B RID: 14667 RVA: 0x0014F008 File Offset: 0x0014D208
		private void OnDestroy()
		{
			if (this.nameInput != null)
			{
				this.nameInput.onValueChanged.RemoveAllListeners();
			}
			if (this.surnameInput != null)
			{
				this.surnameInput.onValueChanged.RemoveAllListeners();
			}
			if (this.attitudeDropdown != null)
			{
				this.attitudeDropdown.onValueChanged.RemoveAllListeners();
			}
			if (this.genderDropdown != null)
			{
				this.genderDropdown.onValueChanged.RemoveAllListeners();
			}
			if (this.dialogueOverrideSelector != null)
			{
				CustomDropdown component = this.dialogueOverrideSelector.GetComponent<CustomDropdown>();
				if (component != null)
				{
					component.onValueChanged.RemoveAllListeners();
				}
			}
			if (this.descriptionInput != null)
			{
				this.descriptionInput.onValueChanged.RemoveAllListeners();
			}
			if (this.birthdayInput != null)
			{
				BirthdayInput birthdayInput = this.birthdayInput;
				birthdayInput.OnBirthdayChanged = (Action<string>)Delegate.Remove(birthdayInput.OnBirthdayChanged, new Action<string>(this.OnBirthdayChanged));
			}
		}

		// Token: 0x040039D3 RID: 14803
		[Header("UI References")]
		public CanvasGroup canvasGroup;

		// Token: 0x040039D4 RID: 14804
		public TMP_InputField nameInput;

		// Token: 0x040039D5 RID: 14805
		public TMP_InputField surnameInput;

		// Token: 0x040039D6 RID: 14806
		public TMP_InputField descriptionInput;

		// Token: 0x040039D7 RID: 14807
		public CustomDropdown attitudeDropdown;

		// Token: 0x040039D8 RID: 14808
		public CustomDropdown genderDropdown;

		// Token: 0x040039D9 RID: 14809
		[Tooltip("Dialogue override selector (on the CustomDropdown in the Advanced tab). It self-populates; this manager just reads/writes the chosen doc id via it. If unassigned, resolved at runtime.")]
		public DialogueOverrideSelector dialogueOverrideSelector;

		// Token: 0x040039DA RID: 14810
		public BirthdayInput birthdayInput;

		// Token: 0x040039DB RID: 14811
		[Header("Settings")]
		public bool autoSaveOnChange = true;

		// Token: 0x040039DC RID: 14812
		public bool validationEnabled = true;

		// Token: 0x040039DD RID: 14813
		public Action OnGeneralInfoChanged;

		// Token: 0x040039DE RID: 14814
		public Action<CharacterGeneralInfo> OnGeneralInfoChangedCharacter;

		// Token: 0x040039DF RID: 14815
		public Action<Gender> OnGenderChanged;

		// Token: 0x040039E0 RID: 14816
		private CharacterDataExtended currentCharacter;

		// Token: 0x040039E1 RID: 14817
		private CharacterStateManager stateManager;

		// Token: 0x040039E2 RID: 14818
		private bool usesExplicitPresetSaving;
	}
}
