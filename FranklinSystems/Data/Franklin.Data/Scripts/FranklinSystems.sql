/*
Adhoc queries.
*/
USE [FranklinSystems]

SELECT * FROM dbo.ClientOrder

-- Create relationships

ALTER TABLE [dbo].[OrderTransaction]  WITH CHECK ADD  CONSTRAINT [FK_OrderTransaction_BuyOrderId] FOREIGN KEY([BuyOrderId])
REFERENCES [dbo].[OrderBook] ([EntryId])
GO

ALTER TABLE [dbo].[OrderTransaction] CHECK CONSTRAINT [FK_OrderTransaction_BuyOrderId]
GO

--
ALTER TABLE [dbo].[OrderTransaction]  WITH CHECK ADD  CONSTRAINT [FK_OrderTransaction_SellOrderId] FOREIGN KEY([SellOrderId])
REFERENCES [dbo].[OrderBook] ([EntryId])
GO

ALTER TABLE [dbo].[OrderTransaction] CHECK CONSTRAINT [FK_OrderTransaction_SellOrderId]
GO

