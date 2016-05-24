DROP TABLE [dbo].[TrelloCardMapping]
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TrelloCardMapping](
	[CompanyID] [int] NOT NULL DEFAULT ((0)),
	[BoardID] [int] NOT NULL,
	[RefNoteID] [uniqueidentifier] NOT NULL,
	[TrelloCardID] [nvarchar](30) NULL, --If the value is null it means that trello was not available on creation.
	[tstamp] [timestamp] NOT NULL,
	[CreatedByID] [uniqueidentifier] NOT NULL,
	[CreatedByScreenID] [char](8) NOT NULL,
	[CreatedDateTime] [datetime] NOT NULL,
	[LastModifiedByID] [uniqueidentifier] NOT NULL,
	[LastModifiedByScreenID] [char](8) NOT NULL,
	[LastModifiedDateTime] [datetime] NOT NULL,
 CONSTRAINT [TrelloCardMapping_PK] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC,
	[BoardID] ASC,
	[RefNoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


