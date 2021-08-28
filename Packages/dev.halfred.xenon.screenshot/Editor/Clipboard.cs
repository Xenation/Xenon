using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

using UnityEngine;

namespace Xenon.Screenshot {
	public class Clipboard {

		public static void SetText(string text) {
			if (!WinSetText(text)) {
				Debug.LogWarning("Could not set clipboard text!");
			}
		}

		public static string GetText() {
			return WinGetText();
		}

		public static void SetImage(byte[] imgData) {
			if (!WinSetData(WinClipboardFormat.DIB, imgData)) {
				Debug.LogWarning("Could not set clipboard bitmap!");
			}
		}

		public static byte[] GetImage() {
			return null;
		}

		#region Windows
		private enum WinClipboardFormat : uint {
			TEXT = 1,
			BITMAP = 2,
			METAFILEPICT = 3,
			SYLK = 4,
			DIF = 5,
			TIFF = 6,
			OEMTEXT = 7,
			DIB = 8,
			PALETTE = 9,
			PENDATA = 10,
			RIFF = 11,
			WAVE = 12,
			UNICODETEXT = 13,
			ENHMETAFILE = 14,
			HDROP = 15,
			LOCALE = 16,
			DIBV5 = 17,

			OWNERDISPLAY = 0x0080,
			DSPTEXT = 0x0081,
			DSPBITMAP = 0x0082,
			DSPMETAFILEPICT = 0x0083,
			DSPENHMETAFILE = 0x008E,

			PRIVATEFIRST = 0x0200,
			PRIVATELAST = 0x02FF,

			GDIOBJFIRST = 0x0300,
			GDIOBJLAST = 0x03FF,
		}

		private static bool WinSetText(string text) {
			byte[] strData = new byte[text.Length * 2 + 2]; // Unicode is 2 bytes per char, we allocate one more char for a null terminated string
			Encoding.Unicode.GetBytes(text, 0, text.Length, strData, 0);
			return WinSetData(WinClipboardFormat.UNICODETEXT, strData);
		}

		private static bool WinSetData(WinClipboardFormat format, byte[] data) {
			if (!OpenClipboard(IntPtr.Zero)) {
				return false;
			}
			EmptyClipboard();

			IntPtr hgData = Marshal.AllocHGlobal(data.Length);
			if (hgData == IntPtr.Zero) {
				return false;
			}

			IntPtr ptData = GlobalLock(hgData);
			if (ptData == IntPtr.Zero) {
				Marshal.FreeHGlobal(hgData);
				return false;
			}
			Marshal.Copy(data, 0, ptData, data.Length);
			GlobalUnlock(hgData);

			if (SetClipboardData((uint) format, ptData) == IntPtr.Zero) {
				Marshal.FreeHGlobal(hgData);
				return false;
			}

			CloseClipboard();

			return true;
		}

		private static string WinGetText() {
			return Encoding.Unicode.GetString(WinGetData(WinClipboardFormat.UNICODETEXT)).TrimEnd('\0');
		}

		private static byte[] WinGetData(WinClipboardFormat format) {
			if (!IsClipboardFormatAvailable((uint) format)) {
				return null;
			}

			if (!OpenClipboard(IntPtr.Zero)) {
				return null;
			}

			IntPtr hgData = GetClipboardData((uint) format);
			if (hgData == IntPtr.Zero) {
				return null;
			}

			IntPtr ptData = GlobalLock(hgData);
			if (ptData == IntPtr.Zero) {
				return null;
			}

			int size = GlobalSize(hgData);
			byte[] data = new byte[size];

			Marshal.Copy(ptData, data, 0, size);

			GlobalUnlock(hgData);
			CloseClipboard();

			return data;
		}

		private static void ThrowWin32() {
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}

		[DllImport("Kernel32.dll", SetLastError = true)]
		private extern static int GlobalSize(IntPtr hMem);

		[DllImport("kernel32.dll", SetLastError = true)]
		private extern static IntPtr GlobalLock(IntPtr hMem);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private extern static bool GlobalUnlock(IntPtr hMem);

		[DllImport("User32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private extern static bool IsClipboardFormatAvailable(uint format);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private extern static bool OpenClipboard(IntPtr hWndNewOwner);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private extern static bool CloseClipboard();

		[DllImport("user32.dll", SetLastError = true)]
		private extern static IntPtr SetClipboardData(uint uFormat, IntPtr data);

		[DllImport("user32.dll", SetLastError = true)]
		private extern static IntPtr GetClipboardData(uint uFormat);

		[DllImport("user32.dll")]
		private extern static bool EmptyClipboard();
		#endregion
	}
}
