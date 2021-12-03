CREATE DATABASE projetoBosch;
GO
--DROP DATABASE projetoBosch SELECT * FROM projetoBosch.dbo.colaborador
------------------------------------------------------------------------------------------------------------------------
-----------------------------------------IMPORTAÇÂO DOS DADOS EM TABELAS-------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------
-- CESTA SECA
CREATE TABLE projetoBosch.dbo.CestaSeca$(
	CPF varchar(11),
	dataNascimentoPuro varchar(50),
	nomeCompleto nvarchar(255),
	unidadeOrganizacional nvarchar(255),
	dataNascimento as CONVERT(DATETIME,dataNascimentoPuro,103)
)
GO
BULK INSERT projetoBosch.dbo.CestaSeca$ FROM 'C:\Users\droit\Desktop\ProjetoBosch\BoschCartaoDigitalBackEnd\scripts\arquivosCSV\cestaSecaCSV.csv'
WITH
(
FIRSTROW = 2,
FIELDTERMINATOR = ';',
ROWTERMINATOR = '\n'
)

--------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------CESTA FRIA-----------------------------------------------------------------------------------
CREATE TABLE projetoBosch.dbo.CestaFria$(
	CPF varchar(11),
	dataNascimentoPuro varchar(50),
	nomeCompleto nvarchar(255),
	unidadeOrganizacional nvarchar(255),
	dataNascimento as CONVERT(DATETIME,dataNascimentoPuro,103)
)
GO
BULK INSERT projetoBosch.dbo.CestaFria$ FROM 'C:\Users\droit\Desktop\ProjetoBosch\BoschCartaoDigitalBackEnd\scripts\arquivosCSV\cestaFriaCSV.csv'
WITH
(
FIRSTROW = 2,
FIELDTERMINATOR = ';',
ROWTERMINATOR = '\n'
)
--------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------PRESENTE BOSCH-----------------------------------------------------------------------------------
CREATE TABLE projetoBosch.dbo.PresenteBosch$(
	CPF varchar(11),
	dataNascimentoPuro varchar(50),
	nomeCompleto nvarchar(255),
	unidadeOrganizacional nvarchar(255),
	dataNascimento as CONVERT(DATETIME,dataNascimentoPuro,103),

)
GO
BULK INSERT projetoBosch.dbo.PresenteBosch$ FROM 'C:\Users\droit\Desktop\ProjetoBosch\BoschCartaoDigitalBackEnd\scripts\arquivosCSV\presenteBoschCSV.csv'
WITH
(
FIRSTROW = 2,
FIELDTERMINATOR = ';',
ROWTERMINATOR = '\n'
)

--------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------MATERIAL ESCOLAR-----------------------------------------------------------------------------------
CREATE TABLE projetoBosch.dbo.MaterialEscolar$(
	CPF varchar(11),
	dataNascimentoPuro varchar(50),
	nomeCompleto nvarchar(255),
	unidadeOrganizacional nvarchar(255),
	KIT1 nvarchar(255),
	KIT2 nvarchar(255),
	KIT3 nvarchar(255),
	KIT4 nvarchar(255),
	KIT5 nvarchar(255),
	KIT6 nvarchar(255),
	dataNascimento as CONVERT(DATETIME,dataNascimentoPuro,103)

)
GO
BULK INSERT projetoBosch.dbo.MaterialEscolar$ FROM 'C:\Users\droit\Desktop\ProjetoBosch\BoschCartaoDigitalBackEnd\scripts\arquivosCSV\materialEscolarCSV.csv'
WITH
(
FIRSTROW = 2,
FIELDTERMINATOR = ';',
ROWTERMINATOR = '\n',
KEEPNULLS
)
--------------------------------------------------------------------------------------------------------------------------------------------
---------------------------------------------------------BRINQUEDOS-----------------------------------------------------------------------------------
CREATE TABLE projetoBosch.dbo.Brinquedos$(
	CPF varchar(11),
	dataNascimentoPuro varchar(50),
	nomeCompleto nvarchar(255),
	unidadeOrganizacional nvarchar(255),
	BRINQUEDO0 nvarchar(255),
	BRINQUEDO1 nvarchar(255),
	BRINQUEDO2 nvarchar(255),
	BRINQUEDO3 nvarchar(255),
	BRINQUEDO4 nvarchar(255),
	BRINQUEDO5 nvarchar(255),
	BRINQUEDO6 nvarchar(255),
	BRINQUEDO7 nvarchar(255),
	BRINQUEDO8 nvarchar(255),
	BRINQUEDO9 nvarchar(255),
	BRINQUEDO10 nvarchar(255),
	BRINQUEDO11 nvarchar(255),
	BRINQUEDO12 nvarchar(255),
	dataNascimento as CONVERT(DATETIME,dataNascimentoPuro,103)

)

