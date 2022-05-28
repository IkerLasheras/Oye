USE [Proyecto]
GO
/****** Object:  Table [dbo].[Bebidas]    Script Date: 28/05/2022 11:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bebidas](
	[idBebida] [int] NOT NULL,
	[tipoBebida] [int] NOT NULL,
	[precio] [numeric](5, 2) NOT NULL,
	[imgBebida] [image] NULL,
	[nombreBebida] [nchar](20) NULL,
 CONSTRAINT [PK_IdBebida] PRIMARY KEY CLUSTERED 
(
	[idBebida] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categorias]    Script Date: 28/05/2022 11:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categorias](
	[idCategoria] [int] NOT NULL,
	[descripcionCategoria] [nchar](100) NOT NULL,
	[imgCategoria] [image] NULL,
	[nombreCategoria] [nchar](20) NULL,
 CONSTRAINT [PK_idCategoria] PRIMARY KEY NONCLUSTERED 
(
	[idCategoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DescripcionPedido]    Script Date: 28/05/2022 11:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DescripcionPedido](
	[idPedido] [int] NULL,
	[idVendido] [int] NULL,
	[tipo] [char](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empleados]    Script Date: 28/05/2022 11:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empleados](
	[idEmpleado] [int] NOT NULL,
	[nombre] [nchar](15) NOT NULL,
	[apellido1] [nchar](15) NOT NULL,
	[apellido2] [nchar](15) NULL,
	[dni] [nchar](9) NOT NULL,
	[telefono] [nchar](9) NULL,
	[email] [nchar](40) NULL,
	[fechNac] [date] NULL,
	[localidad] [nchar](25) NULL,
	[direccion] [nchar](25) NULL,
	[cp] [int] NULL,
	[tipoEmpleado] [int] NULL,
	[cuentaCredito] [nchar](24) NULL,
	[fechIncicioContrato] [date] NOT NULL,
	[imgEmpleado] [image] NULL,
 CONSTRAINT [PK_idEmpleado] PRIMARY KEY NONCLUSTERED 
(
	[idEmpleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IngredientesPlatos]    Script Date: 28/05/2022 11:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IngredientesPlatos](
	[idPlato] [int] NULL,
	[idProducto] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mesas]    Script Date: 28/05/2022 11:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mesas](
	[idMesa] [int] NOT NULL,
	[ubicacion] [nchar](50) NULL,
	[disponible] [bit] NOT NULL,
 CONSTRAINT [PK_idMesa] PRIMARY KEY NONCLUSTERED 
(
	[idMesa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pedidos]    Script Date: 28/05/2022 11:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pedidos](
	[idMesa] [int] NOT NULL,
	[idPedidos] [int] NOT NULL,
	[idEmpleado] [int] NULL,
	[fechaPedido] [datetime] NULL,
	[costeTotal] [numeric](8, 2) NULL,
 CONSTRAINT [PK_idPedidos] PRIMARY KEY NONCLUSTERED 
(
	[idPedidos] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Platos]    Script Date: 28/05/2022 11:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Platos](
	[idPlato] [int] NOT NULL,
	[precio] [numeric](5, 2) NULL,
	[tipoPlato] [int] NULL,
	[nombrePlato] [nchar](20) NULL,
	[imgPlato] [image] NULL,
 CONSTRAINT [PK_idPlato] PRIMARY KEY NONCLUSTERED 
(
	[idPlato] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Productos]    Script Date: 28/05/2022 11:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Productos](
	[idProducto] [int] NOT NULL,
	[codigoProducto] [nchar](15) NOT NULL,
	[nombreProducto] [nchar](20) NOT NULL,
	[cantidadStock] [int] NOT NULL,
	[categoriaProducto] [int] NOT NULL,
	[costeUnitario] [numeric](4, 2) NOT NULL,
	[imgProducto] [image] NULL,
	[unidadMedida] [nchar](4) NULL,
	[ubicacion] [int] NULL,
 CONSTRAINT [PK_idProducto] PRIMARY KEY NONCLUSTERED 
(
	[idProducto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Proveedores]    Script Date: 28/05/2022 11:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proveedores](
	[idProveedor] [int] NOT NULL,
	[categoriaProveedor] [int] NOT NULL,
	[nombreProveedor] [varchar](25) NOT NULL,
	[localizacionProveedor] [varchar](50) NULL,
	[iban] [nchar](24) NULL,
	[direccion] [nchar](50) NULL,
	[telefono] [nchar](11) NULL,
	[email] [nchar](50) NULL,
	[imgProveedor] [image] NULL,
 CONSTRAINT [PK_idProveedor] PRIMARY KEY NONCLUSTERED 
(
	[idProveedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tipobebida]    Script Date: 28/05/2022 11:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tipobebida](
	[idTipoBebida] [int] NOT NULL,
	[nombre] [nchar](20) NOT NULL,
 CONSTRAINT [PK_idTipoBebida] PRIMARY KEY CLUSTERED 
(
	[idTipoBebida] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoEmpleado]    Script Date: 28/05/2022 11:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoEmpleado](
	[idTipoEmpleado] [int] NOT NULL,
	[nombreTipoEmpleado] [nchar](25) NOT NULL,
	[sueldoBase] [numeric](6, 2) NULL,
 CONSTRAINT [PK_idTipoEmpleado] PRIMARY KEY CLUSTERED 
(
	[idTipoEmpleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoPlatos]    Script Date: 28/05/2022 11:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoPlatos](
	[idTipoPlato] [int] NOT NULL,
	[nombreTipo] [nchar](15) NOT NULL,
 CONSTRAINT [PK_idTipoPlato] PRIMARY KEY CLUSTERED 
(
	[idTipoPlato] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ubicacion]    Script Date: 28/05/2022 11:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ubicacion](
	[idUbicacion] [int] NOT NULL,
	[nombreUbicacion] [nchar](20) NULL,
	[descripcionUbicacion] [nchar](50) NULL,
 CONSTRAINT [PK_idUbicacion] PRIMARY KEY CLUSTERED 
(
	[idUbicacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Bebidas]  WITH CHECK ADD  CONSTRAINT [FK_TipoBebida] FOREIGN KEY([tipoBebida])
REFERENCES [dbo].[Tipobebida] ([idTipoBebida])
GO
ALTER TABLE [dbo].[Bebidas] CHECK CONSTRAINT [FK_TipoBebida]
GO
ALTER TABLE [dbo].[DescripcionPedido]  WITH CHECK ADD  CONSTRAINT [FK_IdPedidosDescripcion] FOREIGN KEY([idPedido])
REFERENCES [dbo].[Pedidos] ([idPedidos])
GO
ALTER TABLE [dbo].[DescripcionPedido] CHECK CONSTRAINT [FK_IdPedidosDescripcion]
GO
ALTER TABLE [dbo].[DescripcionPedido]  WITH CHECK ADD  CONSTRAINT [FK_VendidoBebidas] FOREIGN KEY([idVendido])
REFERENCES [dbo].[Bebidas] ([idBebida])
GO
ALTER TABLE [dbo].[DescripcionPedido] CHECK CONSTRAINT [FK_VendidoBebidas]
GO
ALTER TABLE [dbo].[DescripcionPedido]  WITH CHECK ADD  CONSTRAINT [FK_VendidoPlatos] FOREIGN KEY([idVendido])
REFERENCES [dbo].[Platos] ([idPlato])
GO
ALTER TABLE [dbo].[DescripcionPedido] CHECK CONSTRAINT [FK_VendidoPlatos]
GO
ALTER TABLE [dbo].[Empleados]  WITH CHECK ADD  CONSTRAINT [FK_TipoEMpleado] FOREIGN KEY([tipoEmpleado])
REFERENCES [dbo].[TipoEmpleado] ([idTipoEmpleado])
GO
ALTER TABLE [dbo].[Empleados] CHECK CONSTRAINT [FK_TipoEMpleado]
GO
ALTER TABLE [dbo].[IngredientesPlatos]  WITH CHECK ADD  CONSTRAINT [FK_idPlato] FOREIGN KEY([idPlato])
REFERENCES [dbo].[Platos] ([idPlato])
GO
ALTER TABLE [dbo].[IngredientesPlatos] CHECK CONSTRAINT [FK_idPlato]
GO
ALTER TABLE [dbo].[IngredientesPlatos]  WITH CHECK ADD  CONSTRAINT [FK_idProducto] FOREIGN KEY([idProducto])
REFERENCES [dbo].[Productos] ([idProducto])
GO
ALTER TABLE [dbo].[IngredientesPlatos] CHECK CONSTRAINT [FK_idProducto]
GO
ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD  CONSTRAINT [FK_idEmpleado] FOREIGN KEY([idEmpleado])
REFERENCES [dbo].[Empleados] ([idEmpleado])
GO
ALTER TABLE [dbo].[Pedidos] CHECK CONSTRAINT [FK_idEmpleado]
GO
ALTER TABLE [dbo].[Pedidos]  WITH CHECK ADD  CONSTRAINT [FK_idMesa] FOREIGN KEY([idMesa])
REFERENCES [dbo].[Mesas] ([idMesa])
GO
ALTER TABLE [dbo].[Pedidos] CHECK CONSTRAINT [FK_idMesa]
GO
ALTER TABLE [dbo].[Platos]  WITH CHECK ADD  CONSTRAINT [FK_idTipoPlato] FOREIGN KEY([tipoPlato])
REFERENCES [dbo].[TipoPlatos] ([idTipoPlato])
GO
ALTER TABLE [dbo].[Platos] CHECK CONSTRAINT [FK_idTipoPlato]
GO
ALTER TABLE [dbo].[Productos]  WITH CHECK ADD  CONSTRAINT [FK_CategoriaProducto] FOREIGN KEY([categoriaProducto])
REFERENCES [dbo].[Categorias] ([idCategoria])
GO
ALTER TABLE [dbo].[Productos] CHECK CONSTRAINT [FK_CategoriaProducto]
GO
ALTER TABLE [dbo].[Productos]  WITH CHECK ADD  CONSTRAINT [FK_Ubicacion] FOREIGN KEY([ubicacion])
REFERENCES [dbo].[Ubicacion] ([idUbicacion])
GO
ALTER TABLE [dbo].[Productos] CHECK CONSTRAINT [FK_Ubicacion]
GO
ALTER TABLE [dbo].[Proveedores]  WITH CHECK ADD  CONSTRAINT [FK_CategoriaProveedores] FOREIGN KEY([categoriaProveedor])
REFERENCES [dbo].[Categorias] ([idCategoria])
GO
ALTER TABLE [dbo].[Proveedores] CHECK CONSTRAINT [FK_CategoriaProveedores]
GO
