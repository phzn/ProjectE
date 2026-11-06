using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace ProjectExhibitionist.Subway
{
	// Token: 0x02000923 RID: 2339
	public class SubwayDoorSystem : MonoBehaviour
	{
		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06003F67 RID: 16231 RVA: 0x001783DA File Offset: 0x001765DA
		public bool Moving
		{
			get
			{
				return this._slideRoutine != null;
			}
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06003F68 RID: 16232 RVA: 0x001783E5 File Offset: 0x001765E5
		public bool DoorsOpen
		{
			get
			{
				return this._doorsOpen;
			}
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06003F69 RID: 16233 RVA: 0x001783ED File Offset: 0x001765ED
		// (set) Token: 0x06003F6A RID: 16234 RVA: 0x001783F5 File Offset: 0x001765F5
		public bool SimulationPaused
		{
			get
			{
				return this._simulationPaused;
			}
			set
			{
				this._simulationPaused = value;
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06003F6B RID: 16235 RVA: 0x001783FE File Offset: 0x001765FE
		public bool DoorsPassable
		{
			get
			{
				return this._doorsOpen || this._slideActive;
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06003F6C RID: 16236 RVA: 0x00178410 File Offset: 0x00176610
		public IReadOnlyList<SubwayDoorPanel> OpeningPanels
		{
			get
			{
				return this._openingPanels;
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x06003F6D RID: 16237 RVA: 0x00178418 File Offset: 0x00176618
		public bool HasOpeningPanels
		{
			get
			{
				return this._openingPanels.Count > 0;
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06003F6E RID: 16238 RVA: 0x00178428 File Offset: 0x00176628
		public AudioSource DoorAudioSource
		{
			get
			{
				return this._doorAudio;
			}
		}

		// Token: 0x06003F6F RID: 16239 RVA: 0x00178430 File Offset: 0x00176630
		public void ResetToClosed()
		{
			if (this._slideRoutine != null)
			{
				base.StopCoroutine(this._slideRoutine);
			}
			this._slideRoutine = null;
			this._slideActive = false;
			this._doorsOpen = false;
			if (this._doorAudio != null)
			{
				this._doorAudio.Stop();
			}
			if (this._doorOcclusion != null)
			{
				this._doorOcclusion.ResetForReuse();
			}
			foreach (SubwayDoorway subwayDoorway in this.doorways)
			{
				foreach (SubwayDoorPanel subwayDoorPanel in subwayDoorway.panels)
				{
					if (subwayDoorPanel.panel != null)
					{
						subwayDoorPanel.panel.localPosition = subwayDoorPanel.closedLocalPos;
					}
				}
				subwayDoorway.PlayerInRange = false;
				this.HidePrompt(subwayDoorway);
			}
		}

		// Token: 0x06003F70 RID: 16240 RVA: 0x00178540 File Offset: 0x00176740
		private void Awake()
		{
			this.ResolveWiring();
		}

		// Token: 0x06003F71 RID: 16241 RVA: 0x00178548 File Offset: 0x00176748
		public void PrepareForOperationalUse()
		{
			this.ResolveWiring();
			this.ResetToClosed();
			this.ClassifyPlatformSide();
		}

		// Token: 0x06003F72 RID: 16242 RVA: 0x0017855C File Offset: 0x0017675C
		private void ClassifyPlatformSide()
		{
			this._openingPanels.Clear();
			if (this.platformSideReference == null)
			{
				foreach (SubwayDoorway subwayDoorway in this.doorways)
				{
					foreach (SubwayDoorPanel subwayDoorPanel in subwayDoorway.panels)
					{
						if (subwayDoorPanel.panel != null)
						{
							this._openingPanels.Add(subwayDoorPanel);
						}
					}
				}
				Debug.LogError(string.Format("[SubwayDoors] '{0}': platformSideReference MANCANTE — si aprono tutte e {1} le ante, comprese quelle sui binari.", base.name, this._openingPanels.Count), this);
				return;
			}
			float num = 0f;
			int num2 = 0;
			foreach (SubwayDoorway subwayDoorway2 in this.doorways)
			{
				foreach (SubwayDoorPanel subwayDoorPanel2 in subwayDoorway2.panels)
				{
					if (!(subwayDoorPanel2.panel == null))
					{
						num += this.LocalSideCoord(subwayDoorPanel2);
						num2++;
					}
				}
			}
			if (num2 == 0)
			{
				return;
			}
			float num3 = num / (float)num2;
			float num4 = Mathf.Sign(base.transform.InverseTransformPoint(this.platformSideReference.position).z - num3);
			foreach (SubwayDoorway subwayDoorway3 in this.doorways)
			{
				foreach (SubwayDoorPanel subwayDoorPanel3 in subwayDoorway3.panels)
				{
					if (!(subwayDoorPanel3.panel == null) && Mathf.Sign(this.LocalSideCoord(subwayDoorPanel3) - num3) == num4)
					{
						this._openingPanels.Add(subwayDoorPanel3);
					}
				}
			}
			if (this._openingPanels.Count == 0)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"[SubwayDoors] '",
					base.name,
					"': nessun pannello dal lato di '",
					this.platformSideReference.name,
					"', apro tutto per non lasciare il treno sigillato."
				}), this);
				foreach (SubwayDoorway subwayDoorway4 in this.doorways)
				{
					foreach (SubwayDoorPanel subwayDoorPanel4 in subwayDoorway4.panels)
					{
						if (subwayDoorPanel4.panel != null)
						{
							this._openingPanels.Add(subwayDoorPanel4);
						}
					}
				}
				return;
			}
			Debug.Log(string.Format("[SubwayDoors] '{0}': si aprono {1} pannelli su {2} ", base.name, this._openingPanels.Count, num2) + "(lato di '" + this.platformSideReference.name + "').", this);
		}

		// Token: 0x06003F73 RID: 16243 RVA: 0x001788DC File Offset: 0x00176ADC
		private float LocalSideCoord(SubwayDoorPanel p)
		{
			return base.transform.InverseTransformPoint(SubwayDoorSystem.DoorwayWorldCenter(p)).z;
		}

		// Token: 0x06003F74 RID: 16244 RVA: 0x001788F4 File Offset: 0x00176AF4
		private void Start()
		{
			IncrementalPooledZoneActivation componentInParent = base.GetComponentInParent<IncrementalPooledZoneActivation>();
			if (componentInParent == null || !componentInParent.HasSnapshot || componentInParent.IsOperationalReady)
			{
				this.PrepareForOperationalUse();
			}
		}

		// Token: 0x06003F75 RID: 16245 RVA: 0x00178928 File Offset: 0x00176B28
		public static Vector3 DoorwayWorldCenter(SubwayDoorPanel panel)
		{
			if (((panel != null) ? panel.panel : null) == null)
			{
				return Vector3.zero;
			}
			Collider component = panel.panel.GetComponent<Collider>();
			BoxCollider boxCollider = component as BoxCollider;
			if (boxCollider != null)
			{
				return boxCollider.transform.TransformPoint(boxCollider.center);
			}
			if (component != null)
			{
				Bounds bounds = component.bounds;
				if (bounds.size.sqrMagnitude > 1E-06f)
				{
					return bounds.center;
				}
			}
			return panel.panel.position;
		}

		// Token: 0x06003F76 RID: 16246 RVA: 0x001789B0 File Offset: 0x00176BB0
		private void ResolveWiring()
		{
			foreach (SubwayDoorway subwayDoorway in this.doorways)
			{
				if (subwayDoorway.zoneCollider == null)
				{
					Transform transform = base.transform.Find("SubwayDoorZone_" + subwayDoorway.key);
					if (transform != null)
					{
						subwayDoorway.zoneCollider = transform.GetComponent<Collider>();
					}
				}
				if (subwayDoorway.promptUI == null)
				{
					Transform transform2 = base.transform.Find("SubwayDoorUI_" + subwayDoorway.key);
					if (transform2 != null)
					{
						subwayDoorway.promptUI = transform2.GetComponent<InteractablePromptUI>();
					}
				}
			}
		}

		// Token: 0x06003F77 RID: 16247 RVA: 0x00178A80 File Offset: 0x00176C80
		private void Update()
		{
			if (this._simulationPaused)
			{
				return;
			}
			Transform transform = this.ResolvePlayer();
			bool flag = false;
			foreach (SubwayDoorway subwayDoorway in this.doorways)
			{
				bool flag2 = !this._locked && transform != null && subwayDoorway.zoneCollider != null && subwayDoorway.zoneCollider.bounds.Contains(transform.position);
				if (flag2 != subwayDoorway.PlayerInRange)
				{
					subwayDoorway.PlayerInRange = flag2;
					if (!flag2)
					{
						this.HidePrompt(subwayDoorway);
					}
				}
				if (flag2)
				{
					this.RefreshPrompt(subwayDoorway);
					flag = true;
				}
			}
			if (this._locked || !flag || this.Moving)
			{
				return;
			}
			if (this.interactInput == null || !this.interactInput.GetButtonDown())
			{
				return;
			}
			this.Toggle();
		}

		// Token: 0x06003F78 RID: 16248 RVA: 0x00178B78 File Offset: 0x00176D78
		private Transform ResolvePlayer()
		{
			Transform player = PlayerRegistry.Player;
			if (player != null)
			{
				return player;
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag(this.playerTag);
			if (!(gameObject != null))
			{
				return null;
			}
			return gameObject.transform;
		}

		// Token: 0x06003F79 RID: 16249 RVA: 0x00178BB3 File Offset: 0x00176DB3
		public void Toggle()
		{
			if (this._doorsOpen)
			{
				this.CloseAll();
				return;
			}
			this.OpenAll();
		}

		// Token: 0x06003F7A RID: 16250 RVA: 0x00178BCA File Offset: 0x00176DCA
		public void OpenAll()
		{
			if (this._doorsOpen)
			{
				return;
			}
			this._doorsOpen = true;
			this.StartSlide(true);
		}

		// Token: 0x06003F7B RID: 16251 RVA: 0x00178BE3 File Offset: 0x00176DE3
		public void CloseAll()
		{
			if (!this._doorsOpen)
			{
				return;
			}
			this._doorsOpen = false;
			this.StartSlide(false);
		}

		// Token: 0x06003F7C RID: 16252 RVA: 0x00178BFC File Offset: 0x00176DFC
		public void SetLocked(bool locked)
		{
			this._locked = locked;
			if (locked)
			{
				foreach (SubwayDoorway subwayDoorway in this.doorways)
				{
					subwayDoorway.PlayerInRange = false;
					this.HidePrompt(subwayDoorway);
				}
			}
		}

		// Token: 0x06003F7D RID: 16253 RVA: 0x00178C60 File Offset: 0x00176E60
		private void StartSlide(bool open)
		{
			if (this._slideRoutine != null)
			{
				base.StopCoroutine(this._slideRoutine);
			}
			this._slideActive = true;
			this._slideRoutine = base.StartCoroutine(this.SlideRoutine(open));
			this.PlayAtNearestDoorway((this.hissOverride != null) ? this.hissOverride : SubwayAudioLib.DoorHiss, -1f);
			foreach (SubwayDoorway subwayDoorway in this.doorways)
			{
				this.RefreshPrompt(subwayDoorway);
			}
		}

		// Token: 0x06003F7E RID: 16254 RVA: 0x00178D08 File Offset: 0x00176F08
		private IEnumerator SlideRoutine(bool open)
		{
			Vector3[] starts = new Vector3[this._openingPanels.Count];
			for (int i = 0; i < this._openingPanels.Count; i++)
			{
				starts[i] = this._openingPanels[i].panel.localPosition;
			}
			float t = 0f;
			while (t < 1f)
			{
				while (this._simulationPaused)
				{
					yield return null;
				}
				t = Mathf.Min(1f, t + Time.deltaTime / Mathf.Max(0.05f, this.slideDuration));
				float num = Mathf.SmoothStep(0f, 1f, t);
				for (int j = 0; j < this._openingPanels.Count; j++)
				{
					SubwayDoorPanel subwayDoorPanel = this._openingPanels[j];
					if (!(subwayDoorPanel.panel == null))
					{
						Vector3 vector = (open ? subwayDoorPanel.openLocalPos : subwayDoorPanel.closedLocalPos);
						subwayDoorPanel.panel.localPosition = Vector3.Lerp(starts[j], vector, num);
					}
				}
				yield return null;
			}
			this.PlayAtNearestDoorway((this.thunkOverride != null) ? this.thunkOverride : SubwayAudioLib.DoorThunk, -1f);
			this._slideRoutine = null;
			this._slideActive = false;
			yield break;
		}

		// Token: 0x06003F7F RID: 16255 RVA: 0x00178D20 File Offset: 0x00176F20
		public void PlayAtNearestDoorway(AudioClip clip, float volume = -1f)
		{
			if (clip == null || this._openingPanels.Count == 0)
			{
				return;
			}
			if (this._doorAudio == null)
			{
				GameObject gameObject = new GameObject("SubwayDoorAudio");
				gameObject.transform.SetParent(base.transform, false);
				this._doorAudio = gameObject.AddComponent<AudioSource>();
				this._doorAudio.playOnAwake = false;
				this._doorAudio.spatialBlend = 1f;
				SubwayAudioOcclusion.ApplyRolloffToZero(this._doorAudio, 2.2f, 22f);
				this._doorAudio.dopplerLevel = 0f;
				SubwayRideDirector componentInParent = base.GetComponentInParent<SubwayRideDirector>();
				this._doorOcclusion = SubwayAudioOcclusion.Attach(gameObject, (componentInParent != null) ? componentInParent.transform : base.transform);
			}
			Transform transform = this.ResolvePlayer();
			Vector3 vector = base.transform.position;
			float num = float.MaxValue;
			foreach (SubwayDoorPanel subwayDoorPanel in this._openingPanels)
			{
				if (!(((subwayDoorPanel != null) ? subwayDoorPanel.panel : null) == null))
				{
					Vector3 vector2 = SubwayDoorSystem.DoorwayWorldCenter(subwayDoorPanel);
					float num2 = ((transform != null) ? (vector2 - transform.position).sqrMagnitude : 0f);
					if (num2 < num)
					{
						num = num2;
						vector = vector2;
					}
				}
			}
			this._doorAudio.transform.position = vector;
			if (this._doorOcclusion != null)
			{
				this._doorOcclusion.RefreshImmediately();
			}
			float num3 = ((this._doorOcclusion != null) ? this._doorOcclusion.Factor : 1f);
			this._doorAudio.PlayOneShot(clip, ((volume >= 0f) ? volume : this.doorVolume) * num3);
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x00178F08 File Offset: 0x00177108
		private void RefreshPrompt(SubwayDoorway d)
		{
			if (d.promptUI == null || !d.PlayerInRange)
			{
				return;
			}
			InteractionConfigSO interactionConfigSO = (this._doorsOpen ? ((this.configOpen != null) ? this.configOpen : this.configClosed) : this.configClosed);
			if (!d.PromptShown || d.PromptShownOpen != this._doorsOpen)
			{
				if (interactionConfigSO != null)
				{
					d.promptUI.Show(interactionConfigSO);
				}
				else
				{
					d.promptUI.gameObject.SetActive(true);
				}
				d.PromptShown = true;
				d.PromptShownOpen = this._doorsOpen;
			}
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x00178FAA File Offset: 0x001771AA
		private void HidePrompt(SubwayDoorway d)
		{
			if (d.promptUI != null && d.PromptShown)
			{
				d.promptUI.Hide();
			}
			d.PromptShown = false;
		}

		// Token: 0x04003F41 RID: 16193
		[Header("Doorways (authored by Tools/Subway)")]
		[SerializeField]
		private List<SubwayDoorway> doorways = new List<SubwayDoorway>();

		// Token: 0x04003F42 RID: 16194
		[Header("Prompt")]
		[Tooltip("Config shown while the doors are CLOSED (action = 'Open Doors').")]
		[SerializeField]
		private InteractionConfigSO configClosed;

		// Token: 0x04003F43 RID: 16195
		[Tooltip("Config shown while the doors are OPEN (action = 'Close Doors').")]
		[SerializeField]
		private InteractionConfigSO configOpen;

		// Token: 0x04003F44 RID: 16196
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "X", "E");

		// Token: 0x04003F45 RID: 16197
		[SerializeField]
		private string playerTag = "Player";

		// Token: 0x04003F46 RID: 16198
		[Header("Lato banchina")]
		[Tooltip("Un qualsiasi oggetto che sta sulla BANCHINA: si aprono solo le porte rivolte verso di lui, quelle sull'altro lato restano chiuse sui binari. Serve un riferimento e non un flag perché i due treni della stazione hanno la banchina su lati opposti. Vuoto = si aprono tutte, come prima.")]
		[SerializeField]
		private Transform platformSideReference;

		// Token: 0x04003F47 RID: 16199
		[Header("Motion")]
		[Tooltip("Seconds a panel takes to fully open or close.")]
		[SerializeField]
		private float slideDuration = 1.5f;

		// Token: 0x04003F48 RID: 16200
		[Header("Audio (procedural if left empty)")]
		[SerializeField]
		private AudioClip hissOverride;

		// Token: 0x04003F49 RID: 16201
		[SerializeField]
		private AudioClip thunkOverride;

		// Token: 0x04003F4A RID: 16202
		[Range(0f, 1f)]
		[SerializeField]
		private float doorVolume = 0.8f;

		// Token: 0x04003F4B RID: 16203
		private AudioSource _doorAudio;

		// Token: 0x04003F4C RID: 16204
		private SubwayAudioOcclusion _doorOcclusion;

		// Token: 0x04003F4D RID: 16205
		private bool _doorsOpen;

		// Token: 0x04003F4E RID: 16206
		private bool _slideActive;

		// Token: 0x04003F4F RID: 16207
		private bool _locked;

		// Token: 0x04003F50 RID: 16208
		private Coroutine _slideRoutine;

		// Token: 0x04003F51 RID: 16209
		private bool _simulationPaused;

		// Token: 0x04003F52 RID: 16210
		private readonly List<SubwayDoorPanel> _openingPanels = new List<SubwayDoorPanel>();
	}
}
