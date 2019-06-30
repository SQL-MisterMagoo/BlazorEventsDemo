# Blazor Events

A sample project that demostrates one way that EventCallback can hurt.

Have a play - make yourself familiar with the possible performance impact of having the system always call StateHasChanged for you.

Be informed - so you can decide what pattern is right for you.

### Note

The simulated workload is simply a Task.Delay with a random number of milliseconds per button - the point of this is to emphasise the potential impact on CPU that use of EventCallback in the wrong place can have.

Imagine if your view had hundreds of controls all internally re-rendering just because something happened in one of the components.

This is not neccessarily going to affect the DOM - the differ takes care of that - but do you want your server wasting cycles in Server side Blazor? 

Do you want your Client Side Blazor users complaining their browser is hogging CPU?

### What to do?

In my opinion, I have yet to find a right time and place for EventCallback, so my default is to use Actions.

I don't call StateHasChanged on a parent Component, unless I really want to update every child.

The only time this app will perform a mass update is on initial load - as it should - and when you click a component that is bound using EventCallback.

That is why I don't use it.

Convince me otherwise with a Pull Request?

#### Try me

https://blazorevents.azurewebsites.net/

