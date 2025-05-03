ALTER TABLE clientes ADD
    CPF_CNPJ NVARCHAR(20) NOT NULL UNIQUE,
    DataNascimento DATE NULL,
    Telefone NVARCHAR(20) NOT NULL,
    TipoPessoa CHAR(1) NOT NULL, -- 'F' ou 'J'
    IE NVARCHAR(30) NULL,
    IsentoIE BIT NOT NULL DEFAULT 0
	
	
	select * from [dbo].[clientes]



--	CREATE TABLE enderecos (
--    Id UNIQUEIDENTIFIER PRIMARY KEY,
--    ClienteId UNIQUEIDENTIFIER NOT NULL UNIQUE,
--    CEP NVARCHAR(10),
--    Endereco NVARCHAR(100),
--    Numero NVARCHAR(10),
--    Bairro NVARCHAR(50),
--    Cidade NVARCHAR(50),
--    Estado NVARCHAR(2),
--    CONSTRAINT FK_Endereco_Cliente FOREIGN KEY (ClienteId) REFERENCES clientes(Id) ON DELETE CASCADE
--);


--DECLARE @ClienteId UNIQUEIDENTIFIER;
--SELECT TOP 1 @ClienteId = Id FROM clientes WHERE Email = 'joao.silva@email.com';


--INSERT INTO enderecos (Id, ClienteId, CEP, Endereco, Numero, Bairro, Cidade, Estado)
--VALUES (
--    NEWID(),
--    @ClienteId,
--    '06400-000',
--    'Rua das Flores',
--    '123',
--    'Centro',
--    'Barueri',
--    'SP'


--);


--INSERT INTO clientes (Id, Nome, Email, CPF_CNPJ, DataNascimento, Telefone, TipoPessoa, IE, IsentoIE, Logotipo)
--VALUES (
--    NEWID(), -- Gera um GUID aleatório
--    'João da Silva',
--    'joao.silva@email.com',
--    '12345678900',
--    '1990-05-15',
--    '(11) 91234-5678',
--    'F',
--    NULL,
--    0,
--    'n'
--);






























--CREATE PROCEDURE InserirLogradouro
--    @Id UNIQUEIDENTIFIER,
--    @Numero NVARCHAR(MAX),
--    @Rua NVARCHAR(MAX),
--    @Bairro NVARCHAR(MAX),
--    @Cidade NVARCHAR(MAX),
--    @Estado NVARCHAR(MAX),
--    @CEP NVARCHAR(MAX),
--    @Complemento NVARCHAR(MAX),
--    @IdCliente UNIQUEIDENTIFIER
--AS
--BEGIN
--    SET NOCOUNT ON;

--    INSERT INTO dbo.logradouros (
--        Id,
--        Numero,
--        Rua,
--        Bairro,
--        Cidade,
--        Estado,
--        CEP,
--        Complemento,
--        IdCliente
--    )
--    VALUES (
--        @Id,
--        @Numero,
--        @Rua,
--        @Bairro,
--        @Cidade,
--        @Estado,
--        @CEP,
--        @Complemento,
--        @IdCliente
--    );
--END











--DECLARE @Id UNIQUEIDENTIFIER = NEWID();
--DECLARE @IdCliente UNIQUEIDENTIFIER = '2ABF3ACA-5EDD-45CC-8EOF-08DD887698A3'; -- ajuste para o ID real do cliente existente


--DECLARE @Id UNIQUEIDENTIFIER = NEWID();

--EXEC InserirLogradouro
--    @Id = @Id,
--    @Numero = '701',
--    @Rua = 'Rua das Palmeiras',
--    @Bairro = 'Centro',
--    @Cidade = 'Campina Grande',
--    @Estado = 'PB',
--    @CEP = '58400-000',
--    @Complemento = 'Apto 202',
--    @IdCliente = '2ABF3ACA-5EDD-45CC-8E0F-08DD887698A3';
















select * from  [dbo].[logradouros]

select * from [dbo].[clientes]




--delete from  [dbo].[logradouros]

--delete from [dbo].[clientes]