using System;
using BoZo.CharacterCreator;
using Features.CielVoice;
using UnityEngine;

namespace Features.BoobTouch
{
	// Token: 0x020014F3 RID: 5363
	[DisallowMultipleComponent]
	[RequireComponent(typeof(BoobTouchModeController))]
	public class SoapNippleStimulation : MonoBehaviour
	{
		// Token: 0x17001995 RID: 6549
		// (get) Token: 0x0600A5BC RID: 42428 RVA: 0x004539B9 File Offset: 0x00451BB9
		public bool IsStimulating
		{
			get
			{
				return this._phase > SoapNippleStimulation.Phase.Idle;
			}
		}

		// Token: 0x17001996 RID: 6550
		// (get) Token: 0x0600A5BD RID: 42429 RVA: 0x004539C4 File Offset: 0x00451BC4
		public float CurrentWeight
		{
			get
			{
				return this._weight;
			}
		}

		// Token: 0x17001997 RID: 6551
		// (get) Token: 0x0600A5BE RID: 42430 RVA: 0x004539CC File Offset: 0x00451BCC
		public string CurrentPhase
		{
			get
			{
				return this._phase.ToString();
			}
		}

		// Token: 0x0600A5BF RID: 42431 RVA: 0x004539DF File Offset: 0x00451BDF
		private void Awake()
		{
			this._controller = base.GetComponent<BoobTouchModeController>();
			this._gropingController = base.GetComponent<GropingModeController>();
		}

		// Token: 0x0600A5C0 RID: 42432 RVA: 0x004539FC File Offset: 0x00451BFC
		private void Update()
		{
			float time = Time.time;
			float num = ((this._controller != null) ? this._controller.CurrentNippleContact : 0f);
			if (num >= this.contactThreshold)
			{
				if (time >= this._nextTriggerAllowed)
				{
					this._nextTriggerAllowed = time + this.retriggerCooldown;
					this._holdUntil = time + this.holdDuration;
					this._phase = SoapNippleStimulation.Phase.Rising;
					if (this.stimulationVoice != null && (!(this._gropingController != null) || !this._gropingController.RequestStimulationVoice(this.stimulationVoice, this.subtitleHoldDuration)) && CielVoiceController.Instance != null)
					{
						CielVoiceController.Instance.Play(this.stimulationVoice, true, this.subtitleHoldDuration);
					}
					if (this.stimulationFaceIndex >= 0 && this._controller != null)
					{
						this._controller.SetModeFaceOverride(this.stimulationFaceIndex);
					}
					if (this.debugLog)
					{
						Debug.Log(string.Format("[SoapNippleStimulation] Trigger (contact={0:F2}): ", num) + string.Format("voce + erezione, hold {0}s, cooldown {1}s", this.holdDuration, this.retriggerCooldown));
					}
				}
				else if (this._phase == SoapNippleStimulation.Phase.Rising || this._phase == SoapNippleStimulation.Phase.Holding)
				{
					this._holdUntil = Mathf.Max(this._holdUntil, time + this.holdDuration);
				}
			}
			switch (this._phase)
			{
			case SoapNippleStimulation.Phase.Rising:
				this._weight = Mathf.MoveTowards(this._weight, 1f, Time.deltaTime / this.riseDuration);
				if (this._weight >= 1f)
				{
					this._phase = SoapNippleStimulation.Phase.Holding;
				}
				break;
			case SoapNippleStimulation.Phase.Holding:
				if (time >= this._holdUntil)
				{
					this._phase = SoapNippleStimulation.Phase.Falling;
				}
				break;
			case SoapNippleStimulation.Phase.Falling:
				this._weight = Mathf.MoveTowards(this._weight, 0f, Time.deltaTime / this.fallDuration);
				if (this._weight <= 0f)
				{
					this._phase = SoapNippleStimulation.Phase.Idle;
					this.RemoveOverrides();
					if (this._controller != null)
					{
						this._controller.ClearModeFaceOverride();
					}
				}
				break;
			}
			if (this._phase != SoapNippleStimulation.Phase.Idle)
			{
				this.ApplyBlendShapes(Mathf.SmoothStep(0f, 1f, this._weight));
			}
		}

