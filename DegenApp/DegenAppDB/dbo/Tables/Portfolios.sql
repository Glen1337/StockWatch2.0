CREATE TABLE [dbo].[Portfolios] (
    [PortfolioId]      BIGINT         IDENTITY (1, 1) NOT NULL,
    [Title]            NVARCHAR (MAX) NOT NULL,
    [CreationDate]     DATETIME2 (7)  DEFAULT ('0001-01-01T00:00:00.0000000') NOT NULL,
    [GainLoss]         MONEY          DEFAULT ((0.0)) NOT NULL,
    [TotalMarketValue] MONEY          DEFAULT ((0.0)) NOT NULL,
    [Type]             NVARCHAR (MAX) NULL,
    [UserId]           NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Portfolios] PRIMARY KEY CLUSTERED ([PortfolioId] ASC)
);

