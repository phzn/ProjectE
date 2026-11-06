using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Dynamic
{
	// Token: 0x02000049 RID: 73
	[DisallowMultipleComponent]
	public class DC_SourceSettings : MonoBehaviour
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00007DF5 File Offset: 0x00005FF5
		public bool ReadyForCulling
		{
			get
			{
				return this._strategy != null && this._strategy.ReadyForCulling;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x00007E0C File Offset: 0x0000600C
		public int CullingLayer
		{
			get
			{
				return DC_Controller.GetCullingLayer();
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00007E13 File Offset: 0x00006013
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00007E1B File Offset: 0x0000601B
		public SourceType SourceType
		{
			get
			{
				return this._sourceType;
			}
			set
			{
				if (value == this._sourceType)
				{
					return;
				}
				this._sourceType = value;
				this.OnSourceTypeChanged();
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00007E34 File Offset: 0x00006034
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00007E3C File Offset: 0x0000603C
		public int ControllerID { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00007E45 File Offset: 0x00006045
		// (set) Token: 0x060001AB RID: 427 RVA: 0x00007E4D File Offset: 0x0000604D
		public bool IsIncompatible { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00007E56 File Offset: 0x00006056
		// (set) Token: 0x060001AD RID: 429 RVA: 0x00007E5E File Offset: 0x0000605E
		public string IncompatibilityReason { get; private set; }

		// Token: 0x060001AE RID: 430 RVA: 0x00007E67 File Offset: 0x00006067
		private void Reset()
		{
			this.DetectSourceType();
			this.CheckCompatibility();
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00007E76 File Offset: 0x00006076
		private void Awake()
		{
			if (this._strategy == null)
			{
				this.CreateStrategy();
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00007E88 File Offset: 0x00006088
		private void Start()
		{
			try
			{
				if (!this.CheckCompatibility())
				{
					base.enabled = false;
				}
				else
				{
					if (!this._strategy.ReadyForCulling)
					{
						this._strategy.PrepareForCulling();
					}
					DC_Controller.GetById(this.ControllerID).AddObjectForCulling(this._strategy.CreateCullingTarget(), this._strategy.GetColliders());
					Object.Destroy(this);
				}
			}
			catch (Exception ex)
			{
				this.IsIncompatible = true;
				this.IncompatibilityReason = ex.Message + ex.StackTrace;
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00007F20 File Offset: 0x00006120
		public T GetStrategy<T>() where T : IDC_SourceSettingsStrategy
		{
			return (T)((object)this._strategy);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00007F2D File Offset: 0x0000612D
		public bool TryGetBounds(ref Bounds bounds)
		{
			return this._strategy != null && this._strategy.TryGetBounds(ref bounds);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00007F48 File Offset: 0x00006148
		public bool CheckCompatibility()
		{
			if (this._strategy == null)
			{
				this.CreateStrategy();
			}
			string text;
			this.IsIncompatible = !this._strategy.CheckCompatibilityAndGetComponents(out text);
			this.IncompatibilityReason = text;
			return !this.IsIncompatible;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00007F8C File Offset: 0x0000618C
		public void Bake()
		{
			if (Application.isPlaying)
			{
				Debug.Log("'Bake' can only be called in editor mode");
				return;
			}
			if (this._strategy != null && this._strategy.ReadyForCulling)
			{
				this._strategy.ClearData();
			}
			if (this.CheckCompatibility())
			{
				this._strategy.PrepareForCulling();
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00007FDE File Offset: 0x000061DE
		public void ClearBakedData()
		{
			if (Application.isPlaying)
			{
				Debug.Log("'ClearBakedData' can only be called in editor mode");
				return;
			}
			IDC_SourceSettingsStrategy strategy = this._strategy;
			if (strategy == null)
			{
				return;
			}
			strategy.ClearData();
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00008002 File Offset: 0x00006202
		private void DetectSourceType()
		{
			if (base.GetComponent<LODGroup>() != null)
			{
				this.SourceType = SourceType.LODGroup;
				return;
			}
			if (base.GetComponent<MeshRenderer>() != null)
			{
				this.SourceType = SourceType.SingleMesh;
				return;
			}
			this.SourceType = SourceType.Custom;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00008037 File Offset: 0x00006237
		private void OnSourceTypeChanged()
		{
			if (this._strategy != null && this._strategy.ReadyForCulling)
			{
				this._strategy.ClearData();
			}
			this.CreateStrategy();
			this.CheckCompatibility();
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00008068 File Offset: 0x00006268
		private void CreateStrategy()
		{
			if (this.SourceType == SourceType.SingleMesh)
			{
				this._strategy = new DC_RendererSourceSettingsStrategy(this);
				return;
			}
			if (this.SourceType == SourceType.LODGroup)
			{
				this._strategy = new DC_LODGroupSourceSettingsStrategy(this);
				return;
			}
			if (this.SourceType == SourceType.Custom)
			{
				this._strategy = new DC_CustomSourceSettingsStrategy(this);
				return;
			}
			throw new NotSupportedException();
		}

		// Token: 0x04000129 RID: 297
		[SerializeField]
		private SourceType _sourceType;

		// Token: 0x0400012A RID: 298
		[SerializeReference]
		private IDC_SourceSettingsStrategy _strategy;
	}
}
