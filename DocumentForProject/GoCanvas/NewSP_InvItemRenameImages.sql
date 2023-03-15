USE [GoCanvas]
GO
/****** Object:  StoredProcedure [dbo].[sp_InventoryItemImagesMigration]    Script Date: 9/22/2021 10:29:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--Exec [dbo].[sp_InventoryItemImagesMigrationNew] 

Alter Procedure [dbo].[sp_InventoryItemImagesMigrationNew]
	--@item_code varchar(100)
	--,@cur_submissionid varchar(100)
	--,@cur_barcode varchar(500)
	--,@inv_item_id int
As
Begin 
	Select * into #tempImagefile
	from
	(
		Select --Top 100
		inv_item_img_id 
		,image_name as ImageId
		,REVERSE(LEFT(REVERSE([image_url]), CHARINDEX('/', REVERSE([image_url])) - 1)) as [image_name]
		--,concat(image_name,'.jpg') as [image_name]
		,[image_url] 
		From [InventoryMigration].[dbo].[InventoryItemImages]
		Where image_name in
		(
			Select ImageId From [GoCanvas].[dbo].[ImageData]
		)
		--Where inv_item_img_id in (257,258)
	) as tmp

	Declare @tmp_inv_item_img_id int, @tmp_img_id varchar(100),@tmp_img_name varchar(500),@tmp_img_url varchar(500)

	DECLARE  db_cursorMainImg CURSOR FOR Select inv_item_img_id,ImageId,[image_name],[image_url] From #tempImagefile 
	OPEN db_cursorMainImg   
	FETCH NEXT FROM db_cursorMainImg INTO @tmp_inv_item_img_id,@tmp_img_id,@tmp_img_name,@tmp_img_url
											 
	WHILE @@FETCH_STATUS = 0   
	BEGIN 		
		print concat('image_id-',@tmp_img_id,'image_name-',@tmp_img_name)
		Declare @outresult bit
		Declare @response nvarchar(max) = ''
	
		DECLARE @URL NVARCHAR(MAX) = Concat('http://localhost:25197/api/files/renameimages?img_id=',@tmp_img_id,'&img_name=',@tmp_img_name)
		print @URL
		DECLARE @Object INT
		DECLARE @json as table(Json_Table nvarchar(max))
				

		EXEC sp_OACreate 'MSXML2.XMLHTTP', @Object OUT;
		EXEC sp_OAMethod @Object, 'open', NULL, 'GET',
							@URL,
							'false'	
		EXEC sp_OAMethod @Object, 'send'

		--Exec sp_OAMethod @Object, 'responseText', @response OUTPUT

		--IF((Select @response) <> '')
		--	print @response

		INSERT into @json (Json_Table) exec sp_OAGetProperty @Object, 'responseText'
		Exec sp_OADestroy @Object

		 IF((Select count(Json_Table) From @json) > 0)
		 Begin
 				Select @response = Json_Table From @json
				--Select @outresult = Json_Table From @json
				print @response

				If(@response = 'true')
					Set @outresult = 1
				else 
					Set @outresult = 0
			
				print @outresult
		

				IF(@outresult = 1)
				Begin				
					print Concat('images are going to process->result is-',@outresult)
			
					Update [InventoryMigration].[dbo].[InventoryItemImages]
					Set image_name = @tmp_img_name
					Where inv_item_img_id = @tmp_inv_item_img_id

					print Concat('images are processed->result is-',@outresult)
			
				End
				Else
				Begin
					print Concat('could not able to update image into invitemimage table for inv_item_img_id- ',@tmp_inv_item_img_id, ' and result is->', @outresult)
				End
		 End

		FETCH NEXT FROM db_cursorMainImg INTO @tmp_inv_item_img_id,@tmp_img_id,@tmp_img_name,@tmp_img_url
	END   
	--/****End Main Cursor to ******/
	CLOSE db_cursorMainImg   
	DEALLOCATE db_cursorMainImg

	Drop Table #tempImagefile
End


