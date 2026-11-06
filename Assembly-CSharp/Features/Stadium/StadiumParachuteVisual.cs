using System;
using System.Collections.Generic;
using ProjectExhibitionist.FirstPerson;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Features.Stadium
{
	// Token: 0x02000C49 RID: 3145
	[DefaultExecutionOrder(20010)]
	public sealed class StadiumParachuteVisual : MonoBehaviour
	{
		// Token: 0x06005ACA RID: 23242 RVA: 0x00239214 File Offset: 0x00237414
		public void Bind(Animator animator, StadiumParachuteProfile profile = null)
		{
			this._profile = profile;
			if (this.bankPivot != null)
			{
				this._restBank = this.bankPivot.localRotation;
			}
			FirstPersonVisibleProp.Mark(base.gameObject);
			if (animator != null && animator.isHuman && this.bindings != null)
			{
				foreach (StadiumParachuteVisual.BoneBinding boneBinding in this.bindings)
				{
					boneBinding.target = animator.GetBoneTransform(boneBinding.playerBone);
				}
			}
			StadiumBackpackHairCollision stadiumBackpackHairCollision = ((animator != null) ? animator.GetComponentInChildren<StadiumBackpackHairCollision>(true) : null);
			Transform transform = ((stadiumBackpackHairCollision != null) ? stadiumBackpackHairCollision.transform.Find("Shoulder straps") : null);
			this._backpackStraps = ((transform != null) ? transform.GetComponent<SkinnedMeshRenderer>() : null);
			if (this._backpackStraps != null && this._strapPose == null)
			{
				this._strapPose = new Mesh
				{
					name = "Parachute strap attachment pose",
					hideFlags = HideFlags.HideAndDontSave
				};
			}
			if (this.canopyAnimator == null || this.openingClip == null)
			{
				return;
			}
			this.canopyAnimator.applyRootMotion = false;
			this.canopyAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			this._graph = PlayableGraph.Create("Stadium canopy inflation");
			this._graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
			this._opening = AnimationClipPlayable.Create(this._graph, this.openingClip);
			this._opening.SetSpeed(0.0);
			AnimationPlayableOutput.Create(this._graph, "Canopy", this.canopyAnimator).SetSourcePlayable(this._opening);
			this._graph.Play();
		}

		// Token: 0x06005ACB RID: 23243 RVA: 0x002393C4 File Offset: 0x002375C4
		public void Pose(float inflation, float turn, float brake)
		{
			this._inflation = Mathf.Clamp01(inflation);
			this._turn = turn;
			this._brake = brake;
		}

		// Token: 0x06005ACC RID: 23244 RVA: 0x002393E0 File Offset: 0x002375E0
		public void Retire()
		{
			this._retiring = true;
			this._closedFor = 0f;
		}

		// Token: 0x06005ACD RID: 23245 RVA: 0x002393F4 File Offset: 0x002375F4
		private void LateUpdate()
		{
			if (this._retiring)
			{
				this._closedFor += Time.deltaTime;
				this._inflation = 1f - Mathf.Clamp01(this._closedFor / 0.48f);
				if (this._closedFor >= 0.48f)
				{
					base.gameObject.SetActive(false);
					Object.Destroy(base.gameObject);
					return;
				}
			}
			if (this._graph.IsValid())
			{
				this._opening.SetTime((double)(this._inflation * this.openingClip.length));
				this._graph.Evaluate(0f);
			}
			if (this.bankPivot != null)
			{
				float num = ((this._profile != null) ? this._profile.canopyBank : 11f);
				float num2 = ((this._profile != null) ? this._profile.canopyBrakePitch : 5f);
				float num3 = ((this._profile != null) ? this._profile.canopySway : 0.65f);
				this._bank = Mathf.Lerp(this._bank, -this._turn * num, 1f - Mathf.Exp(-4f * Time.deltaTime));
				this.bankPivot.localRotation = this._restBank * Quaternion.Euler(-this._brake * num2, 0f, this._inflation * (this._bank + Mathf.Sin(Time.time * 1.3f) * num3));
			}
			this._strapVertices.Clear();
			if (this._backpackStraps != null && this._strapPose != null)
			{
				this._backpackStraps.BakeMesh(this._strapPose, true);
				this._strapPose.GetVertices(this._strapVertices);
			}
			if (this.bindings != null)
			{
				foreach (StadiumParachuteVisual.BoneBinding boneBinding in this.bindings)
				{
					if (boneBinding.target != null && boneBinding.canopyBone != null)
					{
						Vector3 vector = boneBinding.target.TransformPoint(boneBinding.positionOffset);
						if (boneBinding.backpackStrapVertex >= 0 && boneBinding.backpackStrapVertex < this._strapVertices.Count)
						{
							vector = this._backpackStraps.transform.TransformPoint(this._strapVertices[boneBinding.backpackStrapVertex]);
						}
						boneBinding.canopyBone.SetPositionAndRotation(vector, boneBinding.target.rotation * boneBinding.rotationOffset);
					}
				}
			}
		}

		// Token: 0x06005ACE RID: 23246 RVA: 0x0023969D File Offset: 0x0023789D
		private void OnDisable()
		{
			if (this._graph.IsValid())
			{
				this._graph.Destroy();
			}
		}

		// Token: 0x06005ACF RID: 23247 RVA: 0x002396B8 File Offset: 0x002378B8
		private void OnDestroy()
		{
			if (this._graph.IsValid())
			{
				this._graph.Destroy();
			}
			if (this._strapPose != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this._strapPose);
					return;
				}
				Object.DestroyImmediate(this._strapPose);
			}
		}

		// Token: 0x04005BDE RID: 23518
		public const int LeftStrapAnchorVertex = 77;

		// Token: 0x04005BDF RID: 23519
		public const int RightStrapAnchorVertex = 482;

		// Token: 0x04005BE0 RID: 23520
		public Animator canopyAnimator;

		// Token: 0x04005BE1 RID: 23521
		public AnimationClip openingClip;

		// Token: 0x04005BE2 RID: 23522
		public Transform bankPivot;

		// Token: 0x04005BE3 RID: 23523
		public StadiumParachuteVisual.BoneBinding[] bindings;

		// Token: 0x04005BE4 RID: 23524
		private StadiumParachuteProfile _profile;

		// Token: 0x04005BE5 RID: 23525
		private PlayableGraph _graph;

		// Token: 0x04005BE6 RID: 23526
		private AnimationClipPlayable _opening;

		// Token: 0x04005BE7 RID: 23527
		private float _inflation;

		// Token: 0x04005BE8 RID: 23528
		private float _turn;

		// Token: 0x04005BE9 RID: 23529
		private float _brake;

		// Token: 0x04005BEA RID: 23530
		private float _bank;

		// Token: 0x04005BEB RID: 23531
		private float _closedFor;

		// Token: 0x04005BEC RID: 23532
		private bool _retiring;

		// Token: 0x04005BED RID: 23533
		private Quaternion _restBank;

		// Token: 0x04005BEE RID: 23534
		private SkinnedMeshRenderer _backpackStraps;

		// Token: 0x04005BEF RID: 23535
		private Mesh _strapPose;

		// Token: 0x04005BF0 RID: 23536
		private readonly List<Vector3> _strapVertices = new List<Vector3>(810);

		// Token: 0x02000C4A RID: 3146
		[Serializable]
		public sealed class BoneBinding
		{
			// Token: 0x04005BF1 RID: 23537
			public Transform canopyBone;

			// Token: 0x04005BF2 RID: 23538
			public HumanBodyBones playerBone;

			// Token: 0x04005BF3 RID: 23539
			public Quaternion rotationOffset = Quaternion.identity;

			// Token: 0x04005BF4 RID: 23540
			public Vector3 positionOffset;

			// Token: 0x04005BF5 RID: 23541
			[Tooltip("Local centre of the handle's lower grip bar. Used to inspect the authored palm fit.")]
			public Vector3 gripAnchorLocal;

			// Token: 0x04005BF6 RID: 23542
			[Tooltip("Optional vertex on Ciel's fitted backpack straps. -1 follows only the humanoid bone.")]
			public int backpackStrapVertex = -1;

			// Token: 0x04005BF7 RID: 23543
			[NonSerialized]
			public Transform target;
		}
	}
}
