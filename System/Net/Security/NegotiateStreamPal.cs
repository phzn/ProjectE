using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Net.Security
{
	// Token: 0x0200084C RID: 2124
	internal static class NegotiateStreamPal
	{
		// Token: 0x0600438D RID: 17293 RVA: 0x000EBADA File Offset: 0x000E9CDA
		internal static int QueryMaxTokenSize(string package)
		{
			return SSPIWrapper.GetVerifyPackageInfo(GlobalSSPI.SSPIAuth, package, true).MaxToken;
		}

		// Token: 0x0600438E RID: 17294 RVA: 0x000EBAED File Offset: 0x000E9CED
		internal static SafeFreeCredentials AcquireDefaultCredential(string package, bool isServer)
		{
			return SSPIWrapper.AcquireDefaultCredential(GlobalSSPI.SSPIAuth, package, isServer ? global::Interop.SspiCli.CredentialUse.SECPKG_CRED_INBOUND : global::Interop.SspiCli.CredentialUse.SECPKG_CRED_OUTBOUND);
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x000EBB04 File Offset: 0x000E9D04
		internal static SafeFreeCredentials AcquireCredentialsHandle(string package, bool isServer, NetworkCredential credential)
		{
			SafeSspiAuthDataHandle safeSspiAuthDataHandle = null;
			SafeFreeCredentials safeFreeCredentials;
			try
			{
				global::Interop.SECURITY_STATUS security_STATUS = global::Interop.SspiCli.SspiEncodeStringsAsAuthIdentity(credential.UserName, credential.Domain, credential.Password, out safeSspiAuthDataHandle);
				if (security_STATUS != global::Interop.SECURITY_STATUS.OK)
				{
					if (NetEventSource.IsEnabled)
					{
						NetEventSource.Error(null, SR.Format("{0} failed with error {1}.", "SspiEncodeStringsAsAuthIdentity", string.Format("0x{0:X}", (int)security_STATUS)), "AcquireCredentialsHandle");
					}
					throw new Win32Exception((int)security_STATUS);
				}
				safeFreeCredentials = SSPIWrapper.AcquireCredentialsHandle(GlobalSSPI.SSPIAuth, package, isServer ? global::Interop.SspiCli.CredentialUse.SECPKG_CRED_INBOUND : global::Interop.SspiCli.CredentialUse.SECPKG_CRED_OUTBOUND, ref safeSspiAuthDataHandle);
			}
			finally
			{
				if (safeSspiAuthDataHandle != null)
				{
					safeSspiAuthDataHandle.Dispose();
				}
			}
			return safeFreeCredentials;
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x000EBB9C File Offset: 0x000E9D9C
		internal static string QueryContextClientSpecifiedSpn(SafeDeleteContext securityContext)
		{
			return SSPIWrapper.QueryContextAttributes(GlobalSSPI.SSPIAuth, securityContext, global::Interop.SspiCli.ContextAttribute.SECPKG_ATTR_CLIENT_SPECIFIED_TARGET) as string;
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x000EBBB0 File Offset: 0x000E9DB0
		internal static string QueryContextAuthenticationPackage(SafeDeleteContext securityContext)
		{
			NegotiationInfoClass negotiationInfoClass = SSPIWrapper.QueryContextAttributes(GlobalSSPI.SSPIAuth, securityContext, global::Interop.SspiCli.ContextAttribute.SECPKG_ATTR_NEGOTIATION_INFO) as NegotiationInfoClass;
			if (negotiationInfoClass == null)
			{
				return null;
			}
			return negotiationInfoClass.AuthenticationPackage;
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x000EBBD0 File Offset: 0x000E9DD0
		internal static SecurityStatusPal InitializeSecurityContext(SafeFreeCredentials credentialsHandle, ref SafeDeleteContext securityContext, string spn, ContextFlagsPal requestedContextFlags, SecurityBuffer[] inSecurityBufferArray, SecurityBuffer outSecurityBuffer, ref ContextFlagsPal contextFlags)
		{
			global::Interop.SspiCli.ContextFlags contextFlags2 = global::Interop.SspiCli.ContextFlags.Zero;
			global::Interop.SECURITY_STATUS security_STATUS = (global::Interop.SECURITY_STATUS)SSPIWrapper.InitializeSecurityContext(GlobalSSPI.SSPIAuth, credentialsHandle, ref securityContext, spn, ContextFlagsAdapterPal.GetInteropFromContextFlagsPal(requestedContextFlags), global::Interop.SspiCli.Endianness.SECURITY_NETWORK_DREP, inSecurityBufferArray, outSecurityBuffer, ref contextFlags2);
			contextFlags = ContextFlagsAdapterPal.GetContextFlagsPalFromInterop(contextFlags2);
			return SecurityStatusAdapterPal.GetSecurityStatusPalFromInterop(security_STATUS, false);
		}

		// Token: 0x06004393 RID: 17299 RVA: 0x000EBC08 File Offset: 0x000E9E08
		internal static SecurityStatusPal CompleteAuthToken(ref SafeDeleteContext securityContext, SecurityBuffer[] inSecurityBufferArray)
		{
			return SecurityStatusAdapterPal.GetSecurityStatusPalFromInterop((global::Interop.SECURITY_STATUS)SSPIWrapper.CompleteAuthToken(GlobalSSPI.SSPIAuth, ref securityContext, inSecurityBufferArray), false);
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x000EBC1C File Offset: 0x000E9E1C
		internal static SecurityStatusPal AcceptSecurityContext(SafeFreeCredentials credentialsHandle, ref SafeDeleteContext securityContext, ContextFlagsPal requestedContextFlags, SecurityBuffer[] inSecurityBufferArray, SecurityBuffer outSecurityBuffer, ref ContextFlagsPal contextFlags)
		{
			global::Interop.SspiCli.ContextFlags contextFlags2 = global::Interop.SspiCli.ContextFlags.Zero;
			global::Interop.SECURITY_STATUS security_STATUS = (global::Interop.SECURITY_STATUS)SSPIWrapper.AcceptSecurityContext(GlobalSSPI.SSPIAuth, credentialsHandle, ref securityContext, ContextFlagsAdapterPal.GetInteropFromContextFlagsPal(requestedContextFlags), global::Interop.SspiCli.Endianness.SECURITY_NETWORK_DREP, inSecurityBufferArray, outSecurityBuffer, ref contextFlags2);
			contextFlags = ContextFlagsAdapterPal.GetContextFlagsPalFromInterop(contextFlags2);
			return SecurityStatusAdapterPal.GetSecurityStatusPalFromInterop(security_STATUS, false);
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x000EBC52 File Offset: 0x000E9E52
		internal static Win32Exception CreateExceptionFromError(SecurityStatusPal statusCode)
		{
			return new Win32Exception((int)SecurityStatusAdapterPal.GetInteropFromSecurityStatusPal(statusCode));
		}

		// Token: 0x06004396 RID: 17302 RVA: 0x000EBC60 File Offset: 0x000E9E60
		internal static int VerifySignature(SafeDeleteContext securityContext, byte[] buffer, int offset, int count)
		{
			if (offset < 0 || offset > ((buffer == null) ? 0 : buffer.Length))
			{
				NetEventSource.Info("Argument 'offset' out of range.", null, "VerifySignature");
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || count > ((buffer == null) ? 0 : (buffer.Length - offset)))
			{
				NetEventSource.Info("Argument 'count' out of range.", null, "VerifySignature");
				throw new ArgumentOutOfRangeException("count");
			}
			SecurityBuffer[] array = new SecurityBuffer[]
			{
				new SecurityBuffer(buffer, offset, count, SecurityBufferType.SECBUFFER_STREAM),
				new SecurityBuffer(0, SecurityBufferType.SECBUFFER_DATA)
			};
			int num = SSPIWrapper.VerifySignature(GlobalSSPI.SSPIAuth, securityContext, array, 0U);
			if (num != 0)
			{
				NetEventSource.Info("VerifySignature threw error: " + num.ToString("x", NumberFormatInfo.InvariantInfo), null, "VerifySignature");
				throw new Win32Exception(num);
			}
			if (array[1].type != SecurityBufferType.SECBUFFER_DATA)
			{
				throw new InternalException();
			}
			return array[1].size;
		}

		// Token: 0x06004397 RID: 17303 RVA: 0x000EBD3C File Offset: 0x000E9F3C
		internal static int MakeSignature(SafeDeleteContext securityContext, byte[] buffer, int offset, int count, ref byte[] output)
		{
			SecPkgContext_Sizes secPkgContext_Sizes = SSPIWrapper.QueryContextAttributes(GlobalSSPI.SSPIAuth, securityContext, global::Interop.SspiCli.ContextAttribute.SECPKG_ATTR_SIZES) as SecPkgContext_Sizes;
			int num = count + secPkgContext_Sizes.cbMaxSignature;
			if (output == null || output.Length < num)
			{
				output = new byte[num];
			}
			Buffer.BlockCopy(buffer, offset, output, secPkgContext_Sizes.cbMaxSignature, count);
			SecurityBuffer[] array = new SecurityBuffer[]
			{
				new SecurityBuffer(output, 0, secPkgContext_Sizes.cbMaxSignature, SecurityBufferType.SECBUFFER_TOKEN),
				new SecurityBuffer(output, secPkgContext_Sizes.cbMaxSignature, count, SecurityBufferType.SECBUFFER_DATA)
			};
			int num2 = SSPIWrapper.MakeSignature(GlobalSSPI.SSPIAuth, securityContext, array, 0U);
			if (num2 != 0)
			{
				NetEventSource.Info("MakeSignature threw error: " + num2.ToString("x", NumberFormatInfo.InvariantInfo), null, "MakeSignature");
				throw new Win32Exception(num2);
			}
			return array[0].size + array[1].size;
		}
	}
}