GO
BULK INSERT projetoBosch.dbo.Brinquedos$ FROM 'C:\Users\droit\Desktop\ProjetoBosch\BoschCartaoDigitalBackEnd\scripts\arquivosCSV\brinquedoCSV.csv'
WITH
(
FIRSTROW = 2,
FIELDTERMINATOR = ';',
ROWTERMINATOR = '\n'
)
------------------------------------------------------------------------------------------------------------------------
-----------------------------------------CRIAÇÃO E POPULAÇÃO DE TABELAS DO BD-------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE projetoBosch.dbo.tipoPermissao (
    id int identity(1,1),
    descricao nvarchar(255) not null,
	descricaNormalizada as cast(upper(rtrim(ltrim(descricao))) as nvarchar(255)),
	constraint pk_tipoPermissao_id primary key(id)
);
------------------------------------------------------------------------------------------------------
-- UNIDADE ORGANIZACIONAL

CREATE TABLE projetoBosch.dbo.unidadeOrganizacional (
    id int identity(1,1),
    descricao nvarchar(255) not null,
	descricaNormalizada as cast(upper(rtrim(ltrim(descricao))) as nvarchar(255)),
	constraint pk_unidadeOrganizacional_id primary key(id)
);

INSERT INTO projetoBosch.dbo.unidadeOrganizacional (descricao)
	(SELECT DISTINCT unidadeOrganizacional FROM projetoBosch.dbo.Brinquedos$
	UNION
	SELECT DISTINCT unidadeOrganizacional FROM projetoBosch.dbo.CestaSeca$
	UNION
	SELECT DISTINCT unidadeOrganizacional FROM projetoBosch.dbo.CestaFria$
	UNION
	SELECT DISTINCT unidadeOrganizacional FROM projetoBosch.dbo.MaterialEscolar$
	UNION
	SELECT DISTINCT unidadeOrganizacional FROM projetoBosch.dbo.PresenteBosch$);
------------------------------------------------------------------------------------------------------
-- COLABORADOR

CREATE TABLE projetoBosch.dbo.colaborador (
    id int identity(1,1),
	cpf varchar(11) not null,
	nomeCompleto nvarchar(255),
	dataNascimento date,
	unidadeOrganizacionalID int,
	constraint pk_colaborador_id primary key(id),
	constraint fk_colaborador_unidadeOrganizacionalID foreign key(unidadeOrganizacionalID) references unidadeOrganizacional(id),
	EDV varchar(10),
	Senha varchar(15)
);

