using FastReport;
using FastReport.Utils;

public class CrearReporteProgramaticamente
{
    public void CrearYGuardarReporte()
    {
        // Inicializar FastReport
        Config.WebMode = true;

        // Crear un nuevo reporte
        Report reporte = new Report();

        // Agregar una página al reporte
        ReportPage pagina = new ReportPage();
        reporte.Pages.Add(pagina);

        // Crear un título para la página
        PageHeaderBand encabezado = new PageHeaderBand();
        encabezado.Height = Units.Centimeters * 1;
        pagina.Bands.Add(encabezado);

        // Agregar un texto al encabezado
        TextObject titulo = new TextObject();
        titulo.Bounds = new System.Drawing.RectangleF(0, 0, Units.Centimeters * 19, Units.Centimeters * 1);
        titulo.Text = "Reporte de Productos";
        titulo.HorzAlign = HorzAlign.Center;
        titulo.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
        encabezado.Objects.Add(titulo);

        // Guardar el reporte en un archivo .frx
        reporte.Save("ReporteProductos.frx");
    }
}