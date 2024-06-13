-- ASPNETデータベースを使用する
USE [ASPNET]
GO

-- スタッフ情報（Staff）
DROP TABLE IF EXISTS [Staff];
CREATE TABLE [Staff]
(
	[StaffId] [int] IDENTITY(1,1) NOT NULL,
	[Account] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[DisplayName] [nvarchar](50) NOT NULL,
	[StaffType] [tinyint] NOT NULL DEFAULT 0,
	[UpdateDate] [datetime2](7) NOT NULL DEFAULT (getdate()),
	[UpdateStaffId] [int] NULL,
	[DeleteDate] [datetime2](7) DEFAULT NULL,
	CONSTRAINT [PK_Staff] PRIMARY KEY ( [StaffId] ASC ),
	CONSTRAINT [IX_Account_Unique] UNIQUE ( [Account] ASC ),
	CONSTRAINT [FK_UpdateStaffId_StaffId] FOREIGN KEY ( [UpdateStaffId] ) REFERENCES [Staff] ( [StaffId] )
);
GO

-- ルート管理者の情報を挿入する
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'admin', 'himitu', 'ルート管理者', 9, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff1', 'himitu1', 'スタッフ1', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff2', 'himitu2', 'スタッフ2', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff3', 'himitu3', 'スタッフ3', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff4', 'himitu4', 'スタッフ4', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff5', 'himitu5', 'スタッフ5', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff6', 'himitu6', 'スタッフ6', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff7', 'himitu7', 'スタッフ7', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff8', 'himitu8', 'スタッフ8', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff9', 'himitu9', 'スタッフ9', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff10', 'himitu10', 'スタッフ10', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff11', 'himitu11', 'スタッフ11', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'admin2', 'himitu2', '管理スタッフ2', 9, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff12', 'himitu12', 'スタッフ12', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff13', 'himitu13', 'スタッフ13', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff14', 'himitu14', 'スタッフ14', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff15', 'himitu15', 'スタッフ15', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff16', 'himitu16', 'スタッフ16', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff17', 'himitu17', 'スタッフ17', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff18', 'himitu18', 'スタッフ18', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff19', 'himitu19', 'スタッフ19', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff20', 'himitu20', 'スタッフ20', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff21', 'himitu21', 'スタッフ21', 0, DEFAULT, 1, DEFAULT );
INSERT INTO [Staff] ( [Account], [Password], [DisplayName], [StaffType], [UpdateDate], [UpdateStaffId], [DeleteDate] ) VALUES ( 'staff22', 'himitu22', 'スタッフ22', 0, DEFAULT, 1, DEFAULT );
GO