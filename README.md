alter table recipes add RECIPE_TYPE_ID decimal

create table RecipeType (CREATION_DATE date,CREATION_TIME time,CREATION_USER varchar(100),
UPDATE_DATE date, UPDATE_TIME time,UPDATE_USER varchar(100),ID decimal primary key identity(1,1),
RECIPETYPE_NAME varchar(100),DELETE_FLAG char,ACTIVE char)
insert into RecipeType (CREATION_DATE ,CREATION_TIME ,CREATION_USER,
UPDATE_DATE , UPDATE_TIME ,UPDATE_USER,
RECIPETYPE_NAME ,DELETE_FLAG ,ACTIVE ) values 

((select convert(varchar,getdate(),23)),(select convert(varchar,getdate(),108)),'',
(select convert(varchar,getdate(),23)),(select convert(varchar,getdate(),108)),'',
'Veg','N','Y'
),
((select convert(varchar,getdate(),23)),(select convert(varchar,getdate(),108)),'',
(select convert(varchar,getdate(),23)),(select convert(varchar,getdate(),108)),'',
'Non-Veg','N','Y'
)
