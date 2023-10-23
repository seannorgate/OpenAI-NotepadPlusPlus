using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Kbg.NppPluginNET.PluginInfrastructure;

namespace Kbg.NppPluginNET
{
    class Main
    {
        internal const string PluginName = "SeanCopilot";
        public static ConfigManager configManager = null;
        static string iniFilePath = null;
        static bool someSetting = false;
        static frmCopilotDlg copilotDlg = null;
        static frmSettings frmSettings = null;
        static int idCopilotDlg = -1;
        static int idSettingsDlg = -1;
        static Bitmap tbBmp = SeanCopilot.Properties.Resources.star;
        static Bitmap tbBmp_tbTab = SeanCopilot.Properties.Resources.star_bmp;
        static Icon tbIcon = null;
        private static string sAIInstructions = null;


        public static void OnNotification(ScNotification notification)
        {  
            // This method is invoked whenever something is happening in notepad++
            // use eg. as
            // if (notification.Header.Code == (uint)NppMsg.NPPN_xxx)
            // { ... }
            // or
            //
            // if (notification.Header.Code == (uint)SciMsg.SCNxxx)
            // { ... }
        }

        internal static void CommandMenuInit()
        {
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            GetConfigurations();

            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            iniFilePath = sbIniFilePath.ToString();
            if (!Directory.Exists(iniFilePath)) Directory.CreateDirectory(iniFilePath);
            iniFilePath = Path.Combine(iniFilePath, PluginName + ".ini");
            someSetting = (Win32.GetPrivateProfileInt("SomeSection", "SomeKey", 0, iniFilePath) != 0);

            PluginBase.SetCommand(0, "Sean's Menu Command", myMenuFunction);
            PluginBase.SetCommand(1, "Open Copilot", dockedCopilot, new ShortcutKey(true, true, false, Keys.C)); idCopilotDlg = 1;
            PluginBase.SetCommand(2, "Copilot Settings", SettingsDialog); idSettingsDlg = 2;
        }

        private static void GetConfigurations()
        {
            configManager = new ConfigManager(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"plugins/{PluginName}/{PluginName}.config"));
        }

        public static string GetInstructions()
        {
            if (sAIInstructions == null)
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configManager.GetConfigValue("instructions_path"));
                if (File.Exists(filePath))
                {
                    try
                    {
                        sAIInstructions = File.ReadAllText(filePath);
                    }
                    catch (Exception ex)
                    {
                        sAIInstructions = $"An error occurred while reading the file: {ex.Message}";
                    }
                }
                else
                {
                    sAIInstructions = $"File {filePath} does not exist.";
                }
            }

            return sAIInstructions;
        }

        public static void SetInstructions(string newInstruction)
        {
            try
            {
                File.WriteAllText(Main.configManager.GetConfigValue("instructions_path"), newInstruction);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the file: {ex.Message}");
            }

            sAIInstructions = null;
            GetInstructions();
        }

        internal static void SetToolBarIcon()
        {
            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = tbBmp.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_ADDTOOLBARICON, PluginBase._funcItems.Items[idCopilotDlg]._cmdID, pTbIcons);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_ADDTOOLBARICON, PluginBase._funcItems.Items[idSettingsDlg]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }

        internal static void PluginCleanUp()
        {
            Win32.WritePrivateProfileString("SomeSection", "SomeKey", someSetting ? "1" : "0", iniFilePath);
        }


        internal static void myMenuFunction()
        {
            var scintilla = new ScintillaGateway(PluginBase.GetCurrentScintilla());
            if (scintilla.GetSelectionLength() != 0)
                MessageBox.Show("Hello Sean, you are super cool! model is " + configManager.GetConfigValue("gpt_model"));
            else
                MessageBox.Show(scintilla.GetSelText());
        }

        internal static void dockedCopilot()
        {
            if (copilotDlg == null)
            {
                copilotDlg = new frmCopilotDlg();

                using (Bitmap newBmp = new Bitmap(16, 16))
                {
                    Graphics g = Graphics.FromImage(newBmp);
                    ColorMap[] colorMap = new ColorMap[1];
                    colorMap[0] = new ColorMap();
                    colorMap[0].OldColor = Color.Fuchsia;
                    colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetRemapTable(colorMap);
                    g.DrawImage(tbBmp_tbTab, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
                    tbIcon = Icon.FromHandle(newBmp.GetHicon());
                }

                NppTbData _nppTbData = new NppTbData();
                _nppTbData.hClient = copilotDlg.Handle;
                _nppTbData.pszName = "Copilot";
                _nppTbData.dlgID = idCopilotDlg;
                _nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
                _nppTbData.hIconTab = (uint)tbIcon.Handle;
                _nppTbData.pszModuleName = PluginName;
                IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
                Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
            }
            else
            {
                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DMMSHOW, 0, copilotDlg.Handle);
            }
        }

        internal static void SettingsDialog()
        {
            if (frmSettings == null)
            {
                frmSettings = new frmSettings();

                using (Bitmap newBmp = new Bitmap(16, 16))
                {
                    Graphics g = Graphics.FromImage(newBmp);
                    ColorMap[] colorMap = new ColorMap[1];
                    colorMap[0] = new ColorMap();
                    colorMap[0].OldColor = Color.Fuchsia;
                    colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetRemapTable(colorMap);
                    g.DrawImage(tbBmp_tbTab, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
                    tbIcon = Icon.FromHandle(newBmp.GetHicon());
                }

                NppTbData _nppTbData = new NppTbData();
                _nppTbData.hClient = frmSettings.Handle;
                _nppTbData.pszName = "Copilot Settings";
                _nppTbData.dlgID = idSettingsDlg;
                _nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
                _nppTbData.hIconTab = (uint)tbIcon.Handle;
                _nppTbData.pszModuleName = PluginName;
                IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
                Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
            }
            else
            {
                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DMMSHOW, 0, frmSettings.Handle);
            }
        }
    }
}