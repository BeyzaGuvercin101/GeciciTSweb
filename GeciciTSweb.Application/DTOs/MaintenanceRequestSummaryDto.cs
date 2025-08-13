namespace GeciciTSweb.Application.DTOs
{
    public class MaintenanceRequestSummaryDto
    {
        public MaintenanceRequestDetailDto Request { get; set; } = null!;
        public List<StepDto> Steps { get; set; } = new();
        public List<DepartmentSummaryDto> Departments { get; set; } = new();
        public PermissionsDto Permissions { get; set; } = null!;
    }

    public class StepDto
    {
        public string Name { get; set; } = null!;
        public string State { get; set; } = null!; // Pending, InProgress, Completed
    }

    public class DepartmentSummaryDto
    {
        public string Code { get; set; } = null!; // Integrity, Maintenance, Production
        public string Status { get; set; } = null!;
        public int? CurrentRPN { get; set; }
        public int? ResidualRPN { get; set; }
        public DateTime? PlannedTemporaryRepairDate { get; set; }
        public DateTime? PlannedPermanentRepairDate { get; set; }
        public string? ReturnReasonText { get; set; }
        public string? CancelReasonText { get; set; }
        public UserDto? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }

    public class PermissionsDto
    {
        public bool CanEditIntegrity { get; set; }
        public bool CanEditMaintenance { get; set; }
        public bool CanEditProduction { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
    }
}
