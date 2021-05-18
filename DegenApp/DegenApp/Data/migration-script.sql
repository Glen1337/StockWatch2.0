IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Portfolios] (
    [PortfolioId] bigint NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Portfolios] PRIMARY KEY ([PortfolioId])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201213195631_InitialCreate', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PortfolioId', N'Title') AND [object_id] = OBJECT_ID(N'[Portfolios]'))
    SET IDENTITY_INSERT [Portfolios] ON;
INSERT INTO [Portfolios] ([PortfolioId], [Title])
VALUES (CAST(1 AS bigint), N'User2''s portfolio');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PortfolioId', N'Title') AND [object_id] = OBJECT_ID(N'[Portfolios]'))
    SET IDENTITY_INSERT [Portfolios] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201213200755_InitialSeed', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201214035344_Add-Holding', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Portfolios] ADD [CreationDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
GO

ALTER TABLE [Portfolios] ADD [GainLoss] Money NOT NULL DEFAULT 0.0;
GO

ALTER TABLE [Portfolios] ADD [TotalMarketValue] Money NOT NULL DEFAULT 0.0;
GO

ALTER TABLE [Portfolios] ADD [Type] nvarchar(max) NULL;
GO

ALTER TABLE [Portfolios] ADD [UserId] nvarchar(max) NULL;
GO

CREATE TABLE [Holdings] (
    [HoldingId] bigint NOT NULL IDENTITY,
    [CostBasis] Money NOT NULL,
    [Quantity] float NOT NULL,
    [Symbol] nvarchar(8) NOT NULL,
    [TransactionDate] datetime2 NOT NULL,
    [Action] nvarchar(16) NOT NULL,
    [Type] nvarchar(8) NOT NULL,
    [ReinvestDivs] bit NOT NULL,
    [IsOpen] bit NOT NULL,
    [CurrentPrice] Money NOT NULL,
    [PortfolioId] bigint NOT NULL,
    [StrikePrice] decimal(18,2) NOT NULL,
    [ExpirationDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Holdings] PRIMARY KEY ([HoldingId]),
    CONSTRAINT [FK_Holdings_Portfolios_PortfolioId] FOREIGN KEY ([PortfolioId]) REFERENCES [Portfolios] ([PortfolioId]) ON DELETE CASCADE
);
GO

UPDATE [Portfolios] SET [CreationDate] = '2020-12-13T23:21:28.2621786-05:00', [GainLoss] = 768.0, [Title] = N'User1''s portfolio', [TotalMarketValue] = 1000.01, [Type] = N'Investing'
WHERE [PortfolioId] = CAST(1 AS bigint);
SELECT @@ROWCOUNT;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PortfolioId', N'CreationDate', N'GainLoss', N'Title', N'TotalMarketValue', N'Type', N'UserId') AND [object_id] = OBJECT_ID(N'[Portfolios]'))
    SET IDENTITY_INSERT [Portfolios] ON;
INSERT INTO [Portfolios] ([PortfolioId], [CreationDate], [GainLoss], [Title], [TotalMarketValue], [Type], [UserId])
VALUES (CAST(2 AS bigint), '2020-12-17T04:26:13.2645103-05:00', -324.67, N'User1''s Roth IRA Portfolio', 5204.99, N'Roth IRA', NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PortfolioId', N'CreationDate', N'GainLoss', N'Title', N'TotalMarketValue', N'Type', N'UserId') AND [object_id] = OBJECT_ID(N'[Portfolios]'))
    SET IDENTITY_INSERT [Portfolios] OFF;
GO

CREATE INDEX [IX_Holdings_PortfolioId] ON [Holdings] ([PortfolioId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201214042128_Add-Portfolio-and-seeds', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Holdings]') AND [c].[name] = N'StrikePrice');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Holdings] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Holdings] ALTER COLUMN [StrikePrice] Money NOT NULL;
GO

UPDATE [Portfolios] SET [CreationDate] = '2020-12-13T23:22:47.7086249-05:00'
WHERE [PortfolioId] = CAST(1 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Portfolios] SET [CreationDate] = '2020-12-17T04:27:32.7109492-05:00'
WHERE [PortfolioId] = CAST(2 AS bigint);
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201214042248_FixStrikeType', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

