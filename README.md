
# Project Title

This Project illustrate how to use SeriLog in .Net Core Web API Project.Here we have demostrate StoreManagementSystem where User can perform the CRUD of Product, Purchase Product and Sell Product.We have 3 types of User Role i.e SuperAdmin, Admin and User.In this we have implemented Logging using Serilog which store the Logs in Database as well as in our local file system.
## Installation

For use of this Project previuosly you have to installed Visual Studio 2022.

To Download Visual Studio 2022 Community Edition visit this link [here](https://visualstudio.microsoft.com/downloads/)

To Download SQL Server 2019 Express Edition visit this link [here](https://www.microsoft.com/en-US/download/details.aspx?id=101064)

To Download SQL Server Management Studio 2018 visit this link [here](https://learn.microsoft.com/en-us/sql/ssms/release-notes-ssms?view=sql-server-ver15) 

## Creating Database

After the installation of project you’ll need to create a database for it on a local machine with name "StoreManagement".

```
CREATE DATABASE [StoreManagement]
GO
```

## Modify connection string of your environment 

After creating database you have to modify below line of connection string in appsettings.json file.

```
"ConnectionStrings": {
    "DefaultConnection": "Server={ServerName};DataBase=StoreManagement;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  },
```
For saving the logs into the database you have to modify below connection string in appsettings.json file.

#### *Please Note: This connection string is under the serilog section
```
"connectionString": "Server={ServerName};DataBase=StoreManagement;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  ```

Note: Connection string may vary as per environment and system.

## Migrating Database

After changing connection string as shown above run below command in package manage console for setting up database.

``` PM> update-database```

As the things done successfully, Now your ready to go to run this project.

## WorkFlow

Step 1 : SuperAdmin can register and remove Admin and user, Admin can register user with Name, EmailAddress, Password and IsAdmin (CreateUser, GetAllUsers, GetActiveUsers and RemoveUser API).

Step 2 : After the registration done user i.e Admin and user has to login with EmailAddress and Password (Login API).

Step 3: Admin can perform CRUD of Product with Name, Description and Price (CreateProduct, GetAllProducts, UpdateProduct and RemoveProduct API). Admin can read and insert Puchase Product with ProductId and Quantity (GetPurchaseProduct and CreatePurchaseProduct API). Admin can't update and remove the Purchase Product (UpdatePruchaseProduct and RemovePurchaseProduct).

Step 4: User can perform CRUD of Sell Product with ProductId and Description (CreateSellProduct, GetSellProduct, UpdateSellProduct and RemoveSellProduct API). User can also see list of  Product in stock (ProductStock API).