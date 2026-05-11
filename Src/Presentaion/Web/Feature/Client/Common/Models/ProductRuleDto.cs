namespace Feature.Client.Common.Models
{
    public class ProductRuleDto
    {
        public int Id { get; set; }
        public string RuleCode { get; set; } = string.Empty;
        public string RuleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string QueryCondition { get; set; } = string.Empty;
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }


}