UPDATE [Portfolios] SET [CreationDate] = '2020-12-18T22:12:08.2576308-05:00'
WHERE [PortfolioId] = CAST(1 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Portfolios] SET [CreationDate] = '2020-12-22T03:16:53.2605401-05:00'
WHERE [PortfolioId] = CAST(2 AS bigint);
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201219031208_attributes', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [WatchItems] (
    [WatchItemId] bigint NOT NULL IDENTITY,
    [Symbol] nvarchar(8) NOT NULL,
    [UserId] nvarchar(max) NULL,
    CONSTRAINT [PK_WatchItems] PRIMARY KEY ([WatchItemId])
);
GO

UPDATE [Portfolios] SET [CreationDate] = '2020-12-19T16:01:23.4758630-05:00', [UserId] = N'1'
WHERE [PortfolioId] = CAST(1 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Portfolios] SET [CreationDate] = '2020-12-22T21:06:08.4782339-05:00', [UserId] = N'1'
WHERE [PortfolioId] = CAST(2 AS bigint);
SELECT @@ROWCOUNT;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PortfolioId', N'CreationDate', N'GainLoss', N'Title', N'TotalMarketValue', N'Type', N'UserId') AND [object_id] = OBJECT_ID(N'[Portfolios]'))
    SET IDENTITY_INSERT [Portfolios] ON;
INSERT INTO [Portfolios] ([PortfolioId], [CreationDate], [GainLoss], [Title], [TotalMarketValue], [Type], [UserId])
VALUES (CAST(3 AS bigint), '2021-01-11T23:24:29.4782454-05:00', 19874.73, N'User2''s Primary Portfolio', 52064.29, N'Speculation', N'2');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PortfolioId', N'CreationDate', N'GainLoss', N'Title', N'TotalMarketValue', N'Type', N'UserId') AND [object_id] = OBJECT_ID(N'[Portfolios]'))
    SET IDENTITY_INSERT [Portfolios] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201219210123_add-seeds-and-watchitem-key', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'HoldingId', N'Action', N'CostBasis', N'CurrentPrice', N'ExpirationDate', N'IsOpen', N'PortfolioId', N'Quantity', N'ReinvestDivs', N'StrikePrice', N'Symbol', N'TransactionDate', N'Type') AND [object_id] = OBJECT_ID(N'[Holdings]'))
    SET IDENTITY_INSERT [Holdings] ON;
INSERT INTO [Holdings] ([HoldingId], [Action], [CostBasis], [CurrentPrice], [ExpirationDate], [IsOpen], [PortfolioId], [Quantity], [ReinvestDivs], [StrikePrice], [Symbol], [TransactionDate], [Type])
VALUES (CAST(1 AS bigint), N'buy', 20.22, 0.0, '0001-01-01T00:00:00.0000000', CAST(0 AS bit), CAST(1 AS bigint), 1000.0E0, CAST(1 AS bit), 0.0, N'NKE', '2020-12-19T16:19:35.5256349-05:00', N'share'),
(CAST(2 AS bigint), N'buy', 17.98, 0.0, '0001-01-01T00:00:00.0000000', CAST(0 AS bit), CAST(1 AS bigint), 3000.0E0, CAST(0 AS bit), 0.0, N'SNAP', '2020-12-19T16:19:35.5257294-05:00', N'share'),
(CAST(3 AS bigint), N'buy', 20.22, 0.0, '0001-01-01T00:00:00.0000000', CAST(0 AS bit), CAST(2 AS bigint), 70.0E0, CAST(0 AS bit), 0.0, N'FSLR', '2020-12-19T16:19:35.5257323-05:00', N'share'),
(CAST(4 AS bigint), N'buy', 160.22, 0.0, '0001-01-01T00:00:00.0000000', CAST(0 AS bit), CAST(3 AS bigint), 800.0E0, CAST(1 AS bit), 0.0, N'SPOT', '2020-12-19T16:19:35.5257340-05:00', N'share');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'HoldingId', N'Action', N'CostBasis', N'CurrentPrice', N'ExpirationDate', N'IsOpen', N'PortfolioId', N'Quantity', N'ReinvestDivs', N'StrikePrice', N'Symbol', N'TransactionDate', N'Type') AND [object_id] = OBJECT_ID(N'[Holdings]'))
    SET IDENTITY_INSERT [Holdings] OFF;
GO

UPDATE [Portfolios] SET [CreationDate] = '2020-12-19T16:19:35.5229707-05:00'
WHERE [PortfolioId] = CAST(1 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Portfolios] SET [CreationDate] = '2020-12-22T21:24:20.5253985-05:00'
WHERE [PortfolioId] = CAST(2 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Portfolios] SET [CreationDate] = '2021-01-11T23:42:41.5254105-05:00'
WHERE [PortfolioId] = CAST(3 AS bigint);
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201219211935_add-holding-seeds', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