--USE [GoCanvas]
--GO
--/****** Object:  StoredProcedure [dbo].[sp_InventoryItemImagesMigration]    Script Date: 9/17/2021 4:12:25 PM ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO

----Exec [dbo].[sp_InventoryItemImagesMigration] 'UsEFM1','132219841','2832116186846',1

--ALTER Procedure [dbo].[sp_InventoryItemImagesMigration]
--	@item_code varchar(100)
--	,@cur_submissionid varchar(100)
--	,@cur_barcode varchar(500)
--	,@inv_item_id int
--As
--Begin 
--	Select * into #tempImagefile
--	from
--	(
--		Select ImageId,
--			case when ((ROW_NUMBER() OVER(PARTITION BY SubmissionId  ORDER BY ImageId) - 1) > 0) then @item_code+'_'+Convert(varchar(5),(ROW_NUMBER() OVER(PARTITION BY SubmissionId  ORDER BY ImageId) - 1))+'.jpg'
--					else concat(@item_code,'.jpg') end as [image_name]
--			,concat('http://ssidb-test.systemsource.com/Project/GoCanvasImages/',case when ((ROW_NUMBER() OVER(PARTITION BY SubmissionId  ORDER BY ImageId) - 1) > 0) then @item_code+'_'+Convert(varchar(5),(ROW_NUMBER() OVER(PARTITION BY SubmissionId  ORDER BY ImageId) - 1))+'.jpg'
--					else concat(@item_code,'.jpg') end) as [image_url]
--		--	,case when (ImageNumber <> '0') then concat(ImageId,'_',ImageNumber,'.jpg') else concat(ImageId,'.jpg') end as [image_name]
--		--	,concat('http://ssidb-test.systemsource.com/Project/GoCanvasImages/',case when (ImageNumber <> '0') then concat(ImageId,'_',ImageNumber,'.jpg') else concat(ImageId,'.jpg') end) as [image_url]
--		From [GoCanvas].[dbo].[ImageData] Where SubmissionId = @cur_submissionid
--		And ImageId in
--			(
--				Select ResGrp_Section_Screen_Response_Value
--				FROM [GoCanvas].[dbo].[Submission_Section2_ResourceGroup]
--				Where [Section_Screen_ResponseGroup_Response_Value] = @cur_barcode 
--				And [ResGrp_Section_Screen_Response_Label] = 'Upload Image'
--			)

--	) as tmp

--	Declare @tmp_img_id varchar(100),@tmp_img_name varchar(500),@tmp_img_url varchar(500)

--	DECLARE  db_cursorMainImg CURSOR FOR Select ImageId,[image_name],[image_url] From #tempImagefile 
--	OPEN db_cursorMainImg   
--	FETCH NEXT FROM db_cursorMainImg INTO @tmp_img_id,@tmp_img_name,@tmp_img_url
											 
--	WHILE @@FETCH_STATUS = 0   
--	BEGIN 		
--		Declare @outresult bit
--		--Declare @response nvarchar(max)
--		--Select @sysid =[sys_id]  From [dbo].[request_followup_sys] Where [request_id] = @request_id	
		
--		DECLARE @URL NVARCHAR(MAX) = Concat('http://ssidb-test.systemsource.com/Project/GoCanvasApi/api/files/renameimages?img_id=',@tmp_img_id,'&img_name=',@tmp_img_name)
--		print @URL
--		DECLARE @Object INT
--		DECLARE @json as table(Json_Table bit)

--		--EXEC sp_OACreate 'MSXML2.ServerXMLHTTP', @Object OUT;
--		--EXEC sp_OAMethod @Object, 'open', NULL, 'POST',
--		--				 @URL,
--		--				 'false'
--		--EXEC sp_OAMethod @Object, 'setRequestHeader', null, 'Content-Type', 'application/json'
--		--EXEC sp_OAMethod @Object, 'send'
--		--INSERT into @json (Json_Table) exec sp_OAGetProperty @Object, 'responseText'

		
--		EXEC sp_OACreate 'MSXML2.ServerXMLHTTP.6.0', @Object OUT;
--		EXEC sp_OAMethod @Object, 'open', NULL, 'GET',
--							@URL,
--							'false'	
--		EXEC sp_OAMethod @Object, 'send'
--		INSERT into @json (Json_Table) exec sp_OAGetProperty @Object, 'responseText'

--		--Select  * From @json	


--		Select @outresult = Json_Table From @json


--		--print @outresult
--		--return @outresult
--		--Set @outresult = Select Json_Table From @json

--		--IF IsNull(@sysid,'') <> ''
--		--	Exec @outresult = [dbo].[CALLWEBAPISERVICEGET_ATTACHMENT] @sysid,@request_id, @outresult OUTPUT 
--		--Else
--		--	print Concat('sys id is not found for this request_id',@request_id)

--		IF(@outresult = 1)
--		Begin				
--			--print Concat('request attachment processed',@request_id)
--			Insert Into [InventoryMigrationTest].[dbo].[InventoryItemImages]
--			(
--				[inv_item_id]					
--				,[image_name]
--				,[image_url]
--			)
--			values
--			(
--				@inv_item_id
--				,@tmp_img_name
--				,@tmp_img_url
--			)			
--		End
--		Else
--		Begin
--			print Concat('Api result-',@outresult)
--			print Concat('could not able to insert image into invitem table for inv_item_id- ',@inv_item_id)
--		End

--		FETCH NEXT FROM db_cursorMainImg INTO @tmp_img_id,@tmp_img_name,@tmp_img_url
--	END   
--	--/****End Main Cursor to ******/
--	CLOSE db_cursorMainImg   
--	DEALLOCATE db_cursorMainImg

--	Drop Table #tempImagefile
--End
