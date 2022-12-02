// See https://aka.ms/new-console-template for more information
ReadFile();


static void ReadFile()
{
    string file = @"C:\Users\ALIEN21\Documents\Lorenzo\UsuarioDatosMasivosConErrores.txt";


    string fileError = @"C:\Users\ALIEN21\Documents\Lorenzo\ErrorAdd.txt";

    if (File.Exists(file))
    {
        ML.Result result = new ML.Result();

        result.Objects = new List<object>();

        ML.Result resultError = new ML.Result();

        resultError.Objects = new List<object>();

        StreamReader Textfile = new StreamReader(file);
        string line;
        line = Textfile.ReadLine();
        while ((line = Textfile.ReadLine()) != null)
        {
            string[] lines = line.Split('|');

            ML.Usuario usuario = new ML.Usuario();

            usuario.Nombre = lines[0];
            usuario.ApellidoPaterno = lines[1];
            usuario.ApellidoMaterno = lines[2];
            usuario.FechaNacimiento = lines[3];
            usuario.Sexo = lines[4];
            usuario.Email = lines[5];
            usuario.UserName = lines[6];
            usuario.Password = lines[7];
            usuario.Telefono = lines[8];
            usuario.Celular = lines[9];
            usuario.CURP = lines[10];

            usuario.Rol = new ML.Rol();
            usuario.Rol.IdRol = byte.Parse(lines[11]);

            usuario.Direccion = new ML.Direccion();

            usuario.Direccion.Calle = lines[12];
            usuario.Direccion.NumeroInterior = lines[13];
            usuario.Direccion.NumeroExterior = lines[14];
            usuario.Direccion.Colonia = new ML.Colonia();
            usuario.Direccion.Colonia.IdColonia = byte.Parse(lines[15]);

            result = BL.Usuario.Add(usuario);

            if (result.Correct)
            {
                Console.WriteLine("Se ha agregado");
            }
            else
            {
                resultError.Objects.Add(result.Message);   
            }
        }
        if(resultError.Objects.Count > 0)
        {
            TextWriter tw = new StreamWriter(fileError);
            foreach(string lineaError in resultError.Objects)
            {
                tw.WriteLine(lineaError);
            }
            tw.Close();

        }

    }
}
