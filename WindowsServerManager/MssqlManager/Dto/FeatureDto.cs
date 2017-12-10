using System;

namespace MssqlManager.Dto
{
    public class FeatureDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public bool State { get; set; }
    }
}