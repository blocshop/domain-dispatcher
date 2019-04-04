# DomainDispatcher

Simple publish subscribe event bus suitable for distributing domain events, well integrated with dependency injection and async handling

## Getting Started

Install it from Nuget

```
Install-Package DomainDispatcher
```

### Installing

Add this to your startup

```
services.AddDomainDispatcher((config) =>
{
    config.Subscribe<SpecificDomainEvent, SpecificDomainEventHandler>();
});
```

## Authors

* **Constantine Nalimov** 
* **Vojtech Machacek** 

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details