USE [GoCanvas]
GO
/****** Object:  StoredProcedure [dbo].[sp_InventoryMigrationUpdation]    Script Date: 9/15/2021 9:01:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Exec sp_InventoryMigrationUpdation --132222445

ALTER Procedure [dbo].[sp_InventoryMigrationUpdation]
	--@SubmissionId varchar(50)
As
Begin
	declare @SubmissionId varchar(50)
	declare @tmpDate datetime,@tmpDeviceDate datetime,@tmpUserName varchar(100)

	Select * into #tmpMainSubData from
	(
		Select Distinct SubmissionId,[Date],DeviceDate,UserName From [dbo].[SubmissionsData]

	) as tmpMainSubData

	DECLARE  db_cursorMainSubData CURSOR FOR Select SubmissionId,[Date],DeviceDate,UserName From #tmpMainSubData Order By SubmissionId
	OPEN db_cursorMainSubData   
	FETCH NEXT FROM db_cursorMainSubData INTO @SubmissionId,@tmpDate,@tmpDeviceDate,@tmpUserName
	WHILE @@FETCH_STATUS = 0   
	BEGIN 

		--declare @tmpDate datetime,@tmpDeviceDate datetime,@tmpUserName varchar(100)
		--Select	@tmpDate = [Date] 
		--		,@tmpDeviceDate = DeviceDate
		--		,@tmpUserName = UserName 
		--From [dbo].[SubmissionsData] Where SubmissionId = @SubmissionId
 
	--IF OBJECT_ID('TEMPDB.dbo.##tmpsubmissiondata') IS NOT NULL DROP TABLE ##tmpsubmissiondata
		Select * into #tmpsubmissiondata from
		(	
			Select ss1.Section_Screen_Response_Label,ss1.Section_Screen_Response_Value, ss1.SubmissionId, '' as Section_Screen_ResponseGroup_Response_Value
			From [dbo].[Submission_Section1] ss1 
			Where ss1.SubmissionId = @SubmissionId
			Union All
			Select ss2.[Section_Screen_ResponseGroup_Response_Label],ss2.[Section_Screen_ResponseGroup_Response_Value], ss2.SubmissionId, '' as Section_Screen_ResponseGroup_Response_Value
			From [dbo].[Submission_Section2] ss2 
			Where ss2.SubmissionId = @SubmissionId
			Union All
			Select ss2rg.[ResGrp_Section_Screen_Response_Label],ss2rg.[ResGrp_Section_Screen_Response_Value],ss2rg.SubmissionId ,ss2rg.Section_Screen_ResponseGroup_Response_Value
			From [dbo].[Submission_Section2_ResourceGroup] ss2rg
			Where ss2rg.SubmissionId  = @SubmissionId
		) as tmpsubdata
	
	
		Select * into #tmpInventoryData from
		(
			SELECT SubmissionId,Section_Screen_ResponseGroup_Response_Value,[item_code], [Additional Description],[Area or Room Number],[Back],[Barcode],[Base],[Building Name],[Client]
						   ,[Condition - Select ALL that apply],[Date of Activity],[Depth],[Diameter],[Edge],[Fabric],[Floor],
						   [Frame],[GPS],[Height],[How Many Photos Do You Need?],[Item Type],[Manufacturer],[Notes],[Part Number if available],
						   [Quantity],[Quantity -  Fair],[Quantity - Damaged],[Quantity - Good],[Quantity - Missing Parts],[Quantity - Poor],[Seat],
						   [Seat Height],[Time of Activity],[Top],[Upload Image],[Width],[Finish],[Description],[rfidcode],[ownership],[Condition],[Address],[Custom/Modular/AV - Select all that Apply] 
						FROM (
						  select Section_Screen_Response_Label, Section_Screen_Response_Value, Section_Screen_ResponseGroup_Response_Value, SubmissionId
							from #tmpsubmissiondata) as i
						PIVOT (
						  Max(Section_Screen_Response_Value)
						  FOR Section_Screen_Response_Label
						  IN (
						   [item_code],[Additional Description],[Area or Room Number],[Back],[Barcode],[Base],[Building Name],[Client]
						   ,[Condition - Select ALL that apply],[Date of Activity],[Depth],[Diameter],[Edge],[Fabric],[Floor],
						   [Frame],[GPS],[Height],[How Many Photos Do You Need?],[Item Type],[Manufacturer],[Notes],[Part Number if available],
						   [Quantity],[Quantity -  Fair],[Quantity - Damaged],[Quantity - Good],[Quantity - Missing Parts],[Quantity - Poor],[Seat],
						   [Seat Height],[Time of Activity],[Top],[Upload Image],[Width],[Finish],[Description],[rfidcode],[ownership],[Condition],[Address],[Custom/Modular/AV - Select all that Apply]
						  )
						) AS PivotTable
		) as tmpInvData



		declare 
		 @cur_item_code varchar(100)
		,@cur_category varchar(50)
		,@cur_description nvarchar(max)
		,@cur_manuf varchar(50)
		,@cur_fabric varchar(1000)
		,@cur_finish varchar(1000)
		,@cur_notes nvarchar(max)
		--,@cur_createdate datetime
		--,@cur_updatedate datetime
		,@cur_rfidcode varchar(100)
		,@cur_barcode  varchar(100)
		,@cur_ownership varchar(100)
		,@cur_part_number varchar(100)
		,@cur_additionaldescription nvarchar(max)
		,@cur_diameter		varchar(50)
		,@cur_height		varchar(50)
		,@cur_width			varchar(50)
		,@cur_depth			varchar(50)
		,@cur_top			varchar(50)
		,@cur_edge			varchar(50)
		,@cur_base			varchar(50)
		,@cur_frame			varchar(50)
		,@cur_seat			varchar(50)
		,@cur_back			varchar(50)
		,@cur_seat_height	varchar(50)
		,@cur_createby varchar(100)
		,@cur_updateby varchar(100)
		,@cur_devicedate datetime
		,@cur_date datetime	
		,@cur_ARNum varchar(50),@cur_Qty int,@cur_QtyFair int,@cur_QtyDamaged int,@cur_QtyGood int,@cur_QtyMissingParts int,@cur_QtyPoor int
		,@cur_uploadimg varchar(50)
		,@cur_condition varchar(50)
		,@cur_address varchar(100)
		,@cur_custom varchar(200)
		,@cur_submissionid varchar(50)
		,@cur_building_name varchar(100)
		,@cur_floor varchar(100)

	--loop through the #tmpInventoryData to update inserted data in current instance
		DECLARE  db_cursorMain CURSOR FOR	Select  IsNull([item_code],'') as [item_code]
												   ,IsNull([Item Type],'') as [category]
												   ,IsNull([Description],'') as [description]
												   ,IsNull([Manufacturer],'') as [manuf]
												   ,IsNull([Fabric],'') as [fabric]
												   ,IsNull([Finish],'') as [finish]
												   --,IsNull(,'') as [size]
												   ,IsNull([Notes],'') as [notes]
												   --,IsNull(,'') as [other_notes]
												   --,@tmpcreatedate as [createdate]
												   --,@tmpcreatedate as [updatedate]
												   ,IsNull([rfidcode],'') as [rfidcode]
												   ,IsNull(Section_Screen_ResponseGroup_Response_Value,'') as [barcode]
												   ,IsNull([ownership],'') as [ownership]
												   ,IsNull([Part Number if available],'') as [part_number]
												   ,IsNull([Additional Description],'') as [additionaldescription]
												   ,IsNull([Diameter],'') as [diameter]
												   ,IsNull([Height],'') as [height]
												   ,IsNull([Width],'') as [width]
												   ,IsNull([Depth],'') as [depth]
												   ,IsNull([Top],'') as [top]
												   ,IsNull([Edge],'') as [edge]
												   ,IsNull([Base],'') as [base]
												   ,IsNull([Frame],'') as [frame]
												   ,IsNull([Seat],'') as [seat]
												   ,IsNull([Back],'') as [back]
												   ,IsNull([Seat Height],'') as [seat_height]
												   ,IsNull(@tmpUserName,'') as [createby]
												   ,IsNull(@tmpUserName,'') as [updateby]
												   ,IsNull(@tmpDeviceDate,'') as [devicedate]
												   ,IsNull(@tmpDate,'') as [date]
													--,Section_Screen_ResponseGroup_Response_Value 
													,IsNull([Area or Room Number],'') as [Area or Room Number]
													,IsNull([Quantity],0)				as [Quantity]
													,IsNull([Quantity -  Fair],0)			as [Quantity -  Fair]
													,IsNull([Quantity - Damaged],0)		as [Quantity - Damaged]
													,IsNull([Quantity - Good],0)			as [Quantity - Good]
													,IsNull([Quantity - Missing Parts],0)	as [Quantity - Missing Parts]
													,IsNull([Quantity - Poor],0) 			as [Quantity - Poor] 
													,ISNULL([Upload Image],'') as [Upload Image]
													,IsNull([Condition],'') as [Condition]
													,IsNull([Address],'') as [Address]
													,IsNull([Custom/Modular/AV - Select all that Apply],'') as [Custom]
													,IsNull([Building Name],'') as [Building Name]			
													,IsNull([Floor],'') as [Floor]		
													,SubmissionId
											From #tmpInventoryData 
											Where Section_Screen_ResponseGroup_Response_Value <> ''
											And concat(Section_Screen_ResponseGroup_Response_Value,[Item Type]) not in
											(Select concat([barcode],[category]) From [InventoryMigration].[dbo].[Inventory])
		OPEN db_cursorMain   
		FETCH NEXT FROM db_cursorMain INTO @cur_item_code 
										   ,@cur_category 
										   ,@cur_description 
										   ,@cur_manuf 
										   ,@cur_fabric 
										   ,@cur_finish 
										   ,@cur_notes 
										   --,@cur_createdate 
										   --,@cur_updatedate 
										   ,@cur_rfidcode 
										   ,@cur_barcode  
										   ,@cur_ownership 
										   ,@cur_part_number 
										   ,@cur_additionaldescription 
										   ,@cur_diameter		
										   ,@cur_height		
										   ,@cur_width			
										   ,@cur_depth			
										   ,@cur_top			
										   ,@cur_edge			
										   ,@cur_base			
										   ,@cur_frame			
										   ,@cur_seat			
										   ,@cur_back			
										   ,@cur_seat_height	
										   ,@cur_createby 
										   ,@cur_updateby 
										   ,@cur_devicedate 
										   ,@cur_date 	
										   ,@cur_ARNum,@cur_Qty,@cur_QtyFair,@cur_QtyDamaged,@cur_QtyGood,@cur_QtyMissingParts,@cur_QtyPoor
										   ,@cur_uploadimg
										   ,@cur_condition
										   ,@cur_address 
										   ,@cur_custom
										   ,@cur_building_name
										   ,@cur_floor
										   ,@cur_submissionid
											 
		WHILE @@FETCH_STATUS = 0   
		BEGIN 
				print 'inside the cursor'			
			
				declare @tmpinv_id int, @tmpClientId int, @tmpLocationId int, @tmpBuildingFromAddress varchar(50)
							, @tmpFloorFromAdress varchar(50), @newtmpClient varchar(100), @tmpItemCodeTag varchar(10)

				IF(@cur_category <> '')
					Set @tmpItemCodeTag = dbo.fnFirsties(@cur_category)
				--Select @tmpinv_id = inventory_id From [InventoryMigration].[dbo].[Inventory] Where category = @itemtype And barcode = @itemtype
				declare @tmpcreatedate datetime,@tmpgps varchar(200),@tmpClient varchar(100),@tmpBuilding varchar(50),@tmpFloor varchar(50),@tmpAddress varchar(100)

				Select	 @tmpcreatedate = cast((cast(Concat([Date of Activity],' ',[Time of Activity]) as datetime) at time zone 'UTC') AT TIME ZONE 'Pacific Standard Time' as datetime)  
						,@tmpgps = IsNull(GPS,'')
						,@tmpClient = IsNull([Client],'')
						,@tmpBuilding = IsNull([Building Name],'') 
						,@tmpFloor = IsNull([Floor],'')
						,@tmpAddress = IsNull([Address],'')
				From #tmpInventoryData 
				Where Section_Screen_ResponseGroup_Response_Value = ''
				And SubmissionId = @cur_submissionid

				IF(@tmpClient <> '')
				Begin		
					--print @tmpClient

					Set @newtmpClient = Case when  Lower(@tmpClient) like '%' + Lower('Seattle Showroom') +'%' then 'Seattle Downtown' else @tmpClient end
										   --when @tmpClient like '%portlan%' then 'Portland Office'
									  


					Select @tmpClientId = [client_id] From [InventoryMigration].[dbo].[Client]
					Where client_name = @newtmpClient

					--Select @tmpClientId = [client_id] From [InventoryMigration].[dbo].[Client]
					--Where client_name = @tmpClient

					--print @tmpClientId

					Select @tmpLocationId = [location_id],@tmpBuildingFromAddress = [location] From [InventoryMigration].[dbo].[LocationMap]
					Where [location] = (Case when  Lower(@tmpClient) like '%' + Lower('Seattle Showroom') +'%' then 'Seattle Downtown' 
											 --when  Lower(@tmpClient) like '%' + Lower('Portland') +'%' then 'Portland'
											 --when  Lower(@tmpClient) like '%' + Lower('Seattle') +'%' then 'Seattle'
											 --when  Lower(@tmpClient) like '%' + Lower('Seattle Warehouse') +'%' then 'Seattle Warehouse'
											 --when  Lower(@tmpClient) like '%' + Lower('San Diego') +'%' then 'San Diego'
											 --when  Lower(@tmpClient) like '%' + Lower('San Diego Warehouse') +'%' then 'San Diego Warehouse'
											 --when  Lower(@tmpClient) like '%' + Lower('Newport Beach') +'%' then 'Newport Beach'
											 --when  Lower(@tmpClient) like '%' + Lower('Los Angeles') +'%' then 'Los Angeles'
											 --when  Lower(@tmpClient) like '%' + Lower('LAOC Warehouse') +'%' then 'LAOC Warehouse'
										else 'Default' end)
										--else @tmpClient end)

					--print concat(@tmpLocationId,'-',@tmpBuildingFromAddress)

					If(@cur_building_name <> '')
						Set @tmpBuildingFromAddress = @cur_building_name
				End

				IF(@tmpAddress <> '')
				Begin
					print @tmpAddress
					--Set @tmpFloorFromAdress = ''

					declare @newAddress varchar(50),@len int--, @tmpSuite int
					Set @len = len(@tmpAddress)
					IF(CHARINDEX(',',@tmpAddress) > 0)
					Begin
						Set @newAddress = Substring(@tmpAddress,CHARINDEX(',',@tmpAddress)+1,@len)
					
						--Set @tmpSuite =  Convert(int, Substring(@newAddress,PATINDEX('%[0-9]%',@newAddress),len(@newAddress)) )

						Set @tmpFloorFromAdress = Convert(varchar(50), Substring(@newAddress,PATINDEX('%[0-9]%',@newAddress),len(@newAddress)) / 100)
					End
					Else
					Begin
						Set @tmpFloorFromAdress = ''
					End
					----print concat('floor is-',@tmpFloorFromAdress)
				End

				IF(@cur_floor <> '')
					Set @tmpFloorFromAdress = @cur_floor

				

				IF((Select Count(*) From [InventoryMigration].[dbo].[Inventory] Where category = @cur_category And barcode = @cur_barcode) = 0)
				Begin
					Insert into [InventoryMigration].[dbo].[Inventory]
					(
					   [item_code]
					  ,[category]
					  ,[description]
					  ,[manuf]
					  ,[fabric]
					  ,[finish]
					  --,[size]
					  ,[notes]
					  --,[other_notes]
					  ,[createdate]
					  ,[updatedate]
					  ,[rfidcode]
					  ,[barcode]
					  ,[ownership]
					  ,[part_number]
					  ,[additionaldescription]
					  ,[diameter]
					  ,[height]
					  ,[width]
					  ,[depth]
					  ,[top]
					  ,[edge]
					  ,[base]
					  ,[frame]
					  ,[seat]
					  ,[back]
					  ,[seat_height]
					  ,[createby]
					  ,[updateby]
					  ,[devicedate]
					  ,[date]
					)
					Values
					(
						@cur_item_code 
						--case when (@cur_custom <>'' ) then replace(Substring(@cur_custom,0,CHARINDEX('-', @cur_custom)),' ','')+@cur_manuf+@@IDENTITY
						--	else concat(@tmpItemCodeTag,@cur_manuf,convert(varchar(10),@@IDENTITY)) end 
						,@cur_category 
						,case when (@cur_description = '') then @cur_category else @cur_description end
						--,@cur_description
						,@cur_manuf 
						,@cur_fabric 
						,@cur_finish 
						,@cur_notes 
						,@tmpcreatedate 
						,@tmpcreatedate 
						,@cur_rfidcode 
						,@cur_barcode  
						,@cur_ownership 
						,@cur_part_number 
						,@cur_additionaldescription 
						----(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE('16” 1/2 x16” 1/2', '”', ''),'x','.'),'-','.'),'’',''),' .16 1/2','.16'))
						--,[dbo].[ufn_ConvertToNumber](REPLACE(REPLACE(@cur_diameter, '”', ''),'x',' '))
						--,[dbo].[ufn_ConvertToNumber](REPLACE(REPLACE(@cur_height, '”', ''),'x',' '))
						--,[dbo].[ufn_ConvertToNumber](REPLACE(REPLACE(@cur_width, '”', ''),'x',' '))
						--,[dbo].[ufn_ConvertToNumber](REPLACE(REPLACE(@cur_depth, '”', ''),'x',' '))
						,[dbo].[ufn_ConvertToNumber](REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@cur_diameter, '”', ''),'x','.'),'-','.'),'’',''),' .16 1/2','.16'),',',''))
						,[dbo].[ufn_ConvertToNumber](REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@cur_height, '”', ''),'x','.'),'-','.'),'’',''),' .16 1/2','.16'),',',''))
						,[dbo].[ufn_ConvertToNumber](REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@cur_width, '”', ''),'x','.'),'-','.'),'’',''),' .16 1/2','.16'),',',''))
						,[dbo].[ufn_ConvertToNumber](REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@cur_depth, '”', ''),'x','.'),'-','.'),'’',''),' .16 1/2','.16'),',',''))
						,@cur_top			
						,@cur_edge			
						,@cur_base			
						,@cur_frame			
						,@cur_seat			
						,@cur_back			
						,@cur_seat_height	
						,@cur_createby 
						,@cur_updateby 
						,@cur_devicedate 
						,@cur_date 
					)

					Set @tmpinv_id = @@IDENTITY

				End
				Else
				Begin
					Select @tmpinv_id = inventory_id From [InventoryMigration].[dbo].[Inventory] Where category = @cur_category And barcode = @cur_barcode
				End

				IF(@tmpinv_id > 0)
				Begin		 


					Update [InventoryMigration].[dbo].[Inventory]
					Set [item_code] = case when (@cur_custom <>'' ) then replace(Substring(@cur_custom,0,CHARINDEX('-', @cur_custom)),char(13)+char(10),'')+@cur_manuf+Convert(varchar(10),@tmpinv_id)
							else concat(@tmpItemCodeTag,@cur_manuf,Convert(varchar(10),@tmpinv_id)) end
					Where inventory_id = @tmpinv_id

					IF((Select Count(*) From [InventoryMigration].[dbo].[InventoryItem] Where inventory_id = @tmpinv_id) = 0)
					Begin
						print concat('inventory id -',@tmpinv_id)
						print concat('main qty ', @cur_Qty)
						IF(@cur_Qty > 0)
							--print concat('main qty ', @cur_Qty)
						Begin
							IF(@cur_condition <> '')
							Begin
								print concat('condition', @cur_condition)
								While(@cur_Qty > 0)
								Begin
									print concat('condition qty ', @cur_Qty)
									Insert Into [InventoryMigration].[dbo].[InventoryItem]
									(
									   [inventory_id]
									  ,[location_id]
									  ,[display_on_site]
									  ,[building]
									  ,[floor]
									  ,[mploc]
									  ,[cond]
									  --,[qty]
									  ,[notes]
									  ,[client_id]
									  ,[gps_location]
									  ,[barcode]
									)
									Values
									(
										@tmpinv_id
										,@tmpLocationId
										,1
										,case when (@tmpBuilding <> '') then @tmpBuilding else @tmpBuildingFromAddress end		
										,case when (@tmpFloor <> '' ) then @tmpFloor else @tmpFloorFromAdress end
										,@cur_ARNum
										,@cur_condition
										,''
										,@tmpClientId
										,@tmpgps
										,@cur_barcode
									)
									Set @cur_Qty = @cur_Qty - 1
								End
							End

							While(@cur_QtyFair > 0)
							Begin
								Insert Into [InventoryMigration].[dbo].[InventoryItem]
								(
								   [inventory_id]
								  ,[location_id]
								  ,[display_on_site]
								  ,[building]
								  ,[floor]
								  ,[mploc]
								  ,[cond]
								  --,[qty]
								  ,[notes]
								  ,[client_id]
								  ,[gps_location]
								  ,[barcode]
								)
								Values
								(
									@tmpinv_id
									,@tmpLocationId
									,1
									,case when (@tmpBuilding <> '') then @tmpBuilding else @tmpBuildingFromAddress end		
									,case when (@tmpFloor <> '' ) then @tmpFloor else @tmpFloorFromAdress end
									,@cur_ARNum
									,'Fair'
									,''
									,@tmpClientId
									,@tmpgps
									,@cur_barcode
								)
								Set @cur_QtyFair = @cur_QtyFair - 1
								--Set @cur_Qty = @cur_Qty - 1
							End

							While(@cur_QtyDamaged > 0)
							Begin
								Insert Into [InventoryMigration].[dbo].[InventoryItem]
								(
								   [inventory_id]
								  ,[location_id]
								  ,[display_on_site]
								  ,[building]
								  ,[floor]
								  ,[mploc]
								  ,[cond]
								  --,[qty]
								  ,[notes]
								  ,[client_id]
								  ,[gps_location]
								  ,[barcode]
								)
								Values
								(
									@tmpinv_id
									,@tmpLocationId
									,1
									,case when (@tmpBuilding <> '') then @tmpBuilding else @tmpBuildingFromAddress end		
									,case when (@tmpFloor <> '' ) then @tmpFloor else @tmpFloorFromAdress end
									,@cur_ARNum
									,'Damaged'
									,''
									,@tmpClientId
									,@tmpgps
									,@cur_barcode
								)
								Set @cur_QtyDamaged = @cur_QtyDamaged - 1
								--Set @cur_Qty = @cur_Qty - 1
							End
										
							While(@cur_QtyGood > 0)
							Begin
								Insert Into [InventoryMigration].[dbo].[InventoryItem]
								(
								   [inventory_id]
								  ,[location_id]
								  ,[display_on_site]
								  ,[building]
								  ,[floor]
								  ,[mploc]
								  ,[cond]
								  --,[qty]
								  ,[notes]
								  ,[client_id]
								  ,[gps_location]
								  ,[barcode]
								)
								Values
								(
									@tmpinv_id
									,@tmpLocationId
									,1
									,case when (@tmpBuilding <> '') then @tmpBuilding else @tmpBuildingFromAddress end		
									,case when (@tmpFloor <> '' ) then @tmpFloor else @tmpFloorFromAdress end
									,@cur_ARNum
									,'Good'
									,''
									,@tmpClientId
									,@tmpgps
									,@cur_barcode
								)
								Set @cur_QtyGood = @cur_QtyGood - 1
								--Set @cur_Qty = @cur_Qty - 1
							End

							While(@cur_QtyMissingParts > 0)
							Begin
								Insert Into [InventoryMigration].[dbo].[InventoryItem]
								(
								   [inventory_id]
								  ,[location_id]
								  ,[display_on_site]
								  ,[building]
								  ,[floor]
								  ,[mploc]
								  ,[cond]
								  --,[qty]
								  ,[notes]
								  ,[client_id]
								  ,[gps_location]
								  ,[barcode]
								)
								Values
								(
									@tmpinv_id
									,@tmpLocationId
									,1
									,case when (@tmpBuilding <> '') then @tmpBuilding else @tmpBuildingFromAddress end		
									,case when (@tmpFloor <> '' ) then @tmpFloor else @tmpFloorFromAdress end
									,@cur_ARNum
									,'Missing Parts'
									,''
									,@tmpClientId
									,@tmpgps
									,@cur_barcode
								)
								Set @cur_QtyMissingParts = @cur_QtyMissingParts - 1
								--Set @cur_Qty = @cur_Qty - 1
							End

							While(@cur_QtyPoor > 0)
							Begin
								Insert Into [InventoryMigration].[dbo].[InventoryItem]
								(
								   [inventory_id]
								  ,[location_id]
								  ,[display_on_site]
								  ,[building]
								  ,[floor]
								  ,[mploc]
								  ,[cond]
								  --,[qty]
								  ,[notes]
								  ,[client_id]
								  ,[gps_location]
								  ,[barcode]
								)
								Values
								(
									@tmpinv_id
									,@tmpLocationId
									,1
									,case when (@tmpBuilding <> '') then @tmpBuilding else @tmpBuildingFromAddress end		
									,case when (@tmpFloor <> '' ) then @tmpFloor else @tmpFloorFromAdress end
									,@cur_ARNum
									,'Poor'
									,''
									,@tmpClientId
									,@tmpgps
									,@cur_barcode
								)
								Set @cur_QtyPoor = @cur_QtyPoor - 1
								--Set @cur_Qty = @cur_Qty - 1
							End

						End


						declare @tmpinv_item_id int,@tmpItem_Code varchar(100)
						print concat('upload image', @cur_uploadimg)

						IF(@cur_uploadimg <> '')
						Begin
							Select Top 1 @tmpinv_item_id = [inv_item_id] 
							From [InventoryMigration].[dbo].[InventoryItem] 
							Where [inventory_id] = @tmpinv_id 
							--And [cond] <> 'Damaged'

							Select @tmpItem_Code = item_code From [InventoryMigration].[dbo].[Inventory] 
							Where [inventory_id] = @tmpinv_id 

							IF(@tmpinv_item_id > 0)
							Begin
								print concat('inventory item id -',@tmpinv_item_id)

								Exec [GoCanvas].[dbo].[sp_InventoryItemImagesMigration] @tmpItem_Code, @cur_submissionid,@cur_barcode,@tmpinv_item_id

								print 'inventory item images insertion done.'

								--Select ImageId,ImageNumber From [GoCanvas].[dbo].[ImageData] Where SubmissionId = @SubmissionId And ImageId in
								--					(Select ResGrp_Section_Screen_Response_Value From [GoCanvas].[dbo].[Submission_Section2_ResourceGroup] Where SubmissionId = @SubmissionId)
							
								  --declare @tmpImgId varchar(50), @tmpImgNum varchar(20), @tmpImgName varchar(100)
								  --Select @tmpImgId = ImageId,@tmpImgNum = ImageNumber From [GoCanvas].[dbo].[ImageData] Where SubmissionId = @cur_submissionid
								  --And ImageId in
								  --(
									 -- Select ResGrp_Section_Screen_Response_Value
									 -- FROM [GoCanvas].[dbo].[Submission_Section2_ResourceGroup]
									 -- Where [Section_Screen_ResponseGroup_Response_Value] = @cur_barcode 
									 -- And [ResGrp_Section_Screen_Response_Label] = 'Upload Image'
								  --)

								--IF(IsNull(@tmpImgId,'') <> '')
								--Begin
								--	IF(@tmpImgNum <> '0')
								--	Begin
								--		Set @tmpImgName = @tmpImgId + '_' + @tmpImgNum
								--	End
								--	Else
								--	Begin
								--		Set @tmpImgName = @tmpImgId
								--	End
								--End

								--Declare @Directory TABLE (Files Varchar(MAX))
								--Declare @File TABLE (Name varchar(MAX))
								--INSERT INTO @Directory
								--EXEC [master].[dbo].[XP_CMDSHELL] 'DIR "C:\ssi_upload\attachments\GoCanvasTest"'
								--Insert into @File
								--Select reverse(LEFT(reverse(Files),charindex(' ' ,reverse(Files)))) from @Directory
								--Select * into #tempImagefile
								--from
								--(
								--Select replace(f.Name,' ','') as FName 
								--,case when ((ROW_NUMBER() OVER(PARTITION BY SubmissionId  ORDER BY ImageId) - 1) > 0) then @tmpItem_Code +'_'+Convert(varchar(5),(ROW_NUMBER() OVER(PARTITION BY SubmissionId  ORDER BY ImageId) - 1))+'.jpg'
								--											else concat(@tmpItem_Code,'.jpg') end as [image_name]
								--from  @FILE f Inner Join [GoCanvas].[dbo].[ImageData] imgdata on concat(imgdata.ImageId,'.jpg') = replace(f.Name,' ','')
								--Where imgdata.SubmissionId = @cur_submissionid
								--And imgdata.ImageId in
								--					(
								--						Select ResGrp_Section_Screen_Response_Value
								--						FROM [GoCanvas].[dbo].[Submission_Section2_ResourceGroup]
								--						Where [Section_Screen_ResponseGroup_Response_Value] = @cur_barcode 
								--						And [ResGrp_Section_Screen_Response_Label] = 'Upload Image'
								--					)
								--) as tmp
								--Declare @tmpfname varchar(100),@tmp_image_name varchar(100)
								--DECLARE  db_cursorImageFile CURSOR FOR	Select FName,image_name From #tempImagefile
								--OPEN db_cursorImageFile   
								--FETCH NEXT FROM db_cursorImageFile INTO @tmpfname, @tmp_image_name
								--WHILE @@FETCH_STATUS = 0   
								--BEGIN 
								--	DECLARE @cmd NVARCHAR(4000);
								--	Set @cmd = 'rename ' + 'C:\ssi_upload\attachments\GoCanvasTest'+'\'+@tmpfname + ' ' + @tmp_image_name
								--	print @cmd
								--	exec [master].[dbo].[xp_cmdShell] @cmd
								--	print 'renamed file'

								--FETCH NEXT FROM db_cursorImageFile INTO @tmpfname, @tmp_image_name
								--END
								--CLOSE db_cursorImageFile   
								--DEALLOCATE db_cursorImageFile

								--Drop Table #tempImagefile

								--Insert Into [InventoryMigration].[dbo].[InventoryItemImages]
								--(
								--	[inv_item_id]					
								--	,[image_name]
								--	,[image_url]
								--)
								--Select 
								--	@tmpinv_item_id as [inv_item_id]
								--	,case when ((ROW_NUMBER() OVER(PARTITION BY SubmissionId  ORDER BY ImageId) - 1) > 0) then @tmpItem_Code+'_'+Convert(varchar(5),(ROW_NUMBER() OVER(PARTITION BY SubmissionId  ORDER BY ImageId) - 1))+'.jpg'
								--			else concat(@tmpItem_Code,'.jpg') end as [image_name]
								--	,concat('http://ssidb-test/',case when ((ROW_NUMBER() OVER(PARTITION BY SubmissionId  ORDER BY ImageId) - 1) > 0) then @tmpItem_Code+'_'+Convert(varchar(5),(ROW_NUMBER() OVER(PARTITION BY SubmissionId  ORDER BY ImageId) - 1))+'.jpg'
								--			else concat(@tmpItem_Code,'.jpg') end) as [image_url]
								----	,case when (ImageNumber <> '0') then concat(ImageId,'_',ImageNumber,'.jpg') else concat(ImageId,'.jpg') end as [image_name]
								----	,concat('http://ssidb-test/',case when (ImageNumber <> '0') then concat(ImageId,'_',ImageNumber,'.jpg') else concat(ImageId,'.jpg') end) as [image_url]
								--From [GoCanvas].[dbo].[ImageData] Where SubmissionId = @cur_submissionid
								--And ImageId in
								--  (
								--	  Select ResGrp_Section_Screen_Response_Value
								--	  FROM [GoCanvas].[dbo].[Submission_Section2_ResourceGroup]
								--	  Where [Section_Screen_ResponseGroup_Response_Value] = @cur_barcode 
								--	  And [ResGrp_Section_Screen_Response_Label] = 'Upload Image'
								--  )
								----Values
								----(
								----	@tmpinv_item_id
								----	,concat(@tmpImgName,'.jpg')
								----	,concat('http://ssidb-test/',@tmpImgName,'.jpg')
								----)							

							End


						End
						Else
						Begin
							print concat('inventory id not found-', @tmpinv_id)
						End			

					End
				End
				--print 
			
				FETCH NEXT FROM db_cursorMain INTO @cur_item_code 
										   ,@cur_category 
										   ,@cur_description 
										   ,@cur_manuf 
										   ,@cur_fabric 
										   ,@cur_finish 
										   ,@cur_notes 
										   --,@cur_createdate 
										   --,@cur_updatedate 
										   ,@cur_rfidcode 
										   ,@cur_barcode  
										   ,@cur_ownership 
										   ,@cur_part_number 
										   ,@cur_additionaldescription 
										   ,@cur_diameter		
										   ,@cur_height		
										   ,@cur_width			
										   ,@cur_depth			
										   ,@cur_top			
										   ,@cur_edge			
										   ,@cur_base			
										   ,@cur_frame			
										   ,@cur_seat			
										   ,@cur_back			
										   ,@cur_seat_height	
										   ,@cur_createby 
										   ,@cur_updateby 
										   ,@cur_devicedate 
										   ,@cur_date 
										   --,@cur_itemtype 
										   --,@cur_ssrrvalue 
										   ,@cur_ARNum,@cur_Qty,@cur_QtyFair,@cur_QtyDamaged,@cur_QtyGood,@cur_QtyMissingParts,@cur_QtyPoor
										   ,@cur_uploadimg
										    ,@cur_condition
										   ,@cur_address 
										   ,@cur_custom
										   ,@cur_building_name
										   ,@cur_floor
										   ,@cur_submissionid
		END   
		--/****End Main Cursor to ******/
		CLOSE db_cursorMain   
		DEALLOCATE db_cursorMain

		Drop Table #tmpInventoryData
		Drop Table #tmpsubmissiondata

	FETCH NEXT FROM db_cursorMainSubData INTO @SubmissionId,@tmpDate,@tmpDeviceDate,@tmpUserName
	END   
	--/****End Main Cursor to ******/
	CLOSE db_cursorMainSubData   
	DEALLOCATE db_cursorMainSubData

	Drop Table #tmpMainSubData
