using GMS.Application.Common.Interfaces;
using GMS.Application.Common.Mapping;
using GMS.Application.Common.Models;
using GMS.Application.Features.Grievances.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GMS.Application.Features.Grievances.Queries.GetGrievanceDetail;

public sealed record GetGrievanceDetailQuery(int GrievanceId, int RequestingUserId, bool CanViewAll)
    : IRequest<ApiResponse<GrievanceDto>>;

public sealed class GetGrievanceDetailQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetGrievanceDetailQuery, ApiResponse<GrievanceDto>>
{
    public async Task<ApiResponse<GrievanceDto>> Handle(GetGrievanceDetailQuery request, CancellationToken cancellationToken)
    {
        var grievance = await dbContext.Grievances.AsNoTracking().SingleOrDefaultAsync(x => x.Id == request.GrievanceId, cancellationToken);
        if (grievance is null)
        {
            return ApiResponse<GrievanceDto>.Fail("Grievance not found.", statusCode: 404);
        }

        if (!request.CanViewAll && grievance.CreatedByUserId != request.RequestingUserId)
        {
            return ApiResponse<GrievanceDto>.Fail("Forbidden.", statusCode: 403);
        }

        return ApiResponse<GrievanceDto>.Ok(grievance.ToDto());
    }
}
