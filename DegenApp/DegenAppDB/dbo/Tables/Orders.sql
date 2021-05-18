CREATE TABLE [dbo].[Orders] (
    [OrderId]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [TransactionDate] DATETIME2 (7)  NOT NULL,
    [Quantity]        FLOAT (53)     NOT NULL,
    [Price]           MONEY          NOT NULL,
    [PortfolioId]     BIGINT         NOT NULL,
    [UserId]          NVARCHAR (MAX) DEFAULT (N'') NOT NULL,
    [OrderType]       INT            DEFAULT ((0)) NOT NULL,
    [SecurityType]    INT            DEFAULT ((0)) NOT NULL,
    [Symbol]          NVARCHAR (MAX) DEFAULT (N'') NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([OrderId] ASC),
    CONSTRAINT [FK_Orders_Portfolios_PortfolioId] FOREIGN KEY ([PortfolioId]) REFERENCES [dbo].[Portfolios] ([PortfolioId]) ON DELETE CASCADE
);

