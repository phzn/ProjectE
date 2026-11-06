using System;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000F05 RID: 3845
	[ExecuteAlways]
	[DisallowMultipleComponent]
	public class StaticPosePreview : MonoBehaviour
	{
		// Token: 0x06007367 RID: 29543 RVA: 0x002F1073 File Offset: 0x002EF273
		private void OnEnable()
		{
			if (this.animator == null)
			{
				this.animator = base.GetComponentInChildren<Animator>();
			}
			this.SamplePose();
		}

		// Token: 0x06007368 RID: 29544 RVA: 0x002F1095 File Offset: 0x002EF295
		private void OnValidate()
		{
			if (this.animator == null)
			{
				this.animator = base.GetComponentInChildren<Animator>();
			}
		}

		// Token: 0x06007369 RID: 29545 RVA: 0x002F10B1 File Offset: 0x002EF2B1
		private void Update()
		{
			if (Application.isPlaying)
			{
				return;
			}
			if (!this.sampleInEditMode)
			{
				return;
			}
			this.SamplePose();
		}

		// Token: 0x0600736A RID: 29546 RVA: 0x002F10CC File Offset: 0x002EF2CC
		private void SamplePose()
		{
			if (this.animator == null || this.animator.runtimeAnimatorController == null)
			{
				return;
			}
			AnimatorCullingMode cullingMode = this.animator.cullingMode;
			this.animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			try
			{
				if (!this.animator.isInitialized)
				{
					this.animator.Rebind();
				}
				this.TrySetInt("PostureState", this.postureState);
				if (!string.IsNullOrEmpty(this.forceStatePath))
				{
					int num = Animator.StringToHash(this.forceStatePath);
					if (this.animator.HasState(0, num))
					{
						this.animator.Play(num, 0, 0f);
					}
					else if (!this._warnedMissingState)
					{
						this._warnedMissingState = true;
						Debug.LogWarning(string.Concat(new string[] { "[StaticPosePreview] stato '", this.forceStatePath, "' non trovato sul layer 0 di '", base.name, "': posa non forzata." }), this);
					}
				}
				this.animator.Update(0f);
			}
			finally
			{
				this.animator.cullingMode = cullingMode;
			}
		}

		// Token: 0x0600736B RID: 29547 RVA: 0x002F11F4 File Offset: 0x002EF3F4
		private void TrySetInt(string param, int value)
		{
			foreach (AnimatorControllerParameter animatorControllerParameter in this.animator.parameters)
			{
				if (animatorControllerParameter.type == AnimatorControllerParameterType.Int && animatorControllerParameter.name == param)
				{
					this.animator.SetInteger(param, value);
					return;
				}
			}
		}

		// Token: 0x040076A8 RID: 30376
		[Tooltip("Animator da campionare. Auto-trovato nei figli se vuoto.")]
		public Animator animator;

		// Token: 0x040076A9 RID: 30377
		[Tooltip("Se vero, ricampiona la posa ogni frame in edit mode. Lascialo attivo per vedere subito i cambi di controller/clip mentre posizioni l'NPC.")]
		public bool sampleInEditMode = true;

		// Token: 0x040076AA RID: 30378
		[Tooltip("Valore di PostureState da forzare sul controller Master-seated: 1 = seduto, 0 = in piedi. NB: da solo NON cambia la posa (la entry porta comunque a SitIdle) — per la posa serve forceStatePath.")]
		public int postureState = 1;

		// Token: 0x040076AB RID: 30379
		[Tooltip("Se valorizzato, entra DI COLPO in questo stato dell'Animator (layer 0), senza transizione. Percorso completo, es. 'Base Layer.Standing.Normal Movement' per l'idle in piedi. Lascialo vuoto per la posa seduta di default.")]
		public string forceStatePath = "";

		// Token: 0x040076AC RID: 30380
		private bool _warnedMissingState;
	}
}
