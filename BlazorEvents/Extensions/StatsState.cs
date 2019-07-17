/*
 * A simple State class used for collecting aggregate statistics on rendering calls
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorEvents
{
    public class StatsState
    {
        public StatsState()
        {

        }
        public event EventHandler GotNewStats;

        private int shouldRenderCount;
        public int ShouldRenderCount => shouldRenderCount;
        private int didRenderCount;
        public int DidRenderCount => didRenderCount;

        public void DidRender()
        {
            didRenderCount++;
            RaiseGotNewStats();
        }
        public void ShouldRender()
        {
            shouldRenderCount++;
            RaiseGotNewStats();
        }

        private void RaiseGotNewStats()
        {
            var evt = GotNewStats;
            evt?.Invoke(this, default);
        }
    }
}
