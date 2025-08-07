เริ่มต้นการใช้งานจำเป็นสร้าง Database ตาม Connection String
จากนั้นสามารถ Run Script Sql นี้ได้เลยครับ

USE [ชื่อ database ที่ถูกสร้างขึ้น]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Demo](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DemoName] [nvarchar](50) NOT NULL,
	[DemoDescription] [nvarchar](500) NULL,
 CONSTRAINT [PK_tbl_Demo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[tbl_Demo] ON 
GO
INSERT [dbo].[tbl_Demo] ([ID], [DemoName], [DemoDescription]) VALUES (1, N'John Doe', N'Working on Demo Company')
GO
SET IDENTITY_INSERT [dbo].[tbl_Demo] OFF
GO


