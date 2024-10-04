You have to setup your Sql Server For Database , Since it's on local Sql Server not Remote
after Setup Database 
Add Following Store Procedure :
//GetUsers
CREATE PROCEDURE sp_GetUsers
AS
BEGIN
    SELECT * FROM UserDto_New;
END

//GetUserById
CREATE PROCEDURE sp_GetUserById
    @Id INT
AS
BEGIN
    SELECT * FROM UserDto_New WHERE Id = @Id;
END

// Add USer
CREATE PROCEDURE sp_add_custom_user
    @Email NVARCHAR(255),
    @FirstName NVARCHAR(255),
    @LastName NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Insert the new user into the UserDto_New table
    INSERT INTO UserDto_New (Email, FirstName, LastName)
    VALUES (@Email, @FirstName, @LastName);
END

//Update USer 
CREATE PROCEDURE sp_UpdateUser
    @Id INT,
    @Email NVARCHAR(255),
    @FirstName NVARCHAR(255),
    @LastName NVARCHAR(255)
AS
BEGIN
    UPDATE UserDto_New 
    SET Email = @Email, FirstName = @FirstName, LastName = @LastName 
    WHERE Id = @Id;
END

//Delete User
CREATE PROCEDURE sp_DeleteUser
    @Id INT
AS
BEGIN
    DELETE FROM UserDto_New WHERE Id = @Id;
END

