use master
drop database if exists DISTRIBUIDORA
CREATE DATABASE DISTRIBUIDORA
GO

USE DISTRIBUIDORA

drop table if exists PROVEEDORES 
CREATE TABLE PROVEEDORES(
IdProveedor INT IDENTITY(1,1) PRIMARY KEY,
Nombre VARCHAR(50),
Descripcion VARCHAR(50)
)

drop table if exists TIPO_PRODUCTO 
CREATE TABLE TIPO_PRODUCTO(
IdTipoProducto INT IDENTITY(1,1) PRIMARY KEY,
Nombre VARCHAR(50),
Descripcion VARCHAR(50)
)

drop table if exists PRODUCTOS 
CREATE TABLE PRODUCTOS(
IdProducto INT IDENTITY(1,1) PRIMARY KEY,
Nombre VARCHAR(50),
Clave VARCHAR(8),
Precio DECIMAL,
EsActivo INT,
IdTipoProducto INT FOREIGN KEY REFERENCES TIPO_PRODUCTO(IdTipoProducto)
)


drop table if exists PRODUCTOSPROVEEDORES
CREATE TABLE PRODUCTOSPROVEEDORES(
IdProductosProveedores INT IDENTITY(1,1) PRIMARY KEY,
IdProducto INT FOREIGN KEY REFERENCES PRODUCTOS(IdProducto),
IdProveedor INT FOREIGN KEY REFERENCES PROVEEDORES(IdProveedor),
Clave VARCHAR(8),
Precio DECIMAL
)

go


INSERT INTO PROVEEDORES VALUES('BODEGA AURRERA','DISTRIBUIDORA')
INSERT INTO PROVEEDORES VALUES('SAN JORGE','DISTRIBUIDORA')
INSERT INTO PROVEEDORES VALUES('EL ZORRO','DISTRIBUIDORA')
INSERT INTO PROVEEDORES VALUES('BARA','DISTRIBUIDORA')

INSERT INTO TIPO_PRODUCTO VALUES('LIMPIADOR','FABULOSO')
INSERT INTO TIPO_PRODUCTO VALUES('JABON','ACE')
INSERT INTO TIPO_PRODUCTO VALUES('LIMPIADOR','PINOL')
INSERT INTO TIPO_PRODUCTO VALUES('JABON','ARIEL')
INSERT INTO TIPO_PRODUCTO VALUES('HIGIENE PERSONAL','SHAMPOO')
INSERT INTO TIPO_PRODUCTO VALUES('HIGIENE PERSONAL','ACONDICIONADOR')
INSERT INTO TIPO_PRODUCTO VALUES('JABON DE TOCADOR','ZETS')
INSERT INTO TIPO_PRODUCTO VALUES('JABON DE TOCADOR','PALMOLIVE')
INSERT INTO TIPO_PRODUCTO VALUES('BEBIDA','SODA')

INSERT INTO PRODUCTOS VALUES ('LIMPIADOR FABULOSO','FAB_0001',18,1,1)
INSERT INTO PRODUCTOS VALUES ('JABON ARIEL','ARIEL01',15,1,2)
INSERT INTO PRODUCTOS VALUES ('LIMPIADOR PINOL','PINOL001',13,1,1)

INSERT INTO PRODUCTOSPROVEEDORES VALUES(1,1,'JAR_01',12)

go

--exec SP_ObtenerProductos
create procedure dbo.SP_ObtenerProductos
   as
begin 
       select prod.IdProducto, prod.Nombre, prod.Clave, prod.Precio from PRODUCTOS prod
	   where EsActivo = 1
end
GO

--exec dbo.SP_BuscarProducto @Clave ='L', @IdTipoProducto = 0
create procedure dbo.SP_BuscarProducto
   @Clave VARCHAR(8) = '',
   @IdTipoProducto INT = 0
   as 
begin 
	declare @Query varchar(500), @Bandera int = 0
	set @Query = 'select prod.Nombre, prod.Clave, prod.Precio  from PRODUCTOS prod '

	if @Clave <> '' or @IdTipoProducto <> 0
		set @Query += 'where '
		

	if @Clave <> '' 
	begin
		set @Query += 'prod.Clave like ''%' + @Clave + '%'''
		set @Bandera = 1
	end

	if @IdTipoProducto <> 0
	begin
		if @Bandera = 1  
		begin
			set @Query += 'and '
		end
		set @Query += 'prod.IdTipoProducto = ' + CONVERT(varchar(3), @IdTipoProducto) + ''
	end

       exec(@Query)
