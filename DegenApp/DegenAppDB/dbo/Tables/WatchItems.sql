CREATE TABLE [dbo].[WatchItems] (
    [WatchItemId]   BIGINT          IDENTITY (1, 1) NOT NULL,
    [Symbol]        NVARCHAR (8)    NOT NULL,
    [UserId]        NVARCHAR (MAX)  NULL,
    [Outlook]       NVARCHAR (MAX)  NOT NULL,
    [PercentChange] DECIMAL (12, 4) DEFAULT ((0.0)) NOT NULL,
    [PriceChange]   DECIMAL (12, 4) DEFAULT ((0.0)) NOT NULL,
    CONSTRAINT [PK_WatchItems] PRIMARY KEY CLUSTERED ([WatchItemId] ASC)
);

