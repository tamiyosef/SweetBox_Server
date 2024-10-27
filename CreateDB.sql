﻿Use master
Go
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'SweetBox_DB')
BEGIN
    DROP DATABASE SweetBox_DB;
END
Go
Create Database SweetBox_DB
Go
Use SweetBox_DB
Go

CREATE TABLE Users
(
    UserId INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    UserType NVARCHAR(10) NOT NULL, -- 'seller', 'buyer', 'admin'
    DateCreated DATETIME DEFAULT GETDATE()
);

CREATE TABLE Sellers
(
    SellerId INT PRIMARY KEY, -- המפתח הראשי הוא UserId, מקושר ל-Users
    BusinessName NVARCHAR(100) NOT NULL, -- שם העסק
    BusinessAddress NVARCHAR(255) NOT NULL, -- כתובת העסק
    BusinessPhone NVARCHAR(20) NULL, -- טלפון העסק
    ProfilePicture NVARCHAR(255) NULL, -- URL or path to profile image
    Description NVARCHAR(255) NULL, -- תיאור קצר על העסק
    CONSTRAINT FK_Sellers_Users FOREIGN KEY (SellerId) REFERENCES Users(UserId) -- קישור ל-UserId בטבלת Users);
 );

CREATE TABLE Buyers
(
    BuyerId INT PRIMARY KEY, -- המפתח הראשי הוא UserId, מקושר ל-Users
    ShippingAddress NVARCHAR(255) NULL, -- כתובת למשלוח
    PhoneNumber NVARCHAR(20) NULL, -- טלפון הקונה
    FOREIGN KEY (BuyerId) REFERENCES Users(UserId) -- קישור ל-UserId בטבלת Users
);

CREATE TABLE Admins
(
    UserId INT PRIMARY KEY, -- המפתח הראשי הוא UserId, מקושר ל-Users
    AdminLevel INT NOT NULL DEFAULT 1, -- רמת הרשאה (למשל 1 היא ברירת מחדל)
    FOREIGN KEY (UserId) REFERENCES Users(UserId) -- קישור ל-UserId בטבלת Users
);

CREATE TABLE Products
(
    ProductId INT PRIMARY KEY IDENTITY, -- המפתח הראשי של המוצר
    SellerId INT FOREIGN KEY REFERENCES Sellers(SellerId), -- קישור למוכר בטבלת Sellers
    ProductName NVARCHAR(100) NOT NULL, -- שם המוצר
    Description NVARCHAR(255) NULL, -- תיאור המוצר
    Price DECIMAL(10, 2) NOT NULL, -- מחיר המוצר
    ImageUrl NVARCHAR(255) NULL, -- כתובת תמונה של המוצר
    DateAdded DATETIME DEFAULT GETDATE(), -- תאריך הוספת המוצר
    IsAvailable BIT NOT NULL DEFAULT 1 -- האם המוצר זמין למכירה
);


CREATE TABLE Orders
(
    OrderId INT PRIMARY KEY IDENTITY, -- המפתח הראשי של ההזמנה
    BuyerId INT FOREIGN KEY REFERENCES Buyers(BuyerId), -- קישור לקונה בטבלת Buyers
    OrderDate DATETIME DEFAULT GETDATE(), -- תאריך ביצוע ההזמנה
    TotalAmount DECIMAL(10, 2) NOT NULL, -- סכום כולל של ההזמנה
    OrderStatus NVARCHAR(50) NOT NULL, -- מצב ההזמנה (למשל: 'Pending', 'Shipped', 'Delivered')
    ShippingAddress NVARCHAR(255) NOT NULL -- כתובת משלוח
);

CREATE TABLE OrderItems
(
    OrderItemId INT PRIMARY KEY IDENTITY, -- המפתח הראשי של פריט בהזמנה
    OrderId INT FOREIGN KEY REFERENCES Orders(OrderId), -- קישור להזמנה
    ProductId INT FOREIGN KEY REFERENCES Products(ProductId), -- קישור למוצר
    Quantity INT NOT NULL, -- כמות המוצר בהזמנה
    PriceAtPurchase DECIMAL(10, 2) NOT NULL -- המחיר בזמן הרכישה
);


-- Create a login for the admin user
CREATE LOGIN [SweetBoxAdminLogin] WITH PASSWORD = 'kukuPassword';
Go

-- Create a user in the TamiDB database for the login
CREATE USER [SweetBoxAdminUser] FOR LOGIN [SweetBoxAdminLogin];
Go

-- Add the user to the db_owner role to grant admin privileges
ALTER ROLE db_owner ADD MEMBER [SweetBoxAdminUser];
Go

--EF Code
/*
scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=SweetBox_DB;User ID=SweetBoxAdminLogin;Password=kukuPassword;
" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context SweetBoxDBContext -DataAnnotations -force
*/


-- הוספת מוכר
INSERT INTO Users (FullName, Email, Password, UserType)
VALUES ('Alice Bakery', 'alicebakery@gmail.com', 'password123', '2');

-- הוספת מוכר
INSERT INTO Users (FullName, Email, Password, UserType)
VALUES ('Bob Cakes', 'bobcakes@gmail.com', 'password456', '2');


--ADMIN
insert Into Users (FullName, Email, Password, UserType)
values('Tami Yosef','tamiyosef97@gmail.com',1234,1)

-- שימוש ב-UserId של המוכר Alice
INSERT INTO Sellers (SellerId, BusinessName, BusinessAddress, BusinessPhone, Description, ProfilePicture)
VALUES (1, 'Alice Bakery', '123 Cake Street, SweetTown', '555-1234', 'We bake the best cakes in town.', '/images/alice_profile.png');

-- שימוש ב-UserId של המוכר Bob
INSERT INTO Sellers (SellerId, BusinessName, BusinessAddress, BusinessPhone, Description, ProfilePicture)
VALUES (2, 'Bob Cakes', '456 Cookie Ave, BakeCity', '555-5678', 'Delicious cakes and cookies.', '/images/bob_profile.png');


select * from Users
select * from Sellers


INSERT INTO Products (SellerId, ProductName, Description, Price, ImageUrl)
VALUES (1, 'Cheesecake', 'Delicious creamy cheesecake with a graham cracker crust.', 25.00, '/images/cheesecake.png');

-- הוספת עוגת שוקולד
INSERT INTO Products (SellerId, ProductName, Description, Price, ImageUrl)
VALUES (1, 'Chocolate Cake', 'Rich and moist chocolate cake with dark chocolate ganache.', 30.00, '/images/chocolate_cake.png');

-- הוספת מאפי קינמון
INSERT INTO Products (SellerId, ProductName, Description, Price, ImageUrl)
VALUES (2, 'Cinnamon Rolls', 'Soft and gooey cinnamon rolls with cream cheese icing.', 15.00, '/images/cinnamon_rolls.png');

-- הוספת טארט פירות
INSERT INTO Products (SellerId, ProductName, Description, Price, ImageUrl)
VALUES (2, 'Fruit Tart', 'Fresh fruit tart with a buttery crust and vanilla cream.', 35.00, '/images/fruit_tart.png');

select * from Products
INSERT INTO Users (FullName, Email, Password, UserType)
VALUES ('Charlie Buyer', 'charliebuyer@gmail.com', '6789', '3')

