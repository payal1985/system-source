

Select * From [GoCanvas].[dbo].[SubmissionsData] WHere	FormName = 'Inventory Collection' And SubmissionNumber = '00014'

--Select sd.*,ss1.Section_Screen_Response_Label,ss1.Section_Screen_Response_Value
--		,ss2.[Section_Screen_ResponseGroup_Response_Label],ss2.[Section_Screen_ResponseGroup_Response_Value]
--		,ss2rg.[ResGrp_Section_Screen_Response_Label],ss2rg.[ResGrp_Section_Screen_Response_Value] 
--		,simg.[ImageId],simg.[ImageNumber]
--From [GoCanvas].[dbo].[SubmissionsData] sd
--Left Join [dbo].[Submission_Section1] ss1 On ss1.SubmissionId = sd.SubmissionId
--Left Join [dbo].[Submission_Section2] ss2 On ss2.SubmissionId = sd.SubmissionId
--Left Join [dbo].[Submission_Section2_ResourceGroup] ss2rg On ss2rg.SubmissionId = sd.SubmissionId
--Left Join [dbo].[ImageData] simg On simg.SubmissionId = sd.SubmissionId
--WHere	sd.FormName = 'Inventory Collection' And sd.SubmissionNumber = '00014'

--Drop table #tmpsubmissiondata

Select * into #tmpsubmissiondata from
(

--Select ss1.Section_Screen_Response_Label,ss1.Section_Screen_Response_Value, ss1.SubmissionId 
--From [dbo].[Submission_Section1] ss1 
--Where ss1.SubmissionId in (select SubmissionId From [GoCanvas].[dbo].[SubmissionsData] sd
--							Where sd.FormName = 'Inventory Collection' And sd.SubmissionNumber = '00014')
--Union All
--Select ss2.[Section_Screen_ResponseGroup_Response_Label],ss2.[Section_Screen_ResponseGroup_Response_Value], ss2.SubmissionId 
--From [dbo].[Submission_Section2] ss2 
--Where ss2.SubmissionId in (select SubmissionId From [GoCanvas].[dbo].[SubmissionsData] sd
--						Where sd.FormName = 'Inventory Collection' And sd.SubmissionNumber = '00014')
--Union All
--Select ss2rg.[ResGrp_Section_Screen_Response_Label],ss2rg.[ResGrp_Section_Screen_Response_Value],ss2rg.SubmissionId 
--From [dbo].[Submission_Section2_ResourceGroup] ss2rg
--Where ss2rg.SubmissionId  in (select SubmissionId From [GoCanvas].[dbo].[SubmissionsData] sd
--						Where sd.FormName = 'Inventory Collection' And sd.SubmissionNumber = '00014')
--Union All
--Select simg.[ImageId],simg.[ImageNumber],simg.SubmissionId 
--From [dbo].[ImageData] simg
--Where simg.SubmissionId in (select SubmissionId From [GoCanvas].[dbo].[SubmissionsData] sd
--						Where sd.FormName = 'Inventory Collection' And sd.SubmissionNumber = '00014')

Select ResGrp_Section_Screen_Response_Label
,ResGrp_Section_Screen_Response_Value
,ResGrp_Section_Screen_Response_Type,SubmissionId
,Section_Screen_ResponseGroup_Response_Value 
,'Col' + Cast(ROW_NUMBER() Over (Partition By Section_Screen_ResponseGroup_Response_Value Order By Section_Screen_ResponseGroup_Response_Value) as varchar(10)) as col
From [dbo].[Submission_Section2_ResourceGroup] 
Where SubmissionId = 132325387

) as tmp


 declare @query nvarchar(max);
declare @cols nvarchar(max);


select @cols = stuff((select ','+quotename(ResGrp_Section_Screen_Response_Label)
         from #tmpsubmissiondata as C
         group by c.ResGrp_Section_Screen_Response_Label
         order by c.ResGrp_Section_Screen_Response_Label
         for xml path('')), 1, 1, '');



