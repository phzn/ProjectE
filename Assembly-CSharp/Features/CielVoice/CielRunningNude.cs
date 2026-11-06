using System;
using Features.RunModes;
using Invector.vCharacterController;
using ProjectExhibitionist;
using UnityEngine;

namespace Features.CielVoice
{
	// Token: 0x0200135C RID: 4956
	[DisallowMultipleComponent]
	public class CielRunningNude : MonoBehaviour
	{
		// Token: 0x17001862 RID: 6242
		// (get) Token: 0x06009954 RID: 39252 RVA: 0x003EDB89 File Offset: 0x003EBD89
		private bool ShouldPlay
		{
			get
			{
				return !RunModeService.IsAtHome && this._isNaked && this._isRunning && !this._voiceDucking;
			}
		}

		// Token: 0x17001863 RID: 6243
		// (get) Token: 0x06009955 RID: 39253 RVA: 0x003EDBAD File Offset: 0x003EBDAD
		public bool IsActive
		{
			get
			{
				return this.ShouldPlay;
			}
		}

		// Token: 0x06009956 RID: 39254 RVA: 0x003EDBB8 File Offset: 0x003EBDB8
		private void Awake()
		{
			this.EnsureSources();
			if (this.clothing == null)
			{
				this.clothing = Object.FindObjectOfType<ClothingToggleController>();
			}
			if (this.controller == null)
			{
				this.controller = base.GetComponentInParent<vThirdPersonController>();
			}
			if (this.controller == null)
			{
				this.controller = Object.FindObjectOfType<vThirdPersonController>();
			}
			if (this.voiceController == null)
			{
				this.voiceController = CielVoiceController.Instance;
			}
			this._controllerTf = ((this.controller != null) ? this.controller.transform : null);
			if (this._controllerTf != null)
			{
				this._lastPos = this._controllerTf.position;
			}
			if (this.controller != null)
			{
				this._rb = this.controller.GetComponent<Rigidbody>();
			}
			this._loop = new CrossfadeLoopPlayer(this.sourceA, this.sourceB, this.runningClip, this.loopCrossfade, 0f, 0f);
		}

		// Token: 0x06009957 RID: 39255 RVA: 0x003EDCBD File Offset: 0x003EBEBD
		private void EnsureSources()
		{
			this.sourceA = this.EnsureOne(this.sourceA, "CielRunningNudeSourceA");
			this.sourceB = this.EnsureOne(this.sourceB, "CielRunningNudeSourceB");
		}

		// Token: 0x06009958 RID: 39256 RVA: 0x003EDCF0 File Offset: 0x003EBEF0
		private AudioSource EnsureOne(AudioSource src, string childName)
		{
			if (src == null)
			{
				GameObject gameObject = new GameObject(childName);
				gameObject.transform.SetParent(base.transform, false);
				src = gameObject.AddComponent<AudioSource>();
				src.spatialBlend = 0f;
				if (this.debugLog)
				{
					Debug.LogWarning("[CielRunningNude] " + childName + " non cablato: creato fallback runtime.");
				}
			}
			src.loop = false;
			src.playOnAwake = false;
			if (this.runningClip != null)
			{
				src.clip = this.runningClip;
			}
			src.volume = 0f;
			return src;
		}

		// Token: 0x06009959 RID: 39257 RVA: 0x003EDD84 File Offset: 0x003EBF84
		private void OnEnable()
		{
			if (this.voiceController == null)
			{
				this.voiceController = CielVoiceController.Instance;
			}
			if (this.clothing != null)
			{
				this.clothing.OnCoverageChanged += this.HandleCoverageChanged;
				this._isNaked = !this.clothing.IsFullyClothed;
			}
			if (this.voiceController != null)
			{
				this.voiceController.OnVoiceStarted += this.HandleVoiceStarted;
				this.voiceController.OnVoiceEnded += this.HandleVoiceEnded;
				this._voiceDucking = this.voiceController.IsSpeaking;
			}
		}

