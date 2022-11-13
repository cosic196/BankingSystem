# Banking System

A simple API used to manage bank accounts under specific domain rules:
- System allows for users and user accounts.
- A user can have as many accounts as they want.
- A user can create and delete accounts.
- A user can deposit and withdraw from accounts.
- An account cannot have less than $100 at any time in an account.
- A user cannot withdraw more than 90% of their total balance from an account in a single transaction.
- A user cannot deposit more than $10,000 in a single transaction.

## How to run:

Pull the repository from GitHub and run the docker compose file located in the root folder with `docker-compose up`.
Once the docker container is up and running, navigate to http://localhost:8080/swagger/index.html to get to the Swagger page of the API.

## Project Structure

The solution is structured into 5 separate projects:
- BankingSystem.Api - Represents the API layer (Controllers, API Models, API Model validation, ASP.NET middlewares)
- BankingSystem.Application - Represents the application layer (command and query handlers)
- BankingSystem.Infrastructure - Represents the infrastructure layer (only repository implementation in this case)
- BankingSystem.Domain - Represents the core/domain layer (entities, repository contract, value objects, domain exceptions)
- BankingSystem.Tests - Unit and integration tests for the API