INSERT INTO projetoBosch.dbo.colaborador(cpf, nomeCompleto, dataNascimento,unidadeOrganizacionalID)
	SELECT DISTINCT CPF, nomeCompleto, dataNascimento,id  FROM projetoBosch.dbo.Brinquedos$ B
	INNER JOIN projetoBosch.dbo.unidadeOrganizacional U on B.unidadeOrganizacional = U.descricao
	UNION
	SELECT DISTINCT CPF, nomeCompleto, dataNascimento,id FROM projetoBosch.dbo.CestaSeca$ CS
	INNER JOIN projetoBosch.dbo.unidadeOrganizacional U on CS.unidadeOrganizacional = U.descricao
	UNION
	SELECT DISTINCT CPF, nomeCompleto, dataNascimento,id FROM projetoBosch.dbo.CestaFria$ CF
	INNER JOIN projetoBosch.dbo.unidadeOrganizacional U on CF.unidadeOrganizacional = U.descricao
	UNION
	SELECT DISTINCT CPF, nomeCompleto, dataNascimento,id FROM projetoBosch.dbo.MaterialEscolar$ ME
	INNER JOIN projetoBosch.dbo.unidadeOrganizacional U on ME.unidadeOrganizacional = U.descricao
	UNION
	SELECT DISTINCT CPF, nomeCompleto, dataNascimento,id FROM projetoBosch.dbo.PresenteBosch$ P
	INNER JOIN projetoBosch.dbo.unidadeOrganizacional U on P.unidadeOrganizacional = U.descricao

UPDATE projetoBosch.dbo.colaborador SET EDV=REVERSE(SUBSTRING(cpf,2,7)); --Criação dos EDVS
UPDATE projetoBosch.dbo.colaborador SET Senha=CONCAT(SUBSTRING(nomeCompleto, 4, 1),REVERSE(SUBSTRING(cpf,5,3)), SUBSTRING(nomeCompleto, 2, 1), SUBSTRING(CONVERT(varchar, EDV),2,3)); --Criação das Senhas
------------------------------------------------------------------------------------------------------
--PERMISSAO
CREATE TABLE projetoBosch.dbo.permissao(
	colaboradorID int,
	tipoPermissaoID int,
	constraint pk_permissao_colaboradorID_tipoPermissaoID primary key(colaboradorID, tipoPermissaoID),
	constraint fk_permissao_colaboradorID foreign key(colaboradorID) references colaborador(id),
	constraint fk_permissao_tipoPermissaoID foreign key(tipoPermissaoID) references tipoPermissao(id)
);
------------------------------------------------------------------------------------------------------
--EVENTO
CREATE TABLE projetoBosch.dbo.evento(
	id int identity(1,1),
	nome nvarchar(50) not null,
    descricao nvarchar(255),
	descricaNormalizada as cast(upper(rtrim(ltrim(descricao))) as nvarchar(255)),
	dataInicio datetime,
	dataFim datetime,
	constraint pk_evento_id primary key(id)
);

INSERT INTO projetoBosch.dbo.evento (nome, descricao, dataInicio, dataFim) VALUES ('Natal', 'Natal Bosch 2021', '2021-12-23','2021-12-26')
------------------------------------------------------------------------------------------------------
--BENEFÍCIOS
CREATE TABLE projetoBosch.dbo.beneficio(
	id int identity(1,1),
    descricao nvarchar(255) not null,
	descricaNormalizada as cast(upper(rtrim(ltrim(descricao))) as nvarchar(255)),
	constraint pk_beneficio_id primary key(id)
);
INSERT INTO projetoBosch.dbo.beneficio (descricao) VALUES ('Cesta Seca'),
														('Cesta Fria'),
														('Presente'),
														('ME Kit 1'),
														('ME Kit 2'),
														('ME Kit 3'),
														('ME Kit 4'),
														('ME Kit 5'),
														('ME Kit 6'),		
														('Brinquedo M0'),
														('Brinquedo M1'),
														('Brinquedo M2'),
														('Brinquedo M3'),		
														('Brinquedo M4'),
														('Brinquedo M5'),
														('Brinquedo M6'),					
														('Brinquedo M7'),
														('Brinquedo M8'),
														('Brinquedo M9'),				
														('Brinquedo M10'),
														('Brinquedo M11'),
														('Brinquedo M12'),
														('Brinquedo F0'),
														('Brinquedo F1'),
														('Brinquedo F2'),
														('Brinquedo F3'),
														('Brinquedo F4'),
														('Brinquedo F5'),
														('Brinquedo F6'),
														('Brinquedo F7'),
														('Brinquedo F8'),
														('Brinquedo F9'),
														('Brinquedo F10'),
														('Brinquedo F11'),
														('Brinquedo F12');
