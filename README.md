# Golfs-a-lot Store

## Project Description
This web application allows users to browse golf products. A user can select a store location to see their inventory, choose an item to purchase, as well as see a store's order history. A user can also see their cart with items they plan on purchasing, their own order history, and other customers' order history. One must create an account with Golfs-a-lot before entering the site and making purchases.

## Technologies Used
* C#
* ASP.NET MVC
* Entity Framework
* HTML5 / CSS3
* Microsoft SQL Server
* SQL
* XML
* xUnit Testing

## Features
Ready features:
* Sign up with username and password, and successfully login with that information
* Search for other customers by name or username
* Make orders from desired store locations
* See your order history, another customer's order history, or a store location's order history
To-do list:
* Make cart accessible from any point in the application
* Input validation pop-ups to alert the user of an error or mistake
* Sales and events for cheaper prices / bundles

## Getting Started
Luckily, this application doesn't take much to run.
* Visual Studio 2019 OR Visual Studio Code must be installed
* clone this repository using the following command:
   git clone https://github.com/06012021-dotnet-uta/BrianCorbettP1
* You will then just need to open the main solution: P1Project/ > P1Main/ > P1Main.sln
* From there, it's just a matter of pressing run

## Usage
This is the landing page. Either create an account if you do not have one already or login with an existing one.
![Screenshot 2021-07-23 152354](https://user-images.githubusercontent.com/85184581/126847899-ceb92a3d-f640-48cf-81f0-846f053064bd.png)

The home page has options to 'Shop', 'See Order History', or 'Search for a Customer'.
![home_page](https://user-images.githubusercontent.com/85184581/126847946-46fd7d61-4027-4c11-8f4a-994954700ea1.png)

If you click 'Shop', it brings you to the user's default store location inventory.
You can also click 'Change Store' under the heading to select a different store.
![inventory](https://user-images.githubusercontent.com/85184581/126848066-ed4c0bf2-d49e-481e-89e4-cbf7de93ddce.png)

When a user has decided on an item, they may click 'Add to Cart' where they can specify how many of that item they'd like to purchase.

When a user is done shopping, they'll see a 'Checkout' button where they can finalize their order.
![full_cart](https://user-images.githubusercontent.com/85184581/126848210-51fa71ee-30d9-49fa-bd1f-83672cd113aa.png)

After shopping, a user can check the order history of that location to see their order or see their own order history.

A user may also search for another user using the 'Search for Customer' link. They may search by first name, last name, username, or any combination of the three:
![search_for_rom](https://user-images.githubusercontent.com/85184581/126848312-b444ee6c-4073-4112-9282-41b958befb0f.png)

## License
This project uses the following license: MIT License
