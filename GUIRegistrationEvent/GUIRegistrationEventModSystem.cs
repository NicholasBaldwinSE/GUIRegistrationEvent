using Vintagestory.API.Common;
using HarmonyLib;
using Vintagestory.API.Client;
using System;
using System.Runtime.CompilerServices;

namespace GUIRegistrationEvent
{
    public class GUIRegistrationEventModSystem : ModSystem
    {
        public ICoreClientAPI capi;
        public Harmony harmony;

        // Define the event.
        // Event has to be static to call it in the patch.
        // This can potentially cause leaks, which will have to be accounted for 
        // by end users. Not much I can think of to do about that, frankly.
        public delegate void GUIRegistrationEventDelegate(GuiDialog dialog);
        public static event GUIRegistrationEventDelegate GUIRegistrationEvent;

        public override double ExecuteOrder()
        {
            // This should run as early as posssible.
            return 0;
        }

        public override void StartClientSide(ICoreClientAPI capi)
        {
            this.capi = capi;

            // We get the original method this way since there's no (easy) way to 
            // get a copy of the instance class' method. Since there should only realistically
            // be one class here ever on the clientside, we can pretty much guarantee
            // this will be the method that is always called.
            var originalMethod = capi.Gui.GetType().GetMethod("RegisterDialog");

            // Patch our postfix.
            harmony = new Harmony("GUIRegistrationEvent");
            harmony.Patch(originalMethod, null, new HarmonyMethod(PostfixRegisterDialog));
        }

        public override void Dispose()
        {
            // Side check is probably redundant, but doesn't hurt.
            if (capi != null && capi.Side == EnumAppSide.Client)
            {
                harmony.UnpatchAll("GUIRegistrationEvent");
            }
        }

        [HarmonyPostfix]
        public static void PostfixRegisterDialog(GuiDialog[] dialogs)
        {
            // Invoke the event for every dialog registered.
            // We do this for each individually simply because it makes
            // getting a specific target dialog easier.
            foreach (GuiDialog dialog in dialogs)
            {
                GUIRegistrationEvent?.Invoke(dialog);
            }
        }
    }
}
