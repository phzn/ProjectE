using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x02000025 RID: 37
	[DisallowMultipleComponent]
	public class StaticCullingSource : MonoBehaviour
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00005620 File Offset: 0x00003820
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00005628 File Offset: 0x00003828
		public SourceType SourceType
		{
			get
			{
				return this._sourceType;
			}
			set
			{
				this._sourceType = value;
				this.OnSourceTypeChanged();
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00005637 File Offset: 0x00003837
		public string ValidationError
		{
			get
			{
				return this._validationError;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x0000563F File Offset: 0x0000383F
		public CullingTarget CullingTarget
		{
			get
			{
				return this._target;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00005647 File Offset: 0x00003847
		public IStaticCullingSourceStrategy Strategy
		{
			get
			{
				return this._strategy;
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000564F File Offset: 0x0000384F
		private void Reset()
		{
			this.AutoDetectSourceType();
			this.CreateStrategy();
			this.Validate();
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005664 File Offset: 0x00003864
		private void AutoDetectSourceType()
		{
			if (base.GetComponent<MeshRenderer>() != null)
			{
				this._sourceType = SourceType.MeshRenderer;
				return;
			}
			if (base.GetComponent<LODGroup>() != null)
			{
				this._sourceType = SourceType.LODGroup;
				return;
			}
			if (base.GetComponent<Light>() != null)
			{
				this._sourceType = SourceType.Light;
				return;
			}
			this._sourceType = SourceType.Custom;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x000056BA File Offset: 0x000038BA
		private void OnSourceTypeChanged()
		{
			this._validationError = "";
			this.CreateStrategy();
			this.Validate();
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000056D4 File Offset: 0x000038D4
		private void CreateStrategy()
		{
			if (this._sourceType == SourceType.MeshRenderer)
			{
				this._strategy = new MeshRendererStaticCullingSourceStrategy(base.gameObject);
				return;
			}
			if (this._sourceType == SourceType.LODGroup)
			{
				this._strategy = new LODGroupStaticCullingSourceStrategy(base.gameObject);
				return;
			}
			if (this._sourceType == SourceType.Light)
			{
				this._strategy = new LightStaticCullingSourceStrategy(base.gameObject);
				return;
			}
			if (this._sourceType == SourceType.Custom)
			{
				this._strategy = new CustomStaticCullingSourceStrategy(base.gameObject);
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000574B File Offset: 0x0000394B
		public bool Validate()
		{
			if (this._strategy == null)
			{
				this.AutoDetectSourceType();
				this.CreateStrategy();
			}
			this._validationError = "";
			return this._strategy.Validate(out this._validationError);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000577D File Offset: 0x0000397D
		public bool TryGetBounds(out Bounds bounds)
		{
			return this._strategy.TryGetBounds(out bounds);
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000578C File Offset: 0x0000398C
		public void PrepareForBaking()
		{
			if (this._validationError != "" && !this.Validate())
			{
				throw new Exception("StaticCullingSource::" + base.gameObject.name + " has validation errors");
			}
			this._target = this._strategy.CreateCullingTarget();
			this._strategy.PrepareForBaking();
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000057EF File Offset: 0x000039EF
		public void ClearAfterBaking()
		{
			this._strategy.ClearAfterBaking();
		}

		// Token: 0x040000B1 RID: 177
		[SerializeField]
		private SourceType _sourceType;

		// Token: 0x040000B2 RID: 178
		[SerializeField]
		private string _validationError;

		// Token: 0x040000B3 RID: 179
		[SerializeField]
		private CullingTarget _target;

		// Token: 0x040000B4 RID: 180
		[SerializeReference]
		private IStaticCullingSourceStrategy _strategy;
	}
}
