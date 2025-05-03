
--INSERT INTO [dbo].[logradouros]
--           ([ID]
--           ,[Numero]
--           ,[Rua]
--           ,[Bairro]
--           ,[Cidade]
--           ,[Estado]
--           ,[CEP]
--           ,[Complemento]
--           ,[IdCliente])
--     VALUES
--           ('00000000-0000-0000-0000-000000000000'
--           ,''
--           ,''
--           ,''
--           ,''
--           ,'11'
--           ,'222'
--           ,'<Complemento, nvarchar(255)'
--           ,'c52ca0e7-6d52-4d07-9576-566a8062a913')



	CREATE PROCEDURE InserirLogradouro
	@ID VARCHAR(120),
    @Numero VARCHAR(10),
	@Rua VARCHAR(255),	 
	@Bairro VARCHAR(100),
	@Cidade VARCHAR(100),
	@Estado VARCHAR(50),
	@CEP VARCHAR(10),
	@Complemento VARCHAR(255),
	@IdCliente VARCHAR(100)
AS
BEGIN
    INSERT INTO logradouros ([ID]
           ,[Numero]
           ,[Rua]
           ,[Bairro]
           ,[Cidade]
           ,[Estado]
           ,[CEP]
           ,[Complemento]
           ,[IdCliente])
    VALUES (
	@ID
    ,@Numero
	,@Rua 
	,@Bairro
	,@Cidade
	,@Estado
	,@CEP
	,@Complemento
	,@IdCliente)
END;




		   --EXEC InserirLogradouro 'd0f16619-fe6f-407b-82c0-bae3fe9b12bd'
     --      ,''
     --      ,''
     --      ,''
     --      ,''
     --      ,'11'
     --      ,'222'
     --      ,'<Complemento, nvarchar(255)'
     --      ,'c52ca0e7-6d52-4d07-9576-566a8062a913';





    