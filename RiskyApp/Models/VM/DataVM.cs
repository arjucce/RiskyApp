namespace RiskyApp.Models.VM
{
    public class DataVM
    {                        
        public int DepartmentID { get; set; }    
        public string DepartmentName { get; set; }

        public int OperationTypeID { get; set; }
        public string OperationType { get; set; }

        public string OperationName { get; set; }
        public double Amount { get; set; }
        public string FormatUrl { get; set; }
        public int CategoryId { get; set; }
    }
}
