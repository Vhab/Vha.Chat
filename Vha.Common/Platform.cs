using System;
using System.Collections.Generic;
using System.Text;

namespace Vha.Common
{
    public enum Runtime
    {
        DotNet,
        Mono
    }

    public enum OS
    {
        /// <summary>
        /// Microsoft Windows
        /// </summary>
        Windows,
        /// <summary>
        /// Unix or a variation of it (including Linux, FreeBSD, etc)
        /// </summary>
        Unix,
        /// <summary>
        /// Apple MacOS
        /// </summary>
        MacOS,
        /// <summary>
        /// Running on an unknown other platform
        /// </summary>
        Unknown
    }

    public static class Platform
    {
        public static Runtime Runtime
        {
            get
            {
                if (!_initialized) _detect();
                return _runtime;
            }
        }

        public static OS OS
        {
            get
            {
                if (!_initialized) _detect();
                return _os;
            }
        }

        #region Internal
        private static void _detect()
        {
            // Detect platform
                _runtime = Runtime.DotNet;
            if (Type.GetType("Mono.Runtime") != null)
                _runtime = Runtime.Mono;

            // Detect OS
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX:
                        _os = OS.MacOS;
                    break;
                case PlatformID.Unix:
                        _os = OS.Unix;
                    break;
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                        _os = OS.Windows;
                    break;
                default:
                        _os = OS.Unknown;
                    break;
            }

            _initialized = true;
        }

        private static bool _initialized = false;
        private static Runtime _runtime;
        private static OS _os;
        #endregion
    }
}