		// Token: 0x0600A5C1 RID: 42433 RVA: 0x00453C48 File Offset: 0x00451E48
		private void ApplyBlendShapes(float normalized)
		{
			BlendShapeManager instance = BlendShapeManager.Instance;
			if (instance != null)
			{
				foreach (SoapNippleStimulation.BlendShapeTarget blendShapeTarget in this.blendShapes)
				{
					if (!string.IsNullOrEmpty(blendShapeTarget.Name))
					{
						instance.SetOverride(blendShapeTarget.Name, blendShapeTarget.TargetValue * normalized, "SoapNippleStimulation", BlendShapePriority.Pose, false);
					}
				}
				this._overridesActive = true;
				return;
			}
			if (!this.ResolveFallbackRenderer())
			{
				return;
			}
			for (int j = 0; j < this.blendShapes.Length; j++)
			{
				if (this._fallbackIndices[j] >= 0)
				{
					this._fallbackRenderer.SetBlendShapeWeight(this._fallbackIndices[j], this.blendShapes[j].TargetValue * normalized);
				}
			}
			this._overridesActive = true;
		}

		// Token: 0x0600A5C2 RID: 42434 RVA: 0x00453D08 File Offset: 0x00451F08
		private void RemoveOverrides()
		{
			if (!this._overridesActive)
			{
				return;
			}
			this._overridesActive = false;
			BlendShapeManager instance = BlendShapeManager.Instance;
			if (instance != null)
			{
				foreach (SoapNippleStimulation.BlendShapeTarget blendShapeTarget in this.blendShapes)
				{
					if (!string.IsNullOrEmpty(blendShapeTarget.Name))
					{
						instance.RemoveOverride(blendShapeTarget.Name, "SoapNippleStimulation", false, false, 0f);
					}
				}
				return;
			}
			if (this._fallbackRenderer == null || this._fallbackIndices == null)
			{
				return;
			}
			for (int j = 0; j < this.blendShapes.Length; j++)
			{
				if (this._fallbackIndices[j] >= 0)
				{
					this._fallbackRenderer.SetBlendShapeWeight(this._fallbackIndices[j], 0f);
				}
			}
		}

