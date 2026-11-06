using System;
using Features.Flash;
using UnityEngine;
using UnityEngine.Events;

namespace Features.GoldBikini
{
	// Token: 0x020011BB RID: 4539
	public class BikiniBraJiggleHook : MonoBehaviour
	{
		// Token: 0x06008B46 RID: 35654 RVA: 0x0038EA47 File Offset: 0x0038CC47
		private void Awake()
		{
			if (this.breastJiggle == null)
			{
				this.breastJiggle = base.GetComponentInParent<BreastJiggleImpulse>() ?? base.GetComponentInChildren<BreastJiggleImpulse>();
			}
			if (this.braFlash == null)
			{
				this.braFlash = this.ResolveBraFlash();
			}
		}

		// Token: 0x06008B47 RID: 35655 RVA: 0x0038EA88 File Offset: 0x0038CC88
		private void OnEnable()
		{
			if (this.braFlash == null)
			{
				this.braFlash = this.ResolveBraFlash();
			}
			if (this.braFlash == null)
			{
				return;
			}
			this.braFlash.OnOpened.AddListener(new UnityAction(this.OnFlashOpened));
			this.braFlash.OnClosed.AddListener(new UnityAction(this.CancelPendingImpulse));
		}

		// Token: 0x06008B48 RID: 35656 RVA: 0x0038EAF8 File Offset: 0x0038CCF8
		private void OnDisable()
		{
			this.CancelPendingImpulse();
			if (this.braFlash == null)
			{
				return;
			}
			this.braFlash.OnOpened.RemoveListener(new UnityAction(this.OnFlashOpened));
			this.braFlash.OnClosed.RemoveListener(new UnityAction(this.CancelPendingImpulse));
		}

		// Token: 0x06008B49 RID: 35657 RVA: 0x0038EB54 File Offset: 0x0038CD54
		private ArmsFlashController ResolveBraFlash()
		{
			foreach (ArmsFlashController armsFlashController in base.GetComponents<ArmsFlashController>())
			{
				if (armsFlashController.Config != null && armsFlashController.Config.flashLayerName == this.braLayerName)
				{
					return armsFlashController;
				}
			}
			return null;
		}

		// Token: 0x06008B4A RID: 35658 RVA: 0x0038EBA0 File Offset: 0x0038CDA0
		public void OnFlashOpened()
		{
			this.CancelPendingImpulse();
			if (this.breastJiggle == null || this.braFlash == null)
			{
				return;
			}
			Animator animator = this.braFlash.EnsureDetailAnimator();
			if (animator == null)
			{
				return;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in animator.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer.sharedMesh == null))
				{
					int blendShapeIndex = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(this.releaseBlendShape);
					if (blendShapeIndex >= 0)
					{
						this._releaseRenderer = skinnedMeshRenderer;
						this._releaseIndex = blendShapeIndex;
						this._pending = true;
						return;
					}
				}
			}
		}

		// Token: 0x06008B4B RID: 35659 RVA: 0x0038EC40 File Offset: 0x0038CE40
		private void LateUpdate()
		{
			if (!this._pending)
			{
				return;
			}
			if (this.braFlash == null || !this.braFlash.isActiveAndEnabled || !this.braFlash.IsOpen || this._releaseRenderer == null || !this._releaseRenderer.gameObject.activeInHierarchy)
			{
				this.CancelPendingImpulse();
				return;
			}
			if (this._releaseRenderer.GetBlendShapeWeight(this._releaseIndex) < this.releaseWeight)
			{
				return;
			}
			this._pending = false;
			this.breastJiggle.Shake();
		}

		// Token: 0x06008B4C RID: 35660 RVA: 0x0038ECD1 File Offset: 0x0038CED1
		private void CancelPendingImpulse()
		{
			this._pending = false;
			this._releaseRenderer = null;
			this._releaseIndex = -1;
		}

		// Token: 0x04008E2E RID: 36398
		[SerializeField]
		private BreastJiggleImpulse breastJiggle;

		// Token: 0x04008E2F RID: 36399
		[SerializeField]
		private ArmsFlashController braFlash;

		// Token: 0x04008E30 RID: 36400
		[SerializeField]
		private string braLayerName = "BikiniBraFlash";

		// Token: 0x04008E31 RID: 36401
		[Tooltip("Animated garment shape marking the moment the bra releases the chest.")]
		[SerializeField]
		private string releaseBlendShape = "BraLift";

		// Token: 0x04008E32 RID: 36402
		[Tooltip("Impulse on the first frame this lift weight is crossed; follows clip speed and pauses.")]
		[SerializeField]
		[Range(0f, 100f)]
		private float releaseWeight = 50f;

		// Token: 0x04008E33 RID: 36403
		private SkinnedMeshRenderer _releaseRenderer;

		// Token: 0x04008E34 RID: 36404
		private int _releaseIndex = -1;

		// Token: 0x04008E35 RID: 36405
		private bool _pending;
	}
}
