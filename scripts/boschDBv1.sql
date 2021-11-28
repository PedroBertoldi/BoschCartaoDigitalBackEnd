CREATE DATABASE projetoBosch;
GO

CREATE TABLE projetoBosch.dbo.tipoPermissao (
    id int identity(1,1) primary key,
    descricao nvarchar(255) not null,
	descricaNormalizada as cast(upper(rtrim(ltrim(descricao))) as nvarchar(255))
);

CREATE TABLE projetoBosch.dbo.unidadeOrganizacional (
    id int identity(1,1) primary key,
    descricao nvarchar(255) not null,
	descricaNormalizada as cast(upper(rtrim(ltrim(descricao))) as nvarchar(255))
);

CREATE TABLE projetoBosch.dbo.colaborador (
    id int identity(1,1) primary key,
	cpf varchar(11) not null,
	nomeCompleto nvarchar(255),
	dataNascimento date,
	unidadeOrganizacionalID int,
	constraint colaborador_unidadeOrganizacionalID foreign key(unidadeOrganizacionalID)
	references unidadeOrganizacional(id)
);

CREATE TABLE projetoBosch.dbo.permissao(
	colaboradorID int,
	tipoPermissaoID int,
	primary key(colaboradorID, tipoPermissaoID),
	constraint permissao_colaboradorID foreign key(colaboradorID)
	references colaborador(id),
	constraint permissao_tipoPermissaoID foreign key(tipoPermissaoID) 
	references tipoPermissao(id)
);

CREATE TABLE projetoBosch.dbo.evento(
	id int identity(1,1) primary key,
    descricao nvarchar(255) not null,
	descricaNormalizada as cast(upper(rtrim(ltrim(descricao))) as nvarchar(255)),
	dataInicio datetime,
	dataFim datetime
);

CREATE TABLE projetoBosch.dbo.beneficio(
	id int identity(1,1) primary key,
    descricao nvarchar(255) not null,
	descricaNormalizada as cast(upper(rtrim(ltrim(descricao))) as nvarchar(255))
);

CREATE TABLE projetoBosch.dbo.direito(
	id bigint identity(1,1) primary key,
	colaboradorID int not null,
	eventoID int not null,
	beneficioID int,
	indicadoID int,
	retiradoID int,
	dataRetirada datetime,
	constraint direito_colaboradorID
	foreign key(colaboradorID) references colaborador(id),
	constraint direito_eventoID
	foreign key(eventoID) references evento(id),
	constraint direito_beneficioID
	foreign key(beneficioID) references beneficio(id),
	constraint direito_indicadoID
	foreign key(indicadoID) references colaborador(id),
	constraint direito_retiradoID
	foreign key(retiradoID) references colaborador(id)
);
