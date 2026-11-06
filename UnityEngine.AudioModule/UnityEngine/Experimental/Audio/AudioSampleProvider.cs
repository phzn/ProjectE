using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Audio
{
	// Token: 0x02000025 RID: 37
	[NativeType(Header = "Modules/Audio/Public/ScriptBindings/AudioSampleProvider.bindings.h")]
	[StaticAccessor("AudioSampleProviderBindings", StaticAccessorType.DoubleColon)]
	public class AudioSampleProvider : IDisposable
	{
		// Token: 0x0600017F RID: 383 RVA: 0x00002FEC File Offset: 0x000011EC
		[VisibleToOtherModules]
		internal static AudioSampleProvider Lookup(uint providerId, Object ownerObj, ushort trackIndex)
		{
			AudioSampleProvider audioSampleProvider = AudioSampleProvider.InternalGetScriptingPtr(providerId);
			bool flag = audioSampleProvider != null || !AudioSampleProvider.InternalIsValid(providerId);
			AudioSampleProvider audioSampleProvider2;
			if (flag)
			{
				audioSampleProvider2 = audioSampleProvider;
			}
			else
			{
				audioSampleProvider2 = new AudioSampleProvider(providerId, ownerObj, trackIndex);
			}
			return audioSampleProvider2;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00003024 File Offset: 0x00001224
		internal static AudioSampleProvider Create(ushort channelCount, uint sampleRate)
		{
			uint num = AudioSampleProvider.InternalCreateSampleProvider(channelCount, sampleRate);
			bool flag = !AudioSampleProvider.InternalIsValid(num);
			AudioSampleProvider audioSampleProvider;
			if (flag)
			{
				audioSampleProvider = null;
			}
			else
			{
				audioSampleProvider = new AudioSampleProvider(num, null, 0);
			}
			return audioSampleProvider;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00003058 File Offset: 0x00001258
		private AudioSampleProvider(uint providerId, Object ownerObj, ushort trackIdx)
		{
			this.owner = ownerObj;
			this.id = providerId;
			this.trackIndex = trackIdx;
			this.m_ConsumeSampleFramesNativeFunction = (AudioSampleProvider.ConsumeSampleFramesNativeFunction)Marshal.GetDelegateForFunctionPointer(AudioSampleProvider.InternalGetConsumeSampleFramesNativeFunctionPtr(), typeof(AudioSampleProvider.ConsumeSampleFramesNativeFunction));
			ushort num = 0;
			uint num2 = 0U;
			AudioSampleProvider.InternalGetFormatInfo(providerId, out num, out num2);
			this.channelCount = num;
			this.sampleRate = num2;
			AudioSampleProvider.InternalSetScriptingPtr(providerId, this);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x000030CC File Offset: 0x000012CC
		~AudioSampleProvider()
		{
			this.owner = null;
			this.Dispose();
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00003104 File Offset: 0x00001304
		public void Dispose()
		{
			bool flag = this.id > 0U;
			if (flag)
			{
				AudioSampleProvider.InternalSetScriptingPtr(this.id, null);
				bool flag2 = this.owner == null;
				if (flag2)
				{
					AudioSampleProvider.InternalRemove(this.id);
				}
				this.id = 0U;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00003159 File Offset: 0x00001359
		// (set) Token: 0x06000185 RID: 389 RVA: 0x00003161 File Offset: 0x00001361
		public uint id { get; private set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000186 RID: 390 RVA: 0x0000316A File Offset: 0x0000136A
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00003172 File Offset: 0x00001372
		public ushort trackIndex { get; private set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000188 RID: 392 RVA: 0x0000317B File Offset: 0x0000137B
		// (set) Token: 0x06000189 RID: 393 RVA: 0x00003183 File Offset: 0x00001383
		public Object owner { get; private set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600018A RID: 394 RVA: 0x0000318C File Offset: 0x0000138C
		public bool valid
		{
			get
			{
				return AudioSampleProvider.InternalIsValid(this.id);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600018B RID: 395 RVA: 0x000031A9 File Offset: 0x000013A9
		// (set) Token: 0x0600018C RID: 396 RVA: 0x000031B1 File Offset: 0x000013B1
		public ushort channelCount { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600018D RID: 397 RVA: 0x000031BA File Offset: 0x000013BA
		// (set) Token: 0x0600018E RID: 398 RVA: 0x000031C2 File Offset: 0x000013C2
		public uint sampleRate { get; private set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600018F RID: 399 RVA: 0x000031CC File Offset: 0x000013CC
		public uint maxSampleFrameCount
		{
			get
			{
				return AudioSampleProvider.InternalGetMaxSampleFrameCount(this.id);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000190 RID: 400 RVA: 0x000031EC File Offset: 0x000013EC
		public uint availableSampleFrameCount
		{
			get
			{
				return AudioSampleProvider.InternalGetAvailableSampleFrameCount(this.id);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000191 RID: 401 RVA: 0x0000320C File Offset: 0x0000140C
		public uint freeSampleFrameCount
		{
			get
			{
				return AudioSampleProvider.InternalGetFreeSampleFrameCount(this.id);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000192 RID: 402 RVA: 0x0000322C File Offset: 0x0000142C
		// (set) Token: 0x06000193 RID: 403 RVA: 0x00003249 File Offset: 0x00001449
		public uint freeSampleFrameCountLowThreshold
		{
			get
			{
				return AudioSampleProvider.InternalGetFreeSampleFrameCountLowThreshold(this.id);
			}
			set
			{
				AudioSampleProvider.InternalSetFreeSampleFrameCountLowThreshold(this.id, value);
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000194 RID: 404 RVA: 0x0000325C File Offset: 0x0000145C
		// (set) Token: 0x06000195 RID: 405 RVA: 0x00003279 File Offset: 0x00001479
		public bool enableSampleFramesAvailableEvents
		{
			get
			{
				return AudioSampleProvider.InternalGetEnableSampleFramesAvailableEvents(this.id);
			}
			set
			{
				AudioSampleProvider.InternalSetEnableSampleFramesAvailableEvents(this.id, value);
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000328C File Offset: 0x0000148C
		// (set) Token: 0x06000197 RID: 407 RVA: 0x000032A9 File Offset: 0x000014A9
		public bool enableSilencePadding
		{
			get
			{
				return AudioSampleProvider.InternalGetEnableSilencePadding(this.id);
			}
			set
			{
				AudioSampleProvider.InternalSetEnableSilencePadding(this.id, value);
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000032BC File Offset: 0x000014BC
		public uint ConsumeSampleFrames(NativeArray<float> sampleFrames)
		{
			bool flag = this.channelCount == 0;
			uint num;
			if (flag)
			{
				num = 0U;
			}
			else
			{
				num = this.m_ConsumeSampleFramesNativeFunction(this.id, (IntPtr)sampleFrames.GetUnsafePtr<float>(), (uint)(sampleFrames.Length / (int)this.channelCount));
			}
			return num;
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000330C File Offset: 0x0000150C
		public static AudioSampleProvider.ConsumeSampleFramesNativeFunction consumeSampleFramesNativeFunction
		{
			get
			{
				return (AudioSampleProvider.ConsumeSampleFramesNativeFunction)Marshal.GetDelegateForFunctionPointer(AudioSampleProvider.InternalGetConsumeSampleFramesNativeFunctionPtr(), typeof(AudioSampleProvider.ConsumeSampleFramesNativeFunction));
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00003338 File Offset: 0x00001538
		internal uint QueueSampleFrames(NativeArray<float> sampleFrames)
		{
			bool flag = this.channelCount == 0;
			uint num;
			if (flag)
			{
				num = 0U;
			}
			else
			{
				num = AudioSampleProvider.InternalQueueSampleFrames(this.id, (IntPtr)sampleFrames.GetUnsafeReadOnlyPtr<float>(), (uint)(sampleFrames.Length / (int)this.channelCount));
			}
			return num;
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600019B RID: 411 RVA: 0x00003380 File Offset: 0x00001580
		// (remove) Token: 0x0600019C RID: 412 RVA: 0x000033B8 File Offset: 0x000015B8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event AudioSampleProvider.SampleFramesHandler sampleFramesAvailable;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600019D RID: 413 RVA: 0x000033F0 File Offset: 0x000015F0
		// (remove) Token: 0x0600019E RID: 414 RVA: 0x00003428 File Offset: 0x00001628
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event AudioSampleProvider.SampleFramesHandler sampleFramesOverflow;

		// Token: 0x0600019F RID: 415 RVA: 0x0000345D File Offset: 0x0000165D
		public void SetSampleFramesAvailableNativeHandler(AudioSampleProvider.SampleFramesEventNativeFunction handler, IntPtr userData)
		{
			AudioSampleProvider.InternalSetSampleFramesAvailableNativeHandler(this.id, Marshal.GetFunctionPointerForDelegate<AudioSampleProvider.SampleFramesEventNativeFunction>(handler), userData);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00003473 File Offset: 0x00001673
		public void ClearSampleFramesAvailableNativeHandler()
		{
			AudioSampleProvider.InternalClearSampleFramesAvailableNativeHandler(this.id);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00003482 File Offset: 0x00001682
		public void SetSampleFramesOverflowNativeHandler(AudioSampleProvider.SampleFramesEventNativeFunction handler, IntPtr userData)
		{
			AudioSampleProvider.InternalSetSampleFramesOverflowNativeHandler(this.id, Marshal.GetFunctionPointerForDelegate<AudioSampleProvider.SampleFramesEventNativeFunction>(handler), userData);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00003498 File Offset: 0x00001698
		public void ClearSampleFramesOverflowNativeHandler()
		{
			AudioSampleProvider.InternalClearSampleFramesOverflowNativeHandler(this.id);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000034A8 File Offset: 0x000016A8
		[RequiredByNativeCode]
		private void InvokeSampleFramesAvailable(int sampleFrameCount)
		{
			bool flag = this.sampleFramesAvailable != null;
			if (flag)
			{
				this.sampleFramesAvailable(this, (uint)sampleFrameCount);
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x000034D4 File Offset: 0x000016D4
		[RequiredByNativeCode]
		private void InvokeSampleFramesOverflow(int droppedSampleFrameCount)
		{
			bool flag = this.sampleFramesOverflow != null;
			if (flag)
			{
				this.sampleFramesOverflow(this, (uint)droppedSampleFrameCount);
			}
		}

		// Token: 0x060001A5 RID: 421
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint InternalCreateSampleProvider(ushort channelCount, uint sampleRate);

		// Token: 0x060001A6 RID: 422
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InternalRemove(uint providerId);

		// Token: 0x060001A7 RID: 423
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalGetFormatInfo(uint providerId, out ushort chCount, out uint sRate);

		// Token: 0x060001A8 RID: 424
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern AudioSampleProvider InternalGetScriptingPtr(uint providerId);

		// Token: 0x060001A9 RID: 425
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalSetScriptingPtr(uint providerId, AudioSampleProvider provider);

		// Token: 0x060001AA RID: 426
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool InternalIsValid(uint providerId);

		// Token: 0x060001AB RID: 427
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint InternalGetMaxSampleFrameCount(uint providerId);

		// Token: 0x060001AC RID: 428
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint InternalGetAvailableSampleFrameCount(uint providerId);

		// Token: 0x060001AD RID: 429
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint InternalGetFreeSampleFrameCount(uint providerId);

		// Token: 0x060001AE RID: 430
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint InternalGetFreeSampleFrameCountLowThreshold(uint providerId);

		// Token: 0x060001AF RID: 431
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalSetFreeSampleFrameCountLowThreshold(uint providerId, uint sampleFrameCount);

		// Token: 0x060001B0 RID: 432
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool InternalGetEnableSampleFramesAvailableEvents(uint providerId);

		// Token: 0x060001B1 RID: 433
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalSetEnableSampleFramesAvailableEvents(uint providerId, bool enable);

		// Token: 0x060001B2 RID: 434
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalSetSampleFramesAvailableNativeHandler(uint providerId, IntPtr handler, IntPtr userData);

		// Token: 0x060001B3 RID: 435
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalClearSampleFramesAvailableNativeHandler(uint providerId);

		// Token: 0x060001B4 RID: 436
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalSetSampleFramesOverflowNativeHandler(uint providerId, IntPtr handler, IntPtr userData);

		// Token: 0x060001B5 RID: 437
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalClearSampleFramesOverflowNativeHandler(uint providerId);

		// Token: 0x060001B6 RID: 438
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool InternalGetEnableSilencePadding(uint id);

		// Token: 0x060001B7 RID: 439
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalSetEnableSilencePadding(uint id, bool enabled);

		// Token: 0x060001B8 RID: 440
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr InternalGetConsumeSampleFramesNativeFunctionPtr();

		// Token: 0x060001B9 RID: 441
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern uint InternalQueueSampleFrames(uint id, IntPtr interleavedSampleFrames, uint sampleFrameCount);

		// Token: 0x0400006B RID: 107
		private AudioSampleProvider.ConsumeSampleFramesNativeFunction m_ConsumeSampleFramesNativeFunction;

		// Token: 0x02000026 RID: 38
		// (Invoke) Token: 0x060001BB RID: 443
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate uint ConsumeSampleFramesNativeFunction(uint providerId, IntPtr interleavedSampleFrames, uint sampleFrameCount);

		// Token: 0x02000027 RID: 39
		// (Invoke) Token: 0x060001BF RID: 447
		public delegate void SampleFramesHandler(AudioSampleProvider provider, uint sampleFrameCount);

		// Token: 0x02000028 RID: 40
		// (Invoke) Token: 0x060001C3 RID: 451
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void SampleFramesEventNativeFunction(IntPtr userData, uint providerId, uint sampleFrameCount);
	}
}
