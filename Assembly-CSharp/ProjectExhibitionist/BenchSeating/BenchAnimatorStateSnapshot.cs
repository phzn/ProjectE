using System;
using Invector.vEventSystems;
using UnityEngine;

namespace ProjectExhibitionist.BenchSeating
{
	// Token: 0x02000B27 RID: 2855
	internal sealed class BenchAnimatorStateSnapshot
	{
		// Token: 0x06005100 RID: 20736 RVA: 0x001F5EF7 File Offset: 0x001F40F7
		internal static BenchAnimatorStateSnapshot Capture(Animator animator)
		{
			if (!(animator != null) || !animator.isInitialized || !(animator.runtimeAnimatorController != null))
			{
				return null;
			}
			return new BenchAnimatorStateSnapshot(animator);
		}

		// Token: 0x06005101 RID: 20737 RVA: 0x001F5F20 File Offset: 0x001F4120
		private BenchAnimatorStateSnapshot(Animator animator)
		{
			this.animator = animator;
			this.controller = animator.runtimeAnimatorController;
			this.speed = animator.speed;
			AnimatorControllerParameter[] array = animator.parameters;
			this.parameters = new BenchAnimatorStateSnapshot.ParameterState[array.Length];
			for (int i = 0; i < this.parameters.Length; i++)
			{
				this.parameters[i] = new BenchAnimatorStateSnapshot.ParameterState(animator, array[i]);
			}
			this.layers = new BenchAnimatorStateSnapshot.LayerState[animator.layerCount];
			for (int j = 0; j < this.layers.Length; j++)
			{
				this.layers[j] = new BenchAnimatorStateSnapshot.LayerState(animator, j);
			}
		}

		// Token: 0x06005102 RID: 20738 RVA: 0x001F5FC8 File Offset: 0x001F41C8
		internal void RestoreBeforeEvaluation()
		{
			if (!this.CanRestore)
			{
				return;
			}
			foreach (vIAnimatorStateInfoController vIAnimatorStateInfoController in this.animator.GetComponents<vIAnimatorStateInfoController>())
			{
				if (vIAnimatorStateInfoController.animatorStateInfos != null && vIAnimatorStateInfoController.animatorStateInfos.animator == this.animator)
				{
					vIAnimatorStateInfoController.Register();
				}
			}
			this.animator.speed = this.speed;
			foreach (BenchAnimatorStateSnapshot.ParameterState parameterState in this.parameters)
			{
				parameterState.Restore(this.animator, false);
			}
			bool flag = false;
			int num = 0;
			while (num < this.layers.Length && num < this.animator.layerCount)
			{
				this.layers[num].RestoreCurrentState(this.animator, num);
				this.animator.SetLayerWeight(num, this.layers[num].Weight);
				flag |= this.layers[num].CanRestoreTransition(this.animator, num);
				num++;
			}
			if (!flag)
			{
				return;
			}
			this.animator.Update(0f);
			int num2 = 0;
			while (num2 < this.layers.Length && num2 < this.animator.layerCount)
			{
				this.layers[num2].RestoreTransition(this.animator, num2);
				num2++;
			}
		}

