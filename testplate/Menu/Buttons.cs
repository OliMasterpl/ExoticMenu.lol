using StupidTemplate.Classes;
using StupidTemplate.Mods;
using UnityEngine;
using static StupidTemplate.Settings;

namespace StupidTemplate.Menu
{
    internal class Buttons
    {
        public static ButtonInfo[][] buttons = new ButtonInfo[][]
        {
            new ButtonInfo[] { // Main Mods
                new ButtonInfo { buttonText = "Settings", method =() => SettingsMods.EnterSettings(), isTogglable = false, toolTip = "Opens the main settings page for the menu."},
                new ButtonInfo { buttonText = "Platforms", method = () => Global.Platforms(), toolTip="Skibidi Platforms"},
                new ButtonInfo { buttonText = "Wall walk [RG]", method = () => Global.WallWalk(), toolTip="WAll walk [Right grip]"},
                new ButtonInfo { buttonText = "Moon Gravity", method = () => Global.MoonGravity(), toolTip="yes"},
                new ButtonInfo { buttonText = "Noclip [rt]", method = () => Global.Noclip(), toolTip="yes"},
                new ButtonInfo { buttonText = "Spin Head [x]", method = () => Global.SpinHeadX(), toolTip="yes"},
                new ButtonInfo { buttonText = "Spin Head [y]", method = () => Global.SpinHeadY(), toolTip="yes"},
                new ButtonInfo { buttonText = "Spin Head [z]", method = () => Global.SpinHeadZ(), toolTip="yes"},
                new ButtonInfo { buttonText = "Fix Head", method = () => Global.FixHead(), toolTip="yes"},
                new ButtonInfo { buttonText = "Fly", method = () => Global.Fly(), toolTip="yes"},
                new ButtonInfo { buttonText = "No Finger Movement", method = () => Global.NoFinger(), toolTip="yes"},
                new ButtonInfo { buttonText = "No Tag On Join", method = () => Global.NoTagOnJoin(),disableMethod=()=>Global.TagOnJoin(), toolTip="yes"},
                new ButtonInfo { buttonText = "LongArms", method = () =>  GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f), disableMethod=()=>GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(1f, 1f, 1f), toolTip="yes"},
                new ButtonInfo { buttonText = "Ghost Monk", method = () =>  Global.Ghost(), disableMethod=()=>Global.EnableRig(), toolTip="yes"},
                new ButtonInfo { buttonText = "Invis Monk", method = () =>  Global.Invisible(), disableMethod=()=>Global.DisableInvisible(), toolTip="yes"},
                new ButtonInfo { buttonText = "PBBV Walk", method = () =>  GorillaLocomotion.Player.Instance.disableMovement = true, disableMethod=()=>GorillaLocomotion.Player.Instance.disableMovement = false, toolTip="yes"},
                new ButtonInfo { buttonText = "Helicopter Monke", method = () =>  Global.Helicopter(), toolTip="yes"},
                new ButtonInfo { buttonText = "Destroy gun", method = () =>  Global.DestroyGun(), toolTip="yes"},
                new ButtonInfo { buttonText = "Destroy all", method = () =>  Global.DestroyAll(), toolTip="yes"},
                new ButtonInfo { buttonText = "RGB [<color=red>STUMP</color>]", method = () =>  Global.RGB(), toolTip="yes"},
            },

            new ButtonInfo[] { // Settings
                new ButtonInfo { buttonText = "Return to Main", method =() => Global.ReturnHome(), isTogglable = false, toolTip = "Returns to the main page of the menu."},
                new ButtonInfo { buttonText = "Menu", method =() => SettingsMods.MenuSettings(), isTogglable = false, toolTip = "Opens the settings for the menu."},
            },

            new ButtonInfo[] { // Menu Settings
                new ButtonInfo { buttonText = "Return to Settings", method =() => SettingsMods.EnterSettings(), isTogglable = false, toolTip = "Returns to the main settings page for the menu."},
                new ButtonInfo { buttonText = "Right Hand", enableMethod =() => SettingsMods.RightHand(), disableMethod =() => SettingsMods.LeftHand(), toolTip = "Puts the menu on your right hand."},
                new ButtonInfo { buttonText = "Notifications", enableMethod =() => SettingsMods.EnableNotifications(), disableMethod =() => SettingsMods.DisableNotifications(), enabled = !disableNotifications, toolTip = "Toggles the notifications."},
                new ButtonInfo { buttonText = "FPS Counter", enableMethod =() => SettingsMods.EnableFPSCounter(), disableMethod =() => SettingsMods.DisableFPSCounter(), enabled = fpsCounter, toolTip = "Toggles the FPS counter."},
                new ButtonInfo { buttonText = "Disconnect Button", enableMethod =() => SettingsMods.EnableDisconnectButton(), disableMethod =() => SettingsMods.DisableDisconnectButton(), enabled = disconnectButton, toolTip = "Toggles the disconnect button."},
            },

            new ButtonInfo[] { // Movement Settings
                new ButtonInfo { buttonText = "Return to Settings", method =() => SettingsMods.EnterSettings(), isTogglable = false, toolTip = "Returns to the main settings page for the menu."},
            },

            new ButtonInfo[] { // Projectile Settings
                new ButtonInfo { buttonText = "Return to Settings", method =() => SettingsMods.MenuSettings(), isTogglable = false, toolTip = "Opens the settings for the menu."},
            },
        };
    }
}
