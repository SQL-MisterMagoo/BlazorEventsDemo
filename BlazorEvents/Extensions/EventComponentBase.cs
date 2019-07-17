// Base on Microsoft.AspNetCore.Components.ComponentBase
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// This version adds the ability to flash a yellow border on AfterRender and optionally log method calls to the console
// It relies on a companion component to provide a specific CascadingValue<StyleContainer> - so not for general use

using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace BlazorEvents
{
    /// <summary>
    /// Optional base class for components. Alternatively, components may
    /// implement <see cref="IComponent"/> directly.
    /// </summary>
    public abstract class EventComponentBase : IComponent, IHandleEvent, IHandleAfterRender
    {
        [Parameter] protected RenderFragment ChildContent { get; set; }
        [CascadingParameter(Name = "DynamicStyles")] protected StyleContainer StyleContainerState { get; set; }
        [Inject] public StatsState Stats { get; set; }

        private const string FLASHSTYLE = @"ec-base {   
    display: inline-grid;
    width: fit-content;
    padding:4px;
    margin:0;
    --ec-animation:ECFLASH1;
    animation:var(--ec-animation,ECFLASH1) 200ms ease;
}
@keyframes ECFLASH1 {
    0% {}
    10% { box-shadow: inset 0 0 0 4px yellow; }
    90% { box-shadow: inset 0 0 0 4px yellow; }
    100% {}
}
@keyframes ECFLASH2 {
    0% {}
    10% { box-shadow: inset 0 0 0 4px yellow; }
    90% { box-shadow: inset 0 0 0 4px yellow; }
    100% {}
}";
        private readonly RenderFragment _renderFragment;
        private RenderHandle _renderHandle;
        private bool _initialized;
        private bool _hasNeverRendered = true;
        private bool _hasPendingQueuedRender;
        public bool ConsoleOutput { get; set; } = false;
        public int Toggle { get; set; } = 1;

        static bool initialised { get; set; }

        internal void QC(string message = "", [CallerMemberName] string caller = "")
        {
            if (ConsoleOutput)
                Console.WriteLine($"{GetType().Name}.{caller}: {message}");
        }
        /// <summary>
        /// Constructs an instance of <see cref="ComponentBase"/>.
        /// </summary>
        public EventComponentBase()
        {
            QC();
            _renderFragment = builder =>
            {
                QC($"BuildRenderTree:{GetType().Name}");
                _hasPendingQueuedRender = false;
                _hasNeverRendered = false;
                builder.OpenElement(0, "ec-base");
                builder.AddAttribute(1, "style", $"--ec-animation:ECFLASH{Toggle}");
                BuildRenderTree(builder);
                builder.CloseElement();
            };
        }

        /// <summary>
        /// Renders the component to the supplied <see cref="RenderTreeBuilder"/>.
        /// </summary>
        /// <param name="builder">A <see cref="RenderTreeBuilder"/> that will receive the render output.</param>
        protected virtual void BuildRenderTree(RenderTreeBuilder builder)
        {
            QC();
            // Developers can either override this method in derived classes, or can use Razor
            // syntax to define a derived class and have the compiler generate the method.

            // Other code within this class should *not* invoke BuildRenderTree directly,
            // but instead should invoke the _renderFragment field.
        }

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// </summary>
        protected virtual void OnInit()
        {
            QC();
            if (initialised)
                return;
            if (StyleContainerState is object)
            {
                StyleContainerState.AddStyle(FLASHSTYLE);
                initialised = true;
            }
        }

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        ///
        /// Override this method if you will perform an asynchronous operation and
        /// want the component to refresh when that operation is completed.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        protected virtual Task OnInitAsync()
        {
            QC();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in
        /// the render tree, and the incoming values have been assigned to properties.
        /// </summary>
        protected virtual void OnParametersSet()
        {
            QC();
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in
        /// the render tree, and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        protected virtual Task OnParametersSetAsync()
        {
            QC();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Notifies the component that its state has changed. When applicable, this will
        /// cause the component to be re-rendered.
        /// </summary>
        protected void StateHasChanged()
        {
            QC();
            if (_hasPendingQueuedRender)
            {
                return;
            }

            if (_hasNeverRendered || ShouldRender())
            {
                _hasPendingQueuedRender = true;

                try
                {
                    _renderHandle.Render(_renderFragment);
                }
                catch
                {
                    _hasPendingQueuedRender = false;
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns a flag to indicate whether the component should render.
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShouldRender()
        {
            QC();
            Stats?.ShouldRender();
            return true;
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        protected virtual void OnAfterRender()
        {
            QC();
            Toggle = ( Toggle == 2 ) ? 1 : 2;
            Stats?.DidRender();
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered. Note that the component does
        /// not automatically re-render after the completion of any returned <see cref="Task"/>, because
        /// that would cause an infinite render loop.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing any asynchronous operation.</returns>
        protected virtual Task OnAfterRenderAsync()
        {
            QC();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Executes the supplied work item on the associated renderer's
        /// synchronization context.
        /// </summary>
        /// <param name="workItem">The work item to execute.</param>
        protected Task Invoke(Action workItem)
        {
            QC();
            return _renderHandle.Invoke(workItem);
        }

        /// <summary>
        /// Executes the supplied work item on the associated renderer's
        /// synchronization context.
        /// </summary>
        /// <param name="workItem">The work item to execute.</param>
        protected Task InvokeAsync(Func<Task> workItem)
        {
            QC();
            return _renderHandle.InvokeAsync(workItem);
        }

        void IComponent.Configure(RenderHandle renderHandle)
        {
            QC();
            // This implicitly means a ComponentBase can only be associated with a single
            // renderer. That's the only use case we have right now. If there was ever a need,
            // a component could hold a collection of render handles.
            if (_renderHandle.IsInitialized)
            {
                throw new InvalidOperationException($"The render handle is already set. Cannot initialize a {nameof(ComponentBase)} more than once.");
            }

            _renderHandle = renderHandle;
        }

        /// <summary>
        /// Method invoked to apply initial or updated parameters to the component.
        /// </summary>
        /// <param name="parameters">The parameters to apply.</param>
        public virtual Task SetParametersAsync(ParameterCollection parameters)
        {
            QC();
            parameters.SetParameterProperties(this);
            if (!_initialized)
            {
                _initialized = true;

                return RunInitAndSetParametersAsync();
            }
            else
            {
                return CallOnParametersSetAsync();
            }
        }

        private async Task RunInitAndSetParametersAsync()
        {
            QC();
            OnInit();
            var task = OnInitAsync();

            if (task.Status != TaskStatus.RanToCompletion && task.Status != TaskStatus.Canceled)
            {
                // Call state has changed here so that we render after the sync part of OnInitAsync has run
                // and wait for it to finish before we continue. If no async work has been done yet, we want
                // to defer calling StateHasChanged up until the first bit of async code happens or until
                // the end. Additionally, we want to avoid calling StateHasChanged if no
                // async work is to be performed.
                StateHasChanged();

                try
                {
                    await task;
                }
                catch // avoiding exception filters for AOT runtime support
                {
                    // Ignore exceptions from task cancelletions.
                    // Awaiting a canceled task may produce either an OperationCanceledException (if produced as a consequence of
                    // CancellationToken.ThrowIfCancellationRequested()) or a TaskCanceledException (produced as a consequence of awaiting Task.FromCanceled).
                    // It's much easier to check the state of the Task (i.e. Task.IsCanceled) rather than catch two distinct exceptions.
                    if (!task.IsCanceled)
                    {
                        throw;
                    }
                }

                // Don't call StateHasChanged here. CallOnParametersSetAsync should handle that for us.
            }

            await CallOnParametersSetAsync();
        }

        private Task CallOnParametersSetAsync()
        {
            QC();
            OnParametersSet();
            var task = OnParametersSetAsync();
            // If no async work is to be performed, i.e. the task has already ran to completion
            // or was canceled by the time we got to inspect it, avoid going async and re-invoking
            // StateHasChanged at the culmination of the async work.
            var shouldAwaitTask = task.Status != TaskStatus.RanToCompletion &&
                task.Status != TaskStatus.Canceled;

            // We always call StateHasChanged here as we want to trigger a rerender after OnParametersSet and
            // the synchronous part of OnParametersSetAsync has run.
            StateHasChanged();

            return shouldAwaitTask ?
                CallStateHasChangedOnAsyncCompletion(task) :
                Task.CompletedTask;
        }

        private async Task CallStateHasChangedOnAsyncCompletion(Task task)
        {
            QC();
            try
            {
                await task;
            }
            catch // avoiding exception filters for AOT runtime support
            {
                // Ignore exceptions from task cancelletions, but don't bother issuing a state change.
                if (task.IsCanceled)
                {
                    return;
                }

                throw;
            }

            StateHasChanged();
        }

        Task IHandleEvent.HandleEventAsync(EventCallbackWorkItem callback, object arg)
        {
            QC();
            var task = callback.InvokeAsync(arg);
            var shouldAwaitTask = task.Status != TaskStatus.RanToCompletion &&
                task.Status != TaskStatus.Canceled;

            // After each event, we synchronously re-render (unless !ShouldRender())
            // This just saves the developer the trouble of putting "StateHasChanged();"
            // at the end of every event callback.
            StateHasChanged();

            return shouldAwaitTask ?
                CallStateHasChangedOnAsyncCompletion(task) :
                Task.CompletedTask;
        }

        Task IHandleAfterRender.OnAfterRenderAsync()
        {
            QC();
            OnAfterRender();

            return OnAfterRenderAsync();

            // Note that we don't call StateHasChanged to trigger a render after
            // handling this, because that would be an infinite loop. The only
            // reason we have OnAfterRenderAsync is so that the developer doesn't
            // have to use "async void" and do their own exception handling in
            // the case where they want to start an async task.
        }
    }
}
