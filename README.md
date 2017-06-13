# Lyra Core Library

A .NET Core implementation for using Lyra boards via a CAN interface. To contact Lyra regarding electronic components go to their website [here](http://www.lyraelectronics.com).

To use the source code you will need at least Visual Studio 2015 and the latest .Net Core toolsets. You can find a link to the nuget feed for the compiled libraries below.

[![Build status](https://ci.appveyor.com/api/projects/status/y2wfkk2u29ry7igp/branch/master?svg=true)](https://ci.appveyor.com/project/CicerosPatience/lyra-core/branch/master) [![NuGet](https://img.shields.io/nuget/v/LyraElectronics.Core.svg)](https://www.nuget.org/packages/LyraElectronics.Core/)

## Getting Started

### Implement ICanController

 The `ICanController` is critically responsible for listening to the CAN messages being polled across the CAN line and parsing those messages into `CanMmessage` objects. Those objects are then fired across on the `CanMessageRecievedEvent`. The `CanBoard`s listen on this event to parse their status. As such it is critically important that this event is fired by the `ICanController` implementation.

 The `ICanController` interface should be implemented based on the type of CAN controller being used and will look something like the code below.

```csharp
    public sealed class CanController : ICanController
    {
        // fire in listener methods
        public event CanMessageRecievedEventHandler CanMessageRecieved;

        public void CloseChannel()
        {
            // close the CAN channel
        }

        public void OpenChannel(int baudRate = 250)
        {
            // open the CAN channel
        }

        public void SendMessage(CanMessage message)
        {
            // Send CAN message
        }

    }

```

### Using the CanBoard Classes

When creating the `CanBoard` class, the object should be passed the `ICanController` where it will subscribe to the CanMessageRecieved event internally.

```csharp
SaciaBoard board = new SaciaBoard(2, _controller);

board.SetMovementProperties(1000, 80, 80);
board.SetCurrent(1200, 100);
board.Zero();

board.Run(4000);
```

## Acknowledgements

* Thank you to [Lyra](http://www.lyraelectronics.com) for providing the boards/documentation and valuable feedback/support
* A big thank you to [Biosero](http://www.biosero.com) for their field work and feedback.
