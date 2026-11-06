using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x0200082C RID: 2092
	public static class BMAC_SaveSystem
	{
		// Token: 0x0600378F RID: 14223 RVA: 0x001445E0 File Offset: 0x001427E0
		public static void SaveCharacter(OutfitSystem outfitSystem, string saveName, Texture2D icon = null, CharacterGeneralInfo generalInfo = null)
		{
			GameDataPaths.EnsureCharacterSaveDirectories();
			CharacterData characterData = BMAC_SaveSystem.GetCharacterData(outfitSystem);
			characterData.characterName = saveName;
			characterData.generalInfo = generalInfo;
			string text = JsonUtility.ToJson(characterData);
			string text2 = Path.Combine(BMAC_SaveSystem.filePath, saveName + ".json");
			File.WriteAllText(text2, text);
			Debug.Log("Character Saved:" + text2);
		}

		// Token: 0x06003790 RID: 14224 RVA: 0x0014463C File Offset: 0x0014283C
		public static CharacterData GetCharacterData(OutfitSystem outfitSystem)
		{
			if (outfitSystem.mergedMode)
			{
				return outfitSystem.data;
			}
			CharacterData characterData = new CharacterData();
			Dictionary<string, float> bodyShapeValues = outfitSystem.GetBodyShapeValues();
			characterData.bodyIDs = bodyShapeValues.Keys.ToList<string>();
			characterData.bodyShapes = bodyShapeValues.Values.ToList<float>();
			Dictionary<string, float> faceShapeValues = outfitSystem.GetFaceShapeValues();
			characterData.faceIDs = faceShapeValues.Keys.ToList<string>();
			characterData.faceShapes = faceShapeValues.Values.ToList<float>();
			List<BodyModData> list = new List<BodyModData>();
			List<string> list2 = outfitSystem.bodyModifiers.Keys.ToList<string>();
			for (int i = 0; i < list2.Count; i++)
			{
				BodyModData data = outfitSystem.bodyModifiers[list2[i]].GetData();
				list.Add(data);
			}
			characterData.bodyMods = list;
			characterData.bodyModsKeys = list2;
			List<Outfit> outfits = outfitSystem.GetOutfits();
			List<OutfitData> list3 = new List<OutfitData>();
			for (int j = 0; j < outfits.Count; j++)
			{
				if (!(outfits[j] == null))
				{
					list3.Add(outfits[j].GetOutfitData());
				}
			}
			characterData.stance = outfitSystem.stance;
			characterData.outfitDatas = list3;
			return characterData;
		}

		// Token: 0x06003791 RID: 14225 RVA: 0x00144770 File Offset: 0x00142970
		public static async Task LoadCharacter(OutfitSystem outfitSystem, CharacterData characterObject = null, bool manualShapeApply = false, bool async = false)
		{
			if (outfitSystem == null || !outfitSystem)
			{
				Debug.LogError("[BMAC_SaveSystem] LoadCharacter called with NULL or destroyed OutfitSystem!");
			}
			else
			{
				await BMAC_SaveSystem.LoadCharacterInternal(outfitSystem, characterObject, manualShapeApply, async);
			}
		}

		// Token: 0x06003792 RID: 14226 RVA: 0x001447CC File Offset: 0x001429CC
		private static async Task LoadCharacterInternal(OutfitSystem outfitSystem, CharacterData characterObject, bool manualShapeApply, bool async)
		{
			if (outfitSystem == null || !outfitSystem)
			{
				Debug.LogError("[BMAC_SaveSystem] OutfitSystem was destroyed!");
			}
			else
			{
				bool hasBody = false;
				List<Outfit> outfits = new List<Outfit>();
				if (async)
				{
					try
					{
						List<Outfit> list = await BMAC_SaveSystem.LoadOutfits(characterObject.outfitDatas);
						outfits = list;
						if (outfits.Count == 0)
						{
							await BMAC_SaveSystem.WaitForMapCatalog();
						}
						goto IL_01AA;
					}
					catch (Exception ex)
					{
						Debug.LogError("LoadOutfits failed: " + ex.Message);
						goto IL_01AA;
					}
				}
				foreach (OutfitData outfitData in characterObject.outfitDatas)
				{
					outfits.Add(Resources.Load<Outfit>(outfitData.outfit));
				}
				IL_01AA:
				if (outfitSystem == null || !outfitSystem)
				{
					Debug.LogError("[BMAC_SaveSystem] OutfitSystem was destroyed during LoadOutfits!");
				}
				else
				{
					outfitSystem.RemoveAllOutfits();
					for (int i = 0; i < characterObject.outfitDatas.Count; i++)
					{
						if (outfitSystem == null || !outfitSystem)
						{
							Debug.LogError(string.Format("[BMAC_SaveSystem] OutfitSystem destroyed during outfit iteration {0}!", i));
							return;
						}
						OutfitData outfitData2 = characterObject.outfitDatas[i];
						Outfit outfit = outfits[i];
						if (outfit == null)
						{
							Debug.LogWarning("Outfit Path: " + outfitData2.outfit + " returns null make sure Prefab is named correctly");
						}
						else
						{
							Outfit outfit2 = outfitSystem.InstantiateOutfit(outfit);
							if (outfit.Type.name == "Body")
							{
								hasBody = true;
							}
							outfit2.Attach();
							if (outfit2.customShader)
							{
								outfit2.SetSwatch(outfitData2.swatch, false);
								outfit2.SetColor(outfitData2.color);
							}
							else
							{
								for (int j = 0; j < 9; j++)
								{
									if (outfitData2.colors.Count > j)
									{
										outfit2.SetColor(outfitData2.colors[j], j + 1, false);
									}
									if (j + 1 <= 2 && outfitData2.decal != "")
									{
										outfit2.SetDecalColor(outfitData2.decalColors[j], j + 1);
									}
									if (j + 1 <= 2 && outfitData2.pattern != "")
									{
										outfit2.SetPatternColor(outfitData2.patternColors[j], j + 1);
									}
								}
								outfit2.SetDecal(Resources.Load<Texture>(outfitData2.decal));
								outfit2.SetDecalSize(outfitData2.decalScale);
								outfit2.SetPattern(Resources.Load<Texture>(outfitData2.pattern));
								outfit2.SetPatternSize(outfitData2.patternScale);
							}
						}
					}
					if (!hasBody)
					{
						Outfit outfit3 = outfitSystem.GetOutfit("Body");
						if (outfit3 == null)
						{
							outfit3 = outfitSystem.InstantiateOutfit(Resources.Load<Outfit>("Body/Body_BasicBody"));
						}
						for (int k = 0; k < characterObject.bodyData.colors.Count; k++)
						{
							outfit3.SetColor(characterObject.bodyData.colors[k], k + 1, false);
						}
						outfit3.SetDecal(Resources.Load<Texture>(characterObject.bodyData.decal));
						for (int l = 0; l < characterObject.bodyData.decalColors.Count; l++)
						{
							outfit3.SetDecalColor(characterObject.bodyData.decalColors[l], l + 1);
						}
						outfit3.SetPattern(Resources.Load<Texture>(characterObject.bodyData.pattern));
						for (int m = 0; m < characterObject.bodyData.patternColors.Count; m++)
						{
							outfit3.SetPatternColor(characterObject.bodyData.patternColors[m], m + 1);
						}
					}
					for (int n = 0; n < characterObject.bodyIDs.Count; n++)
					{
						outfitSystem.SetShape(characterObject.bodyIDs[n], characterObject.bodyShapes[n]);
					}
					for (int num = 0; num < characterObject.faceIDs.Count; num++)
					{
						outfitSystem.SetShape(characterObject.faceIDs[num], characterObject.faceShapes[num]);
					}
					if (!manualShapeApply)
					{
						BMAC_SaveSystem.LoadBodyMods(outfitSystem, characterObject);
					}
					if (outfitSystem.animator != null)
					{
						outfitSystem.animator.Rebind();
						outfitSystem.SetStance(characterObject.stance);
					}
					else
					{
						Debug.LogWarning("BMAC_SaveSystem: OutfitSystem is missing an Animator reference, skipping Rebind/stance update");
					}
				}
			}
		}

		// Token: 0x06003793 RID: 14227 RVA: 0x00144828 File Offset: 0x00142A28
		public static void LoadBodyMods(OutfitSystem outfitSystem, CharacterData loadData)
		{
			for (int i = 0; i < loadData.bodyModsKeys.Count; i++)
			{
				outfitSystem.bodyModifiers[loadData.bodyModsKeys[i]].SetData(loadData.bodyMods[i]);
			}
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x00144874 File Offset: 0x00142A74
		public static CharacterData GetDataFromID(string saveName)
		{
			GameDataPaths.EnsureCharacterSaveDirectories();
			string text = Path.Combine(BMAC_SaveSystem.filePath, saveName + ".json");
			Debug.Log("Attempted Load at: " + text);
			if (!File.Exists(text))
			{
				Debug.LogWarning("Save ID: " + saveName + " does not exist. Make sure input matches an existing Save");
				return null;
			}
			return JsonUtility.FromJson<CharacterData>(File.ReadAllText(text));
		}

		// Token: 0x06003795 RID: 14229 RVA: 0x001448D8 File Offset: 0x00142AD8
		public static async Task<List<Outfit>> LoadOutfits(List<OutfitData> outfitDatas)
		{
			return (await Task.WhenAll<Outfit>(outfitDatas.Select<OutfitData, Task<Outfit>>((OutfitData data) => BMAC_SaveSystem.LoadResourceAsync<Outfit>(data.outfit)))).ToList<Outfit>();
		}

		// Token: 0x06003796 RID: 14230 RVA: 0x0014491C File Offset: 0x00142B1C
		private static async Task WaitForMapCatalog()
		{
			while (ChunkManager.Instance != null && ChunkManager.Instance.IsAwaitingCatalog)
			{
				await Task.Yield();
			}
		}

		// Token: 0x06003797 RID: 14231 RVA: 0x00144958 File Offset: 0x00142B58
		public static async Task<T> LoadResourceAsync<T>(string path) where T : Object
		{
			ResourceRequest request = Resources.LoadAsync<T>(path);
			TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
			request.completed += delegate(AsyncOperation operation)
			{
				if (request.asset == null)
				{
					tcs.SetResult(default(T));
					return;
				}
				T t = request.asset as T;
				if (t == null)
				{
					tcs.SetResult(default(T));
					return;
				}
				tcs.SetResult(t);
			};
			return await tcs.Task;
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x0014499C File Offset: 0x00142B9C
		public static void DeleteCharacter(string characterName)
		{
			GameDataPaths.EnsureCharacterSaveDirectories();
			File.Delete(Path.Combine(BMAC_SaveSystem.filePath, characterName + ".json"));
			File.Delete(Path.Combine(BMAC_SaveSystem.iconFilePath, characterName + ".png"));
			File.Delete(BMAC_SaveSystem.assetPath + "/" + characterName + ".asset");
			File.Delete(BMAC_SaveSystem.assetPath + "/" + characterName + ".meta");
			File.Delete(BMAC_SaveSystem.iconAssetPath + characterName + ".png");
			File.Delete(BMAC_SaveSystem.iconAssetPath + characterName + ".meta");
		}

		// Token: 0x04003824 RID: 14372
		public static string filePath = GameDataPaths.CharacterSavesDirectory;

		// Token: 0x04003825 RID: 14373
		public static string iconFilePath = GameDataPaths.CharacterSaveIconsDirectory;

		// Token: 0x04003826 RID: 14374
		public static string assetPath = "Assets/Features/CharacterCustomization/BoZo_ModularAnimeCharacters/CharacterSaves/Resources/";

		// Token: 0x04003827 RID: 14375
		public static string iconAssetPath = "Assets/Features/CharacterCustomization/BoZo_ModularAnimeCharacters/CharacterSaves/Icons/";

		// Token: 0x04003828 RID: 14376
		private static readonly SemaphoreSlim _loadCharacterSemaphore = new SemaphoreSlim(1, 1);
	}
}
