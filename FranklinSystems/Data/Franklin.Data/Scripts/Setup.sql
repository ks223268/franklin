/*
Setup scripts
*/


-- Create relationships

-- All orders should link to the original client order.

ALTER TABLE [dbo].[OrderTransaction]  WITH CHECK ADD  CONSTRAINT [FK_OrderTransaction_BuyOrderId] FOREIGN KEY([BuyOrderId])
REFERENCES [dbo].[ClientOrder] ([OrderId])
GO

ALTER TABLE [dbo].[OrderTransaction] CHECK CONSTRAINT [FK_OrderTransaction_BuyOrderId]
GO

--
ALTER TABLE [dbo].[OrderTransaction]  WITH CHECK ADD  CONSTRAINT [FK_OrderTransaction_SellOrderId] FOREIGN KEY([SellOrderId])
REFERENCES [dbo].[ClientOrder] ([OrderId])
GO

ALTER TABLE [dbo].[OrderTransaction] CHECK CONSTRAINT [FK_OrderTransaction_SellOrderId]
GO

--
ALTER TABLE [dbo].[OrderBook]  WITH CHECK ADD  CONSTRAINT [FK_OrderBook_BuyOrderId] FOREIGN KEY([BuyOrderId])
REFERENCES [dbo].[ClientOrder] ([OrderId])
GO

ALTER TABLE [dbo].[OrderTransaction] CHECK CONSTRAINT [FK_OrderTransaction_BuyOrderId]
GO

--
ALTER TABLE [dbo].[OrderTransaction]  WITH CHECK ADD  CONSTRAINT [FK_OrderTransaction_SellOrderId] FOREIGN KEY([SellOrderId])
REFERENCES [dbo].[ClientOrder] ([OrderId])
GO

ALTER TABLE [dbo].[OrderTransaction] CHECK CONSTRAINT [FK_OrderTransaction_SellOrderId]
GO