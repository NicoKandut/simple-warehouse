# Simple Warehouse API
This document aims to explain all features of the **Simple Warehouse API** and how to use them. The API can be accessed online at [heroku](https://simple-warehouse-api.herokuapp.com/).

## Route Documentation
Here all routes are listed and expained briefly.

________
### Root
**GET [https://simple-warehouse-api.herokuapp.com](https://simple-warehouse-api.herokuapp.com)**    
returns a small welcome message and some links concerning other functions like **owners** and **warehouses**.

__________
### Owners
**GET [https://simple-warehouse-api.herokuapp.com/owners](https://simple-warehouse-api.herokuapp.com/owners)**  
returns **a list of all owners** with some information about them. For now this also returns the passwords of the owners but in the future it will not.

**POST [https://simple-warehouse-api.herokuapp.com/owners](https://simple-warehouse-api.herokuapp.com/owners)**     
creates **a new owner** with the information provided. Returns 201 for success or 500 for error.

**GET [https://simple-warehouse-api.herokuapp.com/owners/:id](https://simple-warehouse-api.herokuapp.com/owners/1)**    
returns **one owner** with some information about it. Additionally, it includes **a list of this owners warehouses**. If an owner is not found this returns 404.

______________
### Warehouses

**GET [https://simple-warehouse-api.herokuapp.com/warehouses](https://simple-warehouse-api.herokuapp.com/warehouses)**  
returns **a list of all warehouses** with some information about them.

**POST [https://simple-warehouse-api.herokuapp.com/warehouses](https://simple-warehouse-api.herokuapp.com/warehouses)**     
creates **a new warehouse** with the information provided. Returns 201 for success or 500 for error.

**GET [https://simple-warehouse-api.herokuapp.com/warehouses/:id](https://simple-warehouse-api.herokuapp.com/warehouses/1)**    
returns **one warehouse** with some information about it. Additionally, it includes **a list of products stored in this warehouse**. If a warehouse is not found this returns 404.