		// Token: 0x0600995A RID: 39258 RVA: 0x003EDE30 File Offset: 0x003EC030
		private void OnDisable()
		{
			if (this.clothing != null)
			{
				this.clothing.OnCoverageChanged -= this.HandleCoverageChanged;
			}
			if (this.voiceController != null)
			{
				this.voiceController.OnVoiceStarted -= this.HandleVoiceStarted;
				this.voiceController.OnVoiceEnded -= this.HandleVoiceEnded;
			}
			CrossfadeLoopPlayer loop = this._loop;
			if (loop != null)
			{
				loop.Stop();
			}
			this._currentVol = 0f;
		}

		// Token: 0x0600995B RID: 39259 RVA: 0x003EDEBA File Offset: 0x003EC0BA
		private void HandleCoverageChanged(bool isFullyClothed)
		{
			this._isNaked = !isFullyClothed;
		}

		// Token: 0x0600995C RID: 39260 RVA: 0x003EDEC6 File Offset: 0x003EC0C6
		private void HandleVoiceStarted(CielVoiceSO voice)
		{
			this._voiceDucking = true;
		}

		// Token: 0x0600995D RID: 39261 RVA: 0x003EDECF File Offset: 0x003EC0CF
		private void HandleVoiceEnded(CielVoiceSO voice)
		{
			this._voiceDucking = false;
		}

		// Token: 0x0600995E RID: 39262 RVA: 0x003EDED8 File Offset: 0x003EC0D8
		private void Update()
		{
			if (this._loop == null)
			{
				return;
			}
			this.UpdateRunningState();
			if (RunModeService.IsAtHome)
			{
				this._loop.Pause();
				this._currentVol = 0f;
				return;
			}
			if (this.ShouldPlay && !this._loop.IsPlaying)
			{
				this._loop.Resume();
			}
			float num = (this.ShouldPlay ? this.volume : 0f);
			float num2 = ((num > this._currentVol) ? this.fadeInDuration : this.fadeOutDuration);
			if (num2 <= 0f)
			{
				this._currentVol = num;
			}
			else
			{
				this._currentVol = Mathf.MoveTowards(this._currentVol, num, this.volume / Mathf.Max(0.0001f, num2) * Time.unscaledDeltaTime);
			}
			this._loop.Tick(this._currentVol);
			if (!this.ShouldPlay && this._currentVol <= 0.001f && this._loop.IsPlaying)
			{
				this._loop.Pause();
			}
		}

		// Token: 0x0600995F RID: 39263 RVA: 0x003EDFDC File Offset: 0x003EC1DC
		private void UpdateRunningState()
		{
			bool flag = this.controller != null && this.controller.isSprinting;
			float num = 0f;
			if (this._rb != null)
			{
				Vector3 velocity = this._rb.velocity;
				velocity.y = 0f;
				num = velocity.magnitude;
			}
			else if (this._controllerTf != null)
			{
				Vector3 vector = this._controllerTf.position - this._lastPos;
				vector.y = 0f;
				float num2 = Mathf.Max(0.0001f, Time.deltaTime);
				num = vector.magnitude / num2;
				this._lastPos = this._controllerTf.position;
			}
			float num3 = (this._isRunning ? Mathf.Max(0f, this.minRunSpeed - this.runStopHysteresis) : this.minRunSpeed);
			bool flag2 = num >= num3 && (!this.requireSprint || flag);
			if (flag2 != this._isRunning && this.debugLog)
			{
				Debug.Log(string.Format("[CielRunningNude] running={0} (speed={1:F1}, sprint={2}, naked={3}, duck={4})", new object[] { flag2, num, flag, this._isNaked, this._voiceDucking }));
			}
			this._isRunning = flag2;
		}

		// Token: 0x04009E6A RID: 40554
		[Header("References")]
		[Tooltip("Primo AudioSource dedicato (crossfade loop). GO sotto Ciel_Invector/Audio.")]
		[SerializeField]
		private AudioSource sourceA;

