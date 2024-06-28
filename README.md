# BackendTestProject
## Features

- Fetch a list of items from the database
- Place an order and split it into packages based on price and weight limits
- Calculate courier prices based on package weight

## Technologies Used

- ASP.NET Core
- Dapper for data access
- SQL Server
- Dependency Injection
- CORS support

## Getting Started

### Prerequisites

- .NET 8
- SQL Server

### Installation

1. Clone the repository:


2. Update the connection string in `appsettings.json`:
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "xxxxxxxxxx"
      }
    }
    ```

3. Restore the dependencies and build the project:


4. Run the application:


## API Endpoints
 ```json
- **Get All Items**: https://localhost:7104/api/Order/items
- **Place Order**: https://localhost:7104/api/Order/placeorder
 ```



### Get Items

Retrieve a list of all items.

- **URL**: `GET /api/order/items`
- **Response**:
    ```json
    [
        {
            "id": 1,
            "name": "Item1",
            "price": 100,
            "weight": 50
        },
        {
            "id": 2,
            "name": "Item2",
            "price": 150,
            "weight": 70
        }
    ]
    ```

### Place Order

Place an order with a list of item IDs and get the split packages.

- **URL**: `POST /api/order/placeorder`
- **Request Body**:
    ```json
    [1, 2, 3]
    ```
- **Response**:
    ```json
    [
        {
            "items": [
                {"id": 1, "name": "Item1", "price": 100, "weight": 50},
                {"id": 2, "name": "Item2", "price": 150, "weight": 70}
            ],
            "totalWeight": 120,
            "totalPrice": 250,
            "courierPrice": 10
        },
        {
            "items": [
                {"id": 3, "name": "Item3", "price": 200, "weight": 100}
            ],
            "totalWeight": 100,
            "totalPrice": 200,
            "courierPrice": 5
        }
    ]
    ```

## Project Structure

- **Controllers**: Contains API controllers.
- **Models**: Contains data models.
- **Services**: Contains business logic.

## Configuration

The connection string is stored in `appsettings.json`. Update it with your SQL Server connection details.