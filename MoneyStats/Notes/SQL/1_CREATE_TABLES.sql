CREATE TABLE [dbo].[User] (
	[Id] int IDENTITY(1, 1) NOT NULL,
	[Username] nvarchar(255) NOT NULL,
	
	[ModifiedBy] int NULL,
	[ModifiedDate] datetime NULL,
	[CreateBy] int NULL,
	[CreateDate] datetime NULL,
	[State] varchar(1) NOT NULL,

	CONSTRAINT [PK_USER_ID] PRIMARY KEY NONCLUSTERED ([Id])
)

CREATE TABLE [dbo].[Setting] (
	[Id] int IDENTITY(1, 1) NOT NULL,
	[UserId] int NOT NULL,
	[MergedFileFolderPath] nvarchar(500) NULL, -- C:\bank\merged
	[MergedFilename] nvarchar(500) NULL, -- merged
	[OriginalFileFolderPath] nvarchar(500) NULL, -- C:\bank
	[OriginalFileExtensionPattern] nvarchar(500) NULL, -- *.xlsx
	[DBExportFolderPath] nvarchar(500) NULL, -- C:\bank\exported
	[DBExportFilename] nvarchar(500) NULL, -- exportedFile
	[DBImportFilePath] nvarchar(500) NULL, -- C:\bank\exported\exportedFile.xlsx
	
	[ModifiedBy] int NULL,
	[ModifiedDate] datetime NULL,
	[CreateBy] int NULL,
	[CreateDate] datetime NULL,
	[State] varchar(1) NOT NULL,

	CONSTRAINT [PK_SETTI_ID] PRIMARY KEY NONCLUSTERED ([Id]),
	CONSTRAINT [FK_USER_ID] FOREIGN KEY ([UserId]) 
		REFERENCES [dbo].[User] ([Id]) 
		ON UPDATE NO ACTION
		ON DELETE NO ACTION
)


CREATE TABLE [dbo].[Currency] (
	[Id] int IDENTITY(1, 1) NOT NULL,
	[Name] nvarchar(255) NOT NULL,
	
	[ModifiedBy] int NULL,
	[ModifiedDate] datetime NULL,
	[CreateBy] int NULL,
	[CreateDate] datetime NULL,
	[State] varchar(1) NOT NULL,
	
	CONSTRAINT [PK_CURR_ID] PRIMARY KEY NONCLUSTERED ([Id])
)


CREATE TABLE [dbo].[Transaction] (
	[Id] int IDENTITY(1, 1) NOT NULL,
	[AccountingDate] datetime NOT NULL,
	[TransactionId] nvarchar(255) NULL,
	[Type] nvarchar(255) NULL,
	[Account] nvarchar(255) NULL,
	[AccountName] nvarchar(255) NULL,
	[PartnerAccount] nvarchar(255) NULL,
	[PartnerName] nvarchar(255) NULL,
	[Sum] numeric(15, 2) NULL,
	[CurrencyId] int NULL,
	[Message] nvarchar(500) NULL,
	
	[ModifiedBy] int NULL,
	[ModifiedDate] datetime NULL,
	[CreateBy] int NULL,
	[CreateDate] datetime NULL,
	[State] varchar(1) NOT NULL,
	
	CONSTRAINT [PK_TRAN_ID] PRIMARY KEY NONCLUSTERED ([Id]),
	CONSTRAINT [FK_TRAN_CURRENCY_ID] FOREIGN KEY ([CurrencyId]) 
		REFERENCES [dbo].[Currency] ([Id]) 
		ON UPDATE NO ACTION
		ON DELETE NO ACTION
)


CREATE TABLE [dbo].[Tag] (
	[Id] int IDENTITY(1, 1) NOT NULL,
	[Title] varchar(255) NOT NULL,
	[Description] varchar(255) NULL,
	
	[ModifiedBy] int NULL,
	[ModifiedDate] datetime NULL,
	[CreateBy] int NULL,
	[CreateDate] datetime NULL,
	[State] varchar(1) NOT NULL,

	CONSTRAINT [PK_TAGT_ID] PRIMARY KEY NONCLUSTERED ([Id])
)


CREATE TABLE [dbo].[TransactionTagConn] (
	[Id] int IDENTITY(1, 1) NOT NULL,
	[TransactionId] int NOT NULL,
	[TagId] int NOT NULL,

	CONSTRAINT [PK_TTCT_ID] PRIMARY KEY NONCLUSTERED ([Id]),
	CONSTRAINT [FK_TRAN_ID] FOREIGN KEY ([TransactionId]) 
		REFERENCES [dbo].[Transaction] ([Id]) 
		ON UPDATE NO ACTION
		ON DELETE NO ACTION,
	CONSTRAINT [FK_TAG_ID] FOREIGN KEY ([TagId]) 
		REFERENCES [dbo].[Tag] ([Id]) 
		ON UPDATE NO ACTION
		ON DELETE NO ACTION
)



