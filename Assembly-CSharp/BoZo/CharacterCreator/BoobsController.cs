using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace BoZo.CharacterCreator
{
	// Token: 0x020007B0 RID: 1968
	[ExecuteAlways]
	public class BoobsController : MonoBehaviour
	{
		// Token: 0x060033F1 RID: 13297 RVA: 0x00127714 File Offset: 0x00125914
		private void OnEnable()
		{
			this.CacheBlendShapeIndices();
			this.animatorParameterHash = Animator.StringToHash(this.animatorParameterName);
			if (this.availableConfigs.Count > 0 && this.currentConfigIndex >= 0 && this.currentConfigIndex < this.availableConfigs.Count)
			{
				this.ApplyConfiguration(this.availableConfigs[this.currentConfigIndex]);
			}
		}

		// Token: 0x060033F2 RID: 13298 RVA: 0x0012777C File Offset: 0x0012597C
		private void OnValidate()
		{
			if (this.availableConfigs.Count > 0)
			{
				this.currentConfigIndex = Mathf.Clamp(this.currentConfigIndex, 0, this.availableConfigs.Count - 1);
				if (this.currentConfig != this.availableConfigs[this.currentConfigIndex])
				{
					this.CacheBlendShapeIndices();
					this.ApplyConfiguration(this.availableConfigs[this.currentConfigIndex]);
				}
			}
		}

		// Token: 0x060033F3 RID: 13299 RVA: 0x001277F1 File Offset: 0x001259F1
		private void Update()
		{
			this.HandleInput();
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x001277FC File Offset: 0x001259FC
		private void HandleInput()
		{
			if (!this.enableInput || !Application.isPlaying || PlayerActivityGate.IsResourceOwned(PlayerContextResource.WorldInteraction))
			{
				return;
			}
			if (this.nextConfigInput.GetButtonDown())
			{
				this.NextConfiguration();
				return;
			}
			if (this.previousConfigInput.GetButtonDown())
			{
				this.PreviousConfiguration();
			}
		}

		// Token: 0x060033F5 RID: 13301 RVA: 0x00127848 File Offset: 0x00125A48
		private void LateUpdate()
		{
			this.ApplyPendingBlendShapeValues();
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x00127850 File Offset: 0x00125A50
		private void ApplyPendingBlendShapeValues()
		{
			if (this.targetRenderer == null)
			{
				return;
			}
			if (BlendShapeManager.Instance != null)
			{
				using (Dictionary<string, int>.Enumerator enumerator = this.blendShapeIndexCache.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, int> keyValuePair = enumerator.Current;
						string key = keyValuePair.Key;
						int value = keyValuePair.Value;
						float num;
						if (this.pendingBlendShapeValues.TryGetValue(value, out num))
						{
							BlendShapeManager.Instance.SetOverride(key, num, "BoobsController", BlendShapePriority.Customization, false);
						}
					}
					return;
				}
			}
			foreach (KeyValuePair<int, float> keyValuePair2 in this.pendingBlendShapeValues)
			{
				this.targetRenderer.SetBlendShapeWeight(keyValuePair2.Key, keyValuePair2.Value);
			}
		}

		// Token: 0x060033F7 RID: 13303 RVA: 0x00127944 File Offset: 0x00125B44
		private void CacheBlendShapeIndices()
		{
			this.blendShapeIndexCache.Clear();
			this.allManagedBlendShapes.Clear();
			if (this.targetRenderer == null || this.targetRenderer.sharedMesh == null)
			{
				return;
			}
			Mesh sharedMesh = this.targetRenderer.sharedMesh;
			foreach (BoobsConfigSO boobsConfigSO in this.availableConfigs)
			{
				if (boobsConfigSO != null && boobsConfigSO.activeBlendShapeNames != null)
				{
					foreach (string text in boobsConfigSO.activeBlendShapeNames)
					{
						if (!string.IsNullOrEmpty(text))
						{
							this.allManagedBlendShapes.Add(text);
						}
					}
				}
			}
			for (int i = 0; i < sharedMesh.blendShapeCount; i++)
			{
				string text2 = sharedMesh.GetBlendShapeName(i);
				if (text2.Contains(":"))
				{
					text2 = text2.Substring(text2.LastIndexOf(':') + 1);
				}
				if (this.allManagedBlendShapes.Contains(text2))
				{
					this.blendShapeIndexCache[text2] = i;
				}
			}
			Debug.Log(string.Format("[BoobsController] Cached {0} blend shape indices", this.blendShapeIndexCache.Count));
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x00127AB8 File Offset: 0x00125CB8
		public void SetConfigurationByIndex(int index)
		{
			if (index < 0 || index >= this.availableConfigs.Count)
			{
				Debug.LogWarning(string.Format("[BoobsController] Invalid configuration index: {0}", index));
				return;
			}
			this.currentConfigIndex = index;
			this.ApplyConfiguration(this.availableConfigs[index]);
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x00127B08 File Offset: 0x00125D08
		public void SetConfigurationByName(string configName)
		{
			for (int i = 0; i < this.availableConfigs.Count; i++)
			{
				if (this.availableConfigs[i] != null && this.availableConfigs[i].configName == configName)
				{
					this.SetConfigurationByIndex(i);
					return;
				}
			}
			Debug.LogWarning("[BoobsController] Configuration not found: " + configName);
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x060033FA RID: 13306 RVA: 0x00127B70 File Offset: 0x00125D70
		public int CurrentBoobsTypeValue
		{
			get
			{
				if (!(this.currentConfig != null))
				{
					return 3;
				}
				return this.currentConfig.animatorBoobsTypeValue;
			}
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x00127B90 File Offset: 0x00125D90
		public void SetStateFromBoobsTypeValue(int boobsTypeValue)
		{
			for (int i = 0; i < this.availableConfigs.Count; i++)
			{
				BoobsConfigSO boobsConfigSO = this.availableConfigs[i];
				if (!(boobsConfigSO == null) && boobsConfigSO.animatorBoobsTypeValue == boobsTypeValue)
				{
					this.currentConfigIndex = i;
					this.currentConfig = boobsConfigSO;
					if (this.animatorParameterHash == 0)
					{
						this.animatorParameterHash = Animator.StringToHash(this.animatorParameterName);
					}
					this.UpdateAnimatorParameter(boobsConfigSO.animatorBoobsTypeValue);
					return;
				}
			}
			Debug.LogWarning(string.Format("[BoobsController] No config with animatorBoobsTypeValue={0}: state unchanged.", boobsTypeValue));
		}

		// Token: 0x060033FC RID: 13308 RVA: 0x00127C1C File Offset: 0x00125E1C
		public void ApplyConfiguration(BoobsConfigSO config)
		{
			if (config == null)
			{
				Debug.LogWarning("[BoobsController] Cannot apply null configuration");
				return;
			}
			this.currentConfig = config;
			this.ResetAllBlendShapes();
			if (!config.IsDefault && config.activeBlendShapeNames != null)
			{
				foreach (string text in config.activeBlendShapeNames)
				{
					this.SetBlendShapeValue(text, 100f);
				}
			}
			this.UpdateAnimatorParameter(config.animatorBoobsTypeValue);
			Debug.Log(string.Format("[BoobsController] Applied configuration: {0} (BoobsType={1})", config.configName, config.animatorBoobsTypeValue));
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x00127CD4 File Offset: 0x00125ED4
		private void ResetAllBlendShapes()
		{
			if (this.targetRenderer == null)
			{
				return;
			}
			this.pendingBlendShapeValues.Clear();
			foreach (KeyValuePair<string, int> keyValuePair in this.blendShapeIndexCache)
			{
				this.pendingBlendShapeValues[keyValuePair.Value] = 0f;
				this.targetRenderer.SetBlendShapeWeight(keyValuePair.Value, 0f);
			}
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x00127D68 File Offset: 0x00125F68
		private void SetBlendShapeValue(string blendShapeName, float value)
		{
			if (this.targetRenderer == null)
			{
				return;
			}
			int num;
			if (!this.blendShapeIndexCache.TryGetValue(blendShapeName, out num))
			{
				Debug.LogWarning("[BoobsController] Blend shape not found in cache: " + blendShapeName);
				return;
			}
			this.pendingBlendShapeValues[num] = value;
			if (BlendShapeManager.Instance != null)
			{
				BlendShapeManager.Instance.SetOverride(blendShapeName, value, "BoobsController", BlendShapePriority.Customization, false);
				return;
			}
			this.targetRenderer.SetBlendShapeWeight(num, value);
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x00127DE1 File Offset: 0x00125FE1
		private void UpdateAnimatorParameter(int value)
		{
			WardrobeBodyPhysicsController componentInParent = base.GetComponentInParent<WardrobeBodyPhysicsController>();
			if (componentInParent != null)
			{
				componentInParent.SetBreastSize(value);
			}
			if (this.targetAnimator == null)
			{
				return;
			}
			this.targetAnimator.SetFloat(this.animatorParameterHash, (float)value);
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x00127E17 File Offset: 0x00126017
		public BoobsConfigSO GetCurrentConfiguration()
		{
			return this.currentConfig;
		}

		// Token: 0x06003401 RID: 13313 RVA: 0x00127E1F File Offset: 0x0012601F
		public int GetCurrentConfigurationIndex()
		{
			return this.currentConfigIndex;
		}

		// Token: 0x06003402 RID: 13314 RVA: 0x00127E27 File Offset: 0x00126027
		public List<BoobsConfigSO> GetAvailableConfigurations()
		{
			return this.availableConfigs;
		}

		// Token: 0x06003403 RID: 13315 RVA: 0x00127E30 File Offset: 0x00126030
		[ContextMenu("Next Configuration")]
		public void NextConfiguration()
		{
			if (this.availableConfigs.Count == 0)
			{
				return;
			}
			int num = (this.currentConfigIndex + 1) % this.availableConfigs.Count;
			this.SetConfigurationByIndex(num);
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x00127E68 File Offset: 0x00126068
		[ContextMenu("Previous Configuration")]
		public void PreviousConfiguration()
		{
			if (this.availableConfigs.Count == 0)
			{
				return;
			}
			int num = this.currentConfigIndex - 1;
			if (num < 0)
			{
				num = this.availableConfigs.Count - 1;
			}
			this.SetConfigurationByIndex(num);
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x00127EA8 File Offset: 0x001260A8
		[ContextMenu("Log Current State")]
		public void LogCurrentState()
		{
			string text = "[BoobsController] Current State:\n";
			text = text + "  Current Config: " + ((this.currentConfig != null) ? this.currentConfig.configName : "None") + "\n";
			text += string.Format("  Config Index: {0}\n", this.currentConfigIndex);
			text = text + "  Animator Value: " + ((this.currentConfig != null) ? this.currentConfig.animatorBoobsTypeValue.ToString() : "N/A") + "\n";
			text += string.Format("  Available Configs: {0}\n", this.availableConfigs.Count);
			if (this.targetRenderer != null && this.targetRenderer.sharedMesh != null)
			{
				text += "\n  Active Blend Shapes:\n";
				foreach (KeyValuePair<string, int> keyValuePair in this.blendShapeIndexCache)
				{
					float blendShapeWeight = this.targetRenderer.GetBlendShapeWeight(keyValuePair.Value);
					if (blendShapeWeight > 0f)
					{
						text += string.Format("    {0}: {1:F1}\n", keyValuePair.Key, blendShapeWeight);
					}
				}
			}
			Debug.Log(text);
		}

		// Token: 0x0400347E RID: 13438
		[Header("References")]
		[Tooltip("The SkinnedMeshRenderer containing the breast blend shapes")]
		[SerializeField]
		private SkinnedMeshRenderer targetRenderer;

		// Token: 0x0400347F RID: 13439
		[Tooltip("The Animator component for setting BoobsType parameter")]
		[SerializeField]
		private Animator targetAnimator;

		// Token: 0x04003480 RID: 13440
		[Header("Animator Settings")]
		[Tooltip("Name of the animator float parameter for boobs type")]
		[SerializeField]
		private string animatorParameterName = "BoobsType";

		// Token: 0x04003481 RID: 13441
		[Header("Configurations")]
		[Tooltip("List of available breast configurations")]
		[SerializeField]
		private List<BoobsConfigSO> availableConfigs = new List<BoobsConfigSO>();

		// Token: 0x04003482 RID: 13442
		[Header("Current Selection")]
		[Tooltip("Currently active configuration index")]
		[SerializeField]
		private int currentConfigIndex;

		// Token: 0x04003483 RID: 13443
		[Tooltip("Currently active configuration (read-only)")]
		[SerializeField]
		private BoobsConfigSO currentConfig;

		// Token: 0x04003484 RID: 13444
		[Header("Input Settings")]
		[Tooltip("Enable input handling for cycling configurations")]
		[SerializeField]
		private bool enableInput = true;

		// Token: 0x04003485 RID: 13445
		[Tooltip("Input to cycle to the next breast configuration")]
		public GenericInput nextConfigInput = new GenericInput("B", "RightShoulder", "");

		// Token: 0x04003486 RID: 13446
		[Tooltip("Input to cycle to the previous breast configuration")]
		public GenericInput previousConfigInput = new GenericInput("", "LeftShoulder", "");

		// Token: 0x04003487 RID: 13447
		private Dictionary<string, int> blendShapeIndexCache = new Dictionary<string, int>();

		// Token: 0x04003488 RID: 13448
		private HashSet<string> allManagedBlendShapes = new HashSet<string>();

		// Token: 0x04003489 RID: 13449
		private int animatorParameterHash;

		// Token: 0x0400348A RID: 13450
		private Dictionary<int, float> pendingBlendShapeValues = new Dictionary<int, float>();

		// Token: 0x0400348B RID: 13451
		private bool needsBlendShapeUpdate;
	}
}
