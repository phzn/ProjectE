using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000838 RID: 2104
	public class CharacterCreator : MonoBehaviour
	{
		// Token: 0x060037C8 RID: 14280 RVA: 0x00146648 File Offset: 0x00144848
		private void Awake()
		{
			this.outfits.Clear();
			this.OutfitDataBase.Clear();
			Outfit[] array = Resources.LoadAll<Outfit>("");
			TexturePackage[] array2 = Resources.LoadAll<TexturePackage>("");
			foreach (Outfit outfit in array)
			{
				if (outfit.showCharacterCreator)
				{
					this.OutfitDataBase.Add(outfit.name, outfit.GetComponent<Outfit>());
				}
			}
			foreach (TexturePackage texturePackage in array2)
			{
				this.textures.Add(texturePackage.GetComponent<TexturePackage>());
			}
			this.GenerateOutfitSelection();
			this.GenerateTextureSelection();
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x001466E8 File Offset: 0x001448E8
		private void OnEnable()
		{
			OutfitSystem outfitSystem = this.character;
			outfitSystem.OnOutfitChanged = (UnityAction<Outfit>)Delegate.Combine(outfitSystem.OnOutfitChanged, new UnityAction<Outfit>(this.OnOutfitUpdate));
			OutfitSystem outfitSystem2 = this.character;
			outfitSystem2.OnRigChanged = (UnityAction<SkinnedMeshRenderer>)Delegate.Combine(outfitSystem2.OnRigChanged, new UnityAction<SkinnedMeshRenderer>(this.OnRigUpdate));
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x00146744 File Offset: 0x00144944
		private void OnDisable()
		{
			OutfitSystem outfitSystem = this.character;
			outfitSystem.OnOutfitChanged = (UnityAction<Outfit>)Delegate.Remove(outfitSystem.OnOutfitChanged, new UnityAction<Outfit>(this.OnOutfitUpdate));
			OutfitSystem outfitSystem2 = this.character;
			outfitSystem2.OnRigChanged = (UnityAction<SkinnedMeshRenderer>)Delegate.Remove(outfitSystem2.OnRigChanged, new UnityAction<SkinnedMeshRenderer>(this.OnRigUpdate));
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x0014679F File Offset: 0x0014499F
		public void Start()
		{
			this.GetBodyBlends();
			this.GetFaceBlends();
			this.GetBodyMods();
			this.SwitchCatagory("Top");
			this.UpdateCharacterSaves();
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x001467C4 File Offset: 0x001449C4
		public void GenerateOutfitSelection()
		{
			foreach (Outfit outfit in this.OutfitDataBase.Values.ToArray<Outfit>())
			{
				OutfitSelector outfitSelector = Object.Instantiate<OutfitSelector>(this.outfitSelectorObject, this.outfitContainer);
				outfitSelector.Init(outfit, this);
				this.outfitSelectors.Add(outfitSelector);
			}
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x0014681C File Offset: 0x00144A1C
		public void GenerateTextureSelection()
		{
			foreach (TexturePackage texturePackage in this.textures)
			{
				Transform transform = null;
				if (texturePackage.type == TextureType.Decal)
				{
					transform = this.decalContainer;
				}
				if (texturePackage.type == TextureType.Pattern)
				{
					transform = this.patternContainer;
				}
				TextureSelector textureSelector = Object.Instantiate<TextureSelector>(this.textureSelectorObject, transform);
				textureSelector.Init(texturePackage, this);
				this.textureSelectors.Add(textureSelector);
			}
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x001468AC File Offset: 0x00144AAC
		public void GetBodyBlends()
		{
			for (int i = 0; i < this.blendSliders.Count; i++)
			{
				Object.Destroy(this.blendSliders[i].gameObject);
			}
			this.blendSliders.Clear();
			foreach (string text in this.character.GetShapes())
			{
				BlendSlider blendSlider = Object.Instantiate<BlendSlider>(this.blendSliderObject, this.bodyShapeContainer);
				blendSlider.Init(this.character, text);
				this.blendSliders.Add(blendSlider);
			}
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x00146960 File Offset: 0x00144B60
		public void GetFaceBlends()
		{
			for (int i = 0; i < this.faceBlendSliders.Count; i++)
			{
				Object.Destroy(this.faceBlendSliders[i].gameObject);
			}
			this.faceBlendSliders.Clear();
			foreach (string text in this.character.GetFaceShapes())
			{
				BlendSlider blendSlider = Object.Instantiate<BlendSlider>(this.blendSliderObject, this.faceShapeContainer);
				blendSlider.Init(this.character, text);
				this.faceBlendSliders.Add(blendSlider);
			}
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x00146A14 File Offset: 0x00144C14
		public void GetBodyMods()
		{
			List<BodyShapeModifier> list = this.character.GetMods().Values.ToList<BodyShapeModifier>();
			for (int i = 0; i < this.ModSliders.Count; i++)
			{
				Object.Destroy(this.ModSliders[i].gameObject);
			}
			this.ModSliders.Clear();
			Transform transform = this.bodyShapeContainer;
			foreach (BodyShapeModifier bodyShapeModifier in list)
			{
				if (bodyShapeModifier.sorting == "Head")
				{
					transform = this.faceModContainer;
				}
				else
				{
					transform = this.bodyModContainer;
				}
				BodyShapeSliders bodyShapeSliders = Object.Instantiate<BodyShapeSliders>(this.modSliderObject, transform);
				bodyShapeSliders.Init(this.character, bodyShapeModifier);
				this.ModSliders.Add(bodyShapeSliders);
			}
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x00146B00 File Offset: 0x00144D00
		public void UpdateCharacterSaves()
		{
			SaveSelector[] array = this.saveSlots.Values.ToArray<SaveSelector>();
			for (int i = 0; i < array.Length; i++)
			{
				Object.Destroy(array[i].gameObject);
			}
			this.saveSlots.Clear();
			GameDataPaths.EnsureCharacterSaveDirectories();
			string[] files = Directory.GetFiles(BMAC_SaveSystem.filePath, "*.json");
			string[] files2 = Directory.GetFiles(BMAC_SaveSystem.iconFilePath, "*.png");
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			for (int j = 0; j < files2.Length; j++)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(files2[j]);
				if (!string.IsNullOrEmpty(fileNameWithoutExtension))
				{
					dictionary[fileNameWithoutExtension] = files2[j];
				}
			}
			for (int k = 0; k < files.Length; k++)
			{
				CharacterData characterData = JsonUtility.FromJson<CharacterData>(File.ReadAllText(files[k]));
				Sprite sprite = null;
				string fileNameWithoutExtension2 = Path.GetFileNameWithoutExtension(files[k]);
				string text;
				if (!string.IsNullOrEmpty(fileNameWithoutExtension2) && dictionary.TryGetValue(fileNameWithoutExtension2, out text))
				{
					byte[] array2 = File.ReadAllBytes(text);
					Texture2D texture2D = new Texture2D(2, 2);
					if (texture2D.LoadImage(array2))
					{
						sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
					}
				}
				SaveSelector saveSelector = Object.Instantiate<SaveSelector>(this.saveSelector, this.saveContainer);
				saveSelector.Init(characterData, sprite, this, fileNameWithoutExtension2);
				this.saveSlots.Add(characterData.characterName, saveSelector);
			}
			foreach (CharacterObject characterObject in Resources.LoadAll<CharacterObject>(""))
			{
				if (!this.saveSlots.ContainsKey(characterObject.data.characterName))
				{
					SaveSelector saveSelector2 = Object.Instantiate<SaveSelector>(this.saveSelector, this.saveContainer);
					Sprite sprite2 = null;
					if (characterObject.icon != null)
					{
						sprite2 = Sprite.Create(characterObject.icon, new Rect(0f, 0f, (float)characterObject.icon.width, (float)characterObject.icon.height), new Vector2(0.5f, 0.5f));
					}
					saveSelector2.Init(characterObject.data, sprite2, this, characterObject.data.characterName);
					this.saveSlots.Add(characterObject.data.characterName, saveSelector2);
				}
			}
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x00146D68 File Offset: 0x00144F68
		public void OpenPage(GameObject page)
		{
			this.currentPage.SetActive(false);
			this.previousPage = this.currentPage;
			this.currentPage = page;
			this.currentPage.SetActive(true);
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x00146D95 File Offset: 0x00144F95
		public void BackPage()
		{
			this.currentPage.SetActive(false);
			this.currentPage = this.previousPage;
			this.currentPage.SetActive(true);
			this.colorPickerControl.RemoveObject();
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x00146DC8 File Offset: 0x00144FC8
		public void SetOutfit(Outfit outfit)
		{
			Outfit outfit2 = Object.Instantiate<Outfit>(outfit, this.character.transform);
			this.SetColorPickerObject(outfit2);
			this.SwitchTextureCatagory(outfit.TextureCatagory);
			this.type = outfit.Type;
		}

		// Token: 0x060037D5 RID: 14293 RVA: 0x00146E08 File Offset: 0x00145008
		public void OnOutfitUpdate(Outfit outfit)
		{
			if (outfit == null)
			{
				return;
			}
			if (outfit.Type.name == "Head")
			{
				this.GetFaceBlends();
			}
			if (outfit.Type.name == "Body")
			{
				this.GetBodyBlends();
			}
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x00146E59 File Offset: 0x00145059
		public void OnRigUpdate(SkinnedMeshRenderer rig)
		{
			this.GetBodyMods();
		}

		// Token: 0x060037D7 RID: 14295 RVA: 0x00146E61 File Offset: 0x00145061
		public void SetOutfitDecal(Texture texture)
		{
			if (!this.colorPickerControl.colorObject)
			{
				return;
			}
			this.colorPickerControl.colorObject.SetDecal(texture);
		}

		// Token: 0x060037D8 RID: 14296 RVA: 0x00146E87 File Offset: 0x00145087
		public void SetOutfitPattern(Texture texture)
		{
			if (!this.colorPickerControl.colorObject)
			{
				return;
			}
			this.colorPickerControl.colorObject.SetPattern(texture);
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x00146EAD File Offset: 0x001450AD
		public void RemoveOutfit()
		{
			if (this.type == null)
			{
				return;
			}
			this.character.RemoveOutfit(this.type, true);
			this.colorPickerControl.RemoveObject();
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x00146EDB File Offset: 0x001450DB
		public void RemoveOutfitDecal()
		{
			if (!this.colorPickerControl.colorObject)
			{
				return;
			}
			this.colorPickerControl.colorObject.SetDecal(null);
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x00146F01 File Offset: 0x00145101
		public void RemoveOutfitPattern()
		{
			if (!this.colorPickerControl.colorObject)
			{
				return;
			}
			this.colorPickerControl.colorObject.SetPattern(null);
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x00146F28 File Offset: 0x00145128
		public void SwitchCatagory(string catagory)
		{
			OutfitType outfitType = null;
			if (this.character.KnownOutfitTypes.TryGetValue(catagory, out outfitType))
			{
				using (List<OutfitSelector>.Enumerator enumerator = this.outfitSelectors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						OutfitSelector outfitSelector = enumerator.Current;
						outfitSelector.SetVisable(outfitType);
					}
					goto IL_005B;
				}
			}
			Debug.LogWarning("[CharacterCreator] Unknown outfit type: " + catagory);
			IL_005B:
			Outfit outfit = this.character.GetOutfit(catagory);
			this.SetColorPickerObject(outfit);
			if (outfit == null)
			{
				return;
			}
			this.SwitchTextureCatagory(outfit.TextureCatagory);
			this.type = outfit.Type;
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x00146FD8 File Offset: 0x001451D8
		public void SwitchTextureCatagory(string catagory)
		{
			if (catagory == "")
			{
				catagory = "Outfit";
			}
			foreach (TextureSelector textureSelector in this.textureSelectors)
			{
				textureSelector.SetVisable(catagory);
			}
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x00147040 File Offset: 0x00145240
		public void SetColorPickerObject(string type)
		{
			Outfit outfit = this.character.GetOutfit(type);
			this.colorPickerControl.ChangeObject(outfit);
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x00147066 File Offset: 0x00145266
		public void SetColorPickerObject(Outfit outfit)
		{
			this.colorPickerControl.ChangeObject(outfit);
		}

		// Token: 0x060037E0 RID: 14304 RVA: 0x00147074 File Offset: 0x00145274
		public void ReplaceCharacter(OutfitSystem character)
		{
			Object.Destroy(this.character.gameObject);
			this.character = character;
			this.Spinner.SetCharacter(character.transform);
		}

		// Token: 0x060037E1 RID: 14305 RVA: 0x0014709E File Offset: 0x0014529E
		public void GetCurrentCatagory()
		{
			if (this.type == null)
			{
				return;
			}
			this.SwitchCatagory(this.type.name);
		}

		// Token: 0x060037E2 RID: 14306 RVA: 0x001470C0 File Offset: 0x001452C0
		public void CopyColor(string copyTo)
		{
			Outfit outfit = this.character.GetOutfit((OutfitType)Enum.Parse(typeof(OutfitType), copyTo));
			this.colorPickerControl.CopyColor(outfit);
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x001470FA File Offset: 0x001452FA
		public void ToggleWalk(bool value)
		{
			this.character.animator.SetBool("isWalk", value);
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x00147112 File Offset: 0x00145312
		public void SaveCharacter()
		{
			base.StartCoroutine(this.Save());
		}

		// Token: 0x060037E5 RID: 14309 RVA: 0x00147121 File Offset: 0x00145321
		public Outfit GetOutfit(string outfitName)
		{
			return this.OutfitDataBase[outfitName];
		}

		// Token: 0x060037E6 RID: 14310 RVA: 0x0014712F File Offset: 0x0014532F
		[ContextMenu("Save")]
		private IEnumerator Save()
		{
			yield return new WaitForEndOfFrame();
			if (this.CharacterName.text.Length == 0)
			{
				Debug.LogWarning("Please enter in a name with at least one letter");
				yield break;
			}
			RenderTexture.active = this.iconTexture;
			Texture2D texture2D = new Texture2D(this.iconTexture.width, this.iconTexture.height, TextureFormat.RGBA32, false);
			Rect rect = new Rect(new Rect(0f, 0f, (float)this.iconTexture.width, (float)this.iconTexture.height));
			texture2D.ReadPixels(rect, 0, 0);
			texture2D.Apply();
			byte[] array = texture2D.EncodeToPNG();
			Texture2D texture2D2 = null;
			GameDataPaths.EnsureCharacterSaveDirectories();
			File.WriteAllBytes(Path.Combine(BMAC_SaveSystem.iconFilePath, this.CharacterName.text + ".png"), array);
			BMAC_SaveSystem.SaveCharacter(this.character, this.CharacterName.text, texture2D2, null);
			this.UpdateCharacterSaves();
			yield break;
		}

		// Token: 0x060037E7 RID: 14311 RVA: 0x00147140 File Offset: 0x00145340
		public async void LoadCharacter(CharacterData data, string storageKey)
		{
			this.loadedCharacterNameText.text = data.characterName;
			this.loadedCharacterStorageKey = storageKey;
			await BMAC_SaveSystem.LoadCharacter(this.character, data, false, true);
		}

		// Token: 0x060037E8 RID: 14312 RVA: 0x00147188 File Offset: 0x00145388
		public void DeleteCharacter()
		{
			if (this.loadedCharacterNameText.text == "")
			{
				return;
			}
			LocalizedGameText localizedGameText = LocalizedGameText.BindFormat(this.DeleteCharacterNameText, "cielos_apps", "Delete: {0}", new object[] { this.loadedCharacterNameText.text });
			if (localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
			this.DeleteConfirmWindow.SetActive(true);
		}

		// Token: 0x060037E9 RID: 14313 RVA: 0x001471EE File Offset: 0x001453EE
		public void ConfirmDelete()
		{
			if (string.IsNullOrWhiteSpace(this.loadedCharacterStorageKey))
			{
				return;
			}
			BMAC_SaveSystem.DeleteCharacter(this.loadedCharacterStorageKey);
			this.loadedCharacterNameText.text = "";
			this.loadedCharacterStorageKey = null;
			this.UpdateCharacterSaves();
		}

		// Token: 0x0400388C RID: 14476
		[Header("Creator Dependencies")]
		public OutfitSystem character;

		// Token: 0x0400388D RID: 14477
		[SerializeField]
		private ColorPickerControl colorPickerControl;

		// Token: 0x0400388E RID: 14478
		[SerializeField]
		private CharacterSpinner Spinner;

		// Token: 0x0400388F RID: 14479
		[SerializeField]
		private Camera iconCamera;

		// Token: 0x04003890 RID: 14480
		[SerializeField]
		private RenderTexture iconTexture;

		// Token: 0x04003891 RID: 14481
		public OutfitType[] outfitTypes;

		// Token: 0x04003892 RID: 14482
		[Header("Outfit Dependencies")]
		private Dictionary<string, Outfit> OutfitDataBase = new Dictionary<string, Outfit>();

		// Token: 0x04003893 RID: 14483
		[SerializeField]
		private OutfitSelector outfitSelectorObject;

		// Token: 0x04003894 RID: 14484
		private List<OutfitSelector> outfitSelectors = new List<OutfitSelector>();

		// Token: 0x04003895 RID: 14485
		[SerializeField]
		private Transform outfitContainer;

		// Token: 0x04003896 RID: 14486
		[Header("Texture Dependencies")]
		[SerializeField]
		private TextureSelector textureSelectorObject;

		// Token: 0x04003897 RID: 14487
		private List<TextureSelector> textureSelectors = new List<TextureSelector>();

		// Token: 0x04003898 RID: 14488
		[SerializeField]
		private Transform decalContainer;

		// Token: 0x04003899 RID: 14489
		[SerializeField]
		private Transform patternContainer;

		// Token: 0x0400389A RID: 14490
		[Header("BodyShape Dependencies")]
		[SerializeField]
		private BlendSlider blendSliderObject;

		// Token: 0x0400389B RID: 14491
		private List<BlendSlider> blendSliders = new List<BlendSlider>();

		// Token: 0x0400389C RID: 14492
		private List<BlendSlider> faceBlendSliders = new List<BlendSlider>();

		// Token: 0x0400389D RID: 14493
		[SerializeField]
		private BodyShapeSliders modSliderObject;

		// Token: 0x0400389E RID: 14494
		private List<BodyShapeSliders> ModSliders = new List<BodyShapeSliders>();

		// Token: 0x0400389F RID: 14495
		[SerializeField]
		private Transform bodyShapeContainer;

		// Token: 0x040038A0 RID: 14496
		[SerializeField]
		private Transform bodyModContainer;

		// Token: 0x040038A1 RID: 14497
		[SerializeField]
		private Transform faceShapeContainer;

		// Token: 0x040038A2 RID: 14498
		[SerializeField]
		private Transform faceModContainer;

		// Token: 0x040038A3 RID: 14499
		[SerializeField]
		private GameObject currentPage;

		// Token: 0x040038A4 RID: 14500
		private GameObject previousPage;

		// Token: 0x040038A5 RID: 14501
		private Dictionary<string, List<GameObject>> outfits = new Dictionary<string, List<GameObject>>();

		// Token: 0x040038A6 RID: 14502
		private List<TexturePackage> textures = new List<TexturePackage>();

		// Token: 0x040038A7 RID: 14503
		[Header("Save Dependencies")]
		[SerializeField]
		private SaveSelector saveSelector;

		// Token: 0x040038A8 RID: 14504
		[SerializeField]
		private Dictionary<string, SaveSelector> saveSlots = new Dictionary<string, SaveSelector>();

		// Token: 0x040038A9 RID: 14505
		[SerializeField]
		private Transform saveContainer;

		// Token: 0x040038AA RID: 14506
		[SerializeField]
		private GameObject DeleteConfirmWindow;

		// Token: 0x040038AB RID: 14507
		[SerializeField]
		private TMP_Text loadedCharacterNameText;

		// Token: 0x040038AC RID: 14508
		[SerializeField]
		private TMP_Text DeleteCharacterNameText;

		// Token: 0x040038AD RID: 14509
		private string loadedCharacterStorageKey;

		// Token: 0x040038AE RID: 14510
		private OutfitType type;

		// Token: 0x040038AF RID: 14511
		[Header("Save Options")]
		public TMP_InputField CharacterName;
	}
}
