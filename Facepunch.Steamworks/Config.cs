﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Facepunch.Steamworks
{
    public static class Config
    {
        /// <summary>
        /// Some platforms allow/need CallingConvention.ThisCall. If you're crashing with argument null
        /// errors on certain platforms, try flipping this to true.
        /// 
        /// I owe this logic to Riley Labrecque's hard work on Steamworks.net - I don't have the knowledge
        /// or patience to find this shit on my own, so massive thanks to him. And also massive thanks to him
        /// for releasing his shit open source under the MIT license so we can all learn and iterate.
        /// 
        /// </summary>
        public static bool UseThisCall { get; set; } = true;


        /// <summary>
        /// The Native dll to look for. This is the steam_api.dll renamed.
        /// We need to rename the dll anyway because we can't dynamically choose the library
        /// ie, we can't load steam_api64.dll on windows 64 platforms. So instead we choose to
        /// keep the library name the same.
        /// 
        /// This is exposed only for the benefit of implementation - and cannot be changed at runtime.
        /// </summary>
        public const string LibraryName = "FacepunchSteamworksApi";
    }
}
