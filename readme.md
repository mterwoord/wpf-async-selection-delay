WPF 4.5 selection delay using async code
===================================

This sample project shows how to implement a delay in a property change, using async task cancellation.
The same can be achieved by the WPF Binding.Delay property, but I found that in some situations this doesn't work as expected.

How does it work?
---------
The window contains a TextBox control, which is bound to the dependency property Input. The Input property changed handler uses a CancellationTokenSource for calling HandleValueChanged, but does this after 1 second. If within that 1 second,
the property gets changed again, the count-down is restarted. This is proven by the TextBlock control in below, which gets updated after the 1 second delay.

License
-------
You can use this code to learn how to do things. If you find improvements, I'd appreciate you letting me know about that, but you're not required. Also, I do not accept liability for any bugs in the code!
