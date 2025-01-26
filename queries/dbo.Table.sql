CREATE TABLE Users (
    UserId INT IDENTITY PRIMARY KEY,  -- Primary key with auto-increment
    Username NVARCHAR(100) UNIQUE NOT NULL,  -- Unique username
    PasswordHash NVARCHAR(255) NOT NULL,  -- Store hashed password
    DateCreated DATETIME DEFAULT GETDATE()  -- When the user was created
);