------------------------------------------------------------------------------------------------------
--BENEFÍCIO-EVENTO
CREATE TABLE projetoBosch.dbo.BeneficioEvento(
	beneficioID int,
	eventoID int,
	constraint pk_beneficioEvento_id primary key(beneficioID, eventoID),
	constraint fk_beneficioEvento_beneficioID foreign key(beneficioID) references projetoBosch.dbo.beneficio(id),
	constraint fk_fk_beneficioEvento_beneficioID_eventoID foreign key(eventoID) references projetoBosch.dbo.evento(id),
)

INSERT INTO projetoBosch.dbo.BeneficioEvento(beneficioID, eventoID) VALUES (1, 1),
									(2, 1),
									(3, 1),
									(4, 1),
									(5, 1),
									(6, 1),
									(7, 1),
									(8, 1),
									(9, 1),		
									(10, 1),
									(11, 1),
									(12, 1),
									(13, 1),		
									(14, 1),
									(15, 1),
									(16, 1),					
									(17, 1),
									(18, 1),
									(19, 1),			
									(20, 1),
									(21, 1),
									(22, 1),
									(23, 1),
									(24, 1),
									(25, 1),
									(26, 1),
									(27, 1),
									(28, 1),
									(29, 1),
									(30, 1),
									(31, 1),
									(32, 1),
									(33, 1),
									(34, 1),
									(35, 1);
------------------------------------------------------------------------------------------------------
--DIREITO
CREATE TABLE projetoBosch.dbo.direito(
	id bigint identity(1,1),
	colaboradorID int not null,
	eventoID int not null,
	beneficioID int,
	indicadoID int,
	retiradoID int,
	dataRetirada datetime,
	constraint pk_direito_id primary key(id),
	constraint fk_direito_colaboradorID foreign key(colaboradorID) references colaborador(id),
	constraint fk_direito_eventoID foreign key(eventoID) references evento(id),
	constraint fk_direito_beneficioID foreign key(beneficioID) references beneficio(id),
	constraint fk_direito_indicadoID foreign key(indicadoID) references colaborador(id),
	constraint fk_direito_retiradoID foreign key(retiradoID) references colaborador(id)
);
GO
------------------------------------------------------------------------------------------------------------------------
-----------------------------------------INCLUSÃO CESTA SECA-------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------


INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
SELECT C.id, 1 as idEvento , B.id FROM projetoBosch.dbo.CestaSeca$ CS
INNER JOIN projetoBosch.dbo.colaborador C ON C.cpf = CS.CPF
INNER JOIN projetoBosch.dbo.beneficio B ON B.descricaNormalizada = 'CESTA SECA'
------------------------------------------------------------------------------------------------------------------------
-----------------------------------------INCLUSÃO CESTA FRIA-------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------
GO
INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
SELECT C.id, 1 as idEvento , B.id FROM projetoBosch.dbo.CestaFria$ CF
INNER JOIN projetoBosch.dbo.colaborador C ON C.cpf = CF.CPF
INNER JOIN projetoBosch.dbo.beneficio B ON B.descricaNormalizada = 'CESTA FRIA'
------------------------------------------------------------------------------------------------------------------------
-----------------------------------------INCLUSÃO PRESENTES-------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------
GO
INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
SELECT C.id, 1 as idEvento , B.id FROM projetoBosch.dbo.PresenteBosch$ PB
INNER JOIN projetoBosch.dbo.colaborador C ON C.cpf = PB.CPF
INNER JOIN projetoBosch.dbo.beneficio B ON B.descricaNormalizada = 'PRESENTE'
------------------------------------------------------------------------------------------------------------------------
-----------------------------------------INCLUSÃO KITS ESCOLARES-------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------
GO
DECLARE @MyCursor CURSOR;
DECLARE @CPF nvarchar(255);
DECLARE @kit1 varchar(2);
DECLARE @kit2 varchar(2);
DECLARE @kit3 varchar(2);
DECLARE @kit4 varchar(2);
DECLARE @kit5 varchar(2);
DECLARE @kit6 varchar(2);
BEGIN
    SET @MyCursor = CURSOR FOR
    SELECT CPF, KIT1, KIT2, KIT3, KIT4, KIT5, KIT6 FROM projetoBosch.dbo.MaterialEscolar$
   

    OPEN @MyCursor 
    FETCH NEXT FROM @MyCursor 
    INTO @CPF,@kit1,@kit2,@kit3,@kit4,@kit5,@kit6

    WHILE @@FETCH_STATUS = 0
    BEGIN
		declare @idColaborador int;
		declare @beneficioID int;
		SELECT @idColaborador = id FROM projetoBosch.dbo.colaborador WHERE cpf=@CPF;
		IF @kit1 IS NOT NULL
			BEGIN
				SELECT @beneficioID = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='ME KIT 1';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador, 1, @beneficioID)			
			END;
		IF @kit2 IS NOT NULL
			BEGIN
				SELECT @beneficioID = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='ME KIT 2';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador, 1, @beneficioID)			
			END;
		IF @kit3 IS NOT NULL
			BEGIN
				SELECT @beneficioID = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='ME KIT 3';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador, 1, @beneficioID)			
			END;
		IF @kit4 IS NOT NULL
			BEGIN
				SELECT @beneficioID = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='ME KIT 4';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador, 1, @beneficioID)			
			END;		
		IF @kit5 IS NOT NULL
			BEGIN
				SELECT @beneficioID = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='ME KIT 5';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador, 1, @beneficioID)			
			END;
		IF @kit6 IS NOT NULL
			BEGIN
				SELECT @beneficioID = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='ME KIT 6';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador, 1, @beneficioID)			
			END;
      
      FETCH NEXT FROM @MyCursor 
      INTO @CPF,@kit1,@kit2,@kit3,@kit4,@kit5,@kit6 
    END; 

    CLOSE @MyCursor ;
    DEALLOCATE @MyCursor;
