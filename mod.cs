using ICities;
using ColossalFramework.UI;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;


namespace AssetPropToolBar
{
    public class AssetPropToolBar : LoadingExtensionBase, IUserMod
    {
        private Dictionary<MethodInfo, RedirectCallsState> redirects = new Dictionary<MethodInfo, RedirectCallsState>();
        
        public string Name
        {
            get { return "Asset Prop ToolBar"; }
        }

        public string Description
        {
            get { return "Simple mod that adds all prop menu in the intersection editor"; }
        }

        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);

            var allFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
            var method = typeof(AssetEditorMainToolbar).GetMethod("RefreshForIntersection", allFlags);
            redirects.Add(method, RedirectionHelper.RedirectCalls(method, typeof(Tool).GetMethod("RefreshForIntersection", allFlags)));
        }

        public override void OnReleased()
        {
            base.OnReleased();

            foreach (var kvp in redirects)
            {

                RedirectionHelper.RevertRedirect(kvp.Key, kvp.Value);
            }
            redirects.Clear();
        }
    }

    public class Tool : MainToolbar
    {
        private void RefreshForIntersection()
        {
            UITabstrip uITabstrip = ToolsModifierControl.mainToolbar.component as UITabstrip;
            uITabstrip.selectedIndex = -1;
            base.RefreshPanel();
            base.SpawnSubEntry(uITabstrip, "Roads", "MAIN_TOOL", null, "ToolbarIcon", true);
            base.SpawnSubEntry(uITabstrip, "Beautification", "MAIN_TOOL", null, "ToolbarIcon", true);
            base.SpawnSeparator(uITabstrip);
            this.AddMainToolbarProps(uITabstrip);
            base.SpawnSeparator(uITabstrip);
            base.SpawnSubEntry(uITabstrip, "AssetEditorSettings", "DECORATIONEDITOR_TOOL", null, "ToolbarIcon", true);
        }

        private void AddMainToolbarProps(UITabstrip strip)
        {
            base.SpawnSubEntry(strip, "PropsBillboards", "DECORATIONEDITOR_TOOL", null, "ToolbarIcon", true);
            base.SpawnSubEntry(strip, "PropsSpecialBillboards", "DECORATIONEDITOR_TOOL", null, "ToolbarIcon", true);
            base.SpawnSubEntry(strip, "PropsIndustrial", "DECORATIONEDITOR_TOOL", null, "ToolbarIcon", true);
            base.SpawnSubEntry(strip, "PropsParks", "DECORATIONEDITOR_TOOL", null, "ToolbarIcon", true);
            base.SpawnSubEntry(strip, "PropsCommon", "DECORATIONEDITOR_TOOL", null, "ToolbarIcon", true);
            base.SpawnSubEntry(strip, "PropsResidential", "DECORATIONEDITOR_TOOL", null, "ToolbarIcon", true);
            base.SpawnSubEntry(strip, "PropsMarkers", "DECORATIONEDITOR_TOOL", null, "ToolbarIcon", true);
        }
    }
}
