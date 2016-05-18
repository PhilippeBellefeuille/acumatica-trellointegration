DROP TABLE [dbo].[TrelloSetup]
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TrelloSetup](
	[CompanyID] [int] NOT NULL DEFAULT ((0)),
	[TrelloUsrToken] [nvarchar](100) NULL,
	[TrelloOrganizationID] [nvarchar](50) NULL,
	[ConnectionDateTime] [datetime] NULL,
	[UserName] [nvarchar](30) NULL,
	[tstamp] [timestamp] NOT NULL,
	[CreatedByID] [uniqueidentifier] NOT NULL,
	[CreatedByScreenID] [char](8) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[LastModifiedByID] [uniqueidentifier] NOT NULL,
	[LastModifiedByScreenID] [char](8) NOT NULL,
	[LastModifiedDateTime] [datetime] NOT NULL,
 CONSTRAINT [TrelloSetup_PK] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


