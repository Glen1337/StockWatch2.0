CREATE TABLE [dbo].[Holdings] (
    [HoldingId]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [CostBasis]       MONEY          NOT NULL,
    [Quantity]        FLOAT (53)     NOT NULL,
    [Symbol]          NVARCHAR (16)  NOT NULL,
    [TransactionDate] DATETIME2 (7)  NOT NULL,
    [ReinvestDivs]    BIT            NOT NULL,
    [IsOpen]          BIT            NOT NULL,
    [CurrentPrice]    MONEY          NOT NULL,
    [PortfolioId]     BIGINT         NOT NULL,
    [StrikePrice]     MONEY          NOT NULL,
    [ExpirationDate]  DATETIME2 (7)  NOT NULL,
    [OrderType]       NVARCHAR (MAX) NOT NULL,
    [SecurityType]    NVARCHAR (MAX) NOT NULL,
    [ContractName]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Holdings] PRIMARY KEY CLUSTERED ([HoldingId] ASC),
    CONSTRAINT [FK_Holdings_Portfolios_PortfolioId] FOREIGN KEY ([PortfolioId]) REFERENCES [dbo].[Portfolios] ([PortfolioId]) ON DELETE CASCADE
);