End




/*

	--Insert into [InventoryMigration].[dbo].[Inventory]
	--(
	--   [item_code]
 --     ,[category]
 --     ,[description]
 --     ,[manuf]
 --     ,[fabric]
 --     ,[finish]
 --     --,[size]
 --     ,[notes]
 --     --,[other_notes]
 --     ,[createdate]
 --     ,[updatedate]
 --     ,[rfidcode]
 --     ,[barcode]
 --     ,[ownership]
 --     ,[part_number]
 --     ,[additionaldescription]
 --     ,[diameter]
 --     ,[height]
 --     ,[width]
 --     ,[depth]
 --     ,[top]
 --     ,[edge]
 --     ,[base]
 --     ,[frame]
 --     ,[seat]
 --     ,[back]
 --     ,[seat_height]
	--  ,[createby]
	--  ,[updateby]
	--  ,[devicedate]
	--  ,[date]
	--)

	--Select	 
	--		--IsNull([Item Type],'') as [Item Type]
	--		--,Section_Screen_ResponseGroup_Response_Value as [barcode]
	--		--,@tmpcreatedate
	--		--,@tmpcreatedate			
	--   IsNull([item_code],'') as [item_code]
 --     ,IsNull([Item Type],'') as [category]
 --     ,IsNull([Description],'') as [description]
 --     ,IsNull([Manufacturer],'') as [manuf]
 --     ,IsNull([Fabric],'') as [fabric]
 --     ,IsNull([Finish],'') as [finish]
 --     --,IsNull(,'') as [size]
 --     ,IsNull([Notes],'') as [notes]
 --     --,IsNull(,'') as [other_notes]
 --     ,@tmpcreatedate as [createdate]
 --     ,@tmpcreatedate as [updatedate]
 --     ,IsNull([rfidcode],'') as [rfidcode]
 --     ,IsNull(Section_Screen_ResponseGroup_Response_Value,'') as [barcode]
 --     ,IsNull([ownership],'') as [ownership]
 --     ,IsNull([Part Number if available],'') as [part_number]
 --     ,IsNull([Additional Description],'') as [additionaldescription]
 --     ,IsNull([Diameter],'') as [diameter]
 --     ,IsNull([Height],'') as [height]
 --     ,IsNull([Width],'') as [width]
 --     ,IsNull([Depth],'') as [depth]
 --     ,IsNull([Top],'') as [top]
 --     ,IsNull([Edge],'') as [edge]
 --     ,IsNull([Base],'') as [base]
 --     ,IsNull([Frame],'') as [frame]
 --     ,IsNull([Seat],'') as [seat]
 --     ,IsNull([Back],'') as [back]
 --     ,IsNull([Seat Height],'') as [seat_height]
	--  ,IsNull(@tmpUserName,'') as [createby]
	--  ,IsNull(@tmpUserName,'') as [updateby]
	--  ,IsNull(@tmpDeviceDate,'') as [devicedate]
	--  ,IsNull(@tmpDate,'') as [date]
	--From #tmpInventoryData
	--Where Section_Screen_ResponseGroup_Response_Value <> ''
	

			--IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', '[Additional Description]') IS NOT NULL
			--Begin
			--	declare @tmpadditionaldesc nvarchar(max)
			--	Select @tmpadditionaldesc = [Additional Description] From ##TempTableTesting Where [Item Type] = @itemtype And Section_Screen_ResponseGroup_Response_Value = @ssrrvalue
			--	Update [InventoryMigration].[dbo].[Inventory] Set additionaldescription = IsNull(@tmpadditionaldesc,'') Where category = @itemtype And barcode = @ssrrvalue
			--	print 'perform update 1'
			--End
					
			--IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', '[Back]') IS NOT NULL
			--Begin
			--	declare @tmpback varchar(50)
			--	Select @tmpback = [Back] From ##TempTableTesting Where [Item Type] = @itemtype And Section_Screen_ResponseGroup_Response_Value = @ssrrvalue
			--	Update [InventoryMigration].[dbo].[Inventory] Set back = IsNull(@tmpback,'') Where category = @itemtype And barcode = @ssrrvalue
			--	print 'perform update 2'
			--End
			
			--IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', '[Base]') IS NOT NULL
			--Begin
			--	declare @tmpbase varchar(50)
			--	Select @tmpbase = [Base] From ##TempTableTesting Where [Item Type] = @itemtype And Section_Screen_ResponseGroup_Response_Value = @ssrrvalue
			--	Update [InventoryMigration].[dbo].[Inventory] Set base = IsNull(@tmpbase,'') Where category = @itemtype And barcode = @ssrrvalue
			--	print 'perform update 3'
			--End
			
			--IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', '[Depth]') IS NOT NULL
			--Begin
			--	declare @tmpdepth varchar(50)
			--	Select @tmpdepth = [Depth] From ##TempTableTesting Where [Item Type] = @itemtype And Section_Screen_ResponseGroup_Response_Value = @ssrrvalue
			--	Update [InventoryMigration].[dbo].[Inventory] Set depth = IsNull(@tmpdepth,'') Where category = @itemtype And barcode = @ssrrvalue
			--	print 'perform update 4'
			--End
						
			--IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', '[Description]') IS NOT NULL
			--Begin
			--	declare @tmpdesc nvarchar(max)
			--	Select @tmpdesc = [Description] From ##TempTableTesting Where [Item Type] = @itemtype And Section_Screen_ResponseGroup_Response_Value = @ssrrvalue
			--	Update [InventoryMigration].[dbo].[Inventory] Set [description] = IsNull(@tmpdesc,'') Where category = @itemtype And barcode = @ssrrvalue
			--End
						
			--IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', '[Diameter]') IS NOT NULL
			--Begin
			--	declare @tmpdiameter varchar(50)
			--	Select @tmpdiameter = [Diameter] From ##TempTableTesting Where [Item Type] = @itemtype And Section_Screen_ResponseGroup_Response_Value = @ssrrvalue
			--	Update [InventoryMigration].[dbo].[Inventory] Set diameter = IsNull(@tmpdiameter,'') Where category = @itemtype And barcode = @ssrrvalue
			--	print 'perform update 5'
			--End
						
			--IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', '[Fabric]') IS NOT NULL
			--Begin
			--	declare @tmpfabric varchar(1000)
			--	Select @tmpfabric = [Fabric] From ##TempTableTesting Where [Item Type] = @itemtype And Section_Screen_ResponseGroup_Response_Value = @ssrrvalue
			--	Update [InventoryMigration].[dbo].[Inventory] Set fabric = IsNull(@tmpfabric,'') Where category = @itemtype And barcode = @ssrrvalue
			--	print 'perform update 6'
			--End
						
			--IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', '[Frame]') IS NOT NULL
			--Begin
			--	declare @tmpframe varchar(50)
			--	Select @tmpframe = [Frame] From ##TempTableTesting Where [Item Type] = @itemtype And Section_Screen_ResponseGroup_Response_Value = @ssrrvalue
			--	Update [InventoryMigration].[dbo].[Inventory] Set frame = IsNull(@tmpframe,'') Where category = @itemtype And barcode = @ssrrvalue
			--	print 'perform update 7'
			--End
						
			
			--IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', '[Height]') IS NOT NULL
			--Begin
			--	declare @tmpheight varchar(50)
			--	Select @tmpheight = [Height] From ##TempTableTesting Where [Item Type] = @itemtype And Section_Screen_ResponseGroup_Response_Value = @ssrrvalue
			--	Update [InventoryMigration].[dbo].[Inventory] Set height = IsNull(@tmpheight,'') Where category = @itemtype And barcode = @ssrrvalue
			--End
						
			--IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', '[Manufacturer]') IS NOT NULL
			--Begin
			--	declare @tmpmanufacturer varchar(50)
			--	Select @tmpmanufacturer = [Manufacturer] From ##TempTableTesting Where [Item Type] = @itemtype And Section_Screen_ResponseGroup_Response_Value = @ssrrvalue
			--	Update [InventoryMigration].[dbo].[Inventory] Set manuf = IsNull(@tmpmanufacturer,'') Where category = @itemtype And barcode = @ssrrvalue
			--End
						
			--IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', '[Notes]') IS NOT NULL
			--Begin
			--	declare @tmpnotes nvarchar(max)
			--	Select @tmpnotes = [Notes] From ##TempTableTesting Where [Item Type] = @itemtype And Section_Screen_ResponseGroup_Response_Value = @ssrrvalue
			--	Update [InventoryMigration].[dbo].[Inventory] Set notes = IsNull(@tmpnotes,'') Where category = @itemtype And barcode = @ssrrvalue
			--End
						
			--IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', '[Part No. if available]') IS NOT NULL
			--Begin
			--	declare @tmppartnum varchar(100)
			--	Select @tmppartnum = [Part No. if available] From ##TempTableTesting Where [Item Type] = @itemtype And Section_Screen_ResponseGroup_Response_Value = @ssrrvalue
			--	Update [InventoryMigration].[dbo].[Inventory] Set part_number = IsNull(@tmppartnum,'') Where category = @itemtype And barcode = @ssrrvalue
			--End
						
			--IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', '[Seat]') IS NOT NULL
			--Begin
			--	declare @tmpseat varchar(50)
			--	Select @tmpseat = [Seat] From ##TempTableTesting Where [Item Type] = @itemtype And Section_Screen_ResponseGroup_Response_Value = @ssrrvalue
			--	Update [InventoryMigration].[dbo].[Inventory] Set seat = IsNull(@tmpseat,'') Where category = @itemtype And barcode = @ssrrvalue
			--End
						
			--IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', '[Top]') IS NOT NULL
			--Begin
			--	declare @tmptop varchar(50)
			--	Select @tmptop = [Top] From ##TempTableTesting Where [Item Type] = @itemtype And Section_Screen_ResponseGroup_Response_Value = @ssrrvalue
			--	Update [InventoryMigration].[dbo].[Inventory] Set [top] = IsNull(@tmptop,'') Where category = @itemtype And barcode = @ssrrvalue
			--End
						
			--IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', '[Width]') IS NOT NULL
			--Begin
			--	declare @tmpwidth varchar(50)
			--	Select @tmpwidth = [Width] From ##TempTableTesting Where [Item Type] = @itemtype And Section_Screen_ResponseGroup_Response_Value = @ssrrvalue
			--	Update [InventoryMigration].[dbo].[Inventory] Set width = IsNull(@tmpwidth,'') Where category = @itemtype And barcode = @ssrrvalue
			--End
						
			--IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', '[Finish]') IS NOT NULL
			--Begin
			--	declare @tmpfinish varchar(1000)
			--	Select @tmpfinish = [Finish] From ##TempTableTesting Where [Item Type] = @itemtype And Section_Screen_ResponseGroup_Response_Value = @ssrrvalue
			--	Update [InventoryMigration].[dbo].[Inventory] Set finish = @tmpfinish Where category = @itemtype And barcode = @ssrrvalue
			--End
						
			--IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', '[Edge]') IS NOT NULL
			--Begin
			--	declare @tmpedge varchar(50)
			--	Select @tmpedge = [Edge] From ##TempTableTesting Where [Item Type] = @itemtype And Section_Screen_ResponseGroup_Response_Value = @ssrrvalue
			--	Update [InventoryMigration].[dbo].[Inventory] Set [edge] = @tmpedge Where category = @itemtype And barcode = @ssrrvalue
			--End
						
			--IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', '[Seat Height]') IS NOT NULL
			--Begin
			--	declare @tmpseatheight varchar(50)
			--	Select @tmpseatheight = [Seat Height] From ##TempTableTesting Where [Item Type] = @itemtype And Section_Screen_ResponseGroup_Response_Value = @ssrrvalue
			--	Update [InventoryMigration].[dbo].[Inventory] Set seat_height = @tmpseatheight Where category = @itemtype And barcode = @ssrrvalue
			--End

			*/