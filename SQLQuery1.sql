create database [web-api];

use [web-api];

-- Criação da tabela de carros Id, Nome e Valor --

create table Carro (
	id int identity(1, 1) not null primary key,
	nome varchar(100) not null,
	valor numeric(13,2) not null
);