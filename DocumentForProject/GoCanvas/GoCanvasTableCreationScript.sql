
--ALTER TABLE [dbo].[Submission_Section2_ResourceGroup] 
--ALTER COLUMN [ResGrp_Section_Screen_Response_Value] varchar(500);

Select * From [dbo].[FormData]--3404
Select * From [dbo].[SubmissionsData]
Select * From [dbo].[Submission_Section1] --Where SubmissionId = 133162178
Select * From [dbo].[Submission_Section2] --Where SubmissionId = 133162178
Select * From [dbo].[Submission_Section2_ResourceGroup] --Where SubmissionId = 133162178
Select * From [dbo].[ImageData] Where SubmissionId = 132296639
--Select * From [dbo].[ImageData] Where ImageId = '11515469876'

Select ResGrp_Section_Screen_Response_Value,Section_Screen_ResponseGroup_Response_Value From [GoCanvas].[dbo].[Submission_Section2_ResourceGroup] Where SubmissionId = 132296639
And ResGrp_Section_Screen_Response_Value In(Select ImageId From [GoCanvas].[dbo].[ImageData] Where SubmissionId = 132296639)

Select ImageId,ImageNumber From [GoCanvas].[dbo].[ImageData] Where SubmissionId = 132296639 And ImageId in
												()

Select ResGrp_Section_Name,ResGrp_Section_Screen_Name,ResGrp_Section_Screen_Response_Guid,ResGrp_Section_Screen_Response_Label,ResGrp_Section_Screen_Response_Value,ResGrp_Section_Screen_Response_Type,SubmissionId,Section_Screen_ResponseGroup_Response_Value From [dbo].[Submission_Section2_ResourceGroup] Where SubmissionId = 132155877

Select	 case when IsNull(ResGrp_Section_Name,'') <> '' then CONCAT('\""',ResGrp_Section_Name, '"\"') else ResGrp_Section_Name end as ResGrp_Section_Name
		,case when IsNull(ResGrp_Section_Screen_Name,'') <> '' then CONCAT('\""',ResGrp_Section_Screen_Name , '"\"') else ResGrp_Section_Screen_Name end as ResGrp_Section_Screen_Name
		,ResGrp_Section_Screen_Response_Guid
		,case when IsNull(ResGrp_Section_Screen_Response_Label,'') <> '' then CONCAT('\""',ResGrp_Section_Screen_Response_Label , '"\"') else ResGrp_Section_Screen_Response_Label end as ResGrp_Section_Screen_Response_Label
	    ,case when IsNull(ResGrp_Section_Screen_Response_Value,'') <> '' then CONCAT('\""', ResGrp_Section_Screen_Response_Value , '"\"') else ResGrp_Section_Screen_Response_Value end as ResGrp_Section_Screen_Response_Value
		,case when IsNull(ResGrp_Section_Screen_Response_Type,'') <> '' then CONCAT('\""',ResGrp_Section_Screen_Response_Type , '"\"') else ResGrp_Section_Screen_Response_Type end as ResGrp_Section_Screen_Response_Type
		,SubmissionId
		,case when IsNull(Section_Screen_ResponseGroup_Response_Value ,'') <> '' then CONCAT('\""',Section_Screen_ResponseGroup_Response_Value , '"\"')  else Section_Screen_ResponseGroup_Response_Value end as Section_Screen_ResponseGroup_Response_Value
From [dbo].[Submission_Section2_ResourceGroup] Where SubmissionId = 132155877

Select ResGrp_Section_Name
,ResGrp_Section_Screen_Name
,ResGrp_Section_Screen_Response_Guid
,('"'+ ResGrp_Section_Screen_Response_Label + '"') as ResGrp_Section_Screen_Response_Label
,('"'+ ResGrp_Section_Screen_Response_Value + '"') as ResGrp_Section_Screen_Response_Value
,ResGrp_Section_Screen_Response_Type
,SubmissionId
,Section_Screen_ResponseGroup_Response_Value 
From [dbo].[Submission_Section2_ResourceGroup] Where SubmissionId = 132325894

