using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Producto
    {
        public static ML.Result Add(ML.Producto producto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using(DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    

                    var addProducto = context.Database.ExecuteSqlRaw($"ProductoAdd '{producto.Nombre}', {producto.PrecioUnitario}, {producto.Stock}, {producto.Proveedor.IdProveedor}, {producto.Departamento.IdDepartamento}, '{producto.Descripcion}', '{producto.Imagen}'  ");
                    
                    if(addProducto > 0)
                    {
                        result.Correct = true;
                        result.Message = "Se ha agregado el producto";
                    }
                
                }

            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.Message = "Ocurrio un error al agregar el producto";
                result.EX = ex;
                throw;
            }

            return result;
        }
        public static ML.Result GetAll(ML.Producto producto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using(DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    producto.Proveedor.IdProveedor = (producto.Proveedor.IdProveedor == null) ? producto.Proveedor.IdProveedor = 0 : producto.Proveedor.IdProveedor;

                    var productos = context.Productos.FromSqlRaw($"ProductoGetAll '{producto.Nombre}', {producto.Proveedor.IdProveedor}").ToList();

                    result.Objects = new List<object>();

                    if(productos != null)
                    {
                        foreach(var objProducto in productos)
                        {
                            ML.Producto productoGet = new ML.Producto();

                            productoGet.IdProducto = objProducto.IdProducto;
                            productoGet.Nombre = objProducto.Nombre;
                            productoGet.PrecioUnitario = objProducto.PrecioUnitario;
                            productoGet.Stock = objProducto.Stock;
                            productoGet.Descripcion = objProducto.Descripcion;
                            productoGet.Imagen = objProducto.Imagen;

                            productoGet.Proveedor = new ML.Proveedor();
                            productoGet.Proveedor.IdProveedor = objProducto.IdProveedor.Value;
                            productoGet.Proveedor.Telefono = objProducto.Telefono;

                            productoGet.Departamento = new ML.Departamento();
                            productoGet.Departamento.IdDepartamento = objProducto.IdDepartamento.Value;
                            productoGet.Departamento.Nombre = objProducto.DepartamentoNombre;
                            
                            productoGet.Departamento.Area = new ML.Area();
                            productoGet.Departamento.Area.IdArea = objProducto.IdArea;
                            productoGet.Departamento.Area.Nombre = objProducto.AreaNombre;

                            result.Objects.Add(productoGet);
                            

                        }
                    }


                }
                result.Correct = true;
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.EX = ex;
                result.Message = "Ocurrio un error al hacer la consulta";
                throw;
            }

            return result;
        }

        public static ML.Result GetById(int IdProducto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    var objProducto = context.Productos.FromSqlRaw($"ProductoGetById {IdProducto}").AsEnumerable().FirstOrDefault();

                    result.Object = new List<object>();

                    if(objProducto != null)
                    {
                        ML.Producto producto = new ML.Producto();

                        producto.IdProducto = objProducto.IdProducto;
                        producto.Nombre = objProducto.Nombre;
                        producto.PrecioUnitario = objProducto.PrecioUnitario;
                        producto.Stock = objProducto.Stock;
                        producto.Descripcion = objProducto.Descripcion;
                        producto.Imagen = objProducto.Imagen;

                        producto.Proveedor = new ML.Proveedor();
                        producto.Proveedor.IdProveedor = objProducto.IdProveedor.Value;
                        producto.Proveedor.Telefono = objProducto.Telefono;

                        producto.Departamento = new ML.Departamento();
                        producto.Departamento.IdDepartamento = objProducto.IdDepartamento.Value;
                        producto.Departamento.Nombre = objProducto.DepartamentoNombre;

                        producto.Departamento.Area = new ML.Area();
                        producto.Departamento.Area.IdArea = objProducto.IdArea;
                        producto.Departamento.Area.Nombre = objProducto.AreaNombre;

                        result.Object = producto;
                    }
                }
                result.Correct = true;
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.EX = ex;
                result.Message = "Ocurrio un error al hacer la consulta";
                throw;
            }

            return result;
        }

        public static ML.Result Update(ML.Producto producto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using(DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    var updatedProducto = context.Database.ExecuteSqlRaw($"ProductoUpdate  {producto.IdProducto}, '{producto.Nombre}', {producto.PrecioUnitario}, {producto.Stock}, {producto.Proveedor.IdProveedor}, {producto.Departamento.IdDepartamento}, '{producto.Descripcion}', '{producto.Imagen}'  ");

                    if(updatedProducto > 0)
                    {
                        result.Correct = true;
                        result.Message = "Se ha actualizado el producto";
                    }
                
                }
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.Message = "Ocurrio un error al agregar el producto";
                result.EX = ex;
                throw;
            }

            return result;
        }

        public static ML.Result Delete(int IdProducto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    var deletedProducto = context.Database.ExecuteSqlRaw($"ProductoDelete {IdProducto}");

                    if(deletedProducto > 0)
                    {
                        result.Correct = true;
                        result.Message = "Se ha eliminado el producto";
                    }
                }
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.EX = ex;
                result.Message = "Ocurrio un error al hacer la consulta";
                throw;
            }


            return result;
        }
    }
}
