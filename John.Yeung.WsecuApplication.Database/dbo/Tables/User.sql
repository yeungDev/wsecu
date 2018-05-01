CREATE TABLE [dbo].[User] (
    [UserId]   INT          IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [UserName] VARCHAR (50) NOT NULL,
    [Email]    VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_User1] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