END;
------------------------------------------------------------------------------------------------------------------------
---------------------------------------------INCLUSÂO BRINQUEDOS---------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------
GO
DECLARE @MyCursor2 CURSOR;
DECLARE @CPF2 nvarchar(255);
DECLARE @B0 varchar(3);
DECLARE @B1 varchar(3);
DECLARE @B2 varchar(3);
DECLARE @B3 varchar(3);
DECLARE @B4 varchar(3);
DECLARE @B5 varchar(3);
DECLARE @B6 varchar(3);
DECLARE @B7 varchar(3);
DECLARE @B8 varchar(3);
DECLARE @B9 varchar(3);
DECLARE @B10 varchar(3);
DECLARE @B11 varchar(3);
DECLARE @B12 varchar(3);
BEGIN
    SET @MyCursor2 = CURSOR FOR
    SELECT CPF, BRINQUEDO0, BRINQUEDO1, BRINQUEDO2, BRINQUEDO3, BRINQUEDO4, 
	BRINQUEDO5,BRINQUEDO6, BRINQUEDO7, BRINQUEDO8, BRINQUEDO9, BRINQUEDO10,
	BRINQUEDO11, BRINQUEDO12 FROM projetoBosch.dbo.Brinquedos$
   

    OPEN @MyCursor2 
    FETCH NEXT FROM @MyCursor2 
    INTO @CPF2,@B0,@B1,@B2, @B3,@B4,@B5,@B6,@B7,@B8,@B9,@B10,@B11,@B12

    WHILE @@FETCH_STATUS = 0
    BEGIN
		declare @idColaborador2 int;
		declare @beneficioID2 int;
		declare @genero varchar(1);
		SELECT @idColaborador2 = id FROM projetoBosch.dbo.colaborador WHERE cpf=@CPF2;
		IF @B0 IS NOT NULL
			BEGIN
				SELECT @genero = SUBSTRING(@B0, 1, 2);
				IF @genero='M'
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO F0';
				ELSE
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO M0';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador2, 1, @beneficioID2)			
			END;
		IF @B1 IS NOT NULL
			BEGIN
				SELECT @genero = SUBSTRING(@B0, 1, 2);
				IF @genero='M'
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO F1';
				ELSE
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO M1';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador2, 1, @beneficioID2)			
			END;
		IF @B2 IS NOT NULL
			BEGIN
				SELECT @genero = SUBSTRING(@B0, 1, 2);
				IF @genero='M'
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO F2';
				ELSE
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO M2';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador2, 1, @beneficioID2)			
			END;
		IF @B3 IS NOT NULL
			BEGIN
				SELECT @genero = SUBSTRING(@B0, 1, 2);
				IF @genero='M'
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO F3';
				ELSE
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO M3';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador2, 1, @beneficioID2)			
			END;	
		IF @B4 IS NOT NULL
			BEGIN
				SELECT @genero = SUBSTRING(@B0, 1, 2);
				IF @genero='M'
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO F4';
				ELSE
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO M4';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador2, 1, @beneficioID2)			
			END;
		IF @B5 IS NOT NULL
			BEGIN
				SELECT @genero = SUBSTRING(@B0, 1, 2);
				IF @genero='M'
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO F5';
				ELSE
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO M5';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador2, 1, @beneficioID2)			
			END;
		IF @B6 IS NOT NULL
			BEGIN
				SELECT @genero = SUBSTRING(@B0, 1, 2);
				IF @genero='M'
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO F6';
				ELSE
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO M6';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador2, 1, @beneficioID2)			
			END;
		IF @B7 IS NOT NULL
			BEGIN
				SELECT @genero = SUBSTRING(@B0, 1, 2);
				IF @genero='M'
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO F7';
				ELSE
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO M7';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador2, 1, @beneficioID2)			
			END;
		IF @B8 IS NOT NULL
			BEGIN
				SELECT @genero = SUBSTRING(@B0, 1, 2);
				IF @genero='M'
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO F8';
				ELSE
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO M8';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador2, 1, @beneficioID2)			
			END;
		IF @B9 IS NOT NULL
			BEGIN
				SELECT @genero = SUBSTRING(@B0, 1, 3); --A partir daqui, o gênero é o 3o dígito
				IF @genero='M'
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO F9';
				ELSE
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO M9';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador2, 1, @beneficioID2)			
			END;
		IF @B10 IS NOT NULL
			BEGIN
				SELECT @genero = SUBSTRING(@B0, 1, 3);
				IF @genero='M'
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO F10';
				ELSE
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO M10';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador2, 1, @beneficioID2)			
			END;
		IF @B11 IS NOT NULL
			BEGIN
				SELECT @genero = SUBSTRING(@B0, 1, 3);
				IF @genero='M'
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO F11';
				ELSE
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO M11';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador2, 1, @beneficioID2)			
			END;
		IF @B12 IS NOT NULL
			BEGIN
				SELECT @genero = SUBSTRING(@B0, 1, 3);
				IF @genero='M'
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO F12';
				ELSE
					SELECT @beneficioID2 = id FROM projetoBosch.dbo.beneficio WHERE descricaNormalizada='BRINQUEDO M12';
				INSERT INTO projetoBosch.dbo.direito(colaboradorID, eventoID, beneficioID)
				VALUES (@idColaborador2, 1, @beneficioID2)			
			END;     
    FETCH NEXT FROM @MyCursor2 
    INTO @CPF2,@B0,@B1,@B2, @B3,@B4,@B5,@B6,@B7,@B8,@B9,@B10,@B11,@B12
    END; 

    CLOSE @MyCursor2 ;
    DEALLOCATE @MyCursor2;
END;
GO
------------------------------------------------------------------------------------------------------------------------
---------------------------------------------REMOÇÃO DAS TABELAS DE CSV---------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------------------------
DROP TABLE projetoBosch.dbo.CestaSeca$;
DROP TABLE projetoBosch.dbo.CestaFria$;
DROP TABLE projetoBosch.dbo.PresenteBosch$;
DROP TABLE projetoBosch.dbo.MaterialEscolar$;
DROP TABLE projetoBosch.dbo.Brinquedos$;

