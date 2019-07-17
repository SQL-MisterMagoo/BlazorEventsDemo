/*
 * A simple class used by the ComponentStyle to provide a cascading value that other components can call back to 
 * Used to add styles to the page at runtime
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEvents
{
    public class StyleContainer
    {
        private List<string> styles;
        public Action StateHasChanged { get; set; }
        public StyleContainer()
        {
            styles = new List<string>();
        }
        public void AddStyle(string style)
        {
            if (!styles.Contains(style))
            {
                styles.Add(style);
                StateHasChanged?.Invoke();
            }
        }
        public string GetStyles()
        {
            return string.Join(" ", styles.GroupBy(r => r).Select(g => g.Key));
        }
    }
}
