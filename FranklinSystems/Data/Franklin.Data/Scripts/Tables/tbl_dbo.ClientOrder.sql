USE [FranklinSystems]
GO

/****** Object:  Table [dbo].[ClientOrder]    Script Date: 1/29/2021 2:23:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ClientOrder](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[SecurityId] [int] NOT NULL,
	[TraderId] [int] NOT NULL,
	[SideCode] [varchar](4) NOT NULL,
	[TypeCode] [varchar](4) NOT NULL,
	[Quantity] [int] NOT NULL,
	[FilledQuantity] [int] NOT NULL,
	[Price] [numeric](10, 2) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_ClientOrder] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


