using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICities;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;
using CitiesHarmony.API;

namespace RemoveParking {
    public class RPModule : IUserMod, ILoadingExtension {
        private const string m_modName = @"Remove Parking";
        private const string m_modDesc = @"Allows you to remove those pesky parking spaces in building assets";
        internal const string m_version = @"0.1.0";
        internal const string m_modVersion = m_version + ".*";
        public string Name => m_modName + ' ' + m_version;
        public string Description => m_modDesc;

        internal delegate void SetButtonVisibilityHandler(bool visibility);
        internal static SetButtonVisibilityHandler SetZonedButtonVisibility;
        internal static SetButtonVisibilityHandler SetParkButtonVisibility;
        internal static SetButtonVisibilityHandler SetServicceButtonVisibility;

        public void OnEnabled() {
            HarmonyHelper.DoOnHarmonyReady(RPPatcher.EnablePatches);
        }

        public void OnDisabled() {
            if (HarmonyHelper.IsHarmonyInstalled) RPPatcher.DisablePatches();
        }

        public void OnCreated(ILoading loading) {
        }

        public void OnLevelLoaded(LoadMode mode) {
            RPPatcher.LateSetup();
            ZonedBuildingWorldInfoPanel infoPanel = UIView.library.Get<ZonedBuildingWorldInfoPanel>(typeof(ZonedBuildingWorldInfoPanel).Name);
            UIButton zonedBtn = AddButton(infoPanel.component, "Remove Parking");
            zonedBtn.Hide();
            zonedBtn.eventClicked += (_, isClicked) => {
                if (InstanceManager.GetPrefabInfo(WorldInfoPanel.GetCurrentInstanceID()) is BuildingInfo buildingInfo) {
                    buildingInfo.m_hasParkingSpaces &= ~VehicleInfo.VehicleType.Car;
                }
            };
            SetZonedButtonVisibility = (visibility) => zonedBtn.isVisible = visibility;
            ParkWorldInfoPanel parkPanel = UIView.library.Get<ParkWorldInfoPanel>(typeof(ParkWorldInfoPanel).Name);
            UIButton parkBtn = AddButton(parkPanel.component, "Remove Parking");
            parkBtn.Hide();
            parkBtn.eventClicked += (_, isClicked) => {
                if (InstanceManager.GetPrefabInfo(WorldInfoPanel.GetCurrentInstanceID()) is BuildingInfo buildingInfo) {
                    buildingInfo.m_hasParkingSpaces &= ~VehicleInfo.VehicleType.Car;
                }
            };
            SetParkButtonVisibility = (visibility) => parkBtn.isVisible = visibility;
            CityServiceWorldInfoPanel servicePanel = UIView.library.Get<CityServiceWorldInfoPanel>(typeof(CityServiceWorldInfoPanel).Name);
            UIButton serviceBtn = AddButton(servicePanel.component, "Remove Parking");
            serviceBtn.Hide();
            serviceBtn.eventClicked += (_, isClicked) => {
                if (InstanceManager.GetPrefabInfo(WorldInfoPanel.GetCurrentInstanceID()) is BuildingInfo buildingInfo) {
                    buildingInfo.m_hasParkingSpaces &= ~VehicleInfo.VehicleType.Car;
                }
            };
            SetServicceButtonVisibility = (visibility) => serviceBtn.isVisible = visibility;
        }

        private static UIButton AddButton(UIComponent parent, string name) {
            UIButton btn = parent.AddUIComponent<UIButton>();
            btn.textScale = 0.9f;
            btn.normalBgSprite = "ButtonMenu";
            btn.hoveredBgSprite = "ButtonMenuHovered";
            btn.pressedBgSprite = "ButtonMenuPressed";
            btn.disabledBgSprite = "ButtonMenuDisabled";
            btn.disabledTextColor = new Color32(128, 128, 128, 255);
            btn.canFocus = false;
            btn.relativePosition = new Vector3(parent.width - 133f - 10f, 100f);
            btn.autoSize = true;
            btn.textPadding = new RectOffset(2, 2, 4, 0);
            btn.textHorizontalAlignment = UIHorizontalAlignment.Center;
            btn.textVerticalAlignment = UIVerticalAlignment.Middle;
            btn.text = name;
            return btn;
        }

        public void OnLevelUnloading() {
            RPPatcher.LateDestroy();
        }

        public void OnReleased() {}
    }
}