		// Token: 0x0600A5C3 RID: 42435 RVA: 0x00453DC8 File Offset: 0x00451FC8
		private bool ResolveFallbackRenderer()
		{
			if (this._fallbackResolved)
			{
				return this._fallbackRenderer != null;
			}
			this._fallbackResolved = true;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in base.transform.root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer.sharedMesh == null) && skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(this.blendShapes[0].Name) >= 0)
				{
					this._fallbackRenderer = skinnedMeshRenderer;
					this._fallbackIndices = new int[this.blendShapes.Length];
					for (int j = 0; j < this.blendShapes.Length; j++)
					{
						this._fallbackIndices[j] = skinnedMeshRenderer.sharedMesh.GetBlendShapeIndex(this.blendShapes[j].Name);
					}
					return true;
				}
			}
			if (this.debugLog)
			{
				Debug.LogWarning("[SoapNippleStimulation] Nessun renderer con i blendshape richiesti.");
			}
			return false;
		}

		// Token: 0x0600A5C4 RID: 42436 RVA: 0x00453EA6 File Offset: 0x004520A6
		private void OnDisable()
		{
			this._phase = SoapNippleStimulation.Phase.Idle;
			this._weight = 0f;
			this.RemoveOverrides();
			if (this._controller != null)
			{
				this._controller.ClearModeFaceOverride();
			}
		}

		// Token: 0x0400AA32 RID: 43570
		private const string BlendShapeSourceId = "SoapNippleStimulation";

		// Token: 0x0400AA33 RID: 43571
		[Header("Voice")]
		[Tooltip("Battuta suonata (audio + sottotitolo) al trigger. Stessa voce della pose 'Stimulation' (Voice_LewdEnjoyment1).")]
		[SerializeField]
		private CielVoiceSO stimulationVoice;

		// Token: 0x0400AA34 RID: 43572
		[Tooltip("Hold del sottotitolo *Moaning* prima del fade-out. Con typewriter e fade del pannello, 5 secondi producono circa 5-6 secondi visibili complessivi. L'audio continua per la sua durata naturale.")]
		[Min(0.1f)]
		[SerializeField]
		private float subtitleHoldDuration = 5f;

		// Token: 0x0400AA35 RID: 43573
		[Header("Faccia")]
		[Tooltip("FaceExpressionIndex forzato per tutta la stimolazione (3 = HornySupreme). In groping gli occhi vengono aperti; la modalita' soap conserva invece la propria regia delle palpebre. La faccia resta finche' i capezzoli non tornano normali (rise+hold+fall, ~1:40), poi torna la base. -1 = disattivato.")]
		[SerializeField]
		private int stimulationFaceIndex = 3;

		// Token: 0x0400AA36 RID: 43574
		[Header("Trigger")]
		[Tooltip("Soglia sul contatto capezzolo (0..1) oltre cui scatta la stimolazione. La metrica e' piena entro ~9 cm dal bone del capezzolo con falloff 3 cm.")]
		[Range(0f, 1f)]
		[SerializeField]
		private float contactThreshold = 0.95f;

		// Token: 0x0400AA37 RID: 43575
		[Tooltip("Cooldown (s) prima che un nuovo passaggio possa rifar partire audio + sottotitolo + drizzamento.")]
		[Min(0f)]
		[SerializeField]
		private float retriggerCooldown = 90f;

		// Token: 0x0400AA38 RID: 43576
		[Header("Erezione capezzoli")]
		[Tooltip("Blendshape portati su gradualmente, con i target della pose Stimulation.")]
		[SerializeField]
		private SoapNippleStimulation.BlendShapeTarget[] blendShapes = new SoapNippleStimulation.BlendShapeTarget[]
		{
			new SoapNippleStimulation.BlendShapeTarget
			{
				Name = "Nipple Depth",
				TargetValue = 30f
			},
			new SoapNippleStimulation.BlendShapeTarget
			{
				Name = "Areola Depth",
				TargetValue = 35f
			}
		};

		// Token: 0x0400AA39 RID: 43577
		[Tooltip("Secondi per arrivare a capezzoli completamente dritti.")]
		[Min(0.1f)]
		[SerializeField]
		private float riseDuration = 5.5f;

		// Token: 0x0400AA3A RID: 43578
		[Tooltip("Tempo MINIMO (s) in cui i capezzoli restano dritti dopo un trigger. Continuare a passare la saponetta sui capezzoli lo rinnova.")]
		[Min(0f)]
		[SerializeField]
		private float holdDuration = 90f;

		// Token: 0x0400AA3B RID: 43579
		[Tooltip("Secondi del ritorno graduale alla normalita' a fine hold.")]
		[Min(0.1f)]
		[SerializeField]
		private float fallDuration = 6f;

		// Token: 0x0400AA3C RID: 43580
		[Header("Debug")]
		[SerializeField]
		private bool debugLog;

		// Token: 0x0400AA3D RID: 43581
		private BoobTouchModeController _controller;

		// Token: 0x0400AA3E RID: 43582
		private GropingModeController _gropingController;

		// Token: 0x0400AA3F RID: 43583
		private SoapNippleStimulation.Phase _phase;

		// Token: 0x0400AA40 RID: 43584
		private float _weight;

		// Token: 0x0400AA41 RID: 43585
		private float _holdUntil;

		// Token: 0x0400AA42 RID: 43586
		private float _nextTriggerAllowed;

		// Token: 0x0400AA43 RID: 43587
		private bool _overridesActive;

		// Token: 0x0400AA44 RID: 43588
		private SkinnedMeshRenderer _fallbackRenderer;

		// Token: 0x0400AA45 RID: 43589
		private int[] _fallbackIndices;

		// Token: 0x0400AA46 RID: 43590
		private bool _fallbackResolved;

		// Token: 0x020014F4 RID: 5364
		[Serializable]
		public class BlendShapeTarget
		{
			// Token: 0x0400AA47 RID: 43591
			public string Name;

			// Token: 0x0400AA48 RID: 43592
			public float TargetValue;
		}

		// Token: 0x020014F5 RID: 5365
		private enum Phase
		{
			// Token: 0x0400AA4A RID: 43594
			Idle,
			// Token: 0x0400AA4B RID: 43595
			Rising,
			// Token: 0x0400AA4C RID: 43596
			Holding,
			// Token: 0x0400AA4D RID: 43597
			Falling
		}
	}
}
