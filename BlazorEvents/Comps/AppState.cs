using System;

namespace BlazorEvents.Comps
{
    public class AppState
    {
        public AppState()
        {

        }
        public event EventHandler GotNewState;

        private bool useWorkload;
        public bool UseWorkload => useWorkload;

        public void SetUseWorkload(bool value)
        {
            useWorkload = value;
            RaiseGotNewState();
        }
        private void RaiseGotNewState()
        {
            var evt = GotNewState;
            evt?.Invoke(this, default);
        }
    }
}
