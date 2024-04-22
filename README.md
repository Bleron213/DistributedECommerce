
<h1 align="center">
  <br>
  <br>
    Distributed E-commerce Web Application
  <br>
</h1>

<h4 align="center"> A distributed Web Application for handling Orders & Products</h4>

<p align="center">

## Key Features

* ASP.NET 8.0
* Mediator and CQRS pattern
* Two simulated microservices, utilizing Clean Architecture
* Domain-Driven Design
* Cross Platform
* Global Exception Handling
* Structured Logging
* Request correlation
* Fully compliant with Dependency Injection
* Dockerized
* Communication using RabbitMQ and classic API requests

## Technology Stack

* ASP.NET Core 8.0 Web API
* Ocelot for API Gateway
* RabbitMQ for Queue-based communication

## How To Use

* Clone the git repo locally

```bash
# Clone this repository
$ git clone https://github.com/Bleron213/DistributedECommerce
```

# How to run in your local machine
* Open the solution in Visual Studio 2022
* Start DistributedECommerce.Order.API, DistributedECommerce.Warehouse.API,DistributedEcommerce.ApiGateway together project. Additionally, make sure there is a valid LocalDb connection and that RabbitMq is running (Windows, No Docker).

# How to run in Docker
* Navigate to the directory where the solution is
* Open a terminal and type **docker-compose up**

## How To Test

In the solution root, you can find a postman collection. Open it in Postman and you can send test requests.
  
## Credits

This software uses the following open source software in its source code:

- [.ASP.NET Core 8](https://github.com/dotnet)
- [MediatR](https://github.com/jbogard/MediatR)
- [EF Core](https://github.com/efcore)
- [Dapper](https://github.com/DapperLib/Dapper)
- [Fluent Validation](https://github.com/FluentValidation/FluentValidation)
- [XUnit](https://github.com/xunit/xunit)
- [Serilog](https://github.com/serilog/serilog)
- [RabbitMQ](https://github.com/rabbitmq)
