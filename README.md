# Products Service

A project following the principles of Clean Architechture and Domain-driven design (DDD), Repository and Unit of Work Patterns.

## Getting started

### Prerequisites

- Docker
- Docker Compose

### How to run:

- Execute:

        docker-compose up
        
- Visual Studio 2019:
    - Select docker-compose as startup project
    - Run docker-compose profile

Visit http://localhost:5000 for SwaggerUI


## Todos and considerations

* Unit tests for Products.Domain need to be completed
* Integration tests for Products.API need to be completed
* Consider using Dapper for querying Products and Product Options. Dapper is much faster than Entity Framework Core. Query calls do not alter the domain, hence they don't have to be made through the repository.
* Use mediatR to fire domain events from Products.Domain project. Handle the domain events to log etc. in Products.API.
* Create seperate methods to update different properties of concern of the domain objects instead of a single update method.


## How to run migrations 
Initial migration is already created, so no need to run this:

    dotnet ef migrations add "SampleMigration" --project "./src/Products.Infrastructure" --startup-project "./src/Products.API" --output-dir "./src/Products.Infrastructure/Persistence/Migrations"