set @query = 'select SubmissionId,  '+@cols+'
      from(select ResGrp_Section_Screen_Response_Label, ResGrp_Section_Screen_Response_Value,Section_Screen_ResponseGroup_Response_Value,SubmissionId
        from #tmpsubmissiondata) as i
      pivot
      (
        max(ResGrp_Section_Screen_Response_Value)
        for ResGrp_Section_Screen_Response_Label in ('+@cols+')
      ) p';
exec sp_executesql @query;


--Select ResGrp_Section_Screen_Response_Label,Col1,Col2,Col3,Col4,Col5,Col6,Col7,Col8,Col9,Col10,Col11,Col12,Col13,Col14,Col15,Col16,Col17,Col18,Col19,Col20,Col21,Col22,Col23,Col24,Col25
--From
--(
--	Select ResGrp_Section_Screen_Response_Label
--	,ResGrp_Section_Screen_Response_Value
--	,ResGrp_Section_Screen_Response_Type,SubmissionId
--	,Section_Screen_ResponseGroup_Response_Value 
--	,'Col' + Cast(ROW_NUMBER() Over (Partition By Section_Screen_ResponseGroup_Response_Value Order By Section_Screen_ResponseGroup_Response_Value) as varchar(10)) as col
--	From [dbo].[Submission_Section2_ResourceGroup] 
--	Where SubmissionId = 132155877
--)Temp
--Pivot
--(
--	max(ResGrp_Section_Screen_Response_Value)
--	For col in (Col1,Col2,Col3,Col4,Col5,Col6,Col7,Col8,Col9,Col10,Col11,Col12,Col13,Col14,Col15,Col16,Col17,Col18,Col19,Col20,Col21,Col22,Col23,Col24,Col25)
--)PIV


--Select * From #tmpsubmissiondata WHere ResGrp_Section_Screen_Response_Label <> 'Upload Image' 

--DECLARE 
--    @PivotColumns NVARCHAR(4000),
--    @sql NVARCHAR(4000),
--    @DeBug BIT = 0;

--SELECT 
--    @PivotColumns = CONCAT(@PivotColumns, N',
--    ', QUOTENAME(td.ResGrp_Section_Screen_Response_Label), N' = CASE WHEN td.ResGrp_Section_Screen_Response_Label = ', QUOTENAME(td.ResGrp_Section_Screen_Response_Label, ''''), N' THEN td.ResGrp_Section_Screen_Response_Value END') 
--SELECT @PivotColumns=STUFF((SELECT DISTINCT ', '+QUOTENAME(ResGrp_Section_Screen_Response_Label) FROM #tmpsubmissiondata FOR XML PATH ('')),1,2,'')
----SELECT STUFF((SELECT DISTINCT ', '+QUOTENAME(ResGrp_Section_Screen_Response_Label) FROM #tmpsubmissiondata FOR XML PATH ('')),1,2,'')
--FROM
--    #tmpsubmissiondata td
--GROUP BY
--    td.ResGrp_Section_Screen_Response_Label
---- ORDER BY ??? if you want the columns in a specific ordinal position.
--;

--SET @sql = CONCAT(N'
--SELECT 
--    td.[ResGrp_Section_Screen_Response_Label]',
--    @PivotColumns, N'
--FROM
--    #tmpsubmissiondata td
--GROUP BY
--    td.[ResGrp_Section_Screen_Response_Label];');

--IF @DeBug = 1
--BEGIN 
--    PRINT (@sql);
--END;
--ELSE 
--BEGIN 
--    EXEC sys.sp_executesql @sql;
--END;

--Declare @DynamicCol nvarchar(max),@DynamicColNull nvarchar(max)
--        ,@Sql nvarchar(max)

--SELECT @DynamicColNull=STUFF((SELECT DISTINCT ', '+'ISNULL('+QUOTENAME(ResGrp_Section_Screen_Response_Label),','+'''0'''+') As '+QUOTENAME(ResGrp_Section_Screen_Response_Label)
--                        FROM #tmpsubmissiondata FOR XML PATH ('')),1,2,'')

--SELECT @DynamicCol=STUFF((SELECT DISTINCT ', '+QUOTENAME(ResGrp_Section_Screen_Response_Label) FROM #tmpsubmissiondata FOR XML PATH ('')),1,2,'')

--SET @Sql='SELECT [ResGrp_Section_Screen_Response_Label], '+@DynamicColNull+' From
--            (   
--            SELECT * from #tmpsubmissiondata
--            )
--            AS Src
--            PIVOT
--            (
--            MAX(ResGrp_Section_Screen_Response_Label) FOR [ResGrp_Section_Screen_Response_Label] IN ('+@DynamicCol+')
--            )AS Pvt'
--PRINT @Sql
--EXEC(@Sql)

--DECLARE @cols AS NVARCHAR(MAX),
--    @query  AS NVARCHAR(MAX)

--select @cols = STUFF((SELECT ',' + QUOTENAME(col) 
--                    from #tmpsubmissiondata
--                    group by col--, id
--                    order by col
--            FOR XML PATH(''), TYPE
--            ).value('.', 'NVARCHAR(MAX)') 
--        ,1,1,'')

--		Select STUFF((SELECT ',' + QUOTENAME(col) 
--                    from #tmpsubmissiondata
--                    group by col--, id
--                    order by col
--            FOR XML PATH(''), TYPE
--            ).value('.', 'NVARCHAR(MAX)') 
--        ,1,1,'')

--set @query = N'SELECT ' + @cols + N' from 
--             (
--                select Section_Screen_Response_Value
--                from #tmpsubmissiondata
--            ) x
--            pivot 
--            (
--                max(Section_Screen_Response_Value)
--                for col in (' + @cols + N')
--            ) p '

--exec sp_executesql @query;

--DECLARE @colsUnpivot AS NVARCHAR(MAX),
--    @query  AS NVARCHAR(MAX),
--    @colsPivot as  NVARCHAR(MAX)

--select @colsUnpivot = stuff((select ','+quotename(C.name)
--         from sys.columns as C
--         where C.object_id = object_id('#tmpsubmissiondata') and C.name <> 'Section_Screen_Response_Label'
--         for xml path('')), 1, 1, '')

--Select stuff((select ','+quotename(C.name)
--         from sys.columns as C
--         where C.object_id = object_id('#tmpsubmissiondata') and C.name <> 'Section_Screen_Response_Label'
--         for xml path('')), 1, 1, '')

--select @colsPivot = STUFF((SELECT  ',' 
--                      + quotename(Section_Screen_Response_Label)
--                    from #tmpsubmissiondata t
--            FOR XML PATH(''), TYPE
--            ).value('.', 'NVARCHAR(MAX)') 
--        ,1,1,'')

----Select STUFF((SELECT  ',' 
----                      + quotename(Section_Screen_Response_Label)
----                    from #tmpsubmissiondata t
----            FOR XML PATH(''), TYPE
----            ).value('.', 'NVARCHAR(MAX)') 
----        ,1,1,'')


--set @query 
--  = 'select Section_Screen_Response_Label, '+@colsPivot+'
--      from
--      (
--        select SubmissionId, Section_Screen_Response_Label, Section_Screen_Response_Value
--        from #tmpsubmissiondata
--        unpivot
--        (
--          Section_Screen_Response_Value for Section_Screen_Response_Label in ('+@colsUnpivot+')
--        ) unpiv
--      ) src
--      pivot
--      (
--        sum(Section_Screen_Response_Value)
--        for Section_Screen_Response_Label in ('+@colsPivot+')
--      ) piv'

--exec(@query)

--WITH Submission
--AS
--(
--   SELECT ROW_NUMBER() OVER(PARTITION BY sd.SubmissionId Order By SubmissionNumber) AS RN
--		,sd.SubmissionId,sd.FormName,sd.Date,sd.DeviceDate,sd.UserName,sd.FirstName,sd.LastName,sd.SubmissionNumber
--		,ss1.Section_Screen_Response_Label,ss1.Section_Screen_Response_Value
--		,ss2.[Section_Screen_ResponseGroup_Response_Label],ss2.[Section_Screen_ResponseGroup_Response_Value]
--		,ss2rg.[ResGrp_Section_Screen_Response_Label],ss2rg.[ResGrp_Section_Screen_Response_Value] 
--		,simg.[ImageId],simg.[ImageNumber]
--From [GoCanvas].[dbo].[SubmissionsData] sd
--Left Join [dbo].[Submission_Section1] ss1 On ss1.SubmissionId = sd.SubmissionId
--Left Join [dbo].[Submission_Section2] ss2 On ss2.SubmissionId = sd.SubmissionId
--Left Join [dbo].[Submission_Section2_ResourceGroup] ss2rg On sd.SubmissionId = sd.SubmissionId
--Left Join [dbo].[ImageData] simg On simg.SubmissionId = sd.SubmissionId
--WHere	sd.FormName = 'Inventory Collection' And sd.SubmissionNumber = '00014'
--)
--SELECT * 
--FROM Submission 
--WHERE rn = 1;

Select ResGrp_Section_Screen_Response_Label
,ResGrp_Section_Screen_Response_Value
,ResGrp_Section_Screen_Response_Type,SubmissionId
,Section_Screen_ResponseGroup_Response_Value 
,'Col' + Cast(ROW_NUMBER() Over (Partition By Section_Screen_ResponseGroup_Response_Value Order By Section_Screen_ResponseGroup_Response_Value) as varchar(10)) as col
From [dbo].[Submission_Section2_ResourceGroup] 
Where SubmissionId = 132155877


----------------------------------------------------------------------------------------

Select * into ##tmpsubmissiondata from
	(

	Select ResGrp_Section_Screen_Response_Label
	,ResGrp_Section_Screen_Response_Value
	,ResGrp_Section_Screen_Response_Type,SubmissionId
	,Section_Screen_ResponseGroup_Response_Value 
	--,'Col' + Cast(ROW_NUMBER() Over (Partition By Section_Screen_ResponseGroup_Response_Value Order By Section_Screen_ResponseGroup_Response_Value) as varchar(10)) as col
	From [dbo].[Submission_Section2_ResourceGroup] 
	Where SubmissionId = 132155877

	) as tmp

declare @query nvarchar(max);
declare @cols nvarchar(max);
--declare @res nvarchar(max) 

select @cols = stuff((select ','+quotename(ResGrp_Section_Screen_Response_Label)
         from ##tmpsubmissiondata as C
         group by c.ResGrp_Section_Screen_Response_Label
         order by c.ResGrp_Section_Screen_Response_Label
         for xml path('')), 1, 1, '');
		 
Select @cols
--set @query = 'select SubmissionId,  '+@cols+'
--	from(select ResGrp_Section_Screen_Response_Label, ResGrp_Section_Screen_Response_Value,Section_Screen_ResponseGroup_Response_Value,SubmissionId
--	from #tmpsubmissiondata) as i
--	pivot
--	(
--	max(ResGrp_Section_Screen_Response_Value)
--	for ResGrp_Section_Screen_Response_Label in ('+@cols+')
--	) p';


--exec sp_executesql  @query  


IF OBJECT_ID('TEMPDB.dbo.##TempTableTesting') IS NOT NULL DROP TABLE ##TempTableTesting

set @query = 'select SubmissionId,  '+@cols+'
into ##TempTableTesting
from(select ResGrp_Section_Screen_Response_Label, ResGrp_Section_Screen_Response_Value,Section_Screen_ResponseGroup_Response_Value,SubmissionId
	from ##tmpsubmissiondata) as i
	pivot
	(
	max(ResGrp_Section_Screen_Response_Value)
	for ResGrp_Section_Screen_Response_Label in ('+@cols+')
	) p'

execute sp_executesql @query

select * from ##TempTableTesting
--drop table  TEMPDB.dbo.##TempTableTesting

DECLARE @Counter INT 
select @Counter = count(*) from ##TempTableTesting

IF COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', 'Height') IS NOT NULL
BEGIN
   select 'exits'
END
Else
	Select 'not exists'

	--Select case when COL_LENGTH('[tempdb].[dbo].[##TempTableTesting]', 'Height') IS NOT NULL then [Fabric] else 'not exists' end as 'column' From ##TempTableTesting

WHILE ((select count(*) from ##TempTableTesting)  > 0)
BEGIN
    --PRINT 'The counter value is = ' + CONVERT(VARCHAR,@Counter)
    --SET @Counter  = @Counter  + 1

	Select * into #temp
	from
	(
	Select ROW_NUMBER() Over (Partition By SubmissionId Order By SubmissionId) as RN,* from ##TempTableTesting
	) as t1

	Insert into [InventoryMigration].[dbo].[Inventory]
	(
		--[item_code]
		[category]
		,[description]
		,[manuf]
		--,[fabric]
		--,[finish]
		--,[size]
		,[notes]
		--,[other_notes]
		,[createdate]
		,[updatedate]
		--,[rfidcode]
		,[barcode]
		--,[ownership]
		,[part_number]
		,[additionaldescription]
		,[diameter]
		,[height]
		,[width]
		,[depth]
		,[top]
		--,[edge]
		,[base]
		,[frame]
		,[seat]
		,[back]
		--,[seat_height]
	)
	--,[Area or Room Number],,[Barcode (If available)],,[Condition],,[Description],,
	--[Floor (Ground=0, Mezzanine=100, Roof=101)],,,[How Many Photos Do You Need?],[Item Type],[Manufacturer]
	--,[Notes],[Part No. if available],[Quantity],,[Upload Image],
	Select	 IsNull([Item Type],'') as [Item Type]
			,IsNull([Description],'') as [Description]
			,IsNull([Manufacturer],'') as [Manufacturer]
			,IsNull([Notes],'') as [Notes]
			,getdate() as [createdate]
			,getdate() as [updatedate]
			,IsNull([Barcode (If available)],'') as [Barcode (If available)]
			,IsNull([Part No. if available],'') as [Part No. if available]
			,IsNull([Additional Description],'') as [Additional Description]
			,IsNull([Diameter],'') as [Diameter]
			,IsNull([Height],'') as [Height]
			,IsNull([Width],'') as [Width]
			,IsNull([Depth],'') as [Depth]
			,IsNull([Top],'') as [Top]
			,IsNull([Base],'') as [Base]
			,IsNull([Frame],'') as [Frame]
			,IsNull([Seat],'') as [Seat]
			,IsNull([Back],'') as [Back]
	From #temp

END

Drop Table #tmpsubmissiondata