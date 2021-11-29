using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using System.Reflection;
using System.Reflection.Emit;
using ColossalFramework;
using UnityEngine;

namespace RemoveParking {
    internal static class RPPatcher {
        private const string HARMONYID = @"com.quistar.removeParking";
        internal static Building[] m_buildings;

        private static void SetTargetPostfix(WorldInfoPanel __instance, InstanceID id) {
            if(__instance is ParkWorldInfoPanel parkPanel) {
                ushort buildingID = id.Building;
                if(id.Type == InstanceType.Building && buildingID != 0 && m_buildings[buildingID].Info is BuildingInfo info) {
                    RPModule.SetParkButtonVisibility((info.m_hasParkingSpaces & VehicleInfo.VehicleType.Car) == VehicleInfo.VehicleType.Car);
                }
            } else if(__instance is ZonedBuildingWorldInfoPanel zonedPanel) {
                ushort buildingID = id.Building;
                if (id.Type == InstanceType.Building && buildingID != 0 && m_buildings[buildingID].Info is BuildingInfo info) {
                    RPModule.SetZonedButtonVisibility((info.m_hasParkingSpaces & VehicleInfo.VehicleType.Car) == VehicleInfo.VehicleType.Car);
                }
            } else if(__instance is CityServiceWorldInfoPanel servicePanel) {
                ushort buildingID = id.Building;
                if (id.Type == InstanceType.Building && buildingID != 0 && m_buildings[buildingID].Info is BuildingInfo info) {
                    RPModule.SetServicceButtonVisibility((info.m_hasParkingSpaces & VehicleInfo.VehicleType.Car) == VehicleInfo.VehicleType.Car);
                }
            }
        }

        internal static void LateSetup() {
            m_buildings = Singleton<BuildingManager>.instance.m_buildings.m_buffer;
        }

        internal static void LateDestroy() {
            m_buildings = null;
        }

        internal static void EnablePatches() {
            Harmony harmony = new Harmony(HARMONYID);
            HarmonyMethod newPostfix = new HarmonyMethod(AccessTools.Method(typeof(RPPatcher), nameof(SetTargetPostfix)));
            harmony.Patch(AccessTools.Method(typeof(WorldInfoPanel), "SetTarget", new Type[] { typeof(Vector3), typeof(InstanceID) }), postfix: newPostfix);
        }

        internal static void DisablePatches() {
            Harmony harmony = new Harmony(HARMONYID);
            harmony.Unpatch(AccessTools.Method(typeof(WorldInfoPanel), "SetTarget", new Type[] { typeof(Vector3), typeof(InstanceID) }), HarmonyPatchType.Postfix, HARMONYID);
        }
    }
}