end
GO


/*exec SP_AgregarProducto 
	@Nombre = 'FANTA 600ml', 
	@Clave = 'FTA-001',  
	@Precio = 18,	
	@EsActivo = 1, 
	@IdTipoProducto = 9 */
create procedure [dbo].[SP_AgregarProducto]
	@Nombre VARCHAR(50),
	@Clave VARCHAR(8),
	@Precio DECIMAL,
	@EsActivo INT,
	@IdTipoProducto INT
   as
begin 
       INSERT INTO PRODUCTOS VALUES 
	   (@Nombre, @Clave, @Precio, @EsActivo, @IdTipoProducto)
end
GO

/*exec SP_ModificarProducto 
	@IdProducto = 5,
	@Nombre = 'FANTA 800ml', 
	@Clave = 'FTA-001',  
	@Precio = 18,	
	@EsActivo = 1, 
	@IdTipoProducto = 9 */
create procedure dbo.SP_ModificarProducto
	@IdProducto INT,
	@Nombre VARCHAR(50),
	@Clave VARCHAR(8),
	@Precio DECIMAL,
	@EsActivo INT,
	@IdTipoProducto INT
   as
begin 
	update PRODUCTOS 
	set Nombre = @Nombre, Clave = @Clave, Precio = @Precio,	EsActivo = @EsActivo, IdTipoProducto = @IdTipoProducto
	where IdProducto = @IdProducto
end
GO

--exec SP_EliminarProducto @IdProducto = 5
create procedure [dbo].[SP_EliminarProducto]
	@IdProducto int
   as
begin 
	   delete from PRODUCTOSPROVEEDORES where IdProducto = @IdProducto
       delete from PRODUCTOS where IdProducto = @IdProducto
  
end
GO
--exec SP_ObtenerTipoProducto
create procedure dbo.SP_ObtenerTipoProducto
   as
begin 
       select TProd.IdTipoProducto, TProd.Nombre, TProd.Descripcion from TIPO_PRODUCTO TProd 
end
GO
--exec SP_ObtenerProveedores
create procedure dbo.SP_ObtenerProveedores
   as
begin 
       select prov.IdProveedor, prov.Nombre, prov.Descripcion from PROVEEDORES prov 
end
GO

/*exec SP_AgregarProductoProveedor 
	@IdProducto = 1,
    @IdProveedor = 1,  
	@Clave = 'FTA-001',  
	@Precio = 18*/
create procedure dbo.SP_AgregarProductoProveedor
    @IdProducto INT,
	@IdProveedor INT ,
	@Clave VARCHAR(8),
	@Precio DECIMAL
   as
begin 
       INSERT INTO PRODUCTOSPROVEEDORES VALUES (@IdProducto, @IdProveedor, @Clave, @Precio)
end
GO

/*exec SP_ModificarProductoProveedor 
    @IdProductosProveedores =1,        
	@IdProducto = 1,
    @IdProveedor = 1,  
	@Clave = 'FTA-001',  
	@Precio = 18*/
create procedure dbo.SP_ModificarProductoProveedor
    @IdProductosProveedores INT,
    @IdProducto INT,
	@IdProveedor INT ,
	@Clave VARCHAR(8),
	@Precio DECIMAL
   as
begin 
    UPDATE PRODUCTOSPROVEEDORES SET IdProducto = @IdProducto, IdProveedor = @IdProveedor, Clave = @Clave, Precio = @Precio
    where IdProductosProveedores = @IdProductosProveedores
end
GO

--exec SP_ObtenerProductoProveedor @IdProducto = 1
create procedure [dbo].[SP_ObtenerProductoProveedor]
	@IdProducto INT
   as
begin 
       select P.Nombre, PP.*
	   from PRODUCTOSPROVEEDORES PP
	   inner join PROVEEDORES P on PP.IdProveedor = P.IdProveedor
	   where IdProducto = @IdProducto
end
GO

--exec SP_EliminarProductosProveedor @IdProductosProveedores = 5
create procedure [dbo].[SP_EliminarProductosProveedores]
	@IdProductosProveedores int
   as
begin 
       delete from PRODUCTOSPROVEEDORES where IdProductosProveedores = @IdProductosProveedores
end
GO

