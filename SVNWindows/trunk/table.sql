GO
CREATE TABLE [dbo].[System_Account] (
[UserName] varchar(255) NOT NULL ,
[Password] varchar(255) NULL ,
[ID] varchar(36) NOT NULL DEFAULT (newid()) ,
[IsDomainAccount] int NOT NULL DEFAULT ((1)) ,
[Description] varchar(255) NULL ,
[IsUse] int NULL ,
[RealName] varchar(255) NULL ,
[Picture] text NULL 
)
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'System_Account', 
'COLUMN', N'IsDomainAccount')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'是否域账号'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'System_Account'
, @level2type = 'COLUMN', @level2name = N'IsDomainAccount'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否域账号'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'System_Account'
, @level2type = 'COLUMN', @level2name = N'IsDomainAccount'
GO
CREATE TABLE [dbo].[SVN_User] (
[ID] varchar(36) NOT NULL DEFAULT (newid()) ,
[DomainAccount] varchar(255) NOT NULL ,
[RealName] nvarchar(255) NOT NULL ,
[Description] nvarchar(255) NULL ,
[Password] varchar(255) NULL ,
[IsUse] int NULL 
)
GO
ALTER TABLE [dbo].[SVN_User] ADD PRIMARY KEY ([ID])
GO
CREATE TABLE [dbo].[SVN_Project] (
[ID] varchar(36) NOT NULL DEFAULT (newid()) ,
[Name] nvarchar(255) NOT NULL ,
[Url] text NOT NULL ,
[Type] int NOT NULL ,
[Description] varchar(255) NOT NULL ,
[Head] varchar(255) NOT NULL ,
[CreateTime] datetime NOT NULL DEFAULT (getdate()) ,
[City] nvarchar(255) NOT NULL ,
[ParentID] varchar(36) NULL ,
[UserID] varchar(36) NULL DEFAULT ('30A102FA-7885-4C1B-BC97-2A45A7D2589A') ,
[IsUse] int NOT NULL DEFAULT ((0)) ,
[Creator] varchar(255) NULL 
)
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'SVN_Project', 
'COLUMN', N'Type')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'1源代码，2产品文档'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'SVN_Project'
, @level2type = 'COLUMN', @level2name = N'Type'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'1源代码，2产品文档'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'SVN_Project'
, @level2type = 'COLUMN', @level2name = N'Type'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'SVN_Project', 
'COLUMN', N'UserID')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'使用哪个svn账号获取日志，这个账号需要有密码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'SVN_Project'
, @level2type = 'COLUMN', @level2name = N'UserID'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'使用哪个svn账号获取日志，这个账号需要有密码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'SVN_Project'
, @level2type = 'COLUMN', @level2name = N'UserID'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'SVN_Project', 
'COLUMN', N'IsUse')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'是否使用，1使用，0禁用'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'SVN_Project'
, @level2type = 'COLUMN', @level2name = N'IsUse'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否使用，1使用，0禁用'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'SVN_Project'
, @level2type = 'COLUMN', @level2name = N'IsUse'
GO
ALTER TABLE [dbo].[SVN_Project] ADD PRIMARY KEY ([ID])
GO
CREATE TABLE [dbo].[SVN_Log] (
[ProjectID] varchar(36) NOT NULL ,
[UserID] varchar(36) NOT NULL ,
[CreateTime] datetime NOT NULL ,
[Revision] int NOT NULL ,
[Message] text NULL ,
[ID] varchar(36) NOT NULL DEFAULT (newid()) 
)
GO
ALTER TABLE [dbo].[SVN_Log] ADD PRIMARY KEY ([ID])
GO
CREATE TABLE [dbo].[SVN_LogFile] (
[ID] varchar(36) NOT NULL DEFAULT (newid()) ,
[LogID] varchar(36) NOT NULL ,
[Path] text NULL ,
[Action] varchar(20) NULL 
)
GO
ALTER TABLE [dbo].[SVN_LogFile] ADD PRIMARY KEY ([ID])
GO
CREATE TABLE [dbo].[SVN_ProjectRelation] (
[ChildID] varchar(36) NOT NULL ,
[ParentID] char(36) NULL ,
[ID] varchar(36) NOT NULL DEFAULT (newid()) 
)
GO
CREATE TABLE [dbo].[SVN_RevisionPublish] (
[ID] varchar(36) NOT NULL DEFAULT (newid()) ,
[LogID] varchar(36) NOT NULL ,
[JenkinsID] varchar(36) NOT NULL ,
[CreateTime] datetime NULL DEFAULT (getdate()) ,
[Description] varchar(255) NULL ,
[SystemAccountID] varchar(36) NULL 
)
GO
