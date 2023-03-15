USE [GoCanvas]
GO
/****** Object:  UserDefinedFunction [dbo].[fnFirsties]    Script Date: 9/14/2021 5:11:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fnFirsties] ( @str NVARCHAR(4000) )
RETURNS NVARCHAR(2000)
AS
BEGIN
    DECLARE @retval NVARCHAR(2000);

    SET @str=RTRIM(LTRIM(@str));
    SET @retval=LEFT(@str,1);

    WHILE CHARINDEX(' ',@str,1)>0 BEGIN
        SET @str=LTRIM(RIGHT(@str,LEN(@str)-CHARINDEX(' ',@str,1)));
        SET @retval+=LEFT(@str,1);
    END

    RETURN @retval;
END
GO
/****** Object:  UserDefinedFunction [dbo].[ufn_ConvertToNumber]    Script Date: 9/14/2021 5:11:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufn_ConvertToNumber](@STR VARCHAR(50))
RETURNS decimal(18,3)
AS
BEGIN
        DECLARE @L VARCHAR(50) = ''
        DECLARE @A DECIMAL(18,3) = 0
        SET @STR = LTRIM(RTRIM(@STR)); -- Remove extra spaces
        IF ISNUMERIC(@STR) > 0 SET @A = CONVERT(DECIMAL(18,3), @STR) -- Check to see if already real number
        IF CHARINDEX(' ',@STR,0) > 0
        BEGIN
            SET @L = SUBSTRING(@STR,1,CHARINDEX(' ',@STR,0) - 1 )
            SET @STR = SUBSTRING(@STR,CHARINDEX(' ',@STR,0) + 1 ,50 )
            SET @A = CONVERT(DECIMAL(18,3), @L)
        END
        IF CHARINDEX('/',@STR,0) > 0
        BEGIN
            SET @L = SUBSTRING(@STR,1,CHARINDEX('/',@STR,0) - 1 )
            SET @STR = SUBSTRING(@STR,CHARINDEX('/',@STR,0) + 1 ,50 )
            SET @A =  @A + ( CONVERT(DECIMAL(18,3), @L) / CONVERT(DECIMAL(18,3), @STR)  )
        END
        RETURN @A
END


GO
/****** Object:  StoredProcedure [dbo].[sp_InventoryItemImagesMigration]    Script Date: 9/14/2021 5:11:18 PM ******/
USE [GoCanvas]
GO
/****** Object:  StoredProcedure [dbo].[sp_InventoryItemImagesMigration]    Script Date: 9/15/2021 9:17:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Exec [dbo].[sp_InventoryItemImagesMigration] 'UsEFM1','132219841','2832116186846',1

ALTER Procedure [dbo].[sp_InventoryItemImagesMigration]
	@item_code varchar(100)
	,@cur_submissionid varchar(100)
	,@cur_barcode varchar(500)
	,@inv_item_id int
As
Begin 
	Select * into #tempImagefile
	from
	(
		Select ImageId,
			case when ((ROW_NUMBER() OVER(PARTITION BY SubmissionId  ORDER BY ImageId) - 1) > 0) then @item_code+'_'+Convert(varchar(5),(ROW_NUMBER() OVER(PARTITION BY SubmissionId  ORDER BY ImageId) - 1))+'.jpg'
					else concat(@item_code,'.jpg') end as [image_name]
			,concat('http://ssidb-test.systemsource.com/Project/GoCanvasImages/',case when ((ROW_NUMBER() OVER(PARTITION BY SubmissionId  ORDER BY ImageId) - 1) > 0) then @item_code+'_'+Convert(varchar(5),(ROW_NUMBER() OVER(PARTITION BY SubmissionId  ORDER BY ImageId) - 1))+'.jpg'
					else concat(@item_code,'.jpg') end) as [image_url]
		--	,case when (ImageNumber <> '0') then concat(ImageId,'_',ImageNumber,'.jpg') else concat(ImageId,'.jpg') end as [image_name]
		--	,concat('http://ssidb-test.systemsource.com/Project/GoCanvasImages/',case when (ImageNumber <> '0') then concat(ImageId,'_',ImageNumber,'.jpg') else concat(ImageId,'.jpg') end) as [image_url]
		From [GoCanvas].[dbo].[ImageData] Where SubmissionId = @cur_submissionid
		And ImageId in
			(
				Select ResGrp_Section_Screen_Response_Value
				FROM [GoCanvas].[dbo].[Submission_Section2_ResourceGroup]
				Where [Section_Screen_ResponseGroup_Response_Value] = @cur_barcode 
				And [ResGrp_Section_Screen_Response_Label] = 'Upload Image'
			)

	) as tmp

	Declare @tmp_img_id varchar(100),@tmp_img_name varchar(500),@tmp_img_url varchar(500)

	DECLARE  db_cursorMainImg CURSOR FOR Select ImageId,[image_name],[image_url] From #tempImagefile 
	OPEN db_cursorMainImg   
	FETCH NEXT FROM db_cursorMainImg INTO @tmp_img_id,@tmp_img_name,@tmp_img_url
											 
	WHILE @@FETCH_STATUS = 0   
	BEGIN 		
		Declare @outresult bit
		--Declare @response nvarchar(max)
		--Select @sysid =[sys_id]  From [dbo].[request_followup_sys] Where [request_id] = @request_id	
		
		DECLARE @URL NVARCHAR(MAX) = Concat('http://ssidb-test.systemsource.com/Project/GoCanvasApi/api/files/renameimages?img_id=',@tmp_img_id,'&img_name=',@tmp_img_name)
		print @URL
		DECLARE @Object INT
		DECLARE @json as table(Json_Table bit)

		--EXEC sp_OACreate 'MSXML2.ServerXMLHTTP', @Object OUT;
		--EXEC sp_OAMethod @Object, 'open', NULL, 'POST',
		--				 @URL,
		--				 'false'
		--EXEC sp_OAMethod @Object, 'setRequestHeader', null, 'Content-Type', 'application/json'
		--EXEC sp_OAMethod @Object, 'send'
		--INSERT into @json (Json_Table) exec sp_OAGetProperty @Object, 'responseText'

		
		EXEC sp_OACreate 'MSXML2.ServerXMLHTTP.6.0', @Object OUT;
		EXEC sp_OAMethod @Object, 'open', NULL, 'GET',
							@URL,
							'false'	
		EXEC sp_OAMethod @Object, 'send'
		INSERT into @json (Json_Table) exec sp_OAGetProperty @Object, 'responseText'

		Select  * From @json	


		Select @outresult = Json_Table From @json


		--print @outresult
		--return @outresult
		--Set @outresult = Select Json_Table From @json

		--IF IsNull(@sysid,'') <> ''
		--	Exec @outresult = [dbo].[CALLWEBAPISERVICEGET_ATTACHMENT] @sysid,@request_id, @outresult OUTPUT 
		--Else
		--	print Concat('sys id is not found for this request_id',@request_id)

		IF(@outresult = 1)
		Begin				
			--print Concat('request attachment processed',@request_id)
			Insert Into [InventoryMigrationTest].[dbo].[InventoryItemImages]
			(
				[inv_item_id]					
				,[image_name]
				,[image_url]
			)
			values
			(
				@inv_item_id
				,@tmp_img_name
				,@tmp_img_url
			)

			--Select 
			--	@inv_item_id as [inv_item_id]
			--	,case when ((ROW_NUMBER() OVER(PARTITION BY SubmissionId  ORDER BY ImageId) - 1) > 0) then @item_code+'_'+Convert(varchar(5),(ROW_NUMBER() OVER(PARTITION BY SubmissionId  ORDER BY ImageId) - 1))+'.jpg'
			--			else concat(@item_code,'.jpg') end as [image_name]
			--	,concat('http://ssidb-test/',case when ((ROW_NUMBER() OVER(PARTITION BY SubmissionId  ORDER BY ImageId) - 1) > 0) then @item_code+'_'+Convert(varchar(5),(ROW_NUMBER() OVER(PARTITION BY SubmissionId  ORDER BY ImageId) - 1))+'.jpg'
			--			else concat(@item_code,'.jpg') end) as [image_url]
			----	,case when (ImageNumber <> '0') then concat(ImageId,'_',ImageNumber,'.jpg') else concat(ImageId,'.jpg') end as [image_name]
			----	,concat('http://ssidb-test/',case when (ImageNumber <> '0') then concat(ImageId,'_',ImageNumber,'.jpg') else concat(ImageId,'.jpg') end) as [image_url]
			--From [GoCanvas].[dbo].[ImageData] Where SubmissionId = @cur_submissionid
			--And ImageId in
			--	(
			--		Select ResGrp_Section_Screen_Response_Value
			--		FROM [GoCanvas].[dbo].[Submission_Section2_ResourceGroup]
			--		Where [Section_Screen_ResponseGroup_Response_Value] = @cur_barcode 
			--		And [ResGrp_Section_Screen_Response_Label] = 'Upload Image'
			--	)
		End
		Else
		Begin
			print Concat('Api result-',@outresult)
			print Concat('could not able to insert image into invitem table for inv_item_id- ',@inv_item_id)
		End

		FETCH NEXT FROM db_cursorMainImg INTO @tmp_img_id,@tmp_img_name,@tmp_img_url
	END   
	--/****End Main Cursor to ******/
	CLOSE db_cursorMainImg   
	DEALLOCATE db_cursorMainImg

	Drop Table #tempImagefile
End
GO
