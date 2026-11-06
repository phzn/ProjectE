using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProjectExhibitionist.Subway
{
	// Token: 0x02000928 RID: 2344
	public class SubwayPassengers : MonoBehaviour
	{
		// Token: 0x06003F91 RID: 16273 RVA: 0x00179600 File Offset: 0x00177800
		private void Awake()
		{
			this._director = base.GetComponentInParent<SubwayRideDirector>();
			if (this._director == null)
			{
				Debug.LogWarning("[SubwayPassengers] '" + base.name + "': nessun SubwayRideDirector nei parent — il nodo Passengers va messo sotto il treno.", this);
				base.enabled = false;
				return;
			}
			SubwayPassengers.Collect(this.seated, base.transform, "Seated");
			SubwayPassengers.Collect(this.standing, base.transform, "Standing");
			SubwayPassengers.SetRosterInactiveImmediately(this.seated);
			SubwayPassengers.SetRosterInactiveImmediately(this.standing);
			if (this.seated.Roster.Count == 0 && this.standing.Roster.Count == 0)
			{
				Debug.LogWarning("[SubwayPassengers] both rosters are empty — disabled.");
				base.enabled = false;
				return;
			}
			this._initialised = true;
			this._director.TrainPresenceChanged += this.OnTrainPresence;
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x001796E0 File Offset: 0x001778E0
		private void OnEnable()
		{
			if (this._initialised && this._hasPendingPresence)
			{
				this.RestartPresenceRoutine();
			}
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x001796F8 File Offset: 0x001778F8
		private void OnDisable()
		{
			if (this._presenceRoutine != null)
			{
				base.StopCoroutine(this._presenceRoutine);
				this._presenceRoutine = null;
			}
			this._hasPendingPresence = false;
			this._requestedPresence = false;
		}

		// Token: 0x06003F94 RID: 16276 RVA: 0x00179723 File Offset: 0x00177923
		private void OnDestroy()
		{
			if (this._director != null)
			{
				this._director.TrainPresenceChanged -= this.OnTrainPresence;
			}
		}

		// Token: 0x06003F95 RID: 16277 RVA: 0x0017974C File Offset: 0x0017794C
		private static void Collect(SubwayPassengers.Group g, Transform owner, string defaultPath)
		{
			if (g.root == null)
			{
				g.root = owner.Find(defaultPath);
			}
			if (g.root == null)
			{
				return;
			}
			foreach (object obj in g.root)
			{
				Transform transform = (Transform)obj;
				Animator componentInChildren = transform.GetComponentInChildren<Animator>();
				SubwayGripIK component = transform.GetComponent<SubwayGripIK>();
				g.Roster.Add(new SubwayPassengers.Member
				{
					Go = transform.gameObject,
					Anim = componentInChildren,
					Grip = component
				});
				g.Slots.Add(new SubwayPassengers.Slot
				{
					Position = transform.localPosition,
					Rotation = transform.localRotation,
					Controller = ((componentInChildren != null) ? componentInChildren.runtimeAnimatorController : null),
					GripTarget = ((component != null) ? component.target : null),
					GripTarget2 = ((component != null) ? component.secondTarget : null),
					GripHand = ((component != null) ? component.hand : SubwayGripIK.GripHand.Right)
				});
			}
			if (g.Boarding.Capacity < g.Roster.Count)
			{
				g.Boarding.Capacity = g.Roster.Count;
			}
			if (g.SlotOrder.Capacity < g.Roster.Count)
			{
				g.SlotOrder.Capacity = g.Roster.Count;
			}
		}

		// Token: 0x06003F96 RID: 16278 RVA: 0x001798F8 File Offset: 0x00177AF8
		private void OnTrainPresence(bool present)
		{
			this._requestedPresence = present;
			this._hasPendingPresence = true;
			if (base.isActiveAndEnabled)
			{
				this.RestartPresenceRoutine();
			}
		}

		// Token: 0x06003F97 RID: 16279 RVA: 0x00179916 File Offset: 0x00177B16
		private void RestartPresenceRoutine()
		{
			if (this._presenceRoutine != null)
			{
				base.StopCoroutine(this._presenceRoutine);
			}
			this._presenceRoutine = base.StartCoroutine(this.ApplyPresenceIncrementally(this._requestedPresence));
		}

		// Token: 0x06003F98 RID: 16280 RVA: 0x00179944 File Offset: 0x00177B44
		private IEnumerator ApplyPresenceIncrementally(bool present)
		{
			int operationsThisFrame = 0;
			yield return null;
			int num;
			for (int i = 0; i < this.seated.Roster.Count; i = num + 1)
			{
				if (SubwayPassengers.SetMemberInactive(this.seated.Roster[i]))
				{
					num = operationsThisFrame + 1;
					operationsThisFrame = num;
					if (num >= Mathf.Max(1, this.lifecycleOperationsPerFrame))
					{
						operationsThisFrame = 0;
						yield return null;
					}
				}
				num = i;
			}
			for (int i = 0; i < this.standing.Roster.Count; i = num + 1)
			{
				if (SubwayPassengers.SetMemberInactive(this.standing.Roster[i]))
				{
					num = operationsThisFrame + 1;
					operationsThisFrame = num;
					if (num >= Mathf.Max(1, this.lifecycleOperationsPerFrame))
					{
						operationsThisFrame = 0;
						yield return null;
					}
				}
				num = i;
			}
			if (!present)
			{
				this._hasPendingPresence = false;
				this._presenceRoutine = null;
				yield break;
			}
			int totalBudget = Mathf.Max(1, this.maxActivePassengers);
			int num2 = SubwayPassengers.MinimumBoardingCount(this.standing);
			int seatedCount = SubwayPassengers.PrepareBoarding(this.seated, Mathf.Max(0, totalBudget - num2));
			for (int i = 0; i < seatedCount; i = num + 1)
			{
				SubwayPassengers.ActivateBoardingMember(this.seated, i, true);
				num = operationsThisFrame + 1;
				operationsThisFrame = num;
				if (num >= Mathf.Max(1, this.lifecycleOperationsPerFrame))
				{
					operationsThisFrame = 0;
					yield return null;
				}
				num = i;
			}
			int standingCount = SubwayPassengers.PrepareBoarding(this.standing, Mathf.Max(0, totalBudget - seatedCount));
			for (int i = 0; i < standingCount; i = num + 1)
			{
				SubwayPassengers.ActivateBoardingMember(this.standing, i, false);
				num = operationsThisFrame + 1;
				operationsThisFrame = num;
				if (num >= Mathf.Max(1, this.lifecycleOperationsPerFrame))
				{
					operationsThisFrame = 0;
					yield return null;
				}
				num = i;
			}
			this._hasPendingPresence = false;
			this._presenceRoutine = null;
			yield break;
		}

		// Token: 0x06003F99 RID: 16281 RVA: 0x0017995C File Offset: 0x00177B5C
		private static int MinimumBoardingCount(SubwayPassengers.Group g)
		{
			int num = Mathf.Clamp(g.max, 0, g.Roster.Count);
			return Mathf.Clamp(g.min, 0, num);
		}

		// Token: 0x06003F9A RID: 16282 RVA: 0x00179990 File Offset: 0x00177B90
		private static int PrepareBoarding(SubwayPassengers.Group g, int budget)
		{
			int count = g.Roster.Count;
			if (count == 0)
			{
				return 0;
			}
			int num = Mathf.Clamp(Mathf.Min(g.max, Mathf.Max(0, budget)), 0, count);
			int num2 = Random.Range(Mathf.Clamp(g.min, 0, num), num + 1);
			SubwayPassengers.Fill(g.Boarding, count);
			SubwayPassengers.Shuffle<int>(g.Boarding, num2);
			if (g.shuffleSlots)
			{
				SubwayPassengers.Fill(g.SlotOrder, count);
				SubwayPassengers.Shuffle<int>(g.SlotOrder, num2);
			}
			return num2;
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x00179A18 File Offset: 0x00177C18
		private static void ActivateBoardingMember(SubwayPassengers.Group g, int boardingIndex, bool isSeated)
		{
			List<int> list = (g.shuffleSlots ? g.SlotOrder : g.Boarding);
			SubwayPassengers.Slot slot = g.Slots[list[boardingIndex]];
			SubwayPassengers.Member member = g.Roster[g.Boarding[boardingIndex]];
			Transform transform = member.Go.transform;
			transform.localPosition = slot.Position;
			transform.localRotation = slot.Rotation;
			if (member.Grip != null)
			{
				member.Grip.target = slot.GripTarget;
				member.Grip.secondTarget = slot.GripTarget2;
				member.Grip.hand = slot.GripHand;
			}
			if (member.Anim != null && slot.Controller != null && member.Anim.runtimeAnimatorController != slot.Controller)
			{
				member.Anim.runtimeAnimatorController = slot.Controller;
			}
			member.Go.SetActive(true);
			SubwayPassengerCollision.Configure(member.Go, member.Anim, isSeated);
			SubwayPassengers.ConfigureVisualCost(member);
			if (member.Anim != null)
			{
				member.Anim.cullingMode = AnimatorCullingMode.CullCompletely;
			}
		}

		// Token: 0x06003F9C RID: 16284 RVA: 0x00179B50 File Offset: 0x00177D50
		private static void ConfigureVisualCost(SubwayPassengers.Member member)
		{
			if (member.VisualCostConfigured)
			{
				return;
			}
			member.VisualCostConfigured = true;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in member.Go.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				skinnedMeshRenderer.updateWhenOffscreen = false;
				skinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
				skinnedMeshRenderer.receiveShadows = false;
			}
			Light[] componentsInChildren2 = member.Go.GetComponentsInChildren<Light>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].enabled = false;
			}
		}

		// Token: 0x06003F9D RID: 16285 RVA: 0x00179BC4 File Offset: 0x00177DC4
		private static void Fill(List<int> list, int n)
		{
			list.Clear();
			for (int i = 0; i < n; i++)
			{
				list.Add(i);
			}
		}

		// Token: 0x06003F9E RID: 16286 RVA: 0x00179BEC File Offset: 0x00177DEC
		private static void Shuffle<T>(List<T> list, int count)
		{
			int count2 = list.Count;
			for (int i = 0; i < count; i++)
			{
				int num = Random.Range(i, count2);
				int num2 = i;
				int num3 = num;
				T t = list[num];
				T t2 = list[i];
				list[num2] = t;
				list[num3] = t2;
			}
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x00179C48 File Offset: 0x00177E48
		private static bool SetMemberInactive(SubwayPassengers.Member member)
		{
			if (member.Go == null || !member.Go.activeSelf)
			{
				return false;
			}
			member.Go.SetActive(false);
			return true;
		}

		// Token: 0x06003FA0 RID: 16288 RVA: 0x00179C74 File Offset: 0x00177E74
		private static void SetRosterInactiveImmediately(SubwayPassengers.Group group)
		{
			for (int i = 0; i < group.Roster.Count; i++)
			{
				SubwayPassengers.SetMemberInactive(group.Roster[i]);
			}
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x00179CA9 File Offset: 0x00177EA9
		private void Reset()
		{
			this.seated.root = base.transform.Find("Seated");
			this.standing.root = base.transform.Find("Standing");
		}

		// Token: 0x04003F67 RID: 16231
		[Tooltip("Passengers on the benches. Root auto-found at Passengers/Seated if left empty.")]
		[SerializeField]
		private SubwayPassengers.Group seated = new SubwayPassengers.Group
		{
			min = 3,
			max = 8
		};

		// Token: 0x04003F68 RID: 16232
		[Tooltip("Passengers standing in the aisle. Root auto-found at Passengers/Standing if left empty.")]
		[SerializeField]
		private SubwayPassengers.Group standing = new SubwayPassengers.Group
		{
			min = 2,
			max = 5
		};

		// Token: 0x04003F69 RID: 16233
		[SerializeField]
		[Min(1f)]
		private int lifecycleOperationsPerFrame = 1;

		// Token: 0x04003F6A RID: 16234
		[Tooltip("Hard runtime budget shared by seated and standing passengers in one carriage.")]
		[SerializeField]
		[Min(1f)]
		private int maxActivePassengers = 5;

		// Token: 0x04003F6B RID: 16235
		private SubwayRideDirector _director;

		// Token: 0x04003F6C RID: 16236
		private Coroutine _presenceRoutine;

		// Token: 0x04003F6D RID: 16237
		private bool _initialised;

		// Token: 0x04003F6E RID: 16238
		private bool _hasPendingPresence;

		// Token: 0x04003F6F RID: 16239
		private bool _requestedPresence;

		// Token: 0x02000929 RID: 2345
		private struct Slot
		{
			// Token: 0x04003F70 RID: 16240
			public Vector3 Position;

			// Token: 0x04003F71 RID: 16241
			public Quaternion Rotation;

			// Token: 0x04003F72 RID: 16242
			public RuntimeAnimatorController Controller;

			// Token: 0x04003F73 RID: 16243
			public Transform GripTarget;

			// Token: 0x04003F74 RID: 16244
			public Transform GripTarget2;

			// Token: 0x04003F75 RID: 16245
			public SubwayGripIK.GripHand GripHand;
		}

		// Token: 0x0200092A RID: 2346
		private class Member
		{
			// Token: 0x04003F76 RID: 16246
			public GameObject Go;

			// Token: 0x04003F77 RID: 16247
			public Animator Anim;

			// Token: 0x04003F78 RID: 16248
			public SubwayGripIK Grip;

			// Token: 0x04003F79 RID: 16249
			public bool VisualCostConfigured;
		}

		// Token: 0x0200092B RID: 2347
		[Serializable]
		private class Group
		{
			// Token: 0x04003F7A RID: 16250
			[Tooltip("Parent of this group's NPCs. Its children are both the roster and the slots.")]
			public Transform root;

			// Token: 0x04003F7B RID: 16251
			[Tooltip("Minimum passengers of this group aboard each visit (clamped to the roster size).")]
			[Min(0f)]
			public int min;

			// Token: 0x04003F7C RID: 16252
			[Tooltip("Maximum passengers of this group aboard each visit (clamped to the roster size).")]
			[Min(0f)]
			public int max;

			// Token: 0x04003F7D RID: 16253
			[Tooltip("Se vero i passeggeri si scambiano i posti a ogni arrivo. Va tenuto SPENTO dove il posto è stato rifinito sul singolo NPC — le prese in piedi, perché l'IK rende in modo diverso su corporature diverse. Spento, resta comunque casuale CHI sale: semplicemente ognuno torna al suo posto.")]
			public bool shuffleSlots = true;

			// Token: 0x04003F7E RID: 16254
			[NonSerialized]
			public readonly List<SubwayPassengers.Member> Roster = new List<SubwayPassengers.Member>();

			// Token: 0x04003F7F RID: 16255
			[NonSerialized]
			public readonly List<SubwayPassengers.Slot> Slots = new List<SubwayPassengers.Slot>();

			// Token: 0x04003F80 RID: 16256
			[NonSerialized]
			public readonly List<int> Boarding = new List<int>();

			// Token: 0x04003F81 RID: 16257
			[NonSerialized]
			public readonly List<int> SlotOrder = new List<int>();
		}
	}
}