		// Token: 0x04009E6B RID: 40555
		[Tooltip("Secondo AudioSource dedicato (crossfade loop). GO sotto Ciel_Invector/Audio.")]
		[SerializeField]
		private AudioSource sourceB;

		// Token: 0x04009E6C RID: 40556
		[Tooltip("Clip del running-nude in loop.")]
		[SerializeField]
		private AudioClip runningClip;

		// Token: 0x04009E6D RID: 40557
		[Tooltip("ClothingToggleController da osservare. Auto-risolto in scena se null.")]
		[SerializeField]
		private ClothingToggleController clothing;

		// Token: 0x04009E6E RID: 40558
		[Tooltip("vThirdPersonController del player (per leggere isSprinting/velocita'). Auto-risolto se null.")]
		[SerializeField]
		private vThirdPersonController controller;

		// Token: 0x04009E6F RID: 40559
		[Tooltip("CielVoiceController da osservare per il ducking. Se null usa l'Instance.")]
		[SerializeField]
		private CielVoiceController voiceController;

		// Token: 0x04009E70 RID: 40560
		[Header("Run detection")]
		[Tooltip("Velocita' orizzontale minima (m/s) per considerare Ciel 'in corsa'. Con walk~1-2 e run~3-4, una soglia ~2.5 distingue la corsa dalla camminata. Questo e' il criterio PRINCIPALE: nel gioco lo sprint di Invector potrebbe non essere usato (sprintSpeed=0).")]
		[Min(0f)]
		[SerializeField]
		private float minRunSpeed = 2.5f;

		// Token: 0x04009E71 RID: 40561
		[Tooltip("Isteresi usata solo per fermare il loop: evita continui on/off quando la velocita' oscilla attorno alla soglia di corsa.")]
		[Min(0f)]
		[SerializeField]
		private float runStopHysteresis = 0.25f;

		// Token: 0x04009E72 RID: 40562
		[Tooltip("Se true richiede ANCHE controller.isSprinting oltre alla velocita'. Lascialo OFF: in questo gioco Ciel corre a runningSpeed senza attivare isSprinting, quindi richiederlo impedirebbe alla voce di partire.")]
		[SerializeField]
		private bool requireSprint;

		// Token: 0x04009E73 RID: 40563
		[Header("Volume")]
		[Tooltip("Volume quando pienamente udibile (stesso valore del breathing).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float volume = 0.18f;

		// Token: 0x04009E74 RID: 40564
		[Header("Loop")]
		[Tooltip("Durata (s) del crossfade al giro del loop.")]
		[Min(0.02f)]
		[SerializeField]
		private float loopCrossfade = 0.25f;

		// Token: 0x04009E75 RID: 40565
		[Header("Fade")]
		[Tooltip("Durata (s) del fade-in.")]
		[Min(0f)]
		[SerializeField]
		private float fadeInDuration = 0.4f;

		// Token: 0x04009E76 RID: 40566
		[Tooltip("Durata (s) del fade-out (stop corsa / ducking).")]
		[Min(0f)]
		[SerializeField]
		private float fadeOutDuration = 0.4f;

		// Token: 0x04009E77 RID: 40567
		[Header("Debug")]
		[SerializeField]
		private bool debugLog;

		// Token: 0x04009E78 RID: 40568
		private bool _isNaked;

		// Token: 0x04009E79 RID: 40569
		private bool _voiceDucking;

		// Token: 0x04009E7A RID: 40570
		private float _currentVol;

		// Token: 0x04009E7B RID: 40571
		private CrossfadeLoopPlayer _loop;

		// Token: 0x04009E7C RID: 40572
		private Transform _controllerTf;

		// Token: 0x04009E7D RID: 40573
		private Rigidbody _rb;

		// Token: 0x04009E7E RID: 40574
		private Vector3 _lastPos;

		// Token: 0x04009E7F RID: 40575
		private bool _isRunning;
	}
}
