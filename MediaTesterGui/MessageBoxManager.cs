#pragma warning disable 0618
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

[assembly: SecurityPermission(SecurityAction.RequestMinimum, UnmanagedCode = true)]
namespace System.Windows.Forms
{
	// Code from: https://www.codeproject.com/script/Articles/ViewDownloads.aspx?aid=18399
	public class MessageBoxManager
	{
		private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
		private delegate bool EnumChildProc(IntPtr hWnd, IntPtr lParam);

		private const int WH_CALLWNDPROCRET = 12;
		private const int WM_INITDIALOG = 0x0110;

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

		[DllImport("user32.dll")]
		private static extern int UnhookWindowsHookEx(IntPtr idHook);

		[DllImport("user32.dll")]
		private static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "GetWindowTextLengthW", CharSet = CharSet.Unicode)]
		private static extern int GetWindowTextLength(IntPtr hWnd);

		[DllImport("user32.dll", EntryPoint = "GetWindowTextW", CharSet = CharSet.Unicode)]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength);

		[DllImport("user32.dll")]
		private static extern int EndDialog(IntPtr hDlg, IntPtr nResult);

		[DllImport("user32.dll")]
		private static extern bool EnumChildWindows(IntPtr hWndParent, EnumChildProc lpEnumFunc, IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "GetClassNameW", CharSet = CharSet.Unicode)]
		private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

		[DllImport("user32.dll")]
		private static extern int GetDlgCtrlID(IntPtr hwndCtl);

		[DllImport("user32.dll")]
		private static extern IntPtr GetDlgItem(IntPtr hDlg, int nIDDlgItem);

		[DllImport("user32.dll", EntryPoint = "SetWindowTextW", CharSet = CharSet.Unicode)]
		private static extern bool SetWindowText(IntPtr hWnd, string lpString);


		[StructLayout(LayoutKind.Sequential)]
		public struct CWPRETSTRUCT
		{
			public IntPtr lResult;
			public IntPtr lParam;
			public IntPtr wParam;
			public uint message;
			public IntPtr hwnd;
		};

		private static HookProc _hookProc;
		private static EnumChildProc _enumProc;
		[ThreadStatic]
		private static IntPtr _hHook;
		[ThreadStatic]
		private static int _nButton;
		[ThreadStatic]
		private static int _extraButtons;
		
		/// <summary>
		/// OK text
		/// </summary>
		public static string OK = "&OK";
		/// <summary>
		/// Cancel text
		/// </summary>
		public static string Cancel = "&Cancel";
		/// <summary>
		/// Abort text
		/// </summary>
		public static string Abort = "&Abort";
		/// <summary>
		/// Retry text
		/// </summary>
		public static string Retry = "&Retry";
		/// <summary>
		/// Ignore text
		/// </summary>
		public static string Ignore = "&Ignore";
		/// <summary>
		/// Yes text
		/// </summary>
		public static string Yes = "&Yes";
		/// <summary>
		/// No text
		/// </summary>
		public static string No = "&No";
		/// <summary>
		/// Help text
		/// </summary>
		public static string Help = "&Help";

		static MessageBoxManager()
		{
			_hookProc = new HookProc(MessageBoxHookProc);
			_enumProc = new EnumChildProc(MessageBoxEnumProc);
			_hHook = IntPtr.Zero;
		}

		/// <summary>
		/// Enables MessageBoxManager functionality
		/// </summary>
		/// <remarks>
		/// MessageBoxManager functionality is enabled on current thread only.
		/// Each thread that needs MessageBoxManager functionality has to call this method.
		/// </remarks>
		public static void Register()
		{
			if (_hHook != IntPtr.Zero)
				throw new NotSupportedException("One hook per thread allowed.");
			_hHook = SetWindowsHookEx(WH_CALLWNDPROCRET, _hookProc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
		}

		/// <summary>
		/// Disables MessageBoxManager functionality
		/// </summary>
		/// <remarks>
		/// Disables MessageBoxManager functionality on current thread only.
		/// </remarks>
		public static void Unregister()
		{
			if (_hHook != IntPtr.Zero)
			{
				UnhookWindowsHookEx(_hHook);
				_hHook = IntPtr.Zero;
			}
		}

		private static IntPtr MessageBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode < 0)
				return CallNextHookEx(_hHook, nCode, wParam, lParam);

			CWPRETSTRUCT msg = (CWPRETSTRUCT) Marshal.PtrToStructure(lParam, typeof(CWPRETSTRUCT));
			IntPtr hook = _hHook;

			if (msg.message == WM_INITDIALOG)
			{
				int nLength = GetWindowTextLength(msg.hwnd);
				StringBuilder className = new StringBuilder(10);
				GetClassName(msg.hwnd, className, className.Capacity);
				if (className.ToString() == "#32770")
				{
					_extraButtons = 0;
					_nButton = 0;
					EnumChildWindows(msg.hwnd, _enumProc, IntPtr.Zero);
					if (_nButton == (int) DialogResult.OK + _extraButtons)
					{
						// Special handling for stand along OK button
						IntPtr hButton = GetDlgItem(msg.hwnd, (int) DialogResult.Cancel);
						if (hButton != IntPtr.Zero)
							SetWindowText(hButton, OK);
					}
				}
			}

			return CallNextHookEx(hook, nCode, wParam, lParam);
		}

		private static bool MessageBoxEnumProc(IntPtr hWnd, IntPtr lParam)
		{
			StringBuilder className = new StringBuilder(10);
			GetClassName(hWnd, className, className.Capacity);
			if (className.ToString() == "Button")
			{
				int ctlId = GetDlgCtrlID(hWnd);
				switch ((DialogResult) ctlId)
				{
					case DialogResult.OK:
						SetWindowText(hWnd, OK);
						break;
					case DialogResult.Cancel:
						SetWindowText(hWnd, Cancel);
						break;
					case DialogResult.Abort:
						SetWindowText(hWnd, Abort);
						break;
					case DialogResult.Retry:
						SetWindowText(hWnd, Retry);
						break;
					case DialogResult.Ignore:
						SetWindowText(hWnd, Ignore);
						break;
					case DialogResult.Yes:
						SetWindowText(hWnd, Yes);
						break;
					case DialogResult.No:
						SetWindowText(hWnd, No);
						break;
					case (DialogResult) 9: // Help
						SetWindowText(hWnd, Help);
						_extraButtons++;
						break;

				}
				_nButton++;
			}

			return true;
		}
	}
}