UPDATE [Holdings] SET [TransactionDate] = '2020-12-19T16:29:09.1126865-05:00'
WHERE [HoldingId] = CAST(1 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Holdings] SET [TransactionDate] = '2020-12-19T16:29:09.1142371-05:00'
WHERE [HoldingId] = CAST(2 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Holdings] SET [TransactionDate] = '2020-12-19T16:29:09.1142415-05:00'
WHERE [HoldingId] = CAST(3 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Holdings] SET [TransactionDate] = '2020-12-19T16:29:09.1142433-05:00'
WHERE [HoldingId] = CAST(4 AS bigint);
SELECT @@ROWCOUNT;

GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'HoldingId', N'Action', N'CostBasis', N'CurrentPrice', N'ExpirationDate', N'IsOpen', N'PortfolioId', N'Quantity', N'ReinvestDivs', N'StrikePrice', N'Symbol', N'TransactionDate', N'Type') AND [object_id] = OBJECT_ID(N'[Holdings]'))
    SET IDENTITY_INSERT [Holdings] ON;
INSERT INTO [Holdings] ([HoldingId], [Action], [CostBasis], [CurrentPrice], [ExpirationDate], [IsOpen], [PortfolioId], [Quantity], [ReinvestDivs], [StrikePrice], [Symbol], [TransactionDate], [Type])
VALUES (CAST(5 AS bigint), N'shortsell', 85.09, 0.0, '0001-01-01T00:00:00.0000000', CAST(0 AS bit), CAST(3 AS bigint), 500.0E0, CAST(0 AS bit), 0.0, N'ROKU', '2020-12-19T16:29:09.1142447-05:00', N'share');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'HoldingId', N'Action', N'CostBasis', N'CurrentPrice', N'ExpirationDate', N'IsOpen', N'PortfolioId', N'Quantity', N'ReinvestDivs', N'StrikePrice', N'Symbol', N'TransactionDate', N'Type') AND [object_id] = OBJECT_ID(N'[Holdings]'))
    SET IDENTITY_INSERT [Holdings] OFF;
GO

UPDATE [Portfolios] SET [CreationDate] = '2020-12-19T16:19:35.5226707-05:00'
WHERE [PortfolioId] = CAST(1 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Portfolios] SET [CreationDate] = '2020-12-19T16:19:35.5223249-05:00'
WHERE [PortfolioId] = CAST(2 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Portfolios] SET [CreationDate] = '2020-12-19T16:19:35.5222673-05:00'
WHERE [PortfolioId] = CAST(3 AS bigint);
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201219212909_add-holding-seed', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

UPDATE [Holdings] SET [TransactionDate] = '0001-01-01T00:00:00.0000000'
WHERE [HoldingId] = CAST(1 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Holdings] SET [TransactionDate] = '0001-01-01T00:00:00.0000000'
WHERE [HoldingId] = CAST(2 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Holdings] SET [TransactionDate] = '0001-01-01T00:00:00.0000000'
WHERE [HoldingId] = CAST(3 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Holdings] SET [TransactionDate] = '0001-01-01T00:00:00.0000000'
WHERE [HoldingId] = CAST(4 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Holdings] SET [TransactionDate] = '0001-01-01T00:00:00.0000000'
WHERE [HoldingId] = CAST(5 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Portfolios] SET [CreationDate] = '0001-01-01T00:00:00.0000000'
WHERE [PortfolioId] = CAST(1 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Portfolios] SET [CreationDate] = '0001-01-01T00:00:00.0000000'
WHERE [PortfolioId] = CAST(2 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Portfolios] SET [CreationDate] = '0001-01-01T00:00:00.0000000'
WHERE [PortfolioId] = CAST(3 AS bigint);
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201219213103_fix-dates-holding-seed', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Holdings]') AND [c].[name] = N'Action');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Holdings] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Holdings] DROP COLUMN [Action];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Holdings]') AND [c].[name] = N'Type');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Holdings] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Holdings] DROP COLUMN [Type];
GO

ALTER TABLE [Holdings] ADD [OrderType] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [Holdings] ADD [SecurityType] int NOT NULL DEFAULT 0;
GO

CREATE TABLE [Orders] (
    [OrderId] bigint NOT NULL IDENTITY,
    [Action] nvarchar(16) NOT NULL,
    [TransactionDate] datetime2 NOT NULL,
    [Quantity] float NOT NULL,
    [Price] Money NOT NULL,
    [PortfolioId] bigint NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderId]),
    CONSTRAINT [FK_Orders_Portfolios_PortfolioId] FOREIGN KEY ([PortfolioId]) REFERENCES [Portfolios] ([PortfolioId]) ON DELETE CASCADE
);
GO

