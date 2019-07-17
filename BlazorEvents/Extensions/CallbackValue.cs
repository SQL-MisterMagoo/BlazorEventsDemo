/*
 * Base class that implements a Value / ValueChanged parameter pair using EventCallback<T>
 */
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorEvents
{
    public class CallbackValue<T> : EventComponentBase
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
                    _value = value;
                    ValueChanged.InvokeAsync(value);
                }
            }
        }
        [Parameter] protected EventCallback<T> ValueChanged { get; set; }
    }
}
