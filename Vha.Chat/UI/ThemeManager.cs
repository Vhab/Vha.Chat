
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Vha.Chat.UI
{
    internal static class ThemeManager
    {
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_OLD = 19;

        private static readonly Color DarkBackground =
            Color.FromArgb(32, 33, 36);

        private static readonly Color DarkSurface =
            Color.FromArgb(42, 44, 48);

        private static readonly Color DarkForeground =
            Color.FromArgb(225, 228, 232);

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(
            IntPtr hwnd,
            int attribute,
            ref int value,
            int valueSize);

        public static void ApplyTitleBar(Form form, bool dark)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                return;

            // The native title bar needs an existing window handle.
            if (!form.IsHandleCreated)
                return;

            int enabled = dark ? 1 : 0;
            int size = sizeof(int);

            try
            {
                int result = DwmSetWindowAttribute(
                    form.Handle,
                    DWMWA_USE_IMMERSIVE_DARK_MODE,
                    ref enabled,
                    size);

                // Compatibility with some older Windows 10 builds.
                if (result != 0)
                {
                    DwmSetWindowAttribute(
                        form.Handle,
                        DWMWA_USE_IMMERSIVE_DARK_MODE_OLD,
                        ref enabled,
                        size);
                }
            }
            catch (DllNotFoundException)
            {
                // Unsupported Windows version.
            }
            catch (EntryPointNotFoundException)
            {
                // DWM API unavailable.
            }
        }

        public static void ApplyControls(Control parent, bool dark)
        {
            if (dark)
            {
                parent.BackColor = DarkBackground;
                parent.ForeColor = DarkForeground;
            }
            else
            {
                parent.ResetBackColor();
                parent.ResetForeColor();
            }

            foreach (Control control in parent.Controls)
            {
                if (control is PictureBox)
                {
                    // Preserve existing images.
                    continue;
                }

                if (dark)
                {
                    if (control is TextBox ||
                        control is ComboBox ||
                        control is ListBox ||
                        control is NumericUpDown)
                    {
                        control.BackColor = DarkSurface;
                        control.ForeColor = DarkForeground;
                    }
                    else
                    {
                        control.BackColor = DarkBackground;
                        control.ForeColor = DarkForeground;
                    }
                }
                else
                {
                    control.ResetBackColor();
                    control.ResetForeColor();
                }

                // Apply to controls inside group boxes and panels.
                if (control.HasChildren)
                    ApplyControls(control, dark);
            }
        }

        public static void ConfigureComboBox(ComboBox combo, bool dark)
        {
            combo.DrawItem -= DrawComboBoxItem;

            if (!dark || combo.DropDownStyle != ComboBoxStyle.DropDownList)
            {
                combo.DrawMode = DrawMode.Normal;
                return;
            }

            combo.DrawMode = DrawMode.OwnerDrawFixed;
            combo.DrawItem += DrawComboBoxItem;
        }

        private static void DrawComboBoxItem(
            object sender,
            DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            ComboBox combo = (ComboBox)sender;

            using (SolidBrush brush = new SolidBrush(DarkSurface))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            TextRenderer.DrawText(
                e.Graphics,
                combo.GetItemText(combo.Items[e.Index]),
                combo.Font,
                e.Bounds,
                DarkForeground,
                TextFormatFlags.Left |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis);

            e.DrawFocusRectangle();
        }
    }
}
