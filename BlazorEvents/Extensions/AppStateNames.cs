/*
 * Some names to be used in AppState
 */
using System;

namespace BlazorEvents
{
    public enum AppStateNames
    {
        // Toggles
        WorkLoad,
        DisplayStats,
        // Configuration
        BallCount
    }

    public static class enumberables
    {
        public static string GetName<T>(this T enumType, int Value)
        {
            return Enum.GetName(typeof(T), Value);
        }
    }
}