--'"+ string + "'

USE [GoCanvas]
GO

--/****** Object:  Table [dbo].[FormData]    Script Date: 7/12/2021 11:27:09 AM ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO

--CREATE TABLE [dbo].[FormData](
--	[FormData_table_id] [uniqueidentifier]  NOT NULL default(newid()),
--	[FormId] [varchar](50) NOT NULL,
--	[OriginatingLibraryTemplateId] [varchar](50) NULL,
--	[GUID] [nvarchar](200) NULL,
--	[Name] [nvarchar](200) NULL,
--	[Status] [nvarchar](50) NULL,
--	[Version] [varchar](25) NULL,
-- CONSTRAINT [PK_FormData] PRIMARY KEY CLUSTERED 
--(
--	[FormData_table_id] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
--) ON [PRIMARY]
--GO

--========================================================================================================================

--USE [GoCanvas]
--GO

--Drop Table [dbo].[SubmissionsData]
--GO

--Drop Table [dbo].[Submission_Section1]
--GO
--Drop Table [dbo].[Submission_Section2]
--GO
--Drop Table [dbo].[Submission_Section2_ResourceGroup]
--GO
--Drop Table [dbo].[ImageData]
--GO


--=============================================================================================================================================
USE [GoCanvas]
GO

/****** Object:  Table [dbo].[SubmissionsData]    Script Date: 7/12/2021 11:28:48 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SubmissionsData](
	[SubmissionsData_table_id] [uniqueidentifier]  NOT NULL default(newid()),
	[SubmissionId] [varchar](50) NOT NULL,
	[FormId] [varchar](50) NOT NULL,
	[FormName] [nvarchar](200) NOT NULL,
	[Date] [datetime] NULL,
	[DeviceDate] [datetime] NULL,
	[UserName] [nvarchar](200) NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[ResponseID] [nvarchar](200) NULL,
	[WebAccessToken] [nvarchar](200) NULL,
	[No] [varchar](25) NULL,
	[SubmissionNumber] [varchar](25) NULL,
	[SubFormSatus] [nvarchar](50) NULL,
	[SubFormVersion] [varchar](25) NULL,
 CONSTRAINT [PK_SubmissionsData] PRIMARY KEY CLUSTERED 
(
	[SubmissionsData_table_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--=================================================================================================================================================

USE [GoCanvas]
GO

/****** Object:  Table [dbo].[Submission_Section1]    Script Date: 7/12/2021 11:28:37 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Submission_Section1](
	[Sub_Section1_table_id] [uniqueidentifier]  NOT NULL default(newid()),
	[SectionName] [varchar](200) NULL,
	[Section_Screen_Name] [varchar](200) NULL,
	[Section_Screen_Response_Guid] [nvarchar](300) NULL,
	[Section_Screen_Response_Label] [nvarchar](200) NULL,
	[Section_Screen_Response_Value] [nvarchar](200) NULL,
	[Section_Screen_Response_Type] [varchar](50) NULL,
	--[SubmissionNumber] [varchar](25) NULL,
	[SubmissionId] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Submission_Section1] PRIMARY KEY CLUSTERED 
(
	[Sub_Section1_table_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--ALTER TABLE [dbo].[Submission_Section1]  WITH CHECK ADD  CONSTRAINT [FK_Submission_Section1_SubmissionsData] FOREIGN KEY([SubmissionId])
--REFERENCES [dbo].[SubmissionsData] ([SubmissionId])
--GO

--ALTER TABLE [dbo].[Submission_Section1] CHECK CONSTRAINT [FK_Submission_Section1_SubmissionsData]
--GO


--======================================================================================================================================
USE [GoCanvas]
GO

/****** Object:  Table [dbo].[Submission_Section2]    Script Date: 7/12/2021 11:29:52 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Submission_Section2](
	[Sub_Section2_table_id] [uniqueidentifier]  NOT NULL default(newid()),
	[SectionName] [varchar](200) NULL,
	[Section_Screen_Name] [varchar](200) NULL,
	[Section_Screen_ResponseGroup_Guid] [varchar](200) NULL,
	[Section_Screen_ResponseGroup_Response_Guid] [varchar](200) NULL,
	[Section_Screen_ResponseGroup_Response_Label] [nvarchar](200) NULL,
	[Section_Screen_ResponseGroup_Response_Value] [nvarchar](200) NULL,
	[Section_Screen_ResponseGroup_Response_Type] [varchar](50) NULL,
	--[SubmissionNumber] [varchar](25) NULL,
	[SubmissionId] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Submission_Section2] PRIMARY KEY CLUSTERED 
(
	[Sub_Section2_table_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--ALTER TABLE [dbo].[Submission_Section2]  WITH CHECK ADD  CONSTRAINT [FK_Submission_Section2_SubmissionsData] FOREIGN KEY([SubmissionId])
--REFERENCES [dbo].[SubmissionsData] ([SubmissionId])
--GO

--ALTER TABLE [dbo].[Submission_Section2] CHECK CONSTRAINT [FK_Submission_Section2_SubmissionsData]
--GO


--==============================================================================================================================
USE [GoCanvas]
GO

/****** Object:  Table [dbo].[Submission_Section2_ResourceGroup]    Script Date: 7/12/2021 11:30:47 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Submission_Section2_ResourceGroup](
	[Sub_Sec2_ResGrp_Section_table_id] [uniqueidentifier]  NOT NULL default(newid()),
	[ResGrp_Section_Name] [varchar](200) NULL,
	[ResGrp_Section_Screen_Name] [varchar](200) NULL,
	[ResGrp_Section_Screen_Response_Guid] [varchar](200) NULL,
	[ResGrp_Section_Screen_Response_Label] [varchar](200) NULL,
	[ResGrp_Section_Screen_Response_Value] [varchar](500) NULL,
	[ResGrp_Section_Screen_Response_Type] [varchar](50) NULL,
	--[SubmissionNumber] [varchar](25) NULL,
	[SubmissionId] [varchar](50) NOT NULL,
	[Section_Screen_ResponseGroup_Response_Value] [nvarchar](200) NULL,
 CONSTRAINT [PK_Submission_Section2_ResourceGroup] PRIMARY KEY CLUSTERED 
(
	[Sub_Sec2_ResGrp_Section_table_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--ALTER TABLE [dbo].[Submission_Section2_ResourceGroup]  WITH CHECK ADD  CONSTRAINT [FK_Submission_Section2_ResourceGroup_SubmissionsData] FOREIGN KEY([SubmissionId])
--REFERENCES [dbo].[SubmissionsData] ([SubmissionId])
--GO

--ALTER TABLE [dbo].[Submission_Section2_ResourceGroup] CHECK CONSTRAINT [FK_Submission_Section2_ResourceGroup_SubmissionsData]
--GO



--=======================================================================================================================================================
USE [GoCanvas]
GO

/****** Object:  Table [dbo].[ImageData]    Script Date: 7/12/2021 11:27:59 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ImageData](
	[Image_table_Id] [uniqueidentifier]  NOT NULL default(newid()),
	[ImageId] [varchar](50) NULL,
	[ImageNumber] [varchar](20) NULL,
	[SubmissionId] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ImageData] PRIMARY KEY CLUSTERED 
(
	[Image_table_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


--ALTER TABLE [dbo].[ImageData]  WITH CHECK ADD  CONSTRAINT [FK_ImageData_SubmissionsData] FOREIGN KEY([SubmissionId])
--REFERENCES [dbo].[SubmissionsData] ([SubmissionId])
--GO

--ALTER TABLE [dbo].[ImageData] CHECK CONSTRAINT [FK_ImageData_SubmissionsData]
--GO


