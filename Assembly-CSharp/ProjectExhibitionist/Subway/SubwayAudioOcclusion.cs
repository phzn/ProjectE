using System;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;

namespace ProjectExhibitionist.Subway
{
	// Token: 0x0200091F RID: 2335
	public class SubwayAudioOcclusion : MonoBehaviour
	{
		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06003F51 RID: 16209 RVA: 0x00177DC7 File Offset: 0x00175FC7
		public float Factor
		{
			get
			{
				return Mathf.Lerp(0.12f, 1f, this._openness);
			}
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x00177DE0 File Offset: 0x00175FE0
		public static SubwayAudioOcclusion Attach(GameObject host, Transform ignoreRoot)
		{
			SubwayAudioOcclusion subwayAudioOcclusion = host.GetComponent<SubwayAudioOcclusion>();
			if (subwayAudioOcclusion == null)
			{
				subwayAudioOcclusion = host.AddComponent<SubwayAudioOcclusion>();
			}
			subwayAudioOcclusion._ignoreRoot = ignoreRoot;
			return subwayAudioOcclusion;
		}

		// Token: 0x06003F53 RID: 16211 RVA: 0x00177E0C File Offset: 0x0017600C
		public static void ApplyRolloffToZero(AudioSource s, float minDistance, float maxDistance)
		{
			s.minDistance = minDistance;
			s.maxDistance = maxDistance;
			float num = Mathf.Clamp01(minDistance / maxDistance);
			AnimationCurve animationCurve = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 1f),
				new Keyframe(num, 1f),
				new Keyframe(Mathf.Lerp(num, 1f, 0.25f), 0.45f),
				new Keyframe(Mathf.Lerp(num, 1f, 0.6f), 0.15f),
				new Keyframe(1f, 0f)
			});
			for (int i = 0; i < animationCurve.length; i++)
			{
				animationCurve.SmoothTangents(i, 0f);
			}
			s.rolloffMode = AudioRolloffMode.Custom;
			s.SetCustomCurve(AudioSourceCurveType.CustomRolloff, animationCurve);
		}

		// Token: 0x06003F54 RID: 16212 RVA: 0x00177EE8 File Offset: 0x001760E8
		private void Awake()
		{
			this._lowPass = base.GetComponent<AudioLowPassFilter>();
			if (this._lowPass == null)
			{
				this._lowPass = base.gameObject.AddComponent<AudioLowPassFilter>();
			}
			this.ResetForReuse();
		}

		// Token: 0x06003F55 RID: 16213 RVA: 0x00177F1B File Offset: 0x0017611B
		private void OnEnable()
		{
			this.ResetForReuse();
		}

		// Token: 0x06003F56 RID: 16214 RVA: 0x00177F23 File Offset: 0x00176123
		public void ResetForReuse()
		{
			this._openness = 1f;
			if (this._lowPass == null)
			{
				this._lowPass = base.GetComponent<AudioLowPassFilter>();
			}
			this.ApplyLowPass();
		}

		// Token: 0x06003F57 RID: 16215 RVA: 0x00177F50 File Offset: 0x00176150
		public void RefreshImmediately()
		{
			this._openness = (this.HasClearPath() ? 1f : 0f);
			this.ApplyLowPass();
		}

		// Token: 0x06003F58 RID: 16216 RVA: 0x00177F74 File Offset: 0x00176174
		private void ApplyLowPass()
		{
			if (this._lowPass == null)
			{
				return;
			}
			float num = this._openness * this._openness;
			this._lowPass.cutoffFrequency = Mathf.Lerp(650f, 22000f, num);
		}

		// Token: 0x06003F59 RID: 16217 RVA: 0x00177FBC File Offset: 0x001761BC
		private void Update()
		{
			float num = (this.HasClearPath() ? 1f : 0f);
			this._openness = Mathf.MoveTowards(this._openness, num, 3f * Time.deltaTime);
			this.ApplyLowPass();
		}

		// Token: 0x06003F5A RID: 16218 RVA: 0x00178004 File Offset: 0x00176204
		private bool HasClearPath()
		{
			Camera main = Camera.main;
			Transform transform = ((main != null) ? main.transform : PlayerRegistry.Player);
			if (transform == null)
			{
				return true;
			}
			Vector3 position = base.transform.position;
			Vector3 position2 = transform.position;
			Vector3 vector = position2 - position;
			if (vector.magnitude < 1f)
			{
				return true;
			}
			Transform player = PlayerRegistry.Player;
			if (this.IsPathClear(position, position2, player))
			{
				return true;
			}
			Vector3 vector2 = Vector3.up * 0.65f;
			if (this.IsPathClear(position + vector2, position2 + vector2, player))
			{
				return true;
			}
			Vector3 vector3 = Vector3.Cross(Vector3.up, vector);
			if (vector3.sqrMagnitude > 0.0001f)
			{
				vector3 = vector3.normalized * 0.35f;
				if (this.IsPathClear(position + vector3, position2 + vector3, player) || this.IsPathClear(position - vector3, position2 - vector3, player))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003F5B RID: 16219 RVA: 0x00178110 File Offset: 0x00176310
		private bool IsPathClear(Vector3 from, Vector3 to, Transform player)
		{
			Vector3 vector = to - from;
			float magnitude = vector.magnitude;
			if (magnitude < 0.001f)
			{
				return true;
			}
			int num = Physics.RaycastNonAlloc(from, vector / magnitude, SubwayAudioOcclusion.Hits, magnitude, -5, QueryTriggerInteraction.Ignore);
			for (int i = 0; i < num; i++)
			{
				Transform transform = SubwayAudioOcclusion.Hits[i].transform;
				if (!(transform == null) && (!(this._ignoreRoot != null) || !transform.IsChildOf(this._ignoreRoot)) && (!(player != null) || !transform.IsChildOf(player)) && !(transform.GetComponentInParent<SubwayPlatformBarrier>() != null))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04003F29 RID: 16169
		private const float OccludedVolumeFactor = 0.12f;

		// Token: 0x04003F2A RID: 16170
		private const float OpenCutoffHz = 22000f;

		// Token: 0x04003F2B RID: 16171
		private const float OccludedCutoffHz = 650f;

		// Token: 0x04003F2C RID: 16172
		private const float SmoothSpeed = 3f;

		// Token: 0x04003F2D RID: 16173
		private Transform _ignoreRoot;

		// Token: 0x04003F2E RID: 16174
		private AudioLowPassFilter _lowPass;

		// Token: 0x04003F2F RID: 16175
		private float _openness = 1f;

		// Token: 0x04003F30 RID: 16176
		private static readonly RaycastHit[] Hits = new RaycastHit[16];
	}
}
