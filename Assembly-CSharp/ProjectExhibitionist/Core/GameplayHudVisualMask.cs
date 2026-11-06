using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Features.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A58 RID: 2648
	[DisallowMultipleComponent]
	public sealed class GameplayHudVisualMask : MonoBehaviour, IMaterialModifier
	{
		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x06004B5D RID: 19293 RVA: 0x001D5266 File Offset: 0x001D3466
		// (set) Token: 0x06004B5E RID: 19294 RVA: 0x001D526E File Offset: 0x001D346E
		public bool IsVisualVisible { get; private set; } = true;

		// Token: 0x06004B5F RID: 19295 RVA: 0x001D5278 File Offset: 0x001D3478
		public static GameplayHudVisualMask Attach(GameObject target, HudDisplayPart category)
		{
			GameplayHudVisualMask gameplayHudVisualMask = target.GetComponent<GameplayHudVisualMask>();
			if (gameplayHudVisualMask == null)
			{
				gameplayHudVisualMask = target.AddComponent<GameplayHudVisualMask>();
			}
			gameplayHudVisualMask.part = category;
			gameplayHudVisualMask.Apply();
			gameplayHudVisualMask.RefreshChildren();
			return gameplayHudVisualMask;
		}

		// Token: 0x06004B60 RID: 19296 RVA: 0x001D52B0 File Offset: 0x001D34B0
		private void RefreshChildren()
		{
			base.GetComponentsInChildren<Graphic>(true, this._descendants);
			foreach (Graphic graphic in this._descendants)
			{
				if (!(graphic.gameObject == base.gameObject))
				{
					GameplayHudVisualMask gameplayHudVisualMask = graphic.GetComponent<GameplayHudVisualMask>();
					if (gameplayHudVisualMask == null)
					{
						gameplayHudVisualMask = graphic.gameObject.AddComponent<GameplayHudVisualMask>();
					}
					gameplayHudVisualMask.Apply();
				}
			}
			this._descendants.Clear();
		}

		// Token: 0x06004B61 RID: 19297 RVA: 0x001D534C File Offset: 0x001D354C
		private void OnTransformChildrenChanged()
		{
			GameplayHudVisualMask.PendingChildren.Add(this);
		}

		// Token: 0x06004B62 RID: 19298 RVA: 0x001D535C File Offset: 0x001D355C
		internal static void RefreshPendingChildren()
		{
			GameplayHudVisualMask.PendingWork.Clear();
			foreach (GameplayHudVisualMask gameplayHudVisualMask in GameplayHudVisualMask.PendingChildren)
			{
				GameplayHudVisualMask.PendingWork.Add(gameplayHudVisualMask);
			}
			GameplayHudVisualMask.PendingChildren.Clear();
			foreach (GameplayHudVisualMask gameplayHudVisualMask2 in GameplayHudVisualMask.PendingWork)
			{
				if (gameplayHudVisualMask2 != null && gameplayHudVisualMask2.isActiveAndEnabled)
				{
					gameplayHudVisualMask2.RefreshChildren();
				}
			}
			GameplayHudVisualMask.PendingWork.Clear();
		}

		// Token: 0x06004B63 RID: 19299 RVA: 0x001D5424 File Offset: 0x001D3624
		private void OnEnable()
		{
			this._materialSuspended = false;
			GameSettingsService.SettingsChanged += this.Apply;
			SceneManager.activeSceneChanged += this.OnSceneChanged;
			bool isVisualVisible = this.IsVisualVisible;
			this.Apply();
			if (isVisualVisible == this.IsVisualVisible)
			{
				this.RefreshMaterial();
			}
			GameplayHudVisualMask.PendingChildren.Add(this);
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x001D5480 File Offset: 0x001D3680
		private void OnDisable()
		{
			GameSettingsService.SettingsChanged -= this.Apply;
			SceneManager.activeSceneChanged -= this.OnSceneChanged;
			GameplayHudVisualMask.PendingChildren.Remove(this);
			this._materialSuspended = true;
			this.ReleaseMaterialLease();
			this.RefreshMaterial();
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x001D54CE File Offset: 0x001D36CE
		private void OnDestroy()
		{
			GameplayHudVisualMask.PendingChildren.Remove(this);
			this.ReleaseMaterialLease();
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x001D54E2 File Offset: 0x001D36E2
		private void OnSceneChanged(Scene previous, Scene current)
		{
			this.Apply();
		}

		// Token: 0x06004B67 RID: 19303 RVA: 0x001D54EA File Offset: 0x001D36EA
		private void OnTransformParentChanged()
		{
			this.Apply();
			this.RefreshChildren();
		}

		// Token: 0x06004B68 RID: 19304 RVA: 0x001D54F8 File Offset: 0x001D36F8
		public void Apply()
		{
			bool flag = GameSettingsService.ShowGameplayUI;
			foreach (GameplayHudVisualMask gameplayHudVisualMask in base.GetComponentsInParent<GameplayHudVisualMask>(true))
			{
				if (gameplayHudVisualMask.part == HudDisplayPart.FaceCamera)
				{
					flag &= GameSettingsService.ShowFaceCamera;
				}
				if (gameplayHudVisualMask.part == HudDisplayPart.DistanceAndScore)
				{
					flag &= GameSettingsService.ShowDistanceAndScore;
				}
			}
			if (SceneManager.GetActiveScene().name == "MainMenu" || GameplayHudVisibility.IsProtected(base.transform))
			{
				flag = true;
			}
			if (this.IsVisualVisible == flag)
			{
				return;
			}
			this.IsVisualVisible = flag;
			if (flag)
			{
				this.ReleaseMaterialLease();
			}
			this.RefreshMaterial();
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x001D5590 File Offset: 0x001D3790
		private void RefreshMaterial()
		{
			if (this._graphic == null)
			{
				this._graphic = base.GetComponent<Graphic>();
			}
			if (this._graphic == null)
			{
				return;
			}
			this._graphic.SetMaterialDirty();
			if (this._graphic.isActiveAndEnabled)
			{
				this._graphic.Rebuild(CanvasUpdate.PreRender);
			}
		}

		// Token: 0x06004B6A RID: 19306 RVA: 0x001D55EC File Offset: 0x001D37EC
		public Material GetModifiedMaterial(Material baseMaterial)
		{
			if (this._materialSuspended || !base.isActiveAndEnabled || this.IsVisualVisible || baseMaterial == null)
			{
				this.ReleaseMaterialLease();
				return baseMaterial;
			}
			if (GameplayHudVisualMask._invisibleShader == null)
			{
				GameplayHudVisualMask._invisibleShader = Resources.Load<Shader>("GameplayHudInvisible");
			}
			if (GameplayHudVisualMask._invisibleShader == null)
			{
				this.ReleaseMaterialLease();
				return baseMaterial;
			}
			if (this._materialLease == null || this._materialLease.Source != baseMaterial)
			{
				this.ReleaseMaterialLease();
				if (!GameplayHudVisualMask.HiddenMaterials.TryGetValue(baseMaterial, out this._materialLease))
				{
					this._materialLease = new GameplayHudVisualMask.HiddenMaterialLease(baseMaterial);
					GameplayHudVisualMask.HiddenMaterials.Add(baseMaterial, this._materialLease);
				}
				this._materialLease.Consumers++;
			}
			if (this._materialLease.Hidden == null)
			{
				this._materialLease.Hidden = new Material(GameplayHudVisualMask._invisibleShader)
				{
					hideFlags = HideFlags.HideAndDontSave
				};
			}
			this._materialLease.Hidden.CopyPropertiesFromMaterial(baseMaterial);
			return this._materialLease.Hidden;
		}

		// Token: 0x06004B6B RID: 19307 RVA: 0x001D5700 File Offset: 0x001D3900
		private void ReleaseMaterialLease()
		{
			GameplayHudVisualMask.HiddenMaterialLease materialLease = this._materialLease;
			if (materialLease == null)
			{
				return;
			}
			this._materialLease = null;
			GameplayHudVisualMask.HiddenMaterialLease hiddenMaterialLease = materialLease;
			int num = hiddenMaterialLease.Consumers - 1;
			hiddenMaterialLease.Consumers = num;
			if (num != 0)
			{
				return;
			}
			GameplayHudVisualMask.HiddenMaterials.Remove(materialLease.Source);
			if (materialLease.Hidden == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(materialLease.Hidden);
				return;
			}
			Object.DestroyImmediate(materialLease.Hidden);
		}

		// Token: 0x04004D34 RID: 19764
		[SerializeField]
		private HudDisplayPart part;

		// Token: 0x04004D35 RID: 19765
		private Graphic _graphic;

		// Token: 0x04004D36 RID: 19766
		private GameplayHudVisualMask.HiddenMaterialLease _materialLease;

		// Token: 0x04004D37 RID: 19767
		private bool _materialSuspended;

		// Token: 0x04004D38 RID: 19768
		private static Shader _invisibleShader;

		// Token: 0x04004D39 RID: 19769
		private static readonly Dictionary<Material, GameplayHudVisualMask.HiddenMaterialLease> HiddenMaterials = new Dictionary<Material, GameplayHudVisualMask.HiddenMaterialLease>(new GameplayHudVisualMask.MaterialIdentityComparer());

		// Token: 0x04004D3A RID: 19770
		private readonly List<Graphic> _descendants = new List<Graphic>();

		// Token: 0x04004D3B RID: 19771
		private static readonly HashSet<GameplayHudVisualMask> PendingChildren = new HashSet<GameplayHudVisualMask>();

		// Token: 0x04004D3C RID: 19772
		private static readonly List<GameplayHudVisualMask> PendingWork = new List<GameplayHudVisualMask>();

		// Token: 0x02000A59 RID: 2649
		private sealed class HiddenMaterialLease
		{
			// Token: 0x06004B6E RID: 19310 RVA: 0x001D57B0 File Offset: 0x001D39B0
			public HiddenMaterialLease(Material source)
			{
				this.Source = source;
			}

			// Token: 0x04004D3E RID: 19774
			public readonly Material Source;

			// Token: 0x04004D3F RID: 19775
			public Material Hidden;

			// Token: 0x04004D40 RID: 19776
			public int Consumers;
		}

		// Token: 0x02000A5A RID: 2650
		private sealed class MaterialIdentityComparer : IEqualityComparer<Material>
		{
			// Token: 0x06004B6F RID: 19311 RVA: 0x001D57BF File Offset: 0x001D39BF
			public bool Equals(Material left, Material right)
			{
				return left == right;
			}

			// Token: 0x06004B70 RID: 19312 RVA: 0x001D57C5 File Offset: 0x001D39C5
			public int GetHashCode(Material material)
			{
				return RuntimeHelpers.GetHashCode(material);
			}
		}
	}
}
