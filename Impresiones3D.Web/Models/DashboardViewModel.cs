namespace Impresiones3D.Web.Models
{
    public class DashboardViewModel
    {
        public int TotalClientes { get; set; }
        public int TotalMateriales { get; set; }
        public int TotalImpresoras { get; set; }
        public int TotalOrdenes { get; set; }
        public int OrdenesPendientes { get; set; }
        public int OrdenesEnProduccion { get; set; }
        public int OrdenesCompletadas { get; set; }
        public int OrdenesEntregadas { get; set; }
        public int OrdenesCanceladas { get; set; }
        public List<OrdenViewModel>? OrdenesRecientes { get; set; }
    }
}