		// Token: 0x06005103 RID: 20739 RVA: 0x001F6134 File Offset: 0x001F4334
		internal void RestoreAfterEvaluation()
		{
			if (!this.CanRestore)
			{
				return;
			}
			foreach (BenchAnimatorStateSnapshot.ParameterState parameterState in this.parameters)
			{
				parameterState.Restore(this.animator, true);
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x06005104 RID: 20740 RVA: 0x001F6175 File Offset: 0x001F4375
		private bool CanRestore
		{
			get
			{
				return this.animator != null && this.animator.runtimeAnimatorController == this.controller;
			}
		}

		// Token: 0x040051A5 RID: 20901
		private readonly Animator animator;

		// Token: 0x040051A6 RID: 20902
		private readonly RuntimeAnimatorController controller;

		// Token: 0x040051A7 RID: 20903
		private readonly BenchAnimatorStateSnapshot.ParameterState[] parameters;

		// Token: 0x040051A8 RID: 20904
		private readonly BenchAnimatorStateSnapshot.LayerState[] layers;

		// Token: 0x040051A9 RID: 20905
		private readonly float speed;

		// Token: 0x02000B28 RID: 2856
		private readonly struct ParameterState
		{
			// Token: 0x06005105 RID: 20741 RVA: 0x001F61A0 File Offset: 0x001F43A0
			internal ParameterState(Animator animator, AnimatorControllerParameter parameter)
			{
				this.hash = parameter.nameHash;
				this.type = parameter.type;
				this.number = ((this.type == AnimatorControllerParameterType.Float) ? animator.GetFloat(this.hash) : 0f);
				this.integer = ((this.type == AnimatorControllerParameterType.Int) ? animator.GetInteger(this.hash) : 0);
				this.flag = (this.type == AnimatorControllerParameterType.Bool || this.type == AnimatorControllerParameterType.Trigger) && animator.GetBool(this.hash);
			}

			// Token: 0x06005106 RID: 20742 RVA: 0x001F6230 File Offset: 0x001F4430
			internal void Restore(Animator animator, bool triggersOnly)
			{
				if (this.type == AnimatorControllerParameterType.Trigger)
				{
					if (triggersOnly && this.flag)
					{
						animator.SetTrigger(this.hash);
						return;
					}
					animator.ResetTrigger(this.hash);
					return;
				}
				else
				{
					if (triggersOnly)
					{
						return;
					}
					switch (this.type)
					{
					case AnimatorControllerParameterType.Float:
						animator.SetFloat(this.hash, this.number);
						return;
					case (AnimatorControllerParameterType)2:
						break;
					case AnimatorControllerParameterType.Int:
						animator.SetInteger(this.hash, this.integer);
						return;
					case AnimatorControllerParameterType.Bool:
						animator.SetBool(this.hash, this.flag);
						break;
					default:
						return;
					}
					return;
				}
			}

			// Token: 0x040051AA RID: 20906
			private readonly int hash;

			// Token: 0x040051AB RID: 20907
			private readonly AnimatorControllerParameterType type;

			// Token: 0x040051AC RID: 20908
			private readonly float number;

			// Token: 0x040051AD RID: 20909
			private readonly int integer;

			// Token: 0x040051AE RID: 20910
			private readonly bool flag;
		}

		// Token: 0x02000B29 RID: 2857
		private readonly struct LayerState
		{
			// Token: 0x06005107 RID: 20743 RVA: 0x001F62C8 File Offset: 0x001F44C8
			internal LayerState(Animator animator, int layer)
			{
				this.Weight = animator.GetLayerWeight(layer);
				this.current = animator.GetCurrentAnimatorStateInfo(layer);
				this.inTransition = animator.IsInTransition(layer);
				this.next = (this.inTransition ? animator.GetNextAnimatorStateInfo(layer) : default(AnimatorStateInfo));
				this.transition = (this.inTransition ? animator.GetAnimatorTransitionInfo(layer) : default(AnimatorTransitionInfo));
			}

			// Token: 0x06005108 RID: 20744 RVA: 0x001F633C File Offset: 0x001F453C
			internal void RestoreCurrentState(Animator animator, int layer)
			{
				if (this.current.fullPathHash == 0 || !animator.HasState(layer, this.current.fullPathHash))
				{
					return;
				}
				animator.Play(this.current.fullPathHash, layer, this.current.normalizedTime);
			}

			// Token: 0x06005109 RID: 20745 RVA: 0x001F6394 File Offset: 0x001F4594
			internal bool CanRestoreTransition(Animator animator, int layer)
			{
				return this.inTransition && this.current.fullPathHash != 0 && this.next.fullPathHash != 0 && animator.HasState(layer, this.current.fullPathHash) && animator.HasState(layer, this.next.fullPathHash);
			}

			// Token: 0x0600510A RID: 20746 RVA: 0x001F63F8 File Offset: 0x001F45F8
			internal void RestoreTransition(Animator animator, int layer)
			{
				if (!this.CanRestoreTransition(animator, layer))
				{
					return;
				}
				if (this.transition.durationUnit == DurationUnit.Fixed)
				{
					animator.CrossFadeInFixedTime(this.next.fullPathHash, this.transition.duration, layer, this.next.normalizedTime * this.next.length, this.transition.normalizedTime);
					return;
				}
				animator.CrossFade(this.next.fullPathHash, this.transition.duration, layer, this.next.normalizedTime, this.transition.normalizedTime);
			}

			// Token: 0x040051AF RID: 20911
			private readonly AnimatorStateInfo current;

			// Token: 0x040051B0 RID: 20912
			private readonly AnimatorStateInfo next;

			// Token: 0x040051B1 RID: 20913
			private readonly AnimatorTransitionInfo transition;

			// Token: 0x040051B2 RID: 20914
			private readonly bool inTransition;

			// Token: 0x040051B3 RID: 20915
			internal readonly float Weight;
		}
	}
}
