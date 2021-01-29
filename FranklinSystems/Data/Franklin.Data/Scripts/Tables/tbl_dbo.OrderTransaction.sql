USE [FranklinSystems]
GO

/****** Object:  Table [dbo].[OrderTransaction]    Script Date: 1/29/2021 2:26:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderTransaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BuyOrderId] [int] NOT NULL,
	[SellOrderId] [int] NOT NULL,
	[QuantityFilled] [int] NOT NULL,
	[MatchedPrice] [numeric](10, 2) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_OrderTransaction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OrderTransaction]  WITH CHECK ADD  CONSTRAINT [FK_OrderTransaction_BuyOrderId] FOREIGN KEY([BuyOrderId])
REFERENCES [dbo].[ClientOrder] ([OrderId])
GO

ALTER TABLE [dbo].[OrderTransaction] CHECK CONSTRAINT [FK_OrderTransaction_BuyOrderId]
GO

ALTER TABLE [dbo].[OrderTransaction]  WITH CHECK ADD  CONSTRAINT [FK_OrderTransaction_SellOrderId] FOREIGN KEY([SellOrderId])
REFERENCES [dbo].[ClientOrder] ([OrderId])
GO

ALTER TABLE [dbo].[OrderTransaction] CHECK CONSTRAINT [FK_OrderTransaction_SellOrderId]
GO


