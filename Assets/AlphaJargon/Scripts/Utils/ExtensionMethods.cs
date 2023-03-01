using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingBlocks.DataTypes;

namespace PixelGame
{
    public static class ExtensionMethods
    {
        public static int ToIndex(this PixelPosition pixelPosition)
        {
            return pixelPosition.x * PixelScreen.GridSideSize + pixelPosition.y;
        }
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this InspectableDictionary<TKey, TValue> inspectableDictionary) {
            var dictionary = new Dictionary<TKey, TValue>();
            foreach (var kvp in inspectableDictionary) {
                dictionary.Add(kvp.Key, kvp.Value);
            }
            return dictionary;
        }

        public static InspectableDictionary<TKey, TValue> FromDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dictionary) {
            var inspectableDictionary = new InspectableDictionary<TKey, TValue>();
            foreach (var kvp in dictionary) {
                inspectableDictionary.Add(kvp.Key, kvp.Value);
            }
            return inspectableDictionary;
        }
    }
}