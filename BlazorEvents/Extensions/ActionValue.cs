/*
 * Base class that implements a Value / ValueChanged parameter pair using Action<T>
 */
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace BlazorEvents
{
    public class ActionValue<T> : EventComponentBase
    {
        private T _value { get; set; }
        [Parameter]
        protected T Value
        {
            get => _value;
            set
            {
                var hasChanged = !EqualityComparer<T>.Default.Equals(value, _value);
                if (hasChanged)
                {
                    _value = value;
                    ValueChanged?.Invoke(value);
                }
            }
        }
        [Parameter] protected Action<T> ValueChanged { get; set; }
    }
}
