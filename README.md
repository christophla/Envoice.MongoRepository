### Envoice 

# MongoRepository [![Build status](https://ci.appveyor.com/api/projects/status/09c4fnv2ov54vpwm?svg=true)](https://ci.appveyor.com/project/christophla/conditions)

MongoDB repository abstraction library with virtual collection support.


## Supported Platforms
* .NET 2.0

## Installation

https://www.myget.org/feed/envoice/package/nuget/Envoice.MongoRepository

Add dependency to you project.json:

``` bash

dotnet add package Envoice.MongoRepository --version 1.0.0 --source https://www.myget.org/F/envoice/api/v3/index.json
```

### Virtual Collections

Virtual collections allow multiple entity types to be stored in a single collection.

#### Connection String

Connection string options can be used to configure the repositories. All connection string options override code-based options, e.g. *MongoRepositoryConfig*.

```
mongodb://{host}:{port}/?virtual=true&virtualCollection=Entities&virtualCollectionGlobal=true
```

- *virtual* (boolean) : Enables virtual collections. Default = false. 
- *virtualCollection* (string) : Default virtual collection for all unmapped entities.
- *virtualCollectionGlobal* (boolean) : Overrides all entities to use the *virtualCollection* regardless of mapping configuration.

#### ASPNET Service (TODO)

The repositories can be configured to use virtual collections in ASPNET applications via the UseMongoRepository method.

``` C#
services.UseMongoRepository(config => {

    config.VirtualCollectionDefault = "Entities";
    config.VirtualCollectionEnabled = true;
    config.VirtualCollectionForceGlobal = false;

});
```

#### Attributes

``` C#
  [VirtualCollectionName("Entities")]
  public class Product {
    ...
  }
```
