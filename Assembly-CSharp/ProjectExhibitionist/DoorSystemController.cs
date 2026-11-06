using System;
using System.Collections.Generic;
using Features.Detection;
using UnityEngine;

namespace ProjectExhibitionist
{
	// Token: 0x020008DA RID: 2266
	public class DoorSystemController : MonoBehaviour
	{
		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06003D37 RID: 15671 RVA: 0x0016BE29 File Offset: 0x0016A029
		// (set) Token: 0x06003D38 RID: 15672 RVA: 0x0016BE30 File Offset: 0x0016A030
		public static DoorSystemController Instance { get; private set; }

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x06003D39 RID: 15673 RVA: 0x0016BE38 File Offset: 0x0016A038
		// (set) Token: 0x06003D3A RID: 15674 RVA: 0x0016BE40 File Offset: 0x0016A040
		public bool SimulationPaused
		{
			get
			{
				return this._simulationPaused;
			}
			set
			{
				if (this._simulationPaused == value)
				{
					return;
				}
				if (value)
				{
					this._simulationPauseStartedAt = Time.time;
					this._simulationPaused = true;
					return;
				}
				float num = Mathf.Max(0f, Time.time - this._simulationPauseStartedAt);
				this._nextRescan += num;
				this._nextPoll += num;
				int num2 = ((this.doors != null) ? this.doors.Count : 0);
				for (int i = 0; i < num2; i++)
				{
					DoorInteraction doorInteraction = this.doors[i];
					if (!(doorInteraction == null))
					{
						float num3;
						if (this._npcLeftAt.TryGetValue(doorInteraction, out num3))
						{
							this._npcLeftAt[doorInteraction] = num3 + num;
						}
						doorInteraction.ShiftSimulationTime(num);
					}
				}
				this._simulationPaused = false;
			}
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x0016BF07 File Offset: 0x0016A107
		private void Awake()
		{
			if (DoorSystemController.Instance != null && DoorSystemController.Instance != this)
			{
				Object.Destroy(this);
				return;
			}
			DoorSystemController.Instance = this;
		}

		// Token: 0x06003D3C RID: 15676 RVA: 0x0016BF30 File Offset: 0x0016A130
		private void Start()
		{
			if (this.doors == null || this.doors.Count == 0)
			{
				this.doors = new List<DoorInteraction>(Object.FindObjectsOfType<DoorInteraction>(true));
			}
			if (this.recenterPromptsOnStart)
			{
				foreach (DoorInteraction doorInteraction in this.doors)
				{
					if (doorInteraction != null)
					{
						doorInteraction.RecenterPrompt();
					}
				}
			}
			if (this.applyAudioOnStart)
			{
				this.ApplyDoorAudioSettings();
			}
			foreach (DoorInteraction doorInteraction2 in this.doors)
			{
				if (!(doorInteraction2 == null))
				{
					this._npcInZone[doorInteraction2] = false;
					this._npcLeftAt[doorInteraction2] = -999f;
				}
			}
			this._audio = base.GetComponent<AudioSource>();
			if (this._audio == null)
			{
				this._audio = base.gameObject.AddComponent<AudioSource>();
			}
			this._audio.playOnAwake = false;
			this._audio.spatialBlend = 0f;
			this.RescanNpcs();
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x0016C078 File Offset: 0x0016A278
		private void OnValidate()
		{
			if (Application.isPlaying && this.applyAudioOnStart && this.doors != null && this.doors.Count > 0)
			{
				this.ApplyDoorAudioSettings();
			}
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x0016C0A8 File Offset: 0x0016A2A8
		private void Update()
		{
			if (this._simulationPaused || !this.npcsCanOpenDoors)
			{
				return;
			}
			if (Time.time >= this._nextRescan)
			{
				this._nextRescan = Time.time + Mathf.Max(0.25f, this.npcRescanInterval);
				this.RescanNpcs();
			}
			if (Time.time >= this._nextPoll)
			{
				this._nextPoll = Time.time + Mathf.Max(0.05f, this.npcPollInterval);
				this.PollDoorsForNpcs();
			}
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x0016C124 File Offset: 0x0016A324
		private void RescanNpcs()
		{
			this._npcs.Clear();
			this._npcs.AddRange(Object.FindObjectsOfType<NPCSenses>(false));
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x0016C144 File Offset: 0x0016A344
		private void PollDoorsForNpcs()
		{
			foreach (DoorInteraction doorInteraction in this.doors)
			{
				if (!(doorInteraction == null))
				{
					Vector3 doorCenter = doorInteraction.GetDoorCenter();
					Vector3 frontNormal = doorInteraction.GetFrontNormal();
					bool flag;
					bool flag2;
					this.ScanNpcs(doorCenter, frontNormal, out flag, out flag2);
					bool flag4;
					bool flag3 = this._npcInZone.TryGetValue(doorInteraction, out flag4) && flag4;
					if (Time.time - doorInteraction.LastPlayerInteractTime < this.npcPlayerYieldSeconds)
					{
						this._npcOpened.Remove(doorInteraction);
						this._openRollFailed.Remove(doorInteraction);
						this._npcInZone[doorInteraction] = flag2;
					}
					else
					{
						if (!doorInteraction.IsOpen)
						{
							if (this._npcOpened.Contains(doorInteraction) && flag2)
							{
								doorInteraction.ForceOpen();
								this.PlayCommon(this.commonOpenSound);
								if (this.verboseNpcLogs)
								{
									Debug.Log("[Doors] NPC reopened '" + doorInteraction.name + "'.", doorInteraction);
								}
							}
							else if (flag && !this._npcOpened.Contains(doorInteraction) && !this._openRollFailed.Contains(doorInteraction))
							{
								if (Random.value < this.npcOpenChance)
								{
									doorInteraction.ForceOpen();
									this.PlayCommon(this.commonOpenSound);
									this._npcOpened.Add(doorInteraction);
									if (this.verboseNpcLogs)
									{
										Debug.Log("[Doors] NPC opened '" + doorInteraction.name + "'.", doorInteraction);
									}
								}
								else
								{
									this._openRollFailed.Add(doorInteraction);
									if (this.verboseNpcLogs)
									{
										Debug.Log("[Doors] NPC ignored '" + doorInteraction.name + "' (open-chance roll failed).", doorInteraction);
									}
								}
							}
						}
						if (!flag2 && flag3)
						{
							this._npcLeftAt[doorInteraction] = Time.time;
							this._openRollFailed.Remove(doorInteraction);
						}
						if (!flag2 && this._npcOpened.Contains(doorInteraction))
						{
							float num2;
							float num = (this._npcLeftAt.TryGetValue(doorInteraction, out num2) ? num2 : Time.time);
							if (Time.time - num >= this.npcCloseDelay)
							{
								if (Random.value < this.npcCloseChance && doorInteraction.IsOpen)
								{
									doorInteraction.ForceClose();
									this.PlayCommon(this.commonCloseSound);
									if (this.verboseNpcLogs)
									{
										Debug.Log("[Doors] NPC closed '" + doorInteraction.name + "' after leaving.", doorInteraction);
									}
								}
								this._npcOpened.Remove(doorInteraction);
							}
						}
						this._npcInZone[doorInteraction] = flag2;
					}
				}
			}
		}

		// Token: 0x06003D41 RID: 15681 RVA: 0x0016C3F4 File Offset: 0x0016A5F4
		private void ScanNpcs(Vector3 doorCenter, Vector3 frontNormal, out bool approaching, out bool nearby)
		{
			approaching = false;
			nearby = false;
			float num = Mathf.Max(this.npcNearbyDistance, this.npcOpenDistance);
			for (int i = 0; i < this._npcs.Count; i++)
			{
				NPCSenses npcsenses = this._npcs[i];
				if (!(npcsenses == null))
				{
					Vector3 position = npcsenses.transform.position;
					if (Mathf.Abs(position.y - doorCenter.y) <= this.npcMaxHeightDelta)
					{
						Vector3 vector = doorCenter - position;
						vector.y = 0f;
						float magnitude = vector.magnitude;
						if (magnitude <= num)
						{
							if (this.npcRequireFrontSide)
							{
								Vector3 vector2 = position - doorCenter;
								vector2.y = 0f;
								if (Vector3.Dot(vector2, frontNormal) < 0f)
								{
									goto IL_0129;
								}
							}
							nearby = true;
							if (magnitude <= this.npcOpenDistance)
							{
								if (magnitude < 0.0001f)
								{
									approaching = true;
								}
								else
								{
									Vector3 vector3 = vector / magnitude;
									Vector3 forward = npcsenses.transform.forward;
									forward.y = 0f;
									if (forward.sqrMagnitude <= 0.0001f)
									{
										approaching = true;
									}
									else if (Vector3.Dot(forward.normalized, vector3) >= this.npcFacingDot)
									{
										approaching = true;
									}
								}
							}
							if (approaching & nearby)
							{
								return;
							}
						}
					}
				}
				IL_0129:;
			}
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x0016C53F File Offset: 0x0016A73F
		private void PlayCommon(AudioClip clip)
		{
			if (clip == null || this._audio == null)
			{
				return;
			}
			this._audio.PlayOneShot(clip, this.commonVolume);
		}

		// Token: 0x06003D43 RID: 15683 RVA: 0x0016C56C File Offset: 0x0016A76C
		public void RecenterAllPrompts()
		{
			foreach (DoorInteraction doorInteraction in this.doors)
			{
				if (doorInteraction != null)
				{
					doorInteraction.RecenterPrompt();
				}
			}
		}

		// Token: 0x06003D44 RID: 15684 RVA: 0x0016C5C8 File Offset: 0x0016A7C8
		public void ApplyDoorAudioSettings()
		{
			foreach (DoorInteraction doorInteraction in this.doors)
			{
				if (!(doorInteraction == null))
				{
					foreach (AudioSource audioSource in doorInteraction.GetComponentsInChildren<AudioSource>(true))
					{
						audioSource.volume = this.doorVolume;
						audioSource.spatialBlend = this.doorSpatialBlend;
						audioSource.rolloffMode = AudioRolloffMode.Linear;
						audioSource.minDistance = this.doorMinDistance;
						audioSource.maxDistance = this.doorMaxDistance;
					}
				}
			}
		}

		// Token: 0x04003D19 RID: 15641
		[Header("Doors")]
		[Tooltip("If empty, all DoorInteraction in the scene are collected at startup.")]
		[SerializeField]
		private List<DoorInteraction> doors = new List<DoorInteraction>();

		// Token: 0x04003D1A RID: 15642
		[Tooltip("Re-centre every door's prompt UI on the door's visual centre at startup.")]
		[SerializeField]
		private bool recenterPromptsOnStart = true;

		// Token: 0x04003D1B RID: 15643
		[Header("Door Audio — controls EVERY door's AudioSource from here")]
		[Tooltip("ONE volume for ALL doors. Applied to every door's own AudioSource at startup (the third-party Door plays its open/close sounds through that AudioSource). Change this instead of editing each door — it overrides the per-door volume.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float doorVolume = 0.3f;

		// Token: 0x04003D1C RID: 15644
		[Tooltip("Make the door sounds 3D (attenuate with distance). The doors ship as 2D (spatialBlend=0), which is why they sound full-volume everywhere. 1 = fully 3D.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float doorSpatialBlend = 1f;

		// Token: 0x04003D1D RID: 15645
		[Tooltip("Distance (m) at which a 3D door sound starts fading. Below this it's at full (doorVolume) loudness.")]
		[SerializeField]
		private float doorMinDistance = 1.5f;

		// Token: 0x04003D1E RID: 15646
		[Tooltip("Distance (m) beyond which a 3D door sound is fully faded out.")]
		[SerializeField]
		private float doorMaxDistance = 12f;

		// Token: 0x04003D1F RID: 15647
		[Tooltip("Re-apply the audio settings to all doors at startup.")]
		[SerializeField]
		private bool applyAudioOnStart = true;

		// Token: 0x04003D20 RID: 15648
		[Header("Common Audio (optional, played by the controller itself)")]
		[Tooltip("Optional extra open/close clips played by the controller (NOT the per-door sounds). Usually leave empty — the doors already make their own sounds.")]
		[SerializeField]
		private AudioClip commonOpenSound;

		// Token: 0x04003D21 RID: 15649
		[SerializeField]
		private AudioClip commonCloseSound;

		// Token: 0x04003D22 RID: 15650
		[Range(0f, 1f)]
		[SerializeField]
		private float commonVolume = 0.8f;

		// Token: 0x04003D23 RID: 15651
		[Header("NPC Behaviour")]
		[Tooltip("Let NPCs open doors by walking into their zone (no changes to the NPCs).")]
		[SerializeField]
		private bool npcsCanOpenDoors = true;

		// Token: 0x04003D24 RID: 15652
		[Tooltip("How often (seconds) to refresh the list of spawned NPCs.")]
		[SerializeField]
		private float npcRescanInterval = 2f;

		// Token: 0x04003D25 RID: 15653
		[Tooltip("How often (seconds) to test NPC-vs-door proximity. Cheap, but no need every frame.")]
		[SerializeField]
		private float npcPollInterval = 0.2f;

		// Token: 0x04003D26 RID: 15654
		[Tooltip("Chance (0..1) that an NPC OPENS a door when it first walks up to it. Rolled ONCE per approach (not every frame); on a miss that NPC ignores the door until it walks away and comes back. 1 = always opens, 0 = never. NOTE: a door the player closed while the NPC is still there is always reopened regardless of this chance.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float npcOpenChance = 1f;

		// Token: 0x04003D27 RID: 15655
		[Tooltip("Chance (0..1) that a door CLOSES when the last NPC leaves its zone. Otherwise the NPC leaves it open.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float npcCloseChance = 0.5f;

		// Token: 0x04003D28 RID: 15656
		[Tooltip("Extra seconds an NPC-opened door stays open after the NPC has left, before the random close roll happens.")]
		[SerializeField]
		private float npcCloseDelay = 1.5f;

		// Token: 0x04003D29 RID: 15657
		[Tooltip("After the PLAYER presses the interact key on a door, the NPC system ignores that door for this many seconds (it 'lets go' so it never fights the player). After it elapses, NPCs treat the door fresh again. Keep short, e.g. 0.5–1s.")]
		[SerializeField]
		private float npcPlayerYieldSeconds = 0.75f;

		// Token: 0x04003D2A RID: 15658
		[Header("NPC Approach (must walk INTO the door from the front, not pass by)")]
		[Tooltip("Max horizontal distance (m) from the door centre for an NPC to open it. Keep small so NPCs only open doors they actually walk up to, not ones they pass in a corridor.")]
		[SerializeField]
		private float npcOpenDistance = 1.1f;

		// Token: 0x04003D2B RID: 15659
		[Tooltip("Wider radius (m) within which an NPC counts as 'still near' the door. While an NPC is within this, the door is kept open and the auto-close timer does not start — so a door isn't shut in an NPC's face the moment it stops looking at the door. Should be >= Npc Open Distance.")]
		[SerializeField]
		private float npcNearbyDistance = 2.2f;

		// Token: 0x04003D2C RID: 15660
		[Tooltip("How directly the NPC must be heading AT the door. dot(npcForward, dirToDoor) must be above this. 1 = straight at it, 0 = sideways. 0.6 ≈ within ~53° of facing the door, so someone walking PARALLEL down a corridor (dot≈0) won't trigger it.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float npcFacingDot = 0.6f;

		// Token: 0x04003D2D RID: 15661
		[Tooltip("Also require the NPC to be on the FRONT side of the door (in front of its face), not approaching the back. Uncheck to allow opening from either side.")]
		[SerializeField]
		private bool npcRequireFrontSide;

		// Token: 0x04003D2E RID: 15662
		[Tooltip("Max height difference (m) between NPC and door centre, so an NPC on another floor lined up above/below can't open it.")]
		[SerializeField]
		private float npcMaxHeightDelta = 1.6f;

		// Token: 0x04003D2F RID: 15663
		[Tooltip("Log to the console every time an NPC opens a door (and whether it had been closed by the player), so you can confirm the NPC-opens-doors behaviour works.")]
		[SerializeField]
		private bool verboseNpcLogs;

		// Token: 0x04003D30 RID: 15664
		private readonly List<NPCSenses> _npcs = new List<NPCSenses>();

		// Token: 0x04003D31 RID: 15665
		private float _nextRescan;

		// Token: 0x04003D32 RID: 15666
		private float _nextPoll;

		// Token: 0x04003D33 RID: 15667
		private readonly Dictionary<DoorInteraction, bool> _npcInZone = new Dictionary<DoorInteraction, bool>();

		// Token: 0x04003D34 RID: 15668
		private readonly Dictionary<DoorInteraction, float> _npcLeftAt = new Dictionary<DoorInteraction, float>();

		// Token: 0x04003D35 RID: 15669
		private readonly HashSet<DoorInteraction> _npcOpened = new HashSet<DoorInteraction>();

		// Token: 0x04003D36 RID: 15670
		private readonly HashSet<DoorInteraction> _openRollFailed = new HashSet<DoorInteraction>();

		// Token: 0x04003D37 RID: 15671
		private AudioSource _audio;

		// Token: 0x04003D38 RID: 15672
		private bool _simulationPaused;

		// Token: 0x04003D39 RID: 15673
		private float _simulationPauseStartedAt;
	}
}
