CREATE TABLE [dbo].[Users]
(
	[Id] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
	[UserName] [nvarchar](50) NULL,
	[UserGuid] [uniqueidentifier] NULL,
	[User1cTNumber] [int] NULL,
	[UserFirstName] [nvarchar](50) NULL,
	[UserSecondName] [nvarchar](50) NULL,
	[UserMiddleName] [nvarchar](50) NULL,
	[UserStatusId] [int] NULL,
	[UserPositionId] [int] NULL,
	[UserPassword] [nvarchar](50) NULL,
	[UserLocationId] [int] NULL,
	[UserIsDeleted] [bit] NOT NULL,
	[UserDivisionId] [int] NULL,
	[UserBarcodeId] [int] NOT NULL,
	[UserRNN] [bigint] NULL,
	[Category] [binary](16) NULL,
	[StartWorkDate] [datetime] NULL,
	[IDRRef] [binary](16) NULL,
	[PasswordChangeDate] [datetime] NULL,
	[ChangPassPeriod] [int] NULL,
	[UserSulCoin] [decimal](18, 2) NULL,
	[UserSpentSulcoin] [decimal](18, 2) NULL,
	[Sex] [nvarchar](50) NULL,
	[DateOfBirth] [nvarchar](50) NULL,
	[Education] [nvarchar](max) NULL,
	[UserCat] [nvarchar](50) NULL
)
GO
CREATE NONCLUSTERED INDEX [IX_Users_UserName]
    ON [dbo].[Users]([UserName] ASC);