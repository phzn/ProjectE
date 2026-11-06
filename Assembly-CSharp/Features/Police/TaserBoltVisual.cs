using System;
using Features.NPC;
using Features.RenderingPrecision;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.Police
{
	// Token: 0x02000DFE RID: 3582
	[DisallowMultipleComponent]
	public sealed class TaserBoltVisual : MonoBehaviour
	{
		// Token: 0x06006A9C RID: 27292 RVA: 0x002AC3F4 File Offset: 0x002AA5F4
		private void Awake()
		{
			if (this.arcMaterial == null)
			{
				Debug.LogWarning("[TaserBoltVisual] Assign the electrical arc material on the TaserBolt prefab.", this);
				base.enabled = false;
				return;
			}
			this._seed = (uint)base.GetInstanceID();
			this._properties = new MaterialPropertyBlock();
			this._visualRoot = new GameObject("Electrical Wake");
			this._visualRoot.layer = base.gameObject.layer;
			this._visualRoot.transform.SetParent(base.transform, false);
			Vector3 lossyScale = base.transform.lossyScale;
			this._visualRoot.transform.localScale = new Vector3(TaserBoltVisual.SafeInverse(lossyScale.x), TaserBoltVisual.SafeInverse(lossyScale.y), TaserBoltVisual.SafeInverse(lossyScale.z));
			this._glow = this.CreateLine("Soft Halo", this.glowWidth, this.glowColor, 29);
			this._core = this.CreateLine("White Hot Core", this.coreWidth, this.coreColor, 29);
			this._arcA = this.CreateLine("Fork A", this.coreWidth * 0.48f, new Color(0.35f, 1.1f, 2f, 0.8f), 9);
			this._arcB = this.CreateLine("Fork B", this.coreWidth * 0.36f, new Color(0.3f, 0.85f, 1.7f, 0.65f), 9);
			this.CreateSparks();
		}

		// Token: 0x06006A9D RID: 27293 RVA: 0x002AC56C File Offset: 0x002AA76C
		private void OnEnable()
		{
			if (this._visualRoot == null)
			{
				return;
			}
			WorldOrigin.Shifted += this.OnWorldOriginShifted;
			this._previousPosition = base.transform.position;
			this._distance = 0f;
			this._clock = 0f;
			this._visualRoot.SetActive(true);
			this.SetLinesEnabled(false);
			this._sparks.Clear(true);
			this._sparks.Play(true);
		}

		// Token: 0x06006A9E RID: 27294 RVA: 0x002AC5EB File Offset: 0x002AA7EB
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.OnWorldOriginShifted;
			if (this._visualRoot != null)
			{
				this._visualRoot.SetActive(false);
			}
		}

		// Token: 0x06006A9F RID: 27295 RVA: 0x002AC618 File Offset: 0x002AA818
		private void OnWorldOriginShifted(Vector3 delta)
		{
			this._previousPosition += delta;
		}

		// Token: 0x06006AA0 RID: 27296 RVA: 0x002AC62C File Offset: 0x002AA82C
		private void LateUpdate()
		{
			if (this._visualRoot == null)
			{
				return;
			}
			bool flag = NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f;
			this._sparks.main.simulationSpeed = (flag ? 0f : NPCSimulationPause.SimulationScale);
			if (flag)
			{
				return;
			}
			this._clock += NPCSimulationPause.SimulationDeltaTime;
			Vector3 position = base.transform.position;
			Vector3 vector = position - this._previousPosition;
			this._distance += vector.magnitude;
			this._previousPosition = position;
			Vector3 vector2 = ((vector.sqrMagnitude > 1E-06f) ? vector.normalized : base.transform.forward);
			this.DrawWake(position, vector2);
		}

		// Token: 0x06006AA1 RID: 27297 RVA: 0x002AC6FC File Offset: 0x002AA8FC
		private void DrawWake(Vector3 head, Vector3 forward)
		{
			float num = Mathf.Min(this.wakeLength, this._distance);
			if (num < 0.008f)
			{
				return;
			}
			this.SetLinesEnabled(true);
			Vector3 normalized = Vector3.Cross(forward, (Mathf.Abs(forward.y) > 0.95f) ? Vector3.right : Vector3.up).normalized;
			Vector3 vector = Vector3.Cross(normalized, forward);
			float num2 = this._clock * this.flickerRate;
			int num3 = Mathf.FloorToInt(num2);
			float num4 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((num2 - (float)num3) * 2.5f));
			Vector3 vector2 = head - forward * num;
			for (int i = 0; i < 29; i++)
			{
				float num5 = (float)i / 28f;
				float num6 = this._distance - num + num * num5;
				float num7 = Mathf.Sin(num5 * 3.1415927f) * Mathf.Min(1f, num / 0.35f);
				float num8 = TaserBoltVisual.ChangingNoise(num6 * 8f, num3, num4, this._seed);
				float num9 = TaserBoltVisual.ChangingNoise(num6 * 9.5f, num3, num4, this._seed + 37U);
				this._spine[i] = vector2 + forward * (num5 * num) + (normalized * num8 + vector * num9) * (this.arcSpread * num7);
			}
			this._spine[28] = head;
			this._core.SetPositions(this._spine);
			this._glow.SetPositions(this._spine);
			this.DrawBranch(this._arcA, 5, 19, normalized, vector, num3, num4, this._seed + 83U, 1f);
			this.DrawBranch(this._arcB, 13, 26, -normalized, -vector, num3, num4, this._seed + 191U, 0.7f);
			float num10 = 0.88f + 0.12f * Mathf.Sin(this._clock * 71f);
			this._properties.SetFloat(TaserBoltVisual.IntensityId, num10);
			this._core.SetPropertyBlock(this._properties);
			this._glow.SetPropertyBlock(this._properties);
			this._arcA.SetPropertyBlock(this._properties);
			this._arcB.SetPropertyBlock(this._properties);
		}

		// Token: 0x06006AA2 RID: 27298 RVA: 0x002AC968 File Offset: 0x002AAB68
		private void DrawBranch(LineRenderer line, int start, int end, Vector3 right, Vector3 up, int tick, float blend, uint seed, float strength)
		{
			for (int i = 0; i < 9; i++)
			{
				float num = (float)i / 8f;
				float num2 = Mathf.Lerp((float)start, (float)end, num);
				int num3 = Mathf.FloorToInt(num2);
				Vector3 vector = Vector3.Lerp(this._spine[num3], this._spine[Mathf.Min(num3 + 1, 28)], num2 - (float)num3);
				float num4 = Mathf.Sin(num * 3.1415927f) * Mathf.Min(1f, this._distance / 0.4f);
				float num5 = TaserBoltVisual.ChangingNoise(num * 6f, tick, blend, seed);
				this._branch[i] = vector + (right * (1.3f + num5) + up * num5) * (this.arcSpread * num4 * strength);
			}
			line.SetPositions(this._branch);
		}

		// Token: 0x06006AA3 RID: 27299 RVA: 0x002ACA58 File Offset: 0x002AAC58
		private LineRenderer CreateLine(string label, float width, Color color, int points)
		{
			GameObject gameObject = new GameObject(label);
			gameObject.layer = base.gameObject.layer;
			gameObject.transform.SetParent(this._visualRoot.transform, false);
			LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
			lineRenderer.sharedMaterial = this.arcMaterial;
			lineRenderer.useWorldSpace = true;
			lineRenderer.positionCount = points;
			lineRenderer.alignment = LineAlignment.View;
			lineRenderer.textureMode = LineTextureMode.Stretch;
			lineRenderer.widthMultiplier = width;
			lineRenderer.widthCurve = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0.05f),
				new Keyframe(0.25f, 0.75f),
				new Keyframe(0.8f, 1f),
				new Keyframe(1f, 0.35f)
			});
			lineRenderer.colorGradient = TaserBoltVisual.MakeGradient(color);
			lineRenderer.numCapVertices = 2;
			lineRenderer.numCornerVertices = 2;
			lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
			lineRenderer.receiveShadows = false;
			lineRenderer.lightProbeUsage = LightProbeUsage.Off;
			lineRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
			lineRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
			return lineRenderer;
		}

		// Token: 0x06006AA4 RID: 27300 RVA: 0x002ACB70 File Offset: 0x002AAD70
		private void CreateSparks()
		{
			GameObject gameObject = new GameObject("Micro Sparks");
			gameObject.layer = base.gameObject.layer;
			gameObject.transform.SetParent(this._visualRoot.transform, false);
			this._sparks = gameObject.AddComponent<ParticleSystem>();
			this._sparks.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			ParticleSystem.MainModule main = this._sparks.main;
			main.loop = true;
			main.playOnAwake = false;
			main.duration = 2f;
			main.startLifetime = new ParticleSystem.MinMaxCurve(0.055f, 0.12f);
			main.startSpeed = new ParticleSystem.MinMaxCurve(0.25f, 1.1f);
			main.startSize = new ParticleSystem.MinMaxCurve(0.012f, 0.027f);
			main.startColor = new Color(0.6f, 1.4f, 2f, 0.8f);
			main.gravityModifier = 0f;
			main.maxParticles = 16;
			main.simulationSpace = ParticleSystemSimulationSpace.World;
			main.scalingMode = ParticleSystemScalingMode.Shape;
			this._sparks.emission.rateOverTime = this.sparksPerSecond;
			ParticleSystem.ShapeModule shape = this._sparks.shape;
			shape.shapeType = ParticleSystemShapeType.Sphere;
			shape.radius = 0.018f;
			ParticleSystem.ColorOverLifetimeModule colorOverLifetime = this._sparks.colorOverLifetime;
			colorOverLifetime.enabled = true;
			colorOverLifetime.color = TaserBoltVisual.MakeGradient(Color.white);
			ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = this._sparks.sizeOverLifetime;
			sizeOverLifetime.enabled = true;
			sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0f));
			ParticleSystemRenderer component = this._sparks.GetComponent<ParticleSystemRenderer>();
			component.sharedMaterial = this.arcMaterial;
			component.renderMode = ParticleSystemRenderMode.Stretch;
			component.lengthScale = 2f;
			component.velocityScale = 0.03f;
			component.shadowCastingMode = ShadowCastingMode.Off;
			component.receiveShadows = false;
			component.lightProbeUsage = LightProbeUsage.Off;
			component.reflectionProbeUsage = ReflectionProbeUsage.Off;
		}

		// Token: 0x06006AA5 RID: 27301 RVA: 0x002ACD7C File Offset: 0x002AAF7C
		private void SetLinesEnabled(bool value)
		{
			Renderer core = this._core;
			Renderer glow = this._glow;
			Renderer arcA = this._arcA;
			this._arcB.enabled = value;
			arcA.enabled = value;
			glow.enabled = value;
			core.enabled = value;
		}

		// Token: 0x06006AA6 RID: 27302 RVA: 0x002ACDC0 File Offset: 0x002AAFC0
		private static Gradient MakeGradient(Color color)
		{
			Gradient gradient = new Gradient();
			gradient.SetKeys(new GradientColorKey[]
			{
				new GradientColorKey(color, 0f),
				new GradientColorKey(color, 1f)
			}, new GradientAlphaKey[]
			{
				new GradientAlphaKey(0f, 0f),
				new GradientAlphaKey(color.a, 0.25f),
				new GradientAlphaKey(color.a, 0.9f),
				new GradientAlphaKey(0f, 1f)
			});
			return gradient;
		}

		// Token: 0x06006AA7 RID: 27303 RVA: 0x002ACE64 File Offset: 0x002AB064
		private static float SafeInverse(float value)
		{
			return 1f / Mathf.Max(0.0001f, Mathf.Abs(value));
		}

		// Token: 0x06006AA8 RID: 27304 RVA: 0x002ACE7C File Offset: 0x002AB07C
		private static float ChangingNoise(float x, int tick, float blend, uint seed)
		{
			return Mathf.Lerp(TaserBoltVisual.Noise(x, tick, seed), TaserBoltVisual.Noise(x, tick + 1, seed), blend);
		}

		// Token: 0x06006AA9 RID: 27305 RVA: 0x002ACE98 File Offset: 0x002AB098
		private static float Noise(float x, int tick, uint seed)
		{
			int num = Mathf.FloorToInt(x);
			return Mathf.Lerp(TaserBoltVisual.Hash(num, tick, seed), TaserBoltVisual.Hash(num + 1, tick, seed), x - (float)num);
		}

		// Token: 0x06006AAA RID: 27306 RVA: 0x002ACEC7 File Offset: 0x002AB0C7
		private static float Hash(int knot, int tick, uint seed)
		{
			int num = knot * 374761393 + tick * 668265263 + (int)(seed * 2246822519U);
			int num2 = (num ^ (int)((uint)num >> 13)) * 1274126177;
			return ((num2 ^ (int)((uint)num2 >> 16)) & 16777215) / 8388607.5f - 1f;
		}

		// Token: 0x04006D4B RID: 27979
		[Header("Electrical wake (world metres)")]
		[SerializeField]
		private Material arcMaterial;

		// Token: 0x04006D4C RID: 27980
		[SerializeField]
		[Range(0.3f, 2.5f)]
		private float wakeLength = 1.35f;

		// Token: 0x04006D4D RID: 27981
		[SerializeField]
		[Range(0.006f, 0.05f)]
		private float coreWidth = 0.021f;

		// Token: 0x04006D4E RID: 27982
		[SerializeField]
		[Range(0.025f, 0.15f)]
		private float glowWidth = 0.085f;

		// Token: 0x04006D4F RID: 27983
		[SerializeField]
		[Range(0.005f, 0.12f)]
		private float arcSpread = 0.055f;

		// Token: 0x04006D50 RID: 27984
		[SerializeField]
		[Range(8f, 40f)]
		private float flickerRate = 22f;

		// Token: 0x04006D51 RID: 27985
		[SerializeField]
		[ColorUsage(true, true)]
		private Color coreColor = new Color(1.65f, 2.25f, 2.6f, 1f);

		// Token: 0x04006D52 RID: 27986
		[SerializeField]
		[ColorUsage(true, true)]
		private Color glowColor = new Color(0.12f, 0.6f, 1.5f, 0.55f);

		// Token: 0x04006D53 RID: 27987
		[SerializeField]
		[Range(0f, 80f)]
		private float sparksPerSecond = 34f;

		// Token: 0x04006D54 RID: 27988
		private const int SpinePoints = 29;

		// Token: 0x04006D55 RID: 27989
		private const int BranchPoints = 9;

		// Token: 0x04006D56 RID: 27990
		private static readonly int IntensityId = Shader.PropertyToID("_Intensity");

		// Token: 0x04006D57 RID: 27991
		private readonly Vector3[] _spine = new Vector3[29];

		// Token: 0x04006D58 RID: 27992
		private readonly Vector3[] _branch = new Vector3[9];

		// Token: 0x04006D59 RID: 27993
		private GameObject _visualRoot;

		// Token: 0x04006D5A RID: 27994
		private LineRenderer _core;

		// Token: 0x04006D5B RID: 27995
		private LineRenderer _glow;

		// Token: 0x04006D5C RID: 27996
		private LineRenderer _arcA;

		// Token: 0x04006D5D RID: 27997
		private LineRenderer _arcB;

		// Token: 0x04006D5E RID: 27998
		private ParticleSystem _sparks;

		// Token: 0x04006D5F RID: 27999
		private MaterialPropertyBlock _properties;

		// Token: 0x04006D60 RID: 28000
		private Vector3 _previousPosition;

		// Token: 0x04006D61 RID: 28001
		private float _distance;

		// Token: 0x04006D62 RID: 28002
		private float _clock;

		// Token: 0x04006D63 RID: 28003
		private uint _seed;
	}
}
