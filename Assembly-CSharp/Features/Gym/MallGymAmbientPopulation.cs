using System;
using System.Collections.Generic;
using Features.NPC;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Features.Gym
{
	// Token: 0x020011B6 RID: 4534
	[DisallowMultipleComponent]
	public sealed class MallGymAmbientPopulation : MonoBehaviour
	{
		// Token: 0x17001635 RID: 5685
		// (get) Token: 0x06008B27 RID: 35623 RVA: 0x0038DB9F File Offset: 0x0038BD9F
		public IReadOnlyList<MallGymAmbientPopulation.AthleteBinding> Athletes
		{
			get
			{
				return this.athletes;
			}
		}

		// Token: 0x17001636 RID: 5686
		// (get) Token: 0x06008B28 RID: 35624 RVA: 0x0038DBA7 File Offset: 0x0038BDA7
		public IReadOnlyList<MallGymAmbientPopulation.StationReservation> ReservedStations
		{
			get
			{
				return this.reservedStations;
			}
		}

		// Token: 0x06008B29 RID: 35625 RVA: 0x0038DBAF File Offset: 0x0038BDAF
		public void Configure(MallGymAmbientPopulation.AthleteBinding[] newAthletes, MallGymAmbientPopulation.StationReservation[] newReservations)
		{
			this.athletes = newAthletes ?? Array.Empty<MallGymAmbientPopulation.AthleteBinding>();
			this.reservedStations = newReservations ?? Array.Empty<MallGymAmbientPopulation.StationReservation>();
		}

		// Token: 0x06008B2A RID: 35626 RVA: 0x0038DBD4 File Offset: 0x0038BDD4
		public void RestoreReservedStations()
		{
			if (this.reservedStations == null)
			{
				return;
			}
			for (int i = 0; i < this.reservedStations.Length; i++)
			{
				MallGymAmbientPopulation.StationReservation stationReservation = this.reservedStations[i];
				if (stationReservation != null && stationReservation.station != null)
				{
					stationReservation.station.SetAmbientOccupied(false);
				}
			}
		}

		// Token: 0x06008B2B RID: 35627 RVA: 0x0038DC23 File Offset: 0x0038BE23
		private void OnEnable()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.ApplyReservations();
			this.StartAthletes();
		}

		// Token: 0x06008B2C RID: 35628 RVA: 0x0038DC39 File Offset: 0x0038BE39
		private void OnDisable()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.StopAthletes();
		}

		// Token: 0x06008B2D RID: 35629 RVA: 0x0038DC39 File Offset: 0x0038BE39
		private void OnDestroy()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.StopAthletes();
		}

		// Token: 0x06008B2E RID: 35630 RVA: 0x0038DC4C File Offset: 0x0038BE4C
		private void LateUpdate()
		{
			if (this.athletes == null || this._authoredLocalPositions.Length != this.athletes.Length)
			{
				return;
			}
			for (int i = 0; i < this.athletes.Length; i++)
			{
				MallGymAmbientPopulation.AthleteBinding athleteBinding = this.athletes[i];
				if (athleteBinding != null && athleteBinding.pinAuthoredTransform && !(athleteBinding.characterRoot == null))
				{
					Transform transform = athleteBinding.characterRoot.transform;
					if (athleteBinding.stationAnchor != null)
					{
						Vector3 position = athleteBinding.stationAnchor.position;
						Quaternion rotation = athleteBinding.stationAnchor.rotation;
						if (!transform.position.Equals(position) || !transform.rotation.Equals(rotation))
						{
							transform.SetPositionAndRotation(position, rotation);
						}
					}
					else
					{
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
		}

		// Token: 0x06008B2F RID: 35631 RVA: 0x0038DD7C File Offset: 0x0038BF7C
		private void ApplyReservations()
		{
			if (this.reservedStations == null)
			{
				return;
			}
			for (int i = 0; i < this.reservedStations.Length; i++)
			{
				MallGymAmbientPopulation.StationReservation stationReservation = this.reservedStations[i];
				if (stationReservation != null && stationReservation.station != null)
				{
					stationReservation.station.SetAmbientOccupied(true);
				}
			}
		}

		// Token: 0x06008B30 RID: 35632 RVA: 0x0038DDCC File Offset: 0x0038BFCC
		private void StartAthletes()
		{
			this.StopAthletes();
			int num = ((this.athletes != null) ? this.athletes.Length : 0);
			this._authoredLocalPositions = new Vector3[num];
			this._authoredLocalRotations = new Quaternion[num];
			for (int i = 0; i < num; i++)
			{
				MallGymAmbientPopulation.AthleteBinding athleteBinding = this.athletes[i];
				if (athleteBinding != null && !(athleteBinding.characterRoot == null))
				{
					GameObject characterRoot = athleteBinding.characterRoot;
					Transform transform = characterRoot.transform;
					this._authoredLocalPositions[i] = transform.localPosition;
					this._authoredLocalRotations[i] = transform.localRotation;
					MallGymAmbientPopulation.DisableDynamicNpcLogic(characterRoot);
					Animator animator = ((athleteBinding.animator != null) ? athleteBinding.animator : characterRoot.GetComponentInChildren<Animator>(true));
					athleteBinding.animator = animator;
					if (animator != null && athleteBinding.animation != null)
					{
						animator.enabled = true;
						animator.applyRootMotion = false;
						PlayableGraph playableGraph;
						AnimationClipPlayable animationClipPlayable = AnimationPlayableUtilities.PlayClip(animator, athleteBinding.animation, out playableGraph);
						animationClipPlayable.SetApplyFootIK(athleteBinding.treadmill != null);
						animationClipPlayable.SetTime((double)(athleteBinding.animation.length * Mathf.Clamp(athleteBinding.normalizedStartTime, 0f, 0.999f)));
						animationClipPlayable.SetSpeed((double)Mathf.Max(0.05f, athleteBinding.animationSpeed));
						playableGraph.Play();
						playableGraph.Evaluate(0f);
						this._graphs.Add(playableGraph);
					}
					if (athleteBinding.treadmill != null)
					{
						athleteBinding.treadmill.SetAmbientRunning(true, athleteBinding.animationSpeed);
					}
					MallGymAmbientPopulation.EnsureLookAtDriver(characterRoot);
				}
			}
		}

		// Token: 0x06008B31 RID: 35633 RVA: 0x0038DF78 File Offset: 0x0038C178
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
				fieldPlayerLookAtCiel.lookRange = 10f;
				fieldPlayerLookAtCiel.expressionParamName = string.Empty;
			}
			fieldPlayerLookAtCiel.lookAt = npcsimpleLookAt;
			fieldPlayerLookAtCiel.enabled = true;
		}

		// Token: 0x06008B32 RID: 35634 RVA: 0x0038DFF0 File Offset: 0x0038C1F0
		private void StopAthletes()
		{
			if (this.athletes != null)
			{
				for (int i = 0; i < this.athletes.Length; i++)
				{
					MallGymAmbientPopulation.AthleteBinding athleteBinding = this.athletes[i];
					if (athleteBinding != null && athleteBinding.treadmill != null)
					{
						athleteBinding.treadmill.SetAmbientRunning(false, 1f);
					}
				}
			}
			for (int j = 0; j < this._graphs.Count; j++)
			{
				PlayableGraph playableGraph = this._graphs[j];
				if (playableGraph.IsValid())
				{
					playableGraph.Destroy();
				}
			}
			this._graphs.Clear();
			if (this.athletes != null)
			{
				for (int k = 0; k < this.athletes.Length; k++)
				{
					MallGymAmbientPopulation.AthleteBinding athleteBinding2 = this.athletes[k];
					if (athleteBinding2 != null)
					{
						if (athleteBinding2.animator != null)
						{
							athleteBinding2.animator.enabled = false;
						}
						if (athleteBinding2.characterRoot != null)
						{
							FieldPlayerLookAtCiel component = athleteBinding2.characterRoot.GetComponent<FieldPlayerLookAtCiel>();
							if (component != null)
							{
								component.enabled = false;
							}
						}
					}
				}
			}
		}

		// Token: 0x06008B33 RID: 35635 RVA: 0x0038E0FC File Offset: 0x0038C2FC
		public static void DisableDynamicNpcLogic(GameObject character)
		{
			if (character == null)
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

		// Token: 0x04008E04 RID: 36356
		[SerializeField]
		private MallGymAmbientPopulation.AthleteBinding[] athletes = Array.Empty<MallGymAmbientPopulation.AthleteBinding>();

		// Token: 0x04008E05 RID: 36357
		[SerializeField]
		private MallGymAmbientPopulation.StationReservation[] reservedStations = Array.Empty<MallGymAmbientPopulation.StationReservation>();

		// Token: 0x04008E06 RID: 36358
		private readonly List<PlayableGraph> _graphs = new List<PlayableGraph>(8);

		// Token: 0x04008E07 RID: 36359
		private Vector3[] _authoredLocalPositions = Array.Empty<Vector3>();

		// Token: 0x04008E08 RID: 36360
		private Quaternion[] _authoredLocalRotations = Array.Empty<Quaternion>();

		// Token: 0x020011B7 RID: 4535
		[Serializable]
		public sealed class AthleteBinding
		{
			// Token: 0x04008E09 RID: 36361
			public GameObject characterRoot;

			// Token: 0x04008E0A RID: 36362
			public Animator animator;

			// Token: 0x04008E0B RID: 36363
			public AnimationClip animation;

			// Token: 0x04008E0C RID: 36364
			public Transform stationAnchor;

			// Token: 0x04008E0D RID: 36365
			public GymTreadmillBeltScroller treadmill;

			// Token: 0x04008E0E RID: 36366
			[Min(0.05f)]
			public float animationSpeed = 1f;

			// Token: 0x04008E0F RID: 36367
			[Range(0f, 0.999f)]
			public float normalizedStartTime;

			// Token: 0x04008E10 RID: 36368
			public bool pinAuthoredTransform = true;
		}

		// Token: 0x020011B8 RID: 4536
		[Serializable]
		public sealed class StationReservation
		{
			// Token: 0x04008E11 RID: 36369
			public GymExerciseStation station;
		}
	}
}
