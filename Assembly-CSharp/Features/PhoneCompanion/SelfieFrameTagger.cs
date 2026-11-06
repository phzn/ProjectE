using System;
using UnityEngine;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E66 RID: 3686
	[DisallowMultipleComponent]
	public class SelfieFrameTagger : MonoBehaviour
	{
		// Token: 0x06006E19 RID: 28185 RVA: 0x002C4404 File Offset: 0x002C2604
		private void Awake()
		{
			this._animator = base.GetComponentInParent<Animator>();
			this.ResolveBones();
		}

		// Token: 0x06006E1A RID: 28186 RVA: 0x002C4418 File Offset: 0x002C2618
		public void ResolveBones()
		{
			if (this._animator == null)
			{
				this._animator = base.GetComponentInParent<Animator>();
			}
			if (this._animator == null || this.points == null)
			{
				return;
			}
			bool isHuman = this._animator.isHuman;
			foreach (SelfieFrameTagger.BodyPoint bodyPoint in this.points)
			{
				if (bodyPoint != null)
				{
					bodyPoint.resolvedBone = (isHuman ? this._animator.GetBoneTransform(bodyPoint.bone) : null);
				}
			}
		}

		// Token: 0x06006E1B RID: 28187 RVA: 0x002C449B File Offset: 0x002C269B
		private bool TryGetWorldPos(SelfieFrameTagger.BodyPoint p, out Vector3 world)
		{
			world = Vector3.zero;
			if (p == null || p.resolvedBone == null)
			{
				return false;
			}
			world = p.resolvedBone.TransformPoint(p.localOffset);
			return true;
		}

		// Token: 0x06006E1C RID: 28188 RVA: 0x002C44D4 File Offset: 0x002C26D4
		public string ResolveTag(Camera cam)
		{
			if (cam == null || this.points == null || this.points.Length == 0)
			{
				return "none";
			}
			if (this._animator == null || !this._animator.isHuman)
			{
				this.ResolveBones();
			}
			float num = -1f;
			string text = "none";
			bool flag = false;
			foreach (SelfieFrameTagger.BodyPoint bodyPoint in this.points)
			{
				Vector3 vector;
				if (bodyPoint != null && !string.IsNullOrEmpty(bodyPoint.tag) && this.TryGetWorldPos(bodyPoint, out vector))
				{
					Vector3 vector2 = cam.WorldToViewportPoint(vector);
					if (vector2.z > 0f && vector2.x >= 0f && vector2.x <= 1f && vector2.y >= 0f && vector2.y <= 1f)
					{
						Vector2 vector3 = new Vector2(vector2.x - 0.5f, vector2.y - 0.5f);
						float num2 = vector3.magnitude / 0.7071f;
						float num3 = SelfieFrameTagger.ProjectedScreenFill(cam, vector2.z, bodyPoint.worldRadius);
						if (num3 >= this.minScreenFill)
						{
							flag = true;
							if (num2 <= this.maxCenterDistance)
							{
								float num4 = 1f - Mathf.Clamp01(num2);
								float num5 = Mathf.Clamp01(num3 * 2f);
								float num6 = Mathf.Lerp(num5, num4, this.centerVsSize) * Mathf.Max(0.01f, bodyPoint.weight);
								if (this.debugLogs)
								{
									Debug.Log(string.Format("[SelfieFrameTagger] {0}: center={1:0.00} size={2:0.00} -> {3:0.00}", new object[] { bodyPoint.tag, num4, num5, num6 }));
								}
								if (num6 > num)
								{
									num = num6;
									text = bodyPoint.tag;
								}
							}
						}
					}
				}
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[SelfieFrameTagger] TAG = {0} (score {1:0.00})", text, num));
			}
			if (!(text == "none") || !flag)
			{
				return text;
			}
			return "fullbody";
		}

		// Token: 0x06006E1D RID: 28189 RVA: 0x002C4708 File Offset: 0x002C2908
		public bool IsFaceInFrame(Camera cam)
		{
			if (cam == null || this.points == null)
			{
				return false;
			}
			if (this._animator == null || !this._animator.isHuman)
			{
				this.ResolveBones();
			}
			SelfieFrameTagger.BodyPoint[] array = this.points;
			int i = 0;
			while (i < array.Length)
			{
				SelfieFrameTagger.BodyPoint bodyPoint = array[i];
				if (bodyPoint != null && !(bodyPoint.tag != "face"))
				{
					Vector3 vector;
					if (!this.TryGetWorldPos(bodyPoint, out vector))
					{
						return false;
					}
					Vector3 vector2 = cam.WorldToViewportPoint(vector);
					return vector2.z > 0f && vector2.x >= 0f && vector2.x <= 1f && vector2.y >= 0f && vector2.y <= 1f && SelfieFrameTagger.ProjectedScreenFill(cam, vector2.z, bodyPoint.worldRadius) >= this.faceVisibleMinFill;
				}
				else
				{
					i++;
				}
			}
			return false;
		}

		// Token: 0x06006E1E RID: 28190 RVA: 0x002C4800 File Offset: 0x002C2A00
		private static float ProjectedScreenFill(Camera cam, float z, float worldRadius)
		{
			if (z <= 0.0001f)
			{
				return 0f;
			}
			float num = cam.fieldOfView * 0.5f * 0.017453292f;
			float num2 = 2f * z * Mathf.Tan(num);
			if (num2 <= 0.0001f)
			{
				return 0f;
			}
			return 2f * worldRadius / num2;
		}

		// Token: 0x06006E1F RID: 28191 RVA: 0x002C4854 File Offset: 0x002C2A54
		private static SelfieFrameTagger.BodyPoint[] DefaultPoints()
		{
			return new SelfieFrameTagger.BodyPoint[]
			{
				new SelfieFrameTagger.BodyPoint
				{
					tag = "face",
					bone = HumanBodyBones.Head,
					localOffset = new Vector3(0f, 0.02f, 0.06f),
					worldRadius = 0.11f,
					weight = 1f
				},
				new SelfieFrameTagger.BodyPoint
				{
					tag = "tits",
					bone = HumanBodyBones.Chest,
					localOffset = new Vector3(0f, 0.02f, 0.08f),
					worldRadius = 0.12f,
					weight = 1.05f
				},
				new SelfieFrameTagger.BodyPoint
				{
					tag = "pussy",
					bone = HumanBodyBones.Hips,
					localOffset = new Vector3(0f, -0.02f, 0.08f),
					worldRadius = 0.09f,
					weight = 1.1f
				},
				new SelfieFrameTagger.BodyPoint
				{
					tag = "ass",
					bone = HumanBodyBones.Hips,
					localOffset = new Vector3(0f, 0f, -0.1f),
					worldRadius = 0.13f,
					weight = 1.05f
				},
				new SelfieFrameTagger.BodyPoint
				{
					tag = "fullbody",
					bone = HumanBodyBones.Spine,
					localOffset = Vector3.zero,
					worldRadius = 0.55f,
					weight = 0.5f
				}
			};
		}

		// Token: 0x06006E20 RID: 28192 RVA: 0x002C49CC File Offset: 0x002C2BCC
		private void OnDrawGizmosSelected()
		{
			if (!this.drawGizmos)
			{
				return;
			}
			if (this._animator == null)
			{
				this._animator = base.GetComponentInParent<Animator>();
			}
			if (this._animator == null || !this._animator.isHuman || this.points == null)
			{
				return;
			}
			this.ResolveBones();
			foreach (SelfieFrameTagger.BodyPoint bodyPoint in this.points)
			{
				if (bodyPoint != null && !(bodyPoint.resolvedBone == null))
				{
					Vector3 vector = bodyPoint.resolvedBone.TransformPoint(bodyPoint.localOffset);
					Gizmos.color = SelfieFrameTagger.ColorFor(bodyPoint.tag);
					Gizmos.DrawWireSphere(vector, bodyPoint.worldRadius);
				}
			}
		}

		// Token: 0x06006E21 RID: 28193 RVA: 0x002C4A7C File Offset: 0x002C2C7C
		private static Color ColorFor(string tag)
		{
			if (tag == "face")
			{
				return Color.cyan;
			}
			if (tag == "tits")
			{
				return Color.magenta;
			}
			if (tag == "pussy")
			{
				return new Color(1f, 0.4f, 0.6f);
			}
			if (!(tag == "ass"))
			{
				return new Color(1f, 1f, 1f, 0.4f);
			}
			return Color.yellow;
		}

		// Token: 0x040070B6 RID: 28854
		public const string TagFace = "face";

		// Token: 0x040070B7 RID: 28855
		public const string TagTits = "tits";

		// Token: 0x040070B8 RID: 28856
		public const string TagPussy = "pussy";

		// Token: 0x040070B9 RID: 28857
		public const string TagAss = "ass";

		// Token: 0x040070BA RID: 28858
		public const string TagFullBody = "fullbody";

		// Token: 0x040070BB RID: 28859
		public const string TagNone = "none";

		// Token: 0x040070BC RID: 28860
		[Header("Punti del corpo (auto-ancorati ai bone)")]
		[Tooltip("I punti valutati allo scatto. I default coprono viso/tette/vagina/sedere/fullbody.")]
		public SelfieFrameTagger.BodyPoint[] points = SelfieFrameTagger.DefaultPoints();

		// Token: 0x040070BD RID: 28861
		[Header("Soglie (tarabili in Play)")]
		[Tooltip("Frazione max della mezza-diagonale dello schermo entro cui un punto e' considerato 'centrato' (0 = solo centro esatto, 1 = tutto lo schermo). Oltre, il punto e' scartato.")]
		[Range(0.1f, 1f)]
		public float maxCenterDistance = 0.6f;

		// Token: 0x040070BE RID: 28862
		[Tooltip("Frazione minima di altezza schermo che la parte deve occupare per 'contare'. Sotto questa, la parte e' troppo piccola/lontana e viene ignorata (es. corpo intero da lontano non taggato come 'tits').")]
		[Range(0f, 1f)]
		public float minScreenFill = 0.06f;

		// Token: 0x040070BF RID: 28863
		[Tooltip("Peso della centratura nello score (vs dimensione). 1 = conta solo essere centrati, 0 = conta solo essere grandi nel frame.")]
		[Range(0f, 1f)]
		public float centerVsSize = 0.55f;

		// Token: 0x040070C0 RID: 28864
		[Header("Debug")]
		public bool debugLogs;

		// Token: 0x040070C1 RID: 28865
		public bool drawGizmos = true;

		// Token: 0x040070C2 RID: 28866
		private Animator _animator;

		// Token: 0x040070C3 RID: 28867
		[Header("Volto in frame (per la censura)")]
		[Tooltip("Frazione minima di altezza schermo che il VOLTO deve occupare per considerarlo 'inquadrato' ai fini della censura. Indipendente dal tag vincente: un selfie corpo+viso ha tag 'tits' ma il volto e' comunque visibile e va censurato. Tienila bassa: basta che la faccia si veda, non che sia il soggetto.")]
		[Range(0f, 0.5f)]
		public float faceVisibleMinFill = 0.04f;

		// Token: 0x02000E67 RID: 3687
		[Serializable]
		public class BodyPoint
		{
			// Token: 0x040070C4 RID: 28868
			[Tooltip("Tag salvato nel filename se questo punto vince l'inquadratura.")]
			public string tag = "face";

			// Token: 0x040070C5 RID: 28869
			[Tooltip("Bone humanoid a cui ancorare il punto.")]
			public HumanBodyBones bone = HumanBodyBones.Head;

			// Token: 0x040070C6 RID: 28870
			[Tooltip("Offset locale (rispetto al bone) per centrare il punto sulla parte del corpo. In metri, nello spazio del bone.")]
			public Vector3 localOffset = Vector3.zero;

			// Token: 0x040070C7 RID: 28871
			[Tooltip("Raggio (m) della parte del corpo: quanto e' 'grande' nel mondo. Usato per stimare quanto riempie il frame. Tette/sedere ~0.12, viso ~0.11, fullbody grande.")]
			public float worldRadius = 0.12f;

			// Token: 0x040070C8 RID: 28872
			[Tooltip("Moltiplicatore di priorita': a parita' di inquadratura, valori >1 vincono piu' facilmente. Tieni 1 per default; alza per parti 'forti'.")]
			public float weight = 1f;

			// Token: 0x040070C9 RID: 28873
			[NonSerialized]
			public Transform resolvedBone;
		}
	}
}
