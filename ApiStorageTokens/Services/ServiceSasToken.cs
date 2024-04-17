using Azure.Data.Tables;
using Azure.Data.Tables.Sas;

namespace ApiStorageTokens.Services
{
    public class ServiceSasToken
    {
        //ESTA CLASE GENERARA TOKENS PARA NUESTRA 
        //TABLA ALUMNOS
        private TableClient tablaAlumnos;

        public ServiceSasToken(IConfiguration configuration)
        {
            string azureKeys =
                configuration.GetValue<string>
                ("AzureKeys:StorageAccount");
            TableServiceClient serviceClient =
                new TableServiceClient(azureKeys);
            this.tablaAlumnos = serviceClient.GetTableClient("alumnos");
        }

        public string GenerateToken(string curso)
        {
            //NECESITAMOS EL TIPO DE PERMISOS DE ACCESO
            //POR AHORA, SOLAMENTE VAMOS A DAR PERMISOS DE 
            //LECTURA
            TableSasPermissions permisos =
                TableSasPermissions.Read;
            //EL ACCESO A LOS ELEMENTOS CON EL TOKEN ESTA 
            //DELIMITADO MEDIANTE UN TIEMPO
            //NECESITAMOS UN CONSTRUCTOR DE PERMISOS CON UN 
            //TIEMPO DETERMINADO DE ACCESO
            TableSasBuilder builder =
                this.tablaAlumnos.GetSasBuilder(permisos,
                DateTime.UtcNow.AddMinutes(30));
            //QUEREMOS LIMITAR EL ACCESO POR CURSO
            builder.PartitionKeyStart = curso;
            builder.PartitionKeyEnd = curso;
            //CON TODO ESTO, YA PODEMOS GENERAR EL TOKEN DE ACCESO
            //QUE SERA UNA URI CON LOS PERMISOS Y EL TIEMPO
            Uri uriToken =
                this.tablaAlumnos.GenerateSasUri(builder);
            //EXTRAEMOS LA RUTA HTTPS DEL URI
            string token = uriToken.AbsoluteUri;
            return token;
        }
    }
}
