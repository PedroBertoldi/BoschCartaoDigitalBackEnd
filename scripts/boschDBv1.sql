CREATE DATABASE projetoBosch;
GO

CREATE TABLE projetoBosch.dbo.tipoPermissao (
    id int identity(1,1),
    descricao nvarchar(255) not null,
	descricaNormalizada as cast(upper(rtrim(ltrim(descricao))) as nvarchar(255)),
	constraint pk_tipoPermissao_id primary key(id)
);

CREATE TABLE projetoBosch.dbo.unidadeOrganizacional (
    id int identity(1,1),
    descricao nvarchar(255) not null,
	descricaNormalizada as cast(upper(rtrim(ltrim(descricao))) as nvarchar(255)),
	constraint pk_unidadeOrganizacional_id primary key(id)
);

CREATE TABLE projetoBosch.dbo.colaborador (
    id int identity(1,1),
	cpf varchar(11) not null,
	nomeCompleto nvarchar(255),
	dataNascimento date,
	unidadeOrganizacionalID int,
	constraint pk_colaborador_id primary key(id),
	constraint fk_colaborador_unidadeOrganizacionalID foreign key(unidadeOrganizacionalID) references unidadeOrganizacional(id)
);

CREATE TABLE projetoBosch.dbo.permissao(
	colaboradorID int,
	tipoPermissaoID int,
	constraint pk_permissao_colaboradorID_tipoPermissaoID primary key(colaboradorID, tipoPermissaoID),
	constraint fk_permissao_colaboradorID foreign key(colaboradorID) references colaborador(id),
	constraint fk_permissao_tipoPermissaoID foreign key(tipoPermissaoID) references tipoPermissao(id)
);

CREATE TABLE projetoBosch.dbo.evento(
	id int identity(1,1),
    descricao nvarchar(255) not null,
	descricaNormalizada as cast(upper(rtrim(ltrim(descricao))) as nvarchar(255)),
	dataInicio datetime,
	dataFim datetime,
	constraint pk_evento_id primary key(id)
);

CREATE TABLE projetoBosch.dbo.beneficio(
	id int identity(1,1),
    descricao nvarchar(255) not null,
	descricaNormalizada as cast(upper(rtrim(ltrim(descricao))) as nvarchar(255)),
	constraint pk_beneficio_id primary key(id)
);

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
