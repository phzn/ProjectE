using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoZo.CharacterCreator
{
	// Token: 0x020007AD RID: 1965
	[ExecuteAlways]
	[DefaultExecutionOrder(20000)]
	public class RuntimeBlendShapeController : MonoBehaviour
	{
		// Token: 0x060033DE RID: 13278 RVA: 0x00126DD1 File Offset: 0x00124FD1
		private void OnEnable()
		{
			if (this.autoFindRenderers)
			{
				this.FindAllRenderers();
			}
			this.BuildBlendShapeList();
		}

		// Token: 0x060033DF RID: 13279 RVA: 0x00126DE7 File Offset: 0x00124FE7
		private void OnValidate()
		{
			if (this.refreshBlendShapes)
			{
				this.refreshBlendShapes = false;
				if (this.autoFindRenderers)
				{
					this.FindAllRenderers();
				}
				this.BuildBlendShapeList();
			}
			this.ApplyBlendShapeValues(false);
		}

		// Token: 0x060033E0 RID: 13280 RVA: 0x00126E13 File Offset: 0x00125013
		private void LateUpdate()
		{
			if (this.overrideAnimator)
			{
				this.ApplyBlendShapeValues(false);
			}
		}

		// Token: 0x060033E1 RID: 13281 RVA: 0x00126E24 File Offset: 0x00125024
		[ContextMenu("Find All Renderers")]
		public void FindAllRenderers()
		{
			this.targetRenderers.Clear();
			this.targetRenderers.AddRange(base.GetComponentsInChildren<SkinnedMeshRenderer>(true));
			Debug.Log(string.Format("[RuntimeBlendShapeController] Found {0} SkinnedMeshRenderers", this.targetRenderers.Count));
		}

		// Token: 0x060033E2 RID: 13282 RVA: 0x00126E64 File Offset: 0x00125064
		[ContextMenu("Build Blend Shape List")]
		public void BuildBlendShapeList()
		{
			Dictionary<string, float> dictionary = new Dictionary<string, float>();
			foreach (RuntimeBlendShapeController.BlendShapeData blendShapeData in this.blendShapes)
			{
				string text = blendShapeData.rendererName + "_" + blendShapeData.name;
				dictionary[text] = blendShapeData.value;
			}
			this.blendShapes.Clear();
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this.targetRenderers)
			{
				if (!(skinnedMeshRenderer == null) && !(skinnedMeshRenderer.sharedMesh == null))
				{
					Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
					int blendShapeCount = sharedMesh.blendShapeCount;
					for (int i = 0; i < blendShapeCount; i++)
					{
						string blendShapeName = sharedMesh.GetBlendShapeName(i);
						float blendShapeWeight = skinnedMeshRenderer.GetBlendShapeWeight(i);
						string text2 = skinnedMeshRenderer.name + "_" + blendShapeName;
						RuntimeBlendShapeController.BlendShapeData blendShapeData2 = new RuntimeBlendShapeController.BlendShapeData
						{
							name = blendShapeName,
							rendererName = skinnedMeshRenderer.name,
							value = (dictionary.ContainsKey(text2) ? dictionary[text2] : blendShapeWeight),
							renderer = skinnedMeshRenderer,
							index = i,
							baseline = blendShapeWeight,
							lastApplied = float.NaN,
							overrideActive = false
						};
						this.blendShapes.Add(blendShapeData2);
					}
				}
			}
			Debug.Log(string.Format("[RuntimeBlendShapeController] Built list with {0} blend shapes", this.blendShapes.Count));
		}

		// Token: 0x060033E3 RID: 13283 RVA: 0x0012702C File Offset: 0x0012522C
		private void ApplyBlendShapeValues(bool forceAll)
		{
			BlendShapeManager blendShapeManager = this.ResolveManager();
			foreach (RuntimeBlendShapeController.BlendShapeData blendShapeData in this.blendShapes)
			{
				if (!(blendShapeData.renderer == null))
				{
					if (Mathf.Abs(blendShapeData.value - blendShapeData.baseline) > 0.001f)
					{
						bool flag = float.IsNaN(blendShapeData.lastApplied) || Mathf.Abs(blendShapeData.value - blendShapeData.lastApplied) > 0.001f;
						if (!forceAll && !flag && blendShapeData.overrideActive)
						{
							if (blendShapeManager == null)
							{
								blendShapeData.renderer.SetBlendShapeWeight(blendShapeData.index, blendShapeData.value);
							}
						}
						else
						{
							if (blendShapeManager != null)
							{
								string text = blendShapeData.name;
								int num = text.LastIndexOf(':');
								if (num >= 0)
								{
									text = text.Substring(num + 1);
								}
								blendShapeManager.SetOverride(text, blendShapeData.value, "RuntimeBlendShapeController", BlendShapePriority.Animation, false);
							}
							else
							{
								blendShapeData.renderer.SetBlendShapeWeight(blendShapeData.index, blendShapeData.value);
							}
							blendShapeData.lastApplied = blendShapeData.value;
							blendShapeData.overrideActive = true;
						}
					}
					else if (blendShapeData.overrideActive)
					{
						if (blendShapeManager != null)
						{
							string text2 = blendShapeData.name;
							int num2 = text2.LastIndexOf(':');
							if (num2 >= 0)
							{
								text2 = text2.Substring(num2 + 1);
							}
							blendShapeManager.RemoveOverride(text2, "RuntimeBlendShapeController", false, false, 0f);
						}
						blendShapeData.lastApplied = float.NaN;
						blendShapeData.overrideActive = false;
					}
					else if (forceAll && blendShapeManager == null)
					{
						blendShapeData.renderer.SetBlendShapeWeight(blendShapeData.index, blendShapeData.value);
					}
				}
			}
		}

		// Token: 0x060033E4 RID: 13284 RVA: 0x00127218 File Offset: 0x00125418
		private BlendShapeManager ResolveManager()
		{
			if (!this.routeThroughBlendShapeManager)
			{
				return null;
			}
			if (!Application.isPlaying)
			{
				return null;
			}
			if (this.cachedManager == null)
			{
				this.cachedManager = base.GetComponentInParent<BlendShapeManager>();
				if (this.cachedManager == null)
				{
					this.cachedManager = BlendShapeManager.Instance;
				}
			}
			return this.cachedManager;
		}

		// Token: 0x060033E5 RID: 13285 RVA: 0x00127274 File Offset: 0x00125474
		[ContextMenu("Reset All to Zero")]
		public void ResetAllToZero()
		{
			BlendShapeManager blendShapeManager = this.ResolveManager();
			foreach (RuntimeBlendShapeController.BlendShapeData blendShapeData in this.blendShapes)
			{
				blendShapeData.value = 0f;
				blendShapeData.lastApplied = float.NaN;
				if (blendShapeData.renderer != null)
				{
					blendShapeData.renderer.SetBlendShapeWeight(blendShapeData.index, 0f);
				}
				if (blendShapeData.overrideActive && blendShapeManager != null)
				{
					string text = blendShapeData.name;
					int num = text.LastIndexOf(':');
					if (num >= 0)
					{
						text = text.Substring(num + 1);
					}
					blendShapeManager.RemoveOverride(text, "RuntimeBlendShapeController", false, false, 0f);
				}
				blendShapeData.overrideActive = false;
			}
			Debug.Log("[RuntimeBlendShapeController] All blend shapes reset to 0");
		}

		// Token: 0x060033E6 RID: 13286 RVA: 0x00127360 File Offset: 0x00125560
		[ContextMenu("Release Manager Overrides")]
		public void ReleaseManagerOverrides()
		{
			BlendShapeManager blendShapeManager = this.ResolveManager();
			if (blendShapeManager == null)
			{
				Debug.LogWarning("[RuntimeBlendShapeController] Nessun BlendShapeManager da cui rimuovere gli override");
				return;
			}
			blendShapeManager.RemoveAllOverridesFromSource("RuntimeBlendShapeController");
			Debug.Log("[RuntimeBlendShapeController] Override rilasciati al BlendShapeManager");
		}

		// Token: 0x060033E7 RID: 13287 RVA: 0x001273A0 File Offset: 0x001255A0
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				BlendShapeManager blendShapeManager = this.cachedManager;
				if (blendShapeManager != null)
				{
					blendShapeManager.RemoveAllOverridesFromSource("RuntimeBlendShapeController");
				}
			}
		}

		// Token: 0x060033E8 RID: 13288 RVA: 0x001273D0 File Offset: 0x001255D0
		[ContextMenu("Read Current Values")]
		public void ReadCurrentValues()
		{
			foreach (RuntimeBlendShapeController.BlendShapeData blendShapeData in this.blendShapes)
			{
				if (blendShapeData.renderer != null)
				{
					blendShapeData.value = blendShapeData.renderer.GetBlendShapeWeight(blendShapeData.index);
					blendShapeData.baseline = blendShapeData.value;
					blendShapeData.lastApplied = float.NaN;
					blendShapeData.overrideActive = false;
				}
			}
			Debug.Log("[RuntimeBlendShapeController] Read current blend shape values from renderers");
		}

		// Token: 0x060033E9 RID: 13289 RVA: 0x0012746C File Offset: 0x0012566C
		public void SetBlendShapeValue(string blendShapeName, float value)
		{
			float num = Mathf.Clamp(value, 0f, 100f);
			foreach (RuntimeBlendShapeController.BlendShapeData blendShapeData in this.blendShapes)
			{
				if (blendShapeData.name == blendShapeName || blendShapeData.name.EndsWith(blendShapeName))
				{
					blendShapeData.value = num;
					blendShapeData.lastApplied = float.NaN;
				}
			}
		}

		// Token: 0x060033EA RID: 13290 RVA: 0x001274F8 File Offset: 0x001256F8
		public float GetBlendShapeValue(string blendShapeName)
		{
			foreach (RuntimeBlendShapeController.BlendShapeData blendShapeData in this.blendShapes)
			{
				if (blendShapeData.name == blendShapeName || blendShapeData.name.EndsWith(blendShapeName))
				{
					return blendShapeData.value;
				}
			}
			return 0f;
		}

		// Token: 0x060033EB RID: 13291 RVA: 0x00127570 File Offset: 0x00125770
		public List<string> GetAllBlendShapeNames()
		{
			List<string> list = new List<string>();
			foreach (RuntimeBlendShapeController.BlendShapeData blendShapeData in this.blendShapes)
			{
				list.Add(blendShapeData.rendererName + "/" + blendShapeData.name);
			}
			return list;
		}

		// Token: 0x060033EC RID: 13292 RVA: 0x001275E0 File Offset: 0x001257E0
		[ContextMenu("Log All Blend Shapes")]
		public void LogAllBlendShapes()
		{
			string text = string.Format("[RuntimeBlendShapeController] All Blend Shapes ({0} total):\n", this.blendShapes.Count);
			string text2 = "";
			foreach (RuntimeBlendShapeController.BlendShapeData blendShapeData in this.blendShapes)
			{
				if (blendShapeData.rendererName != text2)
				{
					text2 = blendShapeData.rendererName;
					text = text + "\n=== " + text2 + " ===\n";
				}
				text += string.Format("  [{0}] {1}: {2:F1}\n", blendShapeData.index, blendShapeData.name, blendShapeData.value);
			}
			Debug.Log(text);
		}

		// Token: 0x04003469 RID: 13417
		private const string DebugSourceId = "RuntimeBlendShapeController";

		// Token: 0x0400346A RID: 13418
		[Header("Target Renderers")]
		[Tooltip("Automatically find all SkinnedMeshRenderers on this GameObject and children")]
		[SerializeField]
		private bool autoFindRenderers = true;

		// Token: 0x0400346B RID: 13419
		[SerializeField]
		private List<SkinnedMeshRenderer> targetRenderers = new List<SkinnedMeshRenderer>();

		// Token: 0x0400346C RID: 13420
		[Header("Settings")]
		[Tooltip("Apply blend shape values in LateUpdate to override Animator")]
		[SerializeField]
		private bool overrideAnimator = true;

		// Token: 0x0400346D RID: 13421
		[Tooltip("Se presente sul personaggio, instrada gli override tramite BlendShapeManager con priorità Animation (la più alta). Garantisce che gli slider non vengano sovrascritti dagli altri sistemi.")]
		[SerializeField]
		private bool routeThroughBlendShapeManager = true;

		// Token: 0x0400346E RID: 13422
		[Tooltip("Refresh the blend shape list")]
		[SerializeField]
		private bool refreshBlendShapes;

		// Token: 0x0400346F RID: 13423
		[Header("Blend Shapes")]
		[SerializeField]
		private List<RuntimeBlendShapeController.BlendShapeData> blendShapes = new List<RuntimeBlendShapeController.BlendShapeData>();

		// Token: 0x04003470 RID: 13424
		private BlendShapeManager cachedManager;

		// Token: 0x020007AE RID: 1966
		[Serializable]
		public class BlendShapeData
		{
			// Token: 0x04003471 RID: 13425
			public string name;

			// Token: 0x04003472 RID: 13426
			public string rendererName;

			// Token: 0x04003473 RID: 13427
			[Range(0f, 100f)]
			public float value;

			// Token: 0x04003474 RID: 13428
			[HideInInspector]
			public SkinnedMeshRenderer renderer;

			// Token: 0x04003475 RID: 13429
			[HideInInspector]
			public int index;

			// Token: 0x04003476 RID: 13430
			[HideInInspector]
			public float baseline;

			// Token: 0x04003477 RID: 13431
			[HideInInspector]
			public float lastApplied;

			// Token: 0x04003478 RID: 13432
			[HideInInspector]
			public bool overrideActive;
		}
	}
}
