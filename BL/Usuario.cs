using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.OleDb;

namespace BL
{
    public class Usuario
    {

        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            

            try
            {
                using (DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {


                    if(usuario.Imagen == null)
                    {
                        var addUsuario = context.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.Nombre}', '{usuario.ApellidoPaterno}', '{usuario.ApellidoMaterno}', '{usuario.FechaNacimiento}', '{usuario.Sexo}', '{usuario.Email}', '{usuario.UserName}', '{usuario.Password}', '{usuario.Telefono}', '{usuario.Celular}', '{usuario.CURP}', {usuario.Rol.IdRol}, null, '{usuario.Direccion.Calle}', '{usuario.Direccion.NumeroInterior}', '{usuario.Direccion.NumeroExterior}', {usuario.Direccion.Colonia.IdColonia} ");

                        if (addUsuario > 0)
                        {
                            result.Correct = true;
                            result.Message = "Se ha agregado el usuario";
                        }
                        else
                        {
                            result.Correct = false;
                            result.Message = "Ocurrio un error";
                        }
                    }
                    else
                    {
                        var addUsuario = context.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.Nombre}', '{usuario.ApellidoPaterno}', '{usuario.ApellidoMaterno}', '{usuario.FechaNacimiento}', '{usuario.Sexo}', '{usuario.Email}', '{usuario.UserName}', '{usuario.Password}', '{usuario.Telefono}', '{usuario.Celular}', '{usuario.CURP}', {usuario.Rol.IdRol}, '{usuario.Imagen}', '{usuario.Direccion.Calle}', '{usuario.Direccion.NumeroInterior}', '{usuario.Direccion.NumeroExterior}', {usuario.Direccion.Colonia.IdColonia} ");

                        if (addUsuario > 0)
                        {
                            result.Correct = true;
                            result.Message = "Se ha agregado el usuario";
                        }
                        else
                        {
                            result.Correct = false;
                            result.Message = "Ocurrio un error";
                        }
                    }

                    

                   

                }
            }
            catch (Exception ex)
            {
               
                result.Correct = false;
                result.EX = ex;
                result.Message = "Error: "+ result.EX +"\n";

                
                
            }
            return result;
        }
        public static ML.Result GetAll(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {

                    usuario.Rol.IdRol = (usuario.Rol.IdRol == null) ? usuario.Rol.IdRol=0 : usuario.Rol.IdRol;
                    var usuarios = context.Usuarios.FromSqlRaw($"UsuarioGetAll '{usuario.Nombre}', '{usuario.ApellidoPaterno}', {usuario.Rol.IdRol} ").ToList();
                    result.Objects = new List<object>();

                    if (usuarios != null)
                    {
                        foreach (var objUsuario in usuarios)
                        {
                            ML.Usuario usuarioGet = new ML.Usuario();

                            usuarioGet.IdUsuario = objUsuario.IdUsuario;
                            usuarioGet.Nombre = objUsuario.Nombre;
                            usuarioGet.ApellidoPaterno = objUsuario.ApellidoPaterno;
                            usuarioGet.ApellidoMaterno = objUsuario.ApellidoMaterno;
                            usuarioGet.FechaNacimiento = objUsuario.FechaNacimiento.ToString("dd-MM-yyyy");
                            usuarioGet.Sexo = objUsuario.Sexo;
                            usuarioGet.Email = objUsuario.Email;
                            usuarioGet.UserName = objUsuario.UserName;
                            usuarioGet.Password = objUsuario.Password;
                            usuarioGet.Telefono = objUsuario.Telefono;
                            usuarioGet.Celular = objUsuario.Celular;
                            usuarioGet.CURP = objUsuario.Curp;

                            usuarioGet.Rol = new ML.Rol();
                            usuarioGet.Rol.IdRol = objUsuario.IdRol.Value;
                            usuarioGet.Rol.Nombre = objUsuario.NombreRol;
                            usuarioGet.Imagen = objUsuario.Imagen;
                            usuarioGet.Estatus = objUsuario.Estatus;

                            usuarioGet.Direccion = new ML.Direccion();
                            usuarioGet.Direccion.IdDireccion = objUsuario.IdDireccion;
                            usuarioGet.Direccion.Calle = objUsuario.Calle;
                            usuarioGet.Direccion.NumeroInterior = objUsuario.NumeroInterior;
                            usuarioGet.Direccion.NumeroExterior = objUsuario.NumeroExterior;


                            usuarioGet.Direccion.Colonia = new ML.Colonia();
                            usuarioGet.Direccion.Colonia.IdColonia = objUsuario.IdColonia;
                            usuarioGet.Direccion.Colonia.Nombre = objUsuario.ColoniaNombre;
                            usuarioGet.Direccion.Colonia.CodigoPostal = objUsuario.CodigoPostal;

                            usuarioGet.Direccion.Colonia.Municipio = new ML.Municipio();
                            usuarioGet.Direccion.Colonia.Municipio.IdMunicipio = objUsuario.IdMunicipio;
                            usuarioGet.Direccion.Colonia.Municipio.Nombre = objUsuario.MunicipioNombre;

                            usuarioGet.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                            usuarioGet.Direccion.Colonia.Municipio.Estado.IdEstado = objUsuario.IdEstado;
                            usuarioGet.Direccion.Colonia.Municipio.Estado.Nombre = objUsuario.EstadoNombre;

                            usuarioGet.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                            usuarioGet.Direccion.Colonia.Municipio.Estado.Pais.IdPais = objUsuario.IdPais;
                            usuarioGet.Direccion.Colonia.Municipio.Estado.Pais.Nombre = objUsuario.PaisNombre;

                            result.Objects.Add(usuarioGet);
                        }
                    }

                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Message = "ocurrio un error al realizar la consuta";
                result.EX = ex;
                throw;
            }

            return result;
        }

        public static ML.Result GetById(int IdUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    var objUsuario = context.Usuarios.FromSqlRaw($"UsuarioGetById {IdUsuario}").AsEnumerable().FirstOrDefault();

                    result.Object = new List<object>();
                    if (objUsuario != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();

                        usuario.IdUsuario = objUsuario.IdUsuario;
                        usuario.Nombre = objUsuario.Nombre;
                        usuario.ApellidoPaterno = objUsuario.ApellidoPaterno;
                        usuario.ApellidoMaterno = objUsuario.ApellidoMaterno;
                        usuario.FechaNacimiento = objUsuario.FechaNacimiento.ToString("dd-MM-yyyy");
                        usuario.Sexo = objUsuario.Sexo;
                        usuario.Email = objUsuario.Email;
                        usuario.UserName = objUsuario.UserName;
                        usuario.Password = objUsuario.Password;
                        usuario.Telefono = objUsuario.Telefono;
                        usuario.Celular = objUsuario.Celular;
                        usuario.CURP = objUsuario.Curp;

                        usuario.Rol = new ML.Rol();
                        usuario.Rol.IdRol = objUsuario.IdRol.Value;
                        usuario.Rol.Nombre = objUsuario.NombreRol;
                        usuario.Imagen = objUsuario.Imagen;
                        usuario.Estatus = objUsuario.Estatus;

                        usuario.Direccion = new ML.Direccion();
                        usuario.Direccion.IdDireccion = objUsuario.IdDireccion;
                        usuario.Direccion.Calle = objUsuario.Calle;
                        usuario.Direccion.NumeroInterior = objUsuario.NumeroInterior;
                        usuario.Direccion.NumeroExterior = objUsuario.NumeroExterior;


                        usuario.Direccion.Colonia = new ML.Colonia();
                        usuario.Direccion.Colonia.IdColonia = objUsuario.IdColonia;
                        usuario.Direccion.Colonia.Nombre = objUsuario.ColoniaNombre;
                        usuario.Direccion.Colonia.CodigoPostal = objUsuario.CodigoPostal;

                        usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                        usuario.Direccion.Colonia.Municipio.IdMunicipio = objUsuario.IdMunicipio;
                        usuario.Direccion.Colonia.Municipio.Nombre = objUsuario.MunicipioNombre;

                        usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                        usuario.Direccion.Colonia.Municipio.Estado.IdEstado = objUsuario.IdEstado;
                        usuario.Direccion.Colonia.Municipio.Estado.Nombre = objUsuario.EstadoNombre;

                        usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais = objUsuario.IdPais;
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.Nombre = objUsuario.PaisNombre;

                        result.Object = usuario;
                    }


                }
                result.Correct = true;
                
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.EX = ex;
                result.Message = "ocurrio un error al realizar la consuta";
                throw;
            }

            return result;
        }

        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    var updateUsuario = context.Database.ExecuteSqlRaw($"UsuarioUpdate {usuario.IdUsuario}, '{usuario.Nombre}', '{usuario.ApellidoPaterno}', '{usuario.ApellidoMaterno}', '{usuario.FechaNacimiento}', '{usuario.Sexo}', '{usuario.Email}', '{usuario.UserName}', '{usuario.Password}', '{usuario.Telefono}', '{usuario.Celular}', '{usuario.CURP}', {usuario.Rol.IdRol}, '{usuario.Imagen}', '{usuario.Direccion.Calle}', '{usuario.Direccion.NumeroInterior}', '{usuario.Direccion.NumeroExterior}', {usuario.Direccion.Colonia.IdColonia}");

                    if (updateUsuario > 0)
                    {
                        result.Correct = true;
                        result.Message = "Se ha acualizado el usuario";
                    }

                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.EX = ex;
                result.Message = "ocurrio un error al realizar la consuta";
                throw;
            }

            return result;
        } 

        public static ML.Result Delete(int IdUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using(DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    var deleteUsuario = context.Database.ExecuteSqlRaw($"UsuarioDelete { IdUsuario}");

                    if(deleteUsuario > 0)
                    {
                        result.Correct = true;
                        result.Message = "Se ha eliminado el usuario";
                    }
                }
            }
            catch(Exception ex)
            {
                result.Correct = true;
                result.Message = "No se pudo eliminar el usuario";
                result.EX = ex;
                throw;
            }
            return result;
        }

        public static ML.Result ConvertirExcelToDataTable(string connectionString)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(OleDbConnection context = new OleDbConnection(connectionString))
                {
                    string query = "SELECT * FROM [Sheet1$]";

                    using (OleDbCommand command = new OleDbCommand())
                    {
                        command.CommandText = query;
                        command.Connection = context;


                        OleDbDataAdapter adapterData = new OleDbDataAdapter();
                        adapterData.SelectCommand = command;

                        DataTable tableUsuario = new DataTable();

                        adapterData.Fill(tableUsuario);

                        if(tableUsuario.Rows.Count > 0)
                        {
                            result.Objects = new List<object>();

                            foreach(DataRow rowUsuario in tableUsuario.Rows)
                            {
                                ML.Usuario usuario = new ML.Usuario();

                                usuario.Nombre = rowUsuario[0].ToString();
                                usuario.ApellidoPaterno = rowUsuario[1].ToString();
                                usuario.ApellidoMaterno = rowUsuario[2].ToString();
                                usuario.FechaNacimiento = rowUsuario[3].ToString();
                                usuario.Sexo = rowUsuario[4].ToString();
                                usuario.Email = rowUsuario[5].ToString();
                                usuario.UserName = rowUsuario[6].ToString();
                                usuario.Password = rowUsuario[7].ToString();
                                usuario.Telefono = rowUsuario[8].ToString();
                                usuario.Celular = rowUsuario[9].ToString();
                                usuario.CURP = rowUsuario[10].ToString();
                                usuario.Imagen = null;

                                usuario.Rol = new ML.Rol();
                                usuario.Rol.IdRol = byte.Parse(rowUsuario[11].ToString());

                                usuario.Direccion = new ML.Direccion();
                                usuario.Direccion.Calle = rowUsuario[12].ToString();
                                usuario.Direccion.NumeroInterior = rowUsuario[13].ToString();
                                usuario.Direccion.NumeroExterior = rowUsuario[14].ToString();
                                usuario.Direccion.Colonia = new ML.Colonia();
                                usuario.Direccion.Colonia.IdColonia = int.Parse(rowUsuario[15].ToString());


                                result.Objects.Add(usuario);

                            }
                            result.Correct = true;
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.EX = ex;
                result.Message = "Error: " + result.EX;
            }

            return result;
        }

        public static ML.Result ValidarExcel(List<object> Object)
        {
            ML.Result result = new ML.Result();

            try
            {
                result.Objects = new List<object>();

                int i = 1;
                foreach(ML.Usuario usuario in Object)
                {
                    ML.ErrorExcel error = new ML.ErrorExcel();
                    error.IdRegistro = i++;

                    usuario.Nombre = (usuario.Nombre == "") ? error.Mensaje += "Ingrese el nombre " : usuario.Nombre ;

                    usuario.ApellidoPaterno = (usuario.ApellidoPaterno == "") ? error.Mensaje += "Ingrese el Apellido Paterno " : usuario.ApellidoPaterno;

                    usuario.ApellidoMaterno = (usuario.ApellidoMaterno == "") ? error.Mensaje += "Ingrese el Apellido Materno " : usuario.ApellidoMaterno;

                    usuario.FechaNacimiento = (usuario.FechaNacimiento == "") ? error.Mensaje += "Ingrese la fecha de nacimiento " : usuario.FechaNacimiento;

                    usuario.Sexo = (usuario.Sexo == "") ? error.Mensaje += "Ingrese el sexo " : usuario.Sexo;

                    usuario.Email = (usuario.Email == "") ? error.Mensaje += "Ingrese el Email " : usuario.Email;

                    usuario.UserName = (usuario.UserName == "") ? error.Mensaje += "Ingres el UserName " : usuario.UserName;

                    usuario.Password = (usuario.Password == "") ? error.Mensaje += "Ingrese la contraseña" : usuario.Password;

                    usuario.Telefono = (usuario.Telefono == "") ? error.Mensaje += "Ingrese el Telefono " : usuario.Telefono;

                    usuario.Celular = (usuario.Celular == "") ? error.Mensaje += "Ingrese el Celular" : usuario.Celular;

                    usuario.CURP = (usuario.CURP == "") ? error.Mensaje += "Ingrese el CURP " : usuario.CURP;

                    if(usuario.Rol.IdRol.ToString() == "")
                    {
                        error.Mensaje += "Ingrese el Rol";
                    }

                    usuario.Direccion.Calle = (usuario.Direccion.Calle == "") ? error.Mensaje += "Ingrese la Calle " : usuario.Direccion.Calle;

                    usuario.Direccion.NumeroInterior = (usuario.Direccion.NumeroInterior == "") ? error.Mensaje += "Ingrese el numero Interior " : usuario.Direccion.NumeroInterior;

                    usuario.Direccion.NumeroExterior = (usuario.Direccion.NumeroExterior == "") ? error.Mensaje += "Ingrese el número exterior " : usuario.Direccion.NumeroExterior;

                    if(usuario.Direccion.Colonia.IdColonia.ToString() == "")
                    {
                        error.Mensaje  += "Ingrese la Colonia";
                    }


                    if(error.Mensaje != null)
                    {
                        result.Objects.Add(error);
                    }

                }
                result.Correct = true;
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.Message = ex.Message;
            }


            return result;
        }

        public static ML.Result UpdateEstatus(int IdUsuario, bool status)
        {
            ML.Result result = new ML.Result();

            try
            {
                using(DL.LsantosProgramacionNcapasContext context = new DL.LsantosProgramacionNcapasContext())
                {
                    var updateEstatus = context.Database.ExecuteSqlRaw($"UsuarioChangeEstatus {IdUsuario}, {status}");

                    if(updateEstatus > 0)
                    {
                        result.Message = "Se ha actualizado el estatus";
                        result.Correct = true;
                    }
                }
            }
            catch(Exception ex)
            {
                result.Message = "Ourrio un error al actualizar el estatus";
                result.EX = ex;
                result.Correct = false;
            }

            return result;
        }
    }
}