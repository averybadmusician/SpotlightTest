using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using Spotlight.Core.Memory;

[assembly: Rage.Attributes.Plugin("SpotlightTest",
    Description = "",
    Author = "BadMusician",
    EntryPoint = "SpotlightTest.Plugin.OnLoad",
    ExitPoint = "SpotlightTest.Plugin.OnUnload",
    PrefersSingleInstance = true,
    SupportUrl = "",
    ShouldTickInPauseMenu = true)]

namespace SpotlightTest
{
    public class Plugin
    {
        public static void OnLoad()
        {
            if (!(GameFunctions.Init() && GameMemory.Init() && GameOffsets.Init()))
            {
                Game.DisplayNotification($"~r~[ERROR] Spotlight: ~s~Failed to initialize, unloading...");
                Game.LogTrivial($"[ERROR] Failed to initialize, unloading...");
                Game.UnloadActivePlugin();
            }
            WinFunctions.CopyTlsValues(WinFunctions.GetProcessMainThreadId(), WinFunctions.GetCurrentThreadId(), GameOffsets.TlsAllocator0, GameOffsets.TlsAllocator1, GameOffsets.TlsAllocator2);

            bool on = false;
            Ped p = Game.LocalPlayer.Character;
            ulong shadowId = Render.GenerateShadowId();

            while (true)
            {
                GameFiber.Yield();
                if (Game.IsKeyDown(System.Windows.Forms.Keys.NumPad0)) on = !on;
                if (p && on)
                {
                    Game.DisplayHelp("Light: ~g~On");
                    Render.SpotLight(p.GetOffsetPositionFront(1), p.Direction,
                        eLightFlags.CanRenderUnderground | eLightFlags.EnableVolume | eLightFlags.DisableSpecular,
                        30, 45, 0.06f, 0.175f, 45, 5, 8.25f, System.Drawing.Color.Red, shadowId);
                }
            }
        }

        public static void OnUnload(bool isTerminating)
        {

        }
    }
}
