using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace BlazorEvents.Comps
{
    public class CallbackValue<T> : ComponentBase
    {
        private T _value { get; set; }
        [Parameter]
        protected T Value
        {
            get =>_value;
            set
            {
                var hasChanged = !EqualityComparer<T>.Default.Equals(value, _value);
                if (hasChanged)
                {
                    Console.WriteLine($"Old:{_value} New:{value}");
                    _value = value;
                    ValueChanged.InvokeAsync(value);
                }
            }
        }
        [Parameter] protected EventCallback<T> ValueChanged { get; set; }
    }
}
