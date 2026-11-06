using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Net.WebSockets
{
	/// <summary>Represents an exception that occurred when performing an operation on a WebSocket connection.</summary>
	// Token: 0x0200083C RID: 2108
	[Serializable]
	public sealed class WebSocketException : Win32Exception
	{
		/// <summary>Creates an instance of the <see cref="T:System.Net.WebSockets.WebSocketException" /> class.</summary>
		// Token: 0x0600433E RID: 17214 RVA: 0x000EA316 File Offset: 0x000E8516
		public WebSocketException()
			: this(Marshal.GetLastWin32Error())
		{
		}

		/// <summary>Creates an instance of the <see cref="T:System.Net.WebSockets.WebSocketException" /> class.</summary>
		/// <param name="error">The error from the WebSocketError enumeration.</param>
		// Token: 0x0600433F RID: 17215 RVA: 0x000EA323 File Offset: 0x000E8523
		public WebSocketException(WebSocketError error)
			: this(error, WebSocketException.GetErrorMessage(error))
		{
		}

		/// <summary>Creates an instance of the <see cref="T:System.Net.WebSockets.WebSocketException" /> class.</summary>
		/// <param name="error">The error from the WebSocketError enumeration.</param>
		/// <param name="message">The description of the error.</param>
		// Token: 0x06004340 RID: 17216 RVA: 0x000EA332 File Offset: 0x000E8532
		public WebSocketException(WebSocketError error, string message)
			: base(message)
		{
			this._webSocketErrorCode = error;
		}

		/// <summary>Creates an instance of the <see cref="T:System.Net.WebSockets.WebSocketException" /> class.</summary>
		/// <param name="error">The error from the WebSocketError enumeration.</param>
		/// <param name="innerException">Indicates the previous exception that led to the current exception.</param>
		// Token: 0x06004341 RID: 17217 RVA: 0x000EA342 File Offset: 0x000E8542
		public WebSocketException(WebSocketError error, Exception innerException)
			: this(error, WebSocketException.GetErrorMessage(error), innerException)
		{
		}

		/// <summary>Creates an instance of the <see cref="T:System.Net.WebSockets.WebSocketException" /> class.</summary>
		/// <param name="error">The error from the WebSocketError enumeration.</param>
		/// <param name="message">The description of the error.</param>
		/// <param name="innerException">Indicates the previous exception that led to the current exception.</param>
		// Token: 0x06004342 RID: 17218 RVA: 0x000EA352 File Offset: 0x000E8552
		public WebSocketException(WebSocketError error, string message, Exception innerException)
			: base(message, innerException)
		{
			this._webSocketErrorCode = error;
		}

		/// <summary>Creates an instance of the <see cref="T:System.Net.WebSockets.WebSocketException" /> class.</summary>
		/// <param name="nativeError">The native error code for the exception.</param>
		// Token: 0x06004343 RID: 17219 RVA: 0x000EA363 File Offset: 0x000E8563
		public WebSocketException(int nativeError)
			: base(nativeError)
		{
			this._webSocketErrorCode = ((!WebSocketException.Succeeded(nativeError)) ? WebSocketError.NativeError : WebSocketError.Success);
			this.SetErrorCodeOnError(nativeError);
		}

		/// <summary>Creates an instance of the <see cref="T:System.Net.WebSockets.WebSocketException" /> class.</summary>
		/// <param name="nativeError">The native error code for the exception.</param>
		/// <param name="message">The description of the error.</param>
		// Token: 0x06004344 RID: 17220 RVA: 0x000EA385 File Offset: 0x000E8585
		public WebSocketException(int nativeError, string message)
			: base(nativeError, message)
		{
			this._webSocketErrorCode = ((!WebSocketException.Succeeded(nativeError)) ? WebSocketError.NativeError : WebSocketError.Success);
			this.SetErrorCodeOnError(nativeError);
		}

		/// <summary>Creates an instance of the <see cref="T:System.Net.WebSockets.WebSocketException" /> class.</summary>
		/// <param name="nativeError">The native error code for the exception.</param>
		/// <param name="innerException">Indicates the previous exception that led to the current exception.</param>
		// Token: 0x06004345 RID: 17221 RVA: 0x000EA3A8 File Offset: 0x000E85A8
		public WebSocketException(int nativeError, Exception innerException)
			: base("An internal WebSocket error occurred. Please see the innerException, if present, for more details.", innerException)
		{
			this._webSocketErrorCode = ((!WebSocketException.Succeeded(nativeError)) ? WebSocketError.NativeError : WebSocketError.Success);
			this.SetErrorCodeOnError(nativeError);
		}

		/// <summary>Creates an instance of the <see cref="T:System.Net.WebSockets.WebSocketException" /> class.</summary>
		/// <param name="error">The error from the WebSocketError enumeration.</param>
		/// <param name="nativeError">The native error code for the exception.</param>
		// Token: 0x06004346 RID: 17222 RVA: 0x000EA3CF File Offset: 0x000E85CF
		public WebSocketException(WebSocketError error, int nativeError)
			: this(error, nativeError, WebSocketException.GetErrorMessage(error))
		{
		}

		/// <summary>Creates an instance of the <see cref="T:System.Net.WebSockets.WebSocketException" /> class.</summary>
		/// <param name="error">The error from the WebSocketError enumeration.</param>
		/// <param name="nativeError">The native error code for the exception.</param>
		/// <param name="message">The description of the error.</param>
		// Token: 0x06004347 RID: 17223 RVA: 0x000EA3DF File Offset: 0x000E85DF
		public WebSocketException(WebSocketError error, int nativeError, string message)
			: base(message)
		{
			this._webSocketErrorCode = error;
			this.SetErrorCodeOnError(nativeError);
		}

		/// <summary>Creates an instance of the <see cref="T:System.Net.WebSockets.WebSocketException" /> class.</summary>
		/// <param name="error">The error from the WebSocketError enumeration.</param>
		/// <param name="nativeError">The native error code for the exception.</param>
		/// <param name="innerException">Indicates the previous exception that led to the current exception.</param>
		// Token: 0x06004348 RID: 17224 RVA: 0x000EA3F6 File Offset: 0x000E85F6
		public WebSocketException(WebSocketError error, int nativeError, Exception innerException)
			: this(error, nativeError, WebSocketException.GetErrorMessage(error), innerException)
		{
		}

		/// <summary>Creates an instance of the <see cref="T:System.Net.WebSockets.WebSocketException" /> class.</summary>
		/// <param name="error">The error from the WebSocketError enumeration.</param>
		/// <param name="nativeError">The native error code for the exception.</param>
		/// <param name="message">The description of the error.</param>
		/// <param name="innerException">Indicates the previous exception that led to the current exception.</param>
		// Token: 0x06004349 RID: 17225 RVA: 0x000EA407 File Offset: 0x000E8607
		public WebSocketException(WebSocketError error, int nativeError, string message, Exception innerException)
			: base(message, innerException)
		{
			this._webSocketErrorCode = error;
			this.SetErrorCodeOnError(nativeError);
		}

		/// <summary>Creates an instance of the <see cref="T:System.Net.WebSockets.WebSocketException" /> class.</summary>
		/// <param name="message">The description of the error.</param>
		// Token: 0x0600434A RID: 17226 RVA: 0x000EA420 File Offset: 0x000E8620
		public WebSocketException(string message)
			: base(message)
		{
		}

		/// <summary>Creates an instance of the <see cref="T:System.Net.WebSockets.WebSocketException" /> class.</summary>
		/// <param name="message">The description of the error.</param>
		/// <param name="innerException">Indicates the previous exception that led to the current exception.</param>
		// Token: 0x0600434B RID: 17227 RVA: 0x000EA429 File Offset: 0x000E8629
		public WebSocketException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x0600434C RID: 17228 RVA: 0x000A5CA4 File Offset: 0x000A3EA4
		private WebSocketException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		/// <summary>Sets the SerializationInfo object with the file name and line number where the exception occurred.</summary>
		/// <param name="info">A SerializationInfo object.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		// Token: 0x0600434D RID: 17229 RVA: 0x000EA433 File Offset: 0x000E8633
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("WebSocketErrorCode", this._webSocketErrorCode);
		}

		/// <summary>The native error code for the exception that occurred.</summary>
		/// <returns>Returns <see cref="T:System.Int32" />.</returns>
		// Token: 0x17000F1D RID: 3869
		// (get) Token: 0x0600434E RID: 17230 RVA: 0x000A5CAE File Offset: 0x000A3EAE
		public override int ErrorCode
		{
			get
			{
				return base.NativeErrorCode;
			}
		}

		/// <summary>Returns a WebSocketError indicating the type of error that occurred.</summary>
		/// <returns>Returns <see cref="T:System.Net.WebSockets.WebSocketError" />.</returns>
		// Token: 0x17000F1E RID: 3870
		// (get) Token: 0x0600434F RID: 17231 RVA: 0x000EA453 File Offset: 0x000E8653
		public WebSocketError WebSocketErrorCode
		{
			get
			{
				return this._webSocketErrorCode;
			}
		}

		// Token: 0x06004350 RID: 17232 RVA: 0x000EA45C File Offset: 0x000E865C
		private static string GetErrorMessage(WebSocketError error)
		{
			switch (error)
			{
			case WebSocketError.InvalidMessageType:
				return SR.Format("The received  message type is invalid after calling {0}. {0} should only be used if no more data is expected from the remote endpoint. Use '{1}' instead to keep being able to receive data but close the output channel.", "WebSocket.CloseAsync", "WebSocket.CloseOutputAsync");
			case WebSocketError.Faulted:
				return "An exception caused the WebSocket to enter the Aborted state. Please see the InnerException, if present, for more details.";
			case WebSocketError.NotAWebSocket:
				return "A WebSocket operation was called on a request or response that is not a WebSocket.";
			case WebSocketError.UnsupportedVersion:
				return "Unsupported WebSocket version.";
			case WebSocketError.UnsupportedProtocol:
				return "The WebSocket request or response operation was called with unsupported protocol(s).";
			case WebSocketError.HeaderError:
				return "The WebSocket request or response contained unsupported header(s).";
			case WebSocketError.ConnectionClosedPrematurely:
				return "The remote party closed the WebSocket connection without completing the close handshake.";
			case WebSocketError.InvalidState:
				return "The WebSocket instance cannot be used for communication because it has been transitioned into an invalid state.";
			}
			return "An internal WebSocket error occurred. Please see the innerException, if present, for more details.";
		}

		// Token: 0x06004351 RID: 17233 RVA: 0x000EA4DB File Offset: 0x000E86DB
		private void SetErrorCodeOnError(int nativeError)
		{
			if (!WebSocketException.Succeeded(nativeError))
			{
				base.HResult = nativeError;
			}
		}

		// Token: 0x06004352 RID: 17234 RVA: 0x000EA4EC File Offset: 0x000E86EC
		private static bool Succeeded(int hr)
		{
			return hr >= 0;
		}

		// Token: 0x040028B4 RID: 10420
		private readonly WebSocketError _webSocketErrorCode;
	}
}
