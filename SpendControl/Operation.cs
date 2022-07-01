using System;

namespace SpendControl
{
    public class Operation
    {
        public Operation()
        {
            Type = "spend";
            Value = 0;
            Category = "shoping";
            Description = "";
            OperationDate = DateTime.Now;
        }

        public Operation(float value, string discription, string category, string type, DateTime operationDate)
        {
            Type = type;
            Value = value;
            Category = category;
            Description = discription;
            OperationDate = operationDate;
        }
        
        public string Type { get; set; }
        
        public float Value { get; set; }
        
        public string Category { get; set; }
        
        public string Description { get; set; }
        
        public DateTime OperationDate { get; set; }
    }
}