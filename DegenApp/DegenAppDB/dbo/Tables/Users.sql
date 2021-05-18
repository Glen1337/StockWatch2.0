CREATE TABLE [dbo].[Users] (
    [UserId]  NVARCHAR (450) NOT NULL,
    [Balance] MONEY          NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

