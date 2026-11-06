using System;
using Features.NPC;
using UnityEngine;

namespace ProjectExhibitionist.Subway
{
	// Token: 0x02000925 RID: 2341
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Animator))]
	public class SubwayGripIK : MonoBehaviour
	{
		// Token: 0x06003F89 RID: 16265 RVA: 0x00179215 File Offset: 0x00177415
		private void Awake()
		{
			this._animator = base.GetComponent<Animator>();
		}

		// Token: 0x06003F8A RID: 16266 RVA: 0x00179224 File Offset: 0x00177424
		private void OnEnable()
		{
			if (this._reaction == null)
			{
				this._reaction = base.GetComponent<SeatedRandomReaction>();
			}
			if (this._reaction != null)
			{
				this._reaction.ReactionActiveChanged += this.OnReactionActiveChanged;
			}
			this._suspended = false;
		}

		// Token: 0x06003F8B RID: 16267 RVA: 0x00179277 File Offset: 0x00177477
		private void OnDisable()
		{
			if (this._reaction != null)
			{
				this._reaction.ReactionActiveChanged -= this.OnReactionActiveChanged;
			}
		}

		// Token: 0x06003F8C RID: 16268 RVA: 0x0017929E File Offset: 0x0017749E
		private void OnReactionActiveChanged(bool reacting)
		{
			this._suspended = reacting;
		}

		// Token: 0x06003F8D RID: 16269 RVA: 0x001792A8 File Offset: 0x001774A8
		private void OnAnimatorIK(int layerIndex)
		{
			if (this._animator == null)
			{
				this._animator = base.GetComponent<Animator>();
			}
			if (layerIndex != 0 || this._animator == null)
			{
				return;
			}
			object obj = this.target != null && this.weight > 0f && !this._suspended;
			bool flag = this.hand == SubwayGripIK.GripHand.Both && this.secondTarget != null;
			object obj2 = obj;
			bool flag2 = obj2 != null && this.hand != SubwayGripIK.GripHand.Left;
			bool flag3 = obj2 != null && (this.hand == SubwayGripIK.GripHand.Left || flag);
			this._animator.SetIKPositionWeight(AvatarIKGoal.RightHand, flag2 ? this.weight : 0f);
			this._animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, flag3 ? this.weight : 0f);
			this._animator.SetIKRotationWeight(AvatarIKGoal.RightHand, flag2 ? this.rotationWeight : 0f);
			this._animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, flag3 ? this.rotationWeight : 0f);
			if (flag2)
			{
				this._animator.SetIKPosition(AvatarIKGoal.RightHand, this.target.position);
				this._animator.SetIKRotation(AvatarIKGoal.RightHand, this.target.rotation);
			}
			if (flag3)
			{
				Transform transform = (flag ? this.secondTarget : this.target);
				this._animator.SetIKPosition(AvatarIKGoal.LeftHand, transform.position);
				this._animator.SetIKRotation(AvatarIKGoal.LeftHand, transform.rotation);
			}
		}

		// Token: 0x04003F59 RID: 16217
		[Tooltip("Punto sul corrimano/palo a cui attaccare la mano. Vuoto = IK spento.")]
		public Transform target;

		// Token: 0x04003F5A RID: 16218
		[Tooltip("Secondo punto di contatto, per le prese a due mani. La sua PRESENZA dice che questo posto è un palo su cui si sta con due mani: senza, le clip a due mani non vengono nemmeno assegnate a questo slot.")]
		public Transform secondTarget;

		// Token: 0x04003F5B RID: 16219
		[Tooltip("Quale mano afferra. 'Both' usa anche secondTarget per l'altra.")]
		public SubwayGripIK.GripHand hand;

		// Token: 0x04003F5C RID: 16220
		[Tooltip("1 = mano esattamente sul target, 0 = solo animazione. Valori intermedi avvicinano la mano senza forzare il braccio.")]
		[Range(0f, 1f)]
		public float weight = 1f;

		// Token: 0x04003F5D RID: 16221
		[Tooltip("Quanto la mano assume l'ORIENTAMENTO del target: ruotando l'empty fai girare il palmo attorno alla barra. 0 = tiene la rotazione della clip. Alzalo solo dopo aver allineato il target alla mano (Tools/Subway/Align Grip Target Rotations To Hands), altrimenti il palmo scatta su un orientamento arbitrario.")]
		[Range(0f, 1f)]
		public float rotationWeight;

		// Token: 0x04003F5E RID: 16222
		private Animator _animator;

		// Token: 0x04003F5F RID: 16223
		private SeatedRandomReaction _reaction;

		// Token: 0x04003F60 RID: 16224
		private bool _suspended;

		// Token: 0x02000926 RID: 2342
		public enum GripHand
		{
			// Token: 0x04003F62 RID: 16226
			Right,
			// Token: 0x04003F63 RID: 16227
			Left,
			// Token: 0x04003F64 RID: 16228
			Both
		}
	}
}
