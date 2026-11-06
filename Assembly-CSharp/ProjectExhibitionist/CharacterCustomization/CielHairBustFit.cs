using System;
using BoZo.CharacterCreator;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization
{
	// Token: 0x02000ACE RID: 2766
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(1000)]
	public sealed class CielHairBustFit : MonoBehaviour
	{
		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x06004E6A RID: 20074 RVA: 0x001E6C6C File Offset: 0x001E4E6C
		private string Source
		{
			get
			{
				string text;
				if ((text = this.sourceId) == null)
				{
					text = (this.sourceId = "CielHairBustFit:" + base.GetInstanceID().ToString());
				}
				return text;
			}
		}

		// Token: 0x06004E6B RID: 20075 RVA: 0x001E6CA4 File Offset: 0x001E4EA4
		public void Configure(SkinnedMeshRenderer renderer)
		{
			if (renderer == null || !renderer.transform.IsChildOf(base.transform))
			{
				throw new ArgumentException("The fitted renderer must belong to this hair hierarchy.", "renderer");
			}
			this.Release();
			this.targetRenderer = renderer;
		}

		// Token: 0x06004E6C RID: 20076 RVA: 0x001E6CDF File Offset: 0x001E4EDF
		private void OnEnable()
		{
			this.Release();
		}

		// Token: 0x06004E6D RID: 20077 RVA: 0x001E6CDF File Offset: 0x001E4EDF
		private void OnDisable()
		{
			this.Release();
		}

		// Token: 0x06004E6E RID: 20078 RVA: 0x001E6CDF File Offset: 0x001E4EDF
		private void OnDestroy()
		{
			this.Release();
		}

		// Token: 0x06004E6F RID: 20079 RVA: 0x001E6CDF File Offset: 0x001E4EDF
		private void OnTransformParentChanged()
		{
			this.Release();
		}

		// Token: 0x06004E70 RID: 20080 RVA: 0x001E6CE8 File Offset: 0x001E4EE8
		private void LateUpdate()
		{
			if (!base.isActiveAndEnabled || !this.TryBind())
			{
				return;
			}
			float num = this.ReadWeight(this.naturalIndex);
			float num2 = this.ReadWeight(this.balloonIndex);
			if ((num != this.lastNatural || !this.manager.HasRendererOverride(this.targetRenderer, "Ciel_BustFit_Natural", this.Source)) && this.manager.SetRendererOverride(this.targetRenderer, "Ciel_BustFit_Natural", num, this.Source, BlendShapePriority.Customization, new float?(0f), CielHairBustFit.GarmentFits, true))
			{
				this.lastNatural = num;
				this.hasRequests = true;
			}
			if ((num2 != this.lastBalloon || !this.manager.HasRendererOverride(this.targetRenderer, "Ciel_BustFit_Balloon", this.Source)) && this.manager.SetRendererOverride(this.targetRenderer, "Ciel_BustFit_Balloon", num2, this.Source, BlendShapePriority.Customization, new float?(0f), CielHairBustFit.GarmentFits, true))
			{
				this.lastBalloon = num2;
				this.hasRequests = true;
			}
		}

		// Token: 0x06004E71 RID: 20081 RVA: 0x001E6DEC File Offset: 0x001E4FEC
		private bool TryBind()
		{
			Animator componentInParent = base.GetComponentInParent<Animator>(true);
			if (this.targetRenderer == null || this.targetRenderer.sharedMesh == null || componentInParent == null || !this.targetRenderer.transform.IsChildOf(base.transform) || this.targetRenderer.GetComponentInParent<Animator>(true) != componentInParent)
			{
				this.Release();
				return false;
			}
			if (this.rig == componentInParent && this.manager != null && this.manager.isActiveAndEnabled && this.body != null && this.body.sharedMesh == this.bodyMesh && this.targetRenderer.sharedMesh == this.hairMesh && this.body.GetComponentInParent<Animator>(true) == componentInParent && this.targetRenderer.transform.IsChildOf(this.manager.transform))
			{
				return true;
			}
			this.Release();
			BlendShapeManager blendShapeManager = BlendShapeManager.ResolveForRig(this.targetRenderer);
			if (blendShapeManager == null || !blendShapeManager.isActiveAndEnabled || this.targetRenderer.sharedMesh.GetBlendShapeIndex("Ciel_BustFit_Natural") < 0 || this.targetRenderer.sharedMesh.GetBlendShapeIndex("Ciel_BustFit_Balloon") < 0)
			{
				return false;
			}
			SkinnedMeshRenderer skinnedMeshRenderer = null;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in componentInParent.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer2.name != "Body_sotai_NSFW") && !(skinnedMeshRenderer2.sharedMesh == null) && !(skinnedMeshRenderer2.GetComponentInParent<Animator>(true) != componentInParent) && skinnedMeshRenderer2.transform.IsChildOf(blendShapeManager.transform))
				{
					if (skinnedMeshRenderer != null)
					{
						return false;
					}
					skinnedMeshRenderer = skinnedMeshRenderer2;
				}
			}
			if (skinnedMeshRenderer == null)
			{
				return false;
			}
			this.rig = componentInParent;
			this.manager = blendShapeManager;
			this.body = skinnedMeshRenderer;
			this.bodyMesh = this.body.sharedMesh;
			this.hairMesh = this.targetRenderer.sharedMesh;
			this.naturalIndex = this.bodyMesh.GetBlendShapeIndex("Breast_Size_03_Natural");
			this.balloonIndex = this.bodyMesh.GetBlendShapeIndex("Breast_Size_04_Balloon");
			return true;
		}

		// Token: 0x06004E72 RID: 20082 RVA: 0x001E7038 File Offset: 0x001E5238
		private float ReadWeight(int index)
		{
			float num = ((index >= 0) ? this.body.GetBlendShapeWeight(index) : 0f);
			if (!float.IsNaN(num) && !float.IsInfinity(num))
			{
				return Mathf.Clamp(num, 0f, 100f);
			}
			return 0f;
		}

		// Token: 0x06004E73 RID: 20083 RVA: 0x001E7084 File Offset: 0x001E5284
		private void Release()
		{
			if (this.manager != null)
			{
				this.manager.RemoveRendererOverridesFromSource(this.Source);
			}
			else if (this.hasRequests && this.targetRenderer != null && this.targetRenderer.sharedMesh != null && this.targetRenderer.transform.IsChildOf(base.transform))
			{
				int blendShapeIndex = this.targetRenderer.sharedMesh.GetBlendShapeIndex("Ciel_BustFit_Natural");
				int blendShapeIndex2 = this.targetRenderer.sharedMesh.GetBlendShapeIndex("Ciel_BustFit_Balloon");
				if (blendShapeIndex >= 0)
				{
					this.targetRenderer.SetBlendShapeWeight(blendShapeIndex, 0f);
				}
				if (blendShapeIndex2 >= 0)
				{
					this.targetRenderer.SetBlendShapeWeight(blendShapeIndex2, 0f);
				}
			}
			this.hasRequests = false;
			this.manager = null;
			this.rig = null;
			this.body = null;
			this.bodyMesh = (this.hairMesh = null);
			this.naturalIndex = (this.balloonIndex = -1);
			this.lastNatural = (this.lastBalloon = float.NaN);
		}

		// Token: 0x04004FC1 RID: 20417
		public const string NaturalFit = "Ciel_BustFit_Natural";

		// Token: 0x04004FC2 RID: 20418
		public const string BalloonFit = "Ciel_BustFit_Balloon";

		// Token: 0x04004FC3 RID: 20419
		private const string NaturalBody = "Breast_Size_03_Natural";

		// Token: 0x04004FC4 RID: 20420
		private const string BalloonBody = "Breast_Size_04_Balloon";

		// Token: 0x04004FC5 RID: 20421
		private static readonly string[] GarmentFits = new string[] { "RainCoat_HideHair", "Ghost_HideHair" };

		// Token: 0x04004FC6 RID: 20422
		[SerializeField]
		private SkinnedMeshRenderer targetRenderer;

		// Token: 0x04004FC7 RID: 20423
		private Animator rig;

		// Token: 0x04004FC8 RID: 20424
		private BlendShapeManager manager;

		// Token: 0x04004FC9 RID: 20425
		private SkinnedMeshRenderer body;

		// Token: 0x04004FCA RID: 20426
		private Mesh bodyMesh;

		// Token: 0x04004FCB RID: 20427
		private Mesh hairMesh;

		// Token: 0x04004FCC RID: 20428
		private int naturalIndex = -1;

		// Token: 0x04004FCD RID: 20429
		private int balloonIndex = -1;

		// Token: 0x04004FCE RID: 20430
		private float lastNatural = float.NaN;

		// Token: 0x04004FCF RID: 20431
		private float lastBalloon = float.NaN;

		// Token: 0x04004FD0 RID: 20432
		private bool hasRequests;

		// Token: 0x04004FD1 RID: 20433
		private string sourceId;
	}
}
