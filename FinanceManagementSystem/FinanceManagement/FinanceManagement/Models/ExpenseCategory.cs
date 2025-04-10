namespace FinanceManagement.Models
{
    public class ExpenseCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public override string ToString()
        {
            return $"{CategoryId}: {CategoryName}"; 
        }
    }
}
