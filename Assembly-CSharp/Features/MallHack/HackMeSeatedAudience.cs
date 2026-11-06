using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Bozo.AnimeCharacters;
using Features.CensorBars;
using Features.Detection;
using Features.NPC;
using Features.NPC.Audio;
using ProjectExhibitionist.NPCs;
using UnityEngine;

namespace Features.MallHack
{
	// Token: 0x0200101B RID: 4123
	[DisallowMultipleComponent]
	public sealed class HackMeSeatedAudience : MonoBehaviour
	{
		// Token: 0x17001429 RID: 5161
		// (get) Token: 0x06007D0E RID: 32014 RVA: 0x00333447 File Offset: 0x00331647
		public HackMeStoreZone Zone
		{
			get
			{
				return this.zone;
			}
		}

		// Token: 0x1700142A RID: 5162
		// (get) Token: 0x06007D0F RID: 32015 RVA: 0x00333450 File Offset: 0x00331650
		public int ReactingCount
		{
			get
			{
				int num = 0;
				using (List<HackMeSeatedAudience.Member>.Enumerator enumerator = this._members.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Started)
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x06007D10 RID: 32016 RVA: 0x003334AC File Offset: 0x003316AC
		public static HackMeSeatedAudience.Intensity ClassifyPayload(string sourceName, bool video)
		{
			if (!HackMeStoreZone.IsProvocativePayload(sourceName, video))
			{
				return HackMeSeatedAudience.Intensity.None;
			}
			return HackMeSeatedAudience.Intensity.Strong;
		}

		// Token: 0x06007D11 RID: 32017 RVA: 0x003334BA File Offset: 0x003316BA
		private void OnEnable()
		{
			HackMeStoreZone.BroadcastStarted += this.HandleBroadcastStarted;
		}

		// Token: 0x06007D12 RID: 32018 RVA: 0x003334CD File Offset: 0x003316CD
		private void OnDisable()
		{
			HackMeStoreZone.BroadcastStarted -= this.HandleBroadcastStarted;
			this.CalmEveryone();
			if (this.streamerCensor != null)
			{
				this.streamerCensor.enabled = false;
			}
		}

		// Token: 0x06007D13 RID: 32019 RVA: 0x00333500 File Offset: 0x00331700
		private void OnDestroy()
		{
			foreach (AnimatorOverrideController[] array in this._coverControllers.Values)
			{
				foreach (AnimatorOverrideController animatorOverrideController in array)
				{
					if (animatorOverrideController != null)
					{
						Object.Destroy(animatorOverrideController);
					}
				}
			}
			this._coverControllers.Clear();
			if (this._lookTarget != null)
			{
				Object.Destroy(this._lookTarget.gameObject);
			}
		}

		// Token: 0x06007D14 RID: 32020 RVA: 0x0033359C File Offset: 0x0033179C
		private void HandleBroadcastStarted(HackMeStoreZone source, string sourceName, bool video)
		{
			if (source == null || source != this.zone || !base.isActiveAndEnabled)
			{
				return;
			}
			HackMeSeatedAudience.Intensity intensity = HackMeSeatedAudience.ClassifyPayload(sourceName, video);
			if (intensity == HackMeSeatedAudience.Intensity.None)
			{
				this.CalmEveryone();
				return;
			}
			float num = (video ? this.videoReactionSeconds : this.photoReactionSeconds);
			this.CollectMembers();
			this.EnsureLookTarget();
			List<HackMeSeatedAudience.Member> list = new List<HackMeSeatedAudience.Member>();
			foreach (HackMeSeatedAudience.Member member in this._members)
			{
				if (member.Scheduled)
				{
					member.EndAt = Mathf.Max(member.EndAt, this._clock + num + Random.Range(0f, this.endSpreadSeconds));
				}
				else
				{
					list.Add(member);
				}
			}
			HackMeSeatedAudience.Shuffle(list);
			int num2 = ((intensity == HackMeSeatedAudience.Intensity.Strong) ? Mathf.RoundToInt((float)list.Count * this.strongCoverShare) : 0);
			int num3 = Mathf.RoundToInt((float)list.Count * ((intensity == HackMeSeatedAudience.Intensity.Strong) ? this.strongPhoneShare : this.mildPhoneShare));
			Vector3 vector = ((this._lookTarget != null) ? this._lookTarget.position : this.zone.WorldCenter);
			for (int i = 0; i < list.Count; i++)
			{
				HackMeSeatedAudience.Member member2 = list[i];
				member2.Role = ((i < num2 && this.coverClips.Length != 0) ? HackMeSeatedAudience.Role.Cover : ((i < num2 + num3) ? HackMeSeatedAudience.Role.Phone : HackMeSeatedAudience.Role.Look));
				member2.PhoneType = ((Random.value < 0.6f) ? 1 : 2);
				float num4 = Vector3.Distance(member2.Seat.transform.position, vector);
				member2.StartAt = this._clock + Mathf.Clamp01(num4 / 25f) * this.startSpreadSeconds * 0.6f + Random.Range(0f, this.startSpreadSeconds * 0.4f);
				member2.EndAt = this._clock + num + Random.Range(0f, this.endSpreadSeconds);
				member2.Scheduled = true;
				member2.Started = false;
				member2.FaceIndex = HackMeSeatedAudience.PickFace(intensity, member2.Seat.GetInstanceID());
			}
			if (intensity == HackMeSeatedAudience.Intensity.Strong && list.Count > 0)
			{
				for (int j = 0; j < this.strongScreams; j++)
				{
					this._screams.Add(new ValueTuple<float, HackMeSeatedAudience.Member>(this._clock + 0.45f + (float)j * Random.Range(0.7f, 1.5f), list[Random.Range(0, list.Count)]));
				}
			}
		}

		// Token: 0x06007D15 RID: 32021 RVA: 0x00333850 File Offset: 0x00331A50
		private void Update()
		{
			if (this.streamerCensor != null)
			{
				bool flag = this.zone != null && this.zone.HasActiveHack;
				if (this.streamerCensor.enabled != flag)
				{
					this.streamerCensor.enabled = flag;
				}
			}
			if (this._members.Count == 0 && this._screams.Count == 0)
			{
				return;
			}
			if (this.zone == null || !this.zone.isActiveAndEnabled || !this.zone.CanProvokeNpcReactions)
			{
				this.CalmEveryone();
				return;
			}
			this._clock += (NPCSimulationPause.IsPaused ? 0f : Time.deltaTime);
			for (int i = 0; i < this._members.Count; i++)
			{
				HackMeSeatedAudience.Member member = this._members[i];
				if (!(member.Seat == null))
				{
					if (member.Scheduled && !member.Started && this._clock >= member.StartAt)
					{
						if (member.Animator != null && member.Animator.isActiveAndEnabled)
						{
							this.Begin(member);
						}
						else if (this._clock >= member.EndAt)
						{
							member.Scheduled = false;
						}
					}
					if (member.Started)
					{
						if (this._clock >= member.EndAt)
						{
							this.End(member);
						}
						else
						{
							this.TickFlash(member);
						}
					}
				}
			}
			for (int j = this._screams.Count - 1; j >= 0; j--)
			{
				if (this._clock >= this._screams[j].Item1)
				{
					this.Scream(this._screams[j].Item2);
					this._screams.RemoveAt(j);
				}
			}
		}

		// Token: 0x06007D16 RID: 32022 RVA: 0x00333A14 File Offset: 0x00331C14
		private void CollectMembers()
		{
			this._members.RemoveAll((HackMeSeatedAudience.Member member) => member.Seat == null);
			if (this.zone == null)
			{
				return;
			}
			SeatedRandomReaction[] array = Object.FindObjectsOfType<SeatedRandomReaction>();
			for (int i = 0; i < array.Length; i++)
			{
				SeatedRandomReaction seat = array[i];
				if (!(seat == null) && !(seat.gameObject.scene != this.zone.gameObject.scene) && this.zone.Contains(seat.transform.position) && !this._members.Exists((HackMeSeatedAudience.Member m) => m.Seat == seat))
				{
					HackMeSeatedAudience.Member member2 = new HackMeSeatedAudience.Member
					{
						Seat = seat,
						Animator = ((seat.animator != null) ? seat.animator : seat.GetComponentInChildren<Animator>()),
						Look = seat.GetComponentInChildren<NPCSimpleLookAt>(true),
						Field = seat.GetComponentInChildren<FieldPlayerLookAtCiel>(true),
						Equipment = seat.GetComponentInChildren<NPCEquipmentManager>(true),
						Personality = seat.GetComponentInChildren<NPCDialogueHolder>(true)
					};
					NPCReactionAudioPlayer componentInChildren = seat.GetComponentInChildren<NPCReactionAudioPlayer>(true);
					member2.Voice = ((componentInChildren != null) ? componentInChildren.GetComponent<AudioSource>() : null);
					this._members.Add(member2);
				}
			}
		}

		// Token: 0x06007D17 RID: 32023 RVA: 0x00333BBC File Offset: 0x00331DBC
		private void Begin(HackMeSeatedAudience.Member member)
		{
			member.Started = true;
			if (member.Marker == null)
			{
				member.Marker = member.Seat.GetComponent<HackMeAudienceMember>();
			}
			if (member.Marker == null)
			{
				member.Marker = member.Seat.gameObject.AddComponent<HackMeAudienceMember>();
			}
			member.Marker.IsBusy = true;
			member.SeatWasEnabled = member.Seat.enabled;
			member.Seat.enabled = false;
			if (member.Field != null)
			{
				member.FieldWasEnabled = member.Field.enabled;
				member.Field.enabled = false;
			}
			if (member.Look != null)
			{
				member.LookHorizontalOnly = member.Look.horizontalOnly;
				member.LookMaxVertical = member.Look.maxVerticalAngle;
				member.LookMaxHorizontal = member.Look.maxHorizontalAngle;
				member.Look.horizontalOnly = false;
				member.Look.maxVerticalAngle = 35f;
				member.Look.maxHorizontalAngle = 80f;
				if (member.Role == HackMeSeatedAudience.Role.Cover)
				{
					member.Look.StopLooking();
				}
				else if (this._lookTarget != null)
				{
					member.Look.StartLookingAt(this._lookTarget);
				}
			}
			Animator animator = member.Animator;
			if (animator == null)
			{
				return;
			}
			animator.SetInteger(HackMeSeatedAudience.FaceHash, member.FaceIndex);
			if (member.Role == HackMeSeatedAudience.Role.Cover && !this.StartCover(member))
			{
				member.Role = HackMeSeatedAudience.Role.Phone;
			}
			if (member.Role == HackMeSeatedAudience.Role.Phone)
			{
				this.StartPhone(member);
			}
		}

		// Token: 0x06007D18 RID: 32024 RVA: 0x00333D58 File Offset: 0x00331F58
		private void StartPhone(HackMeSeatedAudience.Member member)
		{
			Animator animator = member.Animator;
			int num = this.ReactionsLayer(member);
			if (num >= 0)
			{
				animator.SetLayerWeight(num, 1f);
			}
			animator.SetInteger(HackMeSeatedAudience.ReactionTypeHash, member.PhoneType);
			animator.ResetTrigger(HackMeSeatedAudience.ReactionTriggerHash);
			animator.SetTrigger(HackMeSeatedAudience.ReactionTriggerHash);
			if (member.Equipment != null && !string.IsNullOrEmpty(member.Seat.phonePropName))
			{
				member.Equipment.ActivateAndLockProp(member.Seat.phonePropName);
			}
			member.FlashesLeft = ((member.PhoneType == 1) ? Random.Range(3, 7) : 0);
			member.NextFlash = this._clock + Random.Range(0.5f, 1.7f);
		}

		// Token: 0x06007D19 RID: 32025 RVA: 0x00333E18 File Offset: 0x00332018
		private bool StartCover(HackMeSeatedAudience.Member member)
		{
			AnimatorOverrideController animatorOverrideController = this.CoverControllerFor(member.Animator.runtimeAnimatorController, member.Seat.GetInstanceID());
			if (animatorOverrideController == null)
			{
				return false;
			}
			member.OriginalController = member.Animator.runtimeAnimatorController;
			HackMeSeatedAudience.SwapController(member.Animator, animatorOverrideController);
			int num = this.ReactionsLayer(member);
			if (num >= 0)
			{
				member.Animator.SetLayerWeight(num, 1f);
			}
			member.Animator.SetInteger(HackMeSeatedAudience.ReactionTypeHash, 4);
			member.Animator.ResetTrigger(HackMeSeatedAudience.ReactionTriggerHash);
			member.Animator.SetTrigger(HackMeSeatedAudience.ReactionTriggerHash);
			return true;
		}

		// Token: 0x06007D1A RID: 32026 RVA: 0x00333EBC File Offset: 0x003320BC
		private void TickFlash(HackMeSeatedAudience.Member member)
		{
			if (member.Role != HackMeSeatedAudience.Role.Phone || member.FlashesLeft <= 0 || this._clock < member.NextFlash)
			{
				return;
			}
			member.FlashesLeft--;
			member.NextFlash = this._clock + Random.Range(1.4f, 3.6f);
			SmartphoneFlash componentInChildren = member.Seat.GetComponentInChildren<SmartphoneFlash>();
			if (componentInChildren != null && componentInChildren.isActiveAndEnabled)
			{
				componentInChildren.TriggerFlash();
			}
		}

		// Token: 0x06007D1B RID: 32027 RVA: 0x00333F38 File Offset: 0x00332138
		private void Scream(HackMeSeatedAudience.Member member)
		{
			if (member == null || member.Seat == null)
			{
				return;
			}
			bool flag = member.Personality != null && member.Personality.Gender == Gender.Female;
			AudioClip[] array = (flag ? this.femaleScreams : this.maleScreams);
			if (array == null || array.Length == 0)
			{
				array = (flag ? this.maleScreams : this.femaleScreams);
			}
			if (array == null || array.Length == 0)
			{
				return;
			}
			AudioClip audioClip = array[Random.Range(0, array.Length)];
			if (audioClip == null)
			{
				return;
			}
			if (member.Voice != null && member.Voice.isActiveAndEnabled)
			{
				member.Voice.PlayOneShot(audioClip, this.screamVolume);
				return;
			}
			AudioSource.PlayClipAtPoint(audioClip, member.Seat.transform.position + Vector3.up * 1.2f, this.screamVolume);
		}

		// Token: 0x06007D1C RID: 32028 RVA: 0x00334020 File Offset: 0x00332220
		private void End(HackMeSeatedAudience.Member member)
		{
			member.Started = false;
			member.Scheduled = false;
			Animator animator = member.Animator;
			if (animator != null)
			{
				animator.ResetTrigger(HackMeSeatedAudience.ReactionTriggerHash);
				animator.SetInteger(HackMeSeatedAudience.ReactionTypeHash, 0);
				if (member.OriginalController != null)
				{
					HackMeSeatedAudience.SwapController(animator, member.OriginalController);
					member.OriginalController = null;
				}
				animator.SetInteger(HackMeSeatedAudience.FaceHash, 0);
			}
			if (member.Role == HackMeSeatedAudience.Role.Phone && member.Equipment != null && !string.IsNullOrEmpty(member.Seat.phonePropName))
			{
				member.Equipment.DeactivateProp(member.Seat.phonePropName);
			}
			member.FlashesLeft = 0;
			if (member.Look != null)
			{
				member.Look.StopLooking();
				member.Look.horizontalOnly = member.LookHorizontalOnly;
				member.Look.maxVerticalAngle = member.LookMaxVertical;
				member.Look.maxHorizontalAngle = member.LookMaxHorizontal;
			}
			if (member.Field != null)
			{
				member.Field.enabled = member.FieldWasEnabled;
			}
			member.Seat.enabled = member.SeatWasEnabled;
			if (member.Marker != null)
			{
				member.Marker.IsBusy = false;
			}
		}

		// Token: 0x06007D1D RID: 32029 RVA: 0x0033416C File Offset: 0x0033236C
		private void CalmEveryone()
		{
			foreach (HackMeSeatedAudience.Member member in this._members)
			{
				if (!(member.Seat == null))
				{
					if (member.Started)
					{
						this.End(member);
					}
					member.Scheduled = false;
				}
			}
			this._screams.Clear();
		}

		// Token: 0x06007D1E RID: 32030 RVA: 0x003341E8 File Offset: 0x003323E8
		private int ReactionsLayer(HackMeSeatedAudience.Member member)
		{
			if (member.Animator == null)
			{
				return -1;
			}
			int reactionsLayerIndex = member.Seat.reactionsLayerIndex;
			if (reactionsLayerIndex < 0 || reactionsLayerIndex >= member.Animator.layerCount)
			{
				return -1;
			}
			return reactionsLayerIndex;
		}

		// Token: 0x06007D1F RID: 32031 RVA: 0x00334228 File Offset: 0x00332428
		private AnimatorOverrideController CoverControllerFor(RuntimeAnimatorController baseController, int salt)
		{
			if (baseController == null || this.coverClips == null || this.coverClips.Length == 0)
			{
				return null;
			}
			AnimatorOverrideController[] array;
			if (!this._coverControllers.TryGetValue(baseController, out array))
			{
				array = new AnimatorOverrideController[this.coverClips.Length];
				for (int i = 0; i < this.coverClips.Length; i++)
				{
					if (!(this.coverClips[i] == null))
					{
						AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(baseController)
						{
							name = baseController.name + " (HackMe cover " + i.ToString() + ")",
							hideFlags = HideFlags.DontSave
						};
						List<KeyValuePair<AnimationClip, AnimationClip>> list = new List<KeyValuePair<AnimationClip, AnimationClip>>();
						animatorOverrideController.GetOverrides(list);
						bool flag = false;
						for (int j = 0; j < list.Count; j++)
						{
							if (!(list[j].Key == null) && !(list[j].Key.name != "Manny is red carding"))
							{
								list[j] = new KeyValuePair<AnimationClip, AnimationClip>(list[j].Key, this.coverClips[i]);
								flag = true;
							}
						}
						if (!flag)
						{
							Object.Destroy(animatorOverrideController);
						}
						else
						{
							animatorOverrideController.ApplyOverrides(list);
							array[i] = animatorOverrideController;
						}
					}
				}
				this._coverControllers[baseController] = array;
			}
			int num = Mathf.Abs(salt) % array.Length;
			for (int k = 0; k < array.Length; k++)
			{
				AnimatorOverrideController animatorOverrideController2 = array[(num + k) % array.Length];
				if (animatorOverrideController2 != null)
				{
					return animatorOverrideController2;
				}
			}
			return null;
		}

		// Token: 0x06007D20 RID: 32032 RVA: 0x003343BC File Offset: 0x003325BC
		private static void SwapController(Animator animator, RuntimeAnimatorController controller)
		{
			if (animator == null || controller == null || animator.runtimeAnimatorController == controller)
			{
				return;
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			Dictionary<int, float> dictionary2 = new Dictionary<int, float>();
			Dictionary<int, bool> dictionary3 = new Dictionary<int, bool>();
			foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
			{
				if (animatorControllerParameter.type == AnimatorControllerParameterType.Int)
				{
					dictionary[animatorControllerParameter.nameHash] = animator.GetInteger(animatorControllerParameter.nameHash);
				}
				else if (animatorControllerParameter.type == AnimatorControllerParameterType.Float)
				{
					dictionary2[animatorControllerParameter.nameHash] = animator.GetFloat(animatorControllerParameter.nameHash);
				}
				else if (animatorControllerParameter.type == AnimatorControllerParameterType.Bool)
				{
					dictionary3[animatorControllerParameter.nameHash] = animator.GetBool(animatorControllerParameter.nameHash);
				}
			}
			float[] array = new float[animator.layerCount];
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = animator.GetLayerWeight(j);
			}
			AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
			animator.runtimeAnimatorController = controller;
			foreach (KeyValuePair<int, int> keyValuePair in dictionary)
			{
				animator.SetInteger(keyValuePair.Key, keyValuePair.Value);
			}
			foreach (KeyValuePair<int, float> keyValuePair2 in dictionary2)
			{
				animator.SetFloat(keyValuePair2.Key, keyValuePair2.Value);
			}
			foreach (KeyValuePair<int, bool> keyValuePair3 in dictionary3)
			{
				animator.SetBool(keyValuePair3.Key, keyValuePair3.Value);
			}
			int num = 1;
			while (num < array.Length && num < animator.layerCount)
			{
				animator.SetLayerWeight(num, array[num]);
				num++;
			}
			if (animator.HasState(0, currentAnimatorStateInfo.fullPathHash))
			{
				animator.Play(currentAnimatorStateInfo.fullPathHash, 0, Mathf.Repeat(currentAnimatorStateInfo.normalizedTime, 1f));
			}
		}

		// Token: 0x06007D21 RID: 32033 RVA: 0x0033460C File Offset: 0x0033280C
		private void EnsureLookTarget()
		{
			if (this._lookTarget != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[HackMe] Audience Look Target");
			gameObject.hideFlags = HideFlags.DontSave;
			gameObject.transform.SetParent(base.transform, false);
			gameObject.transform.position = ((this.screen != null) ? this.screen.bounds.center : (base.transform.position + Vector3.up * 3f));
			this._lookTarget = gameObject.transform;
		}

		// Token: 0x06007D22 RID: 32034 RVA: 0x003346A8 File Offset: 0x003328A8
		private static int PickFace(HackMeSeatedAudience.Intensity intensity, int salt)
		{
			int num = Mathf.Abs(salt * 7919 + 13) % 20;
			if (intensity == HackMeSeatedAudience.Intensity.Mild)
			{
				if (num >= 13)
				{
					return 3;
				}
				return 1;
			}
			else
			{
				if (num < 11)
				{
					return 1;
				}
				if (num < 15)
				{
					return 5;
				}
				if (num >= 18)
				{
					return 3;
				}
				return 4;
			}
		}

		// Token: 0x06007D23 RID: 32035 RVA: 0x003346EC File Offset: 0x003328EC
		private static void Shuffle(List<HackMeSeatedAudience.Member> list)
		{
			for (int i = list.Count - 1; i > 0; i--)
			{
				int num = Random.Range(0, i + 1);
				int num2 = i;
				int num3 = num;
				HackMeSeatedAudience.Member member = list[num];
				HackMeSeatedAudience.Member member2 = list[i];
				list[num2] = member;
				list[num3] = member2;
			}
		}

		// Token: 0x040080E0 RID: 32992
		private const string CoverSlotClipName = "Manny is red carding";

		// Token: 0x040080E1 RID: 32993
		private const int CoverReactionType = 4;

		// Token: 0x040080E2 RID: 32994
		private static readonly int ReactionTypeHash = Animator.StringToHash("ReactionType");

		// Token: 0x040080E3 RID: 32995
		private static readonly int ReactionTriggerHash = Animator.StringToHash("ReactionTrigger");

		// Token: 0x040080E4 RID: 32996
		private static readonly int FaceHash = Animator.StringToHash("FaceExpressionIndex");

		// Token: 0x040080E5 RID: 32997
		[SerializeField]
		private HackMeStoreZone zone;

		// Token: 0x040080E6 RID: 32998
		[Tooltip("The LED wall panel: students look at its centre.")]
		[SerializeField]
		private Renderer screen;

		// Token: 0x040080E7 RID: 32999
		[Tooltip("Pixelates the screen in Streamer Mode while a payload is on it.")]
		[SerializeField]
		private StreamerCensorTarget streamerCensor;

		// Token: 0x040080E8 RID: 33000
		[Header("Face covering")]
		[SerializeField]
		private AnimationClip[] coverClips = Array.Empty<AnimationClip>();

		// Token: 0x040080E9 RID: 33001
		[Header("Classic screams")]
		[SerializeField]
		private AudioClip[] maleScreams = Array.Empty<AudioClip>();

		// Token: 0x040080EA RID: 33002
		[SerializeField]
		private AudioClip[] femaleScreams = Array.Empty<AudioClip>();

		// Token: 0x040080EB RID: 33003
		[SerializeField]
		[Range(0f, 1f)]
		private float screamVolume = 0.85f;

		// Token: 0x040080EC RID: 33004
		[Header("Reaction")]
		[SerializeField]
		[Min(1f)]
		private float photoReactionSeconds = 22f;

		// Token: 0x040080ED RID: 33005
		[SerializeField]
		[Min(1f)]
		private float videoReactionSeconds = 30f;

		// Token: 0x040080EE RID: 33006
		[SerializeField]
		[Min(0f)]
		private float startSpreadSeconds = 1.6f;

		// Token: 0x040080EF RID: 33007
		[SerializeField]
		[Min(0f)]
		private float endSpreadSeconds = 4f;

		// Token: 0x040080F0 RID: 33008
		[SerializeField]
		[Range(0f, 1f)]
		private float strongPhoneShare = 0.55f;

		// Token: 0x040080F1 RID: 33009
		[SerializeField]
		[Range(0f, 1f)]
		private float strongCoverShare = 0.14f;

		// Token: 0x040080F2 RID: 33010
		[SerializeField]
		[Range(0f, 1f)]
		private float mildPhoneShare = 0.25f;

		// Token: 0x040080F3 RID: 33011
		[SerializeField]
		[Min(0f)]
		private int strongScreams = 3;

		// Token: 0x040080F4 RID: 33012
		private readonly List<HackMeSeatedAudience.Member> _members = new List<HackMeSeatedAudience.Member>();

		// Token: 0x040080F5 RID: 33013
		private readonly Dictionary<RuntimeAnimatorController, AnimatorOverrideController[]> _coverControllers = new Dictionary<RuntimeAnimatorController, AnimatorOverrideController[]>();

		// Token: 0x040080F6 RID: 33014
		[TupleElementNames(new string[] { "at", "member" })]
		private readonly List<ValueTuple<float, HackMeSeatedAudience.Member>> _screams = new List<ValueTuple<float, HackMeSeatedAudience.Member>>();

		// Token: 0x040080F7 RID: 33015
		private Transform _lookTarget;

		// Token: 0x040080F8 RID: 33016
		private float _clock;

		// Token: 0x0200101C RID: 4124
		public enum Intensity
		{
			// Token: 0x040080FA RID: 33018
			Mild,
			// Token: 0x040080FB RID: 33019
			Strong,
			// Token: 0x040080FC RID: 33020
			None
		}

		// Token: 0x0200101D RID: 4125
		private enum Role
		{
			// Token: 0x040080FE RID: 33022
			Look,
			// Token: 0x040080FF RID: 33023
			Phone,
			// Token: 0x04008100 RID: 33024
			Cover
		}

		// Token: 0x0200101E RID: 4126
		private sealed class Member
		{
			// Token: 0x04008101 RID: 33025
			public SeatedRandomReaction Seat;

			// Token: 0x04008102 RID: 33026
			public Animator Animator;

			// Token: 0x04008103 RID: 33027
			public NPCSimpleLookAt Look;

			// Token: 0x04008104 RID: 33028
			public FieldPlayerLookAtCiel Field;

			// Token: 0x04008105 RID: 33029
			public NPCEquipmentManager Equipment;

			// Token: 0x04008106 RID: 33030
			public AudioSource Voice;

			// Token: 0x04008107 RID: 33031
			public NPCDialogueHolder Personality;

			// Token: 0x04008108 RID: 33032
			public HackMeAudienceMember Marker;

			// Token: 0x04008109 RID: 33033
			public bool SeatWasEnabled;

			// Token: 0x0400810A RID: 33034
			public bool FieldWasEnabled;

			// Token: 0x0400810B RID: 33035
			public bool LookHorizontalOnly;

			// Token: 0x0400810C RID: 33036
			public float LookMaxVertical;

			// Token: 0x0400810D RID: 33037
			public float LookMaxHorizontal;

			// Token: 0x0400810E RID: 33038
			public RuntimeAnimatorController OriginalController;

			// Token: 0x0400810F RID: 33039
			public HackMeSeatedAudience.Role Role;

			// Token: 0x04008110 RID: 33040
			public int PhoneType;

			// Token: 0x04008111 RID: 33041
			public int FaceIndex;

			// Token: 0x04008112 RID: 33042
			public bool Scheduled;

			// Token: 0x04008113 RID: 33043
			public bool Started;

			// Token: 0x04008114 RID: 33044
			public float StartAt;

			// Token: 0x04008115 RID: 33045
			public float EndAt;

			// Token: 0x04008116 RID: 33046
			public float NextFlash;

			// Token: 0x04008117 RID: 33047
			public int FlashesLeft;
		}
	}
}
