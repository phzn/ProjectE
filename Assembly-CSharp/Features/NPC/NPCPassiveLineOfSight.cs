using System;
using Features.Detection;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000ED8 RID: 3800
	internal sealed class NPCPassiveLineOfSight
	{
		// Token: 0x060071C1 RID: 29121 RVA: 0x002E37BE File Offset: 0x002E19BE
		public NPCPassiveLineOfSight(Component observer, float fallbackEyeHeight)
		{
			this._observer = observer.transform;
			this._senses = observer.GetComponent<NPCSenses>();
			this._animator = observer.GetComponentInChildren<Animator>();
			this._fallbackEyeHeight = fallbackEyeHeight;
		}

		// Token: 0x060071C2 RID: 29122 RVA: 0x002E37F4 File Offset: 0x002E19F4
		public bool CanSee(Transform target)
		{
			if (target == null || !target.gameObject.activeInHierarchy)
			{
				return false;
			}
			if (target != this._target)
			{
				this._target = target;
				this._targetActor = target.GetComponentInParent<StealthActor>();
			}
			Vector3 vector = ((this._targetActor != null) ? this._targetActor.Position : (target.position + Vector3.up * 1.15f));
			Transform transform = ((this._targetActor != null) ? this._targetActor.transform : target);
			DetectionConfigSO detectionConfigSO = ((SensoryManager.Instance != null) ? SensoryManager.Instance.GlobalConfig : null);
			if (detectionConfigSO == null && this._targetActor != null)
			{
				detectionConfigSO = this._targetActor.GlobalConfig;
			}
			int num = ((detectionConfigSO != null) ? detectionConfigSO.VisionObstacles.value : LayerMask.GetMask(new string[] { "Default", "Floor", "VisionBlocker" }));
			RaycastHit raycastHit;
			return !DetectionLineOfSight.IsBlocked(this.EyePosition, vector, num, this._observer, transform, out raycastHit, (this._targetActor != null) ? this._targetActor.VisibilityInteractionOccluder : null);
		}

		// Token: 0x17001294 RID: 4756
		// (get) Token: 0x060071C3 RID: 29123 RVA: 0x002E393C File Offset: 0x002E1B3C
		private Vector3 EyePosition
		{
			get
			{
				if (this._senses != null)
				{
					return this._senses.EyePosition;
				}
				if (this._animator == null)
				{
					this._animator = this._observer.GetComponentInChildren<Animator>();
				}
				if (this._animator != null && this._animator.avatar != null && this._animator.isHuman)
				{
					Transform boneTransform = this._animator.GetBoneTransform(HumanBodyBones.Head);
					if (boneTransform != null)
					{
						return boneTransform.position;
					}
				}
				return this._observer.position + Vector3.up * this._fallbackEyeHeight;
			}
		}

		// Token: 0x040074A9 RID: 29865
		private readonly Transform _observer;

		// Token: 0x040074AA RID: 29866
		private readonly NPCSenses _senses;

		// Token: 0x040074AB RID: 29867
		private readonly float _fallbackEyeHeight;

		// Token: 0x040074AC RID: 29868
		private Animator _animator;

		// Token: 0x040074AD RID: 29869
		private Transform _target;

		// Token: 0x040074AE RID: 29870
		private StealthActor _targetActor;
	}
}
