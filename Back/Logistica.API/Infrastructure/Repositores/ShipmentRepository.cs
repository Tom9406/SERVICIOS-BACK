using Dapper;
using Encomiendas.API.Application.DTOs;
using Encomiendas.API.Infrastructure.Data;
using Logistica.API.Application.DTOs;
using System.Data;

namespace Encomiendas.API.Infrastructure.Repositories
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ShipmentRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<CreateShipmentResponse> CreateShipmentAsync(CreateShipmentRequest request)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@CompanyID", request.CompanyId);
            parameters.Add("@SenderCustomerID", request.SenderCustomerId);
            parameters.Add("@ReceiverCustomerID", request.ReceiverCustomerId);
            parameters.Add("@SenderAddressID", request.SenderAddressId);
            parameters.Add("@ReceiverAddressID", request.ReceiverAddressId);
            parameters.Add("@OriginBranchID", request.OriginBranchId);
            parameters.Add("@DestinationBranchID", request.DestinationBranchId);
            parameters.Add("@Weight", request.Weight);
            parameters.Add("@PackageDescription", request.Description);
            parameters.Add("@PaymentType", request.PaymentType);
            parameters.Add("@Observations", request.Observations);
            parameters.Add("@UserID", request.UserId);

            using var multi = await connection.QueryMultipleAsync(
                "CreateShipment",
                parameters,
                commandType: CommandType.StoredProcedure
            );//

            var result = await multi.ReadFirstOrDefaultAsync<CreateShipmentResponse>();

            return result ?? new CreateShipmentResponse();
        }
        public async Task ChangeShipmentStatusAsync(ChangeShipmentStatusRequest request)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@ShipmentID", request.ShipmentId);
            parameters.Add("@NewStatusID", request.NewStatusId);
            parameters.Add("@UserID", request.UserId);
            parameters.Add("@BranchID", request.BranchId);
            parameters.Add("@Notes", request.Notes);

            await connection.ExecuteAsync(
                "ChangeShipmentStatus",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<ShipmentTrackingResponseDto> GetTrackingAsync(string trackingNumber)
        {
            using var connection = _connectionFactory.CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                "dbo.GetShipmentTrackingFull",
                new { TrackingNumber = trackingNumber },
                commandType: CommandType.StoredProcedure
            );

            var header = await multi.ReadFirstOrDefaultAsync<ShipmentTrackingHeaderDto>();
            var history = (await multi.ReadAsync<ShipmentHistoryDto>()).ToList();

            return new ShipmentTrackingResponseDto
            {
                TrackingNumber = header.TrackingNumber,
                StatusCode = header.StatusCode,
                StatusName = header.StatusName,
                OriginBranch = header.OriginBranch,
                DestinationBranch = header.DestinationBranch,
                LastUpdate = header.LastUpdate,
                History = history
            };
        }



        public async Task<IEnumerable<ShipmentHistoryDto>> GetShipmentHistoryAsync(int shipmentId, int companyId)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<ShipmentHistoryDto>(
                "dbo.GetShipmentHistory",
                new { ShipmentID = shipmentId, CompanyID = companyId },
                commandType: CommandType.StoredProcedure
            );
        }


    }
}