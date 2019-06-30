using Microsoft.AspNetCore.Components;
using System;

namespace BlazorEvents.Comps
{
    public class ActionValue<T> : ComponentBase
    {
        private T _value { get; set; }
        [Parameter] protected T Value { get=>_value; set {_value=value; ValueChanged?.Invoke(value); } }
        [Parameter] protected Action<T> ValueChanged { get; set; }
    }
}
