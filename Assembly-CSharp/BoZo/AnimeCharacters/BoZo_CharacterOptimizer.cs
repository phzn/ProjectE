using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Features.NPCs;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Bozo.AnimeCharacters
{
	// Token: 0x0200081A RID: 2074
	public class BoZo_CharacterOptimizer
	{
		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x06003760 RID: 14176 RVA: 0x00140240 File Offset: 0x0013E440
		private static string path
		{
			get
			{
				string text = BoZo_CharacterOptimizer.SavePath ?? string.Empty;
				if (text.Length > 0 && text[0] != '/')
				{
					text = "/" + text;
				}
				return text;
			}
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x0014027D File Offset: 0x0013E47D
		private static void LogDebug(string message)
		{
			if (BoZo_CharacterOptimizer._enableDebugLogs)
			{
				Debug.Log(message);
			}
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x0014028C File Offset: 0x0013E48C
		public async void OptimizeCharacter(OutfitSystem source, CharacterData data)
		{
			await this.OptimizeCharacterAsync(source, data);
		}

		// Token: 0x06003763 RID: 14179 RVA: 0x001402D4 File Offset: 0x0013E4D4
		public async Task OptimizeCharacterAsync(OutfitSystem source, CharacterData data)
		{
			int atlasSize = ((source != null) ? source.atlasSize : 2048);
			if (source.mergeMaterial == null)
			{
				Debug.Log("Merge Material required, please assign one in the inspector");
			}
			else
			{
				MergedMaterialData[] localMaterialDatas = ((source.materialData != null) ? ((MergedMaterialData[])source.materialData.Clone()) : new MergedMaterialData[0]);
				List<BoZo_CharacterOptimizer.PropData> propsFromSource = this.CollectPropsFromSource(source);
				await BoZo_CharacterOptimizer._mergeSemaphore.WaitAsync();
				OutfitSystem body = null;
				bool mergeSucceeded = false;
				try
				{
					body = await this.PrepareMergeBase(source, data);
					float height = body.height;
					GameObject gameObject = await this.Merge(body, localMaterialDatas, false, "", atlasSize);
					source.customMaps = body.customMaps;
					Object.Destroy(body.gameObject);
					body = null;
					BMAC_SaveSystem.LoadBodyMods(source, data);
					source.SetCharacterBody(gameObject);
					this.RestorePropsToMergedBody(propsFromSource, source);
					mergeSucceeded = true;
				}
				finally
				{
					BoZo_CharacterOptimizer._mergeSemaphore.Release();
					if (body != null && body.gameObject != null)
					{
						Object.Destroy(body.gameObject);
					}
					if (!mergeSucceeded)
					{
						this.CleanupOrphanedProps(propsFromSource);
					}
				}
			}
		}

		// Token: 0x06003764 RID: 14180 RVA: 0x00140328 File Offset: 0x0013E528
		private void CleanupOrphanedProps(List<BoZo_CharacterOptimizer.PropData> props)
		{
			if (props == null || props.Count == 0)
			{
				return;
			}
			Debug.LogWarning(string.Format("[MERGE] Cleaning up {0} orphaned props after merge failure", props.Count));
			foreach (BoZo_CharacterOptimizer.PropData propData in props)
			{
				if (propData.propObject != null)
				{
					Object.Destroy(propData.propObject);
				}
			}
		}

		// Token: 0x06003765 RID: 14181 RVA: 0x001403B0 File Offset: 0x0013E5B0
		private List<BoZo_CharacterOptimizer.PropData> CollectPropsFromSource(OutfitSystem source)
		{
			List<BoZo_CharacterOptimizer.PropData> list = new List<BoZo_CharacterOptimizer.PropData>();
			if (source == null)
			{
				return list;
			}
			NPCProp[] componentsInChildren = source.GetComponentsInChildren<NPCProp>(true);
			Debug.Log(string.Format("[MERGE] CollectPropsFromSource: Found {0} NPCProp components in source", componentsInChildren.Length));
			foreach (NPCProp npcprop in componentsInChildren)
			{
				if (!(npcprop == null) && !(npcprop.transform.parent == null))
				{
					Transform transform = npcprop.transform;
					Transform parent = transform.parent;
					BoZo_CharacterOptimizer.PropData propData = new BoZo_CharacterOptimizer.PropData
					{
						propObject = transform.gameObject,
						parentBoneName = parent.name,
						localPosition = transform.localPosition,
						localRotation = transform.localRotation,
						localScale = transform.localScale
					};
					list.Add(propData);
					transform.SetParent(null);
					Debug.Log(string.Concat(new string[] { "[MERGE] Collected prop '", npcprop.PropName, "' from bone '", parent.name, "'" }));
				}
			}
			return list;
		}

		// Token: 0x06003766 RID: 14182 RVA: 0x001404D4 File Offset: 0x0013E6D4
		private void RestorePropsToMergedBody(List<BoZo_CharacterOptimizer.PropData> props, OutfitSystem source)
		{
			if (props == null || props.Count == 0 || source == null)
			{
				return;
			}
			Debug.Log("[MERGE] RestorePropsToMergedBody: source = '" + source.name + "'");
			SkinnedMeshRenderer characterBody = source.GetCharacterBody();
			if (characterBody == null)
			{
				Debug.LogError("[MERGE] RestorePropsToMergedBody: No SkinnedMeshRenderer found on source '" + source.name + "'");
				return;
			}
			string text = "[MERGE] RestorePropsToMergedBody: Found SMR on '{0}', bones count: {1}";
			object name = characterBody.gameObject.name;
			Transform[] bones = characterBody.bones;
			Debug.Log(string.Format(text, name, (bones != null) ? bones.Length : 0));
			Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>();
			foreach (Transform transform in characterBody.bones)
			{
				if (transform != null && !dictionary.ContainsKey(transform.name))
				{
					dictionary[transform.name] = transform;
				}
			}
			Debug.Log(string.Format("[MERGE] RestorePropsToMergedBody: Built bone map with {0} bones", dictionary.Count));
			foreach (BoZo_CharacterOptimizer.PropData propData in props)
			{
				Transform transform2;
				if (propData.propObject == null)
				{
					Debug.LogWarning("[MERGE] Prop object is NULL for bone '" + propData.parentBoneName + "'");
				}
				else if (dictionary.TryGetValue(propData.parentBoneName, out transform2))
				{
					propData.propObject.transform.SetParent(transform2);
					propData.propObject.transform.localPosition = propData.localPosition;
					propData.propObject.transform.localRotation = propData.localRotation;
					propData.propObject.transform.localScale = propData.localScale;
					Debug.Log(string.Concat(new string[]
					{
						"[MERGE] Restored prop '",
						propData.propObject.name,
						"' to bone '",
						transform2.name,
						"'"
					}));
				}
				else
				{
					propData.propObject.transform.SetParent(characterBody.rootBone ?? source.transform);
					propData.propObject.transform.localPosition = propData.localPosition;
					propData.propObject.transform.localRotation = propData.localRotation;
					propData.propObject.transform.localScale = propData.localScale;
					Debug.LogWarning(string.Concat(new string[]
					{
						"[MERGE] Prop '",
						propData.propObject.name,
						"' bone '",
						propData.parentBoneName,
						"' not found, parented to root"
					}));
				}
			}
			Debug.Log(string.Format("[MERGE] Restored {0} props to merged body", props.Count));
		}

		// Token: 0x06003767 RID: 14183 RVA: 0x001407C4 File Offset: 0x0013E9C4
		public async void SaveOptimizedCharacter(OutfitSystem source, CharacterData data)
		{
			int atlasSize = ((source != null) ? source.atlasSize : 2048);
			MergedMaterialData[] localMaterialDatas = ((source.materialData != null) ? ((MergedMaterialData[])source.materialData.Clone()) : new MergedMaterialData[0]);
			await BoZo_CharacterOptimizer._mergeSemaphore.WaitAsync();
			OutfitSystem body = null;
			try
			{
				body = await this.PrepareMergeBase(source, data);
				float height = body.height;
				body.data = data;
				GameObject gameObject = await this.Merge(body, localMaterialDatas, true, source.prefabName, atlasSize);
				Object.Destroy(body.gameObject);
				body = null;
				BMAC_SaveSystem.LoadBodyMods(source, data);
				source.SetCharacterBody(gameObject);
				source.MuteHeightChange(false);
			}
			finally
			{
				BoZo_CharacterOptimizer._mergeSemaphore.Release();
				if (body != null && body.gameObject != null)
				{
					Object.Destroy(body.gameObject);
				}
			}
		}

		// Token: 0x06003768 RID: 14184 RVA: 0x0014080C File Offset: 0x0013EA0C
		private async Task<OutfitSystem> PrepareMergeBase(OutfitSystem source, CharacterData data)
		{
			OutfitSystem outfitSystem = Resources.Load<OutfitSystem>("BMAC_MergedCharacterBase");
			OutfitSystem body = Object.Instantiate<OutfitSystem>(outfitSystem);
			body.mergeBase = true;
			body.MuteHeightChange(true);
			body.animator.enabled = false;
			await BMAC_SaveSystem.LoadCharacter(body, data, true, true);
			body.mergeMaterial = source.mergeMaterial;
			return body;
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x00140858 File Offset: 0x0013EA58
		public async Task<GameObject> Merge(OutfitSystem outfitSystem, MergedMaterialData[] materialDatas, bool saveAsPrefab = false, string saveName = "", int atlasSize = 2048)
		{
			GameObject gameObject;
			if (!Application.isPlaying)
			{
				gameObject = null;
			}
			else
			{
				List<Outfit> outfitsToMerge = outfitSystem.GetOutfits();
				SkinnedMeshRenderer rig = outfitSystem.GetCharacterBody();
				Material mergeMaterial = outfitSystem.mergeMaterial;
				List<Outfit> list = new List<Outfit>();
				if (outfitsToMerge != null)
				{
					for (int i = outfitsToMerge.Count - 1; i >= 0; i--)
					{
						Outfit outfit = outfitsToMerge[i];
						if (outfit != null && outfit.skipFromMerge)
						{
							list.Add(outfit);
							outfitsToMerge.RemoveAt(i);
						}
					}
				}
				if (outfitsToMerge == null || outfitsToMerge.Count == 0)
				{
					Debug.LogError("No Skinned Mesh Renderers assigned.");
					gameObject = null;
				}
				else
				{
					List<Transform> masterBones = rig.bones.ToList<Transform>();
					Transform rootBone = rig.rootBone;
					Transform parent = rig.transform.parent;
					Dictionary<string, BoZo_CharacterOptimizer.boneData> boneMap = new Dictionary<string, BoZo_CharacterOptimizer.boneData>();
					for (int j = 0; j < masterBones.Count; j++)
					{
						BoZo_CharacterOptimizer.boneData boneData = new BoZo_CharacterOptimizer.boneData();
						boneData.bone = masterBones[j];
						boneData.index = j;
						boneMap.Add(masterBones[j].name, boneData);
					}
					bool MergeMaterials = true;
					List<Vector3> vertices = new List<Vector3>();
					List<Vector3> normals = new List<Vector3>();
					List<Vector4> tangents = new List<Vector4>();
					List<Vector2> uv = new List<Vector2>();
					List<Vector2> uv2 = new List<Vector2>();
					List<Color> colors = new List<Color>();
					List<BoneWeight> boneWeights = new List<BoneWeight>();
					List<Material> materials = new List<Material>();
					List<Matrix4x4> bindposes = new List<Matrix4x4>();
					Dictionary<string, List<BoZo_CharacterOptimizer.BlendshapeData>> blendshapeGroups = new Dictionary<string, List<BoZo_CharacterOptimizer.BlendshapeData>>();
					int vertexOffset = 0;
					List<List<int>> submeshTriangles = new List<List<int>>();
					Texture2D mergedTexture = new Texture2D(2, 2);
					Dictionary<string, Rect> dictionary = new Dictionary<string, Rect>();
					Material newMaterial = new Material(mergeMaterial);
					newMaterial.mainTexture = null;
					List<Renderer> rendererList = new List<Renderer>();
					foreach (Outfit outfit2 in outfitsToMerge)
					{
						rendererList.AddRange(outfit2.GetComponentsInChildren<Renderer>());
						Animator componentInChildren = outfit2.GetComponentInChildren<Animator>();
						if (componentInChildren != null)
						{
							componentInChildren.enabled = false;
						}
					}
					foreach (Renderer renderer in rendererList)
					{
						renderer.enabled = false;
					}
					ValueTuple<Texture2D, Dictionary<string, Texture2D>, Dictionary<string, Rect>, Dictionary<string, Texture2D>> valueTuple = await this.CreateMergedTextures(outfitsToMerge, materialDatas, atlasSize);
					newMaterial.mainTexture = valueTuple.Item1;
					foreach (MergedMaterialData mergedMaterialData in materialDatas)
					{
						newMaterial.SetTexture(mergedMaterialData.toMateiralProperty, valueTuple.Item2[mergedMaterialData.toMateiralProperty]);
					}
					dictionary = valueTuple.Item3;
					outfitSystem.customMaps = valueTuple.Item4;
					for (int l = 0; l < rendererList.Count; l++)
					{
						BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] ========== Processing renderer {0}: {1} ==========", l, rendererList[l].name));
						SkinnedMeshRenderer skinnedMeshRenderer = rendererList[l].GetComponentInChildren<SkinnedMeshRenderer>(true);
						Outfit componentInParent = rendererList[l].GetComponentInParent<Outfit>(true);
						BoZo_CharacterOptimizer.LogDebug(string.Concat(new string[]
						{
							"[MERGE DEBUG] Outfit: ",
							(componentInParent != null) ? componentInParent.name : null,
							", AttachPoint: '",
							(componentInParent != null) ? componentInParent.AttachPoint : null,
							"'"
						}));
						BoZo_CharacterOptimizer.LogDebug("[MERGE DEBUG] SMR before conversion: " + ((skinnedMeshRenderer != null) ? skinnedMeshRenderer.name : "NULL"));
						BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] Renderer transform position: {0}, rotation: {1}", rendererList[l].transform.position, rendererList[l].transform.rotation.eulerAngles));
						if (skinnedMeshRenderer == null)
						{
							BoZo_CharacterOptimizer.LogDebug("[MERGE DEBUG] Converting MeshRenderer to SkinnedMeshRenderer");
							Mesh sharedMesh = rendererList[l].GetComponentInChildren<MeshFilter>(true).sharedMesh;
							MeshRenderer componentInChildren2 = rendererList[l].GetComponentInChildren<MeshRenderer>(true);
							Material sharedMaterial = componentInChildren2.sharedMaterial;
							GameObject gameObject2 = componentInChildren2.gameObject;
							BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] MeshRenderer GO position: {0}, localPosition: {1}", gameObject2.transform.position, gameObject2.transform.localPosition));
							string text = "[MERGE DEBUG] MeshRenderer GO parent: ";
							Transform parent2 = gameObject2.transform.parent;
							BoZo_CharacterOptimizer.LogDebug(text + ((parent2 != null) ? parent2.name : null));
							Object.DestroyImmediate(componentInChildren2);
							Object.DestroyImmediate(gameObject2.GetComponent<MeshFilter>());
							skinnedMeshRenderer = gameObject2.AddComponent<SkinnedMeshRenderer>();
							skinnedMeshRenderer.sharedMaterial = sharedMaterial;
							skinnedMeshRenderer.sharedMesh = sharedMesh;
							string text2 = "[MERGE DEBUG] After conversion - SMR rootBone: {0}, bones count: {1}";
							object obj = ((skinnedMeshRenderer.rootBone != null) ? skinnedMeshRenderer.rootBone.name : "NULL");
							Transform[] bones = skinnedMeshRenderer.bones;
							BoZo_CharacterOptimizer.LogDebug(string.Format(text2, obj, (bones != null) ? bones.Length : 0));
						}
						Mesh sharedMesh2 = skinnedMeshRenderer.sharedMesh;
						int num = 0;
						bool flag = true;
						Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
						BoZo_CharacterOptimizer.LogDebug("[MERGE DEBUG] SMR rootBone: " + ((skinnedMeshRenderer.rootBone != null) ? skinnedMeshRenderer.rootBone.name : "NULL"));
						if (skinnedMeshRenderer.rootBone != null)
						{
							BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] rootBone '{0}' in boneMap: {1}", skinnedMeshRenderer.rootBone.name, boneMap.ContainsKey(skinnedMeshRenderer.rootBone.name)));
							if (!boneMap.ContainsKey(skinnedMeshRenderer.rootBone.name))
							{
								num = masterBones.Count;
								BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] SEPARATE SKELETON - boneIndexOffset: {0}", num));
								if (componentInParent.AttachPoint == "")
								{
									Debug.Log("What are you doing here? Stop the show...");
									return null;
								}
								BoZo_CharacterOptimizer.LogDebug("[MERGE DEBUG] Setting rootBone to AttachPoint bone: " + componentInParent.AttachPoint);
								BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] AttachPoint bone position: {0}", boneMap[componentInParent.AttachPoint].bone.position));
								skinnedMeshRenderer.rootBone = boneMap[componentInParent.AttachPoint].bone;
								masterBones.AddRange(skinnedMeshRenderer.bones);
								skinnedMeshRenderer.bones[0].SetParent(boneMap[componentInParent.AttachPoint].bone);
								BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] After SetParent - bones[0] position: {0}, localPosition: {1}", skinnedMeshRenderer.bones[0].position, skinnedMeshRenderer.bones[0].localPosition));
								for (int m = 0; m < skinnedMeshRenderer.bones.Length; m++)
								{
									int num2 = 1;
									BoZo_CharacterOptimizer.boneData boneData2 = new BoZo_CharacterOptimizer.boneData();
									boneData2.bone = skinnedMeshRenderer.bones[m];
									boneData2.index = m + num;
									try
									{
										boneMap.Add(skinnedMeshRenderer.bones[m].name, boneData2);
									}
									catch
									{
										Debug.LogWarning(string.Concat(new string[]
										{
											"Duplicate bone naming in: <",
											componentInParent.name,
											"> <",
											skinnedMeshRenderer.bones[m].name,
											">Please give bone a unique name"
										}));
										boneMap.Add(skinnedMeshRenderer.bones[m].name + num2.ToString(), boneData2);
									}
								}
							}
							else
							{
								BoZo_CharacterOptimizer.LogDebug("[MERGE DEBUG] SHARED SKELETON - using main rig bones");
							}
						}
						Transform[] bones2 = skinnedMeshRenderer.bones;
						if (skinnedMeshRenderer.rootBone != null)
						{
							BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] Building bone remap table for {0} bones", bones2.Length));
							for (int n = 0; n < bones2.Length; n++)
							{
								Transform transform = bones2[n];
								int num3 = -1;
								if (boneMap.ContainsKey(transform.name))
								{
									num3 = boneMap[transform.name].index;
								}
								else
								{
									Transform transform2 = transform.parent;
									while (transform2 != null)
									{
										if (boneMap.ContainsKey(transform2.name))
										{
											num3 = boneMap[transform2.name].index;
											break;
										}
										transform2 = transform2.parent;
									}
								}
								if (num3 != -1)
								{
									dictionary2[n] = num3;
								}
							}
							BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] boneIndexMap has {0} entries", dictionary2.Count));
						}
						else
						{
							BoZo_CharacterOptimizer.LogDebug("[MERGE DEBUG] NO SKELETON - hasSkeleton = false");
							flag = false;
							int index = boneMap[componentInParent.AttachPoint].index;
							dictionary2[0] = index;
							BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] AttachPoint '{0}' masterIndex: {1}", componentInParent.AttachPoint, index));
							BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] AttachPoint bone position: {0}", boneMap[componentInParent.AttachPoint].bone.position));
							skinnedMeshRenderer.bones = masterBones.ToArray();
							skinnedMeshRenderer.rootBone = rootBone;
							BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] Assigned {0} masterBones and rootBone '{1}'", masterBones.Count, rootBone.name));
						}
						Matrix4x4 localToWorldMatrix = skinnedMeshRenderer.transform.localToWorldMatrix;
						BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] hasSkeleton: {0}", flag));
						BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] SMR transform position: {0}, localPosition: {1}", skinnedMeshRenderer.transform.position, skinnedMeshRenderer.transform.localPosition));
						string text3 = "[MERGE DEBUG] SMR transform parent: ";
						Transform parent3 = skinnedMeshRenderer.transform.parent;
						BoZo_CharacterOptimizer.LogDebug(text3 + ((parent3 != null) ? parent3.name : null));
						BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] localToWorld matrix: {0}", localToWorldMatrix));
						if (!flag && !string.IsNullOrEmpty(componentInParent.AttachPoint))
						{
							int index2 = boneMap[componentInParent.AttachPoint].index;
							Transform bone = boneMap[componentInParent.AttachPoint].bone;
							BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] AttachBone '{0}' index: {1}, position: {2}", bone.name, index2, bone.position));
							BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] AttachBone worldToLocalMatrix: {0}", bone.worldToLocalMatrix));
							BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] rootBone '{0}' position: {1}", rootBone.name, rootBone.position));
							BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] rootBone localToWorldMatrix: {0}", rootBone.localToWorldMatrix));
							BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] Expected bindpose for attachBone: {0}", bone.worldToLocalMatrix * rootBone.localToWorldMatrix));
						}
						Vector3[] array = new Vector3[sharedMesh2.vertexCount];
						Vector3[] array2 = new Vector3[sharedMesh2.vertexCount];
						Vector4[] array3 = new Vector4[sharedMesh2.vertexCount];
						Vector3[] vertices2 = sharedMesh2.vertices;
						Vector3[] normals2 = sharedMesh2.normals;
						Vector4[] tangents2 = sharedMesh2.tangents;
						bool flag2 = flag && sharedMesh2.boneWeights != null && sharedMesh2.boneWeights.Length == sharedMesh2.vertexCount;
						Matrix4x4[] array4 = (flag2 ? sharedMesh2.bindposes : null);
						Transform[] array5 = (flag2 ? skinnedMeshRenderer.bones : null);
						BoneWeight[] array6 = (flag2 ? sharedMesh2.boneWeights : null);
						for (int num4 = 0; num4 < sharedMesh2.vertexCount; num4++)
						{
							if (flag2)
							{
								Vector3 vector = Vector3.zero;
								Vector3 vector2 = Vector3.zero;
								Vector3 vector3 = Vector3.zero;
								BoneWeight boneWeight = array6[num4];
								for (int num5 = 0; num5 < 4; num5++)
								{
									float num6;
									int num7;
									switch (num5)
									{
									case 0:
										num6 = boneWeight.weight0;
										num7 = boneWeight.boneIndex0;
										break;
									case 1:
										num6 = boneWeight.weight1;
										num7 = boneWeight.boneIndex1;
										break;
									case 2:
										num6 = boneWeight.weight2;
										num7 = boneWeight.boneIndex2;
										break;
									default:
										num6 = boneWeight.weight3;
										num7 = boneWeight.boneIndex3;
										break;
									}
									if (num6 > 0f && num7 >= 0 && num7 < array5.Length && num7 < array4.Length)
									{
										Transform transform3 = array5[num7];
										if (!(transform3 == null))
										{
											Matrix4x4 matrix4x = transform3.localToWorldMatrix * array4[num7];
											vector += num6 * matrix4x.MultiplyPoint3x4(vertices2[num4]);
											vector2 += num6 * matrix4x.MultiplyVector(normals2[num4]);
											Vector3 vector4 = new Vector3(tangents2[num4].x, tangents2[num4].y, tangents2[num4].z);
											vector3 += num6 * matrix4x.MultiplyVector(vector4);
										}
									}
								}
								array[num4] = vector;
								array2[num4] = ((vector2.sqrMagnitude > 1E-12f) ? vector2.normalized : Vector3.up);
								Vector3 vector5 = ((vector3.sqrMagnitude > 1E-12f) ? vector3.normalized : Vector3.right);
								array3[num4] = new Vector4(vector5.x, vector5.y, vector5.z, tangents2[num4].w);
							}
							else
							{
								array[num4] = localToWorldMatrix.MultiplyPoint3x4(vertices2[num4]);
								array2[num4] = localToWorldMatrix.MultiplyVector(normals2[num4]).normalized;
								Vector3 normalized = localToWorldMatrix.MultiplyVector(new Vector3(tangents2[num4].x, tangents2[num4].y, tangents2[num4].z)).normalized;
								array3[num4] = new Vector4(normalized.x, normalized.y, normalized.z, tangents2[num4].w);
							}
						}
						vertices.AddRange(array);
						normals.AddRange(array2);
						tangents.AddRange(array3);
						if (MergeMaterials)
						{
							Vector2[] uv3 = sharedMesh2.uv;
							Vector2[] array7 = new Vector2[uv3.Length];
							Rect rect = dictionary[componentInParent.name];
							for (int num8 = 0; num8 < uv3.Length; num8++)
							{
								Vector2 vector6 = uv3[num8];
								array7[num8] = new Vector2(rect.x + vector6.x * rect.width, rect.y + vector6.y * rect.height);
							}
							uv.AddRange(array7);
						}
						else
						{
							uv.AddRange(sharedMesh2.uv);
							if (sharedMesh2.uv2 != null && sharedMesh2.uv2.Length == sharedMesh2.vertexCount)
							{
								uv2.AddRange(sharedMesh2.uv2);
							}
							else
							{
								uv2.AddRange(new Vector2[sharedMesh2.vertexCount]);
							}
						}
						Color[] colors2 = sharedMesh2.colors;
						if (colors2 != null && colors2.Length == sharedMesh2.vertexCount)
						{
							colors.AddRange(colors2);
						}
						else
						{
							for (int num9 = 0; num9 < sharedMesh2.vertexCount; num9++)
							{
								colors.Add(Color.white);
							}
						}
						BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] Processing bone weights - hasSkeleton: {0}", flag));
						if (flag)
						{
							BoneWeight[] boneWeights2 = sharedMesh2.boneWeights;
							BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] Remapping {0} bone weights", boneWeights2.Length));
							foreach (BoneWeight boneWeight2 in boneWeights2)
							{
								if (!dictionary2.ContainsKey(boneWeight2.boneIndex0))
								{
									int num10 = 0;
									BoZo_CharacterOptimizer.boneData boneData3;
									if (skinnedMeshRenderer.rootBone != null && boneMap.TryGetValue(skinnedMeshRenderer.rootBone.name, out boneData3))
									{
										num10 = boneData3.index;
									}
									boneWeights.Add(new BoneWeight
									{
										boneIndex0 = num10,
										weight0 = 1f
									});
								}
								else
								{
									int num11 = dictionary2[boneWeight2.boneIndex0];
									int num13;
									int num12 = (dictionary2.TryGetValue(boneWeight2.boneIndex1, out num13) ? num13 : num11);
									int num15;
									int num14 = (dictionary2.TryGetValue(boneWeight2.boneIndex2, out num15) ? num15 : num11);
									int num17;
									int num16 = (dictionary2.TryGetValue(boneWeight2.boneIndex3, out num17) ? num17 : num11);
									boneWeights.Add(new BoneWeight
									{
										boneIndex0 = num11,
										boneIndex1 = num12,
										boneIndex2 = num14,
										boneIndex3 = num16,
										weight0 = boneWeight2.weight0,
										weight1 = boneWeight2.weight1,
										weight2 = boneWeight2.weight2,
										weight3 = boneWeight2.weight3
									});
								}
							}
						}
						else
						{
							BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] Creating {0} new bone weights for static mesh", sharedMesh2.vertexCount));
							BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] boneIndexMap[0] = {0}", dictionary2[0]));
							foreach (BoneWeight boneWeight3 in new BoneWeight[sharedMesh2.vertexCount])
							{
								boneWeights.Add(new BoneWeight
								{
									boneIndex0 = dictionary2[boneWeight3.boneIndex0],
									weight0 = 1f
								});
							}
						}
						if (MergeMaterials)
						{
							for (int num18 = 0; num18 < sharedMesh2.subMeshCount; num18++)
							{
								List<int> list2 = sharedMesh2.GetTriangles(num18).ToList<int>();
								for (int num19 = 0; num19 < list2.Count; num19++)
								{
									list2[num19] += vertexOffset;
								}
								if (submeshTriangles.Count == 0)
								{
									submeshTriangles.Add(list2);
								}
								else
								{
									submeshTriangles[0].AddRange(list2);
								}
							}
						}
						else
						{
							for (int num20 = 0; num20 < sharedMesh2.subMeshCount; num20++)
							{
								List<int> list3 = sharedMesh2.GetTriangles(num20).ToList<int>();
								for (int num21 = 0; num21 < list3.Count; num21++)
								{
									list3[num21] += vertexOffset;
								}
								submeshTriangles.Add(list3);
								materials.Add(skinnedMeshRenderer.sharedMaterials[num20]);
							}
						}
						int blendShapeCount = sharedMesh2.blendShapeCount;
						for (int num22 = 0; num22 < blendShapeCount; num22++)
						{
							string text4 = sharedMesh2.GetBlendShapeName(num22);
							string[] array9 = text4.Split(".", StringSplitOptions.None);
							if (array9.Length != 0)
							{
								text4 = text4.Replace(array9[0] + ".", "");
							}
							int blendShapeFrameCount = sharedMesh2.GetBlendShapeFrameCount(num22);
							for (int num23 = 0; num23 < blendShapeFrameCount; num23++)
							{
								float blendShapeFrameWeight = sharedMesh2.GetBlendShapeFrameWeight(num22, num23);
								float blendShapeWeight = skinnedMeshRenderer.GetBlendShapeWeight(num22);
								Vector3[] array10 = new Vector3[sharedMesh2.vertexCount];
								Vector3[] array11 = new Vector3[sharedMesh2.vertexCount];
								Vector3[] array12 = new Vector3[sharedMesh2.vertexCount];
								sharedMesh2.GetBlendShapeFrameVertices(num22, num23, array10, array11, array12);
								if (!blendshapeGroups.ContainsKey(text4))
								{
									blendshapeGroups[text4] = new List<BoZo_CharacterOptimizer.BlendshapeData>();
								}
								blendshapeGroups[text4].Add(new BoZo_CharacterOptimizer.BlendshapeData
								{
									name = text4,
									weight = blendShapeFrameWeight,
									currentWeight = blendShapeWeight,
									deltaVertices = array10,
									deltaNormals = array11,
									deltaTangents = array12,
									vertexOffset = vertexOffset
								});
							}
						}
						vertexOffset += sharedMesh2.vertexCount;
					}
					foreach (Outfit outfit3 in outfitsToMerge)
					{
						Object.DestroyImmediate(outfit3.gameObject);
					}
					Object.DestroyImmediate(rig.gameObject);
					BoZo_CharacterOptimizer.LogDebug("[MERGE DEBUG] ========== CREATING FINAL MESH ==========");
					BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] Total vertices: {0}, Total boneWeights: {1}", vertices.Count, boneWeights.Count));
					BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] masterBones count: {0}", masterBones.Count));
					BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] rootBone: {0}, position: {1}", rootBone.name, rootBone.position));
					for (int num24 = 0; num24 < masterBones.Count; num24++)
					{
						bindposes.Add(masterBones[num24].worldToLocalMatrix * rootBone.localToWorldMatrix);
						if (num24 < 10 || masterBones[num24].name.Contains("head") || masterBones[num24].name.Contains("Head"))
						{
							BoZo_CharacterOptimizer.LogDebug(string.Format("[MERGE DEBUG] Bindpose[{0}] for bone '{1}': position={2}", num24, masterBones[num24].name, masterBones[num24].position));
						}
					}
					Mesh mesh = new Mesh();
					mesh.name = "CombinedSkinnedMesh";
					mesh.SetVertices(vertices);
					mesh.SetNormals(normals);
					mesh.SetTangents(tangents);
					mesh.SetUVs(0, uv);
					mesh.SetUVs(1, uv2);
					mesh.SetColors(colors);
					mesh.boneWeights = boneWeights.ToArray();
					mesh.bindposes = bindposes.ToArray();
					mesh.subMeshCount = submeshTriangles.Count;
					for (int num25 = 0; num25 < submeshTriangles.Count; num25++)
					{
						mesh.SetTriangles(submeshTriangles[num25], num25);
					}
					foreach (KeyValuePair<string, List<BoZo_CharacterOptimizer.BlendshapeData>> keyValuePair in blendshapeGroups)
					{
						string key = keyValuePair.Key;
						List<BoZo_CharacterOptimizer.BlendshapeData> value = keyValuePair.Value;
						float weight = value[0].weight;
						int vertexCount = mesh.vertexCount;
						Vector3[] array13 = new Vector3[vertexCount];
						Vector3[] array14 = new Vector3[vertexCount];
						Vector3[] array15 = new Vector3[vertexCount];
						foreach (BoZo_CharacterOptimizer.BlendshapeData blendshapeData in value)
						{
							for (int num26 = 0; num26 < blendshapeData.deltaVertices.Length; num26++)
							{
								array13[blendshapeData.vertexOffset + num26] = blendshapeData.deltaVertices[num26];
								array14[blendshapeData.vertexOffset + num26] = blendshapeData.deltaNormals[num26];
								array15[blendshapeData.vertexOffset + num26] = blendshapeData.deltaTangents[num26];
							}
						}
						mesh.AddBlendShapeFrame(key, weight, array13, array14, array15);
					}
					SkinnedMeshRenderer skinnedMeshRenderer2 = new GameObject("CombinedSkinnedMesh").AddComponent<SkinnedMeshRenderer>();
					skinnedMeshRenderer2.sharedMesh = mesh;
					skinnedMeshRenderer2.bones = masterBones.ToArray();
					skinnedMeshRenderer2.rootBone = rootBone;
					if (mergedTexture)
					{
						skinnedMeshRenderer2.material = newMaterial;
					}
					else
					{
						skinnedMeshRenderer2.materials = materials.ToArray();
					}
					foreach (string text5 in blendshapeGroups.Keys)
					{
						BoZo_CharacterOptimizer.BlendshapeData blendshapeData2 = blendshapeGroups[text5][0];
						skinnedMeshRenderer2.SetBlendShapeWeight(skinnedMeshRenderer2.sharedMesh.GetBlendShapeIndex(blendshapeData2.name), blendshapeData2.currentWeight);
					}
					skinnedMeshRenderer2.transform.parent = parent;
					Debug.Log("Dynamic skinning merge complete with bone remapping!");
					gameObject = parent.gameObject;
				}
			}
			return gameObject;
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x001408B4 File Offset: 0x0013EAB4
		[return: TupleElementNames(new string[] { "texture", "additionalMaps", "rect", "customMaps" })]
		public async Task<ValueTuple<Texture2D, Dictionary<string, Texture2D>, Dictionary<string, Rect>, Dictionary<string, Texture2D>>> CreateMergedTextures(List<Outfit> outfits, MergedMaterialData[] materialDatas, int atlasSize = 2048)
		{
			List<Texture2D> diffuseMaps = new List<Texture2D>();
			List<Texture2D> temporaryTextures = new List<Texture2D>();
			List<Texture2D> resultTextures = new List<Texture2D>();
			bool resultsTransferred = false;
			Dictionary<string, Texture2D[]> additionalMaps = new Dictionary<string, Texture2D[]>();
			Dictionary<string, Texture2D[]> customMaps = new Dictionary<string, Texture2D[]>();
			Material bakeMaterial = new Material(Shader.Find("BoZo/BakeTexture"));
			ValueTuple<Texture2D, Dictionary<string, Texture2D>, Dictionary<string, Rect>, Dictionary<string, Texture2D>> valueTuple;
			try
			{
				int index = 0;
				foreach (Outfit outfit in outfits)
				{
					SkinnedMeshRenderer componentInChildren = outfit.GetComponentInChildren<SkinnedMeshRenderer>();
					Renderer renderer = outfit.GetComponentInChildren<Renderer>();
					List<Mesh> list = new List<Mesh>();
					foreach (SkinnedMeshRenderer skinnedMeshRenderer in outfit.skinnedRenderers)
					{
						list.Add(skinnedMeshRenderer.sharedMesh);
					}
					if (componentInChildren == null)
					{
						list.Add(outfit.GetComponentInChildren<MeshFilter>(true).sharedMesh);
					}
					Material originalMaterial = renderer.sharedMaterial;
					renderer.sharedMaterial = bakeMaterial;
					try
					{
						if (!outfit.customShader)
						{
							bakeMaterial.mainTexture = originalMaterial.mainTexture;
							bakeMaterial.SetTexture("_DecalMap", originalMaterial.GetTexture("_DecalMap"));
							bakeMaterial.SetFloat("_DecalUVSet", originalMaterial.GetFloat("_DecalUVSet"));
							bakeMaterial.SetFloat("_DecalBlend", originalMaterial.GetFloat("_DecalBlend"));
							bakeMaterial.SetVector("_DecalScale", originalMaterial.GetVector("_DecalScale"));
							bakeMaterial.SetTexture("_PatternMap", originalMaterial.GetTexture("_PatternMap"));
							bakeMaterial.SetFloat("_PatternUVSet", originalMaterial.GetFloat("_PatternUVSet"));
							bakeMaterial.SetFloat("_PatternBlend", originalMaterial.GetFloat("_PatternBlend"));
							bakeMaterial.SetVector("_PatternScale", originalMaterial.GetVector("_PatternScale"));
							for (int j = 0; j < 9; j++)
							{
								bakeMaterial.SetColor("_Color_" + (j + 1).ToString(), originalMaterial.GetColor("_Color_" + (j + 1).ToString()));
								bakeMaterial.SetColor("_Color_" + (j + 1).ToString(), originalMaterial.GetColor("_Color_" + (j + 1).ToString()));
								if (j + 1 <= 3)
								{
									bakeMaterial.SetColor("_DecalColor_" + (j + 1).ToString(), originalMaterial.GetColor("_DecalColor_" + (j + 1).ToString()));
									bakeMaterial.SetColor("_PatternColor_" + (j + 1).ToString(), originalMaterial.GetColor("_PatternColor_" + (j + 1).ToString()));
								}
							}
						}
						else
						{
							diffuseMaps.Add((Texture2D)originalMaterial.mainTexture);
						}
						foreach (MergedMaterialData mergedMaterialData in materialDatas)
						{
							if (!additionalMaps.ContainsKey(mergedMaterialData.toMateiralProperty))
							{
								additionalMaps[mergedMaterialData.toMateiralProperty] = new Texture2D[outfits.Count];
							}
							additionalMaps[mergedMaterialData.toMateiralProperty][index] = (Texture2D)originalMaterial.GetTexture(mergedMaterialData.fromMateiralProperty);
						}
						foreach (IOutfitExtension outfitExtension in outfit.GetComponentsInChildren<IOutfitExtension>())
						{
							if (outfitExtension.GetValue() is Texture2D && !customMaps.ContainsKey(outfitExtension.GetID()))
							{
								customMaps[outfitExtension.GetID()] = new Texture2D[outfits.Count];
							}
							if (outfitExtension.GetValue() is Texture2D)
							{
								customMaps[outfitExtension.GetID()][index] = (Texture2D)outfitExtension.GetValue();
							}
						}
						Texture2D texture2D = await this.BakeTextureAsyncTask(list, bakeMaterial);
						temporaryTextures.Add(texture2D);
						diffuseMaps.Add(texture2D);
					}
					finally
					{
						if (renderer != null)
						{
							renderer.sharedMaterial = originalMaterial;
						}
					}
					RenderTexture.active = null;
					int i = index;
					index = i + 1;
					renderer = null;
					originalMaterial = null;
				}
				List<Outfit>.Enumerator enumerator = default(List<Outfit>.Enumerator);
				Texture2D atlas = new Texture2D(atlasSize, atlasSize);
				resultTextures.Add(atlas);
				Dictionary<string, Texture2D> additionalMapsList = new Dictionary<string, Texture2D>();
				Dictionary<string, Texture2D> atlasCustomMapsList = new Dictionary<string, Texture2D>();
				Rect[] rects = atlas.PackTextures(diffuseMaps.ToArray(), 0, atlasSize);
				Dictionary<string, Rect> rectMap = new Dictionary<string, Rect>();
				for (int k = 0; k < outfits.Count; k++)
				{
					rectMap.Add(outfits[k].name, rects[k]);
				}
				Color32[] array = atlas.GetPixels32();
				array = await this.DilateTextureAsync(atlas, array);
				atlas.SetPixels32(array);
				atlas.Apply();
				foreach (KeyValuePair<string, Texture2D[]> keyValuePair in additionalMaps)
				{
					List<Texture2D> list2 = keyValuePair.Value.ToList<Texture2D>();
					Texture2D texture2D2 = new Texture2D(atlasSize, atlasSize);
					resultTextures.Add(texture2D2);
					Dictionary<string, Texture2D> dictionary = additionalMapsList;
					string text = keyValuePair.Key;
					dictionary[text] = await this.RemapTextureAsync(list2, rects, atlasSize, texture2D2, new Color(1f, 0.73f, 0.73f, 1f));
					dictionary = null;
					text = null;
				}
				Dictionary<string, Texture2D[]>.Enumerator enumerator2 = default(Dictionary<string, Texture2D[]>.Enumerator);
				foreach (KeyValuePair<string, Texture2D[]> keyValuePair2 in customMaps)
				{
					List<Texture2D> list3 = keyValuePair2.Value.ToList<Texture2D>();
					if (list3.Count != 0)
					{
						Texture2D texture2D3 = new Texture2D(atlasSize, atlasSize);
						resultTextures.Add(texture2D3);
						Dictionary<string, Texture2D> dictionary = atlasCustomMapsList;
						string text = keyValuePair2.Key;
						dictionary[text] = await this.RemapTextureAsync(list3, rects, atlasSize, texture2D3, Color.black);
						dictionary = null;
						text = null;
					}
				}
				enumerator2 = default(Dictionary<string, Texture2D[]>.Enumerator);
				resultsTransferred = true;
				valueTuple = new ValueTuple<Texture2D, Dictionary<string, Texture2D>, Dictionary<string, Rect>, Dictionary<string, Texture2D>>(atlas, additionalMapsList, rectMap, atlasCustomMapsList);
			}
			finally
			{
				List<Texture2D>.Enumerator enumerator3 = temporaryTextures.GetEnumerator();
				int num;
				try
				{
					while (enumerator3.MoveNext())
					{
						Texture2D texture2D4 = enumerator3.Current;
						BoZo_CharacterOptimizer.DestroyBakeOwnedObject(texture2D4);
					}
				}
				finally
				{
					if (num < 0)
					{
						((IDisposable)enumerator3).Dispose();
					}
				}
				BoZo_CharacterOptimizer.DestroyBakeOwnedObject(bakeMaterial);
				if (!resultsTransferred)
				{
					enumerator3 = resultTextures.GetEnumerator();
					try
					{
						while (enumerator3.MoveNext())
						{
							Texture2D texture2D5 = enumerator3.Current;
							BoZo_CharacterOptimizer.DestroyBakeOwnedObject(texture2D5);
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator3).Dispose();
						}
					}
				}
			}
			return valueTuple;
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x00140910 File Offset: 0x0013EB10
		public async Task<Texture2D> BakeTextureAsyncTask(List<Mesh> mesh, Material bakeMaterial)
		{
			int textureSize = bakeMaterial.mainTexture.width;
			RenderTexture rt = new RenderTexture(textureSize, textureSize, 0, RenderTextureFormat.ARGB32)
			{
				useMipMap = false,
				autoGenerateMips = false
			};
			Texture2D texture2D;
			try
			{
				rt.Create();
				using (CommandBuffer commandBuffer = new CommandBuffer())
				{
					commandBuffer.SetRenderTarget(rt);
					commandBuffer.ClearRenderTarget(true, true, Color.clear);
					foreach (Mesh mesh2 in mesh)
					{
						commandBuffer.DrawMesh(mesh2, Matrix4x4.identity, bakeMaterial);
					}
					Graphics.ExecuteCommandBuffer(commandBuffer);
				}
				TaskCompletionSource<Texture2D> tcs = new TaskCompletionSource<Texture2D>();
				AsyncGPUReadback.Request(rt, 0, TextureFormat.RGBA32, delegate(AsyncGPUReadbackRequest request)
				{
					Texture2D texture2D2 = null;
					try
					{
						if (request.hasError)
						{
							throw new Exception("Async GPU readback failed.");
						}
						NativeArray<Color32> data = request.GetData<Color32>(0);
						texture2D2 = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
						texture2D2.LoadRawTextureData<Color32>(data);
						texture2D2.Apply();
						tcs.SetResult(texture2D2);
					}
					catch (Exception ex)
					{
						BoZo_CharacterOptimizer.DestroyBakeOwnedObject(texture2D2);
						tcs.SetException(ex);
					}
				});
				texture2D = await tcs.Task;
			}
			finally
			{
				rt.Release();
				BoZo_CharacterOptimizer.DestroyBakeOwnedObject(rt);
			}
			return texture2D;
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void DestroyBakeOwnedObject(Object value)
		{
			if (value == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(value);
				return;
			}
			Object.DestroyImmediate(value);
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x0014095C File Offset: 0x0013EB5C
		public async Task<Texture2D> RemapTextureAsync(List<Texture2D> normalMaps, Rect[] rects, int atlasSize, Texture2D atlasNormal, Color fillColor)
		{
			List<Task> list = new List<Task>();
			Color[] array = new Color[atlasSize * atlasSize];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = fillColor;
			}
			atlasNormal.SetPixels(array);
			Exception submissionError = null;
			try
			{
				for (int j = 0; j < normalMaps.Count; j++)
				{
					if (!(normalMaps[j] == null))
					{
						Rect rect = rects[j];
						int num = Mathf.RoundToInt(rect.x * (float)atlasSize);
						int num2 = Mathf.RoundToInt(rect.y * (float)atlasSize);
						int num3 = Mathf.RoundToInt(rect.width * (float)atlasSize);
						int num4 = Mathf.RoundToInt(rect.height * (float)atlasSize);
						list.Add(BoZo_CharacterOptimizer.RemapTextureBlockAsync(normalMaps[j], atlasNormal, num, num2, num3, num4));
					}
				}
			}
			catch (Exception ex)
			{
				submissionError = ex;
			}
			Task allReads = Task.WhenAll(list);
			try
			{
				await allReads;
			}
			catch
			{
				if (submissionError != null)
				{
					throw new AggregateException(new Exception[] { submissionError, allReads.Exception });
				}
				throw;
			}
			if (submissionError != null)
			{
				ExceptionDispatchInfo.Capture(submissionError).Throw();
			}
			atlasNormal.Apply();
			return atlasNormal;
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x001409C4 File Offset: 0x0013EBC4
		private static async Task RemapTextureBlockAsync(Texture2D source, Texture2D atlas, int x, int y, int width, int height)
		{
			RenderTexture rt = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
			try
			{
				Graphics.Blit(source, rt);
				TaskCompletionSource<bool> completion = new TaskCompletionSource<bool>();
				AsyncGPUReadback.Request(rt, 0, TextureFormat.RGBA32, delegate(AsyncGPUReadbackRequest request)
				{
					try
					{
						if (request.hasError)
						{
							throw new Exception("Normal map readback failed.");
						}
						Color32[] array = request.GetData<Color32>(0).ToArray();
						Color[] array2 = new Color[array.Length];
						for (int i = 0; i < array.Length; i++)
						{
							array2[i] = array[i];
						}
						atlas.SetPixels(x, y, width, height, array2);
						completion.TrySetResult(true);
					}
					catch (Exception ex)
					{
						completion.TrySetException(ex);
					}
				});
				await completion.Task;
			}
			finally
			{
				RenderTexture.ReleaseTemporary(rt);
			}
		}

		// Token: 0x0600376F RID: 14191 RVA: 0x00140A34 File Offset: 0x0013EC34
		public async Task<Color32[]> DilateTextureAsync(Texture2D tex, Color32[] pixels)
		{
			int iterations = 2;
			int w = tex.width;
			int h = tex.height;
			return await Task.Run<Color32[]>(delegate
			{
				Color32[] array = new Color32[pixels.Length];
				Color32[] array2 = new Color32[pixels.Length];
				bool[] array3 = new bool[pixels.Length];
				for (int i = 0; i < pixels.Length; i++)
				{
					array[i] = pixels[i];
					array3[i] = pixels[i].a >= byte.MaxValue;
				}
				for (int j = 0; j < iterations; j++)
				{
					array.CopyTo(array2, 0);
					for (int k = 0; k < h; k++)
					{
						for (int l = 0; l < w; l++)
						{
							int num = k * w + l;
							if (array[num].a < 255)
							{
								for (int m = -1; m <= 1; m++)
								{
									for (int n = -1; n <= 1; n++)
									{
										if (n != 0 || m != 0)
										{
											int num2 = l + n;
											int num3 = k + m;
											if (num2 >= 0 && num2 < w && num3 >= 0 && num3 < h)
											{
												int num4 = num3 * w + num2;
												if (array3[num4])
												{
													array2[num] = array[num4];
													goto IL_012A;
												}
											}
										}
									}
								}
							}
							IL_012A:;
						}
					}
					Color32[] array4 = array;
					array = array2;
					array2 = array4;
					for (int num5 = 0; num5 < pixels.Length; num5++)
					{
						if (array[num5].a >= 1)
						{
							array3[num5] = true;
						}
					}
				}
				return array;
			});
		}

		// Token: 0x0400378A RID: 14218
		public static string SavePath = "/BoZo_ModularAnimeCharacters/CustomCharacters/Prefabs";

		// Token: 0x0400378B RID: 14219
		private static readonly SemaphoreSlim _mergeSemaphore = new SemaphoreSlim(1, 1);

		// Token: 0x0400378C RID: 14220
		private static bool _enableDebugLogs = false;

		// Token: 0x0200081B RID: 2075
		private class boneData
		{
			// Token: 0x0400378D RID: 14221
			public Transform bone;

			// Token: 0x0400378E RID: 14222
			public int index;
		}

		// Token: 0x0200081C RID: 2076
		private class PropData
		{
			// Token: 0x0400378F RID: 14223
			public GameObject propObject;

			// Token: 0x04003790 RID: 14224
			public string parentBoneName;

			// Token: 0x04003791 RID: 14225
			public Vector3 localPosition;

			// Token: 0x04003792 RID: 14226
			public Quaternion localRotation;

			// Token: 0x04003793 RID: 14227
			public Vector3 localScale;
		}

		// Token: 0x0200081D RID: 2077
		private struct BlendshapeData
		{
			// Token: 0x04003794 RID: 14228
			public string name;

			// Token: 0x04003795 RID: 14229
			public float weight;

			// Token: 0x04003796 RID: 14230
			public float currentWeight;

			// Token: 0x04003797 RID: 14231
			public Vector3[] deltaVertices;

			// Token: 0x04003798 RID: 14232
			public Vector3[] deltaNormals;

			// Token: 0x04003799 RID: 14233
			public Vector3[] deltaTangents;

			// Token: 0x0400379A RID: 14234
			public int vertexOffset;
		}
	}
}