UPDATE [Holdings] SET [SecurityType] = 2
WHERE [HoldingId] = CAST(1 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Holdings] SET [SecurityType] = 2
WHERE [HoldingId] = CAST(2 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Holdings] SET [SecurityType] = 2
WHERE [HoldingId] = CAST(3 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Holdings] SET [SecurityType] = 2
WHERE [HoldingId] = CAST(4 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Holdings] SET [SecurityType] = 2
WHERE [HoldingId] = CAST(5 AS bigint);
SELECT @@ROWCOUNT;

GO

CREATE INDEX [IX_Orders_PortfolioId] ON [Orders] ([PortfolioId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201220080454_switch-from-strings-to-attributes-in-holdings', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Holdings]') AND [c].[name] = N'SecurityType');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Holdings] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Holdings] ALTER COLUMN [SecurityType] nvarchar(8) NOT NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Holdings]') AND [c].[name] = N'OrderType');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Holdings] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Holdings] ALTER COLUMN [OrderType] nvarchar(16) NOT NULL;
GO

UPDATE [Holdings] SET [OrderType] = N'Buy', [SecurityType] = N'Share'
WHERE [HoldingId] = CAST(1 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Holdings] SET [OrderType] = N'Buy', [SecurityType] = N'Share'
WHERE [HoldingId] = CAST(2 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Holdings] SET [OrderType] = N'Buy', [SecurityType] = N'Share'
WHERE [HoldingId] = CAST(3 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Holdings] SET [OrderType] = N'Buy', [SecurityType] = N'Share'
WHERE [HoldingId] = CAST(4 AS bigint);
SELECT @@ROWCOUNT;

GO

UPDATE [Holdings] SET [OrderType] = N'Buy', [SecurityType] = N'Share'
WHERE [HoldingId] = CAST(5 AS bigint);
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201220083156_store-enums-as-strings', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [WatchItems] ADD [Outlook] int NOT NULL DEFAULT 0;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Holdings]') AND [c].[name] = N'Symbol');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Holdings] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Holdings] ALTER COLUMN [Symbol] nvarchar(16) NOT NULL;
GO

UPDATE [Portfolios] SET [TotalMarketValue] = 0.0
WHERE [PortfolioId] = CAST(1 AS bigint);
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20201227052629_add-seeds', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Orders]') AND [c].[name] = N'Action');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Orders] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Orders] DROP COLUMN [Action];
GO

ALTER TABLE [Orders] ADD [UserId] nvarchar(max) NOT NULL DEFAULT N'';
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Holdings]') AND [c].[name] = N'SecurityType');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Holdings] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Holdings] ALTER COLUMN [SecurityType] nvarchar(max) NOT NULL;
GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Holdings]') AND [c].[name] = N'OrderType');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Holdings] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [Holdings] ALTER COLUMN [OrderType] nvarchar(max) NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210103200149_order-table', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Orders] ADD [OrderType] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [Orders] ADD [SecurityType] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210103213219_make-order-fields-public', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Orders] ADD [Symbol] nvarchar(max) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210103225545_add-symbol-to-orders', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[WatchItems]') AND [c].[name] = N'UserId');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [WatchItems] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [WatchItems] ALTER COLUMN [UserId] nvarchar(max) NOT NULL;
ALTER TABLE [WatchItems] ADD DEFAULT N'' FOR [UserId];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210111080712_ignore-userid-watchitem', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[WatchItems]') AND [c].[name] = N'UserId');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [WatchItems] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [WatchItems] ALTER COLUMN [UserId] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210111081842_remove-required-userid-watchitem', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[WatchItems]') AND [c].[name] = N'Outlook');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [WatchItems] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [WatchItems] ALTER COLUMN [Outlook] nvarchar(max) NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210112044006_store-outlook-enum-as-string', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Users] (
    [UserId] nvarchar(450) NOT NULL,
    [Balance] Money NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210114045634_add-users-with-balance', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Holdings] ADD [ContractName] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210116055221_add-fields-for-options', N'5.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [WatchItems] ADD [PercentChange] decimal(12,4) NOT NULL DEFAULT 0.0;
GO

ALTER TABLE [WatchItems] ADD [PriceChange] decimal(12,4) NOT NULL DEFAULT 0.0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210418173904_addPricePercentChangeToWatchItem', N'5.0.5');
GO

COMMIT;
GO

