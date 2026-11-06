using System;
using System.Collections.Generic;
using Features.NPC;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace ProjectExhibitionist.NPCs
{
	// Token: 0x02000998 RID: 2456
	[DisallowMultipleComponent]
	public sealed class MallShopAmbientPopulation : MonoBehaviour
	{
		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x0600446F RID: 17519 RVA: 0x0019A19B File Offset: 0x0019839B
		public IReadOnlyList<MallShopAmbientPopulation.WorkerBinding> Workers
		{
			get
			{
				return this.workers;
			}
		}

		// Token: 0x06004470 RID: 17520 RVA: 0x0019A1A3 File Offset: 0x001983A3
		public void Configure(MallShopAmbientPopulation.WorkerBinding[] newWorkers)
		{
			this.workers = newWorkers ?? Array.Empty<MallShopAmbientPopulation.WorkerBinding>();
		}

		// Token: 0x06004471 RID: 17521 RVA: 0x0019A1B5 File Offset: 0x001983B5
		private void OnEnable()
		{
			if (Application.isPlaying)
			{
				this.StartWorkers();
			}
		}

		// Token: 0x06004472 RID: 17522 RVA: 0x0019A1C4 File Offset: 0x001983C4
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				this.StopWorkers();
			}
		}

		// Token: 0x06004473 RID: 17523 RVA: 0x0019A1C4 File Offset: 0x001983C4
		private void OnDestroy()
		{
			if (Application.isPlaying)
			{
				this.StopWorkers();
			}
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x0019A1D4 File Offset: 0x001983D4
		private void LateUpdate()
		{
			if (this.workers == null || this._authoredLocalPositions.Length != this.workers.Length)
			{
				return;
			}
			for (int i = 0; i < this.workers.Length; i++)
			{
				MallShopAmbientPopulation.WorkerBinding workerBinding = this.workers[i];
				if (workerBinding != null && workerBinding.pinAuthoredTransform && !(workerBinding.characterRoot == null) && (i >= this._interactiveWorkers.Length || !(this._interactiveWorkers[i] != null) || !this._interactiveWorkers[i].OwnsPresentation))
				{
					Transform transform = workerBinding.characterRoot.transform;
					if (!transform.localPosition.Equals(this._authoredLocalPositions[i]))
					{
						transform.localPosition = this._authoredLocalPositions[i];
					}
					if (!transform.localRotation.Equals(this._authoredLocalRotations[i]))
					{
						transform.localRotation = this._authoredLocalRotations[i];
					}
				}
			}
		}

		// Token: 0x06004475 RID: 17525 RVA: 0x0019A2D4 File Offset: 0x001984D4
		private void StartWorkers()
		{
			this.StopWorkers();
			int num = ((this.workers != null) ? this.workers.Length : 0);
			this._authoredLocalPositions = new Vector3[num];
			this._authoredLocalRotations = new Quaternion[num];
			this._graphs = new PlayableGraph[num];
			this._interactiveWorkers = new StaticPhoneNPC[num];
			for (int i = 0; i < num; i++)
			{
				MallShopAmbientPopulation.WorkerBinding workerBinding = this.workers[i];
				if (workerBinding != null && !(workerBinding.characterRoot == null))
				{
					GameObject characterRoot = workerBinding.characterRoot;
					Transform transform = characterRoot.transform;
					this._authoredLocalPositions[i] = transform.localPosition;
					this._authoredLocalRotations[i] = transform.localRotation;
					MallShopAmbientPopulation.DisableDynamicNpcLogic(characterRoot);
					Animator animator = ((workerBinding.animator != null) ? workerBinding.animator : characterRoot.GetComponentInChildren<Animator>(true));
					workerBinding.animator = animator;
					StaticPhoneNPC component = characterRoot.GetComponent<StaticPhoneNPC>();
					if (component != null && component.enabled)
					{
						this._interactiveWorkers[i] = component;
						component.PresentationOwnershipChanged += this.OnPresentationOwnershipChanged;
					}
					this.StartWorkerPose(i);
					MallShopAmbientPopulation.EnsureLookAtDriver(characterRoot);
				}
			}
		}

		// Token: 0x06004476 RID: 17526 RVA: 0x0019A404 File Offset: 0x00198604
		private void StopWorkers()
		{
			for (int i = 0; i < this._interactiveWorkers.Length; i++)
			{
				if (this._interactiveWorkers[i] != null)
				{
					this._interactiveWorkers[i].PresentationOwnershipChanged -= this.OnPresentationOwnershipChanged;
				}
			}
			this._interactiveWorkers = Array.Empty<StaticPhoneNPC>();
			for (int j = 0; j < this._graphs.Length; j++)
			{
				PlayableGraph playableGraph = this._graphs[j];
				if (playableGraph.IsValid())
				{
					playableGraph.Destroy();
				}
			}
			this._graphs = Array.Empty<PlayableGraph>();
			if (this.workers == null)
			{
				return;
			}
			for (int k = 0; k < this.workers.Length; k++)
			{
				MallShopAmbientPopulation.WorkerBinding workerBinding = this.workers[k];
				if (workerBinding != null)
				{
					if (workerBinding.animator != null)
					{
						workerBinding.animator.enabled = false;
					}
					if (!(workerBinding.characterRoot == null))
					{
						FieldPlayerLookAtCiel component = workerBinding.characterRoot.GetComponent<FieldPlayerLookAtCiel>();
						if (component != null)
						{
							component.enabled = false;
						}
					}
				}
			}
		}

		// Token: 0x06004477 RID: 17527 RVA: 0x0019A508 File Offset: 0x00198708
		private void OnPresentationOwnershipChanged(StaticPhoneNPC actor, bool owned)
		{
			int i = 0;
			while (i < this._interactiveWorkers.Length)
			{
				if (!(this._interactiveWorkers[i] != actor))
				{
					if (owned)
					{
						if (this._graphs[i].IsValid())
						{
							this._graphs[i].Destroy();
						}
						this._graphs[i] = default(PlayableGraph);
						return;
					}
					this.StartWorkerPose(i);
					return;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06004478 RID: 17528 RVA: 0x0019A57C File Offset: 0x0019877C
		private void StartWorkerPose(int index)
		{
			MallShopAmbientPopulation.WorkerBinding workerBinding = this.workers[index];
			if (workerBinding == null || workerBinding.animator == null || workerBinding.animation == null)
			{
				return;
			}
			if (this._interactiveWorkers[index] != null && this._interactiveWorkers[index].OwnsPresentation)
			{
				return;
			}
			if (this._graphs[index].IsValid())
			{
				this._graphs[index].Destroy();
			}
			Animator animator = workerBinding.animator;
			animator.enabled = true;
			animator.applyRootMotion = false;
			PlayableGraph playableGraph;
			AnimationClipPlayable animationClipPlayable = AnimationPlayableUtilities.PlayClip(animator, workerBinding.animation, out playableGraph);
			animationClipPlayable.SetApplyFootIK(true);
			animationClipPlayable.SetTime((double)(workerBinding.animation.length * Mathf.Clamp(workerBinding.normalizedStartTime, 0f, 0.999f)));
			animationClipPlayable.SetSpeed((double)Mathf.Max(0.05f, workerBinding.animationSpeed));
			playableGraph.Play();
			playableGraph.Evaluate(0f);
			this._graphs[index] = playableGraph;
		}

		// Token: 0x06004479 RID: 17529 RVA: 0x0019A680 File Offset: 0x00198880
		private static void EnsureLookAtDriver(GameObject character)
		{
			NPCSimpleLookAt npcsimpleLookAt = character.GetComponent<NPCSimpleLookAt>();
			if (npcsimpleLookAt == null)
			{
				npcsimpleLookAt = character.GetComponentInChildren<NPCSimpleLookAt>(true);
			}
			if (npcsimpleLookAt == null)
			{
				npcsimpleLookAt = character.AddComponent<NPCSimpleLookAt>();
			}
			npcsimpleLookAt.enabled = true;
			FieldPlayerLookAtCiel fieldPlayerLookAtCiel = character.GetComponent<FieldPlayerLookAtCiel>();
			if (fieldPlayerLookAtCiel == null)
			{
				fieldPlayerLookAtCiel = character.AddComponent<FieldPlayerLookAtCiel>();
				fieldPlayerLookAtCiel.lookRange = 9f;
				fieldPlayerLookAtCiel.expressionParamName = string.Empty;
			}
			fieldPlayerLookAtCiel.lookAt = npcsimpleLookAt;
			fieldPlayerLookAtCiel.enabled = true;
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x0019A6F8 File Offset: 0x001988F8
		public static void DisableDynamicNpcLogic(GameObject character)
		{
			if (character == null)
			{
				return;
			}
			StaticPhoneNPC component = character.GetComponent<StaticPhoneNPC>();
			if (component != null && component.enabled)
			{
				return;
			}
			foreach (Behaviour behaviour in character.GetComponentsInChildren<Behaviour>(true))
			{
				if (!(behaviour == null) && !(behaviour is Animator) && !(behaviour is NPCSimpleLookAt) && !(behaviour is FieldPlayerLookAtCiel))
				{
					behaviour.enabled = false;
				}
			}
			Collider[] componentsInChildren2 = character.GetComponentsInChildren<Collider>(true);
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				if (componentsInChildren2[j] != null)
				{
					componentsInChildren2[j].enabled = false;
				}
			}
			AudioSource[] componentsInChildren3 = character.GetComponentsInChildren<AudioSource>(true);
			for (int k = 0; k < componentsInChildren3.Length; k++)
			{
				if (componentsInChildren3[k] != null)
				{
					componentsInChildren3[k].enabled = false;
				}
			}
			Canvas[] componentsInChildren4 = character.GetComponentsInChildren<Canvas>(true);
			for (int l = 0; l < componentsInChildren4.Length; l++)
			{
				if (componentsInChildren4[l] != null)
				{
					componentsInChildren4[l].enabled = false;
				}
			}
		}

		// Token: 0x04004418 RID: 17432
		[SerializeField]
		private MallShopAmbientPopulation.WorkerBinding[] workers = Array.Empty<MallShopAmbientPopulation.WorkerBinding>();

		// Token: 0x04004419 RID: 17433
		private PlayableGraph[] _graphs = Array.Empty<PlayableGraph>();

		// Token: 0x0400441A RID: 17434
		private StaticPhoneNPC[] _interactiveWorkers = Array.Empty<StaticPhoneNPC>();

		// Token: 0x0400441B RID: 17435
		private Vector3[] _authoredLocalPositions = Array.Empty<Vector3>();

		// Token: 0x0400441C RID: 17436
		private Quaternion[] _authoredLocalRotations = Array.Empty<Quaternion>();

		// Token: 0x02000999 RID: 2457
		[Serializable]
		public sealed class WorkerBinding
		{
			// Token: 0x0400441D RID: 17437
			public string shopName;

			// Token: 0x0400441E RID: 17438
			public GameObject characterRoot;

			// Token: 0x0400441F RID: 17439
			public Animator animator;

			// Token: 0x04004420 RID: 17440
			public AnimationClip animation;

			// Token: 0x04004421 RID: 17441
			[Min(0.05f)]
			public float animationSpeed = 1f;

			// Token: 0x04004422 RID: 17442
			[Range(0f, 0.999f)]
			public float normalizedStartTime;

			// Token: 0x04004423 RID: 17443
			public bool pinAuthoredTransform = true;
		}
	}
}
