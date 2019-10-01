INSERT INTO dbo.[Currency]
([Name], [State]) VALUES
(N'HUF', N'T');

INSERT INTO dbo.[Currency]
([Name], [STATE]) VALUES
(N'EUR', N'T');

INSERT INTO dbo.[Transaction] 
([AccountingDate], [TransactionId], [Type], [Account], [AccountName], [PartnerAccount], [PartnerName], [Sum], [CurrencyId], [Message], [State]) VALUES
('2016-05-31', N'Y0406-G99810-101', N'Csomagdíj', N'104040657157575649481012', N'SZŐCS ÁRON', NULL, NULL, -66.00, 1, NULL, N'T');

INSERT INTO dbo.[Transaction] 
([AccountingDate], [TransactionId], [Type], [Account], [AccountName], [PartnerAccount], [PartnerName], [Sum], [CurrencyId], [Message], [State]) VALUES
('2016-05-27', N'440602******4503', N'Készpénzfelvét K&H ATM-ből', N'104040657157575649481012', N'SZŐCS ÁRON', NULL, N'SZENTMIHALYI UT 131.', -20000.00, 1, NULL, N'T');