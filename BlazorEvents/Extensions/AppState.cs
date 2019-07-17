/*
 * A simple state class for the demo app
 */
using System;
using System.Collections.Generic;

namespace BlazorEvents
{
    public class AppState
    {
        public AppState()
        {
            Values = new Dictionary<AppStateNames, object>();
        }

        public event EventHandler<AppStateNames> GotNewState;

        private Dictionary<AppStateNames, object> Values;
        public object GetValue(AppStateNames name)
        {
            if (Values.ContainsKey(name))
                return Values[name];
            return null;
        }

        public void SetValue(AppStateNames name, object value)
        {
            Values[name] = value;
            RaiseGotNewState(name);
        }
        private void RaiseGotNewState(AppStateNames name)
        {
            var evt = GotNewState;
            evt?.Invoke(this, name);
        }

        public string GetName(AppStateNames Name)
        {
            return Enum.GetName(typeof(AppStateNames), Name);
        }
    }
}
