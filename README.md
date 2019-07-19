# Blazor Events

A sample project that demostrates one way that EventCallback can hurt.

Have a play - make yourself familiar with the possible performance impact of having the system always call StateHasChanged for you.

Be informed - so you can decide what pattern is right for you.

## Binding in blazor

When you want to two-way bind a value to a blazor component, there are two required parameters.

For the purpose of this short introduction, we will call the bound parameter **"Value"**
#### The Value parameter 
- Used to pass a reference from the Parent to the Child component
- The child component can display and modify the Value 

  **Example**

  `[Parameter] protected string Value { get; set; }`

#### The ValueChanged parameter
- For simple blazor two way binding (the child can read and modify the bound value) you must have a parameter that matches your Value parameter name and appends the word **Changed**
- This parameter is used automatically by the default **`@bind-Value`** syntax as a way to receive notifications from the child component when it has modified the bound **Value**
- There are two main ways to declare this parameter - using **`Action`** or using **`EventCallback`**
- 
  **Example - EventCallback**

  `[Parameter] protected EventCallback<T> ValueChanged { get; set; }`

  **Example - Action**

  `[Parameter] protected Action<T> ValueChanged { get; set; }`

#### What's the difference?
*I'll just mention one or two that I care about - there are other 
technical differences, but they are not relevant to this discussion.*

##### For the developer:
**EventCallback** can make life simpler - especially during 
prototyping as it automatically calls StateHasChanged on the 
target component (sometimes called the root Component) which 
will cause a re-render of the parent and all of it's children.

**Action** can make life more difficult - no automatic 
**StateHasChanged** is called so the developer needs to 
manually call it when needed.

##### For performance:
**EventCallback** can cause many more calls to your component code - 
SetParameters, OnParametersSet/Async, ShouldRender, Markup rendering, 
OnAfterRender/Async etc. 

This can double when your component performs async tasks as it will 
render once before the async task and once after 
*(Turn on simulated workload in Sample 1 to see this)*

*Note: The developer can prevent unwanted re-rendering in their own components by returning **`false`** in the **`ShouldRender`** override, but this can become difficult to manage as this method doesn't know which component/state change has triggered the refresh. See Sample 3.*

**Action** can save you a lot of wasted cpu cycles - 
as it allows you to notify the parent/root component of a change 
in **state** without *forcing* a call to **StateHasChanged**. 

This does mean your code needs to call **StateHasChanged** manually 
at the appropriate times, which can itself be complicated and, 
if done without care and attention, could end up causing just 
as many Re-renders as **EventCallback**.

### Notes - Sample 1 - Balls

This page demonstrates a difference between Action and EventCallback in your value binding.

Each Ball is bound to a value in the page - a simple array of integers - rendered in a `for loop`

The balls alternate between using Action and EventCallback - 
for no good reason - don't let that distract you. It is just whimsy.

Otherwise, the balls are identical.

When a Ball is clicked, it increments it's bound **Value** and invokes the **ValueChanged** parameter.

The balls that use **EventCallback** will automatically trigger a 
full re-render of the page, and the statistics will jump up in large numbers - 
*add more balls by typing a number into the text box to see those numbers really grow!*

The simulated workload is simply a Task.Delay with a random number of 
milliseconds per button - the point of this is to emphasise the potential 
impact on CPU that use of EventCallback in the wrong place can have.

**Notice how you get twice as many updates with an async workload!**

**Note: I have wrapped every component in a custom tag whose job is to flash 
a yellow border - which itself forces an actual DOM update - without this, 
you would not see a DOM update but the components would still render internally.
This is just an artifact to make the effect visible.**

### Note - Sample 2 - Form Input

This page is lifted straight out of the blazor docs 
https://docs.microsoft.com/en-us/aspnet/core/blazor/forms-validation?view=aspnetcore-3.0
and demonstrates the re-rendering effect of EventCallback in your 
value binding in a more *usual* page.

Again the yellow border flash is used to highlight when a re-render happens.

### Note - Sample 3 - Form Input With Guard

This is the same page as Sample 2 but with an added guard in 
its **`ShouldRender`** method that prevents the full page 
re-rendering effect of **EventCallback**.

Again the yellow border flash is used to highlight when 
a re-render happens, which should only occur when you submit the form.

### What to do?

In my opinion, I have yet to find a right time and place for 
**EventCallback**, so my default is to use **Action**.

I don't call **StateHasChanged** on a parent Component, unless I ***really*** 
want to update every child.

I believe you should separate your App State logic from your 
UI logic as much as possible, which means not having code automatically 
calling **StateHasChanged**.

It doesn't matter what App State *pattern* you choose, but I 
do believe you should choose one - there are awesome community developed 
component libraries on awesome blazor (https://github.com/AdrienTorris/awesome-blazor) - 
I won't recommend a particular one - I haven't enough experience of them, 
but the authors are definitely awesome!

Please don't treat your "Views" as State Containers - they are there to 
manage rendering, not business logic.

Choose when to update your UI - debate with yourself/your team about 
whether components should handle their own UI state and decide when to refresh 
OR whether a parent component should decide when to refresh its children's UI.

Just have the debate - make a choice - don't fall blindly into 
something that you didn't expect to be a problem.

Got other examples? Or found a bug? => Pull Request!

#### Try me

https://blazorevents.azurewebsites.net/
Note: hosted on the free tier of Azure - load times are slow!

Imagine if your view had hundreds of controls all internally re-rendering just because something happened in one of the components.

This is not neccessarily going to affect the DOM - the differ takes care of that - but do you want your server wasting cycles in Server side Blazor? 

Do you want your Client Side Blazor users complaining their browser is hogging CPU?

MM